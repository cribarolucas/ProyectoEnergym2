<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Calcular_Maquinas.aspx.vb" Inherits="GUI.Calcular_Maquinas" EnableEventValidation = "false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <script type="text/javascript" src="../../Scripts/Propios/CalcularMaquinas.js?Version=3"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />

    <div class="container">
        <div class="form-group">
                    <asp:Label ID="L_Monto" Text="Monto disponible" CssClass="col-sm-4 col-lg-4 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-4 col-lg-4">
                        <asp:TextBox ID="txtMonto" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                        <br/>
                                             <div class="form-group">
                 <asp:RadioButtonList ID="RBList" runat="server" AutoPostBack="True">
                     <asp:ListItem Selected="True" Value="RBMuscu">Gimnasio de Musculacion</asp:ListItem>
                     <asp:ListItem Value="RBCardio">Gimnasio Cardiovascular</asp:ListItem>
                 </asp:RadioButtonList>
            </div>
                    </div>
            </div>


        <br/>
    <br/>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:GridView AutoSizeColumnsMode="Fill" ID="gvProductos2" runat="server" HorizontalAlign="Center"
                OnSelectedIndexChanged="OnSelectedIndexChanged" AutoGenerateColumns="false" DataKeyNames="ID"
                AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue" BorderColor="Transparent"
                AllowPaging="true" OnPageIndexChanging="OnPaging" 
                PageSize="5" EmptyDataText=" " OnRowDataBound="gvProductos2_RowDataBound"
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
                            <asp:Label ID="lblDetalle" runat="server" Text='<%# Eval("Detalle")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Precio" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblPrecio" runat="server" Text='<%# Eval("Precio")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Imagen" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Image ID="imgProductoThumbnail" runat="server" ImageUrl='<%# Eval("FilePathThumbnail")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Cantidad" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblCantidad" Text='<%# Eval("Cantidad")%>' runat="server" ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <br/>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Label ID="lblError" ClientIDMode="Static" CssClass="control-label" runat="server"></asp:Label>
            <br/>
            <asp:Label ID="lblMensaje" ClientIDMode="Static" CssClass="control-label" runat="server"></asp:Label>
            <br/>
            <br/>
        </div>
        <br/>
        <br/>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Button ID="B_CONFIRM" Text="Confirmar" OnClientClick="return ValidarAgregar()" CssClass="btn btn-success" runat="server"></asp:Button>
            <asp:Button ID="B_LIMPIAR" Text="Limpiar" CssClass="btn btn-info" runat="server"></asp:Button>
            <asp:Button ID="B_EXPORTP" Text="Exportar a PDF" CssClass="btn btn-warning" runat="server"></asp:Button>
            <asp:Button ID="B_EXPORTE" Text="Exportar a Excel" CssClass="btn btn-warning" runat="server"></asp:Button>
        </div>
    </div>

            <div class="col-sm-12 col-lg-12 text-center">
            <asp:GridView AutoSizeColumnsMode="Fill" ID="gvProductos" runat="server" HorizontalAlign="Center"
                OnSelectedIndexChanged="OnSelectedIndexChanged" AutoGenerateColumns="false" DataKeyNames="ID"
                AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue" BorderColor="Transparent"
                AllowPaging="true" OnPageIndexChanging="OnPaging" 
                PageSize="5" EmptyDataText=" " OnRowDataBound="gvProductos_RowDataBound"
                CssClass="table" Visible="false">
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
                            <asp:Label ID="lblDetalle" runat="server" Text='<%# Eval("Detalle")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Precio" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblPrecio" runat="server" Text='<%# Eval("Precio")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Cantidad" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblCantidad" Text='<%# Eval("Cantidad")%>' runat="server" ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH2" runat="server">
</asp:Content>
