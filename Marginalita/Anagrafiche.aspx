<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Anagrafiche.aspx.cs" Inherits="Marginalita.Anagrafiche" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section >
            <h1>Anagrafiche</h1>         
        </section>
        <asp:Panel id="PAnagrafica" class="row-cols-sm-auto"  runat="server">
        <div id="ViewProgetti" class="col-sm-4" runat="server">

            <asp:SqlDataSource ID="DProgetti" runat="server"
                ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
                SelectCommand="SELECT * FROM Progetto">
             </asp:SqlDataSource>

            <asp:ListView ID="LProgetti" DataSourceID="DProgetti" DataKeyName="IDProgetto" runat="server">


            </asp:ListView>
            <asp:Button ID="NewProg" runat="server" Text="New" OnClick="NewProgetto" />   
        
        </div>

        <div id="ViewSocieta" class="col-sm-4" runat="server">
            <asp:SqlDataSource ID="DSocieta" runat="server"
                ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
                SelectCommand="SELECT * FROM Societa">
             </asp:SqlDataSource>

            <asp:ListView ID="LSocieta" DataSourceID="DSocieta" DataKeyName="IDSocieta" runat="server">


            </asp:ListView>
  
             <asp:Button ID="NewSoc" runat="server" Text="New" OnClick="NewSocieta" />   

         </div>


        <div id="ViewDipendenti" class="col-sm-4" runat="server">
            <asp:SqlDataSource ID="DDipendenti" runat="server"
                ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
                SelectCommand="SELECT * FROM Dipendenti">
             </asp:SqlDataSource>

            <asp:ListView ID="LDipendenti"  DataSourceID="DDipendenti" DataKeyName="IDDipendenti" runat="server">


            </asp:ListView>
  
                <asp:Button ID="NewDip" runat="server" Text="New" OnClick="NewDipendenti" />   

         </div>
            </asp:Panel>
    </main>

</asp:Content>
