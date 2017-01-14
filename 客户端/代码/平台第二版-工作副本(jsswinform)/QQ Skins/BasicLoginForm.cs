using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Com.Seezt.Skins;

namespace Com.Seezt.Skins
{
    public partial class ILogonForm : Form
    {
        private Graphics g = null;
        private Bitmap Bmp = null;
        private Bitmap closeBmp = null;
        private Bitmap minBmp = null;
        private Bitmap maxBmp = null;
        private ImageAttributes imageAttr = null;
        private Brush titleColor = Brushes.Black;
        private int xwidth = 0;
        public ILogonForm()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
            this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
            closeBmp = ResClass.GetResObj("btn_close_normal");
            minBmp = ResClass.GetResObj("btn_mini_normal");
            maxBmp = ResClass.GetResObj("btn_max_normal");
            if (this.WindowState == FormWindowState.Maximized)
            {
                maxBmp = ResClass.GetResObj("btn_restore_normal");
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;
            Bmp = ResClass.GetResObj("LoginPanel_window_windowBkg");
            imageAttr = new ImageAttributes();
            imageAttr.SetColorKey(Bmp.GetPixel(1, 1), Bmp.GetPixel(1, 1));
            g.DrawImage(Bmp, new Rectangle(0, 0, 5, 26), 0, 0, 5, 26, GraphicsUnit.Pixel, imageAttr);
            g.DrawImage(Bmp, new Rectangle(5, 0, this.Width - 10, 26), 5, 0, Bmp.Width - 10, 26, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, 0, 5, 26), Bmp.Width - 5, 0, 5, 26, GraphicsUnit.Pixel, imageAttr);

            g.DrawImage(Bmp, new Rectangle(0, 26, 1, this.Height - 31), 0, 26, 1, Bmp.Height - 31, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 1, 26, 1, this.Height - 31), 0, 26, 1, Bmp.Height - 31, GraphicsUnit.Pixel);

            g.DrawImage(Bmp, new Rectangle(1, 111, this.Width - 2, 15), 5, 27, 5, 15, GraphicsUnit.Pixel);

            g.DrawImage(Bmp, new Rectangle(1, 126, this.Width - 2, this.Height - 156), 5, 42, 5, Bmp.Height - 72, GraphicsUnit.Pixel);

