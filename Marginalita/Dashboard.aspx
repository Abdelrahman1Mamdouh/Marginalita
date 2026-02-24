<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Marginalita.Dashboard" MasterPageFile="~/Site.Master" Title="DASHBOARD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <section class="DSCard-grid">
            <!-- Card 1 -->
            <div class="DSCard-card">
                <div class="DSCard-text">
                    <div class="DSCard-label">Budget Totale</div>
                    <div class="DSCard-value">

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

                <div class="DSCard-icon DSCard-blue">
                    €
                </div>
            </div>

            <!-- Card 2 -->
            <div class="DSCard-card">
                <div class="DSCard-text">

                    <asp:SqlDataSource ID="SqlDataSourceCosti" runat="server"
                         ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
                         SelectCommand="SELECT SUM(Costo) AS TotaleCosti FROM Fake">
                    </asp:SqlDataSource>

                    <div class="DSCard-label">Costo Totale</div>
                    <div class="DSCard-value">
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

                <div class="DSCard-icon DSCard-purple">
                    👤
                </div>
            </div>

            <!-- Card 3 -->
            <div class="DSCard-card">
                <div class="DSCard-text">
                    <div class="DSCard-label">Margine Totale</div>
                    <asp:SqlDataSource ID="SqlDataSourceMargini" runat="server"
                        ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
                        SelectCommand="SELECT AVG(Margine) AS TotaleMargini FROM Progetto"></asp:SqlDataSource>
                    <div class="DSCard-value">
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

                <div class="DSCard-icon DSCard-green">
                    ↗
                </div>
            </div>

            <!-- Card 4 -->
            <div class="DSCard-card">
                <div class="DSCard-text">
                    <div class="DSCard-label">Report</div>
                       <asp:SqlDataSource 
                           ID="SqlScadenze" runat="server"
                            ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True"
                            SelectCommand="SELECT ID, 
                                                 Nome, 
                                                 Budget, 
                                                 Fine
                                           FROM Progetto
                                           WHERE Fine >= CAST(GETDATE() AS DATE) 
                                           AND Fine <= DATEADD(day, 2, CAST(GETDATE() AS DATE))
                                           ORDER BY Fine ASC">
                       </asp:SqlDataSource> 
                    <asp:GridView ID="GridView2" runat="server"
                        DataSourceID="SqlScadenze"
                        AutoGenerateColumns="False"
                        CssClass="table table-striped w-100 text-center">
                        <Columns>
                            <asp:BoundField DataField="Nome" HeaderText="Progetto" />
                            <asp:BoundField DataField="ScadenzaCalcolata" HeaderText="Data Scadenza" DataFormatString="{0:dd/MM/yyyy}" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </section>

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
                                             JOIN V_Margini AS V ON V.ID = P.ID">

                </asp:SqlDataSource>

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
