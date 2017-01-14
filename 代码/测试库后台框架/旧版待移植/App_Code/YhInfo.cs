using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
///YhInfo 的摘要说明
///直通车用户类
/// </summary>
namespace ZTC
{


    public class YhInfo
    {
  
        private string username; //用户名称
        private string usergzdw; //用户工作单位
        private string userfwsid;  //用户服务商ID
        private string userfwsname; //用户服务商名称
        private string userssbsc;//所属办事处
        private double userdsnum;      //点数 
        private int userxj;           //星级
        private string userstarpath; //路径
        public YhInfo()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //         
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get { return this.username; }
            set { username = value; }
        }
        /// <summary>
        /// 用户工作单位
        /// </summary>
        public string UserGzdw
        {
            get { return this.usergzdw; }
            set { this.usergzdw = value; }

        }
        /// <summary>
        /// 用户服务商ID
        /// </summary>
        public string UserFwsID
        {
            get {return this.userfwsid;}
            set {this.userfwsid=value; }

        }
        /// <summary>
        /// 用户服务商名称
        /// </summary>
        public string UserFwsName
        {
            get { return this.userfwsname; }
            set { this.userfwsname = value; }

        }
        /// <summary>
        /// 用户所属办事处
        /// </summary>
        public string UserSSBSC
        {
            get { return this.userssbsc; }
            set { this.userssbsc = value; }

        }

        /// <summary>
        /// 用户点数
        /// </summary>
        public double UserDsNum
        {
            get;
            set; 

        }
        /// <summary>
        /// 用户下一级点数
        /// </summary>
        public double UserNextDs
        {
            get;
            set;

        }

        /// <summary>
        /// 用户星级
        /// </summary>
        public int UserXJ
        {
            get { return this.userxj; }
            set { this.userxj = value; }
        }
        /// <summary>
        /// 用户等级
        /// </summary>
        public string UserDJ
        {
            get;
            set;
        }
  
        /// <summary>
        /// 用户星级图片路径
        /// </summary>
        public string UserStarPath
        {
            get { return this.userstarpath; }
            set { this.userstarpath= value; }
        }


    }


}