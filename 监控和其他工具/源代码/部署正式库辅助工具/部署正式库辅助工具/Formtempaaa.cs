using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 部署正式库辅助工具
{
    public partial class Formtempaaa : Form
    {
        public Formtempaaa()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = richTextBox1.Text;
            //update   表A  set 字段1    =@zd  ,  字段2 =  @my,字段3=case(.....),字段4=字段4+@num, 字段5=(select ....)   
            //where   JSBH=@n   "
            //表A有修改，修改了字段1和2和3，这些很容易分析出来。
            //被修改字段的修改值为：？？？？      针对哪个人或单子或项目修改的：？？？？。   被修改的目标对应Redis如何更新？？


            //根据表名，调用对应一样名称的插件。
           
        }
    }
}
