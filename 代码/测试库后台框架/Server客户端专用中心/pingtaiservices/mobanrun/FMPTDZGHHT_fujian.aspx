<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FMPTDZGHHT_fujian.aspx.cs" Inherits="pingtaiservices_mobanrun_FMPTDZGHHT_fujian" %>
<form id="form1" runat="server">
    <p>&nbsp;</p>
<p><strong>合同附件：</strong></p>
<p><strong>卖方开户相关资质：</strong></p>
<table width="100%" border="0">
  <tr runat="server" id="s1">
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Seller_YYZZ" href="javascript:void(0)" showtupian="">营业执照</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Seller_ZZJGDMZ" href="javascript:void(0)" showtupian="">组织机构代码证</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Seller_SWDJZ" href="javascript:void(0)" showtupian="">税务登记证</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Seller_FDDBRSQS" href="javascript:void(0)" showtupian="">法定代表人授权书</a></td>
  </tr>
  <tr runat="server" id="s2">
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Seller_KHXKZ" href="javascript:void(0)" showtupian="">开户许可证</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Seller_YLYJK" href="javascript:void(0)" showtupian="">预留印鉴表</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Seller_FDDBRSFZ" href="javascript:void(0)" showtupian="">法定代表人身份证正面</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Seller_FDDBRSFZ_FM" href="javascript:void(0)" showtupian="">法定代表人身份证反面</a></td>
  </tr>
  <tr  runat="server" id="s3">
    <td width="25%" align="left" valign="middle">&nbsp;</td>
    <td width="25%" align="left" valign="middle">&nbsp;</td>
    <td width="25%" align="left" valign="middle">&nbsp;</td>
    <td width="25%" align="left" valign="middle">&nbsp;</td>
  </tr>
 <tr  runat="server" id="s4">
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Seller_SFZ" href="javascript:void(0)" showtupian="">身份证正面</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Seller_SFZ_FM" href="javascript:void(0)" showtupian="">身份证反面</a></td>
    <td width="25%" align="left" valign="middle">&nbsp;</td>
    <td width="25%" align="left" valign="middle">&nbsp;</td>
  </tr>
</table>
        <p>&nbsp;</p>
<p><strong>买方开户相关资质：</strong></p>
<table width="100%" border="0">
  <tr  runat="server" id="b1">
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Buyer_YYZZ" href="javascript:void(0)" showtupian="">营业执照</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Buyer_ZZJGDMZ" href="javascript:void(0)" showtupian="">组织机构代码证</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Buyer_SWDJZ" href="javascript:void(0)" showtupian="">税务登记证</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Buyer_FDDBRSQS" href="javascript:void(0)" showtupian="">法定代表人授权书</a></td>
  </tr>
  <tr  runat="server" id="b2">
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Buyer_KHXKZ" href="javascript:void(0)" showtupian="">开户许可证</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Buyer_YLYJK" href="javascript:void(0)" showtupian="">预留印鉴表</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Buyer_FDDBRSFZ" href="javascript:void(0)" showtupian="">法定代表人身份证正面</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Buyer_FDDBRSFZ_FM" href="javascript:void(0)" showtupian="">法定代表人身份证反面</a></td>
  </tr>
  <tr  runat="server" id="b3">
    <td width="25%" align="left" valign="middle">&nbsp;</td>
    <td width="25%" align="left" valign="middle">&nbsp;</td>
    <td width="25%" align="left" valign="middle">&nbsp;</td>
    <td width="25%" align="left" valign="middle">&nbsp;</td>
  </tr>
 <tr  runat="server" id="b4">
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Buyer_SFZ" href="javascript:void(0)" showtupian="">身份证正面</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_Buyer_SFZ_FM" href="javascript:void(0)" showtupian="">身份证反面</a></td>
    <td width="25%" align="left" valign="middle">&nbsp;</td>
    <td width="25%" align="left" valign="middle">&nbsp;</td>
  </tr>
</table>
    <p>&nbsp;</p>
<p><strong>投标单：</strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a runat="server" id="tbdxz" href="javascript:void(0)" showkjdy="">投标单下载</a></p>
<p>

    <table width="100%" border="0" cellspacing="1" bgcolor="#cccccc" style=" line-height:200%">
  <tr>
    <td colspan="2" align="left" valign="top" bgcolor="#FFFFFF">卖方名称：<asp:Label ID="Label1" runat="server"></asp:Label>
