using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

/// <summary>
///SwFatherObj 的摘要说明
/// </summary>
namespace ShareLiu.SwClasses
{
    public class SwFatherObj
    {


        private string smid;
        private string modetype;
        private string modename;
        private string smdoctitle;
        private string advices;
        private string userid;
        private string username;
        private string deptname;
        private string newfilename;
        private string newfilepath;
        private string oldfilename;
        private string oldfilepath;
      //  private string spid;
       // private string spname;
        private string smstates;
        private DateTime createtime;
        private string textArea;
      //  private DateTime sptime;
        private int islocked;
        public SwFatherObj()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }


        public int Islocked
        {
            get
            {
                return this.islocked;
            }
            set
            {
                this.islocked = value;
            }
        }
        public string Modetype
        {
            get
            {
                return this.modetype;
            }
            set
            {
                this.modetype = value;
            }
        }
        public string Modename
        {
            get
            {
                return this.modename;
            }
            set
            {
                this.modename = value;
            }
        }

        public string TextAre
        {
            get
            {
                return textArea;
            }
            set
            {
                this.textArea = value;
            }
        }


        public string Smdoctitle
        {
            get
            {
                return this.smdoctitle;

            }
            set
            {
                this.smdoctitle = value;
            }
        }
        public string Advices
        {
            get
            {
                return this.advices;
            }
            set
            {
                this.advices = value;
            }
        }
        public string Userid
        {
            get
            {
                return this.userid;
            }
            set
            {
                this.userid = value;
            }
        }
        public string Username
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = value;
            }
        }
        public string Deptname
        {
            get
            {
                return this.deptname;
            }
            set
            {
                this.deptname = value;
            }
        }
        public string Newfilename
        {
            get
            {
                return this.newfilename;
            }
            set
            {
                this.newfilename = value;
            }
        }
        public string NewFilepath
        {
            get
            {
                return this.newfilepath;
            }
            set
            {
                this.newfilepath = value;
            }
        }
        public string Oldfilename
        {
            get
            {
                return this.oldfilename;
            }
            set
            {
                this.oldfilename = value;
            }
        }
        public string Oldfilepath
        {
            get
            {
                return this.oldfilepath;
            }
            set
            {
                this.oldfilepath = value;
            }
        }
        //public string Spid
        //{
        //    get
        //    {
        //        return this.spid;
        //    }
        //    set
        //    {
        //        this.spid = value;
        //    }
        //}
        //public string Spname
        //{
        //    get
        //    {
        //        return this.spname;
        //    }
        //    set
        //    {
        //        this.spname = value;
        //    }
        //}
        public string Smstates
        {
            get
            {
                return this.smstates;
            }
            set
            {
                this.smstates = value;
            }
        }
        public DateTime Createtime
        {
            get
            {
                return this.createtime;
            }
            set
            {
                this.createtime = value;
            }
        }
        //public DateTime Sptime
        //{
        //    get
        //    {
        //        return this.sptime;
        //    }
        //    set
        //    {
        //        this.sptime = value;
        //    }
        //}

    }

}
