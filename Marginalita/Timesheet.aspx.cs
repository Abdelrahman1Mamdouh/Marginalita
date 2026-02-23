using Newtonsoft.Json;
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
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Imposta i parametri del SqlDataSource che invoca la stored procedure pivot
                if (DSMatrix != null)
                {
                    DSMatrix.SelectParameters["Mode"].DefaultValue = "Dipendenti";
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

                //SalvaDatiConStoredProcedure(idProgettoFisso, idDipendente, oreInserite);

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

                //SalvaDatiConStoredProcedure(idProgettoFisso, idDipendente, oreInserite);

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

        //private void SalvaDatiConStoredProcedure(int idProgetto, int idDipendente, decimal ore)
        //{
        //    using (SqlConnection connessione = new SqlConnection(stringaConnessione))
        //    {
        //        SqlCommand comando = new SqlCommand("DivideAndConquer", connessione);
        //        comando.CommandType = CommandType.StoredProcedure;

        //        comando.Parameters.Add("@idProgettoOriginale", SqlDbType.Int).Value = idProgetto;
        //        comando.Parameters.Add("@idDipendente", SqlDbType.Int).Value = idDipendente;
        //        comando.Parameters.Add("@oreInserite", SqlDbType.Decimal).Value = ore;

        //        comando.Parameters.Add("@dataAncoraggio", SqlDbType.DateTime).Value = GetTargetMonday();

        //        connessione.Open();
        //        comando.ExecuteNonQuery();
        //    }
        //}

        private void GrigliaAssenze()
        {
            if (GridAssenze != null && TabellaAssenze != null)
            {
                GridAssenze.DataBind();
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


            // Imposta i parametri del SqlDataSource (nomi devono corrispondere all'InsertCommand)
            TabellaAssenze.InsertParameters["Dipendente"].DefaultValue = idDipendente.ToString();
            TabellaAssenze.InsertParameters["Motivo"].DefaultValue = motivo;
            TabellaAssenze.InsertParameters["Ore"].DefaultValue = ore.ToString(System.Globalization.CultureInfo.InvariantCulture);
            TabellaAssenze.InsertParameters["DataAssenze"].DefaultValue = dataSelezionata.ToString("yyyy-MM-dd");
            TabellaAssenze.InsertParameters["Progetto"].DefaultValue = "10";

            // Esegue l'INSERT tramite SqlDataSource (senza try/catch come richiesto)
            TabellaAssenze.Insert();

            // Aggiorna la griglia
            GrigliaAssenze();
        }

        protected void Extra_Click(object sender, EventArgs e)
        {
            // Leggi i valori dai controlli della pagina
            string fornitore = TIntestazione.Text.Trim();
            string descrizione = TDescrizione.Text.Trim();
            if (!decimal.TryParse(TImporto.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal importo))
                return;

            // Imposta i parametri (nomi devono corrispondere a InsertParameters in .aspx)
            CostiEsterni.InsertParameters["Costo"].DefaultValue = importo.ToString(System.Globalization.CultureInfo.InvariantCulture);
            CostiEsterni.InsertParameters["Fornitore"].DefaultValue = fornitore;
            CostiEsterni.InsertParameters["Descrizione"].DefaultValue = descrizione;
            CostiEsterni.InsertParameters["Progetto"].DefaultValue = "10";

            // Esegue l'INSERT
            CostiEsterni.Insert();

            // Aggiorna visualizzazione
            GrigliaCostiEsterni();
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

                string sql = @"SELECT SUM(Costo) FROM Fake
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


        protected void btnApriCalendario_Click(object sender, EventArgs e)
        {
            pnlCalendario.Visible = !pnlCalendario.Visible;
        }
        protected void CDurata_SelectionChanged(object sender, EventArgs e)
        {
            txtDataVisualizzata.Text = CDurata.SelectedDate.ToShortDateString();
            pnlCalendario.Visible = false;
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

        protected void btnSalvaTutto(object sender, EventArgs e)
        {
            var tvp = BuildTimesheetDataTableFromRepeater();
            if (tvp.Rows.Count == 0) return;

            using (var conn = new SqlConnection(stringaConnessione))
            using (var cmd = new SqlCommand("dbo.DivideAndConquer", conn)) 
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var p = cmd.Parameters.AddWithValue("@rows", tvp);
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName = "dbo.TimesheetRow";
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private DataTable BuildTimesheetDataTableFromRepeater()
        {
            DataTable tvp = new DataTable();
            tvp.Columns.Add("IdDipendente", typeof(int));
            tvp.Columns.Add("Ore", typeof(decimal));
            tvp.Columns.Add("Data", typeof(DateTime));

            foreach (RepeaterItem item in RepSingolo.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfIdDipendente = (HiddenField)item.FindControl("HiddenDipendente");
                    TextBox txtOre = (TextBox)item.FindControl("InputOre");
                    Label lblData = (Label)item.FindControl("DataLabel"); // Assicurati di avere un'etichetta per la data

                    if (hfIdDipendente != null && txtOre != null && lblData != null)
                    {
                        int idDipendente = int.Parse(hfIdDipendente.Value);
                        decimal ore = string.IsNullOrEmpty(txtOre.Text) ? 0 : decimal.Parse(txtOre.Text);
                        DateTime data = DateTime.Parse(lblData.Text); // Ottieni la data dall'etichetta

                        DataRow row = tvp.NewRow();
                        row["IdDipendente"] = idDipendente;
                        row["Ore"] = ore;
                        row["Data"] = GetTargetMonday(); // Aggiungi la data alla riga

                        tvp.Rows.Add(row);
                    }
                }
            }

            return tvp;
        }
    }
}