            g.DrawImage(Bmp, new Rectangle(0, this.Height - 30, 5, 30), 0, Bmp.Height - 30, 5, 30, GraphicsUnit.Pixel, imageAttr);
            g.DrawImage(Bmp, new Rectangle(5, this.Height - 30, this.Width - 10, 30), 5, Bmp.Height - 30, Bmp.Width - 10, 30, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, this.Height - 30, 5, 30), Bmp.Width - 5, Bmp.Height - 30, 5, 30, GraphicsUnit.Pixel, imageAttr);
            g.DrawString(this.Text, new Font("Tahoma", 10F, FontStyle.Bold), titleColor, 8, 3);
            Bmp = ResClass.GetResObj("LoginPanel_LoginLogoFrame_background");
            g.DrawImage(Bmp, new Rectangle(1, 22, 5, 89), 0, 0, 5, Bmp.Height, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(6, 22, this.Width-12, 89), 5, 0, Bmp.Width-10, Bmp.Height, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width-6, 22, 5, 89), Bmp.Width - 5, 0, 5, Bmp.Height, GraphicsUnit.Pixel);
            Bmp = ResClass.GetResObj("holiday_normal");
            g.DrawImage(Bmp, new Rectangle((this.Width-233)/2, 22, 233, 89), 12, 0, Bmp.Width - 23, Bmp.Height, GraphicsUnit.Pixel);
        }

        private void INewForm_Resize(object sender, EventArgs e)
        {
            if (this.ControlBox)
            {
                    this.ButtonClose.Left = this.Width - ButtonClose.Width - 2;
            }

            if (this.MinimizeBox)
            {
                if (this.MaximizeBox)
                {
                    this.ButtonMax.Left = this.Width - ButtonClose.Width - ButtonMax.Width;
                    this.ButtonMin.Left = this.Width - ButtonClose.Width - ButtonMax.Width - ButtonMin.Width;
                    xwidth = this.Width - ButtonClose.Width - ButtonMax.Width - ButtonMin.Width;
                }
                else
                {
                    this.ButtonMax.Enabled = false;
                    xwidth = this.Width - ButtonClose.Width - ButtonMin.Width;
                    this.ButtonMin.Left = this.Width - ButtonClose.Width - ButtonMin.Width;
                }
            }
            else {

                xwidth = this.Width - ButtonClose.Width;
            }
            this.Invalidate();
        }

        private void ButtonClose_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Left)
                this.Close();
        }

        private void ButtonClose_MouseDown(object sender, MouseEventArgs e)
        {
            closeBmp = ResClass.GetResObj("btn_close_down");
            ButtonClose.Invalidate();
        }

        private void ButtonClose_MouseUp(object sender, MouseEventArgs e)
        {
            if (!ButtonClose.IsDisposed)
            {
                closeBmp = ResClass.GetResObj("btn_close_normal");
                ButtonClose.Invalidate();
            }
        }

        private void ButtonClose_MouseLeave(object sender, EventArgs e)
        {
            closeBmp = ResClass.GetResObj("btn_close_normal");
            ButtonClose.Invalidate();
        }

        private void ButtonClose_MouseEnter(object sender, EventArgs e)
        {
            closeBmp = ResClass.GetResObj("btn_close_highlight");
            toolTip1.SetToolTip(ButtonClose, "关闭");
            ButtonClose.Invalidate();
        }

        private void ButtonMin_MouseEnter(object sender, EventArgs e)
        {
            minBmp = ResClass.GetResObj("btn_mini_highlight");
            toolTip1.SetToolTip(ButtonMin, "最小化");
            ButtonMin.Invalidate();
        }

        private void ButtonMin_MouseDown(object sender, MouseEventArgs e)
        {
            minBmp = ResClass.GetResObj("btn_mini_down");
            ButtonMin.Invalidate();
        }

        private void ButtonMin_MouseUp(object sender, MouseEventArgs e)
        {
            minBmp = ResClass.GetResObj("btn_close_normal");
            ButtonMin.Invalidate();
        }

        private void ButtonMin_MouseLeave(object sender, EventArgs e)
        {
            minBmp = ResClass.GetResObj("btn_mini_normal");
            ButtonMin.Invalidate();
        }

        private void ButtonMin_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    if (!this.ShowInTaskbar)
            //    {
            //        this.Hide();
            //    }
            //    else
            //    {
            //        this.WindowState = FormWindowState.Minimized;
            //    }
            //}//已经被子类实现
        }

        private void IForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks >= 2 && this.MaximizeBox)
                {
                    ButtonMax_MouseClick(null, new MouseEventArgs(MouseButtons.Left, 1, 2, 2, 0));
                }
                else
                {
                    BasicForm.ReleaseCapture();
                    BasicForm.SendMessage(Handle, 0x00A1, 2, 0);
                }
            }
        }

        private void ButtonMin_Paint(object sender, PaintEventArgs e)
        {
            if (this.MinimizeBox)
            {
                g = e.Graphics;
                g.DrawImage(minBmp, new Rectangle(0, 0, minBmp.Width, minBmp.Height), 0, 0, minBmp.Width, minBmp.Height, GraphicsUnit.Pixel, imageAttr);
            }
        }

        private void ButtonClose_Paint(object sender, PaintEventArgs e)
        {
            if (this.ControlBox)
            {
                g = e.Graphics;
                g.DrawImage(closeBmp, new Rectangle(0, 0, closeBmp.Width, closeBmp.Height), 0, 0, closeBmp.Width, closeBmp.Height, GraphicsUnit.Pixel, imageAttr);
            }
        }

        private void ButtonMax_MouseDown(object sender, MouseEventArgs e)
        {
            g = ButtonMax.CreateGraphics();
            maxBmp = ResClass.GetResObj("btn_max_down");
            if (this.WindowState == FormWindowState.Maximized)
            {
                maxBmp = ResClass.GetResObj("btn_restore_down");
            }
            g.DrawImage(maxBmp, new Rectangle(0, 0, maxBmp.Width, maxBmp.Height), 0, 0, maxBmp.Width, maxBmp.Height, GraphicsUnit.Pixel);
        }

        private void ButtonMax_MouseEnter(object sender, EventArgs e)
        {
            g = ButtonMax.CreateGraphics();
            maxBmp = ResClass.GetResObj("btn_max_highlight");
            toolTip1.SetToolTip(ButtonMax, "最大化");
            if (this.WindowState == FormWindowState.Maximized)
            {
                maxBmp = ResClass.GetResObj("btn_restore_highlight");
                toolTip1.SetToolTip(ButtonMax, "还原");
            }
            g.DrawImage(maxBmp, new Rectangle(0, 0, maxBmp.Width, maxBmp.Height), 0, 0, maxBmp.Width, maxBmp.Height, GraphicsUnit.Pixel);
        }

        private void ButtonMax_MouseLeave(object sender, EventArgs e)
        {
            maxBmp = ResClass.GetResObj("btn_max_normal");
            if (this.WindowState == FormWindowState.Maximized)
            {
                maxBmp = ResClass.GetResObj("btn_restore_normal");
            }
            ButtonMax.Invalidate();
        }

        private void ButtonMax_MouseUp(object sender, MouseEventArgs e)
        {
            maxBmp = ResClass.GetResObj("btn_max_normal");
            if (this.WindowState == FormWindowState.Maximized)
            {
                maxBmp = ResClass.GetResObj("btn_restore_normal");
            }
            ButtonMax.Invalidate();
        }

        private void ButtonMax_Paint(object sender, PaintEventArgs e)
        {
            if (this.MaximizeBox)
            {
                g = e.Graphics;
                maxBmp = ResClass.GetResObj("btn_max_normal");
                if (this.WindowState == FormWindowState.Maximized)
                {
                    maxBmp = ResClass.GetResObj("btn_restore_normal");
                }
                g.DrawImage(maxBmp, new Rectangle(0, 0, maxBmp.Width, maxBmp.Height), 0, 0, maxBmp.Width, maxBmp.Height, GraphicsUnit.Pixel);
            }
        }

        private void IForm_Load(object sender, EventArgs e)
        {
            if (this.ControlBox)
            {
                this.ButtonClose.Visible = true;
                if (!this.MinimizeBox)
                    this.ButtonMin.Visible = false;
                else
                    this.ButtonMin.Visible = true;
                if (!this.MaximizeBox)
                    this.ButtonMax.Visible = false;
                else
                    this.ButtonMax.Visible = true;
            }
            else
            {
                this.ButtonClose.Visible = false;
                this.ButtonMin.Visible = false;
                this.ButtonMax.Visible = false;
            }
        }

        private void IForm_TextChanged(object sender, EventArgs e)
        {
            this.Invalidate(new Rectangle(8, 3,100,25));
        }

        private void IForm_Activated(object sender, EventArgs e)
        {
            titleColor = Brushes.Black;
            this.Invalidate(new Rectangle(8, 3, 100, 25));
        }

        private void IForm_Deactivate(object sender, EventArgs e)
        {
            titleColor = Brushes.LightGray;
            this.Invalidate(new Rectangle(8, 3, 100, 25));
        }
        

        private void ButtonMax_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.WindowState==FormWindowState.Normal)
                    this.WindowState = FormWindowState.Maximized;
                else
                    this.WindowState = FormWindowState.Normal;
            }
        }
    }
}
