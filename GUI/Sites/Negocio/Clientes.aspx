<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Clientes.aspx.vb" Inherits="GUI.Clientes" EnableEventValidation = "false"%>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <script src="../../Scripts/JQuery/jquery-3.2.1.js"></script>
    <link rel="stylesheet" href="../../Estilos/chosen.css" />
    <script src="../../Scripts/JQuery/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Scripts/Propios/Clientes.js?Version=16"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfClienteID" ClientIDMode="Static" runat="server" />
    <div class="container">
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:GridView ID="gvClientes" ClientIDMode="Static"
                AutoSizeColumnsMode="Fill" runat="server" AllowPaging="true"
                AutoGenerateColumns="false" PageSize="5" BorderColor="Transparent"
                AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue" 
                OnPageIndexChanging="gvClientes_PageIndexChanging"
                CssClass="table table-striped table-condensed table-hover">
                <Columns>
                    <asp:TemplateField HeaderText="Nombre" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("Nombre")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Apellido" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblApellido" runat="server" Text='<%# Eval("Apellido")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DNI" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblDNI" runat="server" Text='<%# Eval("DNI")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CUIT" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblCUIT" runat="server" Text='<%# Eval("CUIT")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Calle" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblCalle" runat="server" Text='<%# Eval("Calle")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Altura" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblAltura" runat="server" Text='<%# Eval("Altura")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Piso" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblPiso" runat="server" Text='<%# Eval("Piso")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Departamento" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblDepartamento" runat="server" Text='<%# Eval("Departamento")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Código postal" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblCodigoPostal" runat="server" Text='<%# Eval("CodigoPostal")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Telefono" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblTelefono" runat="server" Text='<%# Eval("Telefono")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IVA" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblIVA" runat="server" Text='<%# Eval("IVA.Nombre")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Provincia" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblProvincia" runat="server" Text='<%# Eval("Provincia.Nombre")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Localidad" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblLocalidad" runat="server" Text='<%# Eval("Localidad.Nombre")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
             <div class="form-group">
                 <asp:RadioButtonList ID="RBList" runat="server" AutoPostBack="True">
                     <asp:ListItem Selected="True" Value="RBFisica">Persona Fisica</asp:ListItem>
                     <asp:ListItem Value="RBJuridica">Persona Juridica</asp:ListItem>
                 </asp:RadioButtonList>
            </div>
        
        <div class="form-group">
            <asp:Label ID="L_ID" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="ID" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtID" CssClass="form-control" ClientIDMode="Static" Enabled="false" runat="server" />
            </div>
        </div>
        <br/>
        <div class="form-group">
            <asp:Label ID="L_NOM_USU" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="Nombre Usuario" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtNomUsu" CssClass="form-control" MaxLength="20" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_CLAVE" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="Clave" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtClave" CssClass="form-control" TextMode="Password" MaxLength="32" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_CLAVE_R" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="Repetir clave" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtClaveR" CssClass="form-control" TextMode="Password" MaxLength="32" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_NOMBRE" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="Nombre" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtNombre" CssClass="form-control" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_APELLIDO" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="Apellido" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtApellido" CssClass="form-control" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_DNI" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="DNI" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtDNI" TextMode="Number" MaxLength="9" CssClass="form-control" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_CUIT" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="CUIT/CUIL" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtCUIT" TextMode="Number" MaxLength="11" CssClass="form-control" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_CALLE" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="Calle" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtCalle" CssClass="form-control" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_ALTURA" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="Altura" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtAltura" TextMode="Number" CssClass="form-control" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_PISO" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="Piso" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtPiso" TextMode="Number" CssClass="form-control" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_DEPTO" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="Departamento" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtDepto" CssClass="form-control" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_CODPOS" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="Código postal" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtCodPos" TextMode="Number" MaxLength="5" CssClass="form-control" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_EMAIL" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="Email" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtEmail" TextMode="Email" CssClass="form-control" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_TELEFONO" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="Teléfono" />
            <div class="col-sm-2 col-lg-2">
                <asp:TextBox ID="txtTelefono" TextMode="Number" MaxLength="20" CssClass="form-control" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_IVA" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="IVA" />
            <div class="col-sm-2 col-lg-2">
                <asp:DropDownList ID="ddlIVA" class="chosen-select" Width="100%" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_PROV" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="Provincia" />
            <div class="col-sm-2 col-lg-2">
                <asp:DropDownList ID="ddlProvincia" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged"
                    AutoPostBack="true" class="chosen-select" Width="100%" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="L_LOC" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server" Text="Localidad" />
            <div class="col-sm-2 col-lg-2">
                <asp:DropDownList ID="ddlLocalidad" class="chosen-select" Width="100%" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div id="divError" class="col-sm-12 col-lg-12 text-center pt-2">
            <asp:Label ID="lblError" ClientIDMode="Static" runat="server"></asp:Label>
        </div>
        <div class="col-sm-12 col-lg-12 text-center pt-2">
            <asp:Button ID="B_REG_CLI" Text="Registrar" CssClass="btn btn-success" OnClientClick="return Validar()" runat="server"></asp:Button>
            <asp:Button ID="B_EDIT_CLI" Text="Editar" CssClass="btn btn-info" OnClientClick="return ValidarActualizar()" runat="server"></asp:Button>
            <asp:Button ID="B_CLOSE_AC" Text="Cerrar cuenta" CssClass="btn btn-danger" runat="server"></asp:Button>
            <asp:Button ID="B_LIMPIAR" Text="Limpiar" CssClass="btn btn-info" runat="server"></asp:Button>
            <asp:Button ID="B_CONFIRM" Visible="false" Text="Confirmar" CssClass="btn btn-success" runat="server"></asp:Button>
            <asp:Button ID="B_CANCELAR" Visible="false" Text="Cancelar" CssClass="btn btn-danger" runat="server"></asp:Button>
            <asp:Button ID="B_EXPORTP" Text="Exportar a PDF" CssClass="btn btn-warning" runat="server"></asp:Button>
            <asp:Button ID="B_EXPORTE" Text="Exportar a Excel" CssClass="btn btn-warning" runat="server"></asp:Button>
        
        </div>
    </div>
</asp:Content>
