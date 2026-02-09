using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Marginalita
{
    public partial class Timesheet : Page
    {
        string stringaConnessione = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void RepDipendenti_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField idDipendenteHidden = (HiddenField)e.Item.FindControl("HiddenDipendente");
                TextBox txtOre = (TextBox)e.Item.FindControl("InputOre");

                RepeaterItem Progetto = (RepeaterItem)e.Item.Parent.Parent;
                HiddenField idProgettoHidden = (HiddenField)Progetto.FindControl("HiddenProgetto");

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
                string sql = "SELECT Costo FROM Original WHERE Progetto = @p AND Dipendente = @d";
                SqlCommand comando = new SqlCommand(sql, connessione);
                comando.Parameters.AddWithValue("@p", idProgetto);
                comando.Parameters.AddWithValue("@d", idDipendente);

                connessione.Open();
                object risultato = comando.ExecuteScalar();
                return risultato != null ? risultato.ToString() : "";
            }
        }

        protected void InputOre_TextChanged(object sender, EventArgs e)
        {
            TextBox casellaTesto = (TextBox)sender;

            decimal oreInserite;
            if (decimal.TryParse(casellaTesto.Text, out oreInserite))
            {
                if (oreInserite >= 0 && oreInserite <= 8)
                {
                    TextBox casella = (TextBox)sender;

                    RepeaterItem cella = (RepeaterItem)casella.NamingContainer;
                    RepeaterItem riga = (RepeaterItem)cella.Parent.Parent;

                    int idProgetto = int.Parse(((HiddenField)riga.FindControl("HiddenProgetto")).Value);
                    int idDipendente = int.Parse(((HiddenField)cella.FindControl("HiddenDipendente")).Value);

                    int ore = int.TryParse(casella.Text, out int risultato) ? risultato : 0;

                    SalvaDatiConStoredProcedure(idProgetto, idDipendente, ore);
                }
                else
                {
                    casellaTesto.Text = "0";
                }
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
    }
}