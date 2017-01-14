using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// DBproperty 的摘要说明
/// </summary>
namespace FM.ManagerAccount
{
    public class DBproperty
    {
        public DBproperty()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private String UserNumber;
        private String UserName;
        private String UserPassword = "77804d2ba1922c33";
        private String PassQuestion = "77804d2ba1922c33";
        private String PassAnswer = "77804d2ba1922c33";
        private String safeCode = "77804d2ba1922c33";
        private String Email;
        private int IsCorporation = 0;
        private int Integral = 0;
        private int FS_Money = 0;
        private String RegTime = System.DateTime.Now.ToString("yyyy-MM-dd");
        private int GroupID = 0;
        private int isLock = 0;

        public String userNo
        {
            get
            {
                return UserNumber;
            }
            set
            {
                UserNumber = value;
            }
        }

        public String userName
        {
            get
            {
                return UserName;
            }
            set
            {
                UserName = value;
            }
        }

        public String userPassword
        {
            get
            {
                return UserPassword;
            }
            set
            {
                UserPassword = value;
            }
        }

        public String passQuestion
        {
            get
            {
                return PassQuestion;
            }
        }

        public String passAnswer
        {
            get
            {
                return PassAnswer;
            }
        }


        public String SafeCode
        {
            get
            {
                return safeCode;
            }
        }

        public int isCorporation
        {
            get
            {
                return IsCorporation;
            }
        }

        public int integral
        {
            get
            {
                return Integral;
            }
        }

        public int fS_Money
        {
            get
            {
                return FS_Money;
            }
        }

        public String regTime
        {
            get
            {
                return RegTime;
            }
        }

        public int groupID
        {
            get
            {
                return GroupID;
            }
        }

        public int IsLock
        {
            get
            {
                return isLock;
            }
        }


        public String EMAIL
        {
            get
            {
                return Email;
            }
            set
            {
                Email = value + "@fm8844.com";
            }
        }

    }

}