<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="HistorialVentas.aspx.vb" Inherits="GUI.HistorialVentas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <link rel="stylesheet" href="../../Estilos/jquery-ui.min.css" />
    <script src="../../Scripts/JQuery/jquery-3.2.1.js"></script>
    <script src="../../Scripts/JQuery/jquery-ui-1.12.1.min.js"></script>
    <script type="text/javascript" src="../../Scripts/Propios/HistorialVentas.js?Version=1"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfDateFrom" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfDateTo" ClientIDMode="Static" runat="server" />

    <div class="col-sm-11 col-lg-11 form-group">
        <asp:Button ID="L_FEC_D" runat="server" Text="Fecha desde" CssClass="col-sm-6 col-lg-6 btn btn-light text-right" />
        <div class="col-sm-6 col-lg-6 text-left">
            <input type="text" id="txtFecDesde" class="col-sm-4 col-lg-4 form-control text-left" readonly="true" />
        </div>
    </div>
    <div class="col-sm-11 col-lg-11 form-group">
        <asp:Button ID="L_FEC_H" runat="server" Text="Fecha hasta" CssClass="col-sm-6 col-lg-6 btn btn-light text-right" />
        <div class="col-sm-6 col-lg-6 text-left">
            <input type="text" id="txtFecHasta" class="col-sm-4 col-lg-4 form-control text-left" readonly="true" />
        </div>
    </div>
    <div class="col-sm-12 col-lg-12 text-center pt-2">
        <asp:Label ID="lblError" ClientIDMode="Static" CssClass="control-label" runat="server"></asp:Label>
    </div>
    <div class="col-sm-12 col-lg-12 text-center pt-2">
        <asp:Button ID="B_ACEPTAR" CssClass="btn btn-success" Text="Aceptar" OnClientClick="return Validar();" runat="server" />
        <asp:Button ID="B_LIMPIAR" CssClass="btn btn-info" Text="Limpiar" OnClientClick="return Validar();" runat="server" />
    </div>
    <div class="col-sm-12 col-lg-12 pt-2">
        <asp:GridView ID="gvFacturas" runat="server" HorizontalAlign="Center" BorderColor="Transparent"
            OnSelectedIndexChanged="OnSelectedIndexChanged_gvFacturas" AutoGenerateColumns="false"
            AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue"
            OnRowDataBound="gvFacturas_RowDataBound" PageSize="5" AutoSizeColumnsMode="Fill"
            AllowPaging="true" OnPageIndexChanging="OnPaging_gvFacturas" AutoGenerateSelectButton="true"
            CssClass="table table-striped table-condensed table-hover">
            <Columns>
                <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                    <ItemTemplate>
                        <asp:Label ID="lblID" runat="server" CssClass="centerItemTemplate" Text='<%# Eval("ID")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fecha" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                    <ItemTemplate>
                        <asp:Label ID="lblFecha" runat="server" Text='<%# Eval("Fecha")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cliente" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                    <ItemTemplate>
                        <asp:Label ID="lblNomCli" runat="server" Enabled="false" Text='<%# Eval("Cliente.Nombre") & " " & Eval("Cliente.Apellido") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo factura" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                    <ItemTemplate>
                        <asp:Label ID="lblTipoFactura" runat="server" CssClass="centerItemTemplate" Text='<%# Eval("TipoFactura.Tipo")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IVA" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                    <ItemTemplate>
                        <asp:Label ID="lblIVA" runat="server" CssClass="centerItemTemplate" Text='<%# Eval("IVA.Nombre")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Precio total" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                    <ItemTemplate>
                        <asp:Label ID="lblPrecioTotal" runat="server" CssClass="centerItemTemplate" Text='<%# Eval("PrecioTotal")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="col-sm-12 col-lg-12 pt-2">
        <asp:GridView AutoSizeColumnsMode="Fill" ID="gvDetalles" runat="server" HorizontalAlign="Center"
            AutoGenerateColumns="false" PageSize="5" OnRowDataBound="gvDetalles_RowDataBound"
            AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue"
            BorderColor="Transparent" AllowPaging="true" OnPageIndexChanging="OnPaging_gvDetalles"
            CssClass="table table-striped table-condensed table-hover">
            <Columns>
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
                        <asp:Label ID="lblPrecioUnitario" runat="server" Enabled="false" Text='<%# Eval("PrecioUnitario")%>'></asp:Label>
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
</asp:Content>
