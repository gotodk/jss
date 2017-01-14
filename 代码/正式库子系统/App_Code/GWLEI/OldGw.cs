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
///OldGw 的摘要说明
/// </summary>
namespace GWLEI
{
    public class OldGw
    {
        private string pid;
        private string pname;
        private string filezz;
        private string filehqtype;
        private string filestate;
        private string filepath;
        private string oldfilename;
        private string newfilename;
        private string fujianname;
        private byte[] fujianfile;
        private bool fujianexit;
        private string filetype;
        private string dept;
        private string convertfilehqtype;
        private string converttype; //再审转档，一般转档
        private string convertfileid; //转档源id
        private string boolconvert;
        private string gwdinggao;
        public OldGw()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //


        }
        public string Pid
        {
            get
            {
                return this.pid;
            }
            set
            {
                this.pid = value;
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
             this.pname=value;
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
        public string Fildstate
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
        public string Filepath
        {
            get
            {
                return this.filepath; 

            }
            set
            {
                this.filepath = value;
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
        public string Fujianname
        {
            get
            {
                return this.fujianname;
            }
            set
            {
                this.fujianname = value;
            }
        }
        public byte[] Fujianfile
        {
            get
            {
                return this.fujianfile;
            }
            set
            {
                this.fujianfile = value;
            }
        }
        public bool Fujianexit
        {
            get
            {
                return this.fujianexit;
            }
            set
            {
                this.fujianexit = value;
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
        public string Convertfilehqtype
        {
            get
            {
                return this.convertfilehqtype;
            }
            set
            {
                this.convertfilehqtype = value;
            }
        }

        public string Converttype
        {
            get
            {
                return this.converttype;
            }
            set
            {
                this.converttype= value;
            }
        }

        public string Convertfileid
        {
            get
            {
                return this.convertfileid;
            }
            set
            {
                this.convertfileid = value;
            }
        }
        public string Boolconvert
        {
            get
            {
                return this.boolconvert;
            }
            set
            {
                this.boolconvert = value;
            }
        }
        public string Gwdinggao
        {
            get
            {
                return this.gwdinggao;
            }
            set
            {
                this.gwdinggao = value;
            }
        }

    }
}
