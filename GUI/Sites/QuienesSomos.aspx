<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="QuienesSomos.aspx.vb" Inherits="GUI.QuienesSomos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH" runat="server">
    <div class="container">
        <div class="col-sm-12 col-lg-12 text-center">
            <h1>
                <asp:Label ID="L_QUIEN" runat="server" />
            </h1>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Label ID="L_QUIEN_T" CssClass="text-justify text-info" runat="server" />
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <h1>
                <asp:Label ID="L_MISION" runat="server" />
            </h1>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Label ID="L_MISION_T" CssClass="text-justify text-info" runat="server" />
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <h1>
                <asp:Label ID="L_VISION" runat="server" />
            </h1>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <asp:Label ID="L_VISION_T" CssClass="text-justify text-info" runat="server" />
        </div>
    </div>
</asp:Content>
