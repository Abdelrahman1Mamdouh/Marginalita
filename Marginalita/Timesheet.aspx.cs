using System;
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
            //gestioneCostiEsterni.Visible = false; 

            if (!IsPostBack)
            {
                TabellaDipendente.SelectParameters["Monday"].DefaultValue =
                    GetTargetMonday().ToString("yyyy-MM-dd");

                DSMatrix.SelectParameters["Mode"].DefaultValue = "OreInterne";
                DSMatrix.SelectParameters["AnchorDate"].DefaultValue = DateTime.Today.ToString("yyyy-MM-dd");

                ViewFake.DataBind();
                //GrigliaCostiEsterni();
                GrigliaAssenze();
            }
        }
        protected void InputOre_TextChanged(object sender, EventArgs e)
        {
            TextBox tbModificata = (TextBox)sender;

            RepeaterItem riga = (RepeaterItem)tbModificata.NamingContainer;

            TextBox txtInterne = (TextBox)riga.FindControl("OreInterne");
            TextBox txtEsterne = (TextBox)riga.FindControl("OreEsterne");

            decimal oreI = TryParseDecimal(txtInterne.Text);
            decimal oreE = TryParseDecimal(txtEsterne.Text);

            decimal totale = oreI + oreE;

            if (totale > 40)
            {
                txtInterne.ForeColor = System.Drawing.Color.Red;
                txtEsterne.ForeColor = System.Drawing.Color.Red;

            }
            else
            {
                txtInterne.ForeColor = System.Drawing.Color.Black;
                txtEsterne.ForeColor = System.Drawing.Color.Black;
            }

            txtInterne.Text = oreI.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
            txtEsterne.Text = oreE.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
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
        //private void GrigliaCostiEsterni()
        //{
        //    if (CostiEsterni != null && GridCostiEsterni != null)
        //    {
        //        CostiEsterni.DataBind();
        //        GridCostiEsterni.DataBind();
        //    }
        //}

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
        }

        protected void Extra_Click(object sender, EventArgs e)
        {
            //string fornitore = TIntestazione.Text.Trim();
            //string descrizione = TDescrizione.Text.Trim();
            //if (!decimal.TryParse(TImporto.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal importo))
            //    return;

            //CostiEsterni.InsertParameters["Costo"].DefaultValue = importo.ToString(System.Globalization.CultureInfo.InvariantCulture);
            //CostiEsterni.InsertParameters["Fornitore"].DefaultValue = fornitore;
            //CostiEsterni.InsertParameters["Descrizione"].DefaultValue = descrizione;
           
            //CostiEsterni.Insert();
            //GrigliaCostiEsterni();
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
            //DSMatrix.SelectParameters["Mode"].DefaultValue = Mode?.SelectedValue;

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
            TabellaDipendente.DataBind();
            RepSingolo.DataBind();

            DSMatrix.DataBind();
            ViewFake.DataBind();

            GrigliaAssenze();
            //GrigliaCostiEsterni();
        }

        private DataTable BuildTimesheetDataTableFromRepeater()
        {
            DataTable tvp = new DataTable();

            tvp.Columns.Add("IdDip", typeof(int));
            tvp.Columns.Add("dataAncoraggio", typeof(DateTime));
            tvp.Columns.Add("OreInterne", typeof(decimal));
            tvp.Columns.Add("OreEsterne", typeof(decimal));

            DateTime anchor = GetTargetMonday();

            foreach (RepeaterItem item in RepSingolo.Items)
            {
                if (item.ItemType != ListItemType.Item && item.ItemType != ListItemType.AlternatingItem)
                    continue;

                HiddenField hfIdDipendente = (HiddenField)item.FindControl("HiddenDipendente");
                TextBox txtOreInterne = (TextBox)item.FindControl("InputOreInterne");
                TextBox txtOreEsterne = (TextBox)item.FindControl("InputOreEsterne");

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
                row["OreInterne"] = oreI;
                row["OreEsterne"] = oreE;
                tvp.Rows.Add(row);
            }

            return tvp;
        }
        protected void Elimina_Assenza(object sender, EventArgs e)
        {
            TabellaAssenze.Delete();
            Response.Redirect("Timesheet.aspx");
        }

        protected void Elimina_CostiEsterni(object sender, EventArgs e)
        {
            //CostiEsterni.Delete();
            //Response.Redirect("Timesheet.aspx");
        }
    }
}  
