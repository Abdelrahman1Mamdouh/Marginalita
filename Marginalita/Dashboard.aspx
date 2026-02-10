
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Marginalita.Dashboard" MasterPageFile="~/Site.Master" Title="DASHBOARD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <section class="DSCard-grid">
            <!-- Card 1 -->
            <div class="DSCard-card">
                <div class="DSCard-text">
                    <div class="DSCard-label">Budget Totale</div>
                    <div class="DSCard-value">
                        <asp:Label ID="lblMRR" runat="server" Text="" />
                    </div>
                    <div class="DSCard-change DSCard-up">
                        ↑
                <asp:Label ID="lblMRRChange" runat="server" Text="8.4%" />
                        <span class="DSCard-muted">vs last month</span>
                    </div>
                </div>

                <div class="DSCard-icon DSCard-blue">
                    $
                </div>
            </div>

            <!-- Card 2 -->
            <div class="DSCard-card">
                <div class="DSCard-text">
                    <div class="DSCard-label">Costo Totale</div>
                    <div class="DSCard-value">
                        <asp:Label ID="lblUsers" runat="server" Text="" />
                    </div>
                    <div class="DSCard-change DSCard-up">
                        ↑
                <asp:Label ID="lblUsersChange" runat="server" Text="12.3%" />
                        <span class="DSCard-muted">vs last month</span>
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
                    <div class="DSCard-value">
                        <asp:Label ID="lblGrowth" runat="server" Text="" />
                    </div>
                    <div class="DSCard-change DSCard-up">
                        ↑
                <asp:Label ID="lblGrowthChange" runat="server" Text="5.2%" />
                        <span class="DSCard-muted">vs last month</span>
                    </div>
                </div>

                <div class="DSCard-icon DSCard-green">
                    ↗
                </div>
            </div>

            <!-- Card 4 -->
            <div class="DSCard-card">
                <div class="DSCard-text">
                    <div class="DSCard-label">Churn Rate</div>
                    <div class="DSCard-value">
                        <asp:Label ID="lblChurn" runat="server" Text="2.8%" />
                    </div>
                    <div class="DSCard-change DSCard-down">
                        ↓
                <asp:Label ID="lblChurnChange" runat="server" Text="0.5%" />
                        <span class="DSCard-muted">vs last month</span>
                    </div>
                </div>

                <div class="DSCard-icon DSCard-red">
                    👤
                 
                    <br />
                    <br />
                </div>
            </div>
        </section>
        
            <section class="mt-5">
                <div style="display:flex;justify-content:space-between;">
                    <asp:Label ID="Label1"
                        Text="Dashboard Progetti"
                        runat="server" />

                    <asp:LinkButton ID="LinkButton1"
                        Text="+ Aggiungi"
                        runat="server"
                        CssClass="btn btn-dark" 
                       >

                    </asp:LinkButton>
                </div>

             
            </section>

        <section class="mt-3" style="width: 100%">
            <div class="border rounded-3 shadow-sm overflow-hidden">

                <asp:SqlDataSource ID="SqlDGS" runat="server"
                    ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True"
                    SelectCommand="SELECT * FROM V_Dash">
                </asp:SqlDataSource>

                <asp:GridView ID="GridView1" runat="server"
                    DataSourceID="SqlDGS"
                    AutoGenerateColumns="True"
                    CssClass="table table-striped w-100 text-center"
                   
                    >

                    <HeaderStyle CssClass="table-dark" />

                    <Columns>



                        <%--<asp:BoundField DataField="Nome" HeaderText="Nome" />
                        <asp:BoundField DataField="Budget" HeaderText="Cognome" />
                        <asp:BoundField DataField="CostoOrario" HeaderText="Costo Orario" />--%>

                        <asp:TemplateField HeaderText="Margini">
                            <ItemTemplate>
                                <div class="progress" role="progressbar" style="height: 20px;">
                                    <div class="progress-bar <%# 
                                             Convert.ToInt32(Eval("Budget")) > 2000 ? "bg-danger" : 
                                             Convert.ToInt32(Eval("Budget")) > 1500 ? "bg-warning" : "bg-success"%>"
                                        style='<%# "width: " + Eval("Budget") + "%;" %>'>
                                        <%# Eval("Budget") %>%
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


    </body>
</asp:Content>
