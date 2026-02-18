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
        private int LimiteCorrente => 40;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreaGrigliaFake();
                GrigliaCostiEsterni();
                GrigliaAssenze();
            }
        }

        protected void InputOre_TextChanged(object sender, EventArgs e)
        {
            TextBox casellaTesto = (TextBox)sender;
            string testoInput = casellaTesto.Text.Trim();

            decimal oreInserite = 0;
            bool isValido = decimal.TryParse(testoInput, out oreInserite);

            if (testoInput == "" || isValido)
            {
                RepeaterItem riga = (RepeaterItem)casellaTesto.NamingContainer;
                int idDipendente = int.Parse(((HiddenField)riga.FindControl("HiddenDipendente")).Value);

                int idProgettoFisso = 10;

                decimal oreGiaSalvate = GetWeeklyHoursExcludingCurrent(idDipendente, idProgettoFisso);

                SalvaDatiConStoredProcedure(idProgettoFisso, idDipendente, oreInserite);

                if (oreInserite + oreGiaSalvate > 40)
                {
                    decimal oreMassimePossibili = 40 - oreGiaSalvate;
                    oreInserite = oreMassimePossibili > 0 ? oreMassimePossibili : 0;
                    casellaTesto.Text = oreInserite.ToString("0.00");
                    casellaTesto.ForeColor = Color.Red;
                }
                else
                {
                    casellaTesto.ForeColor = Color.Black;
                }

                SalvaDatiConStoredProcedure(idProgettoFisso, idDipendente, oreInserite);

                if (DSFake != null) DSFake.DataBind();
                CreaGrigliaFake();
            }
        }
        protected void RepDipendenti_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TextBox txtOre = (TextBox)e.Item.FindControl("InputOre");
                HiddenField idDipendenteHidden = (HiddenField)e.Item.FindControl("HiddenDipendente");


                string idProgettoFisso = "10";

                if (idDipendenteHidden != null)
                {
                    txtOre.Text = RecuperaOreDalDatabase(idProgettoFisso, idDipendenteHidden.Value);
                }
            }
        }

        private string RecuperaOreDalDatabase(string idProgetto, string idDipendente)
        {
            using (SqlConnection connessione = new SqlConnection(stringaConnessione))
            {
                string sql = "SELECT Ore FROM Fake WHERE Progetto = @p AND Dipendente = @d";
                SqlCommand comando = new SqlCommand(sql, connessione);
                comando.Parameters.AddWithValue("@p", idProgetto);
                comando.Parameters.AddWithValue("@d", idDipendente);
                connessione.Open();
                object risultato = comando.ExecuteScalar();
                return risultato != null ? risultato.ToString() : "";
            }
        }

        private void SalvaDatiConStoredProcedure(int idProgetto, int idDipendente, decimal ore)
        {
            using (SqlConnection connessione = new SqlConnection(stringaConnessione))
            {
                SqlCommand comando = new SqlCommand("DivideAndConquer", connessione);
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@idProgettoOriginale", SqlDbType.Int).Value = idProgetto;
                comando.Parameters.Add("@idDipendente", SqlDbType.Int).Value = idDipendente;
                comando.Parameters.Add("@oreInserite", SqlDbType.Decimal).Value = ore;

                comando.Parameters.Add("@dataAncoraggio", SqlDbType.DateTime).Value = GetTargetMonday();

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

            var costMap = new Dictionary<int, decimal>();
            foreach (DataRow row in dipendenti.Rows)
            {
                int id = Convert.ToInt32(row["ID"]);
                decimal rate = row.IsNull("CostoOrario") ? 0 : Convert.ToDecimal(row["CostoOrario"]);
                costMap[id] = rate;
            }

            var valori = new Dictionary<string, decimal>(StringComparer.Ordinal);
            foreach (DataRow r in fake.Rows)
            {
                try
                {
                    if (r.IsNull("Dipendente") || r.IsNull("Creata") || r.IsNull("Costo"))
                        continue;

                    int dipId = Convert.ToInt32(r["Dipendente"]);
                    DateTime dt = Convert.ToDateTime(r["Creata"]);
                    decimal totalCosto = Convert.ToDecimal(r["Costo"]);

                    if (dt.Year == year && dt.Month == month)
                    {
                        decimal hourlyRate = costMap.ContainsKey(dipId) ? costMap[dipId] : 0;
                        decimal oreCalcolate = (hourlyRate > 0) ? (totalCosto / hourlyRate) : 0;

                        string key = $"{dipId}_{dt.Day}";
                        if (valori.ContainsKey(key)) valori[key] += oreCalcolate;
                        else valori[key] = oreCalcolate;
                    }
                }
                catch { continue; }
            }

            DataTable matrice = new DataTable();
            matrice.Columns.Add("Dipendente", typeof(string));
            for (int d = 1; d <= daysInMonth; d++)
            {
                matrice.Columns.Add(d.ToString(), typeof(decimal));
            }

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
                ViewFake.Columns.Add(new BoundField
                {
                    DataField = d.ToString(),
                    HeaderText = d.ToString(),
                    DataFormatString = "{0:0.##}",
                    HtmlEncode = false
                });
            }

            ViewFake.RowDataBound -= ViewFake_RowDataBound;
            ViewFake.RowDataBound += ViewFake_RowDataBound;

            ViewFake.DataSource = matrice;
            ViewFake.DataBind();
        }

        private void GrigliaAssenze()
        {
            if (ListaAssenze != null && TabellaAssenze != null)
            {
                ListaAssenze.DataBind();
            }
        }

        private void SalvaAssenzaSuDB(int idProgetto, int idDipendente, decimal ore, DateTime data)
        {
            using (SqlConnection connessione = new SqlConnection(stringaConnessione))
            {
                string sql = "INSERT INTO OreAssenze (Progetto, Dipendente, Ore, DataAssenze, Motivo) VALUES (@p, @d, @o, @date, @m)";

                SqlCommand comando = new SqlCommand(sql, connessione);

                comando.Parameters.AddWithValue("@p", idProgetto);
                comando.Parameters.AddWithValue("@d", idDipendente);
                comando.Parameters.AddWithValue("@o", ore);
                comando.Parameters.AddWithValue("@date", data);

                comando.Parameters.AddWithValue("@m", MotivoDDL.SelectedValue);
                connessione.Open();
                comando.ExecuteNonQuery();
            }
        }
        private void GrigliaCostiEsterni()
        {
            if (CostiEsterni != null && ListaCostiEsterni != null)
            {
                CostiEsterni.DataBind();
                ListaCostiEsterni.DataBind();
            }
        }
        protected void Assenze_Click(object sender, EventArgs e)
        {
            string idDipendenteStr = AssenzeDDL.SelectedValue;
            string motivo = MotivoDDL.SelectedValue;
            string oreStr = OreAssenze.Text;
            DateTime dataSelezionata = CDurata.SelectedDate;

            if (string.IsNullOrEmpty(idDipendenteStr) || idDipendenteStr == "0" ||
                string.IsNullOrEmpty(motivo) || string.IsNullOrEmpty(oreStr) ||
                dataSelezionata == DateTime.MinValue)
            {
                return;
            }

            int idDipendente = int.Parse(idDipendenteStr);
            decimal ore = decimal.Parse(oreStr);
            int idProgettoFisso = 10;

            SalvaAssenzaSuDB(idProgettoFisso, idDipendente, ore, dataSelezionata);

            OreAssenze.Text = "";
            MotivoDDL.SelectedIndex = 0;
            AssenzeDDL.SelectedIndex = 0;

            CreaGrigliaFake();
            GrigliaAssenze();
        }

        protected void Extra_Click(object sender, EventArgs e)
        {
            string fornitore = TIntestazione.Text.Trim();
            string descrizione = TDescrizione.Text.Trim();
            decimal importo = 0;

            if (decimal.TryParse(TImporto.Text, out importo) && !string.IsNullOrEmpty(fornitore))
            {
                CostiEsterni.InsertParameters["Fornitore"].DefaultValue = fornitore;
                CostiEsterni.InsertParameters["Descrizione"].DefaultValue = descrizione;
                CostiEsterni.InsertParameters["Costo"].DefaultValue = importo.ToString().Replace(",", ".");
                CostiEsterni.InsertParameters["Progetto"].DefaultValue = "10";

                CostiEsterni.Insert();
                GrigliaCostiEsterni();
            }
        }
        protected void ViewFake_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            var festivita = GetHolidays(year, month);

            for (int d = 1; d <= daysInMonth; d++)
            {
                int cellIndex = d; // d=1 => cell 1
                if (cellIndex >= e.Row.Cells.Count) break;

                DateTime date = new DateTime(year, month, d);
                TableCell cell = e.Row.Cells[cellIndex];

                if (festivita.Contains(date.Date))
                {
                    cell.BackColor = Color.LightGreen;
                }
                else if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    cell.BackColor = Color.LightBlue;
                }
                else if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    cell.BackColor = Color.LightCoral;
                }
                else
                {
                    cell.BackColor = Color.Beige;
                }

            }
        }

        private HashSet<DateTime> GetHolidays(int year, int month)
        {
            var hs = new HashSet<DateTime>();

            try
            {

                hs.Add(new DateTime(year, 1, 1));

                hs.Add(new DateTime(year, 6, 2));

                hs.Add(new DateTime(year, 12, 25));
                hs.Add(new DateTime(year, 12, 26));

            }
            catch
            {
            }

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


            int daysToSubtract = (dayOfWeek == 0) ? 6 : dayOfWeek - 1;
            DateTime CL = today.AddDays(-daysToSubtract);


            DateTime TL = CL;
            if (dayOfWeek >= 1 && dayOfWeek <= 5)
            {
                TL = CL.AddDays(-7);
            }
            return TL.Date;
        }

        private decimal GetWeeklyHoursExcludingCurrent(int idDipendente, int idProgettoEscluso)
        {
            using (SqlConnection conn = new SqlConnection(stringaConnessione))
            {

                string sql = @"SELECT SUM(Ore) FROM Fake
                       WHERE Dipendente = @d 
                       AND Progetto != @p 
                       AND CAST(Creata AS DATE) = CAST(@data AS DATE)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@d", idDipendente);
                cmd.Parameters.AddWithValue("@p", idProgettoEscluso);
                cmd.Parameters.AddWithValue("@data", GetTargetMonday());

                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }
        private List<DateTime> SelectedDatesList
        {
            get
            {
                if (ViewState["SelectedDatesList"] == null)
                {
                    return new List<DateTime>();
                }
                return (List<DateTime>)ViewState["SelectedDatesList"];
            }
            set
            {
                ViewState["SelectedDatesList"] = value;
            }
        }

        protected void CDurata_SelectionChanged(object sender, EventArgs e)
        {
            List<DateTime> currentList = SelectedDatesList;

            DateTime selectedDate = CDurata.SelectedDate.Date;

            if (currentList.Contains(selectedDate))
            {
                currentList.Remove(selectedDate);
            }
            else
            {
                currentList.Add(selectedDate);
            }
            SelectedDatesList = currentList;

            CDurata.SelectedDates.Clear();
            foreach (DateTime d in currentList)
            {
                CDurata.SelectedDates.Add(d);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            CDurata.SelectedDates.Clear();
            foreach (DateTime d in SelectedDatesList)
            {
                CDurata.SelectedDates.Add(d);
            }
        }
    }
}