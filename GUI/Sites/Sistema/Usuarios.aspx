<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Usuarios.aspx.vb" Inherits="GUI.Usuarios" EnableEventValidation = "false"%>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <script src="../../Scripts/JQuery/jquery-3.2.1.js"></script>
    <link rel="stylesheet" href="../../Estilos/chosen.css" />
    <script src="../../Scripts/JQuery/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Scripts/Propios/Usuarios.js?Version=7"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfEsCliente" ClientIDMode="Static" runat="server" />

    <div class="container">
        <div class="col-sm-12 col-lg-12">
            <asp:GridView ID="gvUsuarios" runat="server" AutoSizeColumnsMode="Fill"
                DataKeyNames="ID" AutoGenerateColumns="false" AutoGenerateSelectButton="true"
                OnSelectedIndexChanged="OnSelectedIndexChanged" OnPageIndexChanging="OnPaging"
                OnRowDataBound="gvUsuarios_RowDataBound" PageSize="5" AllowPaging="true"
                AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue" BorderColor="Transparent"
                HorizontalAlign="Center" CssClass="table table-striped table-condensed table-hover">
                <Columns>
                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nombre" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("NombreDeUsuario")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Clave" Visible="false" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblClave" runat="server" Text='<%# Eval("Clave")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Idioma" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblIdioma" runat="server" Text='<%# Eval("Idioma")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Bloqueado" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblBloqueado" runat="server" Text='<%# Eval("Bloqueado")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Intentos fallidos" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblIntFall" runat="server" Text='<%# Eval("CantidadDeIntentosFallidos")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Activo" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblActivo" runat="server" Text='<%# Eval("Activo")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="col-sm-12 col-lg-12 pt-2">
            <div class="col-sm-6 col-lg-6">
                <div class="form-group">
                    <asp:Label ID="L_ID" Text="ID" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-6 col-lg-6">
                        <asp:TextBox ID="txtID" ClientIDMode="Static" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="L_NOM_USU" Text="Nombre" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-6 col-lg-6">
                        <asp:TextBox ID="txtNombre" MaxLength="20" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="L_CLAVE" Text="Clave" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-6 col-lg-6">
                        <asp:TextBox ID="txtClave" TextMode="Password" MaxLength="32" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="L_CLAVE_R" runat="server" CssClass="col-sm-6 col-lg-6 control-label text-right" Text="Repetir clave" />
                    <div class="col-sm-6 col-lg-6">
                        <asp:TextBox ID="txtClaveR" TextMode="Password" MaxLength="32" ClientIDMode="Static" CssClass="form-control" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="L_IDIOMA" Text="Idioma" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-6 col-lg-6">
                        <asp:DropDownList ID="ddlIdiomas" ClientIDMode="Static" class="chosen-select" Height="100%" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="L_BLOQ" Text="Bloqueado" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-6 col-lg-6">
                        <asp:CheckBox ID="cbBloqueado" CssClass="form-check" runat="server"></asp:CheckBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="L_INT_FALL" ClientIDMode="Static" Text="Intentos fallidos" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-6 col-lg-6">
                        <asp:TextBox ID="txtIntFall" MaxLength="1" TextMode="Number" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="L_ACTIVO" Text="Activo" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-6 col-lg-6">
                        <asp:CheckBox ID="cbActivo" CssClass="form-check" runat="server"></asp:CheckBox>
                    </div>
                </div>
                <div class="col-sm-12 col-lg-12 pt-2 text-right">
                    <asp:Label ID="lblError" ClientIDMode="Static" CssClass="control-label" runat="server"></asp:Label>
                </div>
                <div class="col-sm-12 col-lg-12 text-right">
                    <asp:Button ID="B_AGREGAR" Text="Agregar" OnClientClick="return ValidarAgregar();" CssClass="btn btn-success" runat="server"></asp:Button>
                    <asp:Button ID="B_MODIFIC" Text="Modificar" OnClientClick="return ValidarModificar();" CssClass="btn btn-primary" runat="server"></asp:Button>
                    <asp:Button ID="B_ELIMINAR" Text="Eliminar" OnClientClick="return ValidarEliminar();" CssClass="btn btn-danger" runat="server"></asp:Button>
                    <asp:Button ID="B_LIMPIAR" Text="Limpiar" CssClass="btn btn-info" runat="server"></asp:Button>
                <br/>
                     <br/>
                    <asp:Button ID="B_EXPORTP" Text="Exportar a PDF" CssClass="btn btn-warning" runat="server"></asp:Button>
                <asp:Button ID="B_EXPORTE" Text="Exportar a Excel" CssClass="btn btn-warning" runat="server"></asp:Button>
                
                </div>
            </div>
            <div class="col-sm-6 col-lg-6">
                <asp:GridView AutoSizeColumnsMode="Fill" HorizontalAlign="Left"
                    CaptionAlign="Top" ID="gvPermisos" runat="server"
                    AutoGenerateColumns="false" DataKeyNames="ID" ClientIDMode="Static"
                    AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue" BorderColor="Transparent"
                    CssClass="table table-striped table-condensed table-hover">
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSeleccionar" ClientIDMode="Static" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("Nombre")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

