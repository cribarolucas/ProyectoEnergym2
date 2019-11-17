<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Stock.aspx.vb" Inherits="GUI.Stock" 
EnableEventValidation = "false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <script type="text/javascript" src="../../Scripts/Propios/Stock.js?Version=3"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />

    <div class="container">
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:GridView ID="gvStock" ClientIDMode="Static" runat="server"
                OnSelectedIndexChanged="gvStock_SelectedIndexChanged" HorizontalAlign="Center"
                AutoGenerateColumns="false" PageSize="5" BorderColor="Transparent"
                AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue"
                AutoGenerateSelectButton="true" AutoSizeColumnsMode="Fill"
                OnPageIndexChanging="gvStock_PageIndexChanging" AllowPaging="true"
                CssClass="table table-striped table-condensed table-hover">
                <Columns>
                    <asp:TemplateField Visible="false" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblProductoID" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Producto" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblProducto" runat="server" Text='<%# Eval("Nombre")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblStockID" runat="server" Text='<%# Eval("Stock.ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cantidad" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblCantidad" runat="server" Text='<%# Eval("Stock.Cantidad")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="form-group">
            <asp:Label ID="L_ID" Text="ID Producto" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtProdID" ClientIDMode="Static" Enabled="false" CssClass="form-control" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_NOMBRE" Text="Nombre producto" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtNombreProducto" Enabled="false" CssClass="form-control" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_CANT_ST" Text="Cantidad" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtCantidad" MaxLength="3" TextMode="Number" ClientIDMode="Static" CssClass="form-control" runat="server" />
            </div>
        </div>
        <div class="col-sm-12 col-lg-12 text-center pt-2">
            <asp:Label ID="lblError" ClientIDMode="Static" CssClass="control-label" runat="server"></asp:Label>
        </div>
        <div class="col-sm-12 col-lg-12 text-center pt-2">
            <asp:Button ID="B_ODP_GEN" Text="Generar orden de produccón" CssClass="btn btn-success" OnClientClick="return GenerarOrdenProduccion();" runat="server" />
            <asp:Button ID="B_LIMPIAR" Text="Limpiar selección" CssClass="btn btn-info" runat="server" />
        
        <asp:Button ID="B_EXPORTP" Text="Exportar a PDF" CssClass="btn btn-warning" runat="server"></asp:Button>
<asp:Button ID="B_EXPORTE" Text="Exportar a Excel" CssClass="btn btn-warning" runat="server"></asp:Button>
        </div>
    </div>
</asp:Content>
