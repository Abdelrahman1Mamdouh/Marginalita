<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Marginalita.Dashboard" MasterPageFile="~/Site.Master" Title="DASHBOARD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <section class="DSCard-grid">
            <!-- Card 1 -->
            <div class="DSCard-card">
                <div class="DSCard-text">
                    <div class="DSCard-label">Monthly Recurring Revenue</div>
                    <div class="DSCard-value">
                        <asp:Label ID="lblMRR" runat="server" Text="$105,170" />
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
                    <div class="DSCard-label">Active Users</div>
                    <div class="DSCard-value">
                        <asp:Label ID="lblUsers" runat="server" Text="5,500" />
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
                    <div class="DSCard-label">Revenue Growth</div>
                    <div class="DSCard-value">
                        <asp:Label ID="lblGrowth" runat="server" Text="118%" />
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
                </div>
            </div>
        </section>
    </div>
</asp:Content>
