<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Permisos.aspx.vb" Inherits="GUI.Permisos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <script type="text/javascript" src="../../Scripts/Propios/Permisos.js?Version=3"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />

    <div class="container">
        <div id="perfiles" class="col-sm-6 col-lg-6 text-center">
            <div class="col-sm-12 col-lg-12">
                <asp:GridView AutoSizeColumnsMode="Fill" ID="gvGrupoPermisos" runat="server"
                    AutoGenerateColumns="false" DataKeyNames="ID" AutoGenerateSelectButton="true"
                    OnSelectedIndexChanged="OnSelectedIndexChanged_GP" OnPageIndexChanging="OnPaging"
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
                                <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("Nombre")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="col-sm-12 col-lg-12 text-center pt-2">
                <asp:Label ID="lblError" ClientIDMode="Static" CssClass="control-label" runat="server"></asp:Label>
            </div>
            <div class="col-sm-12 col-lg-12 text-center pt-2">
                <div class="form-group">
                    <asp:Label ID="L_ID" Text="ID" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-2 col-lg-2">
                        <asp:TextBox ID="txtID" ClientIDMode="Static" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-sm-12 col-lg-12 text-center pt-2">
                <div class="form-group">
                    <asp:Label ID="L_NOMBRE" Text="Nombre" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-4 col-lg-4">
                        <asp:TextBox ID="txtNombre" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-sm-12 col-lg-12 text-center pt-2">
                <asp:Button ID="B_AGREGAR" Text="Agregar" OnClientClick="return ValidarAgregar()" CssClass="btn btn-success" runat="server"></asp:Button>
                <asp:Button ID="B_MODIFIC" Text="Modificar" OnClientClick="return ValidarModificar()" CssClass="btn btn-primary" runat="server"></asp:Button>
                <asp:Button ID="B_ELIMINAR" Text="Eliminar" OnClientClick="return ValidarEliminar()" CssClass="btn btn-danger" runat="server"></asp:Button>
                <asp:Button ID="B_LIMPIAR" Text="Limpiar" CssClass="btn btn-info" runat="server"></asp:Button>
            </div>
        </div>
        <div id="permisos" class="col-sm-6 col-lg-6 text-center">
            <asp:GridView AutoSizeColumnsMode="Fill" ID="gvPermisos" runat="server"
                AutoGenerateColumns="false" DataKeyNames="ID" ClientIDMode="Static"
                AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue" BorderColor="Transparent"
                HorizontalAlign="Left" CssClass="table table-striped table-condensed table-hover">
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:CheckBox ID="cbSeleccionar" runat="server" />
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
</asp:Content>
