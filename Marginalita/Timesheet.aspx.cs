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
                    CreaGrigliaFake(); 
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
                comando.Parameters.AddWithValue("@dataAncoraggio", DateTime.Now);

                connessione.Open();
                comando.ExecuteNonQuery();
            }
        }

        //CODICE NON FUNZIONANTE 
        //private void CreaGrigliaFake()
        //{
        //    DataView dvDip = TabellaDipendente?.Select(DataSourceSelectArguments.Empty) as DataView;
        //    if (dvDip == null) return;
        //    DataTable dipendenti = dvDip.ToTable();

        //    DateTime dataInizio = DateTime.Today;
        //    DateTime dataFine = DateTimeTime.Today;

        //    if (visualeGiorno.Checked)
        //    {
        //        dataInizio = DateTime.Today;
        //        dataFine = DateTime.Today;
        //    }
        //    else if (visualeSettimana.Checked)
        //    {
        //        int diff = (7 + (DateTime.Today.DayOfWeek - DayOfWeek.Monday)) % 7;
        //        dataInizio = DateTime.Today.AddDays(-1 * diff).Date;
        //        dataFine = dataInizio.AddDays(6);
        //    }
        //    else if (visualeMese.Checked)
        //    {
        //        dataInizio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        //        dataFine = dataInizio.AddMonths(1).AddDays(-1);
        //    }

        //    DataView dvFake = DSFake?.Select(DataSourceSelectArguments.Empty) as DataView;
        //    DataTable fake = dvFake != null ? dvFakeToTable() : new DataTable();

        //    var valori = new Dixtionary<string, decimal>(StringComparer.Ordinal);
        //    foreach (DataRow r in fake.Rows)
        //    {
        //        if (r.IsNull("Dipendente") || r.IsNull("Creata") || r.IsNull("Costo")) continue;
        //        DataSetDateTime dt = Convert.ToDatatTime(r["Creata"]).Date;

        //        if (dt >= dataInizio && dt <= dataFine)
        //        {
        //            int dipId = Convert.ToInt32(r["Dipendnete"]);
        //            string key = $"{dipId}_{dt:yyyyMMdd}";
        //            if (valori.ContainsKey(key)) valori[key] += Convert.ToDecimal(r["Costo"]);
        //            else volori[key] = Convert.ToDecimal(r["Costo"]);
        //        }
        //    }

        //    DataTable matrice = new DatatTable();
        //    matrice.Columns.Add("Dipendente", typeof(string));

        //    for (int d = giornoInizio; d <= numeroGiorni; d++)
        //        matrice.Columns.Add(d.ToString(), typeof(decimal));

        //    foreach (DataRow dip in dipendenti.Rows)
        //    {
        //        DataRow nr = matrice.NewRow();
        //        int dipId = Convert.ToInt32(dip["ID"]);
        //        nr["Dipendente"] = $"{dip["Nome"]} {dip["Cognome"]}";

        //        for (int d = giornoInizio; d <= numeroGiorni; d++)
        //        {
        //            string key = $"{dipId}_{d}";
        //            nr[d.ToString()] = valori.TryGetValue(key, out decimal v) ? (object)v : DBNull.Value;
        //        }
        //        maatrice.Rows.Add(nr);
        //    }

        //    ViewFake.Columns.Clear();
        //    ViewFake.AutoGenerateColumns = false;
        //    ViewFake.Columns.Add(new BoundField { DataField = "Dipendente", HearderText = "Dipendente" });

        //    for (int d = giornoInizio; d <= numeroGiorni; d++)
        //    {
        //        ViewFake.Columns.Add(new BoundField { DataField = dataFine.ToString(), HearderText = dataFine.ToString(), DatatFormatString = "{0:0.##}" });
        //    }

        //    ViewFake.RowDataBound -= ViewFake_RowDataBound;
        //    ViewFake.RowDataBound += ViewFake_RowDataBound;
        //    ViewFake.DataSource = matrice;
        //    ViewFake.DataBind();
        //}

        private void CreaGrigliaFake()
        {
            DataView dvDip = TabellaDipendente?.Select(DataSourceSelectArguments.Empty) as DataView;
            if (dvDip == null) return;
            DataTable dipendenti = dvDip.ToTable();

            DataView dvFake = DSFake?.Select(DataSourceSelectArguments.Empty) as DataView;
            DataTable fake = dvFake != null ? dvFake.ToTable() : new DataTable();

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            int giornoInizio = 1;
            int numeroGiorni = DateTime.DaysInMonth(year, month);

            if (RadioButton3.Checked) 
            {
                giornoInizio = DateTime.Now.Day;
                numeroGiorni = giornoInizio;
            }
            else if (RadioButton4.Checked) 
            {
                int diff = (7 + (DateTime.Now.DayOfWeek - DayOfWeek.Monday)) % 7;
                DateTime lunedi = DateTime.Now.AddDays(-1 * diff);
                giornoInizio = (lunedi.Month == month) ? lunedi.Day : 1;
                numeroGiorni = Math.Min(giornoInizio + 6, DateTime.DaysInMonth(year, month));
            }

            var valori = new Dictionary<string, decimal>(StringComparer.Ordinal);
            foreach (DataRow r in fake.Rows)
            {
                if (r.IsNull("Dipendente") || r.IsNull("Creata") || r.IsNull("Ore")) continue;

                int dipId = Convert.ToInt32(r["Dipendente"]);
                DateTime dt = Convert.ToDateTime(r["Creata"]);

                if (dt.Year == year && dt.Month == month)
                {
                    string key = $"{dipId}_{dt.Day}";
                    if (valori.ContainsKey(key)) valori[key] += Convert.ToDecimal(r["Ore"]);
                    else valori[key] = Convert.ToDecimal(r["Ore"]);
                }
            }

            DataTable matrice = new DataTable();
            matrice.Columns.Add("Dipendente", typeof(string));
            for (int d = giornoInizio; d <= numeroGiorni; d++) matrice.Columns.Add(d.ToString(), typeof(decimal));

            foreach (DataRow dip in dipendenti.Rows)
            {
                DataRow nr = matrice.NewRow();
                int dipId = Convert.ToInt32(dip["ID"]);
                nr["Dipendente"] = $"{dip["Nome"]} {dip["Cognome"]}";
                for (int d = giornoInizio; d <= numeroGiorni; d++)
                {
                    string key = $"{dipId}_{d}";
                    nr[d.ToString()] = valori.TryGetValue(key, out decimal v) ? (object)v : DBNull.Value;
                }
                matrice.Rows.Add(nr);
            }

            ViewFake.Columns.Clear();
            ViewFake.Columns.Add(new BoundField { DataField = "Dipendente", HeaderText = "Dipendente" });
            for (int d = giornoInizio; d <= numeroGiorni; d++)
            {
                ViewFake.Columns.Add(new BoundField { DataField = d.ToString(), HeaderText = d.ToString(), DataFormatString = "{0:0.##}" });
            }

            ViewFake.DataSource = matrice;
            ViewFake.DataBind();
        }

        protected void ViewFake_RowDataBound(object sender, GridViewRowEventArgs e)
{
    if (e.Row.RowType == DataControlRowType.DataRow && ViewFake.HeaderRow != null)
    {
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;

        for (int i = 1; i < e.Row.Cells.Count; i++)
        {
            string headerText = ViewFake.HeaderRow.Cells[i].Text;
            if (int.TryParse(headerText, out int giornoReale))
            {
                try
                {
                    DateTime dt = new DateTime(year, month, giornoReale);
                    if (dt.DayOfWeek == DayOfWeek.Saturday) e.Row.Cells[i].BackColor = Color.LightBlue;
                    else if (dt.DayOfWeek == DayOfWeek.Sunday) e.Row.Cells[i].BackColor = Color.LightCoral;
                }
                catch { }
            }
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