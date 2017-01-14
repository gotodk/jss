using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;

namespace Com.Seezt.Skins
{
    [DefaultEvent("CheckedChanged")]
    public partial class BasicCheckBox : UserControl
    {
        private Graphics g = null;
        private bool m_Checked = false;
        private string m_buttonText = "CheckBox";
        public enum CheckStates { Unchecked, Checked, Indeterminate }
        private CheckStates m_CheckState = CheckStates.Unchecked;
        public delegate void CheckedChangedEventHandler(object sender, bool Checked);
        public event CheckedChangedEventHandler CheckedChanged;
        private Bitmap Bmp = null;
        public BasicCheckBox()
        {
            if (Checked)
            {
                switch (CheckState)
                {
                    case CheckStates.Checked:
                        Bmp = ResClass.GetResObj("cb_c_l");
                        break;
                    case CheckStates.Indeterminate:
                        Bmp = ResClass.GetResObj("cb_b_l");
                        break;
                    default:
                        Bmp = ResClass.GetResObj("cb_c_e");
                        break;
                }
            }
            else
            {
                Bmp = ResClass.GetResObj("cb_n_l");
            }
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
        }

        [Description("按钮上的文本"), Category("Appearance")]
        public string Texts
        {
            get
            {
                return m_buttonText;
            }
            set
            {
                m_buttonText = value;
                this.Invalidate();
            }
        }

        [Description("是否选中"), Category("Appearance")]
        public bool Checked
        {
            get
            {
                return m_Checked;
            }
            set
            {
                m_Checked = value;
                if (value)
                    CheckState = CheckStates.Checked;
                else
                    CheckState = CheckStates.Unchecked;
                if (CheckedChanged != null)
                    CheckedChanged(this, value);
            }
        }

        [Description("选中状态"), Category("Appearance")]
        public CheckStates CheckState
        {
            get
            {
                return m_CheckState;
            }
            set
            {
                m_CheckState = value;
                switch (value)
                {
                    case CheckStates.Unchecked:
                        m_Checked = false;
                        Bmp = ResClass.GetResObj("cb_n_e");
                        break;
                    case CheckStates.Checked:
                        m_Checked = true;
                        Bmp = ResClass.GetResObj("cb_c_e");
                        break;
                    case CheckStates.Indeterminate:
                        m_Checked = true;
                        Bmp = ResClass.GetResObj("cb_c_e");
                        break;
                }
                this.Invalidate();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;
            g.DrawImage(Bmp, new Rectangle(0, (this.Height - 15) / 2, 15, 15), 0, 0, Bmp.Width, Bmp.Height, GraphicsUnit.Pixel);
            g.DrawString(m_buttonText, this.Font, Brushes.Black, 16, (this.Height - 11) / 2);
        }

        protected override void OnResize(EventArgs e)
        {
            this.Height = 15;
        }

        protected override void OnClick(EventArgs e)
        {
            if (!Checked)
            {
                Checked = true;

            }
            else
            {
                Checked = false;
            }
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            if (!Checked)
            {
                Checked = true;

            }
            else
            {
                Checked = false;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (Checked)
            {
                switch (CheckState)
                {
                    case CheckStates.Checked:
                        Bmp = ResClass.GetResObj("cb_c_e");
                        break;
                    case CheckStates.Indeterminate:
                        Bmp = ResClass.GetResObj("cb_b_e");
                        break;
                    default:
                        Bmp = ResClass.GetResObj("cb_c_e");
                        break;
                }
            }
            else
            {
                Bmp = ResClass.GetResObj("cb_n_e");
            }
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (Checked)
            {
                switch (CheckState)
                {
                    case CheckStates.Checked:
                        Bmp = ResClass.GetResObj("cb_c_l");
                        break;
                    case CheckStates.Indeterminate:
                        Bmp = ResClass.GetResObj("cb_b_l");
                        break;
                    default:
                        Bmp = ResClass.GetResObj("cb_c_e");
                        break;
                }
            }
            else
            {
                Bmp = ResClass.GetResObj("cb_n_l");
            }
            this.Invalidate();
        }
    }
}