&nbsp;发布时间：<asp:Label ID="Label2" runat="server"></asp:Label>
&nbsp;投标单号：<asp:Label ID="Label3" runat="server"></asp:Label>
      </td>
  </tr>
  <tr>
    <td width="50%" align="left" valign="top" bgcolor="#FFFFFF">拟卖出商品名称：<asp:Label ID="Label13" runat="server"></asp:Label>
      </td>
    <td width="50%" align="left" valign="top" bgcolor="#FFFFFF">规格：<asp:Label ID="Label14" runat="server"></asp:Label>
      </td>
  </tr>
  <tr>
    <td colspan="2" align="left" valign="top" bgcolor="#FFFFFF"><table width="100%" border="0" bgcolor="#cccccc"  style=" line-height:200%">
      <tr>
        <td align="left" valign="top" bgcolor="#FFFFFF">计价单位</td>
        <td align="left" valign="top" bgcolor="#FFFFFF">投标拟售量</td>
        <td align="left" valign="top" bgcolor="#FFFFFF">投标价格</td>
        <td align="left" valign="top" bgcolor="#FFFFFF">投标金额</td>
        <td align="left" valign="top" bgcolor="#FFFFFF">冻结的<br/>投标保证金</td>
        <td align="left" valign="top" bgcolor="#FFFFFF">平台设定<br/>最大经济批量</td>
        <td align="left" valign="top" bgcolor="#FFFFFF">卖方设定的<br/>经济批量</td>
        <td align="left" valign="top" bgcolor="#FFFFFF">合同期限</td>
      </tr>
      <tr>
        <td align="left" valign="top" bgcolor="#FFFFFF">
            <asp:Label ID="Label4" runat="server"></asp:Label>
          </td>
        <td align="left" valign="top" bgcolor="#FFFFFF">
            <asp:Label ID="Label5" runat="server"></asp:Label>
          </td>
        <td align="left" valign="top" bgcolor="#FFFFFF">
            <asp:Label ID="Label6" runat="server"></asp:Label>
          </td>
        <td align="left" valign="top" bgcolor="#FFFFFF">
            <asp:Label ID="Label7" runat="server"></asp:Label>
          </td>
        <td align="left" valign="top" bgcolor="#FFFFFF">
            <asp:Label ID="Label8" runat="server"></asp:Label>
          </td>
        <td align="left" valign="top" bgcolor="#FFFFFF">
            <asp:Label ID="Label9" runat="server"></asp:Label>
          </td>
        <td align="left" valign="top" bgcolor="#FFFFFF">
            <asp:Label ID="Label10" runat="server"></asp:Label>
          </td>
        <td align="left" valign="top" bgcolor="#FFFFFF">
            <asp:Label ID="Label11" runat="server"></asp:Label>
          </td>
      </tr>
    </table></td>
  </tr>
  <tr>
    <td colspan="2" align="left" valign="top" bgcolor="#FFFFFF">可供货区域：<asp:Label ID="Label12" runat="server"></asp:Label>
      </td>
  </tr>
</table>



    </p>
    <p>&nbsp;</p>
<p><strong>预订单：</strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a runat="server" id="yddxz" href="javascript:void(0)" showkjdy="">预订单下载</a></p>
<p>


    <table width="100%" border="0" cellspacing="1" bgcolor="#cccccc" style=" line-height:200%">
  <tr>
    <td colspan="2" align="left" valign="top" bgcolor="#FFFFFF">买方名称：<asp:Label ID="Label15" runat="server"></asp:Label>
&nbsp;下单时间：<asp:Label ID="Label16" runat="server"></asp:Label>
&nbsp;预订单号：<asp:Label ID="Label17" runat="server"></asp:Label>
      </td>
  </tr>
  <tr>
    <td width="50%" align="left" valign="top" bgcolor="#FFFFFF">拟买入商品名称：<asp:Label ID="Label18" runat="server"></asp:Label>
      </td>
    <td width="50%" align="left" valign="top" bgcolor="#FFFFFF">规格：<asp:Label ID="Label19" runat="server"></asp:Label>
      </td>
  </tr>
  <tr>
    <td colspan="2" align="left" valign="top" bgcolor="#FFFFFF"><table width="100%" border="0" bgcolor="#cccccc"  style=" line-height:200%">
      <tr>
        <td align="left" valign="top" bgcolor="#FFFFFF">计价单位</td>
        <td align="left" valign="top" bgcolor="#FFFFFF">拟订购数量</td>
        <td align="left" valign="top" bgcolor="#FFFFFF">拟买入价格</td>
        <td align="left" valign="top" bgcolor="#FFFFFF">拟订购金额</td>
        <td align="left" valign="top" bgcolor="#FFFFFF">冻结的订金</td>
        <td align="left" valign="top" bgcolor="#FFFFFF">合同期限</td>
      </tr>
      <tr>
        <td align="left" valign="top" bgcolor="#FFFFFF">
            <asp:Label ID="Label20" runat="server"></asp:Label>
          </td>
        <td align="left" valign="top" bgcolor="#FFFFFF">
            <asp:Label ID="Label21" runat="server"></asp:Label>
          </td>
        <td align="left" valign="top" bgcolor="#FFFFFF">
            <asp:Label ID="Label22" runat="server"></asp:Label>
          </td>
        <td align="left" valign="top" bgcolor="#FFFFFF">
            <asp:Label ID="Label23" runat="server"></asp:Label>
          </td>
        <td align="left" valign="top" bgcolor="#FFFFFF">
            <asp:Label ID="Label24" runat="server"></asp:Label>
          </td>
        <td align="left" valign="top" bgcolor="#FFFFFF">
            <asp:Label ID="Label25" runat="server"></asp:Label>
          </td>
 
      </tr>
    </table></td>
  </tr>
  <tr>
    <td colspan="2" align="left" valign="top" bgcolor="#FFFFFF">收货区域：<asp:Label ID="Label26" runat="server"></asp:Label>
      </td>
  </tr>
