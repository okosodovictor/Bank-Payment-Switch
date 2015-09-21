<%@ Page Title="" Language="C#" MasterPageFile="~/BankOne.Master" AutoEventWireup="true" CodeBehind="AddTransactionType.aspx.cs" Inherits="BankSwitch.Web.TransactionTypeManagement.AddTransactionType" %>
   <%@ Register Assembly="AppZoneUI.Framework" Namespace="AppZoneUI.Framework" TagPrefix="cc1" %>
  <%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div>
    <ext:ResourceManager ID="ScriptManager1" runat="server" />
    <cc1:EntityUIControl ID="EntityUIControl" runat="server" UIType=" BankSwitch.UI.TransactionTypeManagement.AddTransactionType, BankSwitch.UI"> </cc1:EntityUIControl>
  </div>
</asp:Content>