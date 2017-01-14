<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Employees_List_Rusult_LZ.aspx.cs" Inherits="Web_HR_Employees_List_Rusult_LZ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../css/style.css" rel="stylesheet" type="text/css" />
    
    </head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table width=750 border="1" cellpadding="0" cellspacing="0" 
            bordercolor="#999999"  class="FormView" style="border-collapse:collapse">
<tr >
                <td width="120" height="25" align="center"  ><span>员工编号</td>
<td width="118" align="center" 
                    ><%=number%></td>
<td width="126" align="center" 
                    >姓  名 </td>
<td width="157" align="center" 
                    ><%=xm%></td>
        <td width="97" align="center" 
                    >隶  属 </td>
<td width="145" align="center" 
                    ><%=ls%></td>
        </tr>
            <tr>
                <td width="120" height="25" align="center" 
                    >部  门</td>
<td align="center" 
                    ><%=bm%></td>
          <td width="126" align="center"  >二级部门/组 </td>
<td align="center"  ><%=ejbmz%> </td>
              <td width="97" align="center"  >岗位名称 </td>
<td align="center"  ><%=gwmc%></td>
          </tr>
            <tr>
                <td width="120" height="25" align="center"  >学  历</td>
<td align="center"  ><%=xl%></td>
          <td width="126" align="center"  >专  业 </td>
<td align="center"  ><%=zy%></td>
              <td width="97" align="center"  >毕业院校 </td>
<td align="center"  ><%=byyx%></td>
          </tr>
            <tr>
                <td width="120" height="25" align="center"  >性  别</td>
  <td align="center"  ><%=xb%></td>
          <td width="126" align="center"  >民  族 </td>
<td align="center"  ><%=mz%></td>
              <td width="97" align="center"  >身份证号码 </td>
<td align="center"  ><%=sfzh%></td>
          </tr>
            <tr>
                <td width="120" height="25" align="center"  >籍  贯</td>
<td align="center"  >&nbsp; <%=jg%></td>
          <td width="126" align="center"  >血  型 </td>
<td align="center"  ><%=xx%></td>
              <td width="97" align="center"  >出生年月 </td>
<td align="center"  ><%=csny%></td>
          </tr>
            <tr>
                <td width="120" height="25" align="center"  >政治面貌</td>
<td align="center"  ><%=zzmm%></td>
          <td width="126" align="center"  >入党时间                    </td>
<td align="center"  >&nbsp;<%=dyrdsj%></td>

                               <td  align="center" >照片                </td>
                <td align="center"  rowspan="5"><%=zp %></td>
            </tr>
            <tr>
                <td width="120" height="25" align="center"  >现住址</td>
<td colspan="4" align="left"  >&nbsp; <%=xzz%>      </td>
          </tr>
            <tr>
                <td width="120" height="25" align="center"  >家庭住址</td>
<td colspan="4" align="left"  >&nbsp; <%=jtdz%> </td>
          </tr>
            <tr>
                <td width="120" height="25" align="center"  >户口所在地</td>
<td colspan="4" align="left"  >&nbsp; <%=hkszd%>    </td>
          </tr>
            <tr>
                <td width="120" height="25" align="center"  >婚姻状况 </td>
  <td align="center"  ><%=hyzk%></td>
          <td width="126" align="center"  >生育状况                    </td>
