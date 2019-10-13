<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Cambiar_Contraseña.aspx.vb" Inherits="GUI.Cambiar_Contraseña" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <script type="text/javascript" src="../../Scripts/Propios/Clientes.js?Version=2"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />
    <div class="container">
        <div class="form-group">
            <asp:Label ID="L_PWD_CURR" Text="Clave actual" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtClave" ClientIDMode="Static" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group pt-2">
            <asp:Label ID="L_PWD_NEW" ClientIDMode="Static" Text="Clave nueva" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtClaveNew" ClientIDMode="Static" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group pt-2">
            <asp:Label ID="L_CLAVE_R" Text="Repetir clave" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtClaveNewR" ClientIDMode="Static" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-sm-12 col-lg-12 text-center pt-2">
            <asp:Label ID="lblError" ClientIDMode="Static" CssClass="control-label" runat="server"></asp:Label>
        </div>
        <div class="col-sm-12 col-lg-12 text-center pt-2">
            <asp:Button ID="B_ACEPTAR" Text="Aceptar" CssClass="btn btn-success" runat="server" OnClientClick="return ActualizarClave();"></asp:Button>
        </div>
    </div>
</asp:Content>
