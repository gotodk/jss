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

namespace Hesion.Brick.Core.WorkFlow{



	/// <summary>
	/// 附加属性
	/// </summary>
	public class AdvProperty{	

		private string ridirect_AfterModify;

		private string ridirect_AfterDelete;

		private string js_AfterDelete;

		private string sp_OnDelete;

		private string sp_OnModify;

		private string js_AfterModify;

		private string addOnJsFile;

		private string sp_OnAdd;

		private string js_AfterAdd;

		private string ridirect_AfterAdd;

		private string addOnCssFile;


		/// <summary>
		/// 修改之后跳转url
		/// </summary>
		public string Ridirect_AfterModify{	
			get{
				return this.ridirect_AfterModify;
			}	
			set{
				this.ridirect_AfterModify = value;
			}	
		}
	
		/// <summary>
		/// 删除之后跳转url
		/// </summary>
		public string Ridirect_AfterDelete{	
			get{
				return this.ridirect_AfterDelete;
			}	
			set{
				this.ridirect_AfterDelete = value;
			}	
		}
	
		/// <summary>
		/// 删除之后执行的js函数名
		/// </summary>
		public string Js_AfterDelete{	
			get{
				return this.js_AfterDelete;
			}	
			set{
				this.js_AfterDelete = value;
			}	
		}
	
		/// <summary>
		/// 删除之后执行的存储过程名
		/// </summary>
		public string Sp_OnDelete{	
			get{
				return this.sp_OnDelete;
			}	
			set{
				this.sp_OnDelete = value;
			}	
		}
	
		/// <summary>
		/// 修改之后执行的存储过程名
		/// </summary>
		public string Sp_OnModify{	
			get{
				return this.sp_OnModify;
			}	
			set{
				this.sp_OnModify = value;
			}	
		}
	
		/// <summary>
		/// 修改之后执行的js函数名
		/// </summary>
		public string Js_AfterModify{	
			get{
				return this.js_AfterModify;
			}	
			set{
				this.js_AfterModify = value;
			}	
		}
	
		/// <summary>
		/// 附加的js文件名
		/// </summary>
		public string AddOnJsFile{	
			get{
				return this.addOnJsFile;
			}	
			set{
				this.addOnJsFile = value;
			}	
		}
	
		/// <summary>
		/// 增加之后执行的存储过程名
		/// </summary>
		public string Sp_OnAdd{	
			get{
				return this.sp_OnAdd;
			}	
			set{
				this.sp_OnAdd = value;
			}	
		}
	
		/// <summary>
		/// 增加之后执行的js函数名
		/// </summary>
		public string Js_AfterAdd{	
			get{
				return this.js_AfterAdd;
			}	
			set{
				this.js_AfterAdd = value;
			}	
		}
	
		/// <summary>
		/// 增加之后跳转url
		/// </summary>
		public string Ridirect_AfterAdd{	
			get{
				return this.ridirect_AfterAdd;
			}	
			set{
				this.ridirect_AfterAdd = value;
			}	
		}
	
		/// <summary>
		/// 附加的样式表文件名
		/// </summary>
		public string AddOnCssFile{	
			get{
				return this.addOnCssFile;
			}	
			set{
				this.addOnCssFile = value;
			}	
		}
	
        
		/// <summary>
		/// 通过xml节点构造本类
		/// </summary>
		/// <param name="Node"></param>
		public AdvProperty(XmlNode Node){
            if (Node != null)
            {
                try
                {
                    addOnCssFile = Node["AddOnCssFile"].InnerXml;
                    ridirect_AfterModify = Node["Ridirect_AfterModify"].InnerXml;
                    ridirect_AfterDelete = Node["Ridirect_AfterDelete"].InnerXml;
                    js_AfterDelete = Node["JS_AfterDelete"].InnerXml;
                    sp_OnDelete = Node["SP_OnDelete"].InnerXml;
                    sp_OnModify = Node["SP_OnModify"].InnerXml;
                    js_AfterModify = Node["JS_AfterModify"].InnerXml;
                    addOnJsFile = Node["AddOnJsFile"].InnerXml;
                    sp_OnAdd = Node["SP_OnAdd"].InnerXml;
                    js_AfterAdd = Node["JS_AfterAdd"].InnerXml;
                    ridirect_AfterAdd = Node["Ridirect_AfterModify"].InnerXml;
                }
                catch
                {
                    return;
                }
            }
            else
            {
                return ;
            }
		}
		
	}
}