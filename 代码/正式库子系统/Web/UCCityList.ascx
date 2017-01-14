<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCCityList.ascx.cs" Inherits="FWPTZS_UCCityList" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="Ajx" %>
<Ajx:ScriptManager ID="ScriptManager1" runat="server">
</Ajx:ScriptManager>
<Ajx:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:DropDownList ID="CityList_Promary" runat="server" OnSelectedIndexChanged="CityList_Promary_SelectedIndexChanged"
            AutoPostBack="True">
        </asp:DropDownList>
        <asp:DropDownList ID="CityList_City" runat="server" OnSelectedIndexChanged="CityList_City_SelectedIndexChanged"
            AutoPostBack="True">
        </asp:DropDownList>
        <asp:DropDownList ID="CityList_qu" runat="server" OnSelectedIndexChanged="CityList_qu_SelectedIndexChanged">
        </asp:DropDownList>
    </ContentTemplate>
    <Triggers>
        <Ajx:AsyncPostBackTrigger ControlID="CityList_Promary" />
        <Ajx:AsyncPostBackTrigger ControlID="CityList_City" />
    </Triggers>
</Ajx:UpdatePanel>
