using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class mywork_GXTW_YXWH : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpager1.OnNeedLoadData += new pagerdemo_commonpager.OnNeedDataHandler(MyWebControl_OnNeedLoadData);//分类列表的分页
        if(!IsPostBack)
        {
            Bind();
        }
    }
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        rpt.DataSource = null;

        if (NewDS != null && NewDS.Tables.Count > 0)
        {
            tempty.Visible = false;
            rpt.DataSource = NewDS;
        }
        //控制在没有数据时显示表头和表脚
        else
        {
            tempty.Visible = true;
        }
        rpt.DataBind();
    }

    private Hashtable SetV()
    {
        string DLZH = User.Identity.Name;

        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 10";
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = "AAA_PTYXB ";
        ht_where["search_mainid"] = " number ";
        ht_where["search_str_where"] = " 1=1 and GXZH='" + DLZH + "' ";
        ht_where["search_paixu"] = " DESC ";
        ht_where["search_paixuZD"] = " CreateTime";

        return ht_where;
    }

    protected void Bind()
    {
        Hashtable HTwhere = SetV(); 
        commonpager1.HTwhere = HTwhere;
        commonpager1.GetFYdataAndRaiseEvent();
    }
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        Bind();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_PTYXB", ""); //生成主键
        string DLZH=User.Identity.Name.Trim();
        string YXMC=txtYXMCAdd.Text.Trim();
        string strInsert = "insert AAA_PTYXB ([Number],GXZH,YXMC,[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) values ('" + Number.Trim() + "','" + DLZH + "','" + YXMC + "',1,'" + DLZH + "',getDate(),getDate())";
        int i = DbHelperSQL.ExecuteSql(strInsert);
        if (i > 0)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('院系新添成功！');</script>");
            Bind();
            txtYXMCAdd.Text = "";
            btnAdd.Visible = true;
            btnSeave.Visible = false;
        }
        else
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('院系新添失败！');</script>");
        }
    }
    protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Up")
        {
            string[] str= e.CommandArgument.ToString().Split('&');
            string number = str[0].ToString();
            string YXMC = str[1].ToString();
            txtYXMCAdd.Text = YXMC;
            lblupdate.Text = "";
            lblupdate.Text = number;
            btnAdd.Visible = false;
            btnSeave.Visible = true;
        }
        else if (e.CommandName == "Del")
        {
            lblupdate.Text = "";
            lblupdate.Text = e.CommandArgument.ToString();
            string str = "select * from AAA_DLZHXXB where I_YXBH='" + lblupdate.Text.Trim() + "'";
            DataSet ds = DbHelperSQL.Query(str);
            if (ds!=null&&ds.Tables[0].Rows.Count>0)
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('该院系下已存在用户的，不能进行删除！');</script>");
                return;
            }

            string strDel = "delete AAA_PTYXB  where Number='" + lblupdate.Text.Trim() + "'";
            int i = DbHelperSQL.ExecuteSql(strDel);
            if (i > 0)
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('删除成功！');</script>");
                Bind();
                lblupdate.Text = "";
                txtYXMCAdd.Text = "";
                btnAdd.Visible =true ;
                btnSeave.Visible = false;
            }
            else
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('删除失败！');</script>");
            }
        }
    }
    protected void btnSeave_Click(object sender, EventArgs e)
    {
        string str = "update AAA_PTYXB  set YXMC='" + txtYXMCAdd.Text.Trim() + "' where Number='" + lblupdate.Text.Trim() + "'";
        int i = DbHelperSQL.ExecuteSql(str);
        if (i > 0)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('院系修改成功！');</script>");
            Bind();
            txtYXMCAdd.Text = "";
            lblupdate.Text = "";
            btnAdd.Visible = true;
            btnSeave.Visible = false;
        }
        else
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('院系修改失败！');</script>");
        }
    }
}