</table>







    </p>
    <p>&nbsp;</p>
<p><strong>投标相关资质：</strong></p>
<table width="100%" border="0">
  <tr runat="server" id="Tr1">
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_ZLBZYZM" href="javascript:void(0)" showtupian="">质量标准与证明</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_CPJCBG" href="javascript:void(0)" showtupian="">产品检测报告</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_PGFZRFLCNS" href="javascript:void(0)" showtupian="">品管负责人法律承诺书</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_FDDBRCNS" href="javascript:void(0)" showtupian="">法定代表人承诺书</a></td>
  </tr>
  <tr runat="server" id="Tr2">
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_SHFWGDYCN" href="javascript:void(0)" showtupian="">售后服务规定与承诺</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_CPSJSQS" href="javascript:void(0)" showtupian="">产品送检授权书</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_SLZM" href="javascript:void(0)" showtupian="">税率证明</a></td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_ZZ01" href="javascript:void(0)" showtupian=""></a>&nbsp;</td>
  </tr>
  <tr  runat="server" id="Tr3">
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_ZZ02" href="javascript:void(0)" showtupian=""></a>&nbsp;</td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_ZZ03" href="javascript:void(0)" showtupian=""></a>&nbsp;</td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_ZZ04" href="javascript:void(0)" showtupian=""></a>&nbsp;</td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_ZZ05" href="javascript:void(0)" showtupian=""></a>&nbsp;</td>
  </tr>
 <tr  runat="server" id="Tr4">
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_ZZ06" href="javascript:void(0)" showtupian=""></a>&nbsp;</td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_ZZ07" href="javascript:void(0)" showtupian=""></a>&nbsp;</td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_ZZ08" href="javascript:void(0)" showtupian=""></a>&nbsp;</td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_ZZ09" href="javascript:void(0)" showtupian=""></a>&nbsp;</td>
  </tr>
     <tr  runat="server" id="Tr5">
    <td width="25%" align="left" valign="middle"><a runat="server" id="lab_ZZ10" href="javascript:void(0)" showtupian=""></a>&nbsp;</td>
    <td width="25%" align="left" valign="middle">&nbsp;</td>
    <td width="25%" align="left" valign="middle">&nbsp;</td>
    <td width="25%" align="left" valign="middle">&nbsp;</td>
  </tr>
</table>
    <p>&nbsp;</p>
<p><strong>其他资质：</strong></p>
<table width="100%" border="0">
  <tr runat="server" id="Tr6">
    <td width="25%" align="left" valign="middle"><a runat="server" id="linkBZH" href="javascript:void(0)" showkjdy="">保证函</a>&nbsp;</td>
    <td width="25%" align="left" valign="middle"><a runat="server" id="linkLVZMWJ" href="javascript:void(0)" showtupian="">履约争议处理证明文件</a>&nbsp;</td>
    <td width="25%" align="left" valign="middle">&nbsp;</td>
    <td width="25%" align="left" valign="middle">&nbsp;</td>
  </tr>
</table>

<p>&nbsp;</p>
<p><strong>提货与发货信息</strong></p>
    <p runat="server" id="thyfhxxtxt"> </p>
<p>
    <asp:GridView ID="dataGridView3" runat="server" AutoGenerateColumns="False" Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="提货单编号">
                <ItemTemplate>
                    <a runat="server" id="hl_thdbh" href="javascript:void(0)" showkjdy='<%# Eval("提货单编号") %>'><%# Eval("提货单编号") %></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="提货数量" HeaderText="提货数量" />
            <asp:BoundField DataField="提货金额" HeaderText="提货金额" />
            <asp:BoundField DataField="当前状态" HeaderText="当前状态" />
                        <asp:TemplateField HeaderText="发货单编号">
                <ItemTemplate>
                    <a runat="server" id="hl_fhdbh" href="javascript:void(0)" showkjdy='<%# Eval("发货单编号") %>'><%# Eval("发货单编号") %></a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </p>
 
<p>&nbsp;</p>
</form>
