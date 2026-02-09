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
        protected void Page_Load(object sender, EventArgs e)
        {
            CalcolaBudget();
        }

        //protected void btnVisualizza_Click(object sender, EventArgs e)
        //{
            
        //    Response.Redirect("DettagliProgetto.aspx");
        //}

        private void CalcolaBudget()
        {

            //E' UNA PROVA Questo metodo mi somma tutti i budget inserendo la somma nel primo riquadro della dashboard
            string connString = @"Data Source=(LocalDB)\SQL2025;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                // Query SQL per effettuare la somma di tutti i budget di tutti i progetti 
                string query = "SELECT SUM(Budget) FROM Progetto";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                int count = (int)cmd.ExecuteScalar(); // Restituisce il primo valore della prima riga
                //lblMRR.Text = count.ToString();
            }
        }
    }
}