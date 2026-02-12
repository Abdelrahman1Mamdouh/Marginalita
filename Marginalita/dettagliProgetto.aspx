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
        O.Creata AS Creata,
        P.Durata AS Durata
        FROM Progetto AS P
        LEFT JOIN Original AS O ON O.Progetto = P.ID
        LEFT JOIN Contratto AS C ON C.ID = P.Margine
        WHERE P.ID = @ID">
        <SelectParameters>
            <%-- Name=@ID nella query, QueryStringField='id' perché l'URL è ?id=... --%>
            <asp:QueryStringParameter Name="ID" QueryStringField="id" Type="Int32" />
        </SelectParameters>

    </asp:SqlDataSource>

    <asp:SqlDataSource
        runat="server" ID="ChartMARGINE"
        ConnectionString="Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True" ProviderName="System.Data.SqlClient"
        SelectCommand=" SELECT V.Label, V.Valore
                        FROM Progetto 
                        CROSS APPLY (VALUES 
                       ('Margine',  CAST(ISNULL(Margine, 0) AS INT)),
                       ('Restante', 100 - CAST(ISNULL(Margine, 0) AS INT))) V(Label, Valore)
                       WHERE ID = @ID;">

        <SelectParameters>
            <%-- Name=@ID nella query, QueryStringField='id' perché l'URL è ?id=... --%>
            <asp:QueryStringParameter Name="ID" QueryStringField="id" Type="Int32" />
        </SelectParameters>

    </asp:SqlDataSource>

    <div>
        <asp:FormView ID="FV" DataSourceID="PROG" runat="server" RenderOuterTable="false">
            <ItemTemplate>
                <asp:Label runat="server" Text="Nome del Progetto" CssClass="fw-semibold" />
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
                    <!-- Card 1 -->
                    <div class="DSCard-card">
                        <div class="DSCard-text">
                            <asp:Label ID="lblBudget" runat="server" Text="Budget" CssClass="DSCard-label" />
                            <div class="DSCard-value">
                                <asp:Label ID="lblMRR" runat="server" Text='<%# Eval("Budget") %>' />
                            </div>
                        </div>
                        <asp:Label Text="$" runat="server" ID="txtMRR" CssClass="DSCard-icon DSCard-green" />
                    </div>

                    <!-- Card 2 -->
                    <div class="DSCard-card">
                        <div class="DSCard-text">
                            <asp:Label ID="lblStartDate" runat="server" Text="Data Creazione" CssClass="DSCard-label" />
                            <div class="DSCard-value">
                                <asp:Label ID="lblUsers" runat="server" Text='<%# Eval("Creata") %>' />
                            </div>
                        </div>
                        <asp:Label Text="📅" runat="server" ID="txtStartDate" CssClass="DSCard-icon DSCard-pastalblue" />
                    </div>

                    <!-- Card 3 -->
                    <div class="DSCard-card">
                        <div class="DSCard-text">
                            <asp:Label ID="lblEndDate" runat="server" Text="Durata" CssClass="DSCard-label" />
                            <div class="DSCard-value">
                                <asp:Label ID="lblGrowth" runat="server" Text='<%# Eval("Durata") %>' />
                            </div>
                        </div>
                        <asp:Label Text="📆" runat="server" ID="txtEndDate" CssClass="DSCard-icon DSCard-orange" />
                    </div>
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
                    <!-- TOT ORE-->
                    <div class="DSCard-card">
                        <div class="DSCard-text">
                            <asp:Label ID="Label4" runat="server" Text="Total Hours" CssClass="DSCard-label" />
                            <div class="DSCard-value">
                                <asp:Label ID="lblHoursDone3" runat="server" Text="INSERIRE DA QUERY" CssClass="kpi3-big2" />
                            </div>
                        </div>
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
                </section>
                <section class="DSCard-grid">
                    <!-- CHART MARGINE -->
                    <div class="DSCard-card">
                        <div class="DSCard-text">

                            <asp:Label runat="server" Text="Margine" CssClass="DSCard-label" />
                            <div class="DSCard-value">
                                <asp:Label runat="server" Text='<%# Eval("ContrattoMargine") %>' />
                            </div>

                            <div class="DSCard-value">
                                <asp:Chart ID="Chart1" runat="server" DataSourceID="ChartMARGINE">
                                    <Series>
                                        <asp:Series Name="Series1"
                                            ChartType="Doughnut"
                                            XValueMember="Label"
                                            YValueMembers="Valore"
                                            IsValueShownAsLabel="true"
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
