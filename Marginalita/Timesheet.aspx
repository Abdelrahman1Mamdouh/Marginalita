<%@ Page Title="Timesheet" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Timesheet.aspx.cs" Inherits="Marginalita.Timesheet" %>

<asp:Content ID="content" runat="server" ContentPlaceHolderID="MainContent">

    <asp:HiddenField ID="CostiAssenzeID" runat="server" />
    <asp:HiddenField ID="DeleteCostiAssenze" runat="server" Value="0" />

    <asp:SqlDataSource runat="server" ID="TabellaProgetto"
        ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
        SelectCommand="SELECT ID, Nome FROM Progetto" />

    <asp:SqlDataSource runat="server" ID="TabellaDipendente"
        ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
        SelectCommand="
            Select
                d.ID,
                d.Nome,
                d.Cognome,
                CONCAT(d.Nome, ' ' , d.Cognome) AS Nominativo,
            CAST(ISNULL(x.OreInterne, 0) AS DECIMAL(10,2)) AS OreInterne,
            CAST(ISNULL(x.OreEsterne, 0) AS DECIMAL(10,2)) AS OreEsterne
        FROM Dipendente d
        LEFT JOIN
        (
            SELECT
                f.Dipendente,
                SUM(CASE WHEN f.Vedi = 0 AND f.Progetto IS NOT NULL THEN (f.Costo / NULLIF(d.CostoOrario,0)) ELSE 0 END) AS OreInterne,
                SUM(CASE WHEN f.Vedi = 1 AND f.Progetto IS NOT NULL THEN (f.Costo / NULLIF(d.CostoOrario,0)) ELSE 0 END) AS OreEsterne
            FROM Fake f
            INNER JOIN Dipendente d ON d.ID = f.Dipendente
            WHERE f.Creata &gt; = DATEFROMPARTS(YEAR(@Monday), MONTH(@Monday), 1)
            AND f.Creata &lt; DATEADD(DAY, 1, EOMONTH(@Monday))
            GROUP BY f.Dipendente
        ) x ON x.Dipendente = d.ID
        ORDER BY d.ID
    ">
        <SelectParameters>
            <asp:Parameter Name="Monday" Type="DateTime" />
        </SelectParameters>
    </asp:SqlDataSource>


    <asp:SqlDataSource runat="server" ID="CostiEsterni"
        ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
        SelectCommand="SELECT ID, DipendenteID, ProgettoID FROM DipendenteProgetto"
        InsertCommand="INSERT INTO DipendenteProgetto (DipendenteID, ProgettoID) VALUES (@DipendenteID, @ProgettoID)"
        DeleteCommand="DELETE FROM DipendenteProgetto WHERE ID=@ID">
        <DeleteParameters>
            <asp:Parameter Name="ID" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="DipendenteID" Type="Int32" />
            <asp:Parameter Name="ProgettoID" Type="Int32" />


        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="TabellaAssenze"
        ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
        SelectCommand="SELECT ID, Ore, DataAssenze, Dipendente, Motivo FROM V_OreAssenze"
        InsertCommand="INSERT INTO OreAssenze (Dipendente, Motivo, Ore, DataAssenze) VALUES (@Dipendente, @Motivo, @Ore, @DataAssenze)"
        DeleteCommand="DELETE FROM OreAssenze WHERE ID = @ID">

        <DeleteParameters>
            <asp:Parameter Name="ID" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="Dipendente" Type="Int32" />
            <asp:Parameter Name="Motivo" Type="Int32" />
            <asp:Parameter Name="Ore" Type="Decimal" />
            <asp:Parameter Name="DataAssenze" Type="DateTime" />

        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="TabellaMotivo"
        ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
        SelectCommand="SELECT ID, Descrizione FROM Motivo" />

    <div class="container-fluid mt-4">
        <section class="DSCard-grid">


            <div id="gestioneAssenze" class="DSCard-card p-3">
                <div class="DSCard-text w-100">
                    <div class="DSCard-label"><i class="bi bi-calendar"></i>Assenze</div>
                    <div class="mt-2 overflow-auto mb-3" style="max-height: 120px;">
                        <asp:GridView ID="GridAssenze" runat="server" DataSourceID="TabellaAssenze" AutoGenerateColumns="False" DataKeyNames="ID" CssClass="table table-sm border-0 small" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="Dipendente" HeaderText="Dipendente" />
                                <asp:BoundField DataField="Ore" HeaderText="Ore" />
                                <asp:BoundField DataField="DataAssenze" HeaderText="Data" DataFormatString="{0:dd/MM}" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnDelete" runat="server" CommandName="Delete" OnClientClick="return confirm('Eliminare?');" CssClass="text-danger"><i class="bi bi-trash"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="bg-light p-2 rounded">
                        <asp:DropDownList ID="AssenzeDDL" runat="server" DataSourceID="TabellaDipendente" DataTextField="Nominativo" DataValueField="ID" AppendDataBoundItems="true" CssClass="form-select form-select-sm mb-1">
                            <asp:ListItem Value="0">Seleziona dipendente</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="MotivoDDL" runat="server" DataSourceID="TabellaMotivo" DataTextField="Descrizione" DataValueField="ID" AppendDataBoundItems="true" CssClass="form-select form-select-sm mb-1">
                            <asp:ListItem Value="">Seleziona motivo</asp:ListItem>
                        </asp:DropDownList>
                        <div class="d-flex gap-1 mb-1">
                            <asp:TextBox runat="server" ID="OreAssenze" TextMode="Number" placeholder="Ore" CssClass="form-control form-control-sm" />
                            <asp:TextBox ID="txtDataVisualizzata" runat="server" ReadOnly="true" placeholder="Data" CssClass="form-control form-control-sm" />
                            <asp:LinkButton ID="btnApriCalendario" runat="server" OnClick="btnApriCalendario_Click" CssClass="btn btn-outline-secondary btn-sm">
                                <i class="bi bi-calendar"></i>
                            </asp:LinkButton>
                        </div>
                        <asp:Panel ID="pnlCalendario" runat="server" Visible="false" CssClass="position-absolute bg-white border shadow p-1" Style="z-index: 1000;">
                            <asp:Calendar ID="CDurata" runat="server" OnSelectionChanged="CDurata_SelectionChanged" />
                        </asp:Panel>
                        <asp:Button ID="InvioAssenze" Text="+ Aggiungi" OnClick="Assenze_Click" runat="server" CssClass="btn btn-dark btn-sm w-100 mt-1" />
                    </div>
                </div>
            </div>

            <div id="gestioneVincoli" class="DSCard-card p-3" runat="server" style="display: flex; flex-direction: column;">
                <div class="DSCard-text w-100" style="flex: 1 1 auto;">
                    <div class="DSCard-label"><i class="bi bi-receipt"></i>Vincoli</div>

                    <div class="mt-2 overflow-auto mb-3" style="max-height: 120px; width: 100%;">
                        <asp:GridView ID="GridCostiEsterni" runat="server"
                            DataSourceID="CostiEsterni"
                            AutoGenerateColumns="False"
                            DataKeyNames="ID"
                            CssClass="table table-sm border-0 small"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="DipendenteID" HeaderText="Dipendente" />
                                <asp:BoundField DataField="ProgettoID" HeaderText="Progetto" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnDelete" runat="server" CommandName="Delete"
                                            CssClass="text-danger">
                                                        <i class="bi bi-trash"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="bg-light p-2 rounded">
                    <div class="d-flex flex-column" style="width: 100%;">

                        <asp:DropDownList ID="DDLDipendenteVincoli" runat="server"
                            DataSourceID="TabellaDipendente"
                            DataTextField="Nominativo"
                            DataValueField="ID"
                            AppendDataBoundItems="true"
                            AutoPostBack="true"
                            CssClass="form-select form-select-sm mb-2"
                            Style="width: 100% !important; max-width: none !important; display: block;">
                            <asp:ListItem Value="0">Seleziona dipendente</asp:ListItem>
                        </asp:DropDownList>

                        <asp:DropDownList ID="DDLProgettiVincoli" runat="server"
                            DataSourceID="TabellaProgetto"
                            DataTextField="Nome"
                            DataValueField="ID"
                            AppendDataBoundItems="true"
                            AutoPostBack="true"
                            CssClass="form-select form-select-sm mb-2"
                            Style="width: 100% !important; max-width: none !important; display: block;">
                            <asp:ListItem Value="0">Seleziona progetto</asp:ListItem>
                        </asp:DropDownList>

                        <asp:Button ID="BExtra" Text="+ Aggiungi" OnClick="Extra_Click" runat="server"
                            CssClass="btn btn-dark btn-sm mt-1"
                            Style="width: 100% !important; display: block;" />
                    </div>
                </div>

            </div>

            <div id="divOre" class="DSCard-card p-3">
                <div class="DSCard-text w-100">
                    <div class="DSCard-label"><i class="bi bi-person-badge"></i>Ore Mensili</div>
                    <div class="mt-3 overflow-auto" style="max-height: 250px;">
                        <table class="table table-sm border-0">
                            <thead>
                                <tr>
                                    <th class="border-0">Dipendente</th>
                                    <th class="border-0 text-center" style="width: 70px;">
                                        <small>
                                            <asp:Label ID="OreSett" Text="Ore" runat="server" /></small>
                                    </th>
                                    <th class="border-0 text-center" style="width: 70px;">
                                        <small>
                                            <asp:Label ID="OreEst" Text="Ore esterne" runat="server" Visible="false" /></small>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="RepSingolo" runat="server" DataSourceID="TabellaDipendente">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="align-middle border-0">
                                                <strong><%# Eval("Nome") %> <%# Eval("Cognome") %></strong>
                                                <asp:HiddenField ID="HiddenDipendente" runat="server" Value='<%# Eval("ID") %>' />
                                            </td>
                                            <td class="border-0 text-center">
                                                <asp:UpdatePanel ID="upOreInterne" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                    <ContentTemplate>
                                                        <asp:TextBox runat="server" ID="OreInterne"
                                                            Text='<%# Eval("OreInterne","{0:0.##}") %>'
                                                            TextMode="Number"
                                                            CssClass="form-control form-control-sm text-center d-inline-block ore-input ore-interne"
                                                            Style="width: 70px;"
                                                            OnTextChanged="InputOre_TextChanged" AutoPostBack="true" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td class="border-0 text-center">
                                                <asp:UpdatePanel ID="upOreEsterne" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" Visible="false">
                                                    <ContentTemplate>
                                                        <asp:TextBox runat="server" ID="OreEsterne"
                                                            Text='<%# Eval("OreEsterne","{0:0.##}") %>'
                                                            TextMode="Number"
                                                            CssClass="form-control form-control-sm text-center d-inline-block ore-input ore-esterne"
                                                            Style="width: 70px;"
                                                            OnTextChanged="InputOre_TextChanged" AutoPostBack="true" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

        </section>
    </div>

    <div style="clear: both; display: flex; justify-content: flex-end; align-items: center; gap: 10px; padding-top: 15px; border-top: 1px solid #eee;">
        <br />




        <asp:RadioButton ID="ModSet" Text="Settimanale" GroupName="scarico" runat="server" />
        <asp:RadioButton ID="Modalita" Text="Mensile" GroupName="scarico" runat="server" />

        <asp:Button ID="SalProg" runat="server" Text="Invio" OnClick="btnSalvaTutto"
            CssClass="btn btn-success btn-custom-size shadow-sm" />

    </div>

    <div class="mt-3">
        <asp:Panel ID="pnlEsito" runat="server" Visible="false"
            CssClass="alert border-0 shadow-sm"
            Style="background-color: #fff3cd; color: #856404; border-left: 5px solid #f0ad4e;"
            role="alert">
            <asp:Label ID="lblEsito" runat="server" />
        </asp:Panel>
    </div>

    <section class="DSCard-card p-4 mt-5">
        <asp:SqlDataSource ID="DSMatrix" runat="server"
            ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
            SelectCommandType="StoredProcedure" SelectCommand="Time_Sheet">
            <SelectParameters>
                <asp:Parameter Name="Mode" Type="String" />
                <asp:Parameter Name="AnchorDate" Type="DateTime" />
            </SelectParameters>
        </asp:SqlDataSource>

        <div style="display: block; width: 100%; margin-bottom: 20px;">
            <div class="table-responsive">
                <asp:GridView ID="ViewFake" runat="server" DataSourceID="DSMatrix"
                    AutoGenerateColumns="True"
                    CssClass="table table-sm align-middle w-100">
                    <HeaderStyle CssClass="table-dark" />
                </asp:GridView>
            </div>
        </div>
    </section>
    <div>
        <div style="clear: both; display: flex; justify-content: flex-end; align-items: center; gap: 10px; padding-top: 15px; border-top: 1px solid #eee;">
            <asp:DropDownList ID="Mode" runat="server" AutoPostBack="true"
                OnSelectedIndexChanged="ChangeFake" CssClass="form-select form-select-sm w-auto">
                <asp:ListItem Value="Assegnazione">Assegnazione</asp:ListItem>
                <asp:ListItem Value="Calendario">Calendario</asp:ListItem>

                <asp:ListItem Value="Progetti">Progetti</asp:ListItem>
            </asp:DropDownList>

            <div class="input-group input-group-sm w-auto" style="position: relative;">
                <asp:TextBox ID="TextBox1" runat="server" ReadOnly="true"
                    CssClass="form-control" placeholder="Mese" Style="width: 100px;" />
                <asp:LinkButton ID="SelectMonth" runat="server" OnClick="ApriCalendario"
                    CssClass="btn btn-outline-secondary">
             <i class="bi bi-calendar3"></i>
                </asp:LinkButton>

                <asp:Panel ID="Panel1" runat="server" Visible="false"
                    Style="position: absolute; z-index: 1000; background: white; border: 1px solid #ccc; bottom: 40px; right: 0; min-width: 250px;">
                    <asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="ChangeFake" />
                </asp:Panel>
            </div>
            <asp:Button ID="Export" runat="server" Text="Export Excel"
                OnClick="ExportExcel" CssClass="btn btn-success shadow-sm btn-custom-size" />
        </div>
    </div>
    <asp:HiddenField ID="hfSelectedDates" runat="server" />
</asp:Content>
