using System;
using System.ComponentModel;
using System.Drawing;


namespace 客户端主程序.SubForm
{
    public partial class LabelTx : System.Windows.Forms.Label
    {
        int lineDistance = 5;//行间距    
        Graphics gcs;
        int iHeight = 0, height = 200;
        string[] nrLine;
        string[] nrLinePos;   
        int searchPos = 0;
        int section = 1;
        public int LineDistance
        {
            get { return lineDistance; }
            set
            {
                lineDistance = value;
                Changed(this.Font, this.Width, this.Text);
            }
        }

        bool DXQ = false;
        /// <summary>
        /// 带不带详情
        /// </summary>
        public bool daixiangqing
        {
            get { return DXQ; }
            set
            {
                DXQ = value;
            }
        }

        public LabelTx()
            : base()
        {
            //this.TextChanged += new EventHandler(LabelTx_TextChanged);   
            this.SizeChanged += new EventHandler(LabelTx_SizeChanged);
            this.FontChanged += new EventHandler(LabelTx_FontChanged);
            //this.Font = new Font(this.Font.FontFamily, this.Font.Size, GraphicsUnit.Pixel);  
        }
        void LabelTx_FontChanged(object sender, EventArgs e)
        {
            Changed(this.Font, this.Width, this.Text);
        }
        void LabelTx_SizeChanged(object sender, EventArgs e)
        {
            Changed(this.Font, this.Width, this.Text);
        }
        public LabelTx(IContainer container)
        {
            container.Add(this);
            //base.Height   
            //InitializeComponent();   
        }
        public int FHeight
        {
            get { return this.Font.Height; }
        }
        protected int Height
        {
            get { return height; }
            set
            {
                height = value;
                base.Height = value;
            }
        }
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                //is.Font.Size.                       
                base.Text = value;
                Changed(this.Font, this.Width, value);
            }
        }
        protected void Changed(Font ft, int iWidth, string value)
        {
            iHeight = 0;
            if (value != "")
            {
                if (gcs == null)
                {
                    gcs = this.CreateGraphics();
                    SizeF sf0 = gcs.MeasureString(new string('测', 20), ft);
                    searchPos = (int)(iWidth * 20 / sf0.Width);
                }
                nrLine = value.Split(new string[1] { "\r\n" }, StringSplitOptions.None);
                section = nrLine.Length;
                nrLinePos = new string[section];
                SizeF sf1, sf2;
                string temps, tempt;
                string drawstring;
                int temPos, ipos;
                for (int i = 0; i < section; i++)
                {
                    ipos = 0;
                    temPos = searchPos;
                    if (searchPos >= nrLine[i].Length)
                    {
                        ipos += nrLine[i].Length;
                        nrLinePos[i] += "," + ipos.ToString();
                        iHeight++;
                        continue;
                    }
                    drawstring = nrLine[i];
                    nrLinePos[i] = "";
                    while (drawstring.Length > searchPos)
                    {
                        bool isfind = false;
                        for (int j = searchPos; j < drawstring.Length; j++)
                        {
                            temps = drawstring.Substring(0, j);
                            tempt = drawstring.Substring(0, j + 1);
                            sf1 = gcs.MeasureString(temps, ft);
                            sf2 = gcs.MeasureString(tempt, ft);
                            if (sf1.Width < iWidth && sf2.Width > iWidth)
                            {
                                iHeight++;
                                ipos += j;
                                nrLinePos[i] += "," + ipos.ToString();
                                isfind = true;
                                drawstring = drawstring.Substring(j);
                                break;
                            }
                        }
                        if (!isfind)
                        {
                            //drawstring = drawstring.Substring(searchPos);   
                            //iHeight++;   
                            break;
                        }
                    }
                    ipos += drawstring.Length;
                    nrLinePos[i] += "," + ipos.ToString();
                    iHeight++;
                    //tempLine = (int)(sf1.Width - 1) / this.Width + 1;                          
                    //iHeight += tempLine;   
                }
            }
            else
            {
                section = 0;
            }
            if (!DXQ)
            {
                this.Height = iHeight * (ft.Height + lineDistance) + this.Padding.Top * 2;
            }
            else
            {
                this.Height = base.Height;
            }
           

        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            //base.OnPaint(e);              
            //if (isPaint) return;   
            //isPaint = true;   
            Graphics g = e.Graphics;
            String drawString = this.Text;
            Font drawFont = this.Font;
            SolidBrush drawBrush = new SolidBrush(this.ForeColor);
            SizeF textSize = g.MeasureString(this.Text, this.Font);//文本的矩形区域大小     
            int lineCount = Convert.ToInt16(textSize.Width / this.Width) + 1;//计算行数     
            int fHeight = this.Font.Height;
            int htHeight = this.Padding.Top;

            this.AutoSize = false;
            float x = this.Padding.Left;
            float y = 0.0F;
            StringFormat drawFormat = new StringFormat();
            int step = 1;
            bool isFirst = true;
            SizeF sf1, sf2;
            string subN, subN1;
            lineCount = drawString.Length;//行数不超过总字符数目      
            int i, idx, first;
            string subStr, tmpStr = "", midStr = "";
            string[] idxs;
            for (i = 0; i < section; i++)
            {
                if (htHeight >= base.Height && DXQ)
                {
                    break;
                }
                first = 0;
                subStr = nrLine[i];
                if (nrLinePos[i] != null) tmpStr = nrLinePos[i].TrimStart(',');
                midStr = subStr.Substring(first);
                if (tmpStr != "")
                {
                    idxs = tmpStr.Split(',');
                    for (int j = 0; j < idxs.Length; j++)
                    {
                        if (htHeight >= base.Height && DXQ)
                        {
                            break;
                        }

                        idx = int.Parse(idxs[j]);

                        //到了最后一行，或超出边界，并且带详情
                        if ((htHeight + (fHeight + lineDistance) >= base.Height || j == idxs.Length - 1) && DXQ)
                        {
                            //附加文字的文字数量
                            string xq_str = "详情》";
                            int changdu = xq_str.Length;
                            SizeF sf_kongge = gcs.MeasureString("空格", drawFont);

                            int sl = (idx - first) - changdu - 8;
                            if (sl <= 0)
                            {
                                sl = idx - first;
                            }
                            midStr = subStr.Substring(first, sl);
                            e.Graphics.DrawString(midStr, drawFont, drawBrush, x, Convert.ToInt16(htHeight), drawFormat);
 
                            Brush Bushstr = new SolidBrush(Color.FromArgb(70, 238, 19));
                            SizeF sf0 = gcs.MeasureString(midStr, drawFont);

                            g.DrawString("详情》", drawFont, Bushstr, sf0.Width + sf_kongge.Width, Convert.ToInt16(htHeight), drawFormat);
                            
                        }
                        else
                        {
                            midStr = subStr.Substring(first, idx - first);
                            e.Graphics.DrawString(midStr, drawFont, drawBrush, x, Convert.ToInt16(htHeight), drawFormat);
                        }
                        
                        htHeight += (fHeight + lineDistance);
                        first = idx;

                        
                    }
                    //midStr = subStr.Substring(first);   
                }
                //e.Graphics.DrawString(midStr, drawFont, drawBrush, x, Convert.ToInt16(htHeight), drawFormat);  
                //htHeight += ( lineDistance);//fHeight +   
            }






        }
    }
}
