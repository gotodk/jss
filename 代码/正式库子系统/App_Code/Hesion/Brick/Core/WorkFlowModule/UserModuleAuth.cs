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
	/// Ա����ģ��ķ���Ȩ������
	/// </summary>
	public class UserModuleAuth{	

		private bool canAdd;

		private bool canView;

		private bool canModify;

        private int viewType;

        private int modifyType;

        private int roleType;

        //private bool canViewAll;

        //private bool canModifyAll;

        /// <summary>
        /// Ȩ����� <!--1.��λ���� 2.�������� 3.Ա������-->
        /// </summary>
        public int RoleType
        {
            get
            {
                return this.roleType;
            }
            set
            {
                this.roleType = value;
            }
        }

		/// <summary>
		/// �Ƿ��������Ȩ��
		/// </summary>
		public bool CanAdd{	
			get{
				return this.canAdd;
			}	
			set{
				this.canAdd = value;
			}	
		}
	
		/// <summary>
		/// �Ƿ���в鿴Ȩ��
		/// </summary>
		public bool CanView{	
			get{
				return this.canView;
			}	
			set{
				this.canView = value;
			}	
		}
	
		/// <summary>
		/// �Ƿ�����޸�ɾ��Ȩ��
		/// </summary>
		public bool CanModify{	
			get{
				return this.canModify;
			}	
			set{
				this.canModify = value;
			}	
		}
	
		/// <summary>
		/// �Ƿ���в鿴�����˲����ĵ��ݵ�Ȩ��
		/// </summary>
        //public bool CanViewAll{	
        //    get{
        //        return this.canViewAll;
        //    }	
        //    set{
        //        this.canViewAll = value;
        //    }	
        //}
        /// <summary>
        /// �鿴��Ȩ�� <!--1.���� 2.���� 3.ȫ��˾-->
        /// </summary>
        public int  ViewType
        {
            get
            {
                return this.viewType;
            }
            set
            {
                this.viewType = value;
            }
        }
		/// <summary>
		/// �Ƿ�����޸�ɾ�������˲����ĵ��ݵ�Ȩ��
		/// </summary>
        //public bool CanModifyAll{	
        //    get{
        //        return this.canModifyAll;
        //    }	
        //    set{
        //        this.canModifyAll = value;
        //    }	
        //}
        /// <summary>
        /// �޸�Ȩ��
        /// </summary>
        public int ModifyType
        {
            get
            {
                return this.modifyType;
            }
            set
            {
                this.modifyType = value;
            }
        }
	
	
	}
}