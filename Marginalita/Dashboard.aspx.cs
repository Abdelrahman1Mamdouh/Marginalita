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
