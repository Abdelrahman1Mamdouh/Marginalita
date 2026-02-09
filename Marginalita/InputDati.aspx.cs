using System;
using System.Collections;
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
            var dati = (Dictionary<string, string>)Session["evento"];
            //txtID.Text = dati["ID"];
            //txtNome.Text = dati["Nome"];
            //txtBudget.Text = dati["Budget"];
            //txtDurata.Text = dati["Durata"];
            //txtDescrizione.Text = dati["Descrizione"];
            //txtResiduo.Text = dati["Residuo"];
            //txtMargine.Text = dati["Margine"];
            //txtSocieta.Text = dati["Societa"];
        


            vedi = (bool[])Session["input"];
            ViewProgetti.Visible = vedi[0];
            ViewSocieta.Visible = vedi[1];
            ViewDipendenti.Visible = vedi[2];

            if (vedi[0] && dati != null)
            {

                TNomePro.Text = dati["Nome"];
                TBudget.Text = dati["Budget"];
                TDurata.Text = dati["Durata"];
                TDescritione.Text = dati["Descrizione"];
                DDLSocieta.Text = dati["Societa"];
            }

            if (vedi[1] && dati != null)
            {

            }

            if (vedi[2] && dati != null)
            {

            }

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