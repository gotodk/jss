using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Web.Script.Serialization;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = "运行测试： " + DateTime.Now.ToLongTimeString();
    }

   



}