<%@ Page Title="" Language="C#" MasterPageFile="~/BankOne.Master" AutoEventWireup="true" CodeBehind="ViewFeeList.aspx.cs" Inherits="BankSwitch.Web.FeeManagement.ViewFeeList" %>
  <%@ Register Assembly="AppZoneUI.Framework" Namespace="AppZoneUI.Framework" TagPrefix="cc1" %>
  <%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div>
    <ext:ResourceManager ID="ScriptManager1" runat="server" />
    <cc1:EntityUIControl ID="EntityUIControl" runat="server" UIType=" BankSwitch.UI.FeeManagement.ViewFeeList, BankSwitch.UI"> </cc1:EntityUIControl>
  </div>
</asp:Content>
