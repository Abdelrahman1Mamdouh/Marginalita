<%@ Page Title="Timesheet" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Timesheet.aspx.cs" Inherits="Marginalita.Timesheet" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="content" runat="server" ContentPlaceHolderID="MainContent">
    
    <asp:SqlDataSource runat="server" ID="TabellaProgetto" 
        ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
        SelectCommand="SELECT ID, Nome FROM Progetto"> 
    </asp:SqlDataSource>
    
    <asp:SqlDataSource runat="server" ID="TabellaDipendente" 
        ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
        SelectCommand="SELECT ID, Nome, Cognome FROM Dipendente"> 
    </asp:SqlDataSource>
    
    <div id="visualeOre">
        <p>Visualizza per:</p>
        <asp:RadioButton ID="visualeGiorno" runat="server" 
            GroupName="PeriodoSelezione" 
            Text="Giorno" 
            AutoPostBack="true" 
            OnCheckedChanged="visualizzaGiorno" 
            Checked="true" />

        <asp:RadioButton ID="visualeSettimana" runat="server" 
            GroupName="PeriodoSelezione" 
            Text="Settimana" 
            AutoPostBack="true" 
            OnCheckedChanged="visualizzaSettimana" />

        <asp:RadioButton ID="visualeMese" runat="server" 
            GroupName="PeriodoSelezione" 
            Text="Mese" 
            AutoPostBack="true" 
            OnCheckedChanged="visualizzaMese" />
    </div>

    <table border="1" style="border-collapse: collapse; width: 100%;">
        <thead>
            <tr>
                <th style="padding:5px; background-color:#eee;"> </th>
                <asp:Repeater ID="RepDipendente" runat="server" DataSourceID="TabellaDipendente">
                    <ItemTemplate>
                        <th style="padding:5px;"><%# Eval("Nome") %> <%# Eval("Cognome") %></th>
                    </ItemTemplate>
                </asp:Repeater>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="RepProgetto" runat="server" DataSourceID="TabellaProgetto">
                <ItemTemplate>
                    <tr>
                        <td style="padding:5px; font-weight:bold;"><%# Eval("Nome") %>
                            <asp:HiddenField ID="HiddenProgetto" runat="server" Value='<%# Eval("ID") %>' />
                        </td>
                        <asp:Repeater runat="server" ID="RepCelle" DataSourceID="TabellaDipendente" OnItemDataBound="RepDipendenti_ItemDataBound">
                            <ItemTemplate>
                                <td style="padding:5px;">
                                    <asp:HiddenField ID="HiddenDipendente" runat="server" Value='<%# Eval("ID") %>' />
                                    <asp:TextBox runat="server" ID="InputOre" TextMode="Number" Columns="1" AutoPostBack="true" OnTextChanged="InputOre_TextChanged"/>
                                </td>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr> 
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
</asp:Content>