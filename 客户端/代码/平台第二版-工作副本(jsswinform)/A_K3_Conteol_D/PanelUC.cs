using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace 客户端主程序.SubForm.CenterForm.publicUC
{
    public partial class PanelUC : System.Windows.Forms.Panel
    {
        private Color bordercolor = Color.Gray;
        [Description("设置边框颜色，默认灰色"), Category("Appearance")]
        public Color BorderColor
        {
            get
            {
                return bordercolor;
            }
            set
            {
                bordercolor = value;
            }
        }



        private int setborder = 1;
        [Description("设置边框宽度，默认1"), Category("Appearance")]
        public int SetShowBorder
        {
            get
            {
                return setborder;
            }
            set
            {
                setborder = value;
            }
        }


        public PanelUC()
        {
            InitializeComponent();
        }


        private void PanelUC_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                //勾画边框
                ControlPaint.DrawBorder(e.Graphics,
                                    this.ClientRectangle,
                                    bordercolor,
                                    setborder,
                                    ButtonBorderStyle.Solid,
                                    bordercolor,
                                    setborder,
                                    ButtonBorderStyle.Solid,
                                    bordercolor,
                                    setborder,
                                    ButtonBorderStyle.Solid,
                                    bordercolor,
                                    setborder,
                                    ButtonBorderStyle.Solid);
            }
            catch { }


        }


    }
}
