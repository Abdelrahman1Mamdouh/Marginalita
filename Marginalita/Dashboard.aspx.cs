using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Marginalita
{
    public partial class Dashboard : System.Web.UI.Page
    {

        bool[] vedi = new bool[3];


        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Chart1_DataBound(object sender, EventArgs e)
        {
            Chart1.Palette = ChartColorPalette.None;
            foreach (var point in Chart1.Series["Series1"].Points)
            {
                if (point.AxisLabel == "Costi")
                    point.Color = Color.Red;
                else if (point.AxisLabel == "Margine")
                    point.Color = Color.Green;

                point.LabelForeColor = Color.White;
                point.Font = new Font("Arial", 10, FontStyle.Bold);
                point.Label = point.YValues[0].ToString("0") + "%";
            }
        }

        protected void btnVisualizza_Click(object sender, EventArgs e)
        {

            Response.Redirect("dettagliProgetto.aspx");
        }


        public void Aggiungi_Progetti(object sender, EventArgs e)
        {

            vedi[0] = true;
            vedi[1] = false;
            vedi[2] = false;

            Session["vedi"] = vedi;

            Response.Redirect("InputDati.aspx");

        }
    }
}
