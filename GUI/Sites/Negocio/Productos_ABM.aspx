<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Productos_ABM.aspx.vb" Inherits="GUI.ProductosABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH" runat="server">
    <script type="text/javascript" src="../../Scripts/Propios/Productos.js?Version=7"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />

    <div class="container">
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:GridView AutoSizeColumnsMode="Fill" ID="gvProductos" runat="server" HorizontalAlign="Center"
                OnSelectedIndexChanged="OnSelectedIndexChanged" AutoGenerateColumns="false" DataKeyNames="ID"
                AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue" BorderColor="Transparent"
                AllowPaging="true" OnPageIndexChanging="OnPaging" AutoGenerateSelectButton="true"
                PageSize="5" EmptyDataText="No records has been added." OnRowDataBound="gvProductos_RowDataBound"
                CssClass="table">
                <Columns>
                    <asp:TemplateField HeaderText="Codigo" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nombre" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("Nombre")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Detalle" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:TextBox ID="lblDetalle" runat="server" TextMode="MultiLine" Rows="5" Enabled="false" Text='<%# Eval("Detalle")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Precio" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblPrecio" runat="server" Text='<%# Eval("Precio")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stock" Visible="false" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblCantidad" runat="server" Text='<%# Eval("Stock.Cantidad")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false" HeaderText="Imagen" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Image ID="imgProducto" Width="30%" runat="server" ImageUrl='<%# Eval("FilePath")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Imagen" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Image ID="imgProductoThumbnail" runat="server" ImageUrl='<%# Eval("FilePathThumbnail")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Alto" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblAlto" runat="server" Text='<%# Eval("Alto")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Largo" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblLargo" runat="server" Text='<%# Eval("Largo")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ancho" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblAncho" runat="server" Text='<%# Eval("Ancho")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Label ID="lblError" ClientIDMode="Static" CssClass="control-label" runat="server"></asp:Label>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <div class="col-sm-6 col-lg-6 text-center">
                <div class="form-group">
                    <asp:Label ID="L_ID" Text="ID" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-4 col-lg-4">
                        <asp:TextBox ID="txtID" ClientIDMode="Static" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="L_NOMBRE" Text="Nombre" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-4 col-lg-4">
                        <asp:TextBox ID="txtNombre" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="L_DETAIL" Text="Detalle" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-4 col-lg-4">
                        <asp:TextBox ID="txtDetalle" TextMode="MultiLine" Rows="5" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="L_PRECIO" Text="Precio" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-4 col-lg-4">
                        <asp:TextBox ID="txtPrecio" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="L_IMAGEN" Text="Imagen" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-4 col-lg-4">
                        <asp:FileUpload ID="fuProducto" ClientIDMode="Static" runat="server"></asp:FileUpload>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="L_ALTO" Text="Alto" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-4 col-lg-4">
                        <asp:TextBox ID="txtAlto" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="L_ANCHO" Text="Ancho" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-4 col-lg-4">
                        <asp:TextBox ID="txtAncho" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="L_LARGO" Text="Largo" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-4 col-lg-4">
                        <asp:TextBox ID="txtLargo"  ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-lg-6 text-left">
                <asp:Image ID="imgProducto" runat="server" Width="80%" Height="80%" />
            </div>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Button ID="B_AGREGAR" Text="Agregar" OnClientClick="return ValidarAgregar()" CssClass="btn btn-success" runat="server"></asp:Button>
            <asp:Button ID="B_MODIFIC" Text="Modificar" OnClientClick="return ValidarModificar()" CssClass="btn btn-primary" runat="server"></asp:Button>
            <asp:Button ID="B_ELIMINAR" Text="Eliminar" OnClientClick="return ValidarEliminar()" CssClass="btn btn-danger" runat="server"></asp:Button>
            <asp:Button ID="B_LIMPIAR" Text="Limpiar" CssClass="btn btn-info" runat="server"></asp:Button>
        </div>
    </div>
</asp:Content>
