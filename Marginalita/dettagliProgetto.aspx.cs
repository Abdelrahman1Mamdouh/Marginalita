using System;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;


namespace Marginalita
{
    public partial class dettagliProgetto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Chart_DataBound(object sender, EventArgs e)
        {

            Chart Chart1 = (Chart)sender;
            Chart1.Palette = ChartColorPalette.None;
            foreach (var point in Chart1.Series["Series1"].Points)
            {
                if (point.AxisLabel == "Residuo")
                    point.Color = Color.Red;
                else if (point.AxisLabel == "Margine")
                    point.Color = Color.Green;

                point.LabelForeColor = Color.White;
                point.Font = new Font("Arial", 10, FontStyle.Bold);
                point.Label = point.YValues[0].ToString("0") + "%";
            }
        }
    }
}
