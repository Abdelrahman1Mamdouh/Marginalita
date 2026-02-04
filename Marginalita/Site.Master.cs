using System;

namespace Marginalita
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
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
    }
}