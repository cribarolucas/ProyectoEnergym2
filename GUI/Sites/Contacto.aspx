<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Contacto.aspx.vb" Inherits="GUI.Contacto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH" runat="server">
    <script type="text/javascript" src="../../Scripts/Propios/Contacto.js?Version=1"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />
    <div class="container">
        <div class="col-sm-12 col-lg-12 text-center">
            <h4>
                <asp:Label ID="L_CONTACT" runat="server" />
            </h4>
        </div>
        <div class="form-group">
            <asp:Label ID="L_NOMBRE" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
            <div class="col-sm-3 col-lg-3">
                <asp:TextBox ID="txtNombre" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_APELLIDO" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
            <div class="col-sm-3 col-lg-3">
                <asp:TextBox ID="txtApellido" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_EMAIL" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
            <div class="col-sm-3 col-lg-3">
                <asp:TextBox ID="txtEmail" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_TELEFONO" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
            <div class="col-sm-3 col-lg-3">
                <asp:TextBox ID="txtTelefono" TextMode="Number" MaxLength="20" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_CONSULTA" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
            <div class="col-sm-3 col-lg-3">
                <asp:TextBox ID="txtConsulta" ClientIDMode="Static" TextMode="MultiLine" Rows="5" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-sm-12 col-lg-12 text-center pt-2">
            <asp:Label ID="lblError" ClientIDMode="Static" CssClass="control-label" runat="server"></asp:Label>
        </div>
        <div class="col-sm-12 col-lg-12 text-center pt-2">
            <asp:Button ID="B_ACEPTAR" Text="Aceptar" OnClientClick="return Validar();" CssClass="btn btn-success" runat="server" />
        </div>
    </div>
</asp:Content>
