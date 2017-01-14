<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NBYGJJRFZZBB.aspx.cs" Inherits="Web_siyuefenkaohe_NBYGJJRFZZBB" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>内部员工经纪人发展周报表</title>
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
                <radTS:Tab ID="Tab1" runat="server" Text="分公司经纪人发展日报表" NavigateUrl="http://192.168.0.10:9111/Web/siyuefenkaohe/FGSYJRB_New.aspx">
                </radTS:Tab>
                <radTS:Tab ID="Tab2" runat="server" Text="内部员工经纪人发展周报表" NavigateUrl="http://192.168.0.10:9111/Web/siyuefenkaohe/NBYGJJRFZZBB.aspx" ForeColor="Red" >
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
        <br />
        <br />
        <table width="100%">
            <tr>
                <td align="center" style="font-size: 12pt; font-weight: bold; height: 30px">
                    内部员工经纪人发展周报表
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="1px" bordercolor="#000000" style="word-break: break-all;
                        border-style: solid; border-collapse: collapse; text-align: center; line-height: 25px;
                        padding: 3px;" rules="all" class="GridViewStyle" align="center" cellpadding="0"
                        cellspacing="0">
                        <tr>
                            <td rowspan="3" nowrap="nowrap" width="80px">
                                员工姓名
                            </td>
                            <td colspan="6" nowrap="nowrap">
                                经纪人发展奖励情况
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
                            <td colspan="2" nowrap="nowrap">
                                经纪人类别
                            </td>
                            <td width="80px" rowspan="2" nowrap="nowrap" style="line-height: 18px">
                                有效经纪人<br />
                                是否≥30个
                            </td>
                            <td width="90px" rowspan="2" nowrap="nowrap" style="line-height: 18px">
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
                            <td nowrap="nowrap" width="70px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="70px">
                                本周新增
                            </td>
                            <td nowrap="nowrap" width="70px">
                                自然人
                            </td>
                            <td nowrap="nowrap" width="70px">
                                单位
                            </td>
                            <td nowrap="nowrap" width="50px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="60px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="50px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="60px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="50px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="60px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="50px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="60px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="50px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="60px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="50px">
                                卖出
                            </td>
                            <td nowrap="nowrap" width="50px">
                                买入
                            </td>
                            <td nowrap="nowrap" width="50px">
                                卖出
                            </td>
                            <td nowrap="nowrap" width="50px">
                                买入
                            </td>
                            <td nowrap="nowrap" width="50px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="60px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="50px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="60px">
                                今日新增
                            </td>
                            <td nowrap="nowrap" width="50px">
                                累计
                            </td>
                            <td nowrap="nowrap" width="60px">
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
                                        <%#Eval("员工姓名")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("累计")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("本周新增")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("自然人")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("单位")%>
                                    </td>
                                    <td nowrap="nowrap">
                                        <%#Eval("超过三十个")%>
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
                    1、员工排序按"奖励计提金额"中的"累计"项由高至低排序；<br />
                    2、有效经纪人的判定： 1）资质齐全； 2）每个经纪人账户下至少虚拟一个交易方开户，该交易方要至少完成2次卖出和2次买入的交易。<br />
                    3、内部员工计提标准：按照富美字【40】号文规定，经纪人为“自然人”奖励20元，“单位”奖励50元。<br />
                    4、8月1日—10月31日总部员工每人必须最少完成30个“有效经纪人”的注册和模拟演练，总部员工本人注册的不计入数量。<br />各部门负责人与分管领导为第一责任人，必须按照公司要求按时按量完成；不能完成的，部门负责人10月份的绩效成绩予以否决。<br />
                    5、内部员工发展经纪人，须填写《员工发展经纪人信息表》，以部门为单位每周一报备一次，发送至业务拓展部进行汇总，每周更新一次。<br />
                    6、总部将对所有经纪人的身份进行全检回访，发现作假的，将扣罚所有奖金。<br />
                    7、“奖励政策”部分，员工发展的“有效经纪人”除按照原有规定享受奖励外，待平台正式上线后，另可享受以下收益。<br />
                    计算方式如下：员工个人发展的所有经纪人名下买家卖家当月卖出金额×0.06%＋买入金额×0.10%。此部分收益由财务中心随当月工资一同发放。
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
