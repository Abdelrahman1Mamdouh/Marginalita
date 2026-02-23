<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dettagliProgetto.aspx.cs" Inherits="Marginalita.dettagliProgetto" MasterPageFile="~/Site.Master" Title="DETTAGLI PROGETTO" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:SqlDataSource
        runat="server" ID="PROG"
        ConnectionString="Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True" ProviderName="System.Data.SqlClient"
        SelectCommand="
        SELECT P.ID AS ID,
        P.Nome As Nome,
        P.Budget As Budget,
        P.Descrizione As Descrizione,
        P.Margine AS ProgettoMargine,
        P.residuo AS Residuo,
        C.Margine AS ContrattoMargine,
        F.Creata AS Creata,
        P.Inizio As Inizio,
        P.Fine AS Fine,
        P.Durata AS Scadenza
        FROM Progetto AS P
        LEFT JOIN FAKE AS F ON F.Progetto = P.ID
        LEFT JOIN Contratto AS C ON C.ID = P.Margine
        WHERE P.ID = @ID">
        <SelectParameters>
            <asp:QueryStringParameter Name="ID" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>


    <asp:SqlDataSource runat="server" ID="TotOre"
        ConnectionString="Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True" ProviderName="System.Data.SqlClient"
        SelectCommand="
                        SELECT CAST(SUM(X.OreDip) AS INT) AS TotOreProgetto
                        FROM (
                            SELECT F.Dipendente, SUM(F.Costo) / D.CostoOrario AS OreDip
                            FROM Fake F
                            JOIN Dipendente D ON D.ID = F.Dipendente
                            WHERE F.Progetto = @ProgettoId
                            GROUP BY F.Dipendente, D.CostoOrario
                        ) X;">

        <SelectParameters>
            <asp:QueryStringParameter Name="ProgettoId" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>


    <asp:SqlDataSource runat="server"
        ID="ChartMARGINE"
        ConnectionString="Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True"
        ProviderName="System.Data.SqlClient"
        SelectCommand="
                        SELECT 'Residuo' AS Label, Round(Residuo * 100 / Budget,0) AS Value
                        FROM Progetto
                        WHERE ID = @ID
                        UNION ALL
                        SELECT 'Margine' AS Label, Round((Budget - Residuo)*100 / Budget,0) AS Value
                        FROM Progetto
                        WHERE ID = @ID;">
        <SelectParameters>
            <asp:QueryStringParameter Name="ID" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>

    <div>
        <asp:FormView ID="FV" DataSourceID="PROG" runat="server" RenderOuterTable="false">
            <ItemTemplate>
                <section class="DSCard-grid">
                    <!-- Nome del App -->
                    <div class="DSCard-desc">
                        <div>
                            <asp:Label ID="lblNomeProgetto" runat="server" CssClass="desc" Text='<%# Eval("Nome") %>' />
                        </div>
                    </div>
                </section>

                <br />

                <asp:Label ID="lblDettagli" runat="server" Text="Dettagli Progetto" CssClass="fw-semibold" />

                <section class="DSCard-grid">
                    <!-- Budget -->
                    <div class="DSCard-card">
                        <div class="DSCard-text">
                            <asp:Label ID="lblBudget" runat="server" Text="Bilancio Preventivo" CssClass="DSCard-label" />
                            <div class="DSCard-value">
                                <asp:Label ID="lblMRR" runat="server" Text='<%# Eval("Budget") %>' />
                            </div>
                        </div>
                        <asp:Label Text="$" runat="server" ID="txtMRR" CssClass="DSCard-icon DSCard-green" />
                    </div>
                    <!-- TOT COSTO -->
                    <div class="DSCard-card">
                        <div class="DSCard-text">
                            <asp:Label runat="server" Text="Residuo" CssClass="DSCard-label" />
                            <div class="DSCard-value">
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Residuo") %>' />
                            </div>
                        </div>
                    </div>

                    <asp:FormView ID="FV" DataSourceID="TotOre" runat="server" RenderOuterTable="false">
                        <ItemTemplate>
                            <!-- TOT ORE-->
                            <div class="DSCard-card">
                                <div class="DSCard-text">
                                    <asp:Label ID="Label4" runat="server" Text="Ore di lavoro totali" CssClass="DSCard-label" />
                                    <div class="DSCard-value">
                                        <asp:Label ID="lblHoursDone3" runat="server" Text='<%# Eval("TotOreProgetto") %>' CssClass="kpi3-big2" />
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:FormView>
                </section>

                <br />

                <asp:Label ID="lblDescrizione" runat="server" Text="Descrizione" CssClass="fw-semibold" />
                <section class="DSCard-grid">
                    <div class="DSCard-card">
                        <div class="DSCard-text">
                            <asp:Label ID="lblDes" runat="server"
                                Text='<%# Eval("Descrizione") %>' />
                        </div>
                    </div>
                </section>
                <section class="DSCard-grid">

                    <!-- Data Creazione -->
                    <div class="DSCard-card">
                        <div class="DSCard-text">
                            <asp:Label ID="lblStartDate" runat="server" Text="Data Creazione" CssClass="DSCard-label" />
                            <div class="DSCard-value">
                                <asp:Label ID="lblUsers" runat="server" Text='<%# Eval("Inizio","{0:dd/MM/yyyy}") %>' />
                            </div>
                        </div>
                        <asp:Label Text="&#128197;" runat="server" ID="txtStartDate" CssClass="DSCard-icon DSCard-pastalblue" />
                    </div>

                    <!-- Scadenza -->
                    <div class="DSCard-card">
                        <div class="DSCard-text">
                            <asp:Label ID="lblEndDate" runat="server" Text="Scadenza" CssClass="DSCard-label" />
                            <div class="DSCard-value">
                                <asp:Label ID="lblGrowth" runat="server" Text='<%# Eval("Fine","{0:dd/MM/yyyy}") %>' />
                            </div>
                        </div>
                        <asp:Label Text="&#128198;" runat="server" ID="txtEndDate" CssClass="DSCard-icon DSCard-orange" />
                    </div>

                    <!-- Durata -->
                    <div class="DSCard-card">
                        <div class="DSCard-text">
                            <asp:Label runat="server" Text="Durata (Giorni)" CssClass="DSCard-label" />
                            <div class="DSCard-value">
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Scadenza") %>' />
                            </div>
                        </div>
                    </div>
                </section>
                <section class="DSCard-grid">

                    <!-- CHART MARGINE -->
                    <div class="DSCard-card">
                        <div class="DSCard-text">

                            <asp:Label runat="server" Text="Margine" CssClass="DSCard-label" />
                            <div class="DSCard-value">
                                <asp:Label runat="server" Text='<%# Eval("ContrattoMargine") + "%" %>' />
                            </div>

                            <div class="DSCard-value">
                                <asp:Chart ID="Chart1" runat="server" DataSourceID="ChartMARGINE">
                                    <Series>
                                        <asp:Series Name="Series1"
                                            ChartType="Doughnut"
                                            XValueMember="Label"
                                            YValueMembers="Value"
                                            IsValueShownAsLabel="false"
                                            LegendText="#VALX (#PERCENT{P0})"
                                            Label="#VALY"
                                            BorderWidth="0" />
                                    </Series>

                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1">
                                            <Area3DStyle Enable3D="true" />
                                        </asp:ChartArea>
                                    </ChartAreas>

                                    <Legends>
                                        <asp:Legend Enabled="true" />
                                    </Legends>
                                </asp:Chart>
                            </div>
                        </div>
                    </div>
                </section>
            </ItemTemplate>
        </asp:FormView>
    </div>
</asp:Content>
