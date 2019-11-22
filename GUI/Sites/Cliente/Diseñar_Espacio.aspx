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
    <script src="../../Scripts/Propios/Diseñar_Espacio.js?Version=5"></script>
    

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
                <div id="myProductInfo"></div>                
            </div>
            <div class="col-sm-8 col-lg-8">
                <div id="myDiagramDiv" style="border: solid 1px black; height: 550px; width: 750px "></div>
            </div>
            <div class="col-sm-12 col-lg-12" style="text-align:center; padding-top:1rem;">
                   <button type="button" class="btn-test btn btn-success">Calcular</button> 
            </div>
        </div>

    </div>
   

    <script>
        window.onload = init();
    </script>
 
     <div class="col-sm-12 col-lg-12 text-center">

    <asp:Button ID="B_LIMPIAR" Text="Limpiar" CssClass="btn btn-info" runat="server"></asp:Button>
        <asp:Button ID="B_EXPORTP" Text="Exportar" CssClass="btn btn-danger" runat="server" OnClientClick="return imprimir();"></asp:Button>
    <asp:button id="B_ACEPTAR" class="btn btn-test" runat="server" OnClick="asdf_Click" OnClientClick="return calcular();" Text="Aceptar" style="display:none;"></asp:button>
    
         
         <br/>

     </div>
    <asp:HiddenField ID="hf_producto" runat="server" Value="" />
</asp:Content>





  


