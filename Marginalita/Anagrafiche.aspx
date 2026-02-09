
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
                    SelectCommand="SELECT ID, Nome, Budget, Durata, Descrizione, Residuo, Margine, Societa FROM Progetto"
                    DeleteCommand="DELETE FROM Progetto WHERE ID = @ID"></asp:SqlDataSource>
                <asp:ListView ID="LProgetti" DataSourceID="DProgetti" DataKeyName="IDProgetto" runat="server">



                    <ItemTemplate>
                        <asp:Table runat="server">

                            <asp:TableRow ID="DPro" HorizontalAlign="Center" VerticalAlign="Middle" runat="server">
                                <asp:TableCell Width="10" runat="server">
                                    <asp:Label ID="PLID" runat="Server" Text='<%#Eval("ID") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="40" runat="server">
                                    <asp:Label ID="PLNome" runat="Server" Text='<%#Eval("Nome") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="40" runat="server">
                                    <asp:Label ID="PLBudget" runat="Server" Text='<%#Eval("Budget") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="40" runat="server">
                                    <asp:Label ID="PLDurata" runat="Server" Text='<%#Eval("Durata") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="80" unat="server">
                                    <asp:Label ID="PLDescrizione" runat="Server" Text='<%#Eval("Descrizione") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="40" runat="server">
                                    <asp:Label ID="PLResiduo" runat="Server" Text='<%#Eval("Residuo") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="40" runat="server">
                                    <asp:Label ID="PLMargine" runat="Server" Text='<%#Eval("Margine") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="40" runat="server">
                                    <asp:Label ID="PLSocieta" runat="Server" Text='<%#Eval("Societa") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="10" runat="server">
                                    <asp:Button ID="UpPro" runat="server" Text="📝" OnClick="NewProgetto" CommandArgument='<%#Eval("ID") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="10" runat="server">
                                    <asp:Button ID="DelPro" runat="server" Text='<%# "\uD83D\uDDD1" %>' CommandName="DeleteProgetto" CommandArgument='<%#Eval("ID") %>' />
                                </asp:TableCell>
                            </asp:TableRow>

                        </asp:Table>

                    </ItemTemplate>


                </asp:ListView>

                <asp:Button ID="NewProg" runat="server" Text="New" OnClick="NewProgetto" />


            </div>

            <div id="ViewSocieta" class="col-33 " runat="server">
                <h2>Societa</h2>
                <asp:SqlDataSource ID="DSocieta" runat="server"
                    ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
                    SelectCommand="SELECT ID, Intestazione, Email FROM Societa"
                    DeleteCommand="DELETE FROM Societa WHERE ID = @ID"></asp:SqlDataSource>
                <asp:ListView ID="LSocieta" DataSourceID="DSocieta" DataKeyName="IDSocieta" runat="server">

                    <ItemTemplate>
                        <asp:Table ID="Stable" runat="server">

                            <asp:TableRow runat="server">
                                <asp:TableCell Width="10" runat="server">
                                    <asp:Label ID="SLID" runat="Server" Text='<%#Eval("ID") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="80" runat="server">
                                    <asp:Label ID="SLIntestazione" runat="Server" Text='<%#Eval("Intestazione") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="10" runat="server">
                                    <asp:Button ID="UpSoc" runat="server" Text="📝" OnClick="NewSocieta" CommandArgument='<%#Eval("ID") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="10" runat="server">
                                    <asp:Button ID="DelSoc" runat="server" Text='<%# "\uD83D\uDDD1" %>' CommandName="DeleteSocieta" CommandArgument='<%#Eval("ID") %>' />
                                </asp:TableCell>


                            </asp:TableRow>
                        </asp:Table>

                    </ItemTemplate>

                </asp:ListView>

                <asp:Button ID="NewSoc" runat="server" Text="New" OnClick="NewSocieta" />

            </div>


            <div id="ViewDipendenti" class="col-33" runat="server">
                <h2>Dipendenti</h2>
                <asp:SqlDataSource ID="DDipendenti" runat="server"
                    ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
                    SelectCommand="SELECT ID, NOme, Cognome, CostoOrario FROM Dipendente"
                    DeleteCommand="DELETE FROM Dipendente WHERE ID = @ID"></asp:SqlDataSource>

                <asp:ListView ID="LDipendenti" DataSourceID="DDipendenti" DataKeyName="IDDipendenti" runat="server">

                    <ItemTemplate>
                        <asp:Table ID="Dtable" runat="server">

                            <asp:TableRow runat="server">
                                <asp:TableCell Width="10" runat="server">
                                    <asp:Label ID="DLID" runat="Server" Text='<%#Eval("ID") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="40" runat="server">
                                    <asp:Label ID="DLNome" runat="Server" Text='<%#Eval("Nome") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="40" runat="server">
                                    <asp:Label ID="DLCognome" runat="Server" Text='<%#Eval("Cognome") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="40" runat="server">
                                    <asp:Label ID="DLCostoOrario" runat="Server" Text='<%#Eval("CostoOrario") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="10" runat="server">
                                    <asp:Button ID="UpDip" runat="server" Text="📝" OnClick="NewDipendente" CommandArgument='<%#Eval("ID") %>' />
                                </asp:TableCell>
                                <asp:TableCell Width="10" runat="server">
                                    <asp:Button ID="DelDip" runat="server" Text='<%# "\uD83D\uDDD1" %>' CommandName="DeleteDipendente" CommandArgument='<%#Eval("ID") %>' />
                                </asp:TableCell>

                            </asp:TableRow>
                        </asp:Table>

                    </ItemTemplate>

                </asp:ListView>

                <asp:Button ID="NewDip" runat="server" Text="New" OnClick="NewDipendente" />

            </div>
        </asp:Panel>
    </main>

</asp:Content>