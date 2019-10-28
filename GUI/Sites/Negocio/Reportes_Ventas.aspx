<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Reportes_Ventas.aspx.vb" Inherits="GUI.Reportes_Ventas" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <link rel="stylesheet" href="../../Estilos/jquery-ui.min.css" />
    <script src="../../Scripts/JQuery/jquery-3.2.1.js"></script>
    <script src="../../Scripts/JQuery/jquery-ui-1.12.1.min.js"></script>
    <script type="text/javascript" src="../../Scripts/Propios/Reportes_Ventas.js?Version=2"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfDateFrom" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hfDateTo" ClientIDMode="Static" runat="server" />    
    <div class="container">
        <div class="col-sm-12 col-lg-12 text-center">
            <div class="col-sm-5 col-lg-5"></div>
            <div class="radio col-sm-4 col-lg-4 text-left">
                <asp:RadioButton ID="RB_PROD" Text="Productos" ClientIDMode="Static" GroupName="rbg" Checked="true" CssClass="radio" AutoPostBack="true" runat="server" />
                <asp:RadioButton ID="RB_CLIENT" Text="Clientes" ClientIDMode="Static" GroupName="rbg" CssClass="radio" AutoPostBack="true" runat="server" />
            </div>
            <div class="col-sm-3 col-lg-3"></div>
        </div>
        <div class="col-sm-11 col-lg-11 form-group">
            <asp:Button ID="L_FEC_D" runat="server" Text="Fecha desde" CssClass="col-sm-6 col-lg-6 btn btn-light text-right" Enabled="false" />
            <div class="col-sm-6 col-lg-6 text-left">
                <input type="text" id="txtFecDesde" class="col-sm-4 col-lg-4 form-control text-left" readonly="true" />
            </div>
        </div>
        <div class="col-sm-11 col-lg-11 form-group">
            <asp:Button ID="L_FEC_H" runat="server" Text="Fecha hasta" CssClass="col-sm-6 col-lg-6 btn btn-light text-right" Enabled="false" />
            <div class="col-sm-6 col-lg-6 text-left">
                <input type="text" id="txtFecHasta" class="col-sm-4 col-lg-4 form-control text-left" readonly="true" />
            </div>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Label ID="lblError" ClientIDMode="Static" CssClass="control-label" runat="server" />
        </div>
        <div class="pt-2 col-sm-12 col-lg-12 text-center">
            <asp:Button ID="B_ACEPTAR" Text="Aceptar" CssClass="btn btn-success" OnClientClick="return ValidarListar();" runat="server" />
        </div>
        <div class="col-sm-12 col-lg-12 text-center text-info">
            <asp:Label ID="L_REP_CLI" Visible="false" runat="server" />
        </div>
        <div class="col-sm-12 col-lg-12 text-center text-info">
            <asp:Label ID="L_REP_PROD" Visible="false" runat="server" />
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Chart ID="GraficoTorta" runat="server" ViewStateMode="Enabled" Width="600px">
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                </ChartAreas>
                <Legends>
                    <asp:Legend Name="Legend1">
                    </asp:Legend>
                </Legends>
            </asp:Chart>
        </div>
    </div>
</asp:Content>
