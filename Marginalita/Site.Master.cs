using System;

namespace Marginalita
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        bool[] vedi = new bool[3];

        private bool IsSidebarOpen
        {
            get { return Session["SidebarOpen"] != null && (bool)Session["SidebarOpen"]; }
            set { Session["SidebarOpen"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IsSidebarOpen = false;
                lstProjects.Visible = false;
            }

            ApplySidebar();
        }
        protected void BtnToggleMenu_Click(object sender, EventArgs e)
        {
            IsSidebarOpen = !IsSidebarOpen;
            ApplySidebar();
        }
        protected void BtnCloseSidebar_Click(object sender, EventArgs e)
        {
            IsSidebarOpen = false;
            ApplySidebar();
        }

        private void ApplySidebar()
        {
            pnlSidebar.CssClass = IsSidebarOpen ? "sidebar open" : "sidebar";

            btnOverlay.Visible = IsSidebarOpen;
            btnOverlay.CssClass = IsSidebarOpen ? "sidebar-overlay show" : "sidebar-overlay";
        }
        protected void SubMenu(object sender, EventArgs e)
        {
            IsSidebarOpen = true;

            bool show = !AnProg.Visible;
            AnProg.Visible = show;
            AnSoc.Visible = show;
            AnDip.Visible = show;

            ApplySidebar();
        }

        protected void Progetti(object sender, EventArgs e)
        {
            vedi[0] = true;
            vedi[1] = false;
            vedi[2] = false;

            Session["vedi"] = vedi;
            IsSidebarOpen = false;
            Response.Redirect("Anagrafiche.aspx");
        }

        protected void Societa(object sender, EventArgs e)
        {
            vedi[0] = false;
            vedi[1] = true;
            vedi[2] = false;

            Session["vedi"] = vedi;
            IsSidebarOpen = false;
            Response.Redirect("Anagrafiche.aspx");
        }

        protected void Dipendenti(object sender, EventArgs e)
        {
            vedi[0] = false;
            vedi[1] = false;
            vedi[2] = true;

            Session["vedi"] = vedi;

            IsSidebarOpen = false;
            Response.Redirect("Anagrafiche.aspx");
        }
        protected void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string term = txtSearch.Text.Trim();

            if (term.Length < 2)
            {
                lstProjects.Visible = false;
                lstProjects.Items.Clear();
                return;
            }

            SearchProgetti.SelectParameters["term"].DefaultValue = term;

            lstProjects.DataBind();

            if (lstProjects.Items.Count == 0)
            {
                lstProjects.Visible = false;
            }
            else
            {
                lstProjects.Visible = true;
            }
        }

        protected void LstProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedId = lstProjects.SelectedValue;

            if (!string.IsNullOrEmpty(selectedId))
            {
                Response.Redirect("dettagliProgetto.aspx?id=" + selectedId);
            }
        }
    }
}
