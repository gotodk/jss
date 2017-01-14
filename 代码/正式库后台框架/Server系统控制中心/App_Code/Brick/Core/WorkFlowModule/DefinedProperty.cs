using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;
using System.Xml;

namespace Hesion.Brick.Core.WorkFlow
{
    /// <summary>
    /// DefinedProperty 的摘要说明
    /// </summary>
    public class DefinedProperty
    {
        private string url;

        public string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
            }
        }
        /// <summary>
        /// 通过xml节点构造本类
        /// </summary>
        /// <param name="Node"></param>
        public DefinedProperty(XmlNode Node)
        {
            if (Node != null)
            {
                try
                {
                    url = Node["Url"].InnerXml;
                }
                catch
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
}