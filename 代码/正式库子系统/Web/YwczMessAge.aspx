<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YwczMessAge.aspx.cs" Inherits="Web_YwczMessAge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="90%" border="0" cellpadding="10" cellspacing="0" style="font-size:9pt">
  <tr>
    <td align="center" valign="middle" bgcolor="#FFFFF0" style="color:#F00">业务操作平台请示批复业务功能升级通知</td>
  </tr>
  <tr>
    <td><ol>
      <li>添加了新的“转发”，“终止”按钮，不仅在上方的单据列表实现“转发”，和“终止”操作，在查看单据“审签详情”中也可以进行此两项操作。新增按钮放置在了审签详情标签右侧。 </li>
      <li>“转发“功能不再受单据审签状态影响,除”终止“状态的单据，其他单据都可以由审签人转发给任何人，包括已经在审批流程中的人。</li>
      <li>添加“终止”单据时添加输入终止意见功能，系统自动记录终止人和终止时间，同时在审签意见模块标红显示此两项内容。</li>
      <li>增加备忘功能。在单据列表添加“备忘“标签。可以对自己需要特别关注的任何状态的审签单据进行备忘，同时在备忘一栏内显示，并可以撤销单据备忘设置。 </li>
      <li>.调换了审签人和审签意见的位置。 </li>
      <li>增加在当前审签意见根据审签级别顺序显示。</li>
      <li>转发单据时，转发意见一栏自动添加信息：xxx转发至xxx、xxx 。整个转发意见显示格式变为：[xxx转发至xxx  xxx ]转发意见。</li>
      <li>添加了审签意见修改功能，下一级审签人没有审签之前，或者同级审签人没有审签的情况下，可以修改审签意见了。</li>
      <li>业务操作平台请示批复添加“手机短信提醒”选项功能。系统右上角添加了“接收短信提醒“复选框，默认不勾选。如果经常外出不在线，请选择“接受短信提醒”，勾选此项，以便及时接收系统提醒。</li>
      <li>业务操作平台提醒页面中，隐藏了工号列，仅显示姓名，增加闪动提醒、语音提醒，可以选择语音提醒音，并调整显示位置。</li>
      <li>分配审签人和转发功能修改。增加自定义审签人组，并调整操作界面。</li>
    </ol></td>
  </tr>
  <tr>
     <td align="right" valign="middle"  >信息化中心</td>
  </tr>
  <tr>
     <td align="right" valign="middle" >2010-07-30</td>
  </tr>
</table>
    </div>
    </form>
</body>
</html>
