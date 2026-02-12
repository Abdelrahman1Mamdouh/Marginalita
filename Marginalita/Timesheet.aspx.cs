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

        // Metodi per gestire il cambio dei RadioButton (Risolve l'errore di compilazione)
        protected void visualizzaGiorno(object sender, EventArgs e) { CreaGrigliaFake(); }
        protected void visualizzaSettimana(object sender, EventArgs e) { CreaGrigliaFake(); }
        protected void visualizzaMese(object sender, EventArgs e) { CreaGrigliaFake(); }

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
                    CreaGrigliaFake(); // Aggiorna il riepilogo
                }
                else
                {
                    casellaTesto.Text = LimiteCorrente.ToString();
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
                string sql = "SELECT Costo FROM Original WHERE Progetto = @p AND Dipendente = @d";
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
                SqlCommand comando = new SqlCommand("SalvaOre", connessione);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@idProgetto", idProgetto);
                comando.Parameters.AddWithValue("@idDipendente", idDipendente);
                comando.Parameters.AddWithValue("@costo", ore);
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

            var valori = new Dictionary<string, decimal>(StringComparer.Ordinal);
            foreach (DataRow r in fake.Rows)
            {
                if (r.IsNull("Dipendente") || r.IsNull("Creata") || r.IsNull("Costo")) continue;
                int dipId = Convert.ToInt32(r["Dipendente"]);
                DateTime dt = Convert.ToDateTime(r["Creata"]);
                if (dt.Year == year && dt.Month == month)
                {
                    string key = $"{dipId}_{dt.Day}";
                    if (valori.ContainsKey(key)) valori[key] += Convert.ToDecimal(r["Costo"]);
                    else valori[key] = Convert.ToDecimal(r["Costo"]);
                }
            }

            DataTable matrice = new DataTable();
            matrice.Columns.Add("Dipendente", typeof(string));
            for (int d = 1; d <= daysInMonth; d++) matrice.Columns.Add(d.ToString(), typeof(decimal));

            foreach (DataRow dip in dipendenti.Rows)
            {
                DataRow nr = matrice.NewRow();
                int dipId = Convert.ToInt32(dip["ID"]);
                nr["Dipendente"] = $"{dip["Nome"]} {dip["Cognome"]}";
                for (int d = 1; d <= daysInMonth; d++)
                {
                    string key = $"{dipId}_{d}";
                    nr[d.ToString()] = valori.TryGetValue(key, out decimal v) ? (object)v : DBNull.Value;
                }
                matrice.Rows.Add(nr);
            }

            ViewFake.Columns.Clear();
            ViewFake.AutoGenerateColumns = false;
            ViewFake.Columns.Add(new BoundField { DataField = "Dipendente", HeaderText = "Dipendente" });
            for (int d = 1; d <= daysInMonth; d++)
            {
                ViewFake.Columns.Add(new BoundField { DataField = d.ToString(), HeaderText = d.ToString(), DataFormatString = "{0:0.##}" });
            }

            ViewFake.RowDataBound -= ViewFake_RowDataBound;
            ViewFake.RowDataBound += ViewFake_RowDataBound;
            ViewFake.DataSource = matrice;
            ViewFake.DataBind();
        }

        protected void ViewFake_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                for (int d = 1; d < e.Row.Cells.Count; d++)
                {
                    DateTime dt = new DateTime(year, month, d);
                    if (dt.DayOfWeek == DayOfWeek.Saturday) e.Row.Cells[d].BackColor = Color.LightBlue;
                    else if (dt.DayOfWeek == DayOfWeek.Sunday) e.Row.Cells[d].BackColor = Color.LightCoral;
                }
            }
        }

        private HashSet<DateTime> GetHolidays(int year, int month)
        {
            var hs = new HashSet<DateTime>();
            try
            {
                hs.Add(new DateTime(year, 1, 1));
                hs.Add(new DateTime(year, 12, 25));
                hs.Add(new DateTime(year, 12, 26));
            }
            catch { }
            return hs;
        }
    }
}