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
	/// 员工对模块的访问权限属性
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
        /// 权限类别 <!--1.岗位名称 2.部门名称 3.员工工号-->
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
		/// 是否具有增加权限
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
		/// 是否具有查看权限
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
		/// 是否具有修改删除权限
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
		/// 是否具有查看所有人产生的单据的权限
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
        /// 查看的权限 <!--1.个人 2.部门 3.全公司-->
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
		/// 是否具有修改删除所有人产生的单据的权限
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
        /// 修改权限
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