<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Default.aspx.vb" Inherits="GUI._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH" runat="server">
    <script type="text/javascript" src="../Scripts/Propios/Inicio.js?Version=2"></script>
    <div class="container">
        <div class="col-sm-12 col-lg-12 text-center text-info">
            <asp:Label ID="L_QUIEN_T" runat="server" />
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Image ID="Energym_Logo" runat="server" ImageUrl="~/Images/Energym - Logo.jpg" Height="15%" Width="15%" />
        </div>
    </div>
</asp:Content>
