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

namespace Hesion.Brick.Core{



	/// <summary>
	/// 员工信息类
	/// </summary>
	public class User{	

		private string number;

		private string name;

		private string jobName;

		private string jobNumber;

		private string deptName;

        private string lsName;



		/// <summary>
		/// 员工号
		/// </summary>
		public string Number{	
			get{
				return this.number;
			}	
			set{
				this.number = value;
			}	
		}
	
		/// <summary>
		/// 员工名称
		/// </summary>
		public string Name{	
			get{
				return this.name;
			}	
			set{
				this.name = value;
			}	
		}
	
		/// <summary>
		/// 员工岗位名称
		/// </summary>
		public string JobName{
			get{
				return this.jobName;
			}	
			set{
				this.jobName = value;
			}	
		}
	
		/// <summary>
		/// 员工岗位编号
		/// </summary>
		public string JobNumber{	
			get{
				return this.jobNumber;
			}	
			set{
				this.jobNumber = value;
			}	
		}
	
		/// <summary>
		/// 员工所属部门
		/// </summary>
		public string DeptName{	
			get{
				return this.deptName;
			}	
			set{
				this.deptName = value;
			}	
		}


        /// <summary>
        /// 员工隶属
        /// </summary>
        public string LSName
        {
            get
            {
                return this.lsName;
            }
            set
            {
                this.lsName = value;
            }
        }
	
	}
}