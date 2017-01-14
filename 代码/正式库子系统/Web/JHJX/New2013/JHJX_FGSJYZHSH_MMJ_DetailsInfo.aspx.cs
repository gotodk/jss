using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using System.Data;
using System.Configuration;

public partial class Web_JHJX_New2013_JHJX_FGSJYZHSH_MMJ_DetailsInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.UCCityList1.initdefault();
            ViewState["JSBH"] = "";
            ViewState["DLYX"] = "";
            ViewState["YHM"] = "";
            ViewState["JYFMC"] = "";
            ViewState["GLJJRBH"] = "";
            if (Request["JSBH"] != null)
            {
                ViewState["JSBH"] = Request["JSBH"].ToString();
                BindData();
            }

        }
    }



    /// <summary>
    /// 绑定界面数据信息
    /// </summary>
    /// <param name="strJSBH"></param>
    /// <param name="strJSLX"></param>
    /// <param name="jjrbh"></param>
    protected void BindData()
    {
        string strSql = "select B_DLYX,B_YHM,B_JSZHLX,J_SELJSBH,J_BUYJSBH,J_JJRJSBH,J_JJRSFZTXYHSH,J_JJRZGZSBH,JJRZGZS,J_JRZTXYHBZ,S_SFYBJJRSHTG,S_SFYBFGSSHTG,I_ZCLB,I_JYFMC,I_YYZZZCH,I_YYZZSMJ,I_SFZH,I_SFZSMJ,I_SFZFMSMJ,I_ZZJGDMZDM,I_ZZJGDMZSMJ,I_SWDJZSH,I_SWDJZSMJ,I_YBNSRZGZSMJ,I_KHXKZH,I_KHXKZSMJ,I_YLYJK,I_FDDBRXM,I_FDDBRSFZH,I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ,I_FDDBRSQS,I_JYFLXDH,I_SSQYS,I_SSQYSHI,I_SSQYQ,I_XXDZ,I_LXRXM,I_LXRSJH,I_KHYH,I_YHZH,I_PTGLJG,GLJJRBH,JJRSHZT,JJRSHSJ,JJRSHYJ,FGSSHZT,FGSSHR,FGSSHSJ,FGSSHYJ,convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间'  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX   where b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + ViewState["JSBH"].ToString() + "' and a.SFYX='是' and a.SFDQMRJJR='是'";

        DataTable dataTable = DbHelperSQL.Query(strSql).Tables[0];
        if (dataTable.Rows.Count > 0)
        {

            ViewState["DLYX"] = dataTable.Rows[0]["B_DLYX"].ToString();
            ViewState["YHM"] = dataTable.Rows[0]["B_YHM"].ToString();

            if (dataTable.Rows[0]["B_JSZHLX"].ToString() == "经纪人交易账户")
            {
                this.radJJR.Checked = true;
                this.radMMJ.Checked = false;
                this.trPTGLJG.Visible = true;
            }
            else
            {
                this.trPTGLJG.Visible = false;
                this.radJJR.Checked = false;
                this.radMMJ.Checked = true;
            }

            if (dataTable.Rows[0]["I_ZCLB"].ToString() == "单位")
            {
                this.radDW.Checked = true;
                this.radZRR.Checked = false;
            }
            else
            {
                this.radDW.Checked = false;
                this.radZRR.Checked = true;
            }

            this.txtJYFMC.Text = dataTable.Rows[0]["I_JYFMC"].ToString();

            ViewState["JYFMC"] = dataTable.Rows[0]["I_JYFMC"].ToString();

            this.txtYYZZZCH.Text = dataTable.Rows[0]["I_YYZZZCH"].ToString();
            this.linkYYZZ.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_YYZZSMJ"].ToString().Replace(@"\", "/");

            this.txtSFZH.Text = dataTable.Rows[0]["I_SFZH"].ToString();
            this.linkSFZH.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_SFZSMJ"].ToString().Replace(@"\", "/");
            this.linkSFZH_FM.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_SFZFMSMJ"].ToString().Replace(@"\", "/");

            this.txtZZJGDMZ.Text = dataTable.Rows[0]["I_ZZJGDMZDM"].ToString();
            this.linkZZJGDMZ.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_ZZJGDMZSMJ"].ToString().Replace(@"\", "/");


            this.txtSWDJZ.Text = dataTable.Rows[0]["I_SWDJZSH"].ToString();
            this.linkSWDJZ.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_SWDJZSMJ"].ToString().Replace(@"\", "/");


            this.linkYBNSRZGZMSMJ.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_YBNSRZGZSMJ"].ToString().Replace(@"\", "/");

            this.txtKHXKZH.Text = dataTable.Rows[0]["I_KHXKZH"].ToString();
            this.linkKHXKZH.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_KHXKZSMJ"].ToString().Replace(@"\", "/");

            this.linkYLYJK.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_YLYJK"].ToString().Replace(@"\", "/");

            this.txtFDDBRXM.Text = dataTable.Rows[0]["I_FDDBRXM"].ToString();

            this.txtFDDBRSHZHJSMJ.Text = dataTable.Rows[0]["I_FDDBRSFZH"].ToString();
            this.linkFDDBRSHZHJSM.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_FDDBRSFZSMJ"].ToString().Replace(@"\", "/");
            this.linkFDDBRSHZHJSM_FM.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_FDDBRSFZFMSMJ"].ToString().Replace(@"\", "/");

            this.linkFDDBRSQS.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_FDDBRSQS"].ToString().Replace(@"\", "/");

            this.txtJYFLXDH.Text = dataTable.Rows[0]["I_JYFLXDH"].ToString();
            //所属区域
            //所属区域
            this.UCCityList1.SelectedItem = new string[] { dataTable.Rows[0]["I_SSQYS"].ToString(), dataTable.Rows[0]["I_SSQYSHI"].ToString(), dataTable.Rows[0]["I_SSQYQ"].ToString() };
            this.UCCityList1.EnabledItem = new bool[] { false, false, false };

            this.txtXXDZ.Text = dataTable.Rows[0]["I_XXDZ"].ToString();
            this.txtLXRXM.Text = dataTable.Rows[0]["I_LXRXM"].ToString();

            this.txtLXRSJH.Text = dataTable.Rows[0]["I_LXRSJH"].ToString();
            this.txtKHYH.Text = dataTable.Rows[0]["I_KHYH"].ToString();
            this.txtYHZH.Text = dataTable.Rows[0]["I_YHZH"].ToString();


            this.ddlPTGLJG.Items.Add(dataTable.Rows[0]["I_PTGLJG"].ToString());
            this.ddlPTGLJG.Enabled = false;

            ViewState["GLJJRBH"] = dataTable.Rows[0]["GLJJRBH"].ToString();


            DataTable dataTableTwo = DbHelperSQL.Query("select J_JJRZGZSBH,JJRZGZS,I_JYFMC,I_JYFLXDH from AAA_DLZHXXB  where J_JJRJSBH='" + ViewState["GLJJRBH"].ToString().Trim() + "'").Tables[0];
            if (dataTableTwo != null && dataTableTwo.Rows.Count > 0)
            {
                this.txtGLJJRZGZSBH.Text = dataTableTwo.Rows[0]["J_JJRZGZSBH"].ToString();
                this.txtGLJJRMC.Text = dataTableTwo.Rows[0]["I_JYFMC"].ToString();
                this.txtGLJJRLXDH.Text = dataTableTwo.Rows[0]["I_JYFLXDH"].ToString();
            }



            this.txtZLTJSJ.Text = dataTable.Rows[0]["资料提交时间"].ToString();

            this.txtCS_JL.Text = dataTable.Rows[0]["JJRSHZT"].ToString();
            this.txtCS_SJ.Text = dataTable.Rows[0]["JJRSHSJ"].ToString();
            this.txtCS_SHYJ.Text = dataTable.Rows[0]["JJRSHYJ"].ToString();

            this.txtFS_JL.Text = dataTable.Rows[0]["FGSSHZT"].ToString();
            this.txtFS_SJ.Text = dataTable.Rows[0]["FGSSHSJ"].ToString();


            this.txtFS_YJ.Text = dataTable.Rows[0]["FGSSHYJ"].ToString();


            //决定要显示的页面
            if (dataTable.Rows[0]["I_ZCLB"].ToString() == "单位")
            {
                this.trYYZZZCH.Visible = true;
                this.trSFZH.Visible = false;
                this.trZZJGDMZ.Visible = true;

                this.trSWDJZSH.Visible = true;
                //this.trYBNSRZGZMSMJ.Visible = true;
                this.trKHXKZH.Visible = true;

                this.trYLYJK.Visible = true;

                this.trFDDBRXM.Visible = true;
                this.trFDDBRSHZHJSMJ.Visible = true;
                this.trFDDBRSQS.Visible = true;

            }
            else
            {
                this.trYYZZZCH.Visible = false;
                this.trSFZH.Visible = true;
                this.trZZJGDMZ.Visible = false;

                this.trSWDJZSH.Visible = false;
                this.trYBNSRZGZMSMJ.Visible = false;
                this.trKHXKZH.Visible = false;

                this.trYLYJK.Visible = false;

                this.trFDDBRXM.Visible = false;
                this.trFDDBRSHZHJSMJ.Visible = false;
                this.trFDDBRSQS.Visible = false;

            }




        }
    }

    /// <summary>
    /// 返回列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("JHJX_FGSJYYHCK_MMJ.aspx");
    }
}