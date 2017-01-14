<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jhjx_wykfjuk.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_SPMM_jhjx_wykfjuk" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%" border="1" align="center" cellpadding="0" cellspacing="0" style="font-size: 12px; font-family: 宋体; word-break: break-all; border-top: none 0 none; border-bottom: none 0 none">
                <tr>
                    <td colspan="6" align="center" style="font-weight: bold; height: 20px">违约扣发记录
                    </td>
                </tr>
                <tr>
                    <td style=" text-align:center " >
                        序号
                    </td>
                    <td style=" text-align:center " >
                        产生时间
                    </td>
                    <td style=" text-align:center ">
                        金额
                    </td>
                    <td style=" text-align:center ">
                        项目
                    </td>
                    <td style=" text-align:center ">
                        性质
                    </td>
                    <td style=" text-align:center ">
                        摘要 
                    </td>
                </tr>

                <%     
                   
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        { 
                %>
                <tr>
                    <td align="center" style="height: 30px">
                        <%=i+1 %>
                    </td>
                    <td align="center">
                        <%=ds.Tables[0].Rows[i]["流水产生时间"].ToString()%>
                    </td>
                   
                    <td align="center">
                        <%=ds.Tables[0].Rows[i]["金额"].ToString()%>
                    </td>
                    <td align="center">
                        <%=ds.Tables[0].Rows[i]["项目"].ToString()%>
                    </td>
                    <td align="center">
                        <%=ds.Tables[0].Rows[i]["性质"].ToString()%>
                    </td>
                     <td align="center">
                        <%=ds.Tables[0].Rows[i]["摘要"].ToString()%>
                    </td>

                </tr>
                <%
                                    }
                                    
                                }
                %>
            </table>
        </div>
    </form>
</body>
</html>
