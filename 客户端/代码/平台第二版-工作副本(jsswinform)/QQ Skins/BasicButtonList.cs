using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Com.Seezt.Skins
{
    public partial class BasicButtonList : UserControl
    {
        public BasicButtonList()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private LinkButton[] buttons;

        [Description("按钮"), Category("Appearance")]
        public LinkButton[] Buttons
        {
            get
            {
                return buttons;
            }
            set
            {
                buttons = value;
                ChangeUI();
            }
        }

        private void ChangeUI()
        {
            if (Buttons != null)
            {
                this.Height = 20 * Buttons.Length;
                for (int i = 0; i < Buttons.Length; i++)
                {
                    LinkButton lb = new LinkButton();
                    lb.Name = "linkButton_" + i;
                    lb.Location = new Point(0, 20 * i);
                    lb.Size = new Size(this.Width, 20);
                    lb.Checked = false;
                    this.Controls.Add(lb);
                }
            }
        }
    }
}
