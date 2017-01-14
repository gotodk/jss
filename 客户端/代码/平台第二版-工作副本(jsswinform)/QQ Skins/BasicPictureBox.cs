using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Com.Seezt.Skins
{
    public class BasicPictureBox : PictureBox
    {
        private string text;
        [Description("文本"), Category("Appearance")]
        public string Texts
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
    }
}
