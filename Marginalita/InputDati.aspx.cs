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
        Dictionary<string, string> dati = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if ( Session["DatiProgetto"] != null)
            {
                 dati = (Dictionary<string, string>)Session["DatiProgetto"];
                DDLMargine.Visible = false;
                DDLSocieta.Visible = false;
                HidID.Value = dati["ID"];

            }

            if (Session["vedi"] == null)
            {
                if (!IsPostBack)
                {
                    Response.Redirect("Anagrafiche.aspx");
                }
            }
            else
            vedi = (bool[])Session["vedi"];

                if (vedi[0] )
            {
                ViewProgetti.Visible = vedi[0];
                ViewSocieta.Visible = vedi[1];
                ViewDipendenti.Visible = vedi[2];


                if (Session["DatiProgetto"] != null)
                {
                    TNomePro.Text = dati["Nome"];
                    TBudget.Text = dati["Budget"];
                    TDurata.Text = dati["Durata"];
                    TDescritione.Text = dati["Descrizione"];
                    //   DDLSocieta.Text = dati["Societa"];
                    //DDLSocieta.DataValueField= dati["Societa"];
                }

            }

            if (vedi[1] )
            {
                ViewProgetti.Visible = vedi[0];
                ViewSocieta.Visible = vedi[1];
                ViewDipendenti.Visible = vedi[2];

                if (Session["DatiProgetto"] != null)
                {
                    TIntestazione.Text = dati["Intestazione"];
                    TEmail.Text = dati["Email"];
                }
         


            }

            if (vedi[2] )
            {
                ViewProgetti.Visible = vedi[0];
                ViewSocieta.Visible = vedi[1];
                ViewDipendenti.Visible = vedi[2];

                if (Session["DatiProgetto"] != null)
                {
                    TLNomeDip.Text = dati["Nome"];
                    TCognome.Text = dati["Cognome"];
                    TCosto.Text = dati["Costo"];
                }
                   

            }

            Session["vedi"] = null;
        }

        //Gestione progetti
        protected void SalProgetto(object sender, EventArgs e)
        {
            DProgetti.Insert();
            
            Response.Redirect("Anagrafiche.aspx");
        }

        protected void ModProgetto(object sender, EventArgs e)
        {
            DProgetti.Update();
            Response.Redirect("Anagrafiche.aspx");
        }

        protected void EliProgetto(object sender, EventArgs e)
        {
            DProgetti.Delete();
            Response.Redirect("Anagrafiche.aspx");
        }


        //Gestione societa
        protected void SalSocieta(object sender, EventArgs e)
        {
            DSocieta.Insert();
            Response.Redirect("Anagrafiche.aspx");
        }

        protected void ModSocieta(object sender, EventArgs e)
        {
            DSocieta.Update();
            Response.Redirect("Anagrafiche.aspx");
        }

        protected void EliSocieta(object sender, EventArgs e)
        {
            DSocieta.Delete();
            Response.Redirect("Anagrafiche.aspx");
        }


        //Gestione dipendenti
        protected void SalDipendenti(object sender, EventArgs e)
        {
            DDipendenti.Insert();
            
            Response.Redirect("Anagrafiche.aspx");
        }

        protected void ModDipendenti(object sender, EventArgs e)
        {
            DDipendenti.Update();
           
            Response.Redirect("Anagrafiche.aspx");
        }

        protected void EliDipendenti(object sender, EventArgs e)
        {
            DDipendenti.Delete();
            
            Response.Redirect("Anagrafiche.aspx");
        }
    }
}