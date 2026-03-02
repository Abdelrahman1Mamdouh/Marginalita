using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Marginalita
{
    public partial class Dashboard : System.Web.UI.Page
    {

        bool[] vedi = new bool[3];


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Assicurati che il grafico sia bindato
                Chart1.DataBind();

                foreach (var point in Chart1.Series["Series1"].Points)
                {
                    if (point.AxisLabel == "Costi")
                        point.Color = System.Drawing.Color.Red; // o System.Drawing.Color.FromArgb(220,53,69) per rosso personalizzato
                    else if (point.AxisLabel == "Margine")
                        point.Color = System.Drawing.Color.Green; // o System.Drawing.Color.FromArgb(40,167,69) per verde personalizzato

                    point.LabelForeColor = System.Drawing.Color.White;   // testo bianco
                    point.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold); // grassetto
                    point.Label = point.YValues[0].ToString("0") + "%"; // mostra il valore
                }
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
        //protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    // Verifichiamo che il comando sia quello del nostro pulsante
        //    if (e.CommandName == "btnVisualizza")
        //    {
        //        // Recuperiamo l'ID passato tramite CommandArgument
        //        string idSelezionato = e.CommandArgument.ToString();

        //        // Reindirizziamo alla pagina Dettagli.aspx passando l'ID
        //        Response.Redirect("dettagliProgetto.aspx?id=" + idSelezionato);
        //    }
        //}

        //private void CalcolaBudget()
        //{

        //    //E' UNA PROVA Questo metodo mi somma tutti i budget inserendo la somma nel primo riquadro della dashboard
        //    string connString = @"Data Source=(LocalDB)\SQL2025;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True";
        //    using (SqlConnection conn = new SqlConnection(connString))
        //    {
        //        // Query SQL per effettuare la somma di tutti i budget di tutti i progetti 
        //        string query = "SELECT SUM(Budget) FROM Progetto";
        //        SqlCommand cmd = new SqlCommand(query, conn);

        //        conn.Open();
        //        int count = (int)cmd.ExecuteScalar(); // Restituisce il primo valore della prima riga
        //        //lblMRR.Text = count.ToString();
        //    }
        //}
    }
}
