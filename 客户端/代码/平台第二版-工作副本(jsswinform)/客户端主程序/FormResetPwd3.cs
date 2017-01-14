using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using System.Threading;
using 客户端主程序.DataControl;
using System.Text.RegularExpressions;

namespace 客户端主程序
{
    public partial class FormResetPwd3 : BasicForm
    {
        /// <summary>
        /// 验证码（在第一步中首先生成，返回到服务器，然后服务器插入记录，反向查询后返回该验证码）
        /// </summary>
        public string ValsNumber;
        /// <summary>
        /// 将要修改密码的用户email
        /// </summary>
        public string uid;
        /// <summary>
        /// 由第一步查询返回的DataSet
        /// </summary>
        public DataSet dsReturn;
        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="vals">验证码</param>
        /// <param name="email">用户Email</param>
        public FormResetPwd3(string vals, string email, DataSet ds)
        {
            InitializeComponent();
            this.ValsNumber = vals;
            this.uid = email;
            this.dsReturn = ds;
        }
        /// <summary>
        /// 再次发送验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbSendMsgAgain_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {

        }
    }
}
