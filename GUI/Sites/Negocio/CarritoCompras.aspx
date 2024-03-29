﻿<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="CarritoCompras.aspx.vb" Inherits="GUI.CarritoCompras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH" runat="server">
    <script type="text/javascript" src="../../Scripts/Propios/CarritoCompras.js?Version=6"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfUsuarioConectado" ClientIDMode="Static" runat="server" />
    <div class="col-sm-12 col-lg-12">
        <asp:GridView ID="gvCarrito" HorizontalAlign="Center" ClientIDMode="Static" runat="server"
            AutoSizeColumnsMode="Fill" PageSize="5" AllowPaging="true" BorderColor="Transparent"
            AutoGenerateColumns="false" AutoGenerateDeleteButton="true" ShowFooter="true"
            AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue"
            OnPageIndexChanging="OnPaging" OnRowDeleting="gvCarrito_RowDeleting"
            OnRowDataBound="gvCarrito_RowDataBound" CssClass="table table-striped table-condensed table-hover">
            <Columns>
                <asp:TemplateField Visible="false" HeaderText="ID" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                    <ItemTemplate>
                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("Producto.ID")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nombre" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                    <ItemTemplate>
                        <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("Producto.Nombre")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cantidad" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                    <ItemTemplate>
                        <asp:TextBox ID="txtCantidad" ClientIDMode="Static" runat="server" Text='<%# Eval("Cantidad")%>' MaxLength="3" TextMode="Number"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Precio" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" FooterStyle-CssClass="text-center">
                    <ItemTemplate>
                        <asp:Label ID="lblPrecio" runat="server" CssClass="centerItemTemplate" Text='<%# Eval("Producto.Precio")%>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="C_TOTAL" runat="server" Text="Total" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center"></asp:Label>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subtotal" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" FooterStyle-CssClass="text-center">
                    <ItemTemplate>
                        <asp:Label ID="lblSubtotal" runat="server" Text='<%# Eval("PrecioSubtotal") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotal" runat="server" Text="Valor"></asp:Label>
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div id="divError" class="col-sm-12 col-lg-12 text-center pt-2">
        <asp:Label ID="lblError" ClientIDMode="Static" runat="server"></asp:Label>
    </div>
    <div class="col-sm-12 col-lg-12 text-center pt-2">
        <asp:Button runat="server" ID="B_UPD_CART" CssClass="btn btn-info" Text="Actualizar Carrito" OnClientClick="return ValidarActualizar();" />
        <asp:Button runat="server" ID="B_PEDIDO" CssClass="btn btn-success" Text="Realizar pedido" OnClientClick="return ValidarRealizarPedido();" />
    </div>
</asp:Content>
