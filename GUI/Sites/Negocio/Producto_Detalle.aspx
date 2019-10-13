<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Producto_Detalle.aspx.vb" Inherits="GUI.Producto_Detalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <div class="container">
        <div class="col-sm-6">
            <div class="form-group">
                <asp:Label ID="L_NOMBRE" Text="Nombre" CssClass="col-sm-5 col-lg-5 control-label text-right" runat="server"></asp:Label>
                <div class="col-sm-7 col-lg-7">
                    <asp:TextBox ID="txtNombre" CssClass="form-control" Enabled="false" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="L_DETAIL" Text="Detalle" CssClass="col-sm-5 col-lg-5 control-label text-right" runat="server"></asp:Label>
                <div class="col-sm-7 col-lg-7">
                    <asp:TextBox ID="txtDetalle" CssClass="form-control" Enabled="false" TextMode="MultiLine" Rows="5" Width="100%" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="L_ALTO" Text="Alto" CssClass="col-sm-5 col-lg-5 control-label text-right" runat="server"></asp:Label>
                <div class="col-sm-7 col-lg-7">
                    <asp:TextBox ID="txtAlto" CssClass="form-control" Enabled="false" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="L_ANCHO" Text="Ancho" CssClass="col-sm-5 col-lg-5control-label text-right" runat="server"></asp:Label>
                <div class="col-sm-7 col-lg-7">
                    <asp:TextBox ID="txtAncho" CssClass="form-control" Enabled="false" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="L_LARGO" Text="Largo" CssClass="col-sm-5 col-lg-5 control-label text-right" runat="server"></asp:Label>
                <div class="col-sm-7 col-lg-7">
                    <asp:TextBox ID="txtLargo" CssClass="form-control" Enabled="false" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="form-group col-sm-8 text-right pt-2">
                <asp:Button ID="B_VOLVER" CssClass="btn btn-info" Text="Cerrar" runat="server" />
            </div>
        </div>
        <div class="col-sm-6">
            <asp:Image ID="imgProducto" ImageUrl='<%# Eval("FilePath")%>' runat="server" Width="100%" Height="100%"></asp:Image>
        </div>
    </div>
</asp:Content>
