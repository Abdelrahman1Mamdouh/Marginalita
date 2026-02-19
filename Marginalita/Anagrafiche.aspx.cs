using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Marginalita
{
    public partial class Anagrafiche : System.Web.UI.Page
    {
        bool[] vedi = new bool[3];

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
            vedi[0] = true;
            vedi[1] = false;
            vedi[2] = false;

            Session["DatiProgetto"] = null;
            Session["vedi"] = vedi;
            Response.Redirect("InputDati.aspx");
        }

        protected void NewSocieta(object sender, EventArgs e)
        {
            vedi[0] = false;
            vedi[1] = true;
            vedi[2] = false;

            Session["DatiProgetto"] = null;
            Session["vedi"] = vedi;
            Response.Redirect("InputDati.aspx");
        }

        protected void NewDipendente(object sender, EventArgs e)
        {
            vedi[0] = false;
            vedi[1] = false;
            vedi[2] = true;

            Session["DatiProgetto"] = null;
            Session["vedi"] = vedi;
            Response.Redirect("InputDati.aspx");
        }

        // Utility: prova a trovare una Label ricorsivamente nella gerarchia del container
        private Label FindLabelRecursive(Control container, string id)
        {
            if (container == null) return null;
            var lbl = container.FindControl(id) as Label;
            if (lbl != null) return lbl;
            foreach (Control c in container.Controls)
            {
                lbl = FindLabelRecursive(c, id);
                if (lbl != null) return lbl;
            }
            return null;
        }

        protected void UpPro_Click(object sender, EventArgs e)
        {
            vedi[0] = true; vedi[1] = false; vedi[2] = false;
            Session["vedi"] = vedi;

            var btn = (Button)sender;
            var container = btn.NamingContainer as Control;
            if (container == null) return;

            string id = btn.CommandArgument;

            // trova le label: supporta sia GridViewRow che vecchia ListViewDataItem
            var nomeLbl = FindLabelRecursive(container, "PLNome");
            var budgetLbl = FindLabelRecursive(container, "PLBudget");
            var durataLbl = FindLabelRecursive(container, "PLDurata");
            var descrLbl = FindLabelRecursive(container, "PLDescrizione");
            var societaLbl = FindLabelRecursive(container, "PLSocieta");

            var nome = nomeLbl?.Text;
            var budget = budgetLbl?.Text;
            var durata = durataLbl?.Text;
            var descrizione = descrLbl?.Text;
            var societa = societaLbl?.Text;

            Session["DatiProgetto"] = new Dictionary<string, string>
            {
                { "ID", id },
                { "Nome", nome },
                { "Budget", budget },
                { "Durata", durata },
                { "Descrizione", descrizione },
                { "Societa", societa },
            };
            Response.Redirect("InputDati.aspx");
        }

        protected void UpSocieta_Click(object sender, EventArgs e)
        {
            vedi[0] = false; vedi[1] = true; vedi[2] = false;
            Session["vedi"] = vedi;

            var btn = (Button)sender;
            var container = btn.NamingContainer as Control;
            if (container == null) return;

            string id = btn.CommandArgument;

            var intestazioneLbl = FindLabelRecursive(container, "SLIntestazione");
            var emailLbl = FindLabelRecursive(container, "SLEmail");

            var intestazione = intestazioneLbl?.Text;
            var email = emailLbl?.Text;

            Session["DatiProgetto"] = new Dictionary<string, string>
            {
                { "ID", id },
                { "Intestazione", intestazione },
                { "Email", email }
            };
            Response.Redirect("InputDati.aspx");
        }

        protected void UpDipendente_Click(object sender, EventArgs e)
        {
            vedi[0] = false; vedi[1] = false; vedi[2] = true;
            Session["vedi"] = vedi;

            var btn = (Button)sender;
            var container = btn.NamingContainer as Control;
            if (container == null) return;

            string id = btn.CommandArgument;

            var nomeLbl = FindLabelRecursive(container, "DLNome");
            var cognomeLbl = FindLabelRecursive(container, "DLCognome");
            var costoLbl = FindLabelRecursive(container, "DLCostoOrario");

            var nome = nomeLbl?.Text;
            var cognome = cognomeLbl?.Text;
            var costo = costoLbl?.Text;

            Session["DatiProgetto"] = new Dictionary<string, string>
            {
                { "ID", id },
                { "Nome", nome },
                { "Cognome", cognome },
                { "Costo", costo }
            };
            Response.Redirect("InputDati.aspx");
        }
    }
}