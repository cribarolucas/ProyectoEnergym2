<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Pedidos.aspx.vb" Inherits="GUI.Pedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <link rel="stylesheet" href="../../Estilos/jquery-ui.min.css" />
    <script src="../../Scripts/JQuery/jquery-3.2.1.js"></script>
    <script src="../../Scripts/JQuery/jquery-ui-1.12.1.min.js"></script>
    <link rel="stylesheet" href="../../Estilos/chosen.css" />
    <script src="../../Scripts/JQuery/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Scripts/Propios/Pedidos.js?Version=3"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfPedidoID" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfEstadoPedidoID" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfDateFrom" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfDateTo" ClientIDMode="Static" runat="server" />

    <div class="container">
        <div class="col-sm-11 col-lg-11 form-group">
            <asp:Label ID="L_PED_EST" Text="Estado de pedido" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" />
            <div class="col-sm-4 col-lg-4">
                <asp:DropDownList ID="ddlEstado" class="chosen-select" Height="100%" Width="65%" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div class="col-sm-11 col-lg-11 form-group">
            <asp:Button ID="L_FEC_D" runat="server" Text="Fecha desde" CssClass="col-sm-6 col-lg-6 btn btn-light text-right" Enabled="false" />
            <div class="col-sm-6 col-lg-6 text-left">
                <input type="text" id="txtFecDesde" class="col-sm-5 col-lg-5 form-control text-left" readonly="true" />
            </div>
        </div>
        <div class="col-sm-11 col-lg-11 form-group">
            <asp:Button ID="L_FEC_H" runat="server" Text="Fecha hasta" CssClass="col-sm-6 col-lg-6 btn btn-light text-right" Enabled="false" />
            <div class="col-sm-6 col-lg-6 text-left">
                <input type="text" id="txtFecHasta" class="col-sm-5 col-lg-5 form-control text-left" readonly="true" />
            </div>
        </div>
        <div class="col-sm-12 col-lg-12 text-center pt-2">
            <asp:GridView ID="gvPedidos" ClientIDMode="Static" runat="server" HorizontalAlign="Center"
                OnSelectedIndexChanged="gvPedidos_SelectedIndexChanged" AutoSizeColumnsMode="Fill"
                AutoGenerateColumns="false" AutoGenerateSelectButton="true" OnRowDataBound="gvPedidos_RowDataBound"
                AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue" BorderColor="Transparent"
                OnPageIndexChanging="gvPedidos_PageIndexChanging" PageSize="5" AllowPaging="true"
                CssClass="table table-striped table-condensed table-hover">
                <Columns>
                    <asp:TemplateField Visible="false" HeaderText="ID" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblFecha" runat="server" Text='<%# Eval("Fecha")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblClienteID" Visible="false" Text='<%# Eval("Cliente.ID") %>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cliente" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblCliente" runat="server" Text='<%# Eval("Cliente.Nombre") & " " & Eval("Cliente.Apellido")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Precio Total" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblPrecioTotal" runat="server" CssClass="centerItemTemplate" Text='<%# Eval("PrecioTotal")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblEstadoID" runat="server" Text='<%# Eval("Estado.ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estado" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblEstado" runat="server" Text='<%# Eval("Estado.Nombre")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblClienteIVAID" runat="server" Text='<%# Eval("Cliente.IVA.ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:GridView ID="gvDetalles" ClientIDMode="Static" runat="server"
                OnRowDataBound="gvDetalles_RowDataBound" AutoSizeColumnsMode="Fill"
                AutoGenerateColumns="false" PageSize="5" HorizontalAlign="Center"
                AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue" BorderColor="Transparent"
                OnPageIndexChanging="gvDetalles_PageIndexChanging" AllowPaging="true"
                CssClass="table table-striped table-condensed table-hover">
                <Columns>
                    <asp:TemplateField Visible="false" HeaderText="ID" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID")%>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblProductoID" runat="server" Text='<%# Eval("Producto.ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Producto" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblProducto" runat="server" Text='<%# Eval("Producto.Nombre")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cantidad" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblCantidad" runat="server" Text='<%# Eval("Cantidad")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Precio Unitario" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblPrecioUnitario" runat="server" Text='<%# Eval("PrecioUnitario")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Precio Subtotal" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblPrecioSubtotal" runat="server" Text='<%# Eval("PrecioSubtotal")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Label ID="lblError" ClientIDMode="Static" CssClass="control-label" runat="server"></asp:Label>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Button ID="B_PED_LIS" Text="Listar pedidos" CssClass="btn btn-primary" OnClientClick="return ValidarListar();" runat="server" />
            <asp:Button ID="B_PED_APR" Text="Aprobar pedido" CssClass="btn btn-success" OnClientClick="return ValidarCambioEstado();" runat="server" />
            <asp:Button ID="B_PED_CAN" Text="Cancelar pedido" CssClass="btn btn-danger" OnClientClick="return ValidarCambioEstado();" runat="server" />
            <asp:Button ID="B_LIMPIAR" Text="Limpiar selección" CssClass="btn btn-info" runat="server" />
        </div>
    </div>

</asp:Content>