<td align="center"  >&nbsp;<%=syzk%> </td>
              <td  >&nbsp;                    </td>              
                <td  >&nbsp;                    </td>
            </tr>
            <tr>
                <td width="120" align="center"  >子女状况 </td>
			  <td colspan="5" align="left" valign="top" 
                    style="border-right: 1px solid; border-top: 1px solid; border-left: 1px solid; border-bottom: 1px solid;">
                <table width="100%" border="0" cellpadding="0" cellspacing="0" 
                      style="border-style: solid; border-width: 0px" >
                      <%
                          if (znds.Tables[0].Rows.Count > 0)
                          {
                              for (int i = 0; i < znds.Tables[0].Rows.Count; i++)
                              {
                       %>
                      <tr>
                          <td  align="center">&nbsp;性别： <%=znds.Tables[0].Rows[i]["EmployeeZN_Sex"]%></td>
                          <td >&nbsp;出生日期： <%=znds.Tables[0].Rows[i]["EmployeeZN_BirthDay"]%>                            </td>
                      </tr>
                      <%
                              }
                          }
                        %>
                     
                  </table>                </td>
          </tr>
            <tr>
                <td width="120" height="25" align="center"  >个人电话 </td>
  <td align="center"  >&nbsp; <%=grdh%></td>
          <td width="126" align="center"  >&nbsp; 家庭电话 </td>
              <td align="center"  >&nbsp; <%=jtdh %></td>
              <td align="center"  >&nbsp;                    </td>
              <td  >&nbsp;                    </td>
            </tr>
            <tr>
                <td width="120" height="25" align="center"  >紧急联系人 </td>
  <td align="center"  >&nbsp; <%=jjlxr %></td>
          <td width="126" align="center"  >紧急联系人电话 </td>
              <td align="center"  >&nbsp; <%=jjlxrdh %></td>
              <td align="center"  >&nbsp;                    </td>
              <td  >&nbsp;                    </td>
            </tr>
            <tr>
                <td width="120" height="25" align="center"  >外语语种 </td>
  <td align="center"  >&nbsp; <%=wyyz %></td>
          <td width="126" align="center"  >外语水平 </td>
              <td align="center"  >&nbsp; <%=wysp %></td>
              <td align="center"  >计算机水平 </td>
              <td align="center"  >&nbsp; <%=jsjsp %></td>
          </tr>
            <tr>
                <td width="120" height="25" align="center"  >入职前培训期开始时间 </td>
  <td align="center"  >&nbsp;<%=rzqpxkssj %></td>
          <td width="126" align="center"  >入职前培训期结束时间 </td>
              <td align="center"  >&nbsp;<%=rzqpxjssj %></td>
              <td align="center"  >&nbsp;                    </td>
              <td  >&nbsp;                    </td>
            </tr>
            <tr>
                <td width="120" height="25" align="center"  >考察期开始时间 </td>
  <td align="center"  ><%=kcqkssj %></td>
          <td width="126" align="center"  >考察期结束时间 </td>
              <td align="center"  >&nbsp;<%=kcqjssj %></td>
              <td align="center"  >&nbsp;                    </td>
              <td  >&nbsp;                    </td>
            </tr>
            <tr>
                <td width="120" height="25" align="center"  >入职时间</td>
  <td align="center"  >&nbsp;<%=rzsj %></td>
          <td width="126" align="center"  >转正时间</td>
              <td align="center"  >&nbsp;<%=zzsj %></td>
              <td align="center"  >离职时间</td>
              <td align="center"  >&nbsp;<%=lzsj %></td>
              
              <td  style="border-right: 1px solid; border-top: 1px solid; border-left: 1px solid; border-bottom: 1px solid;border-color: #000000; border-width: 1px 1px 1px 1px;">&nbsp;                    </td>
            </tr>
            <tr>
                <td width="120" align="center" 
                     >任职资格证书名称</td>
						  <td colspan="5" align="left" valign="top" 
                    style="border-right: 1px solid; border-top: 1px solid; border-left: 1px solid; border-bottom: 1px solid;">
                  <table width="100%" border="0" cellpadding="0" cellspacing="0" style="border-style: solid; border-width: 1px" >
                <%
                          if (rzzgds.Tables[0].Rows.Count > 0)
                          {
                              for (int i = 0; i < rzzgds.Tables[0].Rows.Count; i++)
                              {
                  %>
                      <tr>
                          <td  align="center" >&nbsp;名称：<%=rzzgds.Tables[0].Rows[i]["MC"]%></td>
                          <td >&nbsp; <%=rzzgds.Tables[0].Rows[i]["YXQQ"]%> &nbsp; 至 &nbsp; <%=rzzgds.Tables[0].Rows[i]["YXQZ"]%>             </td>
                      </tr>
                      <%
                          }
                          }       
                       %>
                  </table>                </td>
          </tr>
            <tr>
                <td width="120" align="center"  ><br />教<br />
                    育<br />
                    经<br />
              历<br />               </td>
