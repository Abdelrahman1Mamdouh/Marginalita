<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InputDati.aspx.cs" Inherits="Marginalita.InputDati" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section>
            <h1>Inserimento dati</h1>
        </section>
        <asp:HiddenField ID="HID" runat="server" />
        <asp:HiddenField ID="ProgFin" runat="server" Value="0" />

        <asp:SqlDataSource
            ID="DProgetti"
            runat="server"
            ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
            SelectCommand="SELECT Nome, Budget, Inizio, Fine, Descrizione, Residuo, Margine, Societa FROM Progetto WHERE Vedi=1"
            InsertCommand="INSERT INTO Progetto (Nome, Budget, Inizio, Fine, Descrizione, Societa,Margine, Residuo) VALUES (@Nome, @Budget, @Inizio, @Fine, @Descrizione, @Societa, @Margine, @Residuo)"
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
                <asp:ControlParameter Name="Residuo" ControlID="TBudget" PropertyName="Value" />
            </InsertParameters>
            <UpdateParameters>
                <asp:ControlParameter Name="ID" ControlID="HID" PropertyName="Value" />
                <asp:ControlParameter Name="Nome" ControlID="TNomePro" PropertyName="Text" />
                <asp:ControlParameter Name="Budget" ControlID="TBudget" PropertyName="Value" />
                <asp:ControlParameter Name="Inizio" ControlID="CDInizio" PropertyName="SelectedDate" />
                <asp:ControlParameter Name="Fine" ControlID="CDFine" PropertyName="SelectedDate" />
                <asp:ControlParameter Name="Descrizione" ControlID="TDescritione" PropertyName="Text" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:ControlParameter Name="ID" ControlID="HID" PropertyName="Value" />
                <asp:ControlParameter Name="Vedi" ControlID="ProgFin" PropertyName="Value" />
            </DeleteParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource
            ID="DSocieta"
            runat="server"
            ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
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

        <asp:SqlDataSource
            ID="DContratto"
            runat="server"
            ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
            SelectCommand="SELECT * FROM Contratto "></asp:SqlDataSource>

        <asp:SqlDataSource
            ID="DFake"
            runat="server"
            ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
            DeleteCommand="UPDATE Fake SET Vedi = @Vedi WHERE Progetto = @ID">
            <DeleteParameters>
                <asp:ControlParameter Name="ID" ControlID="HID" PropertyName="Value" />
                <asp:ControlParameter Name="Vedi" ControlID="ProgFin" PropertyName="Value" />
            </DeleteParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource
            ID="DDipendenti"
            runat="server"
            ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dgs.mdf;Integrated Security=True;TrustServerCertificate=True;"
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

        <div id="ViewProgetti" runat="server" style="display:flex;flex-direction:column">
            <h2>Progetti</h2>

            <asp:Label ID="LNomePro" Text="Nome progetto" runat="server" />
            <asp:TextBox runat="server" ID="TNomePro" />

            <asp:Label ID="LBudget" Text="Budget progetto" runat="server" />
            <asp:TextBox runat="server" ID="TBudget" />

            <asp:Label ID="LDInizio" Text="Inizio progetto" runat="server" />
            <asp:Calendar ID="CDInizio" runat="server" />

            <asp:Label ID="LDFine" Text="FIne progetto" runat="server" />
            <asp:Calendar ID="CDFine" runat="server" />

            <asp:Label ID="LDescritione" TextMode="Multiline" Text="Descritione progetto" runat="server" />
            <asp:TextBox runat="server" ID="TDescritione" />

            <asp:DropDownList ID="DDLSocieta" AutoPostBack="true" DataSourceID="DSocieta" DataTextField="Intestazione" DataValueField="ID" runat="server"></asp:DropDownList>

            <asp:DropDownList ID="DDLMargine" AutoPostBack="true" DataSourceID="DContratto" DataTextField="Tipo" DataValueField="ID" runat="server"></asp:DropDownList>

            <asp:Button ID="ModProg" runat="server" Text="Modifica" OnClick="ModProgetto" />
            <asp:Button ID="SalProg" runat="server" Text="Salva" OnClick="SalProgetto" style="background-color:black;color:white;padding:5px 10px;margin-top:10px;margin-bottom:10px;border-radius:5px"/>
            <asp:Button ID="EliProg" runat="server" Text="Elimina" OnClick="EliProgetto" />
        </div>

        <div id="ViewSocieta" runat="server" style="display:flex;flex-direction:column">
            <h2>Societa</h2>

            <asp:Label ID="LIntestazione" Text="Intestazione societa" runat="server" />
            <asp:TextBox runat="server" ID="TIntestazione" />

            <asp:Label ID="LEmail" Text="Email societa" runat="server" />
            <asp:TextBox runat="server" ID="TEmail" />

            <asp:Button ID="ModSoc" runat="server" Text="Modifica" OnClick="ModSocieta" />
            <asp:Button ID="SalSoc" runat="server" Text="Salva" OnClick="SalSocieta" style="background-color:black;color:white;padding:5px 10px;margin-top:10px;margin-bottom:10px;border-radius:5px" />
            <asp:Button ID="EliSoc" runat="server" Text="Elimina" OnClick="EliSocieta" />
        </div>

        <div id="ViewDipendenti" runat="server" style="display:flex;flex-direction:column">

            <h2>Dipendenti</h2>

            <asp:Label ID="LNomeDip" Text="Nome" runat="server" />
            <asp:TextBox runat="server" ID="TLNomeDip" />

            <asp:Label ID="LCognome" Text="Cognome" runat="server" />
            <asp:TextBox runat="server" ID="TCognome" />

            <asp:Label ID="LCosto" Text="Costo orario" runat="server" />
            <asp:TextBox runat="server" ID="TCosto" />

            <asp:Button ID="ModDip" runat="server" Text="Modifica" OnClick="ModDipendenti" />
            <asp:Button ID="SalDip" runat="server" Text="Salva" OnClick="SalDipendenti" style="background-color:black;color:white;padding:5px 10px;margin-top:10px;margin-bottom:10px;border-radius:5px"/>
            <asp:Button ID="EliDip" runat="server" Text="Elimina" OnClick="EliDipendenti" />

        </div>
        <asp:Button ID="Ann" runat="server" Text="Annulla" OnClick="Annulla" style="border:2px solid red;padding:5px 10px;background-color:transparent; border-radius:5px;color:red"/>
    </main>

</asp:Content>
