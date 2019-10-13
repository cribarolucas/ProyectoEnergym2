<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Productos.aspx.vb" Inherits="GUI.Productos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">

    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />
    <div class="container">
        <div class="container">
            <div class="col-sm-2 col-lg-2 text-right">
                <asp:Button ID="B_SEE_CART" Text="Ir al carrito" NavigateUrl="~/Sites/Negocio/CarritoCompras.aspx" CssClass="btn btn-info" runat="server" />
            </div>
            <div class="col-sm-10 col-lg-10 form-group text-left">
                <asp:Label ID="L_ITEMS_T" Text="Cantidad de ítems en el carrito:" Font-Bold="true" CssClass="col-sm-4 col-lg-4 control-label text-right" runat="server" />
                <div class="col-sm-8 col-lg-8">
                    <asp:Label ID="L_ITEMS_V" Font-Bold="true" CssClass="control-label text-left" runat="server" />
                </div>
            </div>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:DataList ID="dlProductos" RepeatDirection="Horizontal" RepeatColumns="5" OnItemCommand="dlProductos_ItemCommand"
                OnItemDataBound="dlProductos_ItemDataBound" runat="server" CellSpacing="50">
                <ItemTemplate>
                    <div class="col-sm-12 col-lg-12">
                        <asp:Label ID="lblID" Text='<%# Eval("ID")%>' Visible="false" runat="server" />
                    </div>
                    <div class="pt-3">
                        <div class="card">
                            <div class="card-block">
                                <div class="d-flex justify-content-around">
                                    <asp:Image ID="imgProducto" ImageUrl='<%# Eval("FilePath")%>' CssClass="center-block" Width="100%" Height="100%" runat="server" />
                                </div>
                                <div>
                                    <asp:Label ID="lblNombre" Text='<%# Eval("Nombre")%>' runat="server" />
                                </div>
                                <div>
                                    <asp:Label ID="L_PRECIO" Text='Precio' ClientIDMode="Static" runat="server" />
                                    <div class="pl-3">
                                        <asp:Label ID="txtPrecio" ClientIDMode="Static" Text='<%# Eval("Precio")%>' CssClass="text-left" runat="server" />
                                    </div>
                                </div>
                                <div class="btn-group-vertical">
                                    <asp:Button ID="B_VER_DET" CssClass="btn btn-info btn-block" ClientIDMode="Static" Text="Ver detalle" CommandName="SeeDetails" runat="server" />
                                    <asp:Button ID="B_ADD_CART" CssClass="btn btn-success btn-block" ClientIDMode="Static" Text="Añadir al carrito" CommandName="AddShopCart" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
                <SeparatorStyle BorderStyle="Solid" />
            </asp:DataList>
        </div>
    </div>
</asp:Content>
