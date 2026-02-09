<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InputDati.aspx.cs" Inherits="Marginalita.InputDati" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section>
            <h1>Inserimento dati</h1>
        </section>


        <asp:SqlDataSource ID="DSocieta" runat="server"
            ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
            SelectCommand="SELECT * FROM Societa"></asp:SqlDataSource>

        <asp:SqlDataSource ID="DContratto" runat="server"
            ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
            SelectCommand="SELECT * FROM Contratto"></asp:SqlDataSource>

        <div id="ViewProgetti" runat="server">
            <h2>Progetti</h2>


            <asp:Label ID="LNomePro" Text="Nome progetto" runat="server" />
            <asp:TextBox runat="server" ID="TNomePro" />

            <asp:Label ID="LBudget" Text="Budget progetto" runat="server" />
            <asp:TextBox runat="server" ID="TBudget" />

            <asp:Label ID="LDurata" Text="Durata progetto" runat="server" />
            <asp:TextBox runat="server" ID="TDurata" />

            <asp:Label ID="LDescritione" TextMode="Multiline" Text="Descritione progetto" runat="server" />
            <asp:TextBox runat="server" ID="TDescritione" />

            <asp:DropDownList ID="DDLSocieta" AutoPostBack="true" DataSourceID="DSocieta" DataTextFiled="Intestazione" runat="server"></asp:DropDownList>

            <asp:DropDownList ID="DDLMargine" AutoPostBack="true" DataSourceID="DContratto" DataTextFiled="Canone" runat="server"></asp:DropDownList>





            <asp:Button ID="ModProg" runat="server" Text="Modifica" OnClick="ModProgetto" />
            <asp:Button ID="SalProg" runat="server" Text="Salva" OnClick="SalProgetto" />
            <asp:Button ID="EliProg" runat="server" Text="Elimina" OnClick="EliProgetto" />

        </div>

        <div id="ViewSocieta" runat="server">
            <h2>Societa</h2>

            <asp:Label ID="LIntestazione" Text="Intestazione societa" runat="server" />
            <asp:TextBox runat="server" ID="TIntestazione" />

            <asp:Label ID="LEmail" Text="Email societa" runat="server" />
            <asp:TextBox runat="server" ID="TEmail" />

            <asp:Button ID="ModSoc" runat="server" Text="Modifica" OnClick="ModSocieta" />
            <asp:Button ID="SalSoc" runat="server" Text="Salva" OnClick="SalSocieta" />
            <asp:Button ID="EliSoc" runat="server" Text="Elimina" OnClick="EliSocieta" />
        </div>


        <div id="ViewDipendenti" runat="server">

            <h2>Dipendenti</h2>

            <asp:Label ID="LNomeDip" Text="Nome" runat="server" />
            <asp:TextBox runat="server" ID="TLNomeDip" />

            <asp:Label ID="LCognome" Text="Cognome" runat="server" />
            <asp:TextBox runat="server" ID="TCognome" />

            <asp:Label ID="LCosto" Text="Cognome orario" runat="server" />
            <asp:TextBox runat="server" ID="Cognome" />



            <asp:Button ID="ModDip" runat="server" Text="Modifica" OnClick="ModDipendenti" />
            <asp:Button ID="SalDip" runat="server" Text="Salva" OnClick="SalDipendenti" />
            <asp:Button ID="EliDip" runat="server" Text="Elimina" OnClick="EliDipendenti" />

        </div>

    </main>

</asp:Content>