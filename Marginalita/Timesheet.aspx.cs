using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Marginalita
{
    public partial class Timesheet : Page
    {
        string stringaConnessione = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;";
        private int LimiteCorrente => visualeGiorno.Checked ? 8 : (visualeSettimana.Checked ? 40 : 160);

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void InputOre_TextChanged(object sender, EventArgs e)
        {
            TextBox casellaTesto = (TextBox)sender;

            if (decimal.TryParse(casellaTesto.Text, out decimal oreInserite))
            {
                if (oreInserite >= 0 && oreInserite <= LimiteCorrente)
                {
                    RepeaterItem cella = (RepeaterItem)casellaTesto.NamingContainer;
                    RepeaterItem riga = (RepeaterItem)cella.Parent.Parent;

                    int idProgetto = int.Parse(((HiddenField)riga.FindControl("HiddenProgetto")).Value);
                    int idDipendente = int.Parse(((HiddenField)cella.FindControl("HiddenDipendente")).Value);

                    SalvaDatiConStoredProcedure(idProgetto, idDipendente, (int)oreInserite);
                }
                else
                {
                    casellaTesto.Text = LimiteCorrente.ToString();
                }
            }
        }

        protected void RepDipendenti_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TextBox txtOre = (TextBox)e.Item.FindControl("InputOre");
                HiddenField idDipendenteHidden = (HiddenField)e.Item.FindControl("HiddenDipendente");

                RepeaterItem Progetto = (RepeaterItem)e.Item.Parent.Parent;
                HiddenField idProgettoHidden = (HiddenField)Progetto.FindControl("HiddenProgetto");

                txtOre.Attributes["max"] = LimiteCorrente.ToString();

                if (idDipendenteHidden != null && idProgettoHidden != null)
                {
                    txtOre.Text = RecuperaOreDalDatabase(idProgettoHidden.Value, idDipendenteHidden.Value);
                }
            }
        }

        private string RecuperaOreDalDatabase(string idProgetto, string idDipendente)
        {
            using (SqlConnection connessione = new SqlConnection(stringaConnessione))
            {
                string sql = "SELECT Ore FROM Original WHERE Progetto = @p AND Dipendente = @d";
                SqlCommand comando = new SqlCommand(sql, connessione);
                comando.Parameters.AddWithValue("@p", idProgetto);
                comando.Parameters.AddWithValue("@d", idDipendente);
                connessione.Open();
                object risultato = comando.ExecuteScalar();
                return risultato != null ? risultato.ToString() : "";
            }
        }

        private void SalvaDatiConStoredProcedure(int idProgetto, int idDipendente, int ore)
        {
            using (SqlConnection connessione = new SqlConnection(stringaConnessione))
            {
                SqlCommand comando = new SqlCommand("SalvaOre", connessione);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@idProgetto", idProgetto);
                comando.Parameters.AddWithValue("@idDipendente", idDipendente);
                comando.Parameters.AddWithValue("@costo", ore);
                connessione.Open();
                comando.ExecuteNonQuery();
            }
        }
        protected void visualizzaGiorno(object sender, EventArgs e) { }
        protected void visualizzaSettimana(object sender, EventArgs e) { }
        protected void visualizzaMese(object sender, EventArgs e) { }

        //private void GestisciColore(TextBox txt, int valore)
        //{
        //    txt.CssClass = "ora-base";

        //    if (visualeGiorno.Checked)
        //    {
        //        if (valore == 6 || valore == 7) txt.CssClass = "ora-warning";
        //        else if (valore >= 8) txt.CssClass = "ora-danger";
        //    }
        //    else if (visualeSettimana.Checked)
        //    {
        //        if (valore >= 35 && valore <= 39) txt.CssClass = "ora-warning";
        //        else if (valore >= 40) txt.CssClass = "ora-danger";
        //    }
        //    else if (visualeMese.Checked)
        //    {
        //        if (valore >= 150 && valore <= 159) txt.CssClass = "ora-warning";
        //        else if (valore >= 160) txt.CssClass = "ora-danger";
        //    }
        //}
    }
}