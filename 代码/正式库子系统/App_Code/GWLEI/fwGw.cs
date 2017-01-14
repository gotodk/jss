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
///fwGw 的摘要说明
/// </summary>
namespace GWLEI
{
    public class fwGw
    {
        private string cid;
        private string sourcefileid;
        private string pid;
        private string fwpid;
        private string fwtime;
        private string fwstate;
        private string filechangestate;
        private string dinggaotime;
        private string boolconvert;
        public fwGw() 
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        public string Cid
        {
            get
            {
                return this.cid;
            }
            set
            {
                this.cid = value;
            }
        }
        public string Sourcefileid
        {
            get
            {
                return this.sourcefileid;
            }
            set
            {
                this.sourcefileid = value;
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
                this.pid = value;
            }
        }
        public string Fwpid
        {
            get
            {
                return this.fwpid;
            }
            set
            {
                this.fwpid = value;
            }
        }
        public string Fwtime
        {
            get
            {
                return this.fwtime;
            }
            set
            {
                this.fwtime = value;
            }
        }
        public string Fwstate
        {
            get
            {
                return this.fwstate;
            }
            set
            {
                this.fwstate = value;
            }
        }
        public string Filechangestate
        {
            get
            {
                return this.filechangestate;
            }
            set
            {
                this.filechangestate= value;
            }
        }
        public string Dinggaotime
        {
            get
            {
                return this.dinggaotime;
            }
            set
            {
                this.dinggaotime = value;
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

    }
}
