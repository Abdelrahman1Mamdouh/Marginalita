<%@ Page Title="Timesheet" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Timesheet.aspx.cs" Inherits="Marginalita.Timesheet" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="content" runat="server" ContentPlaceHolderID="MainContent">
	    
		
		<asp:SqlDataSource runat="server" ID="Time" 
            ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
            SelectCommand="SELECT Nome, Cognome, CostoOrario FROM Dipendenti"> 
        </asp:SqlDataSource>

<%--        <asp:GridView runat="server" ID="Orari" DataSourceID="Time" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="Dipendente">
                    <ItemTemplate>
                        <%# Eval("Nome") %>
                        <%# Eval("Cognome") %>
                        <%# Eval("CostoOrario") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
    </asp:GridView>--%>
         <asp:Repeater id="Repeater2" runat="server">
         
          <HeaderTemplate>
             Company data:
          </HeaderTemplate>
             
          <ItemTemplate>
             <%# DataBinder.Eval(Container.DataItem, "Nome") %> (<%# DataBinder.Eval(Container.DataItem, "Nome") %>)
          </ItemTemplate>
             
          <SeparatorTemplate>, </SeparatorTemplate>
       </asp:Repeater>
</asp:Content>