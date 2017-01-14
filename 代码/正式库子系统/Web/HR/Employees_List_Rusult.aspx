<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Employees_List_Rusult.aspx.cs" Inherits="Web_HR_Employees_List_Rusult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../css/style.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        .style1
        {
            width: 254px;
            height: 25px;
        }
        .style2
        {
            height: 25px;
        }
    </style>
    
    </head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table width="800" border="1" cellpadding="0" cellspacing="0"  bordercolor="#999999"  class="FormView" style="border-collapse:collapse">
            <tr >
                <td height="25" align="center" style="width: 122px" >员工编号</td>
                <td width="118" align="center"><%=number%></td>
                <td align="center" style="width: 128px">姓&nbsp; 名 </td>
                <td width="157" align="center" ><%=xm%></td>
                <td width="97" align="center">隶&nbsp; 属 </td>
                <td align="center" style="width: 145px"><%=ls%></td>
            </tr>
            <tr>
                <td height="25" align="center" style="width: 122px">部&nbsp; 门</td>
                <td align="center"><%=bm%></td>
                <td align="center" style="width: 128px"  >二级部门/组 </td>
                <td align="center"  ><%=ejbmz%> </td>
                <td width="97" align="center"  >岗位名称 </td>
                <td align="center" style="width: 145px"  ><%=gwmc%></td>
            </tr>
            <tr>
                <td height="25" align="center" style="width: 122px"  >学&nbsp; 历</td>
                <td align="center"  ><%=xl%></td>
                <td align="center" style="width: 128px"  >专&nbsp; 业 </td>
                <td align="center"  ><%=zy%></td>
                <td width="97" align="center"  >毕业院校 </td>
                <td align="center" style="width: 145px"  ><%=byyx%></td>
            </tr>
            <tr>
                <td height="25" align="center" style="width: 122px"  >性&nbsp; 别</td>
                <td align="center"  ><%=xb%></td>
                <td align="center" style="width: 128px"  >民&nbsp; 族 </td>
                <td align="center"  ><%=mz%></td>
                <td width="97" align="center"  >身份证号码 </td>
                <td align="center" style="width: 145px"  ><%=sfzh%></td>
            </tr>
            <tr>
                <td height="25" align="center" style="width: 122px"  >籍&nbsp; 贯</td>
                <td align="center"  >&nbsp; <%=jg%></td>
                <td align="center" style="width: 128px"  >血&nbsp; 型 </td>
                <td align="center"  ><%=xx%></td>
                <td width="97" align="center"  >出生年月 </td>
                <td align="center" style="width: 145px"  ><%=csny%></td>
            </tr>
            <tr>
                <td height="25" align="center" style="width: 122px"  >政治面貌</td>
                <td align="center"  ><%=zzmm%></td>
                <td align="center" style="width: 128px"  >入党时间</td>
                <td align="center"  >&nbsp;<%=dyrdsj%></td>
                <td  align="center"   rowspan="5" >照片</td>
                <td align="center"  rowspan="5" style="width: 145px"><%=zp %></td>
            </tr>
            <tr>
                <td height="25" align="center" style="width: 122px"  >现住址</td>
                <td colspan="3" align="left"  >&nbsp; <%=xzz%> </td>
            </tr>
           
            <tr>
                <td height="25" align="center" style="width: 122px"  >户口所在地</td>
                <td colspan="3" align="left"  >&nbsp; <%=hkszd%>    </td>
            </tr>
            <tr>
                <td height="25" align="center" style="width: 122px"  >婚姻状况 </td>
                <td align="center"  ><%=hyzk%></td>
                <td align="center" style="width: 128px"  >生育状况 </td>
                <td align="center"  >&nbsp;<%=syzk%> </td>
                            
            </tr>
            <tr>
                <td align="center" style="width: 122px"  >子女状况 </td>
			    <td colspan="5" align="left" valign="top" >
                   <table width="100%"  cellpadding="0" cellspacing="0" class="FormView" border="1px" bordercolor="#999999" style="border-collapse:collapse" >
                      <tr>
                          <td  align="center" class="style1">&nbsp;性别</td>
                          <td  align="center" class="style2" >&nbsp;出生日期</td>
                      </tr>
                      <%
                          if (znds.Tables[0].Rows.Count > 0)
                          {
                              for (int i = 0; i < znds.Tables[0].Rows.Count; i++)
                              {
                       %>
                      <tr>
                          <td  align="center" style="height: 25px; width: 234px;">&nbsp; <%=znds.Tables[0].Rows[i]["EmployeeZN_Sex"]%></td>
                          <td style="height: 25px" align="center"  >&nbsp; <%=znds.Tables[0].Rows[i]["EmployeeZN_BirthDay"]%>                            </td>
                      </tr> 
                      <%
                              }
                          }
                        %>
                     
                   </table>                
                </td>
            </tr>
            <tr>
                <td height="25" align="center" style="width: 122px"  >个人电话 </td>
                <td align="center"  >&nbsp; <%=grdh%></td>
                <td align="center" style="width: 128px"  >&nbsp; 家庭电话 </td>
                <td align="center"  >&nbsp; <%=jtdh %></td>
                <td align="center"  >&nbsp;</td>
                <td style="width: 145px"  >&nbsp;</td>
            </tr>
            <tr>
                <td height="25" align="center" style="width: 122px"  >紧急联系人 </td>
                <td align="center"  >&nbsp; <%=jjlxr %></td>
                <td align="center" style="width: 128px"  >紧急联系人电话 </td>
                <td align="center"  >&nbsp; <%=jjlxrdh %></td>
                <td align="center"  >&nbsp;                    </td>
              <td style="width: 145px"  >&nbsp;                    </td>
            </tr>
            <tr>
                <td height="25" align="center" style="width: 140px"  >紧急联系人与本人的关系 </td>
                <td align="center"  >&nbsp; <%=jjlxrybrgx%></td>
                <td align="center" style="width: 128px"  >紧急联系人通信地址 </td>
                <td colspan="3"   align="left"><%=jtdz%>                 </td>
            </tr>
            <tr>
                <td height="25" align="center" style="width: 122px"  >外语语种 </td>
                <td align="center"  >&nbsp; <%=wyyz %></td>
                <td align="center" style="width: 128px"  >外语水平 </td>
                <td align="center"  >&nbsp; <%=wysp %></td>
                <td align="center"  >计算机水平 </td>
                <td align="center" style="width: 145px"  >&nbsp; <%=jsjsp %></td>
            </tr>
            <tr>
                <td height="25" align="center" style="width: 122px"  >入职前培训期开始时间 </td>
                <td align="center" colspan ="2"  >&nbsp;<%=rzqpxkssj %></td>
                <td align="center">入职前培训期结束时间 </td>
                <td align="center"  colspan ="2" >&nbsp;<%=rzqpxjssj %></td>                
              <%--  <td align="center"  ></td>
                <td style="width: 145px"  ></td>--%>
            </tr>
            <tr>
                <td height="25" align="center" style="width: 122px"  >考察期开始时间 </td>
                <td align="center" colspan ="2" ><%=kcqkssj %></td>                
                <td align="center"  >考察期结束时间 </td>
                <td align="center" colspan ="2" >&nbsp;<%=kcqjssj %></td>
                <%--<td align="center"  ></td>
                <td style="width: 145px"  ></td>--%>
            </tr>
             <tr>
                <td height="25" align="center" style="width: 122px"  >劳动合同开始时间</td>
                <td align="center"  ><%=ldhtkssj %></td>                
                <td align="center" style="width: 128px"  >劳动合同到期时间</td>
                <td align="center"  ><%=ldhtdqsj %></td>
                <td align="center"  >合同期限</td>
                <td style="width: 145px" align ="center" ><%=ldhtqx %></td>
            </tr>
            <tr>
                <td height="25" align="center" style="width: 122px"  >入职时间</td>
                <td align="center"  >&nbsp;<%=rzsj %></td>
                <td align="center" style="width: 128px"  >转正时间</td>
                <td align="center"  >&nbsp;<%=zzsj %></td>
                <td align="center"  >&nbsp;</td>
                <td align="center" style="width: 145px"  >&nbsp;</td>
              
              
            </tr>
        <%--    <tr>
                <td align="center" style="width: 122px">任职资格证书名称</td>
				<td colspan="5" align="left" valign="top"  >
                   <table width="100%" cellpadding="0" cellspacing="0" class="FormView" border="1px" bordercolor="#999999" style="border-collapse:collapse"  >
                     <tr>
                          <td  align="center" style="height: 25px; width: 254px;" >&nbsp;名称</td>
                          <td  align="center" style="height: 25px" > 有效期</td>
                     </tr>
                <%
                          if (rzzgds.Tables[0].Rows.Count > 0)
                          {
                              for (int i = 0; i < rzzgds.Tables[0].Rows.Count; i++)
                              {
                  %>
                      <tr>
                          <td  align="center" style="height: 25px; width: 235px;" >&nbsp;<%=rzzgds.Tables[0].Rows[i]["MC"]%></td>
                          <td align="center" style="height: 25px" >&nbsp; <%=rzzgds.Tables[0].Rows[i]["YXQQ"]%> &nbsp; 至 &nbsp; <%=rzzgds.Tables[0].Rows[i]["YXQZ"]%> </td>
                      </tr>
                      <%
                          }
                          }       
                       %>
                   </table>            
                </td>
            </tr>--%>
            <tr>
                <td align="center" style="width: 122px"  ><br />教<br />
                    育<br />
                    经<br />
                    历<br />               
                </td>
                <td colspan="5" align="left" class="style2" valign="top" >
                    <table width="100%" cellpadding="0" cellspacing="0" class="FormView" border="1px" bordercolor="#999999" style="border-collapse:collapse">
                        <tr align="center">
                            <td width="100" height="25"a lign ="center">起止时间</td>
                            <td width="150" align ="center">学校名称</td>
                            <td width="100" align ="center">专&nbsp; 业</td>
                            <td width="60" align ="center">学&nbsp; 历</td>
                            <td width="100" align ="center">学&nbsp; 位 </td>
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
                    </table>             
                </td>
            </tr>
            <tr>
                <td align="center" class="style3" style="width: 122px" ><br />
                  工<br />
                  作<br />
                  经<br />
                  历<br />                 
                </td>
                <td colspan="5" align="left" valign="top" class="style3" >
                   <table width="100%" cellpadding="0" cellspacing="0" border="1px" bordercolor="#999999" style="border-collapse:collapse">
                      <tr align="center">
                         <td width="100" height="25" align ="center">起止时间</td>
                         <td width="150" align ="center">单位名称</td>
                         <td width="100" align ="center">职&nbsp; 务</td>
                         <td width="60" align ="center">证明人</td>
                         <td width="100" align ="center">联系电话</td>
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
                   </table>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 122px" ><br />
                  培<br />
                  训<br />
                  经<br />
                  历<br />
                </td>
                <td colspan="5" align="left" valign="top" >
                    <table width="100%" cellpadding="0" cellspacing="0" class="FormView" border="1px" bordercolor="#999999" style="border-collapse:collapse">
                       <tr align="center">
                          <td width="145" height="25" align ="center">培训时间</td>
                          <td width="204" align ="center">培训机构</td>
                          <td width="159" align ="center">培训课程</td>
                          <td width="149" align ="center">培训方式</td>
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
                   </table>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 122px" ><br />
                  家<br />
                  庭<br />
                  情<br />
                  况<br />                
                </td>
                <td colspan="5" align="left" valign="top" >
                   <table width="100%" cellpadding="0" cellspacing="0" class="FormView" border="1px" bordercolor="#999999" style="border-collapse:collapse">
                      <tr align="center">
                         <td height="25" style="width: 100px" align ="center">姓名</td>
                         
                         <td width="50" align ="center">关系</td>
                         <td width="260" align ="center">工作单位</td>
                         <td width="60" align ="center">职务</td>
                         <td width="100" align ="center">联系电话</td>
                      </tr>
                      <%
                        if (jtzkds.Tables[0].Rows.Count > 0)
                          {
                              for (int i = 0; i < jtzkds.Tables[0].Rows.Count; i++)
                              {
                      %>
                      <tr>
                         <td align="center" style="width: 100px; height: 26px" >&nbsp;<%=jtzkds.Tables[0].Rows[i]["XM"]%></td>
                         <td align="center" style="width: 50px; height: 26px" >&nbsp;<%=jtzkds.Tables[0].Rows[i]["GX"]%></td>
                         <td align="center" style="height: 26px" >&nbsp;<%=jtzkds.Tables[0].Rows[i]["GZDW"]%></td>
                         <td align="center" style="height: 26px" >&nbsp;<%=jtzkds.Tables[0].Rows[i]["GZGW"]%></td>
                         <td align="center" style="height: 26px" >&nbsp;<%=jtzkds.Tables[0].Rows[i]["LXDH"]%></td>
                      </tr>
                       <%
                          }
                          }       
                       %>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 122px" ><br />
                历<br />
                次<br />
                调<br />
                岗<br />
                记<br />
                录<br />                 
                </td>
                <td colspan="5" align="left" valign="top" >
                   <table width="100%" cellpadding="0" cellspacing="0" class="FormView" border="1px" bordercolor="#999999" style="border-collapse:collapse">
                      <tr align="center">
                         <td width="100" height="25" align ="center">到岗时间</td>
                         <td width="100"  align ="center"> 原岗位</td>
                         <td width="100"  align ="center">调往岗位</td>
                         <td width="80" align ="center">调岗任职状态</td>
                         <td width="80" align ="center" >调岗性质</td>
                         <td width="80" align ="center">备注</td>
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
                         <td align="center" >&nbsp;<%=tgglds.Tables[0].Rows[i]["TWGW"]%></td>
                         <td align="center" >&nbsp;<%=tgglds.Tables[0].Rows[i]["TGRZZT"]%></td>
                         <td align="center" >&nbsp;<%=tgglds.Tables[0].Rows[i]["TGLB"]%></td>
                         <td align="center" >&nbsp;<%=tgglds.Tables[0].Rows[i]["BZ"]%></td>
                      </tr>
                       <%
                          }
                          }       
                       %>
                   </table>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 122px" >
                <p align="center" ><br />
                奖<br />
                惩<br />
                记<br />
                录<br /></p>
                </td>
                <td colspan="5" align="left" valign="top" >
                   <table width="100%" cellpadding="0" cellspacing="0" class="FormView" border="1px" bordercolor="#999999" style="border-collapse:collapse">
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
                   </table>
                </td>
            </tr>
        <%--    <tr>
                <td align="center" style="width: 122px" >
                <br />
                合<br />
                同<br />
                记<br />
                录<br />
                </td>
                <td colspan="5" align="left" valign="top" >
                   <table width="100%" cellpadding="0" cellspacing="0" class="FormView" border="1px" bordercolor="#999999" style="border-collapse:collapse">
                      <tr >
                         <td align="right" style="width:100px;height:25px ">&nbsp;&nbsp;合同签订日期</td>
                         <td align="right" style="width:150px" >&nbsp;&nbsp;合同到期日期</td>
                         <td align="right" style="width:100px" >&nbsp;&nbsp;合同期限</td>
                         <td align="center" style="width:100px" >&nbsp;&nbsp;备注</td>
                      </tr>
                      <%
                          if (htjlds.Tables[0].Rows.Count > 0)
                          {
                              for (int i = 0; i < htjlds.Tables[0].Rows.Count; i++)
                              {
                      %>
      
                      <tr>
                         <td height="25" align="center" >&nbsp;<%=htjlds.Tables[0].Rows[i]["HTKSSJ"]%></td>
                         <td align="center" >&nbsp;<%=htjlds.Tables[0].Rows[i]["HTDQSJ"]%></td>
                         <td align="center" >&nbsp;<%=htjlds.Tables[0].Rows[i]["LDHTQX"]%></td>
                         <td align="center" >&nbsp;<%=htjlds.Tables[0].Rows[i]["BZ"]%></td>
                      </tr>
                       <%
                          }
                          }       
                       %>
                   </table>
                </td>
            </tr>--%>
            <tr>
                <td align="center" style="width: 122px" >&nbsp;</td>
                <td colspan="5" align="right" valign="top" >
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="返回" />
                </td>
          </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
