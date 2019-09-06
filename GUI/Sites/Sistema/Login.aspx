<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Login.aspx.vb" Inherits="GUI.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH" runat="server">
    <script type="text/javascript" src="../../Scripts/Propios/Login.js?Version=8"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />
    <div class="container">
        <div class="form-group">
            <asp:Label ID="L_USUARIO" CssClass="col-sm-6 col-lg-6 control-label text-right" ClientIDMode="Static" Text="Usuario" runat="server" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtUser" CssClass="form-control" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_CLAVE" CssClass="col-sm-6 col-lg-6 control-label text-right" ClientIDMode="Static" Text="Contraseña" runat="server"></asp:Label>
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtClave" CssClass="form-control" ClientIDMode="Static" runat="server" TextMode="Password" />
            </div>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Label ID="lblError" ClientIDMode="Static" CssClass="control-label" runat="server"></asp:Label>
        </div>
        <div class="col-sm-12 col-lg-12 text-center pt-2">
            <asp:Button ID="B_ACEPTAR" Text="Aceptar" CssClass="btn btn-success" runat="server" OnClientClick="return Validar();"></asp:Button>
            <asp:Button ID="B_CONFIRM" Visible="false" Text="Confirmar" CssClass="btn btn-success" runat="server"></asp:Button>
            <asp:Button ID="B_CANCELAR" Visible="false" Text="Cancelar" CssClass="btn btn-danger" runat="server"></asp:Button>
        </div>
    </div>
</asp:Content>
