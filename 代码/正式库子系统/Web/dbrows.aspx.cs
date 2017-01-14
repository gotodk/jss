using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using Galaxy.ClassLib.DataBaseFactory;
using Hesion.Brick.Core.WorkFlow;
using Infragistics.WebUI.UltraWebGrid;

public partial class Web_dbrows : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //判断是否有权限
            DefinedModule Dfmodule = new DefinedModule("SJLFXGJ");
            Authentication auth = Dfmodule.authentication;
            if (auth == null)
            {
                //MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
            }

            if (!auth.GetAuthByUserNumber(User.Identity.Name).CanView)
            {
                //MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
            }

            showall();
        }


    }

    public void showall()
    {
        DataSet dsall = new DataSet();
        DataTable dt_zi = new DataTable();
        dt_zi = DbHelperSQL.Query("SELECT '数据库名'='0', '表名'='0' , '数据量' = 0 FROM sysobjects WHERE 1=2 ").Tables[0].Clone();
        dt_zi.TableName = "zibiao";
        DataSet dsdb = DbHelperSQL.Query("Select '数据库名'=Name FROM Master..SysDatabases order by Name");
        if (dsdb != null && dsdb.Tables != null && dsdb.Tables.Count > 0)
        {
            DataTable dt = dsdb.Tables[0];
            dsall.Tables.Add(dt.Copy());
            foreach (DataRow dr in dt.Rows)
            {
                //Response.Write("数据库:[" + dr[0].ToString() + "]<hr />");
                DataSet dstable = DbHelperSQL.Query("SELECT '数据库名'='" + dr[0].ToString() + "', '表名'=Name , '数据量' = 0 FROM " + dr[0].ToString() + "..sysobjects WHERE type = 'U'");
                if (dstable != null && dstable.Tables != null && dstable.Tables.Count > 0)
                {
                    DataTable dt_tb = dstable.Tables[0];
                    
                    foreach (DataRow dr0 in dt_tb.Rows)
                    {
                        dr0["数据量"] = Convert.ToInt32(DbHelperSQL.Query("SELECT count(*) from  " + dr[0].ToString() + ".." + dr0["表名"]).Tables[0].Rows[0][0]);
                        dt_zi.Rows.Add(dr0.ItemArray);
                        
                    }  
                }
            }
        }
        DataView dv = new DataView(dt_zi);
        dv.Sort = "数据库名 desc , 数据量 desc";
        dt_zi = dv.ToTable();
        //for (int i = 0; i < dt_zi.Rows.Count; i++)
        //{
        //    Response.Write(dt_zi.Rows[i]["数据库名"].ToString() + "|" + dt_zi.Rows[i]["表名"].ToString() + "|" + dt_zi.Rows[i]["数据量"].ToString() + "<hr />");
        //}
            
        dsall.Tables.Add(dt_zi);
        dsall.Relations.Add("gx", dsall.Tables[0].Columns["数据库名"], dt_zi.Columns["数据库名"]);

        GV_show.DataSource = dsall;
        GV_show.DataBind();



    }
}