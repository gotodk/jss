using FMOP.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_JHJX_New2013_JHJX_ZHPLZC : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (this.txtNum.Text != "" && Convert.ToInt32(this.txtNum.Text) > 0)
        {
            ArrayList sqllist = new ArrayList();

            string SQlz = "";
            string SQLjs = "";
            string Number = "";
            string stroder = "";
            string yhm = "";
            string seljsbh = "";
            string butjsbh = "";
            string jjrjsbh = "";
            string JJRZGZSBH = "";
            DateTime dt = DateTime.Now;

            string sfzh = "";
            string sfzfj = "";
            string sfzfmfj = "";

            string yyzz="";
            string yyzzfj="";


            string zzjgdmz = "";
            string zzjgdmafj = "";

            string swdjz = "";
            string swdjzfj = "";


            string ybnsrzgzsfj = "";

            string frdbxm = "";
            string frdbsfzh = "";
            string frdbsfzsmj = "";
            string srdbsfzfmmj = "";
            string frdbsqs = "";


            string jszhlx = "";

            string JJRFL = "";

            string gljjrdlyx = "";
            string gljjrbh = "";
            string gljjryhm = "";

            if(radJJR.Checked)
            {
                jszhlx = "经纪人交易账户";
                JJRFL = "一般经纪人";
            }
            if(radMMJ.Checked)
            {
                jszhlx = "买家卖家交易账户";
                JJRFL = "";
                gljjrdlyx = this.txtGLJJRDLYX.Text;//"yali@qq.com";
                gljjrbh = this.txtGLJJRJSBH.Text;//"J140828000008";
                gljjryhm = this.txtGLJJRYHM.Text;//"yaliyhm";
            }

            if(jszhlx.Equals(""))
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert(请选择结算账户类型！');</script>");
            }

            string zqzkzh = "";

            string strdw = "";

            ErpNumber erpNumber = new ErpNumber();
            if(radDW.Checked)
            {
                strdw="单位";
                yyzz="123456";
                yyzzfj = "201309\\3e8d31d1-8f4f-464e-bed8-8c267104b626.jpg" ;
                 zzjgdmz = "123456";
                 zzjgdmafj = "201309\\3e8d31d1-8f4f-464e-bed8-8c267104b626.jpg";

                 swdjz = "123456";
                 swdjzfj = "201309\\3e8d31d1-8f4f-464e-bed8-8c267104b626.jpg";


                 ybnsrzgzsfj = "201309\\3e8d31d1-8f4f-464e-bed8-8c267104b626.jpg";
                  frdbxm = "zm";
                  frdbsfzh = "123456";
                  frdbsfzsmj = "201309\\3e8d31d1-8f4f-464e-bed8-8c267104b626.jpg";
                  srdbsfzfmmj = "201309\\3e8d31d1-8f4f-464e-bed8-8c267104b626.jpg";
                  frdbsqs = "201309\\3e8d31d1-8f4f-464e-bed8-8c267104b626.jpg";
                sfzh="";
                sfzfj="";
                sfzfmfj="";
                zqzkzh = erpNumber.GetZQZJNumber("单位");
            }
            if (radZRR.Checked)
            {
                strdw="自然人";
                sfzh="37120219865213";
                sfzfj="201310\\eb99401d-139b-4b0b-82b6-3330293a9bff.jpg";
                sfzfmfj = "201310\\eb99401d-139b-4b0b-82b6-3330293a9bff.jpg";
                yyzz="";
                yyzzfj = "" ;
                 zzjgdmz = "";
                 zzjgdmafj = "";

                 swdjz = "";
                 swdjzfj = "";


                 ybnsrzgzsfj = "";
                 frdbxm = "";
                 frdbsfzh = "";
                 frdbsfzsmj = "";
                 srdbsfzfmmj = "";
                 frdbsqs = "";
                 zqzkzh = erpNumber.GetZQZJNumber("个人");
               
            }

            if(strdw.Equals(""))
            {
                 this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert(请选择注册类别！');</script>");
            }

            

            for (int i = 1; i <= Convert.ToInt32(this.txtNum.Text); i++)
            {
                stroder = i.ToString("#0000");
                yhm = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Second.ToString() + dt.Millisecond.ToString() + stroder;

               
                if (radDW.Checked)
                {
                    
                    zqzkzh = erpNumber.GetZQZJNumber("单位");
                }
                if (radZRR.Checked)
                {
                   
                    zqzkzh = erpNumber.GetZQZJNumber("个人");

                }


               

             
                Number = jhjx_PublicClass.GetNextNumberZZ("AAA_DLZHXXB", "");

                if (jszhlx.Equals("经纪人交易账户"))
                {
                    seljsbh = "";
                    butjsbh = "C" + Number;
                    jjrjsbh = "J" + Number;
                   
                        gljjrdlyx = yhm  + "zhaomin@qq.com";
                        gljjrbh = jjrjsbh;
                        gljjryhm = "zhaomin" + yhm ;
                        JJRZGZSBH = "D" + Number;
                   
                }

                if (jszhlx.Equals("买家卖家交易账户"))
                {
                    seljsbh = "B" + Number;
                    butjsbh = "C" + Number;
                    jjrjsbh = "";
                    JJRZGZSBH = "";
                }


               

                
             
                SQlz = " insert into AAA_DLZHXXB( Number, B_DLYX, B_YHM, B_DLMM, B_JSZHMM, B_JSZHLX, B_YXYZM, B_YZMGQSJ, B_SFYZYX," +
                "B_SFYXDL, B_SFDJ, B_SFXM, B_ZCSJ, B_ZCIP, B_DLCS," +
                "B_ZHDQXYFZ, B_ZHDQKYYE, B_DSFCGKYYE, J_SELJSBH, J_BUYJSBH, J_JJRJSBH, " +
                " J_JJRZGZSBH, JJRZGZS, J_JJRZGZSYXQKSSJ, J_JJRZGZSYXQJSSJ, " +
                "S_SFYBJJRSHTG, S_SFYBFGSSHTG, I_ZCLB, I_JYFMC, I_YYZZZCH, I_YYZZSMJ, I_SFZH, I_SFZSMJ, I_SFZFMSMJ," +
                "I_ZZJGDMZDM, I_ZZJGDMZSMJ, I_SWDJZSH, I_SWDJZSMJ, I_YBNSRZGZSMJ, I_KHXKZH, I_YLYJK, I_KHXKZSMJ," +
                "I_FDDBRXM, I_FDDBRSFZH, I_FDDBRSFZSMJ, I_FDDBRSFZFMSMJ, I_FDDBRSQS, I_JYFLXDH, I_SSQYS, I_SSQYSHI," +
                "I_SSQYQ, I_XXDZ, I_LXRXM, I_LXRSJH, I_KHYH, I_YHZH, I_DSFCGZT, I_ZQZJZH, I_PTGLJG, I_ZLTJSJ, " +
                " I_JJRFL, I_YWGLBMFL,  CheckState, CreateUser, CreateTime,J_JJRSFZTXYHSH )" +
                "values" +
                "('" + Number + "','" + yhm  + "zhaomin@qq.com" + "','" + "zhaomin" + yhm + "','123456','123456','" + jszhlx + "','JYC6fRU9F5','" + dt.AddDays(1) + "','是','是','否','否','" + dt + "','192.168.16.21','1'," +
                "'0','"+this.txtKHYE.Text.Trim()+"','"+this.txtKHYE.Text.Trim()+"','" + seljsbh + "','" + butjsbh + "','" + jjrjsbh + "','" + JJRZGZSBH + "','/JHJXPT/SaveDir/JJRZGZS/82a0ceae-680f-4f1f-9f9e-bc94d43ede9f.png','" + dt + "','" + dt.AddYears(1) + "','是','是','" + strdw + "','" + "zhaomin" + yhm  + "','" + yyzz + "','" + yyzzfj + "','" + sfzh + "','" + sfzfj + "','" + sfzfmfj + "','" + zzjgdmz + "','" + zzjgdmafj + "','" + swdjz + "','" + swdjzfj + "','" + ybnsrzgzsfj + "','123456','201310\\d320b4ba-111c-48ba-860a-26235b230d9d.jpg','201310\\d320b4ba-111c-48ba-860a-26235b230d9d.jpg','" + frdbxm + "','" + frdbsfzh + "','" + frdbsfzsmj + "','" + srdbsfzfmmj + "','" + frdbsqs + "','12354865454','山东省','济南市','长清区','平安镇富美路一号','zm','13586945687','浦发银行','1234562865','开通','" + zqzkzh + "','山东分公司','" + dt + "','" + JJRFL + "','分公司',0,'admin','" + dt + "','否')";

                sqllist.Add(SQlz);

              


                SQLjs = " insert into AAA_MJMJJYZHYJJRZHGLB ( Number, DLYX, JSZHLX, GLJJRDLZH, GLJJRBH, GLJJRYHM, SFDQMRJJR, SQSJ, JJRSHZT, JJRSHSJ," +
                "JJRSHYJ, FGSSHZT, FGSSHR, FGSSHSJ, FGSSHYJ, SFSCGLJJR, SFZTYHXYW,  SFYX,  " +
                "  CheckState, CreateUser, CreateTime, CheckLimitTime) " +
                " values " +
                "( '" + jhjx_PublicClass.GetNextNumberZZ("AAA_MJMJJYZHYJJRZHGLB", "") + "','" + yhm  + "zhaomin@qq.com" + "','" + jszhlx + "','" + gljjrdlyx + "','" + gljjrbh + "','" + gljjryhm + "','是','" + dt + "','审核通过','" + dt + "','审核通过','审核通过','系统管理员','" + dt + "','审核通过','是','否','是',0,'admin','" + dt + "','" + dt + "' )";

                sqllist.Add(SQLjs);
              
            }

            DbHelperSQL.ExecSqlTran(sqllist);

            Response.Write("插入成功！");

        }
        else
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert(请输入生成账户数目！');</script>");
        } 

    }
}