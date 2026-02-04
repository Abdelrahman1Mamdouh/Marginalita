<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dettagliProgetto.aspx.cs" Inherits="Marginalita.dettagliProgetto" MasterPageFile="~/Site.Master" Title="DETTAGLI PROGETTO" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <section class="DSCard-grid">
            <!-- App -->
            <div class="DSCard-card">
                <div>
                    <asp:Image ID="LogoApp" runat="server" />
                </div>
            </div>
        </section>
        <br />
        <asp:Label ID="lblDettagli" runat="server" Text="Dettagli Progetto" />
        <section class="DSCard-grid">
            <!-- Card 1 -->
            <div class="DSCard-card">
                <div class="DSCard-text">
                    <div class="DSCard-label">Budget</div>
                    <div class="DSCard-value">
                        <asp:Label ID="lblMRR" runat="server" Text="$250,000" />
                    </div>
                </div>
                <div class="DSCard-icon DSCard-green">
                    $
                </div>
            </div>

            <!-- Card 2 -->
            <div class="DSCard-card">
                <div class="DSCard-text">
                    <div class="DSCard-label">Start Date</div>
                    <div class="DSCard-value">
                        <asp:Label ID="lblUsers" runat="server" Text="17 Jun, 2020" />
                    </div>
                </div>

                <div class="DSCard-icon DSCard-pastalblue">
                    📅
                </div>
            </div>

            <!-- Card 3 -->
            <div class="DSCard-card">
                <div class="DSCard-text">
                    <div class="DSCard-label">End Date</div>
                    <div class="DSCard-value">
                        <asp:Label ID="lblGrowth" runat="server" Text="04 Jul, 2020" />
                    </div>
                </div>

                <div class="DSCard-icon DSCard-orange">
                    📆
                </div>
            </div>
        </section>

        <br />

        <asp:Label ID="lblDescrizione" runat="server" Text="Descrizione" />
        <section class="DSCard-grid">
            <!-- Descrizione -->
            <div class="DSCard-card">
                <div class="DSCard-text">
                    <div class="DSCard-label">Budget</div>
                    <div>
                        <asp:Label ID="lblDes" runat="server"
                            Text="You need to develop an application on something like React native, so that it is for Android and iOS. 
                            There are about 30 screens, the design and layout in the sketch is ready. 
                            The main pages are login, getting a task, a list of tasks, a map, a history of tasks, calling the camera to complete a task.
                            The storage and processing server is on our side, there is a ready-made api for the web service that you will need to use." />
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
