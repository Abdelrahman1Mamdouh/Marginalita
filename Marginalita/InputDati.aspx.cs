using Antlr.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
            if (Session["DatiProgetto"] != null)
            {
                dati = (Dictionary<string, string>)Session["DatiProgetto"];
                DropDownMargine.Visible = false;
                DDLMargine.Visible = false;
                DropDownSocieta.Visible = false;
                DDLSocieta.Visible = false;
                if (dati.ContainsKey("ID")) HID.Value = dati["ID"];
                SalDip.Visible = false;
                SalSoc.Visible = false;
                SalProg.Visible = false;
                AnnullaProg.Visible = true;
                AnnullaSoc.Visible = true;
                AnnullaDip.Visible = true;
            }
            else
            {
                ModDip.Visible = false;
                ModSoc.Visible = false;
                ModProg.Visible = false;
                EliDip.Visible = false;
                EliSoc.Visible = false;
                EliProg.Visible = false;
            }

            if (Session["vedi"] == null)
            {
                if (!IsPostBack)
                {
                    Response.Redirect("Anagrafiche.aspx");
                    return;
                }
            }
            else
            {
                vedi = (bool[])Session["vedi"];
            }

            // Impostiamo sempre le visibilità in base a 'vedi'
            ViewProgetti.Visible = vedi[0];
            ViewSocieta.Visible = vedi[1];
            ViewDipendenti.Visible = vedi[2];

            // Popoliamo i campi solo al primo caricamento per evitare di sovrascrivere input utente in postback
            if (!IsPostBack && Session["DatiProgetto"] != null)
            {
                // Progetti
                if (vedi[0])
                {
                    TNomePro.Text = dati.ContainsKey("Nome") ? dati["Nome"] : "";
                    TBudget.Text = dati.ContainsKey("Budget") ? dati["Budget"] : "";

                    if (dati.ContainsKey("Inizio") && !string.IsNullOrWhiteSpace(dati["Inizio"]))
                    {
                        var s = dati["Inizio"];
                        DateTime parsedIn;
                        if (DateTime.TryParseExact(s, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out parsedIn)
                            || DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out parsedIn)
                            || DateTime.TryParse(s, out parsedIn))
                        {
                            CDInizio.SelectedDate = parsedIn;
                        }
                    }

                    if (dati.ContainsKey("Fine") && !string.IsNullOrWhiteSpace(dati["Fine"]))
                    {
                        var s = dati["Fine"];
                        DateTime parsedFin;
                        if (DateTime.TryParseExact(s, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out parsedFin)
                            || DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out parsedFin)
                            || DateTime.TryParse(s, out parsedFin))
                        {
                            CDFine.SelectedDate = parsedFin;
                        }
                    }

                    TDescritione.Text = dati.ContainsKey("Descrizione") ? dati["Descrizione"] : "";
                }

                // Societa
                if (vedi[1])
                {
                    TIntestazione.Text = dati.ContainsKey("Intestazione") ? dati["Intestazione"] : "";
                    TEmail.Text = dati.ContainsKey("Email") ? dati["Email"] : "";
                    // se necessario impostare DDLSocieta.SelectedValue qui
                }

                // Dipendenti
                if (vedi[2])
                {
                    TLNomeDip.Text = dati.ContainsKey("Nome") ? dati["Nome"] : "";
                    TCognome.Text = dati.ContainsKey("Cognome") ? dati["Cognome"] : "";
                    TCosto.Text = dati.ContainsKey("Costo") ? dati["Costo"] : "";
                }
            }
        }

        //Gestione progetti
        protected void SalProgetto(object sender, EventArgs e)
        {
            // DProgetti Insert usa CDInizio per il parametro Durata
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
            DFake.Delete();
            Response.Redirect("Anagrafiche.aspx");
        }
        protected void AnnullaProgetto(object sender, EventArgs e)
        {
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
            DProgetti.Delete();
            DFake.Delete();
            Response.Redirect("Anagrafiche.aspx");
        }
        protected void AnnullaSocieta(object sender, EventArgs e)
        {
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

        protected void AnnullaDipendenti(object sender, EventArgs e)
        {
            Response.Redirect("Anagrafiche.aspx");
        }
    }
}