<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Diseñar_Espacio.aspx.vb" Inherits="GUI.Diseñar_Espacio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />

    <link rel="stylesheet" href="../../Estilos/jquery-ui.min.css" />
    <script src="../../Scripts/GoJS/jquery.min.js"></script>
    <script src="../../Scripts/GoJS/jquery-ui.min.js"></script>

    <style type="text/css">
        .ui-accordion .ui-accordion-content {
            padding: 1px;
        }
    </style>

    <script src="../../Scripts/GoJS/go-debug.js"></script>
    <script src="../../Scripts/Propios/Diseñar_Espacio.js"></script>

    <div id="sample">
        <div class="container" style="width: 100%; white-space: nowrap;">
            <div id="accordion" class="col-sm-4 col-lg-4">
                <h4><asp:Label ID="L_PRODUC" runat="server"></asp:Label></h4>
                <div>
                    <div id="myPaletteProducts" style="width: 140px; height: 360px"></div>
                </div>
                <h4><asp:Label ID="L_ESP_OCUP" runat="server"></asp:Label></h4>
                <div>
                    <div id="myPaletteOS" style="width: 140px; height: 360px"></div>
                </div>
            </div>
            <div class="col-sm-8 col-lg-8">
                <div id="myDiagramDiv" style="border: solid 1px black; height: 550px; width: 750px "></div>
            </div>
        </div>

    </div>
    <script>
        window.onload = init();
    </script>
  
    </asp:Content>


<%--<asp:Content ID="Content3" ContentPlaceHolderID="CPH2" runat="server">
    
    <br/>
    <br/>

      <div class="container">
         <div class="col-sm-12 col-lg-12 text-center">
                             <div class="form-group">
                    <asp:Label ID="L_Monto" Text="Monto disponible" CssClass="col-sm-6 col-lg-6 control-label text-right" runat="server"></asp:Label>
                    <div class="col-sm-4 col-lg-4">
                        <asp:TextBox ID="txtMonto" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
             </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:GridView AutoSizeColumnsMode="Fill" ID="gvProductos" runat="server" HorizontalAlign="Center"
                OnSelectedIndexChanged="OnSelectedIndexChanged" AutoGenerateColumns="false"
                AlternatingRowStyle-BackColor="PowderBlue" HeaderStyle-BackColor="CornflowerBlue" BorderColor="Transparent"
                AllowPaging="true" OnPageIndexChanging="OnPaging" 
                PageSize="5" EmptyDataText="No records has been added." OnRowDataBound="gvProductos_RowDataBound"
                CssClass="table">
                <Columns>
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
                                        <asp:TemplateField HeaderText="Tipo" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:Label ID="lblTipo" runat="server" ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        </div>

    </asp:Content>--%>



  


