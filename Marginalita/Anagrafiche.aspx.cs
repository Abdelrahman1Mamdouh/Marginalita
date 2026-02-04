using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Marginalita
{
    public partial class Anagrafiche : System.Web.UI.Page
    {

        bool[] vedi = new bool[3];
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void NewProgetto(object sender, EventArgs e) 
        {
            vedi[0] = true;
            vedi[1] = false; 
            vedi[2] = false;

            Session["vedi"] = vedi;
            Response.Redirect("InputDati.aspx");
        }
        protected void NewSocieta(object sender, EventArgs e) 
        {
            vedi[0] = false;
            vedi[1] = true;
            vedi[2] = false;

            Session["vedi"] = vedi;
            Response.Redirect("InputDati.aspx");
        }
        protected void NewDipendenti(object sender, EventArgs e) 
        {
            vedi[0] = false;
            vedi[1] = false;
            vedi[2] = true;

            Session["vedi"] = vedi;
            Response.Redirect("InputDati.aspx");
        }

    }
}