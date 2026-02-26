<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Anagrafiche.aspx.cs" Inherits="Marginalita.Anagrafiche" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section>
            <h1>Anagrafiche</h1>
        </section>
        <asp:Panel ID="PAnagrafica" class="row-cols-sm-auto gridd" runat="server">
            <div id="ViewProgetti" class="col-33" runat="server">
                <h2>Progetti</h2>
                <asp:SqlDataSource ID="DProgetti" runat="server"
                    ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
                    SelectCommand="SELECT ID, Nome, Budget, Descrizione, Inizio, Fine, Margine, Societa FROM VProg"></asp:SqlDataSource>
                <asp:GridView ID="GridProgetti" DataSourceID="DProgetti" runat="server" AutoGenerateColumns="False" CssClass="table w-100 text-center">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="UpPro" CssClass="w-auto" runat="server" Text="📝" OnClick="UpPro_Click" CommandArgument='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Nome">
                            <ItemTemplate>
                                <asp:Label ID="PLNome" runat="server" Text='<%# Eval("Nome") %>' CssClass="w-75" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Budget">
                            <ItemTemplate>
                                <asp:Label ID="PLBudget" runat="server" Text='<%# Eval("Budget") %>' CssClass="w-50" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Descrizione">
                            <ItemTemplate>
                                <asp:Label ID="PLDescrizione" runat="server" Text='<%# Eval("Descrizione") %>' CssClass="w-100" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Margine">
                            <ItemTemplate>
                                <asp:Label ID="PLMargine" runat="server" Text='<%# Eval("Margine") %>' CssClass="w-50" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Societa">
                            <ItemTemplate>
                                <asp:Label ID="PLSocieta" runat="server" Text='<%# Eval("Societa") %>' CssClass="w-50" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button ID="NewProg" runat="server" Text="+ New" OnClick="NewProgetto" CssClass="btn btn-dark mt-2 btn-custom-size" />
            </div>
            <div id="ViewSocieta" class="col-33 " runat="server">
                <h2>Societa</h2>
                <asp:SqlDataSource ID="DSocieta" runat="server"
                    ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
                    SelectCommand="SELECT ID, Intestazione, Email FROM Societa"
                    DeleteCommand="DELETE FROM Societa WHERE ID = @ID"></asp:SqlDataSource>
                <asp:GridView ID="GridSocieta" DataSourceID="DSocieta" runat="server" AutoGenerateColumns="False" CssClass="table w-100 text-center">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="UpSoc" runat="server" Text="📝" OnClick="UpSocieta_Click" CommandArgument='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Intestazione">
                            <ItemTemplate>
                                <asp:Label ID="SLIntestazione" runat="server" Text='<%# Eval("Intestazione") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <asp:Label ID="SLEmail" runat="server" Text='<%# Eval("Email") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button ID="NewSoc" runat="server" Text="+ New" OnClick="NewSocieta" CssClass="btn btn-dark mt-2 btn-custom-size" />
            </div>
            <div id="ViewDipendenti" class="col-33" runat="server">
                <h2>Dipendenti</h2>
                <asp:SqlDataSource ID="DDipendenti" runat="server"
                    ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
                    SelectCommand="SELECT ID, Nome, Cognome, CostoOrario FROM Dipendente"
                    DeleteCommand="DELETE FROM Dipendente WHERE ID = @ID"></asp:SqlDataSource>
                <asp:GridView ID="GridDipendenti" DataSourceID="DDipendenti" runat="server" AutoGenerateColumns="False" CssClass="table w-100 text-center">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="UpDip" runat="server" Text="📝" OnClick="UpDipendente_Click" CommandArgument='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Nome">
                            <ItemTemplate>
                                <asp:Label ID="DLNome" runat="server" Text='<%# Eval("Nome") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Cognome">
                            <ItemTemplate>
                                <asp:Label ID="DLCognome" runat="server" Text='<%# Eval("Cognome") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="CostoOrario">
                            <ItemTemplate>
                                <asp:Label ID="DLCostoOrario" runat="server" Text='<%# Eval("CostoOrario") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button ID="NewDip" runat="server" Text="+ New" OnClick="NewDipendente" CssClass="btn btn-dark mt-2 btn-custom-size" />
            </div>
        </asp:Panel>
    </main>
</asp:Content>
