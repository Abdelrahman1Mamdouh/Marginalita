<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Marginalita.Dashboard" MasterPageFile="~/Site.Master" Title="DASHBOARD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <div class="dashboardV2">
            <section class="dashCards-grid d-flex flex-nowrap" style="height: 20%">
                <!-- Card 1 -->
                <div style="display: flex; flex-direction: column; width: 30%; justify-content: space-between; /*background-color: greenyellow*/" id="card">
                    <div class="dashCard">
                        <div class="dashCard-text">
                            <div class="dashCard-label">Budget Totale</div>
                            <div class="dashCard-value">

                                <asp:SqlDataSource ID="SqlDataSourceBudget" runat="server"
                                    ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
                                    SelectCommand="SELECT SUM(Budget) AS TotaleBudget FROM Progetto"></asp:SqlDataSource>
                                <asp:Repeater ID="rptTotale" runat="server" DataSourceID="SqlDataSourceBudget">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMRR" runat="server" Text='<%# Eval("TotaleBudget", "{0:C}") %>' />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="DSCard-change DSCard-up">
                                <%-- ↑--%>
                                <%--<asp:Label ID="lblMRRChange" runat="server" Text="8.4%" />
                        <span class="DSCard-muted">vs last month</span>--%>
                            </div>
                        </div>

                        <div class="dashCard-icon dash-blue">
                            <asp:Label ID="euroIcon" Text="&#x20AC;" runat="server" />
                        </div>
                    </div>

                    <!-- Card 2 -->
                    <div class="dashCard">
                        <div class="dashCard-text">

                            <asp:SqlDataSource ID="SqlDataSourceCosti" runat="server"
                                ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
                                SelectCommand="SELECT SUM(Costo) AS TotaleCosti FROM Fake"></asp:SqlDataSource>

                            <div class="dashCard-label">Costo Totale</div>
                            <div class="dashCard-value">
                                <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSourceCosti">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUsers" runat="server" Text='<%# Eval("TotaleCosti", "{0:C}") %>' />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="DSCard-change DSCard-up">
                                <%--↑--%>
                                <asp:Label ID="lblUsersChange" runat="server" Text="" />
                            </div>
                        </div>

                        <div class="dashCard-icon dash-purple">
                            <asp:Label ID="Label2" Text="&#128179;" runat="server" />
                        </div>
                    </div>

                    <!-- Card 3 -->
                    <div class="dashCard">
                        <div class="dashCard-text">
                            <div class="dashCard-label">Margine Totale</div>
                            <asp:SqlDataSource ID="SqlDataSourceMargini" runat="server"
                                ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
                                SelectCommand="SELECT AVG(CAST(Margine AS INT)) AS TotaleMargini FROM V_Margini"></asp:SqlDataSource>
                            <div class="dashCard-value">
                                <asp:Repeater ID="Repeater2" runat="server" DataSourceID="SqlDataSourceMargini">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGrowth" runat="server" Text='<%# Eval("TotaleMargini")+ "%" %>' />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="DSCard-change DSCard-up">
                                <%-- ↑--%>
                                <asp:Label ID="lblGrowthChange" runat="server" Text="" />
                                <%--<span class="DSCard-muted">vs last month</span>--%>
                            </div>
                        </div>

                        <div class="dashCard-icon dash-green">
                            <asp:Label ID="Label3" Text="&percnt;" runat="server" />
                        </div>
                    </div>
                </div>

                <!-- Grafico -->

                <div style="width: 35%; align-content: center; display: flex; justify-content: center;" class="dashCard">
                    <div class="dashCard-label">Chart Margine</div>
                    <!-- Chart -->
                    <asp:SqlDataSource runat="server"
                        ID="ChartMARGINE"
                        ConnectionString="Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True"
                        ProviderName="System.Data.SqlClient"
                        SelectCommand="
                    SELECT 'Costi' AS Label,AVG(Round((Budget - Residuo)*100 / Budget,0)) AS Value
                    FROM Progetto
                   
                    UNION ALL
                    SELECT 'Margine' AS Label,AVG(Round(Residuo * 100 / Budget,0)) AS Value
                    FROM Progetto;"></asp:SqlDataSource>


                    <asp:Chart ID="Chart1"
                        runat="server"
                        DataSourceID="ChartMARGINE"
                        CssClass="w-auto justify-content-center"
                        OnDataBound="Chart1_DataBound">
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

                <!-- Card 4 -->
                <div style="width: 35%; /*background-color: mediumvioletred*/">
                    <div class="dashCard">
                        <div class="dashCard-text">
                            <div class="dashCard-label">Report</div>
                            <asp:SqlDataSource
                                ID="SqlScadenze" runat="server"
                                ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True"
                                SelectCommand='SELECT 
                                   P.ID, 
                                   P.Nome, 
                                   P.Budget, 
                                   P.Fine,
                                   P.GiorniRimanenti,
                                   CAST(V.Margine AS INT) AS MargineIntero
                                   FROM Progetto AS P
                                   JOIN V_Margini AS V ON V.ID = P.ID
                                   WHERE P.Fine 
                                   BETWEEN CAST(GETDATE() AS DATE) 
                                   AND DATEADD(DAY, 10, CAST(GETDATE() AS DATE))
                                   OR V.Margine&lt;30'></asp:SqlDataSource>
                            <asp:GridView ID="GridView2" runat="server"
                                DataSourceID="SqlScadenze"
                                AutoGenerateColumns="False"
                                CssClass="table table-striped w-100 text-center">
                                <Columns>
                                    <asp:BoundField DataField="Nome" HeaderText="Progetto" />
                                    <asp:BoundField DataField="GiorniRimanenti" HeaderText="GiorniRimansti" />
                                    <asp:BoundField DataField="MargineIntero" HeaderText="Margine" />
                                    <%--<asp:BoundField DataField="Fine" HeaderText="Data Scadenza" DataFormatString="{0:dd/MM/yyyy}" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </section>
        </div>
        <section class="mt-5">
            <div style="display: flex; justify-content: space-between;">
                <asp:Label ID="Label1"
                    Text="Dashboard Progetti"
                    runat="server" />

                <asp:LinkButton ID="LinkButton1"
                    Text="+ Aggiungi"
                    runat="server"
                    CssClass="btn btn-dark"
                    OnClick="Aggiungi_Progetti">

                </asp:LinkButton>
            </div>


        </section>

        <section class="mt-3" style="width: 100%">
            <div class="border rounded-3 shadow-sm overflow-hidden">

                <asp:SqlDataSource ID="SqlDGS" runat="server"
                    ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
                    SelectCommand="SELECT P.ID, 
                                            P.Nome, 
                                             P.Budget, 
                                             P.Descrizione, 
                                             CAST(V.Margine AS INT) AS MargineIntero
                                             FROM Progetto AS P
                                             JOIN V_Margini AS V ON V.ID = P.ID"></asp:SqlDataSource>

                <asp:GridView ID="GridView1" runat="server"
                    DataSourceID="SqlDGS"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped w-100 text-center">

                    <HeaderStyle CssClass="table-dark" />

                    <Columns>



                        <asp:BoundField DataField="Nome" HeaderText="Nome" />
                        <asp:BoundField DataField="Budget" HeaderText="Budget" />
                        <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" />

                        <asp:TemplateField HeaderText="Margini">
                            <ItemTemplate>
                                <div class="progress" role="progressbar" style="height: 20px;">
                                    <div class="progress-bar <%# 
                                             Convert.ToInt32(Eval("MargineIntero")) <= 65  ? "bg-danger" : 
                                             Convert.ToInt32(Eval("MargineIntero")) <= 70 ? "bg-warning" : "bg-success"%>"
                                        style='<%# "width:" + Convert.ToInt32(Eval("MargineIntero")) + "%;" %>'>
                                        <%# Eval("MargineIntero") %>%
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Dettagli">
                            <ItemTemplate>

                                <asp:LinkButton ID="btnVisualizza"
                                    runat="server"
                                    CssClass="btn btn-outline-primary"
                                    OnClick="btnVisualizza_Click"
                                    CommandArgument='<%# Eval("ID") %>'
                                    PostBackUrl='<%# "dettagliProgetto.aspx?id=" + Eval("ID") %>'> 
                                             <i class="bi bi-eye"></i> Visualizza
                                </asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </section>
    </div>
</asp:Content>
