using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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
                // Imposta i parametri del SqlDataSource che invoca la stored procedure pivot
                if (DSMatrix != null)
                {
                    DSMatrix.SelectParameters["Mode"].DefaultValue = "Progetti";
                    DSMatrix.SelectParameters["AnchorDate"].DefaultValue = DateTime.Today.ToString("yyyy-MM-dd");
                }

                // bind della GridView: la proc DB restituisce già la matrice pivotata
                if (ViewFake != null) ViewFake.DataBind();

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

                // aggiorna le sorgenti dati e rilegga la matrice dal DB
                if (DSFake != null) DSFake.DataBind();
                if (ViewFake != null) ViewFake.DataBind();
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

        private void GrigliaAssenze()
        {
            if (GridAssenze != null && TabellaAssenze != null)
            {
                GridAssenze.DataBind();
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
            if (CostiEsterni != null && GridCostiEsterni != null)
            {
                CostiEsterni.DataBind();
                GridCostiEsterni.DataBind();
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

            int idDipendente = int.Parse(AssenzeDDL.SelectedValue);
            decimal ore = decimal.Parse(oreStr);
            int idProgettoFisso = 10;

            SalvaAssenzaSuDB(idProgettoFisso, idDipendente, ore, dataSelezionata);

            //OreAssenze.Text = "";
            //MotivoDDL.SelectedIndex = 0;
            //AssenzeDDL.SelectedIndex = 0;

            //aggiorna la matrice ricaricando direttamente dal DB(proc pivot)
            //if (ViewFake != null) ViewFake.DataBind();
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

        protected void btnApriCalendario_Click(object sender, EventArgs e)
        {
            pnlCalendario.Visible = !pnlCalendario.Visible;
        }
        protected void CDurata_SelectionChanged(object sender, EventArgs e)
        {
            txtDataVisualizzata.Text = CDurata.SelectedDate.ToShortDateString();
            pnlCalendario.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            CDurata.SelectedDates.Clear();
            foreach (DateTime d in SelectedDatesList)
            {
                CDurata.SelectedDates.Add(d);
            }
        }

        protected void ExportExcel(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Esempio.xls"));
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            ViewFake.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        protected void ChangeFake(object sender, EventArgs e)
        {
            DSMatrix.SelectParameters["Mode"].DefaultValue = Mode?.SelectedValue;

            DateTime anchor = (Calendar1 != null && Calendar1.SelectedDate != DateTime.MinValue)
                ? Calendar1.SelectedDate
                : DateTime.Today;

            DSMatrix.SelectParameters["AnchorDate"].DefaultValue = anchor.ToString("yyyy-MM-dd");
            ViewFake.DataBind();
        }
        protected void ApriCalendario(object sender, EventArgs e)
        {
            Panel1.Visible = !Panel1.Visible;
        }
    }
}