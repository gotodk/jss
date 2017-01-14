using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Com.Seezt.Skins;

namespace Com.Seezt.Skins
{
    public partial class BasicMainForm : Form
    {
        private Graphics g = null;
        private Bitmap Bmp = null;
        private Bitmap closeBmp = null;
        private Bitmap minBmp = null;
        private Bitmap maxBmp = null;
        private ImageAttributes imageAttr = null;
        private Brush titleColor = Brushes.Black;

        public BasicMainForm()
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

        private void IMainForm_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            Bmp = ResClass.GetResObj("MainPanel_window_windowBkg");
            imageAttr = new ImageAttributes();
            imageAttr.SetColorKey(Bmp.GetPixel(1, 1), Bmp.GetPixel(1, 1));
            g.DrawImage(Bmp, new Rectangle(0, 0, 100, 121), 0, 0, 100, 121, GraphicsUnit.Pixel, imageAttr);
            g.DrawImage(Bmp, new Rectangle(100, 0, this.Width-190, 121), 100, 0, Bmp.Width-190, 121, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 90, 0, 90, 121), Bmp.Width-90, 0, 90, 121, GraphicsUnit.Pixel, imageAttr);
            g.DrawImage(Bmp, new Rectangle(0, 121, 5, this.Height - 177), 0, 122, 5, 18, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(5, 121, this.Width - 10, this.Height - 177), 5, 121, 5, 10, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, 121, 5, this.Height - 177), Bmp.Width - 5, 121, 5, 10, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(0, this.Height - 56, 100, 56), 0,Bmp.Height - 56, 100, 56, GraphicsUnit.Pixel, imageAttr);
            g.DrawImage(Bmp, new Rectangle(100, this.Height - 56, this.Width-190, 56), 100, Bmp.Height - 56, Bmp.Width-190, 56, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 90, this.Height - 56, 90, 56), Bmp.Width - 90, Bmp.Height - 56, 90, 56, GraphicsUnit.Pixel, imageAttr);
            g.DrawString(this.Text, new Font("Tahoma", 10F, FontStyle.Bold), titleColor, 5, 3);

            Bmp = ResClass.GetResObj("MainPanel_SearchContact_background");
            g.DrawImage(Bmp, new Rectangle(1, 97, 9, 22), 0, 0, 9, 22, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(10, 97, this.Width-20, 22), 9, 0, Bmp.Width-18, 22, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width-10, 97, 9, 22), Bmp.Width-9, 0, 9, 22, GraphicsUnit.Pixel);

            Bmp = ResClass.GetResObj("MainPanel_SearchContact_imageNoInputBtnRightNormal");
            g.DrawImage(Bmp, new Rectangle(this.Width - 36, 97, 35, 22), 0, 0, 35, 22, GraphicsUnit.Pixel, imageAttr);

            Bmp = ResClass.GetResObj("MainPanel_Cutline_file");
            g.DrawImage(Bmp, new Rectangle(44, this.Height-29, this.Width-50, 2), 0, 0, 149, 2, GraphicsUnit.Pixel);

            Bmp = ResClass.GetResObj("menu_btn_normal");
            g.DrawImage(Bmp, new Rectangle(2, this.Height - 43, 42, 42), 0, 0, 42, 42, GraphicsUnit.Pixel);

            Bmp = ResClass.GetResObj("mainpanel_tabctrl_background");
            g.DrawImage(Bmp, new Rectangle(1, 120, 33, this.Height - 136-56), 0, 0, 33, 75, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(1, this.Height - 137 - 56, 33, 138), 0, 75, 33, 136, GraphicsUnit.Pixel);
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
            {
                if (!this.ShowInTaskbar)
                {
                    this.Hide();
                }
                else {
                    this.WindowState = FormWindowState.Minimized;
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
            this.Invalidate(new Rectangle(5, 1,100,31));
        }

        private void IForm_Activated(object sender, EventArgs e)
        {
            titleColor = Brushes.Black;
            this.Invalidate(new Rectangle(5, 1, 100, 31));
        }

        private void IForm_Deactivate(object sender, EventArgs e)
        {
            titleColor = Brushes.Black;
            this.Invalidate(new Rectangle(5, 1, 100, 31));
        }

        private void ButtonMax_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.WindowState==FormWindowState.Normal)
                    this.WindowState = FormWindowState.Maximized;
                else
                    this.WindowState = FormWindowState.Normal;
                this.Invalidate();
            }
        }

        private void IForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks >= 2 && this.MaximizeBox)
                {
                    if (e.Y <= 73)
                    {
                        ButtonMax_MouseClick(null, new MouseEventArgs(MouseButtons.Left, 1, 2, 2, 0));
                    }
                }
                else
                {
                    if (this.Cursor != Cursors.Default)
                    {
                        oldPoint = Control.MousePosition;
                        oldSize = this.Size;
                        if (e.X > this.Width - 5 || e.X < 5 && (e.Y > 5 && e.Y < this.Height - 5))
                        {
                            isVsize = true;
                        }
                        else if (e.Y > this.Height - 5 || e.Y < 5 && (e.X > 5 && e.X < this.Width - 5))
                        {
                            isHsize = true;
                        }
                        
                    }
                    else if (e.Y <= 73 || e.Y < this.Height && e.Y > this.Height - 56)
                    {
                        BasicForm.ReleaseCapture();
                        BasicForm.SendMessage(Handle, 0x00A1, 2, 0);
                    }
                }
            }
        }
        private bool isHsize, isVsize =false;
        private Point oldPoint;
        private Size oldSize;
        private void IMainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isHsize)
            {
                Point newPoint = Control.MousePosition;
                this.Height = oldSize.Height + newPoint.Y - oldPoint.Y;
                this.Invalidate(true);
            }
            else if (isVsize) 
            {
                Point newPoint = Control.MousePosition;
                this.Width = oldSize.Width + newPoint.X - oldPoint.X;
                this.Invalidate(true);
            }
            else
            {
                if (e.X > this.Width - 5 || e.X < 5 && (e.Y > 5 && e.Y < this.Height - 5))
                {
                    this.Cursor = Cursors.SizeWE;
                }
                else if (e.Y > this.Height - 5 || e.Y < 5 && (e.X > 5 && e.X < this.Width - 5))
                {
                    this.Cursor = Cursors.SizeNS;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void IMainForm_MouseUp(object sender, MouseEventArgs e)
        {
            isVsize = isHsize = false;
        }

        private bool mini = false;
        protected override void OnMouseEnter(EventArgs e)
        {
            if (mini && WindowState == FormWindowState.Normal)
                miniTimer.Enabled = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (this.Top == 0 && WindowState == FormWindowState.Normal)
            {
                Point p = Control.MousePosition;
                if (p.X < this.Left || p.X > this.Left + this.Width || p.Y > this.Top + this.Height)
                    miniTimer.Enabled = true;
            }
        }

        private void miniTimer_Tick(object sender, EventArgs e)
        {
            if (mini)
            {
                if (this.Top != 0)
                    this.Top = 0;
                else
                {
                    mini = false;
                    miniTimer.Enabled = false;
                }
            }
            else
            {
                if (this.Top == -(this.Height - 3))
                {
                    mini = true;
                    miniTimer.Enabled = false;
                }
                else
                    this.Top = -(this.Height - 3);
            }
        }
    }
}
