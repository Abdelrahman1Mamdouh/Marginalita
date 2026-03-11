using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Marginalita
{
    public partial class Timesheet : Page
    {
        string stringaConnessione = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True";


        protected void Page_Load(object sender, EventArgs e)
        {


            DateTime anchor = GetSelectedAnchorDate();

            TabellaDipendente.SelectParameters["Monday"].DefaultValue =
                anchor.ToString("yyyy-MM-dd");

            if (!IsPostBack)
            {
                DSMatrix.SelectParameters["Mode"].DefaultValue = "Assegnazione";
                DSMatrix.SelectParameters["AnchorDate"].DefaultValue = anchor.ToString("yyyy-MM-dd");
                TextBox1.Text = anchor.ToString("MM/yyyy");

                RepSingolo.DataBind();
                ViewFake.DataBind();
                GrigliaCostiEsterni();
                GrigliaAssenze();
            }

            ModSet.Checked = true;

        }

        protected void AssenzeOre_TextChanged(object sender, EventArgs e)
        {
            int maxOre;
            string OrAs = OreAssenze.Text;  

            if (OrAs != "")
            {
                maxOre = 8;
                int.TryParse(OrAs, out int oreAssenze);
                if (oreAssenze > maxOre)
                {
                    OreAssenze.ForeColor = System.Drawing.Color.Red;


                    OreAssenze.Text = maxOre.ToString();
                    ;

                }
                else
                {
                    OreAssenze.ForeColor = System.Drawing.Color.Black;

                }

            }

        }


        protected void InputOre_TextChanged(object sender, EventArgs e)
        {
            int maxOre;
          TextBox tbModificata = (TextBox)sender;


            RepeaterItem riga = (RepeaterItem)tbModificata.NamingContainer;

            TextBox txtInterne = (TextBox)riga.FindControl("OreInterne");
            TextBox txtEsterne = (TextBox)riga.FindControl("OreEsterne");
            HiddenField HID = (HiddenField)riga.FindControl("HiddenDipendente");





            decimal oreI = TryParseDecimal(txtInterne.Text);
            decimal oreE = TryParseDecimal(txtEsterne.Text);
            int ID = Int32.Parse(HID.Value);

            TabellaAssenze.SelectCommand = $"SELECT ID, Ore, DataAssenze, Dipendente, Motivo FROM OreAssenze WHERE Dipendente = {ID.ToString()}";




            var dv = (DataView)TabellaAssenze.Select(DataSourceSelectArguments.Empty);
            decimal oreAssenze = 0;
            if (dv != null)
            {
                foreach (DataRowView row in dv)
                {
                    oreAssenze += Convert.ToDecimal(row["Ore"]);
                }



            }
            TabellaAssenze.SelectCommand = "SELECT ID, Ore, DataAssenze, Dipendente, Motivo FROM V_OreAssenze";

            GridAssenze.DataBind();
            
            bool mod = Modalita.Checked;
            

            if (mod)
            {
                maxOre = 160;
            }
            else
            {
                maxOre = 40;
            }

            decimal totale = oreI + oreE + oreAssenze;

            if (totale > maxOre)
            {
                txtInterne.ForeColor = System.Drawing.Color.Red;
                txtEsterne.ForeColor = System.Drawing.Color.Red;

                txtInterne.Text = (maxOre - (oreE + oreAssenze)).ToString();
                //txtEsterne.Text = (maxOre - (oreI + oreAssenze)).ToString();

            }
            else
            {
                txtInterne.ForeColor = System.Drawing.Color.Black;
                txtEsterne.ForeColor = System.Drawing.Color.Black;
            }
        }
        

        private decimal TryParseDecimal(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return 0;

            decimal.TryParse(input.Replace(",", "."),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal result);

            return result;
        }
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


            TabellaAssenze.InsertParameters["Dipendente"].DefaultValue = idDipendente.ToString();
            TabellaAssenze.InsertParameters["Motivo"].DefaultValue = motivo;
            TabellaAssenze.InsertParameters["Ore"].DefaultValue = ore.ToString(System.Globalization.CultureInfo.InvariantCulture);
            TabellaAssenze.InsertParameters["DataAssenze"].DefaultValue = dataSelezionata.ToString("yyyy-MM-dd");

            TabellaAssenze.Insert();
            GrigliaAssenze();

         
                AssenzeDDL.SelectedIndex = 0;
               MotivoDDL.SelectedIndex = 0;
                OreAssenze.Text = string.Empty;
                CDurata.SelectedDate = DateTime.MinValue;
                txtDataVisualizzata.Text = string.Empty;
                 pnlCalendario.Visible = false;
            

        }

        protected void Extra_Click(object sender, EventArgs e)
        {
            string IdDip = DDLDipendenteVincoli.SelectedValue;
            string IdProg = DDLProgettiVincoli.SelectedValue;


            CostiEsterni.InsertParameters["DipendenteID"].DefaultValue = IdDip.ToString();
            CostiEsterni.InsertParameters["ProgettoID"].DefaultValue = IdProg.ToString();
    

            CostiEsterni.Insert();
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
            string fileName = $"Report_{DSMatrix.SelectParameters["Mode"].DefaultValue}_" + DateTime.Now.ToString("MMMM_yyyy") + ".xls";
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
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
            DateTime anchor = GetSelectedAnchorDate();

            DSMatrix.SelectParameters["Mode"].DefaultValue = Mode?.SelectedValue;
            DSMatrix.SelectParameters["AnchorDate"].DefaultValue = anchor.ToString("yyyy-MM-dd");

            TabellaDipendente.SelectParameters["Monday"].DefaultValue = anchor.ToString("yyyy-MM-dd");

            TextBox1.Text = anchor.ToString("MM/yyyy");
            Panel1.Visible = false;

            RepSingolo.DataBind();
            ViewFake.DataBind();
        }
        protected void ApriCalendario(object sender, EventArgs e)
        {
            Panel1.Visible = !Panel1.Visible;
        }

        protected void btnSalvaTutto(object sender, EventArgs e)
        {
            HideMessage();

            try
            {
                bool mod = Modalita.Checked;

                var tvp = BuildTimesheetDataTableFromRepeater(mod);
                if (tvp.Rows.Count == 0)
                {
                    ShowMessage("Inserisci almeno un valore ore prima di inviare.");
                    return;
                }

                string SP = mod ? "dbo.Mensile" : "dbo.Settimanale";

                using (var conn = new SqlConnection(stringaConnessione))
                using (var cmd = new SqlCommand(SP, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var p = cmd.Parameters.AddWithValue("@rows", tvp);
                    p.SqlDbType = SqlDbType.Structured;
                    p.TypeName = "dbo.TimesheetRow";

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                DateTime anchor = GetSelectedAnchorDate();

                TabellaDipendente.SelectParameters["Monday"].DefaultValue = anchor.ToString("yyyy-MM-dd");
                DSMatrix.SelectParameters["AnchorDate"].DefaultValue = anchor.ToString("yyyy-MM-dd");

                TabellaDipendente.DataBind();
                RepSingolo.DataBind();
                DSMatrix.DataBind();
                ViewFake.DataBind();
                GrigliaAssenze();
                GrigliaCostiEsterni();

                ShowMessage("Salvataggio completato con successo.", true);
            }
            catch (SqlException ex)
            {
                ShowMessage(ex.Message);
            }
            catch (Exception ex)
            {
                ShowMessage("Errore imprevisto: " + ex.Message);
            }
        }

        private DataTable BuildTimesheetDataTableFromRepeater(bool mod)
        {
            DataTable tvp = new DataTable();

            tvp.Columns.Add("IdDip", typeof(int));
            tvp.Columns.Add("dataAncoraggio", typeof(DateTime));
            tvp.Columns.Add("OreI", typeof(decimal));
            tvp.Columns.Add("OreE", typeof(decimal));

            DateTime anchor;

            if (mod)
            {
                anchor = (Calendar1 != null && Calendar1.SelectedDate != DateTime.MinValue)
            ? Calendar1.SelectedDate
            : DateTime.Today;
            }
            else
            {
                anchor = (Calendar1 != null && Calendar1.SelectedDate != DateTime.MinValue)
            ? Calendar1.SelectedDate
            : DateTime.Today; //change later to GetTargetMonday()
            }

            

            foreach (RepeaterItem item in RepSingolo.Items)
            {
                if (item.ItemType != ListItemType.Item && item.ItemType != ListItemType.AlternatingItem)
                    continue;

                HiddenField hfIdDipendente = (HiddenField)item.FindControl("HiddenDipendente");
                TextBox txtOreInterne = (TextBox)item.FindControl("OreInterne");
                TextBox txtOreEsterne = (TextBox)item.FindControl("OreEsterne");

                if (hfIdDipendente == null || txtOreInterne == null || txtOreEsterne == null)
                    continue;

                if (!int.TryParse(hfIdDipendente.Value, out int idDip))
                    continue;

                decimal oreI = 0;
                if (!string.IsNullOrWhiteSpace(txtOreInterne.Text))
                    decimal.TryParse(txtOreInterne.Text.Replace(",", "."), System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out oreI);

                decimal oreE = 0;
                if (!string.IsNullOrWhiteSpace(txtOreEsterne.Text))     
                    decimal.TryParse(txtOreEsterne.Text.Replace(",", "."), System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out oreE);


                if (oreI <= 0 && oreE <= 0) continue;

                DataRow row = tvp.NewRow();
                row["IdDip"] = idDip;
                row["dataAncoraggio"] = anchor;
                row["OreI"] = oreI;
                row["OreE"] = oreE;
                tvp.Rows.Add(row);
            }

            return tvp;
        }
        protected void Elimina_Assenza(object sender, EventArgs e)
        {
            //TabellaAssenze.Delete();
            //Response.Redirect("Timesheet.aspx");
        }

        protected void Elimina_CostiEsterni(object sender, EventArgs e)
        {
            //CostiEsterni.Delete();
            //Response.Redirect("Timesheet.aspx");
        }

        private DateTime GetSelectedAnchorDate()
        {
            if (Calendar1 != null && Calendar1.SelectedDate != DateTime.MinValue)
                return Calendar1.SelectedDate.Date;

            return DateTime.Today.Date;
        }


        private void ShowMessage(string message, bool success = false)
        {
            pnlEsito.Visible = true;
            pnlEsito.CssClass = success
                ? "alert alert-success mb-0 py-2 px-3"
                : "alert alert-warning mb-0 py-2 px-3";

            lblEsito.Text = message;
        }

        private void HideMessage()
        {
            pnlEsito.Visible = false;
            lblEsito.Text = "";
        }
    }
}
