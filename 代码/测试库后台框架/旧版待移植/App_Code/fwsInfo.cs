using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
///直通车空间
/// </summary>
namespace ZTC
{
    /// <summary>
    /// 服务商类
    /// </summary>
    public class fwsInfo
    {
        public fwsInfo()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //           
        }
        private string fwsid;
        /// <summary>
        /// 服务商编号
        /// </summary>
        public string fwsID
        {
            get { return this.fwsid; }
            set { this.fwsid = value; }
        }

        private string fwsname;
        /// <summary>
        /// 服务商名称
        /// </summary>
        public string fwsName
        {
            get { return this.fwsname; }
            set { fwsname= value; }
        }

        private string fwsssbsc;
        /// <summary>
        /// 服务商所属办事处
        /// </summary>
        public string fwsSSBSC
        {
            get { return this.fwsssbsc; }
            set { fwsssbsc = value; }
        }

        private double fwsanum;
        /// <summary>
        /// 服务商A分
        /// </summary>
        public double fwsANum
        {
            get { return this.fwsanum; }
            set { this.fwsanum = value; }
        }
        private double fwsuserhynum;
        /// <summary>
        /// 服务商所属用户的总活跃值
        /// </summary>
        public double fwsUserHyNum
        {
            get { return this.fwsuserhynum; }
            set { this.fwsuserhynum = value; }
        }

        private double fwskf;
        /// <summary>
        /// 服务商扣分
        /// </summary>
        public double fwsKF
        {
            get { return this.fwskf; }
            set { this.fwskf = value; }
        }
        private int fwsxj;
        /// <summary>
        /// 服务商星级
        /// </summary>
        public int fwsXJ
        {
            get{return this.fwsxj;}
            set{ this.fwsxj = value;}
        }
        /// <summary>
        /// 服务商等级:星，钻，冠
        /// </summary>
        public string fwsDJ
        {
            get;
            set;
        }
        private double fwszf;
        /// <summary>
        /// 服务商总分
        /// </summary>
        public double fwsZF
        {
            get { return this.fwszf; }
            set { this.fwszf = value; }
        }
        /// <summary>
        /// 服务商下一级相差分数
        /// </summary>
        public double fwsNextF
        {
            get;
            set;
        }
        private string fwsstarpath;
        /// <summary>
        /// 服务商星级图片路径
        /// </summary>
        public string fwsStarPath
        {
            get { return this.fwsstarpath; }
            set { this.fwsstarpath = value; }
        }
        /// <summary>
        /// 联系人
        /// </summary>
        public string fwsLXR
        {
            get;
            set;
        }
        /// <summary>
        /// 服务商固话
        /// </summary>
        public string fwsGH
        {
            get;
            set;
        }
        /// <summary>
        /// 服务商省份
        /// </summary>
        public string fwsSF
        {
            get;
            set;
        }

    
    }
}