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
            ApplySidebar();
        }
        protected void BtnToggleMenu_Click(object sender, EventArgs e)
        {
            IsSidebarOpen = !IsSidebarOpen;
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
            AnProg.Visible = true;
            AnSoc.Visible = true;
            AnDip.Visible = true;
        }

        protected void Progetti(object sender, EventArgs e)
        {
            vedi[0] = true;
            vedi[1] = false;
            vedi[2] = false;

            Session["vedi"] = vedi;
            Response.Redirect("Anagrafiche.aspx");
        }

        protected void Societa(object sender, EventArgs e) 
        {
            vedi[0] = false;
            vedi[1] = true;
            vedi[2] = false;

            Session["vedi"] = vedi;
            Response.Redirect("Anagrafiche.aspx");
        }

        protected void Dipendenti(object sender, EventArgs e) 
        {
            vedi[0] = false;
            vedi[1] = false;
            vedi[2] = true;

            Session["vedi"] = vedi;
            Response.Redirect("Anagrafiche.aspx");
        }

    }
}