<td colspan="5" align="left" class="style2" valign="top" >
      
                <table width="100%" cellpadding="0" cellspacing="0" class="FormView" border="1px" bordercolor="#999999" style="border-collapse:collapse">
      <tr align="center">
                        <td width="100" height="25">起止时间</td>
                <td width="150" >学校名称</td>
                        <td width="100" >专  业</td>
                        <td width="60" >学  历</td>
                        <td width="100" >学  位 </td>
                        <td ><p align="center" >教育类型</p></td>
                    </tr>
                    <%
                        if (jyjlds.Tables[0].Rows.Count > 0)
                          {
                              for (int i = 0; i < jyjlds.Tables[0].Rows.Count; i++)
                              {
                  %>
                    <tr>
                        <td height="25" align="center" >&nbsp;<%=jyjlds.Tables[0].Rows[i]["QZSJ"]%></td>
                      <td align="center" >&nbsp;<%=jyjlds.Tables[0].Rows[i]["YXMC"]%></td>
                      <td align="center" >&nbsp;<%=jyjlds.Tables[0].Rows[i]["ZY"]%></td>
                      <td align="center" >&nbsp;<%=jyjlds.Tables[0].Rows[i]["XL"]%></td>
                      <td align="center" >&nbsp;<%=jyjlds.Tables[0].Rows[i]["XW"]%></td>
                      <td align="center" >&nbsp;<%=jyjlds.Tables[0].Rows[i]["JYLX"]%></td>
                  </tr>
                     <%
                          }
                          }       
                       %>
                </table>              </td>
          </tr>
            <tr>
                <td width="120" align="center" class="style3" ><br />工<br />
                  作<br />
                  经<br />
                  历<br />                 </td>
  <td colspan="5" align="left" valign="top" class="style3" ><table width="100%" cellpadding="0" cellspacing="0" border="1px" bordercolor="#999999" style="border-collapse:collapse">
    <tr align="center">
      <td width="100" height="25" >起止时间</td>
      <td width="150" >单位名称</td>
      <td width="100" >职  务</td>
      <td width="60" >证明人</td>
      <td width="100" >联系电话</td>
      <td ><p align="center"  >离职原因</p></td>
    </tr>
     <%
         if (gzjlds.Tables[0].Rows.Count > 0)
                          {
                              for (int i = 0; i < gzjlds.Tables[0].Rows.Count; i++)
                              {
                  %>
    <tr>
      <td align="center" >&nbsp;<%=gzjlds.Tables[0].Rows[i]["QZSJ"]%></td>
      <td align="center" >&nbsp;<%=gzjlds.Tables[0].Rows[i]["DWMC"]%></td>
      <td align="center" >&nbsp;<%=gzjlds.Tables[0].Rows[i]["ZW"]%></td>
      <td align="center" >&nbsp;<%=gzjlds.Tables[0].Rows[i]["ZMR"]%></td>
      <td align="center" >&nbsp;<%=gzjlds.Tables[0].Rows[i]["LXDH"]%></td>
      <td height="25" align="center" >&nbsp;<%=gzjlds.Tables[0].Rows[i]["LZYY"]%></td>
    </tr>
     <%
                          }
                          }       
                       %>
  </table></td>
          </tr>
            <tr>
                <td width="120" align="center" ><br />培<br />
                  训<br />
                  经<br />
                  历<br />
                                 </td>
  <td colspan="5" align="left" valign="top" ><table width="100%" cellpadding="0" cellspacing="0" class="FormView" border="1px" bordercolor="#999999" style="border-collapse:collapse">
    <tr align="center">
      <td width="145" height="25" >培训时间</td>
      <td width="204" >培训机构</td>
      <td width="159" >培训课程</td>
      <td width="149" >培训方式</td>
      </tr>
       <%
           if (pxjlds.Tables[0].Rows.Count > 0)
                          {
                              for (int i = 0; i < pxjlds.Tables[0].Rows.Count; i++)
                              {
                  %>
    <tr>
      <td height="25" align="center" >&nbsp;<%=pxjlds.Tables[0].Rows[i]["PXSJ"]%></td>
      <td align="center" >&nbsp;<%=pxjlds.Tables[0].Rows[i]["PXXM"]%></td>
      <td align="center" >&nbsp;<%=pxjlds.Tables[0].Rows[i]["PXKC"]%></td>
      <td align="center" >&nbsp;<%=pxjlds.Tables[0].Rows[i]["PXFS"]%></td>
      </tr>
      <%
                          }
                          }       
                       %>
  </table></td>
          </tr>
            <tr>
                <td width="120" align="center" ><br />家<br />
                  庭<br />
                  情<br />
                  况<br />                </td>
  <td colspan="5" align="left" valign="top" ><table width="100%" cellpadding="0" cellspacing="0" class="FormView" border="1px" bordercolor="#999999" style="border-collapse:collapse">
    <tr align="center">
      <td width="100" height="25" >姓  名</td>
      <td width="150" >关  系</td>
      <td width="100" >工作单位</td>
      <td width="60" >职务</td>
      <td width="100" >联系电话</td>
      </tr>
      <%
          if (jtzkds.Tables[0].Rows.Count > 0)
                          {
                              for (int i = 0; i < jtzkds.Tables[0].Rows.Count; i++)
                              {
                  %>
    <tr>
      <td height="25" align="center" >&nbsp;<%=jtzkds.Tables[0].Rows[i]["XM"]%></td>
      <td align="center" >&nbsp;<%=jtzkds.Tables[0].Rows[i]["GX"]%></td>
      <td align="center" >&nbsp;<%=jtzkds.Tables[0].Rows[i]["GZDW"]%></td>
      <td align="center" >&nbsp;<%=jtzkds.Tables[0].Rows[i]["GZGW"]%></td>
      <td align="center" >&nbsp;<%=jtzkds.Tables[0].Rows[i]["LXDH"]%></td>
      </tr>
       <%
                          }
                          }       
                       %>
  </table></td>
          </tr>
            <tr>
                <td width="120" align="center" 
                     
                    ><br />历<br />
                次<br />
                调<br />
                岗<br />
                记<br />
                录<br />                 </td>
  <td colspan="5" align="left" valign="top" ><table width="100%" cellpadding="0" cellspacing="0" class="FormView" border="1px" bordercolor="#999999" style="border-collapse:collapse">
    <tr align="center">
      <td width="100" height="25" >起止时间</td>
      <td width="150" >岗位</td>
      <td width="100" >调岗任职状态</td>
      <td width="60" >调岗性质</td>
      <td width="100" >备注</td>
    </tr>
     <%
         if (tgglds.Tables[0].Rows.Count > 0)
                          {
                              for (int i = 0; i < tgglds.Tables[0].Rows.Count; i++)
                              {
                  %>
    <tr>
      <td height="25" align="center" >&nbsp;<%=tgglds.Tables[0].Rows[i]["DGSJ"]%></td>
      <td align="center" >&nbsp;<%=tgglds.Tables[0].Rows[i]["YGW"]%></td>
      <td align="center" >&nbsp;<%=tgglds.Tables[0].Rows[i]["ZW"]%></td>
      <td align="center" >&nbsp;<%=tgglds.Tables[0].Rows[i]["TGLB"]%></td>
      <td align="center" >&nbsp;<%=tgglds.Tables[0].Rows[i]["BZ"]%></td>
    </tr>
     <%
                          }
                          }       
                       %>
  </table></td>
          </tr>
            <tr>
                <td width="120" align="center" >
                <p align="center" ><br />
                奖<br />
                惩<br />
                记<br />
                录<br /></p></td>
  <td colspan="5" align="left" valign="top" ><table width="100%" cellpadding="0" cellspacing="0" class="FormView" border="1px" bordercolor="#999999" style="border-collapse:collapse">
    <tr align="center">
      <td width="100" height="25" >时间</td>
      <td width="150" >奖惩事由</td>
      <td width="100" >奖惩方式</td>
      </tr>
       <%
           if (jfglds.Tables[0].Rows.Count > 0)
                          {
                              for (int i = 0; i < jfglds.Tables[0].Rows.Count; i++)
                              {
                  %>
      
    <tr>
      <td height="25" align="center" >&nbsp;<%=jfglds.Tables[0].Rows[i]["SJ"]%></td>
      <td align="center" >&nbsp;<%=jfglds.Tables[0].Rows[i]["JCSY"]%></td>
      <td align="center" >&nbsp;<%=jfglds.Tables[0].Rows[i]["JCFS"]%></td>
      </tr>
       <%
                          }
                          }       
                       %>
  </table></td>
          </tr>
            <tr>
                <td width="120" align="center" >
                    &nbsp;</td>
  <td colspan="5" align="right" valign="top" >
      <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="返回" />
                </td>
          </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
