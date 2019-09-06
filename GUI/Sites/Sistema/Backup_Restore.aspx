<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Sites/MasterPage.Master" CodeBehind="Backup_Restore.aspx.vb" Inherits="GUI.Backup_Restore" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <script type="text/javascript" src="../../Scripts/Propios/BackupRestore.js"></script>
    <asp:HiddenField ID="hfLeyendasIdiomaActual" ClientIDMode="Static" runat="server" />
    <div class="container">
        <div class="col-sm-12 col-lg-12 text-center">
            <div class="col-sm-5 col-lg-5"></div>
            <div class="radio col-sm-5 col-lg-5 text-left">
                <asp:RadioButton ID="RB_BACKUP" Text="Resguardar" Checked="true" GroupName="rbg" ClientIDMode="Static" CssClass="radio" runat="server" />
                <asp:RadioButton ID="RB_RESTORE" Text="Restaurar" GroupName="rbg" ClientIDMode="Static" CssClass="radio" runat="server" />
            </div>
            <div class="col-sm-2 col-lg-2"></div>
        </div>
        <div class="col-sm-12 col-lg-12 text-center">
            <div class="col-sm-4 col-lg-4"></div>
            <div class="col-sm-8 col-lg-8 text-left">
                <asp:ListBox ID="lbBackups" ClientIDMode="Static" runat="server" Width="40%"></asp:ListBox>
            </div>
        </div>
        <div class="col-sm-12 col-lg-12 text-center pt-2">
            <div class="col-sm-4 col-lg-4"></div>
            <div class="col-sm-4 col-lg-4 text-left">
                <asp:Label ID="lblError" ClientIDMode="Static" runat="server" />
            </div>
            <div class="col-sm-4 col-lg-4"></div>
        </div>
        <div class="col-sm-12 col-lg-12 text-center pt-2">
            <div class="col-sm-5 col-lg-5"></div>
            <div class="col-sm-5 col-lg-5 text-left">
                <asp:Button ID="B_ACEPTAR" CssClass="btn btn-success" runat="server" Text="Aceptar" OnClientClick="return Validar()" />
            </div>
            <div class="col-sm-2 col-lg-2"></div>
        </div>
    </div>
</asp:Content>

