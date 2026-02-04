using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Marginalita
{
    public partial class InputDati : System.Web.UI.Page
    {
        bool[] vedi = new bool[3];
        protected void Page_Load(object sender, EventArgs e)
        {
            vedi = (bool[]) Session["vedi"];
            ViewProgetti.Visible = vedi[0];
            ViewSocieta.Visible = vedi[1];
            ViewDipendenti.Visible = vedi[2];
        }

        protected void ModProgetto(object sender, EventArgs e)
        {
            Response.Redirect("Anagrafiche.aspx");
        }

        protected void SalProgetto(object sender, EventArgs e)
        {
        }

        protected void EliProgetto(object sender, EventArgs e)
        {
        }

        protected void ModSocieta(object sender, EventArgs e)
        {
            Response.Redirect("Anagrafiche.aspx");
        }

        protected void SalSocieta(object sender, EventArgs e)
        {
        }

        protected void EliSocieta(object sender, EventArgs e)
        {
        }

        protected void ModDipendenti(object sender, EventArgs e)
        {
            Response.Redirect("Anagrafiche.aspx");
        }

        protected void SalDipendenti(object sender, EventArgs e)
        {
        }

        protected void EliDipendenti(object sender, EventArgs e)
        {
        }
    }
}