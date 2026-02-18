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
         SelectCommand="SELECT Dipendente, Creata, Costo FROM Fake WHERE MONTH(Creata) = MONTH(GETDATE()) AND YEAR(Creata) = YEAR(GETDATE())"
        InsertCommand="INSERT INTO Fake (Dipendente, Ore, Descrizione, Progetto) VALUES (@Costo, @Fornitore, @Descrizione, @Progetto)">
            <InsertParameters>
               <asp:Parameter Name="Costo" Type="Decimal"/>
               <asp:Parameter Name="Fornitore" Type="String"/>
               <asp:Parameter Name="Descrizione" Type="String"/>
               <asp:Parameter Name="Progetto" Type="Int32"/>
            </InsertParameters>
    </asp:SqlDataSource>

        <asp:SqlDataSource runat="server" ID="CostiEsterni"
         ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
         SelectCommand="SELECT ID, Costo, Fornitore, Descrizione, Progetto FROM CostiEsterni"
         InsertCommand="INSERT INTO CostiEsterni (Costo, Fornitore, Descrizione, Progetto) VALUES (@Costo, @Fornitore, @Descrizione, @Progetto)">
            <InsertParameters>
                <asp:Parameter Name="Costo" Type="Decimal"/>
                <asp:Parameter Name="Fornitore" Type="String"/>
                <asp:Parameter Name="Descrizione" Type="String"/>
                <asp:Parameter Name="Progetto" Type="Int32"/>
            </InsertParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="TabellaAssenze" 
        ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
        SelectCommand="SELECT ID, Ore, DataAssenze, Progetto, Dipendente, Motivo FROM OreAssenze"
        InsertCommand="INSERT INTO OreAssenze (Dipendente, Motivo, Ore, DataAssenze, Progetto) VALUES (@Dipendente, @Motivo, @Ore, @DataAssenze, @Progetto)">
            <InsertParameters>
                <asp:Parameter Name="Dipendente" Type="Int32"/>
                <asp:Parameter Name="Motivo" Type="Int32"/>
                <asp:Parameter Name="Ore" Type="Decimal"/>
                <asp:Parameter Name="DataAssenze" Type="DateTime"/>
                <asp:Parameter Name="Progetto" Type="Int32"/>
                <asp:ControlParameter Name="Durata" ControlID="hfSelectedDates" PropertyName="Value" />
            </InsertParameters>
     </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="TabellaMotivo" 
    ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
    SelectCommand="SELECT ID, Descrizione FROM Motivo"> 
</asp:SqlDataSource>

    <br />
    <br />
    <div id="divOre">
          <table ID="OreSettimanali" border="1" style="border-collapse: collapse; width: 60%;">
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
    </div>
    <br />
    <br />

    <div id="gestioneAssenze">

    <asp:Panel ID="PanelAssenze" class="row-cols-sm-auto gridd" runat="server">
    <asp:GridView ID="GridAssenze" runat="server" DataSourceID="TabellaAssenze" 
        AutoGenerateColumns="False" DataKeyNames="ID" CssClass="table table-bordered table-striped">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
            <asp:BoundField DataField="Dipendente" HeaderText="Dipendente" />
            <asp:BoundField DataField="Motivo" HeaderText="Motivo" />
            <asp:BoundField DataField="Ore" HeaderText="Ore" />
            <asp:BoundField DataField="DataAssenze" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />
        </Columns>
    </asp:GridView>
</asp:Panel>

        <asp:Label id="LDipendente" Text="Dipendente: " runat="server"/>
        <asp:DropDownList ID="AssenzeDDL" runat="server" 
            DataSourceID="TabellaDipendente" 
            DataTextField="Cognome" 
            DataValueField="ID" 
            AutoPostBack="true">
            <asp:ListItem Selected="True" Value="">Scegli dipendente: </asp:ListItem>
        </asp:DropDownList>

        <asp:Label id="LMotivo" Text="Motivo: " runat="server"/>
        <asp:DropDownList ID="MotivoDDL" runat="server"
            DataSourceID="TabellaMotivo" 
            DataTextField="Descrizione" 
            DataValueField="ID" 
            AutoPostBack="true">
        <asp:ListItem Selected="True" Value="">Scegli motivo: </asp:ListItem>
        </asp:DropDownList>

        <asp:Label id="LOre" Text="Ore: " runat="server"/>
        <asp:TextBox runat="server" ID="OreAssenze" TextMode="Number" min="0"/>

        <asp:HiddenField ID="hfSelectedDates" runat="server" />
        <asp:Label id="LData" Text="Date: " runat="server"/>

       <%--<asp:Calendar ID="CDurata" runat="server" OnSelectionChanged="CDurata_SelectionChanged" />--%>
        <div class="calendar-container" style="position: relative;">
            <asp:TextBox ID="txtDataVisualizzata" runat="server" ReadOnly="true" placeholder="Seleziona data: " />
    
            <asp:LinkButton ID="btnApriCalendario" runat="server" OnClick="btnApriCalendario_Click" Text="📅" />

            <asp:Panel ID="pnlCalendario" runat="server" Visible="false" 
                style="position:absolute; z-index:1000; background:white; border:1px solid #ccc;">
                <asp:Calendar ID="CDurata" runat="server" OnSelectionChanged="CDurata_SelectionChanged" />
            </asp:Panel>
        </div>
        </div>

        <asp:Button id="InvioAssenze" Text="Invio" OnClick="Assenze_Click" runat="server"/>

        <br />
        <br />
    </div>

    <div id="gestioneCostiEsterni">
     
    <asp:Panel ID="PanelCostiEsterni" class="row-cols-sm-auto gridd" runat="server">
    <asp:GridView ID="GridCostiEsterni" runat="server" DataSourceID="CostiEsterni" 
        AutoGenerateColumns="False" DataKeyNames="ID" CssClass="table table-bordered table-striped">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
            <asp:BoundField DataField="Costo" HeaderText="Costo" />
            <asp:BoundField DataField="Fornitore" HeaderText="Fornitore" />
            <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" />
        </Columns>
    </asp:GridView>
</asp:Panel>

        <asp:Label id="LIntestazione" Text="Intestazione: " runat="server"/>
        <asp:TextBox runat="server" ID="TIntestazione" AutoPostBack="true"/>

        <asp:Label id="LDescrizione" Text="Descrizione: " runat="server"/>
        <asp:TextBox runat="server" ID="TDescrizione" AutoPostBack="true"/>

        <asp:Label id="LImporto" Text="Importo: " runat="server"/>
        <asp:TextBox runat="server" ID="TImporto" TextMode="Number" AutoPostBack="true"/>

        <asp:Button id="Button1" Text="Invio" OnClick="Extra_Click" runat="server"/>
        <br />
        <br />
    </div>

    <div id="ViewOre" class="col-33" runat="server">

           <asp:Panel ID="PFake" class="row-cols-sm-auto gridd" runat="server">
               <asp:GridView ID="ViewFake" runat="server"
                 AutoGenerateColumns="false"
                 CssClass="table table-bordered table-fixed">
               </asp:GridView>
           </asp:Panel>
    </div>
</asp:Content>