using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
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

            if (vedi[0]) 
            { AnaView.DataSourceID = "DProgetti";
              
            }

            if (vedi[1])
            { AnaView.DataSourceID = "DSocieta"; }

            if (vedi[2])
            { AnaView.DataSourceID = "DDipendenti"; }

            
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

        protected void UpPro_Click(object sender, EventArgs e)
        {

            vedi[0] = true;
            vedi[1] = false;
            vedi[2] = false;


          
            Session["vedi"] = vedi;


            var item = ((Button)sender).NamingContainer as ListViewDataItem;
            if (item == null)
                return;

            // Trova la Table (è il primo controllo figlio di item)
            var table = item.Controls.OfType<Table>().FirstOrDefault();
            if (table == null || table.Rows.Count == 0)
                return;

            // Trova la TableRow (è la prima riga della tabella)
            var row = table.Rows[0];

            // Ora puoi trovare i Label nella TableRow
            string id = ((Button)sender).CommandArgument;
            var nome = (row.FindControl("PLNome") as Label)?.Text;
            var budget = (row.FindControl("PLBudget") as Label)?.Text;
            var durata = (row.FindControl("PLDurata") as Label)?.Text;
            var descrizione = (row.FindControl("PLDescrizione") as Label)?.Text;
            var societa = (row.FindControl("PLSocieta") as Label)?.Text;

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
            vedi[0] = false;
            vedi[1] = true;
            vedi[2] = false;

            Session["vedi"] = vedi;

            var item = ((Button)sender).NamingContainer as ListViewDataItem;
            if (item == null)
                return;

            var table = item.Controls.OfType<Table>().FirstOrDefault();
            if (table == null || table.Rows.Count == 0)
                return;

            var row = table.Rows[0];

            string id = ((Button)sender).CommandArgument;
            var intestazione = (row.FindControl("SLIntestazione") as Label)?.Text;
            var email = (row.FindControl("SLEmail") as Label)?.Text;

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
            vedi[0] = false;
            vedi[1] = false;
            vedi[2] = true;

            Session["vedi"] = vedi;

            var item = ((Button)sender).NamingContainer as ListViewDataItem;
            if (item == null)
                return;

            var table = item.Controls.OfType<Table>().FirstOrDefault();
            if (table == null || table.Rows.Count == 0)
                return;

            var row = table.Rows[0];

            string id = ((Button)sender).CommandArgument;
            var nome = (row.FindControl("DLNome") as Label)?.Text;
            var cognome = (row.FindControl("DLCognome") as Label)?.Text;
            var costo = (row.FindControl("DLCostoOrario") as Label)?.Text;

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