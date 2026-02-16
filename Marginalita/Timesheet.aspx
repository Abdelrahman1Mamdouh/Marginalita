<%@ Page Title="Timesheet" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Timesheet.aspx.cs" Inherits="Marginalita.Timesheet" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="content" runat="server" ContentPlaceHolderID="MainContent">
    
    <asp:SqlDataSource runat="server" ID="TabellaProgetto" 
        ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
        SelectCommand="SELECT ID, Nome FROM Progetto"> 
    </asp:SqlDataSource>
    
    <asp:SqlDataSource runat="server" ID="TabellaDipendente" 
        ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
        SelectCommand="SELECT ID, Nome, Cognome, CostoOrario FROM Dipendente"> 
    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="DSFake"
         ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
         SelectCommand="SELECT Dipendente, Creata, Costo FROM Fake WHERE MONTH(Creata) = MONTH(GETDATE()) AND YEAR(Creata) = YEAR(GETDATE())">
    </asp:SqlDataSource>
    <br />
    <br />

  <table border="1" style="border-collapse: collapse; width: 60%;">
    <thead>
        <tr style="background-color:#eee;">
            <th style="padding:10px;">Dipendente</th>
            <th style="padding:10px;">Ore Settimanali</th>
        </tr>
    </thead>
    <tbody>
        <asp:Repeater ID="RepSingolo" runat="server" DataSourceID="TabellaDipendente">
            <ItemTemplate>
                <tr>
                    <td style="padding:10px; font-weight:bold;">
                        <%# Eval("Nome") %> <%# Eval("Cognome") %>
                        <asp:HiddenField ID="HiddenDipendente" runat="server" Value='<%# Eval("ID") %>' />
                        <asp:HiddenField ID="HiddenProgettoFisso" runat="server" Value="1" /> 
                    </td>
                    <td style="padding:10px;">
                        <asp:TextBox runat="server" ID="InputOre" TextMode="Number" min="0" max="40" 
                            Columns="5" AutoPostBack="true" OnTextChanged="InputOre_TextChanged"/>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>
    <br />
    <br />
   <asp:Panel ID="PFake" class="row-cols-sm-auto gridd" runat="server">
       <div id="ViewOre" class="col-33" runat="server">
    
       <asp:GridView ID="ViewFake" runat="server"
         AutoGenerateColumns="false"
         CssClass="table table-bordered table-fixed">
     </asp:GridView>
 </div>
</asp:Panel>
</asp:Content>