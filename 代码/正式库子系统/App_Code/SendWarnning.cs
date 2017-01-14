using System;
using System.Collections.Generic;
using System.Web;
using Hesion.Brick.Core;

/// <summary>
///SendWarnning 的摘要说明
/// </summary>
namespace FMOP.Tools
{
	public class SendWarnning
	{
		public SendWarnning()
		{
			//
			//TODO: 在此处添加构造函数逻辑
			//
		}
		/// <summary>
		/// 发送单据审批提醒
		/// </summary>
		/// <param name="ordernumber"></param>
		/// <param name="module"></param>
		/// <param name="username"></param>
		public static void sendwarnning(string ordernumber,string module,string username)
		{
			string role;

			Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);

			if(wf.warning != null)
			{
				wf.warning.CreateWarningToRole(ordernumber, 1, username);
			}

			if (wf.check != null)
			{
				role = wf.check.GetFirstCheckRole(ordernumber, username);
			}
			else
			{
				role = "";
			}
			//添加审核提醒
			if (role != null && role.Trim() != "")
			{
				string msg = string.Empty;
				msg = "单号为:" + ordernumber + "的" + wf.property.Title + "已经填写完成,请尽快审核!";
				//发送提醒信息
				if (wf.warning != null)
				{
					CustomWarning.CreateWarningToJobName(ordernumber, msg, module, username, role, 1,wf.check.GetFirstCheckRoleLimitTime());
				}
			}
			else
			{
				string msg = string.Empty;
				msg = "单号为:" + ordernumber + "的" + wf.property.Title + "无审批流程,系统已默认审批完成!";
				string ModuleUrl = "WorkFlow_View.aspx?module=" + module + "&number=" + ordernumber;
				CustomWarning.AddWarning(msg, ModuleUrl, 1, username, username);
			}
		}
	}
}
