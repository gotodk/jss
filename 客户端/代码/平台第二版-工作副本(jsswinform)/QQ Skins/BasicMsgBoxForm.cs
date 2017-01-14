using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Com.Seezt.Skins;

namespace Com.Seezt.Skins
{
    public partial class BasicMsgBoxForm : Com.Seezt.Skins.BasicForm1
    {
        private string message = "";
        private int mode = 1;

        public BasicMsgBoxForm(string message,string title,MessageBoxButtons mbb,MessageBoxIcon mbi)
        {
            this.message=message;
            this.Text=title;
            InitializeComponent();
            switch (mbb)
            {
                case MessageBoxButtons.AbortRetryIgnore:
                    break;
                case MessageBoxButtons.OK:
                    ok.Left = this.Width - ok.Width - 10;
                    break;
                case MessageBoxButtons.OKCancel:
                    mode = 1;
                    ok.Texts = "确定";
                    canel.Texts = "取消";
                    canel.Visible = true;
                    break;
                case MessageBoxButtons.RetryCancel:
                    no.Visible = true;
                    ok.Visible = false;
                    Retry.Visible = true;
                    break;
                case MessageBoxButtons.YesNo:
                    mode = 2;
                    ok.Left = ok.Left-30;
                    canel.Left = canel.Left - 30;
                    ok.Texts = "是";
                    canel.Texts = "否";
                    canel.Visible = true;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    mode = 3;
                    ok.Texts = "是";
                    no.Texts = "取消";
                    canel.Texts = "否";
                    canel.Visible = true;
                    no.Visible = true;
                    break;
            }

            switch (mbi)
            {
                case MessageBoxIcon.Asterisk:
                    iconPic.Image = ResClass.GetResObj("sysmessagebox_inforFile");
                    break;
                case MessageBoxIcon.Error:
                    iconPic.Image = ResClass.GetResObj("sysmessagebox_errorFile");
                    break;
                case MessageBoxIcon.Exclamation:
                    iconPic.Image = ResClass.GetResObj("sysmessagebox_warningFile");
                    break;
                case MessageBoxIcon.None:
                    break;
                case MessageBoxIcon.Question:
                    iconPic.Image = ResClass.GetResObj("sysmessagebox_questionFile");
                    break;
            }
        }

        private void IMsgBoxForm_Load(object sender, EventArgs e)
        {
            label1.Text = message;
        }

        private void iButton1_Click(object sender, EventArgs e)
        {
            if (mode == 1)
            {
                this.DialogResult = DialogResult.OK;
            }
            if (mode == 2)
            {
                this.DialogResult = DialogResult.Yes;
            }
            this.Close();
        }

        private void canel_Click(object sender, EventArgs e)
        {
            if (mode == 2)
            {
                this.DialogResult = DialogResult.No;
            }
            this.Close();
        }

        private void IMsgBoxForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                iButton1_Click(null,null);
            }
        }

        private void Retry_MouseClick(object sender, MouseEventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void no_MouseClick(object sender, MouseEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
