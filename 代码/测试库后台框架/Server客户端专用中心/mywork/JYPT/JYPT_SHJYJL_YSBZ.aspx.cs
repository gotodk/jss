using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using FMipcClass;
using FMDBHelperClass;

public partial class mywork_JYPT_JYPT_SHJYJL_YSBZ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpager1.OnNeedLoadData += new pagerdemo_commonpager.OnNeedDataHandler(MyWebControl_OnNeedLoadData);//�����б�ķ�ҳ
        if (!IsPostBack)
        {
            if (Request["spbh"] != null && Request["spbh"].ToString() != "")
            {
                hidSPBH.Value = Request["spbh"].ToString();
            }
            if (Request["dlyx"] != null && Request["dlyx"].ToString() != "")
            {
                hidDLYX.Value = Request["dlyx"].ToString();
            }

            //��ȡ��½�ߵ��û���
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable input = new Hashtable();
            input["@B_DLYX"] = hidDLYX.Value;
            string sql_sel = "select B_YHM from AAA_DLZHXXB where B_DLYX=@B_DLYX";
            Hashtable return_YHM = I_DBL.RunParam_SQL(sql_sel, "�û���", input);
            if ((bool)return_YHM["return_float"])
            {
                DataSet dsyhm = (DataSet)return_YHM["return_ds"];
                hidYHM.Value = dsyhm.Tables[0].Rows[0]["B_YHM"].ToString();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('S1:ϵͳ�쳣��');</script>");
                return;
            }

            setURL();

            SetContent();
            RepeaterBind();
        }
    }
    private void setURL()
    {
        aSPSC.HRef = "JYPT_SHJYJL_SPSC.aspx?spbh=" + hidSPBH.Value + "&dlyx=" + hidDLYX.Value;
        aSPMS.HRef = "JYPT_SHJYJL_SPMS.aspx?spbh=" + hidSPBH.Value + "&dlyx=" + hidDLYX.Value;
        aJYBZ.HRef = "JYPT_SHJYJL_YSBZ.aspx?spbh=" + hidSPBH.Value + "&dlyx=" + hidDLYX.Value;
        aZLBZ.HRef = "JYPT_SHJYJL_ZLBZ.aspx?spbh=" + hidSPBH.Value + "&dlyx=" + hidDLYX.Value;
    }
    /// <summary>
    /// ������Ʒ��ź͵�½������ҵ�ǰ�Ľ����׳���Ϣ����ȷ��ҳ����Ӧ����ʾ�����ݡ�
    /// </summary>
    private void SetContent()
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable input = new Hashtable();
        input["@DLYX"] = hidDLYX.Value;
        input["@spbh"] = hidSPBH.Value.ToString();       

        string sql_jy = "select * from AAA_SPXXJYB where JYRDLYX=@DLYX and spbh=@spbh and jysx='���ձ�׼'";
        Hashtable result_jy = I_DBL.RunParam_SQL(sql_jy, "�ύ", input);
        DataSet ds_jy = new DataSet();
        if ((bool)result_jy["return_float"])
        {
            ds_jy = (DataSet)result_jy["return_ds"];
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('S2:ϵͳ�쳣��');</script>");
            return;
        }
        if (ds_jy != null && ds_jy.Tables[0].Rows.Count > 0)
        {
            trTJ.Visible = false;
            trCK.Visible = true;          
            divCK.InnerHtml = "�����ύ���µ����շ������飬�������£�<br/>" + ds_jy.Tables[0].Rows[0]["JYNR"].ToString().Replace("\r\n", "<br/>");
            hidCan.Value = "false";
        }
        else
        {
            string sql_zc = "select a.*,b.* from AAA_SPXXJYB_ZCXQ as a left join AAA_SPXXJYB as b on a.parentnumber=b.number where ZCRDLYX=@DLYX and spbh=@spbh and jysx='���ձ�׼'";
            DataSet ds_zc = new DataSet();
            Hashtable result_zc = I_DBL.RunParam_SQL(sql_zc, "֧��", input);
            if ((bool)result_zc["return_float"])
            {
                ds_zc = (DataSet)result_zc["return_ds"];
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('S3:ϵͳ�쳣��');</script>");
                return;
            }
          
            if (ds_zc != null && ds_zc.Tables[0].Rows.Count > 0)
            {
                trTJ.Visible = false;
                trCK.Visible = true;              
                divCK.InnerHtml = "���Ѿ����û� <span style=\"color :Red\">" + ds_zc.Tables[0].Rows[0]["JYRYHM"].ToString() + "</span> ��������շ�����ʾ��֧�֡�";
                hidCan.Value = "false";
            }
            else
            {
                trTJ.Visible = true;
                trCK.Visible = false;
                hidCan.Visible = true;
            }
        }
    }

    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        rpt.DataSource = null;

        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
            tempty.Visible = false;
            rpt.DataSource = NewDS;
        }
        //������û������ʱ��ʾ��ͷ�ͱ��
        else
        {
            tempty.Visible = true;
        }
        rpt.DataBind();
    }

    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 7 ";
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (select a.number,a.spbh,a.jynr as ��������,a.jyryhm as ������,a.JYRDLYX as ����������,a.zccs+1 as ֧�ִ���,��������,CONVERT(varchar(10), ROUND(CONVERT(float,(a.zccs+1))/CONVERT(float,��������)*100.00,2))+'%' as ֧����,ROUND(CONVERT(float,(a.zccs+1))/CONVERT(float,��������)*100.00,2) as zclsort from AAA_SPXXJYB as a left join (select spbh,jysx,SUM(ZCCS+1) as �������� from AAA_SPXXJYB where jysx='���ձ�׼' group by SPBH,jysx) as b on a.spbh=b.spbh and a.jysx=b.jysx where a.jysx='���ձ�׼') as tab ";
        ht_where["search_mainid"] = " number ";
        ht_where["search_str_where"] = " 1=1 ";
        ht_where["search_paixu"] = " DESC ";
        ht_where["search_paixuZD"] = " zclsort DESC, spbh";

        return ht_where;
    }

    protected void RepeaterBind()
    {
        Hashtable HTwhere = SetV();
        HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and spbh='" + hidSPBH.Value + "'";
        commonpager1.HTwhere = HTwhere;
        commonpager1.GetFYdataAndRaiseEvent();
    }

    protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblJYSC = (Label)e.Item.FindControl("lbljynr");
        if (lblJYSC.Text.Length > 26)
            lblJYSC.Text = lblJYSC.Text.Substring(0, 26) + "...";

        Label lblJYZ = (Label)e.Item.FindControl("lblJYZ");
        if (lblJYZ.Text.Length > 10)
        {
            lblJYZ.Text = lblJYZ.Text.Substring(0, 10) + "...";
        }

        LinkButton likb = (LinkButton)e.Item.FindControl("btnZhiChi");
        if (hidCan.Value == "false")
        {
            likb.Enabled = false;
            likb.ToolTip = divCK.InnerHtml;
        }

    }
    protected void btnTJ_Click(object sender, EventArgs e)
    { 
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable input = new Hashtable();
        input["@jynr"] = txtSPXMC.Text.Trim();
        input["@spbh"] = hidSPBH.Value.ToString();
        string sql_jy = "select * from AAA_SPXXJYB where jynr=@jynr and spbh=@spbh and jysx='���ձ�׼'";

        Hashtable result_jy = I_DBL.RunParam_SQL(sql_jy, "����", input);
        DataSet ds_jy = new DataSet();
        if ((bool)result_jy["return_float"])
        {
            ds_jy = (DataSet)result_jy["return_ds"];
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('S4:ϵͳ�쳣��');</script>");
            return;
        }
        if (ds_jy != null && ds_jy.Tables[0].Rows.Count > 0)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('�˽����Ѵ��ڣ�������֧�ִ˽�������ύ�µĽ��飡');</script>");
            return;
        }
        if (txtSPXMC.Text.Trim().Length > 3000)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('���Ľ����Ѿ�����3000���֣����޸ĺ������ύ��');</script>");
            return;
        }
        
        if (hidSPBH.Value != "" && hidDLYX.Value != "")
        {
            //��ȡ�²������ݵ�number
            object[] re = IPC.Call("��ȡ����", new object[] { "AAA_SPXXJYB", "" });
            string KeyNumber = "";
            if (re[0].ToString() == "ok")
            {
                //������ǵõ�Զ�̷��������ķ���ֵ����ͬ���͵ģ����н���ǿ��ת�����ɡ�
                KeyNumber = (string)(re[1]);
            }
            else
            {
                //Զ�̷���ִ�г�������ˡ�����õ����Ǿ���ĳ��������Ϣ��һ�㲻�÷������û�����ʱ�����Ϳ϶���string��
                string reFF = re[1].ToString();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('S5:ϵͳ�쳣��');</script>");
                return;
            }

            input["@KeyNumber"] = KeyNumber;
            input["@DLYX"] = hidDLYX.Value.ToString();
            input["@YHM"] = hidYHM.Value.ToString();
            input["@CreateTime"] = DateTime.Now.ToString();

            string sql_insert = "INSERT INTO [AAA_SPXXJYB]([Number],[JYRDLYX],[JYRYHM],[SPBH],[JYSX],[JYNR],[ZCCS],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES(@KeyNumber,@DLYX,@YHM,@spbh,'���ձ�׼', @jynr,0,1,'admin',@CreateTime,@CreateTime)";

            Hashtable result = I_DBL.RunParam_SQL(sql_insert, input);
            if (((bool)result["return_float"]) == true && result["return_other"].ToString() == "1")
            {
                string url = "JYPT_SHJYJL_YSBZ.aspx?spbh=" + hidSPBH.Value + "&dlyx=" + hidDLYX.Value;
                Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('���շ����ύ�ɹ���');window.location.href='" + url + "';</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('���շ����ύʧ�ܣ�');</script>");
                return;
            }  
        }
    }
    protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "ZhiChi")
        {
            string[] cdarg = e.CommandArgument.ToString().Split(',');
            string parentnumber = cdarg[0].ToString();
            string jyzdlyx = cdarg[1].ToString();

            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable input = new Hashtable();
            input["@parentNumber"] = parentnumber;
            input["@DLYX"] = hidDLYX.Value.ToString();
            input["@YHM"] = hidYHM.Value.ToString();
            input["@ZCSJ"] = DateTime.Now.ToString();
            input["@spbh"] = hidSPBH.Value.ToString();

            ArrayList al = new ArrayList();
            string sql_insert = "INSERT INTO [AAA_SPXXJYB_ZCXQ](parentNumber,ZCRDLYX,ZCRYHM,ZCSJ) VALUES(@parentnumber,@DLYX,@YHM,@ZCSJ)";
            al.Add(sql_insert);
            string sql_update = "UPDATE AAA_SPXXJYB set ZCCS=ZCCS+1 where number=@parentnumber and  SPBH=@spbh and jysx='���ձ�׼'";
            al.Add(sql_update);

            Hashtable result = I_DBL.RunParam_SQL(al, input);

            if (((bool)result["return_float"]) == true && result["return_other"].ToString() == al.Count.ToString())
            {
                //�����Ľ��鱻�������֧�֣��������˼�5��,�첽����
                AddJF(I_DBL, parentnumber, jyzdlyx);
                string url = "JYPT_SHJYJL_YSBZ.aspx?spbh=" + hidSPBH.Value + "&dlyx=" + hidDLYX.Value;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('֧�ֳɹ���');window.location.href='" + url + "';</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('֧��ʧ�ܣ�');</script>");
                return;
            }
        }
        if (e.CommandName == "ViewXQ")
        {
            string[] cmdstr = e.CommandArgument.ToString().Split('|');
            spanTGR.InnerText = cmdstr[0].ToString();
            divJYXQ.InnerHtml = cmdstr[1].ToString().Replace("\r\n", "<br/>");
            divCKXQ.Visible = true;
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        divCKXQ.Visible = false;
        spanTGR.InnerText = "";
        divJYXQ.InnerHtml = "";
    }

    //�����Ľ����5��֧�ֵģ��ͼ�5��
    private void AddJF(I_Dblink I_DBL, string parentnumber, string jyzdlyx)
    {
        //�ж��Ƿ���Ҫ���ӻ���2013-10-25
        Hashtable input = new Hashtable();
        input["@parentNumber"] = parentnumber;
        string sql_zcsl = "select * from AAA_SPXXJYB_ZCXQ where parentnumber=@parentNumber";
        DataSet ds_zcsl = new DataSet();
        Hashtable result_zcsl = I_DBL.RunParam_SQL(sql_zcsl, "֧������", input);
        if ((bool)result_zcsl["return_float"])
        {
            ds_zcsl = (DataSet)result_zcsl["return_ds"];
        }
        else
        {
            ds_zcsl = null;
        }
        if (ds_zcsl != null && ds_zcsl.Tables[0].Rows.Count == 5)
        {//���֧�������Ѿ���5���ˣ�������������û��ֵķ���
            DataTable dt = new DataTable();
            dt.TableName = "����";
            dt.Columns.Add("��¼����");
            dt.Columns.Add("��ע");
            dt.Rows.Add(new string[] { jyzdlyx, "���շ���" + parentnumber });
            object[] run_xyjf = IPC.Call("���õȼ����ִ���", new object[] { "���̾��齻��", dt });
        }
    }
}