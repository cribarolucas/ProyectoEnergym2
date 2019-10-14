<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Idiomas.aspx.vb" Inherits="GUI.Idiomas" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <script src="../../Scripts/JQuery/jquery-3.2.1.js"></script>
    <link rel="stylesheet" href="../../Estilos/chosen.css" />
    <script src="../../Scripts/JQuery/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Scripts/Propios/Idiomas.js?Version=9"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />
    <div class="container">
        <div id="idiomas" class="col-sm-6 col-lg-6 text-center">
            <div class="col-sm-12 col-lg-12">
                <asp:GridView AutoSizeColumnsMode="Fill" ID="gvIdiomas" runat="server"
                    OnSelectedIndexChanged="OnSelectedIndexChanged_I" AutoGenerateColumns="false" DataKeyNames="Codigo"
                    AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue" BorderColor="Transparent"
                    AllowPaging="true" OnPageIndexChanging="OnPaging_I" AutoGenerateSelectButton="true"
                    PageSize="5" HorizontalAlign="Center" CssClass="table table-striped table-condensed table-hover">
                    <Columns>
                        <asp:TemplateField HeaderText="Codigo" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("Codigo")%>'></asp:Label>
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
            <div class="col-sm-12 col-lg-12 pt-2">
                <asp:Label ID="lblErrorI" ClientIDMode="Static" runat="server"></asp:Label>
            </div>
            <div class="col-sm-12 col-lg-12 pt-2">
                <div class="form-group">
                    <asp:Label ID="L_COD_IDI" Text="ID" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-3 col-lg-3">
                        <asp:TextBox ID="txtIdiomaCodigo" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                        <%--<asp:DropDownList ID="ddlIdioma" ClientIDMode="Static" class="chosen-select" Height="100%" runat="server"></asp:DropDownList>--%>
                    </div>
                </div>
            </div>
            <div class="col-sm-12 col-lg-12 pt-2">
                <div class="form-group">
                    <asp:Label ID="L_NOM_IDI" Text="Nombre" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-6 col-lg-6">
                        <asp:TextBox ID="txtIdiomaNombre" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-sm-12 col-lg-12 pt-2">
                <asp:Button ID="B_AGREGAR" Text="Agregar" CssClass="btn btn-success" OnClientClick="return ValidarAgregarModificarIdioma()" runat="server"></asp:Button>
                <asp:Button ID="B_MODIF_I" Text="Modificar" CssClass="btn btn-primary" OnClientClick="return ValidarAgregarModificarIdioma()" runat="server"></asp:Button>
                <asp:Button ID="B_ELIMINAR" Text="Eliminar" CssClass="btn btn-danger" OnClientClick="return ValidarEliminarIdioma()" runat="server"></asp:Button>
                <asp:Button ID="B_LIMPIARI" Text="Limpiar" CssClass="btn btn-info" runat="server"></asp:Button>
            </div>
        </div>
        <div id="leyendas" class="col-sm-6 col-lg-6 text-center">
            <div class="col-sm-12 col-lg-12">
                <asp:GridView AutoSizeColumnsMode="Fill" AutoGenerateSelectButton="true"
                    ID="gvLeyendas" runat="server" AutoGenerateColumns="false" DataKeyNames="Codigo"
                    AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue" BorderColor="Transparent"
                    OnSelectedIndexChanged="OnSelectedIndexChanged_L" AllowPaging="true"
                    OnPageIndexChanging="OnPaging_L" PageSize="15" Visible="false"
                    HorizontalAlign="Center" CssClass="table table-striped table-condensed table-hover">
                    <Columns>
                        <asp:TemplateField HeaderText="Código Leyenda" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblCodLey" runat="server" Text='<%# Eval("Codigo")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descripción" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblDescLey" runat="server" Text='<%# Eval("Descripcion")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="col-sm-12 col-lg-12 pt-2">
                <asp:Label ID="lblErrorL" ClientIDMode="Static" runat="server"></asp:Label>
            </div>
            <div class="col-sm-12 col-lg-12 pt-2">
                <div class="form-group">
                    <asp:Label ID="L_COD_LEY" Text="ID" Visible="false" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-4 col-lg-4">
                        <asp:TextBox ID="txtLeyID" ClientIDMode="Static" Enabled="false" Visible="false" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-sm-12 col-lg-12 pt-2">
                <div class="form-group">
                    <asp:Label ID="L_DESC_LEY" Visible="false" Text="Descripción" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-6 col-lg-6">
                        <asp:TextBox ID="txtLeyDesc" Visible="false" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div>
                <div class="col-sm-12 col-lg-12 pt-2">
                    <asp:Button ID="B_MODIF_L" Text="Modificar" Visible="false" CssClass="btn btn-primary" OnClientClick="return ValidarModificarLeyenda()" runat="server"></asp:Button>
                    <asp:Button ID="B_LIMPIARL" Text="Limpiar" Visible="false" CssClass="btn btn-info" runat="server"></asp:Button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
