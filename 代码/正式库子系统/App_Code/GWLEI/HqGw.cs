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
///HqGw 的摘要说明
/// </summary>
namespace GWLEI
{
    public class HqGw
    {
        private string fileid;
        private string filehqtype;
        private string pid;
        private string pname;
        private string filezz;
        private string filestate;
        private string gwstate;
        private string newfilepath;
        private string newfilename;
        private string oldfilename;
        private string changemess;
        private string changetime; 
        private string jielun;
        private string filetype;
        private string dept;
        public HqGw()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        public string Fileid
        {
            get
            {
                return this.fileid;
            }
            set
            {
                this.fileid = value;
            }
        }
        public string Filehqtype
        {
            get
            {
                return this.filehqtype;
            }
            set
            {
                this.filehqtype = value;
            }
        }
        public string Pid
        {
            get
            {
                return this.pid;
            }
            set
            {
                this.pid= value;
            }
        }
        public string Pname
        {
            get
            {
                return this.pname;
            }
            set
            {
                this.pname= value;
            }
        }
        public string Filezz
        {
            get
            {
                return this.filezz;
            }
            set
            {
                this.filezz = value;
            }
        }
        public string Filestate
        {
            get
            {
                return this.filestate;
            }
            set
            {
                this.filestate = value;
            }
        }
        public string Gwstate
        {
            get
            {
                return this.gwstate;
            }
            set
            {
                this.gwstate = value;
            }
        }
        public string Newfilepath
        {
            get
            {
                return this.newfilepath;
            }
            set
            {
                this.newfilepath= value;
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
        public string Changemess
        {
            get
            {
                return this.changemess;
            }
            set
            {
                this.changemess= value;
            }
        }
        public string Changetime
        {
            get
            {
                return this.changetime;
            }
            set
            {
                this.changetime = value;
            }
        }
        public string Jielun
        {
            get
            {
                return this.jielun;
            }
            set
            {
                this.jielun = value;
            }
        }
        public string Filetype
        {
            get
            {
                return this.filetype;
            }
            set
            {
                this.filetype = value;
            }
        }
        public string Dept
        {
            get
            {
                return this.dept;
            }
            set
            {
                this.dept= value;
            }
        }



    }
}
