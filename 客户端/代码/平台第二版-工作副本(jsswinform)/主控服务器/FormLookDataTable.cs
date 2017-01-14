using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 主控服务器
{
    public partial class FormLookDataTable : Form
    {
        DataTable dt;
        string name;
        public FormLookDataTable(DataTable dt_temp,string name_temp)
        {
            InitializeComponent();

            dt = dt_temp;
            name = name_temp;
        }

        private void FormLookDataTable_Load(object sender, EventArgs e)
        {
            this.Text = name;
            dataGridView1.DataSource = dt;
        }
    }
}
