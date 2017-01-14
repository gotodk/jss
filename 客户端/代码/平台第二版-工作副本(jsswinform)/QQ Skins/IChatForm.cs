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
    public partial class IChatForm : Form
    {
        private Graphics g = null;
        private Bitmap Bmp = null;
        private Bitmap closeBmp = null;
        private Bitmap minBmp = null;
        private Bitmap maxBmp = null;
        private ImageAttributes imageAttr = null;
        private int xwidth = 0;

        public IChatForm()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
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

        private void IChatForm_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            Bmp = ResClass.GetResObj("ChatFrame_Window_windowBkg");
            imageAttr = new ImageAttributes();
            imageAttr.SetColorKey(Bmp.GetPixel(1, 1), Bmp.GetPixel(1, 1));
            g.DrawImage(Bmp, new Rectangle(0, 0, 50, 100), 0, 0, 50, 100, GraphicsUnit.Pixel, imageAttr);
            g.DrawImage(Bmp, new Rectangle(50, 0, this.Width - 120, 100), 50, 0, Bmp.Width - 120, 100, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 80, 0, 80, 100), Bmp.Width - 80, 0, 80, 100, GraphicsUnit.Pixel, imageAttr);
            g.DrawImage(Bmp, new Rectangle(0, 100, 5, this.Height - 105), 0, 100, 5, Bmp.Height - 105, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(5, 100, this.Width - 10, this.Height - 105), 5, 100, 5, Bmp.Height - 105, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, 100, 5, this.Height - 105), Bmp.Width - 5, 100, 5, Bmp.Height - 105, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(0, this.Height - 5, 5, 5), 0, Bmp.Height - 5, 5, 5, GraphicsUnit.Pixel, imageAttr);
            g.DrawImage(Bmp, new Rectangle(5, this.Height - 5, this.Width - 10, 5), 5, Bmp.Height - 5, Bmp.Width - 10, 5, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, this.Height - 5, 5, 5), Bmp.Width - 5, Bmp.Height - 5, 5, 5, GraphicsUnit.Pixel, imageAttr);

            Bmp = ResClass.GetResObj("ChatFrame_ShowMsgFrame_background");
            g.DrawImage(Bmp, new Rectangle(5, 85, 5, 15), 0, 0, 5, 20, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(10, 85, this.Width - 170, 15), 5, 0, Bmp.Width - 10, 20, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 165, 85, 5, 15), Bmp.Width - 5, 0, 5, 20, GraphicsUnit.Pixel);
            
            g.DrawImage(Bmp, new Rectangle(5, 100, 5, this.Height - 255), 0, 15, 5, Bmp.Height - 17, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(10, 100, this.Width - 170, this.Height - 255), 5, 15, 5, Bmp.Height - 17, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 165, 100, 5, this.Height - 255), Bmp.Width - 5, 15, 5, Bmp.Height - 17, GraphicsUnit.Pixel);
            
            g.DrawImage(Bmp, new Rectangle(5, this.Height - 156, 5, 1), 0, Bmp.Height - 1, 5, 1, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(10, this.Height - 156, this.Width - 170, 1), 5, Bmp.Height - 1, Bmp.Width - 10, 1, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 165, this.Height - 156, 5, 1), Bmp.Width - 5, Bmp.Height - 1, 5, 1, GraphicsUnit.Pixel);

            Bmp = ResClass.GetResObj("ChatFrame_EditMsgFrame_background");
            g.DrawImage(Bmp, new Rectangle(5, 357, 5, 5), 0, 0, 5, 5, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(10, 357, this.Width - 170, 5), 5, 0, Bmp.Width - 10, 5, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 165, 357, 5, 5), Bmp.Width - 5, 0, 5, 5, GraphicsUnit.Pixel);
            
            g.DrawImage(Bmp, new Rectangle(5, 362, 5, this.Height - 408), 0, 5, 5, Bmp.Height - 20, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(10, 362, this.Width - 170, this.Height - 408), 5, 5, 5, Bmp.Height - 20, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 165, 362, 5, this.Height - 408), Bmp.Width - 5, 5, 5, Bmp.Height - 20, GraphicsUnit.Pixel);
            
            g.DrawImage(Bmp, new Rectangle(5, this.Height - 46, 5, 10), 0, Bmp.Height - 10, 5, 10, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(10, this.Height - 46, this.Width - 170, 10), 5, Bmp.Height - 10, Bmp.Width - 10, 10, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 165, this.Height - 46, 5, 10), Bmp.Width - 5, Bmp.Height - 10, 5, 10, GraphicsUnit.Pixel);
        }

        private void IChatForm_Resize(object sender, EventArgs e)
        {
            this.ButtonClose.Left = this.Width - ButtonClose.Width;
            this.ButtonMax.Left = this.Width - ButtonClose.Width - ButtonMax.Width;
            this.ButtonMin.Left = this.Width - ButtonClose.Width - ButtonMax.Width - ButtonMin.Width;
            this.Refresh();
        }

        private void ButtonClose_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
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
            if (e.Button == MouseButtons.Left)
                this.WindowState = FormWindowState.Minimized;
        }

        private void IChatForm_MouseDown(object sender, MouseEventArgs e)
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

        private void ButtonMax_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void IForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.Y <= 50 && e.X < xwidth)
                {
                    //topPanel_MouseClick(null, e);
                }
            }
        }

        private void qqShowBg_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            Bmp = ResClass.GetResObj("ChatFrame_QQShow_background");
            g.DrawImage(Bmp, new Rectangle(0, 0, 5, 5), 0, 0, 5, 5, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(5, 0, this.qqShowBg.Width - 10, 5), 5, 0, Bmp.Width - 10, 5, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.qqShowBg.Width - 5, 0, 5, 5), Bmp.Width - 5, 0, 5, 5, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(0, 5, 5, this.qqShowBg.Height - 15), 0, 5, 5, Bmp.Height - 20, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(5, 5, this.qqShowBg.Width - 10, this.qqShowBg.Height - 15), 5, 5, 5, Bmp.Height - 20, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.qqShowBg.Width - 5, 5, 5, this.qqShowBg.Height - 15), Bmp.Width - 5, 5, 5, Bmp.Height - 20, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(0, this.qqShowBg.Height - 10, 5, 10), 0, Bmp.Height - 10, 5, 10, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(5, this.qqShowBg.Height - 10, this.qqShowBg.Width - 10, 10), 5, Bmp.Height - 10, Bmp.Width - 10, 10, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.qqShowBg.Width - 5, this.qqShowBg.Height - 10, 5, 10), Bmp.Width - 5, Bmp.Height - 10, 5, 10, GraphicsUnit.Pixel);
        }

        private void toolBarBg_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            Bmp = ResClass.GetResObj("ChatFrame_QuickbarFrame_background");
            g.DrawImage(Bmp, new Rectangle(0, 0, 2, 22), 0, 0, 2, 22, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(2, 0, this.Width - 169, 22), 2, 0, Bmp.Width - 4, 22, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 167, 0, 2, 22), Bmp.Width - 2, 0, 2, 22, GraphicsUnit.Pixel);
            //g.DrawImage(Bmp, new Rectangle(0, 5, 5, this.EditMsgBg.Height - 15), 0, 5, 5, Bmp.Height - 20, GraphicsUnit.Pixel);
            //g.DrawImage(Bmp, new Rectangle(5, 5, this.EditMsgBg.Width - 10, this.EditMsgBg.Height - 15), 5, 5, 5, Bmp.Height - 20, GraphicsUnit.Pixel);
            //g.DrawImage(Bmp, new Rectangle(this.EditMsgBg.Width - 5, 5, 5, this.EditMsgBg.Height - 15), Bmp.Width - 5, 5, 5, Bmp.Height - 20, GraphicsUnit.Pixel);
            //g.DrawImage(Bmp, new Rectangle(0, this.EditMsgBg.Height - 10, 5, 10), 0, Bmp.Height - 10, 5, 10, GraphicsUnit.Pixel);
            //g.DrawImage(Bmp, new Rectangle(5, this.EditMsgBg.Height - 10, this.EditMsgBg.Width - 10, 10), 5, Bmp.Height - 10, Bmp.Width - 10, 10, GraphicsUnit.Pixel);
            //g.DrawImage(Bmp, new Rectangle(this.EditMsgBg.Width - 5, this.EditMsgBg.Height - 10, 5, 10), Bmp.Width - 5, Bmp.Height - 10, 5, 10, GraphicsUnit.Pixel);
        }
    }
}
