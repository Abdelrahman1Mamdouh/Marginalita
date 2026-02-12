using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Marginalita
{
    public partial class Timesheet : Page
    {
        string stringaConnessione = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True";

        // Proprietà per determinare il tetto massimo di ore
        private int LimiteCorrente => visualeGiorno.Checked ? 8 : (visualeSettimana.Checked ? 40 : 160);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreaGrigliaFake();
            }
        }

        protected void InputOre_TextChanged(object sender, EventArgs e)
        {
            TextBox casellaTesto = (TextBox)sender;

            if (decimal.TryParse(casellaTesto.Text, out decimal oreInserite))
            {
                if (oreInserite >= 0 && oreInserite <= LimiteCorrente)
                {
                    RepeaterItem cella = (RepeaterItem)casellaTesto.NamingContainer;
                    RepeaterItem riga = (RepeaterItem)cella.Parent.Parent;

                    int idProgetto = int.Parse(((HiddenField)riga.FindControl("HiddenProgetto")).Value);
                    int idDipendente = int.Parse(((HiddenField)cella.FindControl("HiddenDipendente")).Value);

                    SalvaDatiConStoredProcedure(idProgetto, idDipendente, (int)oreInserite);
                }
                else
                {
                    casellaTesto.Text = "0";
                    //casellaTesto.Text = LimiteCorrente.ToString();
                }
            }
        }

        protected void RepDipendenti_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TextBox txtOre = (TextBox)e.Item.FindControl("InputOre");
                HiddenField idDipendenteHidden = (HiddenField)e.Item.FindControl("HiddenDipendente");

                RepeaterItem Progetto = (RepeaterItem)e.Item.Parent.Parent;
                HiddenField idProgettoHidden = (HiddenField)Progetto.FindControl("HiddenProgetto");

                // Imposta il limite massimo nell'input HTML
                txtOre.Attributes["max"] = LimiteCorrente.ToString();

                if (idDipendenteHidden != null && idProgettoHidden != null)
                {
                    txtOre.Text = RecuperaOreDalDatabase(idProgettoHidden.Value, idDipendenteHidden.Value);
                }
            }
        }

        private string RecuperaOreDalDatabase(string idProgetto, string idDipendente)
        {
            using (SqlConnection connessione = new SqlConnection(stringaConnessione))
            {
                string sql = "SELECT Ore FROM Original WHERE Progetto = @p AND Dipendente = @d";
                SqlCommand comando = new SqlCommand(sql, connessione);
                comando.Parameters.AddWithValue("@p", idProgetto);
                comando.Parameters.AddWithValue("@d", idDipendente);
                connessione.Open();
                object risultato = comando.ExecuteScalar();
                return risultato != null ? risultato.ToString() : "";
            }
        }

        private void SalvaDatiConStoredProcedure(int idProgetto, int idDipendente, int ore)
        {
            using (SqlConnection connessione = new SqlConnection(stringaConnessione))
            {
                SqlCommand comando = new SqlCommand("DivideAndConquer", connessione);
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@idProgettoOriginale", idProgetto);
                comando.Parameters.AddWithValue("@idDipendente", idDipendente);
                comando.Parameters.AddWithValue("@oreInserite", (decimal)ore);
                comando.Parameters.AddWithValue("@dataAncoraggio", GetTargetMonday());

                connessione.Open();
                comando.ExecuteNonQuery();
            }
        }
        private void CreaGrigliaFake()
        {
            DataView dvDip = TabellaDipendente?.Select(DataSourceSelectArguments.Empty) as DataView;
            if (dvDip == null) return;
            DataTable dipendenti = dvDip.ToTable();

            DataView dvFake = DSFake?.Select(DataSourceSelectArguments.Empty) as DataView;
            DataTable fake = dvFake != null ? dvFake.ToTable() : new DataTable();

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            // Aggrega ore per dipendente/giorno (chiave: "{dipId}_{day}")
            var valori = new Dictionary<string, decimal>(StringComparer.Ordinal);
            foreach (DataRow r in fake.Rows)
            {
                try
                {
                    if (r.IsNull("Dipendente") || r.IsNull("Creata") || r.IsNull("Ore"))
                        continue;

                    int dipId = Convert.ToInt32(r["Dipendente"]);
                    DateTime dt = Convert.ToDateTime(r["Creata"]);
                    if (dt.Year != year || dt.Month != month) continue;

                    decimal ore = Convert.ToDecimal(r["Ore"]);
                    string key = $"{dipId}_{dt.Day - 1}";

                    if (valori.ContainsKey(key)) valori[key] += ore;
                    else valori[key] = ore;
                }
                catch
                {
                    // Ignora righe malformate e continua
                    continue;
                }
            }

            // Costruisce la DataTable:colonna = Dipendente, poi colonne 1..daysInMonth
            var matrice = new DataTable();
            matrice.Columns.Add("Dipendente", typeof(string));
            for (int d = 1; d <= daysInMonth; d++)
                matrice.Columns.Add(d.ToString(), typeof(decimal));

            foreach (DataRow dip in dipendenti.Rows)
            {
                int dipId = Convert.ToInt32(dip["ID"]);
                string nomeCompleto = dip.Table.Columns.Contains("NomeCompleto")
                    ? dip["NomeCompleto"].ToString()
                    : $"{dip["Nome"]} {dip["Cognome"]}";

                DataRow nr = matrice.NewRow();
                nr["Dipendente"] = nomeCompleto;

                for (int d = 1; d <= daysInMonth; d++)
                {
                    string key = $"{dipId}_{d}";
                    nr[d.ToString()] = valori.TryGetValue(key, out decimal oreVal) ? (object)oreVal : DBNull.Value;
                }

                matrice.Rows.Add(nr);
            }

            // Costruisce colonne GridView dinamiche e associa
            ViewFake.Columns.Clear();
            ViewFake.AutoGenerateColumns = false;

            ViewFake.Columns.Add(new BoundField { DataField = "Dipendente", HeaderText = "Dipendente" });

            for (int d = 1; d <= daysInMonth; d++)
            {
                ViewFake.Columns.Add(new BoundField
                {
                    DataField = d.ToString(),
                    HeaderText = d.ToString(),
                    DataFormatString = "{0:0.##}",
                    HtmlEncode = false
                });
            }

            // Assicura che l'handler non venga registrato più volte
            ViewFake.RowDataBound -= ViewFake_RowDataBound;
            ViewFake.RowDataBound += ViewFake_RowDataBound;

            ViewFake.DataSource = matrice;
            ViewFake.DataBind();
        }

        // Evento che colora le celle: sabato, domenica, festività
        protected void ViewFake_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            // Ottieni festività del mese (implementazione di esempio)
            var festivita = GetHolidays(year, month);

            // La prima cella è 'Dipendente' -> le colonne giorno iniziano dall'indice 1
            for (int d = 1; d <= daysInMonth; d++)
            {
                int cellIndex = d; // d=1 => cell 1
                if (cellIndex >= e.Row.Cells.Count) break;

                DateTime date = new DateTime(year, month, d);
                TableCell cell = e.Row.Cells[cellIndex];

                if (festivita.Contains(date.Date))
                {
                    cell.BackColor = Color.LightGreen; // festività
                }
                else if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    cell.BackColor = Color.LightBlue; // sabato
                }
                else if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    cell.BackColor = Color.LightCoral; // domenica
                }
                else
                {
                    cell.BackColor = Color.Beige;
                }

            }
        }

        // Restituisce le festività per l'anno/mese: sostituire con query DB se serve
        private HashSet<DateTime> GetHolidays(int year, int month)
        {
            var hs = new HashSet<DateTime>();

            // ggiungere festività comuni; sostituire con la lista reale
            try
            {

                hs.Add(new DateTime(year, 1, 1));

                hs.Add(new DateTime(year, 6, 2));

                hs.Add(new DateTime(year, 12, 25));
                hs.Add(new DateTime(year, 12, 26));

            }
            catch
            {
                // ignora errori di costruzione date
            }

            // Filtra solo quelle del mese richiesto
            var result = new HashSet<DateTime>();
            foreach (var dt in hs)
            {
                if (dt.Year == year && dt.Month == month) result.Add(dt.Date);
            }

            return result;

            
        }

        private DateTime GetTargetMonday()
        {
            DateTime today = DateTime.Today;
            int dayOfWeek = (int)today.DayOfWeek;

            // CL: Your "Current Monday" logic (Sunday = 6 days back)
            int daysToSubtract = (dayOfWeek == 0) ? 6 : dayOfWeek - 1;
            DateTime CL = today.AddDays(-daysToSubtract);

            // TL: Your "Target Monday" logic (Workdays move back 7 days)
            DateTime TL = CL;
            if (dayOfWeek >= 1 && dayOfWeek <= 5)
            {
                TL = CL.AddDays(-7);
            }
            return TL.Date;
        }
    }
}