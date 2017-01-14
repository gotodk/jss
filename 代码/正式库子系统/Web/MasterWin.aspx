<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MasterWin.aspx.cs" Inherits="MasterWin"
    EnableEventValidation %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>选择画面</title>
    <base target="_self" />

    <script type="text/javascript" src="../js/common-private.js"></script>

    <script language="javaScript" type="text/javascript">
        function setValue(parmlist)
        {
            var param;
            var aryControl;
            var controlName;
            param = parmlist.split(",");
            for (var i=0; i < param.length; i++)
            {
                aryControl = param[i].split("=");
                if(aryControl.length >= 2)
                {
                    var parentWindow=window.parent.opener;
                    controlName = aryControl[0]
                    
                    //父窗体控件不为空时，则赋值 undefined
                    if( parent.opener.document.getElementById(controlName) != null )
                    {
                        parent.opener.document.getElementById(controlName).value = CheckDateTime(aryControl[1]);
                    }
                }
            }
           window.close();
           return false;
        }
        //恢复查询，不采用条件进行查询
        function cancelQualification()
        {
            document.getElementById('txtFlag').value = '0';
            formsubmit();
        }

        //使用条件查寻
        function executeQualification()
        {
            document.getElementById('txtFlag').value = '1';
            document.getElementById('txtCondition').value = '1';
            formsubmit();
        }
        
        function setDisplay()
        {
            document.getElementById('execQuery').style.display  = 'none';
            document.getElementById('cancelQuery').style.display  = 'none';
        }
    </script>

    <style type="text/css">
    #query{
        position:absolute;
	left:expression((document.body.clientWidth-query.offsetWidth)/2);
	top:72px;
	width:600px;
	height:100%;
	z-index:999;
	display:none;
    }
    </style>
</head>
<body style="margin: 0px;">
    <form id="form1" runat="server">
    <div style="border: 0px; padding: 0px; margin: 0px; overflow: auto; width: 600px;
        height: 450px">
        <radG:RadGrid ID="saleGrid" runat="server" AllowPaging="True" OnPageIndexChanged="RadGrid1_PageIndexChanged"
            GridLines="None" AutoGenerateColumns="False" OnItemDataBound="saleGrid_ItemDataBound"
            AllowSorting="True" PagerStyle-CssClass="GridPager" PagerStyle-Height="20px"
            PagerStyle-PagerTextFormat="{4} &nbsp;|&nbsp;&nbsp;&nbsp;{0}&nbsp;/&nbsp;{1}&nbsp;页;&nbsp;&nbsp;(&nbsp;{2}&nbsp;-{3}&nbsp;)&nbsp;条记录&nbsp;&nbsp;,共{5}条"
            PagerStyle-VerticalAlign="Top" SortingSettings-SortedAscToolTip="升序" SortingSettings-SortedDescToolTip="降序"
            SortingSettings-SortToolTip="按此项排序" Width="800px" Skin="Default" ShowGroupPanel="True"
            MasterTableView-NoMasterRecordsText="没有可显示的记录" GroupPanel-Text="可拖动此项至表格上方的区域进行分组"
            GroupingSettings-GroupContinuesFormatString="接下页..." GroupingSettings-GroupContinuedFormatString="接上页"
            GroupingSettings-GroupSplitDisplayFormat="共有{1}条记录，本页显示{0}条." HierarchySettings-ExpandTooltip="展开"
            HierarchySettings-CollapseTooltip="折叠" GroupingSettings-ExpandTooltip="展开" GroupingSettings-CollapseTooltip="折叠"
            PagerStyle-Mode="NextPrev">
            <ExportSettings>
                <Pdf PageBottomMargin="" PageFooterMargin="" PageHeaderMargin="" PageHeight="11in"
                    PageLeftMargin="" PageRightMargin="" PageTopMargin="" PageWidth="8.5in" />
            </ExportSettings>
            <PagerStyle CssClass="GridPager" Height="30px" Mode="NextPrevAndNumeric" PagerTextFormat="{4} &#160;|&#160;&#160;&#160;{0}&#160;/&#160;{1}&#160;页;&#160;&#160;(&#160;{2}&#160;-{3}&#160;)&#160;条记录&#160;&#160;,共{5}条"
                VerticalAlign="Top" />
            <GroupPanel Text="可拖动此项至表格上方的区域进行分组">
            </GroupPanel>
            <MasterTableView>
                <ExpandCollapseColumn Resizable="False" Visible="False">
                    <HeaderStyle Width="20px" />
                </ExpandCollapseColumn>
                <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px" />
                </RowIndicatorColumn>
                <Columns>
                    <radG:GridTemplateColumn UniqueName="TemplateColumn">
                        <ItemStyle Width="0px" />
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtn" runat="server" ImageUrl="../images/Update.gif" CommandName="ceshi" />
                        </ItemTemplate>
                    </radG:GridTemplateColumn>
                </Columns>
            </MasterTableView>
            <HierarchySettings ExpandTooltip="展开" CollapseTooltip="折叠"></HierarchySettings>
            <ClientSettings AllowColumnsReorder="True">
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
            <SortingSettings SortToolTip="按此项排序" SortedAscToolTip="升序" SortedDescToolTip="降序">
            </SortingSettings>
            <GroupingSettings GroupContinuesFormatString="接下页..." GroupContinuedFormatString="接上页"
                GroupSplitDisplayFormat="共有{1}条记录，本页显示{0}条." ExpandTooltip="展开" CollapseTooltip="折叠">
            </GroupingSettings>
        </radG:RadGrid>
        <table>
            <tr>
                <td>
                    <input type="button" value="条件查询" id="execQuery" onclick="display();" />
                </td>
                <td>
                    <input type="button" value="取消条件查询" id="cancelQuery" onclick="cancelQualification();" />
                </td>
            </tr>
        </table>
    </div>
    <div id="query" runat="server">
    </div>
    <input type="hidden" id="txtFlag" value="0" runat="server" />
    <input type="hidden" id="txtCondition" value="0" runat="server" />
    </form>
</body>
</html>
