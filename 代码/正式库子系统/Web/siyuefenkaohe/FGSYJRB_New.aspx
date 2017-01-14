<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FGSYJRB_New.aspx.cs" Inherits="Web_siyuefenkaohe_FGSYJRB_New" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>分公司业绩日报</title>
    <link type="text/css" rel="Stylesheet" href="/css/style.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Default2006" Height="16px"
            Width="980px">
            <Tabs>
                <radTS:Tab ID="Tab3" runat="server" Text="分公司业绩日报" NavigateUrl="http://192.168.0.10/Web/siyuefenkaohe/FGSYJRB.aspx">
                </radTS:Tab>
                <radTS:Tab ID="Tab1" runat="server" Text="分公司经纪人发展日报表" ForeColor="Red" NavigateUrl="http://192.168.0.10:9111/Web/siyuefenkaohe/FGSYJRB_New.aspx">
                </radTS:Tab>
                <radTS:Tab ID="Tab2" runat="server" Text="内部员工经纪人发展周报表" NavigateUrl="http://192.168.0.10:9111/Web/siyuefenkaohe/NBYGJJRFZZBB.aspx">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
        <br />
        <br />
        <table width="100%">
            <tr>
                <td align="center" style="font-size: 12pt; font-weight: bold; height: 30px">
                    分公司业绩日报
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="1px" bordercolor="#000000" style="word-break: break-all;
                        border-style: solid; border-collapse: collapse; text-align: center; line-height: 25px;
                        padding: 3px;" rules="all" class="GridViewStyle" align="center" cellpadding="0"
                        cellspacing="0">
                        <tr>
                            <td rowspan="3" nowrap="nowrap" width="120px">
                                分公司
                            </td>
                            <td colspan="12" nowrap="nowrap">
                                经纪人发展奖励情况（不含总部员工发展）
                            </td>
                            <td colspan="8" nowrap="nowrap">
                                技术服务费情况
                            </td>
                            <td colspan="2" rowspan="2" nowrap="nowrap">
                                产生交易商品数
                            </td>
                            <td colspan="2" nowrap="nowrap">
                                累计交易额来源构成
                            </td>
                            <td colspan="4" nowrap="nowrap">
                                交易额情况
                            </td>
                            <td colspan="4" nowrap="nowrap">
                                产生交易的县市区个数
                            </td>
                            <td colspan="4" nowrap="nowrap">
                                经纪人收益情况
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" nowrap="nowrap">
                                有效经纪人总数
                            </td>
                            <td colspan="5" nowrap="nowrap">
                                经纪人类别
                            </td>
                            <td width="80px" rowspan="2" nowrap="nowrap" style="line-height: 18px">
                                自然人经纪人<br />
                                是否超标
                            </td>
                            <td width="60px" rowspan="2" nowrap="nowrap" style="line-height: 18px">
                                调研报告<br />
                                是否合格
                            </td>
                            <td width="80px" rowspan="2" nowrap="nowrap" style="line-height: 18px">
                                有效经纪人<br />
                                在500个以下
                            </td>
                            <td width="80px" rowspan="2" nowrap="nowrap" style="line-height: 18px">
                                有效经纪人<br />
                                在1500个以下
                            </td>
                            <td width="80px" rowspan="2" nowrap="nowrap" style="line-height: 18px">
                                奖励总额<br />
                                （元）
                            </td>
                            <td colspan="2" nowrap="nowrap">
                                计提来源经纪人数
                            </td>
                            <td colspan="2" nowrap="nowrap">
                                计提来源卖家数
                            </td>
                            <td colspan="2" nowrap="nowrap">
                                计提来源买家数
                            </td>
                            <td colspan="2" nowrap="nowrap">
                                计提金额
                            </td>
                            <td rowspan="2" nowrap="nowrap" width="50px">
                                前五名<br />
                                商品
                            </td>
                            <td rowspan="2" nowrap="nowrap" style="line-height: 18px" width="50px">
                                前五名<br />
                                县市区
                            </td>
                            <td colspan="2" nowrap="nowrap">
                                累计
                            </td>
                            <td colspan="2" nowrap="nowrap">
                                今日新增
                            </td>
                            <td colspan="2" nowrap="nowrap">
                                卖家县市区
                            </td>
                            <td colspan="2" nowrap="nowrap">
                                买家县市区
                            </td>
                            <td colspan="2" nowrap="nowrap">
                                经纪人收益金额
                            </td>
                            <td colspan="2" nowrap="nowrap">
                                产生收益的经纪人数量
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" width="60px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="60px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="60px">
                                自然人
                            </td>
                            <td nowrap="nowrap" width="60px">
                                单位
                            </td>
                            <td nowrap="nowrap" width="60px">
                                行业协会
                            </td>
                            <td nowrap="nowrap" width="60px">
                                媒体
                            </td>
                            <td nowrap="nowrap" width="60px">
                                银行
                            </td>
                            <td nowrap="nowrap" width="40px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="55px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="40px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="55px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="40px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="55px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="40px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="55px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="40px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="55px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="40px">
                                卖出
                            </td>
                            <td nowrap="nowrap" width="40px">
                                买入
                            </td>
                            <td nowrap="nowrap" width="40px">
                                卖出
                            </td>
                            <td nowrap="nowrap" width="40px">
                                买入
                            </td>
                            <td nowrap="nowrap" width="40px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="55px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="40px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="55px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="40px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="55px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="50px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="60px">
                                今日新增
                            </td>
                        </tr>
                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td nowrap="nowrap">
                                        <%#Eval("分公司")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("累计")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("今日新增")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("自然人")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("单位")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("行业协会")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("媒体")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("银行")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("自然人超标")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("调研报告合格")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("五百以下")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("一千五以下")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("奖励总额")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                    <td nowrap="nowrap">
                                        0
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="font-size: 10pt; line-height: 20px">
                    说明：<br />
                    1、分公司排序按"奖励总额"项由高至低排序；
                    <br />
                    2、有效经纪人的判定：1）资质齐全； 2）每个经纪人账户下至少虚拟一个交易方开户，关联交易方要至少完成2次卖出和2次买入的交易。<br />
                    3、分公司计提标准（总收益，非经纪人收益）：按照富美字【39】号文规定，经纪人为“自然人”奖励20元，“单位”奖励50元，“行业协会”奖励70元，“媒体”奖励100元，“银行”奖励150元。<br />
                    4、每个分公司的有效经纪人奖励上限数量累计为5000个（含）（按时间先后，5000个之后的不再奖励）；同时，自然人数量占比不得高于85%；自然人经纪人比重超标的和调研报告不合格的，奖励标准减半；1500个以下的奖励为0，分公司负责人予以免职。<br />
                    5、涉及的累计数量、自然人、单位、行业协会、媒体、银行数量，均显示实际数量。但实际计算奖励，以前5000个计算。<br />
                    6、总部将对所有经纪人的身份进行全检回访，发现作假的，将扣罚所有奖金。
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
