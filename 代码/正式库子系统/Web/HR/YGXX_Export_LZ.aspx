<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YGXX_Export_LZ.aspx.cs" Inherits="Web_HR_YGXX_Export_LZ" %>

<%@ Register assembly="RadTabStrip.Net2" namespace="Telerik.WebControls" tagprefix="radTS" %>
<%@ Register Assembly="Infragistics2.WebUI.UltraWebGrid.ExcelExport.v8.1, Version=8.1.20081.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" TagPrefix="igxl" %>
<%@ Register Assembly="Infragistics2.WebUI.UltraWebGrid.v8.1, Version=8.1.20081.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid" TagPrefix="igtbl" %>   

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>离职人员信息导出</title>
     <link href="../../css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Default2006" 
        Height="16px" Width="980px">
     <Tabs>
     <radTS:Tab ID="Tab1" runat="server" Text="离职人员信息导出"></radTS:Tab>
     </Tabs>
     </radTS:RadTabStrip>
     
     
     <table style="width: 1050px">          
          <tr><td align="right">导出离职人员基本信息</td><td align="right" style="width:50%"><asp:Button 
            ID="btnOut" runat="server" onclick="Button1_Click" Text="导出到Excel" Visible="True" 
            Width="120px" />&nbsp;&nbsp;<asp:Button ID="Button1" runat="server" onclick="Button2_Click" Text="返回上一页" />
                      </td></tr></table> 
     
    
        <igtbl:UltraWebGrid ID="UltraWebGrid1" runat="server" Height="450px" 
            Width="1050px" DataSourceID="SqlDataSource1">
            <Bands>
                <igtbl:UltraGridBand>
                    <AddNewRow View="NotSet" Visible="NotSet">
                    </AddNewRow>
                    <Columns>
                        <igtbl:UltraGridColumn BaseColumnName="员工编号" IsBound="True" Key="员工编号">
                            <Header Caption="员工编号">
                            </Header>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="姓名" IsBound="True" Key="姓名">
                            <Header Caption="姓名">
                                <RowLayoutColumnInfo OriginX="1" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="1" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="性别" IsBound="True" Key="性别">
                            <Header Caption="性别">
                                <RowLayoutColumnInfo OriginX="2" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="2" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        
                         <igtbl:UltraGridColumn BaseColumnName="出生日期" DataType="System.DateTime" 
                            IsBound="True" Key="出生日期">
                            <Header Caption="出生日期">
                                <RowLayoutColumnInfo OriginX="6" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="6" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        
                         <igtbl:UltraGridColumn BaseColumnName="身份证号" IsBound="True" Key="身份证号">
                            <Header Caption="身份证号">
                                <RowLayoutColumnInfo OriginX="5" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="5" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        
                         <igtbl:UltraGridColumn BaseColumnName="现住址" IsBound="True" Key="现住址">
                            <Header Caption="现住址">
                                <RowLayoutColumnInfo OriginX="21" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="21" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        
                         <igtbl:UltraGridColumn BaseColumnName="入职前培训期结束时间" DataType="System.DateTime" 
                            IsBound="True" Key="入职前培训期结束时间">
                            <Header Caption="入职前培训期结束时间">
                                <RowLayoutColumnInfo OriginX="34" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="34" />
                            </Footer>
                        </igtbl:UltraGridColumn>  
                        
                        <igtbl:UltraGridColumn BaseColumnName="人员类别" IsBound="True" Key="人员类别">
                            <Header Caption="人员类别">
                                <RowLayoutColumnInfo OriginX="27" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="27" />
                            </Footer>
                        </igtbl:UltraGridColumn>                      
                        
                         <igtbl:UltraGridColumn BaseColumnName="离职时间" 
                            IsBound="True" Key="离职时间">
                            <Header Caption="离职时间">
                                <RowLayoutColumnInfo OriginX="41" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="41" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        
                        <igtbl:UltraGridColumn BaseColumnName="所属部门" IsBound="True" Key="所属部门">
                            <Header Caption="离职前所属部门">
                                <RowLayoutColumnInfo OriginX="3" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="3" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="岗位名称" IsBound="True" Key="岗位名称">
                            <Header Caption="离职前岗位名称">
                                <RowLayoutColumnInfo OriginX="4" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="4" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                       
                        <igtbl:UltraGridColumn BaseColumnName="民族" IsBound="True" Key="民族">
                            <Header Caption="民族">
                                <RowLayoutColumnInfo OriginX="7" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="7" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="政治面貌" IsBound="True" Key="政治面貌">
                            <Header Caption="政治面貌">
                                <RowLayoutColumnInfo OriginX="8" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="8" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="入党时间" IsBound="True" Key="入党时间">
                            <Header Caption="入党时间">
                                <RowLayoutColumnInfo OriginX="9" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="9" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="学历" IsBound="True" Key="学历">
                            <Header Caption="学历">
                                <RowLayoutColumnInfo OriginX="10" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="10" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="学位" IsBound="True" Key="学位">
                            <Header Caption="学位">
                                <RowLayoutColumnInfo OriginX="11" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="11" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="专业" IsBound="True" Key="专业">
                            <Header Caption="专业">
                                <RowLayoutColumnInfo OriginX="12" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="12" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="毕业院校" IsBound="True" Key="毕业院校">
                            <Header Caption="毕业院校">
                                <RowLayoutColumnInfo OriginX="13" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="13" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="教育类型" IsBound="True" Key="教育类型">
                            <Header Caption="教育类型">
                                <RowLayoutColumnInfo OriginX="14" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="14" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="外语语种" IsBound="True" Key="外语语种">
                            <Header Caption="外语语种">
                                <RowLayoutColumnInfo OriginX="15" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="15" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="外语水平" IsBound="True" Key="外语水平">
                            <Header Caption="外语水平">
                                <RowLayoutColumnInfo OriginX="16" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="16" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="计算机水平" IsBound="True" Key="计算机水平">
                            <Header Caption="计算机水平">
                                <RowLayoutColumnInfo OriginX="17" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="17" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="职称" IsBound="True" Key="职称">
                            <Header Caption="职称">
                                <RowLayoutColumnInfo OriginX="18" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="18" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="籍贯" IsBound="True" Key="籍贯">
                            <Header Caption="籍贯">
                                <RowLayoutColumnInfo OriginX="19" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="19" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="户口所在地" IsBound="True" Key="户口所在地">
                            <Header Caption="户口所在地">
                                <RowLayoutColumnInfo OriginX="20" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="20" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                       
                        <igtbl:UltraGridColumn BaseColumnName="家庭住址" IsBound="True" Key="家庭住址">
                            <Header Caption="家庭住址">
                                <RowLayoutColumnInfo OriginX="22" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="22" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="邮编" IsBound="True" Key="邮编">
                            <Header Caption="邮编">
                                <RowLayoutColumnInfo OriginX="23" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="23" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="个人电话" IsBound="True" Key="个人电话">
                            <Header Caption="个人电话">
                                <RowLayoutColumnInfo OriginX="24" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="24" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="家庭电话" IsBound="True" Key="家庭电话">
                            <Header Caption="家庭电话">
                                <RowLayoutColumnInfo OriginX="25" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="25" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="婚姻状况" IsBound="True" Key="婚姻状况">
                            <Header Caption="婚姻状况">
                                <RowLayoutColumnInfo OriginX="26" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="26" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        
                        <igtbl:UltraGridColumn BaseColumnName="工资档级" IsBound="True" Key="工资档级">
                            <Header Caption="工资档级">
                                <RowLayoutColumnInfo OriginX="28" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="28" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="标准工资" DataType="System.Double" 
                            IsBound="True" Key="标准工资">
                            <Header Caption="标准工资">
                                <RowLayoutColumnInfo OriginX="29" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="29" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="是否干部" IsBound="True" Key="是否干部">
                            <Header Caption="是否干部">
                                <RowLayoutColumnInfo OriginX="30" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="30" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="用工性质" IsBound="True" Key="用工性质">
                            <Header Caption="用工性质">
                                <RowLayoutColumnInfo OriginX="31" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="31" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="员工状态" IsBound="True" Key="员工状态">
                            <Header Caption="员工状态">
                                <RowLayoutColumnInfo OriginX="32" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="32" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="入职前培训期开始时间" DataType="System.DateTime" 
                            IsBound="True" Key="入职前培训期开始时间">
                            <Header Caption="入职前培训期开始时间">
                                <RowLayoutColumnInfo OriginX="33" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="33" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                       
                        <igtbl:UltraGridColumn BaseColumnName="考察期开始时间" DataType="System.DateTime" 
                            IsBound="True" Key="考察期开始时间">
                            <Header Caption="考察期开始时间">
                                <RowLayoutColumnInfo OriginX="35" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="35" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="考察期结束时间" DataType="System.DateTime" 
                            IsBound="True" Key="考察期结束时间">
                            <Header Caption="考察期结束时间">
                                <RowLayoutColumnInfo OriginX="36" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="36" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="转正时间" DataType="System.DateTime" 
                            IsBound="True" Key="转正时间">
                            <Header Caption="转正时间">
                                <RowLayoutColumnInfo OriginX="37" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="37" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="劳动合同开始时间" DataType="System.DateTime" 
                            IsBound="True" Key="劳动合同开始时间">
                            <Header Caption="劳动合同开始时间">
                                <RowLayoutColumnInfo OriginX="38" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="38" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="劳动合同结束时间" DataType="System.DateTime" 
                            IsBound="True" Key="劳动合同结束时间">
                            <Header Caption="劳动合同结束时间">
                                <RowLayoutColumnInfo OriginX="39" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="39" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="合同期限" IsBound="True" Key="合同期限">
                            <Header Caption="合同期限">
                                <RowLayoutColumnInfo OriginX="40" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="40" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                      
                        <igtbl:UltraGridColumn BaseColumnName="是否加入工会" IsBound="True" Key="是否加入工会">
                            <Header Caption="是否加入工会">
                                <RowLayoutColumnInfo OriginX="42" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="42" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                    </Columns>
                </igtbl:UltraGridBand>
            </Bands>
            <DisplayLayout AllowColSizingDefault="Free" AllowColumnMovingDefault="OnServer" 
                AllowDeleteDefault="Yes" AllowSortingDefault="OnClient" 
                AllowUpdateDefault="Yes" BorderCollapseDefault="Separate" 
                HeaderClickActionDefault="SortMulti" Name="UltraWebGrid1" 
                RowHeightDefault="20px" RowSelectorsDefault="No" 
                SelectTypeRowDefault="Extended" StationaryMargins="Header" 
                StationaryMarginsOutlookGroupBy="True" TableLayout="Fixed" Version="4.00" 
                ViewType="OutlookGroupBy">
                <FrameStyle BackColor="Window" BorderColor="InactiveCaption" 
                    BorderStyle="Solid" BorderWidth="1px" Font-Names="Microsoft Sans Serif" 
                    Font-Size="8.25pt" Height="451px" Width="1050px">
                </FrameStyle>
                <Pager MinimumPagesForDisplay="2">
                    <PagerStyle BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                        WidthTop="1px" />
                    </PagerStyle>
                </Pager>
                <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                </EditCellStyleDefault>
                <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                        WidthTop="1px" />
                </FooterStyleDefault>
                <HeaderStyleDefault BackColor="LightGray" BorderStyle="Solid" 
                    HorizontalAlign="Left">
                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                        WidthTop="1px" />
                </HeaderStyleDefault>
                <RowStyleDefault BackColor="Window" BorderColor="Silver" BorderStyle="Solid" 
                    BorderWidth="1px" Font-Names="Microsoft Sans Serif" Font-Size="8.25pt">
                    <Padding Left="3px" />
                    <BorderDetails ColorLeft="Window" ColorTop="Window" />
                </RowStyleDefault>
                <GroupByRowStyleDefault BackColor="Control" BorderColor="Window">
                </GroupByRowStyleDefault>
                <GroupByBox>
                    <BoxStyle BackColor="ActiveBorder" BorderColor="Window">
                    </BoxStyle>
                </GroupByBox>
                <AddNewBox Hidden="False">
                    <BoxStyle BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid" 
                        BorderWidth="1px">
                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                            WidthTop="1px" />
                    </BoxStyle>
                </AddNewBox>
                <ActivationObject BorderColor="" BorderWidth="">
                </ActivationObject>
                <FilterOptionsDefault AllowRowFiltering="OnClient" FilterUIType="FilterRow">
                    <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" 
                        BorderWidth="1px" CustomRules="overflow:auto;" 
                        Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px" Height="300px" 
                        Width="200px">
                        <Padding Left="2px" />
                    </FilterDropDownStyle>
                    <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                    </FilterHighlightRowStyle>
                    <FilterOperandDropDownStyle BackColor="White" BorderColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px" CustomRules="overflow:auto;" 
                        Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px">
                        <Padding Left="2px" />
                    </FilterOperandDropDownStyle>
                </FilterOptionsDefault>
            </DisplayLayout>
        </igtbl:UltraWebGrid>
         
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:FMOPConn %>" 
        SelectCommand="SELECT 员工编号,姓名,性别,所属部门,岗位名称,身份证号,convert(varchar(10),出生日期,120) as 出生日期,民族,政治面貌,入党时间,学历,学位,专业,毕业院校,教育类型,外语语种,外语水平,计算机水平,职称,籍贯,户口所在地,现住址,家庭住址,邮编,个人电话,家庭电话,婚姻状况,人员类别,工资档级,标准工资,是否干部,用工性质,员工状态,convert(varchar(10),入职前培训期开始时间,120) as 入职前培训期开始时间,convert(varchar(10),入职前培训期结束时间,120) as 入职前培训期结束时间,convert(varchar(10),考察期开始时间,120) as 考察期开始时间,convert(varchar(10),考察期结束时间,120) as 考察期结束时间,convert(varchar(10),转正时间,120) as 转正时间,convert(varchar(10),劳动合同开始时间,120) as 劳动合同开始时间,convert(varchar(10),劳动合同结束时间,120) as 劳动合同结束时间,合同期限,convert(varchar(10),离职时间,120) as 离职时间,是否加入工会 FROM [HR_Employees_YGXXExport] where 员工状态 like '%离职%'"></asp:SqlDataSource>         
    <igxl:UltraWebGridExcelExporter ID="UltraWebGridExcelExporter1" runat="server">
    </igxl:UltraWebGridExcelExporter>
    </form>
</body>
</html>
