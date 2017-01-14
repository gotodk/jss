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
	/// 工作流模块基本属性
	/// </summary>
	public class Property{	

		private int authType;

		private int moduleType;

		private int lastYear;

		private int lastMonth;

		private int sequence;

		private string moduleName;

		private string title;

		private int smallClassId;


		/// <summary>
		/// 验证类型(0:不验证;1:通过权限机制验证)
		/// </summary>
		public int AuthType{	
			get{
				return this.authType;
			}	
			set{
				this.authType = value;
			}	
		}
	
		/// <summary>
		/// 模块类型(1:工作流;2:文字报表;3:图表;4:自定义模块)
		/// </summary>
		public int ModuleType{	
			get{
				return this.moduleType;
			}	
			set{
				this.moduleType = value;
			}	
		}
	
		/// <summary>
		/// 上一张单据产生的年份
		/// </summary>
		public int LastYear{	
			get{
				return this.lastYear;
			}	
			set{
				this.lastYear = value;
			}	
		}
	
		/// <summary>
		/// 上一张单据产生的月份
		/// </summary>
		public int LastMonth{	
			get{
				return this.lastMonth;
			}	
			set{
				this.lastMonth = value;
			}	
		}
	
		/// <summary>
		/// 上一张单据的序列号
		/// </summary>
		public int Sequence{	
			get{
				return this.sequence;
			}	
			set{
				this.sequence = value;
			}	
		}
	
		/// <summary>
		/// 模块名称
		/// </summary>
		public string ModuleName{	
			get{
				return this.moduleName;
			}	
			set{
				this.moduleName = value;
			}	
		}
	
		/// <summary>
		/// 模块中文名称
		/// </summary>
		public string Title{	
			get{
				return this.title;
			}	
			set{
				this.title = value;
			}	
		}
	
		/// <summary>
		/// 模块所属小类的id
		/// </summary>
		public int SmallClassId{	
			get{
				return this.smallClassId;
			}	
			set{
				this.smallClassId = value;
			}	
		}
	

		/// <summary>
		/// 查询数据集初始化本类的属性
		/// </summary>
		/// <param name="ProRow"></param>
		public Property(DataSet ProRow){
            if (ProRow != null)
            {
                smallClassId = int.Parse(ProRow.Tables[0].Rows[0]["SmallClassId"].ToString());
                authType = int.Parse(ProRow.Tables[0].Rows[0]["AuthType"].ToString());
                moduleType = int.Parse(ProRow.Tables[0].Rows[0]["ModuleType"].ToString());
                lastYear = int.Parse(ProRow.Tables[0].Rows[0]["LastYear"].ToString());
                lastMonth = int.Parse(ProRow.Tables[0].Rows[0]["LastMonth"].ToString());
                sequence = int.Parse(ProRow.Tables[0].Rows[0]["Sequence"].ToString());
                moduleName =ProRow.Tables[0].Rows[0]["name"].ToString();
                title = ProRow.Tables[0].Rows[0]["Title"].ToString();
            }
            else
            {
                return;
            }
			
		}
		
	}
}