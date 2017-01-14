using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using System.Data;
using FMOP.DB;
using System.Collections;
public partial class Web_JHJX_HTJK_YJTJ_Set : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
   
            Set();
        }
    }
    private void Set()
    {
        string sql = "select * from AAA_YJTJWHB";
        DataSet ds = DbHelperSQL.Query(sql);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            spanNum.InnerText = ds.Tables[0].Rows[0]["Number"].ToString();
            if (ds.Tables[0].Rows[0]["LYJSBJTXT"].ToString() != "" && ds.Tables[0].Rows[0]["LYJSBJTXT"].ToString() != "0")
            {
                txtLVJSTX_Buyer.Text = ds.Tables[0].Rows[0]["LYJSBJTXT"].ToString();
            }
            if (ds.Tables[0].Rows[0]["LYJSSJTXT"].ToString() != "" && ds.Tables[0].Rows[0]["LYJSSJTXT"].ToString() != "0")
            {
                txtLVJSTX_Saler.Text = ds.Tables[0].Rows[0]["LYJSSJTXT"].ToString();
            }
            if (ds.Tables[0].Rows[0]["LYBZJXYDYJEBFB"].ToString() != "" && ds.Tables[0].Rows[0]["LYBZJXYDYJEBFB"].ToString() != "0")
            {
                txtLYBZJ.Text = ds.Tables[0].Rows[0]["LYBZJXYDYJEBFB"].ToString();
            }
            if (ds.Tables[0].Rows[0]["YCFHYJTS"].ToString() != "" && ds.Tables[0].Rows[0]["YCFHYJTS"].ToString() != "0")
            {
                txtZCFHR.Text = ds.Tables[0].Rows[0]["YCFHYJTS"].ToString();
            }
        }
    }


    protected void btnCommit_Click(object sender, EventArgs e)
    {
        if (txtLVJSTX_Buyer.Text.Trim() == "" && txtLVJSTX_Saler.Text.Trim() == "" && txtLYBZJ.Text.Trim() == "" || txtZCFHR.Text.Trim() == "")
        {
            MessageBox.ShowAlertAndBack(this, "未设置任何条件，不需要提交！");
        }
        if (txtLVJSTX_Buyer.Text.Trim() == "0" || txtLVJSTX_Saler.Text.Trim() == "0" || txtLYBZJ.Text.Trim() == "0" || txtZCFHR.Text.Trim() == "0")
        {
            MessageBox.ShowAlertAndBack(this, "条件中只能录入大于0的整数，请修改！");
        }

        if (spanNum.InnerText.Trim() == "")
        {           
            WorkFlowModule WFM = new WorkFlowModule("AAA_YJTJWHB");
            string KeyNumber = WFM.numberFormat.GetNextNumber();
            string sql_insert = "INSERT INTO [AAA_YJTJWHB]([Number],[LYJSBJTXT],[LYJSSJTXT],[YCFHYJTS],[LYBZJXYDYJEBFB],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES('" + KeyNumber + "' ,'" + txtLVJSTX_Buyer.Text.Trim() + "' ,'" + txtLVJSTX_Saler.Text.Trim() + "','" + txtZCFHR.Text.Trim() + "' ,'" + txtLYBZJ.Text.Trim() + "' ,1 ,'" + User.Identity.Name.ToString() + "' ,'" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
            try
            {
                DbHelperSQL.ExecuteSql(sql_insert);
                MessageBox.ShowAndRedirect(this, "提交成功！", "YJTJ_Set.aspx");
            }
            catch (Exception ex)
            {
                MessageBox.ShowAlertAndBack(this, "提交失败！");
            }
        }

        else if (spanNum.InnerText.Trim() != "")
        {
            string sql_update = "UPDATE [AAA_YJTJWHB] SET [LYJSBJTXT] = '"+txtLVJSTX_Buyer.Text .Trim ()+"',[LYJSSJTXT] = '"+txtLVJSTX_Saler.Text .Trim ()+"',[YCFHYJTS] = '"+txtZCFHR.Text .Trim ()+"' ,[LYBZJXYDYJEBFB] = '"+txtLYBZJ.Text .Trim ()+"',[CreateUser] ='"+User.Identity.Name .ToString ()+"',[CreateTime] ='"+DateTime .Now .ToString()+"' WHERE number='" + spanNum.InnerText.Trim() + "'";

            try
            {
                DbHelperSQL.ExecuteSql(sql_update);
                MessageBox.ShowAndRedirect(this, "更新成功！", "YJTJ_Set.aspx");
            }
            catch (Exception ex)
            {
                MessageBox.ShowAlertAndBack(this, "更新失败！");
            }
        }

       
    }
    protected void btnReSet_Click(object sender, EventArgs e)
    {
        Set();
    }
}