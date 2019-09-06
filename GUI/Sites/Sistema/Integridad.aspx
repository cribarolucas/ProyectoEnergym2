<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Integridad.aspx.vb" Inherits="GUI.Integridad" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <div class="container pt-2">
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Button ID="B_VERIF_IN" Text="Verificar" CssClass="btn btn-info" runat="server" />
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Label ID="lblError" Visible="false" CssClass="control-label" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>
