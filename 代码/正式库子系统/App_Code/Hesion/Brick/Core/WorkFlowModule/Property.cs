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
	/// ������ģ���������
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
		/// ��֤����(0:����֤;1:ͨ��Ȩ�޻�����֤)
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
		/// ģ������(1:������;2:���ֱ���;3:ͼ��;4:�Զ���ģ��)
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
		/// ��һ�ŵ��ݲ��������
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
		/// ��һ�ŵ��ݲ������·�
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
		/// ��һ�ŵ��ݵ����к�
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
		/// ģ������
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
		/// ģ����������
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
		/// ģ������С���id
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
		/// ��ѯ���ݼ���ʼ�����������
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