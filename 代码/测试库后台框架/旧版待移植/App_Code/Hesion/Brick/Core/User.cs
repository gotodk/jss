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
	/// Ա����Ϣ��
	/// </summary>
	public class User{	

		private string number;

		private string name;

		private string jobName;

		private string jobNumber;

		private string deptName;

        private string lsName;



		/// <summary>
		/// Ա����
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
		/// Ա������
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
		/// Ա����λ����
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
		/// Ա����λ���
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
		/// Ա����������
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
        /// Ա������
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