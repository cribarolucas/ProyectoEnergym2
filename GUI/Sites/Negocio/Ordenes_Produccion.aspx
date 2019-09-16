<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Ordenes_Produccion.aspx.vb" Inherits="GUI.OrdenesProduccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <link rel="stylesheet" href="../../Estilos/jquery-ui.min.css" />
    <script src="../../Scripts/JQuery/jquery-3.2.1.js"></script>
    <script src="../../Scripts/JQuery/jquery-ui-1.12.1.min.js"></script>
    <link rel="stylesheet" href="../../Estilos/chosen.css" />
    <script src="../../Scripts/JQuery/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Scripts/Propios/Ordenes_Produccion.js?Version=2"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfOrdenProduccionID" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfEstadoOrdenID" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfDateFrom" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfDateTo" ClientIDMode="Static" runat="server" />

    <div class="container">
        <div class="col-sm-11 col-lg-11 form-group">
            <asp:Label ID="L_OP_EST" Text="Estado de pedido" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" />
            <div class="col-sm-3 col-lg-3">
                <asp:DropDownList ID="ddlEstado" class="chosen-select" Height="200%" Width="90%" runat="server"></asp:DropDownList>
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
            <asp:GridView ID="gvOrdenes" ClientIDMode="Static" runat="server"
                OnSelectedIndexChanged="gvOrdenes_SelectedIndexChanged" HorizontalAlign="Center"
                AutoGenerateColumns="false" AutoGenerateSelectButton="true"
                AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue"
                OnPageIndexChanging="gvOrdenes_PageIndexChanging" BorderColor="Transparent"
                AutoSizeColumnsMode="Fill" PageSize="5" AllowPaging="true"
                CssClass="table table-striped table-condensed table-hover">
                <Columns>
                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
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
                    <asp:TemplateField HeaderText="Fecha inicio" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblFechaInicio" runat="server" Text='<%# Eval("FechaInicio")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha fin" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblFechaFin" runat="server" Text='<%# Eval("FechaFin")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:GridView ID="gvDetalles" ClientIDMode="Static" runat="server"
                AutoGenerateColumns="false" PageSize="5" BorderColor="Transparent"
                AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue"
                OnPageIndexChanging="gvDetalles_PageIndexChanging" HorizontalAlign="Center"
                AutoSizeColumnsMode="Fill" AllowPaging="true"
                CssClass="table table-striped table-condensed table-hover">
                <Columns>
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
                </Columns>
            </asp:GridView>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Label ID="lblError" ClientIDMode="Static" CssClass="control-label" runat="server"></asp:Label>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Button ID="B_ODP_LIS" Text="Listar pedidos" CssClass="btn btn-primary" OnClientClick="return ValidarListar();" runat="server" />
            <asp:Button ID="B_ODP_APR" Text="Aprobar orden de producción" CssClass="btn btn-success" OnClientClick="return ValidarAprCanc();" runat="server" />
            <asp:Button ID="B_ODP_CAN" Text="Cancelar orden de producción" CssClass="btn btn-danger" OnClientClick="return ValidarAprCanc();" runat="server" />
            <asp:Button ID="B_LIMPIAR" Text="Limpiar selección" CssClass="btn btn-info" runat="server" />
        </div>
    </div>
</asp:Content>
