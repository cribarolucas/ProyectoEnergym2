<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Bitacora.aspx.vb" Inherits="GUI.Bitacora" %>
 
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <style>
    .ui-icon.ui-icon-circle-triangle-w{
        
    width: 0;
    height: 0;
    border-top: 10px solid transparent;
    border-bottom: 10px solid transparent;
    border-right: 10px solid #343a40;

    }
    .ui-icon.ui-icon-circle-triangle-e{
  width: 0; 
  height: 0; 
  border-top: 10px solid transparent;
  border-bottom: 10px solid transparent;
  
  border-left: 10px solid #343a40;
    }
</style> 
    <link rel="stylesheet" href="../../Estilos/jquery-ui.min.css" />
    <script src="../../Scripts/JQuery/jquery-3.2.1.js"></script>
    <script src="../../Scripts/JQuery/jquery-ui-1.12.1.min.js"></script>
    <link rel="stylesheet" href="../../Estilos/chosen.css" />
    <script src="../../Scripts/JQuery/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Scripts/Propios/Bitacora.js?Version=4"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfDateFrom" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfDateTo" ClientIDMode="Static" runat="server" />
    <div class="container">
        <div class="col-sm-12 col-lg-12 form-group">
            <asp:Button ID="L_USUARIO" Text="Usuario" CssClass="col-sm-6 col-lg-6 btn btn-light text-right" Enabled="false" runat="server" />
            <div class="col-sm-2 col-lg-2 text-left">
                <asp:DropDownList ID="ddlUsuario" class="chosen-select" Height="200%" Width="110%" runat="server" />
            </div>
        </div>
        <div class="col-sm-12 col-lg-12 form-group">
            <asp:Button ID="L_ES_ERROR" Text="Es error" CssClass="col-sm-6 col-lg-6 btn btn-light text-right" Enabled="false" runat="server" />
            <div class="col-sm-5 col-lg-5 text-left">
                <asp:CheckBox ID="cbError" CssClass="checkbox" runat="server" />
            </div>
        </div>
        <div class="col-sm-12 col-lg-12 form-group">
            <asp:Button ID="L_FEC_D" runat="server" Text="Fecha desde" CssClass="col-sm-6 col-lg-6 btn btn-light text-right" Enabled="false" />
            <div class="col-sm-6 col-lg-6 text-left">
                <input type="text" id="txtFecDesde" class="col-sm-4 col-lg-4 form-control text-left" readonly="true" />
            </div>
        </div>
        <div class="col-sm-12 col-lg-12 form-group">
            <asp:Button ID="L_FEC_H" runat="server" Text="Fecha hasta" CssClass="col-sm-6 col-lg-6 btn btn-light text-right" Enabled="false" />
            <div class="col-sm-6 col-lg-6 text-left">
                <input type="text" id="txtFecHasta" class="col-sm-4 col-lg-4 form-control text-left" readonly="true" />
            </div>
        </div>
        <div class="pt-2 col-sm-12 col-lg-12 text-center">
            <asp:GridView AllowPaging="true" OnPageIndexChanging="OnPaging" PageSize="5"
                ID="gvBitacora" runat="server" AutoGenerateColumns="false" DataKeyNames="ID"
                AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue"
                BorderColor="Transparent" AutoSizeColumnsMode="Fill" HorizontalAlign="Center"
                CssClass="table table-striped table-condensed table-hover">
                <Columns>
                    <asp:TemplateField HeaderText="Codigo" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="C_ID" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nombre de usuario" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="C_NOM_USU" runat="server" Text='<%# Eval("Usuario.NombreDeUsuario")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha y hora" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="C_FEC_HORA" runat="server" Text='<%# Eval("FechaHora")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Descripción" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="C_DESC" runat="server" Text='<%# Eval("Descripcion")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Es error?" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="C_ES_ERROR" runat="server" Text='<%# Eval("EsError")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="col-sm-12 col-lg-12 text-center pt-2">
            <asp:Label ID="lblError" ClientIDMode="Static" CssClass="control-label" runat="server" />
        </div>
        <div class="col-sm-12 col-lg-12 text-center pt-2">
            <asp:Button ID="B_ACEPTAR" Text="Aceptar" CssClass="btn btn-success" OnClientClick="return Validar()" runat="server" />
        </div>
    </div>
</asp:Content>
