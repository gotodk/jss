using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class GetKey : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write(GetKeys());
    }
    private string GetKeys()
    {
        string KeyStr = "NULL|NULL|NULL";
        try
        {
            string serverPath = "C:\\PinKey.txt";
            using (StreamReader sr = new StreamReader(serverPath))
            {
                KeyStr = sr.ReadToEnd();
            }
        }
        catch
        {
            throw;
        }
        return KeyStr;
    }
}