using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace Marginalita
{
    public partial class Anagrafiche : System.Web.UI.Page
    {

        bool[] vedi = new bool[3];
        bool[] inputDati = new bool[3];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["vedi"] == null)
            {
                vedi[0] = true;
                vedi[1] = true;
                vedi[2] = true;
            }
            else { vedi = (bool[])Session["vedi"]; }
            ViewProgetti.Visible = vedi[0];
            ViewSocieta.Visible = vedi[1];
            ViewDipendenti.Visible = vedi[2];
        }

        protected void NewProgetto(object sender, EventArgs e)
        {
            inputDati[0] = true;
            inputDati[1] = false;
            inputDati[2] = false;


            Session["evento"] = null;
            Session["input"] = inputDati;
            Response.Redirect("InputDati.aspx");
        }
        protected void NewProgetto(object sender, ListViewItemEventArgs e)
        {
            inputDati[0] = true;
            inputDati[1] = false;
            inputDati[2] = false;

            
            Session["evento"] = new Dictionary<string, string>
        {
            { "ID",  e.Item.FindControl("ID").ToString() },
            { "Nome", ((Label)e.Item.FindControl("PLNome")).Text },
            { "Budget", ((Label)e.Item.FindControl("PLBudget")).Text },
            { "Durata",((Label)e.Item.FindControl("PLDurata")).Text },
            { "Descrizione", ((Label)e.Item.FindControl("PLDescrizione")).Text },
          
            { "Societa", ((Label)e.Item.FindControl("PLSocieta")).Text}
        };
            Session["input"] = inputDati;
            Response.Redirect("InputDati.aspx");
        }
        protected void NewSocieta(object sender, EventArgs e)
        {
            inputDati[0] = false;
            inputDati[1] = true;
            inputDati[2] = false;

            Session["evento"] = null;
            Session["input"] = inputDati;
            Response.Redirect("InputDati.aspx");
        }
        protected void NewSocieta(object sender, ListViewItemEventArgs e)
        {
            inputDati[0] = false;
            inputDati[1] = true;
            inputDati[2] = false;

            Session["evento"] = e;
            Session["input"] = inputDati;
            Response.Redirect("InputDati.aspx");
        }
        protected void NewDipendente(object sender, EventArgs e)
        {
            inputDati[0] = false;
            inputDati[1] = false;
            inputDati[2] = true;

            Session["evento"] = null;
            Session["input"] = inputDati;
            Response.Redirect("InputDati.aspx");
        }

        protected void NewDipendente(object sender, ListViewItemEventArgs e)
        {
            inputDati[0] = false;
            inputDati[1] = false;
            inputDati[2] = true;

            Session["evento"] = e;
            Session["input"] = inputDati;
            Response.Redirect("InputDati.aspx");
        }


    }
}