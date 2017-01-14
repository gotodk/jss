using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Hesion.Brick.Core;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;
using System.Threading;

public partial class Web_XXHZX_XXHZX_CXSCMKLB : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Application["uuu"] != null && Application["uuu"] == "开始")
        //{
        //   // this.Button1.Enabled = false;
        //}


        User us = Users.GetUserByNumber(User.Identity.Name);
        if (us.JobName != "系统管理员")
        {
            Response.Write("<script language=javascript>window.alert('您没有此模块的权限权限！');history.back();</script>");
            Response.End();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
         
            // Application["uuu"] = "开始";
              Copy_System_Modules cp = new Copy_System_Modules();
              GenerateProgramaAuthrity();
             SaveGuodu();
            // Application["uuu"] = "结束";
            Response.Write("<script language=javascript>window.parent.frames('leftFrame').location.reload();</script>");
            //Response.Write("<Script language ='javaScript'>alert('生成成功!');history.back();</Script>");
            //Response.End();
        }
        catch(Exception ex)
        {
            this.divError.Visible = true;
            this.divError.InnerText = ex.ToString();
         
        }
        
    }

    protected void SaveGuodu()
    {
        DbHelperSQL.ExecuteSql("TRUNCATE TABLE LMQXB    TRUNCATE TABLE system_auth   insert into LMQXB select * from LMQXB_guodu   insert into system_auth(ModuleName,ModuleType,Role,Type,CanAdd,CanEdit,CanView,NeedAudit) select ModuleName,ModuleType,Role,Type,CanAdd,CanEdit,CanView,NeedAudit from system_auth_guodu");
    }
    protected void GenerateProgramaAuthrity()
    {
        DataTable dataTable = DbHelperSQL.Query("update system_Modules set Sequence = 1 where  name='LMQXB_guodu'  TRUNCATE TABLE LMQXB_guodu  select ModuleName,[Role],[Type],SmallClassId as 'id' from system_auth_guodu inner join system_modules on ModuleName=name  where CanAdd='1' or CanEdit='1' or CanView='1' or needaudit = '0' " +
" order by id asc,[Role] asc,[Type] asc ").Tables[0];
        string strSql = "";
        string strInsertSql = "";
        string strId = "";
        string strType = "";
    
        string strRole="";
        foreach(DataRow dataRow in dataTable.Rows)
        {

            if (dataRow["id"].ToString() == "198")
            {
                
            }
        

            strSql = " SELECT id,parentid,title,px,show FROM system_ModuleGroup where id='" + dataRow["id"].ToString() + "'";
            DataTable dataTableCopy;


               if (strRole != dataRow["Role"].ToString()||strType != dataRow["Type"].ToString())
               {
                   do
                   {
                   dataTableCopy = DbHelperSQL.Query(strSql).Tables[0];
                   WorkFlowModule lmQXB = new WorkFlowModule("LMQXB_guodu");
                   string KeyNumber = lmQXB.numberFormat.GetNextNumber(); //主表的Number
                   strInsertSql = "insert LMQXB_guodu(Number,LMID,LMMC,YH,[TYPE],[Role],CheckState,CreateUser,CreateTime,CheckLimitTime) " +
" values('" + KeyNumber + "','" + dataTableCopy.Rows[0]["id"].ToString() + "','" + dataTableCopy.Rows[0]["title"].ToString() + "','" + dataRow["Role"].ToString()
+ "','" + dataRow["Type"].ToString() + "','" + dataRow["Role"].ToString() + "','1','admin',GETDATE(),GETDATE())";
                   DbHelperSQL.ExecuteSql(strInsertSql);
                   strSql = " SELECT id,parentid,title,px,show FROM system_ModuleGroup where id='" + dataTableCopy.Rows[0]["parentid"].ToString() + "'";
                   } while (Convert.ToInt32(dataTableCopy.Rows[0]["parentid"].ToString())!=0);
               }
               //strId = dataRow["id"].ToString();
              // strType = dataRow["Type"].ToString();
              // strRole = dataRow["Role"].ToString();
        }
    
    
    }



















}
