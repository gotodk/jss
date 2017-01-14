using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
/// <summary>
///DLDate 的摘要说明
/// </summary>
namespace ShareLiu
{
    public class DLDate
    {
        public DLDate()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 绑定GridView
        /// </summary>
        /// <param name="gv">GridView控件名</param>
        /// <param name="ds">DataSet记录集</param>
        public static void BindGridView(GridView gv,DataSet ds)
        {
            gv.DataSource = ds;
            gv.DataBind();
        }
    }
}
