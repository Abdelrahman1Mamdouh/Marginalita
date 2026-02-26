<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InputDati.aspx.cs" Inherits="Marginalita.InputDati" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <section class="mb-4">
            <h1 class="fw-bold">Inserimento Dati</h1>
            <hr />
        </section>
        <asp:HiddenField ID="HID" runat="server" />
        <asp:HiddenField ID="ProgFin" runat="server" Value="0" />
        <asp:SqlDataSource ID="DProgetti" runat="server" ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
            SelectCommand="SELECT Nome, Budget, Inizio, Fine, Descrizione, Residuo, Margine, Societa FROM Progetto WHERE Vedi=1"
            InsertCommand="INSERT INTO Progetto (Nome, Budget, Inizio, Fine, Descrizione, Societa,Margine, Residuo) VALUES (@Nome, @Budget, @Inizio, @Fine, @Descrizione, @Societa, @Margine, 0)"
            UpdateCommand="UPDATE Progetto SET Nome=@Nome, Budget=@Budget, Inizio=@Inizio, Fine=@Fine, Descrizione=@Descrizione WHERE ID=@ID"
            DeleteCommand="UPDATE Progetto SET Vedi = @Vedi WHERE ID = @ID">
            <InsertParameters>
                <asp:ControlParameter Name="Nome" ControlID="TNomePro" PropertyName="Text" />
                <asp:ControlParameter Name="Budget" ControlID="TBudget" PropertyName="Text" />
                <asp:ControlParameter Name="Inizio" ControlID="CDInizio" PropertyName="SelectedDate" />
                <asp:ControlParameter Name="Fine" ControlID="CDFine" PropertyName="SelectedDate" />
                <asp:ControlParameter Name="Descrizione" ControlID="TDescritione" PropertyName="Text" />
                <asp:ControlParameter Name="Societa" ControlID="DDLSocieta" PropertyName="SelectedValue" />
                <asp:ControlParameter Name="Margine" ControlID="DDLMargine" PropertyName="SelectedValue" />
            </InsertParameters>
            <UpdateParameters>
                <asp:ControlParameter Name="ID" ControlID="HID" PropertyName="Value" />
                <asp:ControlParameter Name="Nome" ControlID="TNomePro" PropertyName="Text" />
                <asp:ControlParameter Name="Budget" ControlID="TBudget" PropertyName="Text" />
                <asp:ControlParameter Name="Inizio" ControlID="CDInizio" PropertyName="SelectedDate" />
                <asp:ControlParameter Name="Fine" ControlID="CDFine" PropertyName="SelectedDate" />
                <asp:ControlParameter Name="Descrizione" ControlID="TDescritione" PropertyName="Text" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:ControlParameter Name="ID" ControlID="HID" PropertyName="Value" />
                <asp:ControlParameter Name="Vedi" ControlID="ProgFin" PropertyName="Value" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DSocieta" runat="server" ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
            SelectCommand="SELECT * FROM Societa WHERE Vedi=1"
            InsertCommand="INSERT INTO Societa (Intestazione, Email) VALUES (@Intestazione, @Email)"
            UpdateCommand="UPDATE Societa SET Intestazione=@Intestazione, Email=@Email WHERE ID=@ID"
            DeleteCommand="UPDATE Societa SET Vedi = @Vedi WHERE ID = @ID">
            <InsertParameters>
                <asp:ControlParameter Name="Intestazione" ControlID="TIntestazione" PropertyName="Text" />
                <asp:ControlParameter Name="Email" ControlID="TEmail" PropertyName="Text" />
            </InsertParameters>
            <UpdateParameters>
                <asp:ControlParameter Name="ID" ControlID="HID" PropertyName="Value" />
                <asp:ControlParameter Name="Intestazione" ControlID="TIntestazione" PropertyName="Text" />
                <asp:ControlParameter Name="Email" ControlID="TEmail" PropertyName="Text" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:ControlParameter Name="ID" ControlID="HID" PropertyName="Value" />
                <asp:ControlParameter Name="Vedi" ControlID="ProgFin" PropertyName="Value" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DContratto" runat="server" ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
            SelectCommand="SELECT * FROM Contratto "></asp:SqlDataSource>
        <asp:SqlDataSource ID="DFake" runat="server" ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
            DeleteCommand="UPDATE Original SET Vedi = @Vedi WHERE Progetto = @ID">
            <DeleteParameters>
                <asp:ControlParameter Name="ID" ControlID="HID" PropertyName="Value" />
                <asp:ControlParameter Name="Vedi" ControlID="ProgFin" PropertyName="Value" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DDipendenti" runat="server" ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
            SelectCommand="SELECT ID, Nome, Cognome, CostoOrario FROM Dipendente WHERE Vedi=1"
            InsertCommand="INSERT INTO Dipendente (Nome, Cognome, CostoOrario) VALUES (@Nome, @Cognome, @CostoOrario)"
            UpdateCommand="UPDATE Dipendente SET Nome=@Nome, Cognome=@Cognome, CostoOrario=@CostoOrario WHERE ID=@ID"
            DeleteCommand="UPDATE Dipendente SET Vedi = @Vedi WHERE ID = @ID">
            <InsertParameters>
                <asp:ControlParameter Name="Nome" ControlID="TLNomeDip" PropertyName="Text" />
                <asp:ControlParameter Name="Cognome" ControlID="TCognome" PropertyName="Text" />
                <asp:ControlParameter Name="CostoOrario" ControlID="TCosto" PropertyName="Text" />
            </InsertParameters>
            <UpdateParameters>
                <asp:ControlParameter Name="ID" ControlID="HID" PropertyName="Value" />
                <asp:ControlParameter Name="Nome" ControlID="TLNomeDip" PropertyName="Text" />
                <asp:ControlParameter Name="Cognome" ControlID="TCognome" PropertyName="Text" />
                <asp:ControlParameter Name="CostoOrario" ControlID="TCosto" PropertyName="Text" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:ControlParameter Name="ID" ControlID="HID" PropertyName="Value" />
                <asp:ControlParameter Name="Vedi" ControlID="ProgFin" PropertyName="Value" />
            </DeleteParameters>
        </asp:SqlDataSource>

        <div id="ViewProgetti" runat="server" class="DSCard-card p-4 mb-4">
            <div class="DSCard-text w-100">
                <div class="DSCard-label mb-4" style="font-size: 18px; font-weight: 800; color: var(--text);">Progetto</div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="mb-3">
                            <asp:Label ID="LNomePro" Text="Nome progetto" runat="server" CssClass="DSCard-label" />
                            <asp:TextBox runat="server" ID="TNomePro" CssClass="form-control-styled" />
                        </div>
                        <div class="mb-3">
                            <asp:Label ID="LBudget" Text="Budget progetto" runat="server" CssClass="DSCard-label" />
                            <asp:TextBox runat="server" ID="TBudget" CssClass="form-control-styled" />
                        </div>
                        <div class="mb-3">
                            <label ID="DropDownSocieta" class="DSCard-label" runat="server">Società</label>
                            <asp:DropDownList ID="DDLSocieta" AutoPostBack="true" DataSourceID="DSocieta" DataTextField="Intestazione" DataValueField="ID" runat="server" CssClass="form-control-styled"></asp:DropDownList>
                        </div>
                        <div class="mb-3">
                            <label ID="DropDownMargine" class="DSCard-label" runat="server">Tipo Contratto</label>
                            <asp:DropDownList ID="DDLMargine" AutoPostBack="true" DataSourceID="DContratto" DataTextField="Tipo" DataValueField="ID" runat="server" CssClass="form-control-styled"></asp:DropDownList>
                        </div>
                        <div class="mb-3">
                            <asp:Label ID="LDescritione" Text="Descrizione progetto" runat="server" CssClass="DSCard-label" />
                            <asp:TextBox runat="server" ID="TDescritione" TextMode="MultiLine" Rows="3" CssClass="form-control-styled" />
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-6 text-center">
                                <asp:Label ID="LDInizio" Text="Inizio progetto" runat="server" CssClass="DSCard-label fw-bold d-block mb-2" />
                                <div class="calendar-box border rounded p-2 bg-white shadow-sm d-inline-block">
                                    <asp:Calendar ID="CDInizio" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-6 text-center">
                                <asp:Label ID="LDFine" Text="Fine progetto" runat="server" CssClass="DSCard-label fw-bold d-block mb-2" />
                                <div class="calendar-box border rounded p-2 bg-white shadow-sm d-inline-block">
                                    <asp:Calendar ID="CDFine" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="mt-2 p-3 text-end">
                        <asp:Button ID="SalProg" runat="server" Text="Salva" OnClick="SalProgetto"
                            CssClass="btn btn-success btn-custom-size shadow-sm" />
                        <asp:Button ID="ModProg" runat="server" Text="Modifica" OnClick="ModProgetto"
                            CssClass="btn btn-primary btn-custom-size shadow-sm" />
                        <asp:Button ID="EliProg" runat="server" Text="Elimina" OnClick="EliProgetto"
                            CssClass="btn btn-danger btn-custom-size shadow-sm" />
                        <asp:Button ID="AnnullaProg" runat="server" Text="Annulla" OnClick="AnnullaProgetto"
                            CssClass="btn btn-dark btn-custom-size shadow-sm" />
                    </div>
                </div>
            </div>
        </div>

        <div class="w-100">
            <div id="ViewSocieta" runat="server" class="DSCard-card p-4 h-100">
                <div class="DSCard-text w-100">
                    <div class="DSCard-label mb-4" style="font-size: 18px; font-weight: 800; color: var(--text);">Società</div>
                    <div class="mb-3">
                        <asp:Label ID="LIntestazione" Text="Intestazione società" runat="server" CssClass="DSCard-label" />
                        <asp:TextBox runat="server" ID="TIntestazione" CssClass="form-control-styled" />
                    </div>

                    <div class="mb-4">
                        <asp:Label ID="LEmail" Text="Email società" runat="server" CssClass="DSCard-label" />
                        <asp:TextBox runat="server" ID="TEmail" CssClass="form-control-styled" />
                    </div>
                    <div class="mt-2 p-3 text-end">
                        <asp:Button ID="SalSoc" runat="server" Text="Salva" OnClick="SalSocieta"
                            CssClass="btn btn-custom-size btn-success text-white fw-bold shadow-sm" />
                        <asp:Button ID="ModSoc" runat="server" Text="Modifica" OnClick="ModSocieta"
                            CssClass="btn btn-custom-size btn-primary shadow-sm" />
                        <asp:Button ID="EliSoc" runat="server" Text="Elimina" OnClick="EliSocieta"
                            CssClass="btn btn-custom-size btn-danger shadow-sm" />
                        <asp:Button ID="AnnullaSoc" runat="server" Text="Annulla" OnClick="AnnullaSocieta"
                            CssClass="btn btn-dark btn-custom-size shadow-sm" />
                    </div>
                </div>
            </div>
        </div>

        <div class="w-100">
            <div id="ViewDipendenti" runat="server" class="DSCard-card p-4 w-100 h-100">
                <div class="DSCard-text w-100">
                    <div class="DSCard-label mb-4" style="font-size: 18px; font-weight: 800; color: var(--text);">Dipendenti</div>
                    <div class="row">
                        <div class="col-6 mb-3">
                            <asp:Label ID="LNomeDip" Text="Nome" runat="server" CssClass="DSCard-label" />
                            <asp:TextBox runat="server" ID="TLNomeDip" CssClass="form-control-styled w-100" />
                        </div>
                        <div class="col-6 mb-3">
                            <asp:Label ID="LCognome" Text="Cognome" runat="server" CssClass="DSCard-label" />
                            <asp:TextBox runat="server" ID="TCognome" CssClass="form-control-styled w-100" />
                        </div>
                    </div>
                    <div class="mb-4 d-flex align-items-center gap-3">
                        <asp:Label ID="LCosto" Text="Costo orario (€)" runat="server" CssClass="DSCard-label" Style="min-width: 120px;" />
                        <asp:TextBox runat="server" ID="TCosto" CssClass="form-control-styled w-100" />
                    </div>
                    <div class="mt-2 p-3 text-end">
                        <asp:Button ID="ModDip" runat="server" Text="Modifica" OnClick="ModDipendenti"
                            CssClass="btn btn-custom-size btn-primary shadow-sm" />
                        <asp:Button ID="EliDip" runat="server" Text="Elimina" OnClick="EliDipendenti"
                            CssClass="btn btn-custom-size btn-danger shadow-sm" />
                        <asp:Button ID="SalDip" runat="server" Text="Salva" OnClick="SalDipendenti"
                            CssClass="btn btn-custom-size btn-success text-white fw-bold shadow-sm" />
                        <asp:Button ID="AnnullaDip" runat="server" Text="Annulla" OnClick="AnnullaDipendenti"
                            CssClass="btn btn-dark btn-custom-size shadow-sm" />
                    </div>
                </div>
            </div>
        </div>
    </main>
</asp:Content>