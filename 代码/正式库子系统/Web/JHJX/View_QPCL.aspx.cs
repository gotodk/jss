using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Configuration;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;
using System.Threading;
//using TestPT2013services;
//using CallPT2013Services;//正式库service

public partial class Web_JHJX_View_QPCL : System.Web.UI.Page
{
    static string   selleremail="", buyeremail = "",ZBDBXXBBH="",zfje="",zflyzh="",zfmbzh=""; 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
 

            
            if (Context.Handler.ToString() == "ASP.web_jhjx_jhjx_qpcl_aspx")
            {
                DataRow[] drs = this.Context.Items["Text"] as DataRow[];

                InitialData(drs[0]);

                if (divQPZT.InnerText.Trim() == "清盘结束")
                {
                    this.txtPTYJ.ReadOnly = true;
                    this.btnAdd.Enabled = false;
                }
              
            }           
        }       

    }
    //初始化页面数据
    protected void InitialData(DataRow dr)
    {
        divQRRQ.InnerText = dr["确认时间"].ToString().Trim();
        divHTJSRQ.InnerText = dr["合同结束日期"].ToString().Trim();
        divHTBH.InnerText = dr["电子购货合同编号"].ToString().Trim();
        divSPBH.InnerText = dr["商品编号"].ToString().Trim();
        divSPMC.InnerText = dr["商品名称"].ToString().Trim();
        divSPGG.InnerText = dr["规格"].ToString().Trim();
        divQPZT.InnerText = dr["清盘状态"].ToString().Trim();
        divHTSL.InnerText = dr["合同数量"].ToString().Trim();
        divDJ.InnerText = dr["单价"].ToString().Trim();
        divHTJE.InnerText = dr["合同金额"].ToString().Trim();
        divHKDJJE.InnerText = dr["争议金额"].ToString().Trim();
        divLYBZJ.InnerText = dr["履约保证金金额"].ToString().Trim();
        divZYSL.InnerText = dr["争议数量"].ToString().Trim();
        divZYJE.InnerText = dr["争议金额"].ToString().Trim();
        divQPYY.InnerText = dr["清盘原因"].ToString().Trim();
        divQPKSSJ.InnerText = dr["清盘开始时间"].ToString().Trim();
        divQPJSSJ.InnerText = dr["清盘结束时间"].ToString().Trim();
        selleremail = dr["卖方邮箱"].ToString().Trim();
        buyeremail = dr["买方邮箱"].ToString().Trim();
        ZBDBXXBBH = dr["主键"].ToString().Trim();
        zfje = dr["转付金额"].ToString().Trim();
        zflyzh = dr["转付来源账户"].ToString().Trim();
        zfmbzh = dr["转付目标账户"].ToString().Trim();

        divCLYJ.InnerText = dr["处理依据"].ToString().Trim();
        this.lwjck.HRef = "../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dr["证明文件路径"].ToString().Replace(@"\", "/");

        string buyer = DbHelperSQL.GetSingle("SELECT I_JYFMC FROM AAA_DLZHXXB WHERE B_DLYX='"+dr["买方邮箱"].ToString().Trim()+"'").ToString().Trim();
        string seller = DbHelperSQL.GetSingle("SELECT I_JYFMC FROM AAA_DLZHXXB WHERE B_DLYX='" + dr["卖方邮箱"].ToString().Trim() + "'").ToString().Trim();
        string scf = DbHelperSQL.GetSingle("SELECT I_JYFMC FROM AAA_DLZHXXB WHERE B_DLYX='" + dr["证明上传方邮箱"].ToString().Trim() + "'").ToString().Trim();
        txtCLSM.Text = seller + "卖方与" + buyer + "买方双方就" + dr["电子购货合同编号"].ToString().Trim()+"《电子购货合同》存在的所有争议已达成最终处理结果，现由"+scf+"上传证明文件。请贵平台按证明文件规定将" + dr["转付来源账户"].ToString().Trim() + "账户上的" + dr["转付金额"].ToString().Trim() + "元转付给" + dr["转付目标账户"].ToString().Trim() + "账户";

        txtJGQR.Text = seller + "卖方与" + buyer + "买方双方就" + dr["电子购货合同编号"].ToString().Trim() + "《电子购货合同》存在的所有争议已达成最终处理结果，现对"+scf+"上传的证明文件确认是真实的，证明文件中所表述的双方处理结果，是双方真实的意思表示";
         object obj=DbHelperSQL.GetSingle("  select PTCLYJ from AAA_RGQPCLB where ZBDBXXBBH='" + ZBDBXXBBH + "'");
        if ( obj != null)
        { txtPTYJ.Text = obj.ToString(); }
    }

    /// <summary>
    /// 获取写入账款流水明细表的SQL语句
    /// </summary>
    /// <param name="num">对照表主键编号</param>
    /// <param name="dlyx">登陆邮箱</param>
    /// <param name="mname">模块名</param>
    /// <param name="je">金额</param>
    /// <param name="lydh">来源单号</param>
    /// <param name="replacex1">替代字符串</param>
    /// <param name="replacex2">替代字符串</param>
    /// <returns></returns>
    protected string SqlInsertText(string num,string dlyx,string mname, string je,string lydh,string replacex1,string replacex2 ,string zhxz)
    {
        WorkFlowModule WFM = new WorkFlowModule("AAA_ZKLSMXB");
        string key=WFM.numberFormat.GetNextNumberZZ("");

        DataTable dtemp1 = DbHelperSQL.Query("select * from AAA_moneyDZB where Number='"+num+"'").Tables[0];
        string xm = dtemp1.Rows[0]["XM"].ToString().Trim();
        string xz = dtemp1.Rows[0]["XZ"].ToString().Trim();
        string zy = dtemp1.Rows[0]["ZY"].ToString().Trim().Replace("[x1]", replacex1).Replace("[x2]", replacex2);
        string sjlx = dtemp1.Rows[0]["SJLX"].ToString().Trim();
        string yslx = dtemp1.Rows[0]["YSLX"].ToString().Trim();

        DataTable dtemp2 = DbHelperSQL.Query("select B_JSZHLX, J_SELJSBH,J_BUYJSBH,J_JJRJSBH from AAA_DLZHXXB  where B_DLYX='" + dlyx + "'").Tables[0];
        string jszhlx = dtemp2.Rows[0]["B_JSZHLX"].ToString().Trim();

        string jsbh = "";
        if (zhxz == "买家" && dtemp2.Rows[0]["J_BUYJSBH"]!=null)
        {
           jsbh=dtemp2.Rows[0]["J_BUYJSBH"].ToString().Trim();
        }
        else if (zhxz == "卖家" && dtemp2.Rows[0]["J_SELJSBH"]!=null)
        {
            jsbh = dtemp2.Rows[0]["J_SELJSBH"].ToString().Trim();
        }
        else if (zhxz == "经纪人" && dtemp2.Rows[0]["J_JJRJSBH"]!=null)
        {
            jsbh = dtemp2.Rows[0]["J_JJRJSBH"].ToString().Trim();
        }
            

        string str = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM] ,[XZ],[ZY] ,[JKBH],[SJLX],[CheckState],[CreateUser],[CreateTime]) VALUES ('" + key + "','" + dlyx + "','" + jszhlx + "','" + jsbh + "','"+mname+"','"+lydh+"',getdate(),'" + yslx + "',"+je+",'" + xm + "','" + xz + "','" + zy + "','接口编号','" + sjlx + "',0,'" + User.Identity.Name.ToString() + "',GETDATE())";

        dtemp1.Dispose();
        dtemp2.Dispose();

        return str;
 
    }
   
    protected void btnCommit_Click(object sender, EventArgs e)
    {

        //测试库服务
        //TestPT2013services.ws2013 ws = new TestPT2013services.ws2013();

        //正式库服务,布署时修改一下
        //CallPT2013Services.ws2013 ws = new CallPT2013Services.ws2013();
        if (txtPTYJ.Text.Trim() == "")
        {
            MessageBox.Show(this, "请先填写平台意见");

        }
        else
        {
            //人工清盘处理表编号
            WorkFlowModule WFM = new WorkFlowModule("AAA_RGQPCLB");
            string KeyNumber = WFM.numberFormat.GetNextNumber();

            List<string> list = new List<string>();

            //解冻卖家的履约保证金
            string str1 = SqlInsertText("1304000048", selleremail, "AAA_ZBDBXXB", divLYBZJ.InnerText.Trim(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "","卖家");
            list.Add(str1);

            //扣罚卖家的延迟录入发货信息违约赔偿金
            object obj1304000026 = DbHelperSQL.GetSingle("SELECT SUM(JE) FROM AAA_ZKLSMXB JOIN AAA_THDYFHDXXB ON LYDH=AAA_THDYFHDXXB.Number  where XM='违约赔偿金' and XZ='超过最迟发货日后录入发货信息' and SJLX='预' AND ZBDBXXBBH='" + ZBDBXXBBH + "' AND DLYX='" + selleremail + "'");
            double je1304000026 = obj1304000026 == null ? 0 : Convert.ToDouble(obj1304000026);
            if (obj1304000026 != null)//zhouli 作废 2014.07.28 需求要求
            {
                string str2 = SqlInsertText("1304000026", selleremail, "AAA_ZBDBXXB", je1304000026.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "","卖家");
                list.Add(str2);
            }

            //扣罚卖家的延迟录入发票邮寄信息违约赔偿金
            object obj1304000028 = DbHelperSQL.GetSingle("SELECT SUM(JE) FROM AAA_ZKLSMXB JOIN AAA_THDYFHDXXB ON LYDH=AAA_THDYFHDXXB.Number   where XM='违约赔偿金' and XZ='发货单生成后5日内未录入发票邮寄信息' and SJLX='预'  AND ZBDBXXBBH='" + ZBDBXXBBH + "' AND DLYX='" + selleremail + "' ");
            double je1304000028 = obj1304000028 == null ? 0 : Convert.ToDouble(obj1304000028);
            if (obj1304000028 != null)//zhouli 作废 2014.07.28 需求要求
            {
                string str3 = SqlInsertText("1304000028", selleremail, "AAA_ZBDBXXB", je1304000028.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "","卖家");
                list.Add(str3);
            }

            //给买家增加延迟录入发货信息补偿收益
            object obj1304000036 = DbHelperSQL.GetSingle("SELECT SUM(JE) FROM AAA_ZKLSMXB JOIN AAA_THDYFHDXXB ON LYDH=AAA_THDYFHDXXB.Number   where XM='补偿收益' and XZ='收到卖方发货延迟补偿' and SJLX='预'  AND ZBDBXXBBH='" + ZBDBXXBBH + "' AND DLYX='" + buyeremail + "'");
            double je1304000036 = obj1304000036 == null ? 0 : Convert.ToDouble(obj1304000036);
            if (obj1304000036 != null)//zhouli 作废 2014.07.28 需求要求
            {
                string str4 = SqlInsertText("1304000036", buyeremail, "AAA_ZBDBXXB", je1304000036.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "","买家");
                list.Add(str4);

            }

            //给买家增加延迟录入发票信息补偿收益
            object obj1304000037 = DbHelperSQL.GetSingle("SELECT SUM(JE)  FROM AAA_ZKLSMXB JOIN AAA_THDYFHDXXB ON LYDH=AAA_THDYFHDXXB.Number where XM='补偿收益' and XZ='收到卖方发票延迟补偿' and SJLX='预' AND ZBDBXXBBH='" + ZBDBXXBBH + "' AND DLYX='" + buyeremail + "'");
            double je1304000037 = obj1304000037 == null ? 0 : Convert.ToDouble(obj1304000037);
            if (obj1304000037 != null)//zhouli 作废 2014.07.28 需求要求
            {
                string str5 = SqlInsertText("1304000037", buyeremail, "AAA_ZBDBXXB", je1304000037.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "","买家");
                list.Add(str5);
            }

            double lybzj =divLYBZJ.InnerText.Trim()==""?0: Convert.ToDouble(divLYBZJ.InnerText.Trim());
            double c = lybzj - je1304000026 - je1304000028 < 0 ? 0 : lybzj - je1304000026 - je1304000028;
            double el = divHTSL.InnerText.Trim() == "" ? 0 : Convert.ToDouble(divHTSL.InnerText.Trim());
            double je1304000031 = 0, je1304000030=0;
            if (divQPYY.InnerText.Trim() == "《电子购货合同》期满")
            {
                //违约赔偿金,电子购货合同期满卖家未录入发货信息扣罚
                object sl = DbHelperSQL.GetSingle("select SUM(T_THSL) from AAA_THDYFHDXXB where ZBDBXXBBH='"+ZBDBXXBBH+"' and F_DQZT in ('未生成发货单','已生成发货单')");
                double d = sl == null ? 0 : Convert.ToDouble(sl);
                je1304000031 = Math.Round( (d / el) * c,2);



                //if (je1304000031 != 0)
                //{
                    //对卖家的扣罚
                    string str6 = SqlInsertText("1304000031", selleremail, "AAA_ZBDBXXB", je1304000031.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "","卖家");
                    list.Add(str6);
                    
                    //对买家的补偿
                    string str7 = SqlInsertText("1304000039", buyeremail, "AAA_ZBDBXXB", je1304000031.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "", "买家");
                    list.Add(str7);
                //}
            }
            else if (divQPYY.InnerText.Trim() == "废标")
            {
                //违约赔偿金,电子购货合同履约保证金不足废标扣罚
                object sl = DbHelperSQL.GetSingle("select SUM(T_THSL) from AAA_THDYFHDXXB where ZBDBXXBBH='" + ZBDBXXBBH + "' and F_DQZT not in ('未生成发货单','已生成发货单','撤销')");
                double d = sl == null ? el-0 :el - Convert.ToDouble(sl);
                je1304000030 = Math.Round((d / el) * c,2);

                //if (je1304000030 != 0)
                //{
                    //对卖家的扣罚
                    string str8 = SqlInsertText("1304000030", selleremail, "AAA_ZBDBXXB", je1304000030.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "","卖家");
                    list.Add(str8);

                    //对买家的补偿
                    string str9 = SqlInsertText("1304000038", buyeremail, "AAA_ZBDBXXB", je1304000030.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "", "买家");
                    list.Add(str9);
                //}
            }


            //流水表中买家未解冻货款的解冻，一个提货单一条记录DataTable
            //先获取一个关于每个发货单需要解冻多少的
            DataTable djd = DbHelperSQL.Query("SELECT LYDH,SUM(CJE) AS JDJE FROM (SELECT  DLYX,XM,XZ, CASE WHEN XZ='下达提货单冻结' THEN  JE ELSE -1*JE END AS CJE,LYDH  from  AAA_ZKLSMXB JOIN AAA_THDYFHDXXB ON AAA_THDYFHDXXB.Number=LYDH where XM='货款' AND XZ IN ('下达提货单冻结','无异议收货解冻','强制解冻','卖方主动退货解冻','请重新发货解冻') AND DLYX='" + buyeremail + "' AND ZBDBXXBBH='" + ZBDBXXBBH + "') A GROUP BY LYDH").Tables[0];
            //买家此类需人工清盘解冻的总额
            double zjdje = 0;
            if (djd != null && djd.Rows.Count > 0)
            {
                zjdje = Convert.ToDouble(djd.Compute("Sum(JDJE)", ""));
                for (int i = 0; i < djd.Rows.Count; i++)
                {
                    //发货单号
                    string thdh = djd.Rows[i]["LYDH"].ToString().Trim();
                    string fhdh = "F" + djd.Rows[i]["LYDH"].ToString().Trim();

                    if (djd.Rows[i]["JDJE"] != null && Convert.ToDouble(djd.Rows[i]["JDJE"]) != 0)
                    {
                        string jdje = djd.Rows[i]["JDJE"].ToString();
                        string strs = SqlInsertText("1304000052", buyeremail, "AAA_THDYFHDXXB", jdje, thdh, fhdh, divHTBH.InnerText.Trim(), "买家");
                        list.Add(strs);
                    }
                }
            }

            //卖家当前余额
            object objsellye = DbHelperSQL.GetSingle("select B_ZHDQKYYE from AAA_DLZHXXB where B_DLYX='" + selleremail + "'");
            double sellye = objsellye == null ? 0 : Convert.ToDouble(objsellye);

            //买家当前余额
            object objbuyye = DbHelperSQL.GetSingle("select B_ZHDQKYYE from AAA_DLZHXXB where B_DLYX='" + buyeremail + "'");
            double buyye = objbuyye == null ? 0 : Convert.ToDouble(objbuyye);

            //卖家变动金额，不含转付
            double sellchange = Convert.ToDouble(divLYBZJ.InnerText.Trim()) - je1304000026 - je1304000028 - je1304000030 - je1304000031;

            //卖家可用余额
            double kysellye = sellye +sellchange;

            //买家变动金额，不含转付
            double buychange = je1304000036 + je1304000037 + je1304000030 + je1304000031 + zjdje;
            //买家可用余额
            double kybuyye = buyye + buychange;

            //按双方协议或者法律文书达成的转让，以及双方最终余额的更新
            //实际转服金额
            double sjzfje = 0;

            //资金帐款明细表的写入
            if (selleremail == buyeremail)
            {
                double sjye = kybuyye + kysellye - sellye;

                //实际转付金额，由于是同一家，故对余额没有影响
 
                sjzfje = sjye >= Convert.ToDouble(zfje) ? Convert.ToDouble(zfje) : sjye;

               
                if (zflyzh == buyeremail)
                {
                    string strsellzf = SqlInsertText("1304000040", selleremail, "AAA_ZBDBXXB", sjzfje.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "", "卖家");

                    string strbuyzr = SqlInsertText("1304000033", buyeremail, "AAA_ZBDBXXB", sjzfje.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "", "买家");

                    list.Add(strsellzf);
                    list.Add(strbuyzr);     
                }
                else
                {
                    string strsellzf = SqlInsertText("1304000033", selleremail, "AAA_ZBDBXXB", sjzfje.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "", "卖家");

                    string strbuyzr = SqlInsertText("1304000040", buyeremail, "AAA_ZBDBXXB", sjzfje.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "", "买家");

                    list.Add(strsellzf);
                    list.Add(strbuyzr);
                }
            }
            //买卖双方非一家
            else
            {
                if (zflyzh == selleremail)
                {
                    //实际转付金额,如果余额小于转付金额，按余额转
                    sjzfje = kysellye >= Convert.ToDouble(zfje) ? Convert.ToDouble(zfje) : kysellye;
                    //卖家最终余额
                    //double zzsellye = kysellye - sjzfje;
                    //买家最终余额
                    //double zzbuyye = kybuyye + sjzfje;

                    //更新帐款流水明细
                    string strsellzf = SqlInsertText("1304000033", selleremail, "AAA_ZBDBXXB", sjzfje.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "","卖家");

                    string strbuyzr = SqlInsertText("1304000040", buyeremail, "AAA_ZBDBXXB", sjzfje.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "", "买家");

                    list.Add(strsellzf);
                    list.Add(strbuyzr);
                 

                }
                else if (zflyzh == buyeremail)
                {

                    //实际转付金额,如果余额小于转付金额，按余额转
                    sjzfje = kybuyye >= Convert.ToDouble(zfje) ? Convert.ToDouble(zfje) : kybuyye;
                    //卖家最终余额
                    //double zzsellye = kysellye + sjzfje;
                    //买家最终余额
                    //double zzbuyye = kybuyye - sjzfje;

                    //更新帐款流水明细
                    string strsellzf = SqlInsertText("1304000040", selleremail, "AAA_ZBDBXXB", sjzfje.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "","卖家");

                    string strbuyzr = SqlInsertText("1304000033", buyeremail, "AAA_ZBDBXXB", sjzfje.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "", "买家");

                    list.Add(strsellzf);
                    list.Add(strbuyzr);     
                }
 
            }    
            
            //仅当买家向卖家支付争议金额时，扣减卖家技术服务费
            double je1306000001 = 0, je1304000042 = 0, je1304000043= 0;
            string selljjr = "",buyjjr="";
            //获取买卖双方的经纪人
            DataTable dtb1 = DbHelperSQL.Query("select Number,T_YSTBDGLJJRYX,Y_YSYDDGLJJRYX  from AAA_ZBDBXXB where Number='" + ZBDBXXBBH + "'").Tables[0];
            if (dtb1 != null && dtb1.Rows.Count > 0)
            {
                selljjr = dtb1.Rows[0]["T_YSTBDGLJJRYX"].ToString();
                buyjjr = dtb1.Rows[0]["Y_YSYDDGLJJRYX"].ToString();
            }

            if (zflyzh == buyeremail)
            {
                //技术服务费取解冻货款与页面转付金额两者最小值的1%
                je1306000001 = zjdje < Convert.ToDouble(zfje) ? zjdje * 0.01 : Convert.ToDouble(zfje) * 0.01;
                //写入资金流水明细
                string strwhf = SqlInsertText("1306000001", selleremail, "AAA_ZBDBXXB", je1306000001.ToString(), ZBDBXXBBH, divHTBH.InnerText.Trim(), "","卖家");

                list.Add(strwhf);
              
                //计算经纪人的收益并写入资金流水明细
                je1304000042 = je1306000001 * 0.27;
                je1304000043 = je1306000001 * 0.35;

                string selljrrsy = SqlInsertText("1304000042", selljjr, "AAA_ZBDBXXB", je1304000042.ToString(), ZBDBXXBBH, selleremail, ZBDBXXBBH,"经纪人");
                string buyjjrsy = SqlInsertText("1304000043", buyjjr, "AAA_ZBDBXXB", je1304000043.ToString(), ZBDBXXBBH, buyeremail, ZBDBXXBBH, "经纪人");
                list.Add(selljrrsy);
                list.Add(buyjjrsy);

            }

            //写入人工清盘处理表
            string insert = " insert into [AAA_RGQPCLB] ([Number] ,[S_JDLYBZJ],[S_YCFHPCJ],[S_YCFPPCJ],[B_YCFHBCSY],[B_YCFPBCSY],[B_HKJD] ,[SJZFJE],[PTCLYJ],[CheckState],[CreateUser] ,[CreateTime],[ZBDBXXBBH],[S_QMWLFHXXKF],[B_QMWLFHXXBC],[S_HTYCZZKF],[B_HTYCZZBC]) values ('" + KeyNumber + "'," + divLYBZJ.InnerText.Trim() + "," + je1304000026 + "," + je1304000028 + "," + je1304000036 + "," + je1304000037 + "," + zjdje + "," + sjzfje + ",'" + txtPTYJ.Text.Trim() + "',1,'" + User.Identity.Name + "',getdate(),'"+ZBDBXXBBH+"','"+je1304000031+"','"+je1304000031+"','"+je1304000030+"','"+je1304000030+"')";
            list.Add(insert);
            

            //更新中标定标信息表中的状态
            string update = "update AAA_ZBDBXXB set Z_QPZT='清盘结束',Z_QPJSSJ=GETDATE() where Number='"+ZBDBXXBBH+"'";
            list.Add(update);

            //更改账户余额
            if (buyeremail != selleremail)
            {
                if (zflyzh == selleremail)
                {
                    sellchange = sellchange - sjzfje;
                    buychange = buychange + sjzfje;    

                }
                else if (zflyzh == buyeremail)
                {
                    //当转出账户是买家时需扣除卖家（不是买家）的技术服务费
                    sellchange = sellchange + sjzfje - je1306000001;
                    buychange = buychange - sjzfje;
                   
                }


                string buyfh = buychange < 0 ? "-" : "+";
                string upbuyye = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE " + buyfh + " " + Math.Abs(buychange).ToString() + " where B_DLYX='" + buyeremail + "'";

                list.Add(upbuyye);

                string sellfh = sellchange < 0 ? "-" : "+";
                string upsellye = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE " + sellfh + " " + Math.Abs(sellchange).ToString() + " where B_DLYX='" + selleremail + "'";

                list.Add(upsellye);

            }
            else if (buyeremail == selleremail)
            {
                //更改余额，注意扣减技术服务费
                double change = buychange + sellchange - je1306000001;
                if (change != 0)
                {
                    string fh = change < 0 ? "-" : "+";
                    string upye = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE " + fh + " " + Math.Abs(change).ToString() + " where B_DLYX='" + selleremail + "'";

                    list.Add(upye);

                }
            }
            string time = Convert.ToDateTime(divQRRQ.InnerText.Trim()).ToString("yyyy年M月d日H时m分"); 
            
            if (DbHelperSQL.ExecuteSqlTran(list) > 0)
            {
                //////调用服务更改最终余额
                ////if (buyeremail != selleremail)
                ////{
                ////    if (zflyzh == selleremail)
                ////    {
                ////        sellchange = sellchange - sjzfje;
                ////        buychange = buychange + sjzfje;

                ////        MonneyService(buyeremail, buychange, selleremail, sellchange, ws);

                ////    }
                ////    else if (zflyzh == buyeremail)
                ////    {
                ////        //当转出账户是买家时需扣除卖家（不是买家）的技术服务费
                ////        sellchange = sellchange + sjzfje-je1306000001;
                ////        buychange = buychange - sjzfje;

                ////        MonneyService(buyeremail, buychange, selleremail, sellchange, ws);
                ////    }

                ////}
                ////else if (buyeremail == selleremail)
                ////{
                ////    //调用服务更改余额，注意扣减技术服务费
                ////    double change = buychange + sellchange-je1306000001;
                ////    if (change != 0)
                ////    {
                ////        MonneyService(selleremail,change,ws);
                      
                ////    }
                ////}

                List<Hashtable> ls = new List<Hashtable>();
                //卖方提醒
                DataSet ds = DbHelperSQL.Query("SELECT I_JYFMC 交易方名称,B_YHM AS 用户名,B_JSZHLX AS 结算账户类型,J_BUYJSBH AS 买家角色编号 FROM AAA_DLZHXXB WHERE B_DLYX='" + selleremail + "'");
                if (zflyzh == buyeremail)
                {
                    Hashtable sel = new Hashtable();
                    sel["type"] = "集合集合经销平台";
                    sel["提醒对象登陆邮箱"] = selleremail;
                    sel["提醒对象用户名"] = ds.Tables[0].Rows[0]["用户名"].ToString().Trim();
                    sel["提醒对象结算账户类型"] = ds.Tables[0].Rows[0]["结算账户类型"].ToString().Trim();
                    sel["提醒对象角色编号"] = ds.Tables[0].Rows[0]["买家角色编号"].ToString().Trim();
                    sel["提醒对象角色类型"] = "卖家";
                    sel["提醒内容文本"] = "尊敬的" + ds.Tables[0].Rows[0]["交易方名称"].ToString().Trim() + "，您" + divHTBH.InnerText.Trim() + "《电子购货合同》已人工清盘结束， " + zfje + "元的赔付款已转付至您的交易账户， " + je1306000001.ToString("#0.00") + "元的技术服务费已同时扣收，敬请知悉。";
                    sel["创建人"] = User.Identity.Name;
                    ls.Add(sel);

                    string strSQL = "select I_JYFMC,B_JSZHLX,J_JJRJSBH from AAA_DLZHXXB where B_DLYX='" + selljjr + "'";
                    DataSet dsSelJJR= DbHelperSQL.Query(strSQL);
                    if (dsSelJJR != null && dsSelJJR.Tables[0].Rows.Count>0)
                    {
                        Hashtable SelJJR = new Hashtable();
                        SelJJR["type"] = "集合集合经销平台";
                        SelJJR["提醒对象登陆邮箱"] = selljjr;
                        SelJJR["提醒对象用户名"] = dsSelJJR.Tables[0].Rows[0]["I_JYFMC"].ToString().Trim();
                        SelJJR["提醒对象结算账户类型"] = dsSelJJR.Tables[0].Rows[0]["B_JSZHLX"].ToString().Trim();
                        SelJJR["提醒对象角色编号"] = dsSelJJR.Tables[0].Rows[0]["J_JJRJSBH"].ToString().Trim();
                        SelJJR["提醒对象角色类型"] = "经纪人";
                        SelJJR["提醒内容文本"] = "尊敬的" + dsSelJJR.Tables[0].Rows[0]["I_JYFMC"].ToString().Trim() + "，您的交易账户于" + time + "新增经纪人收益" + je1304000042.ToString("#0.00") + "元，请继续努力！";
                        SelJJR["创建人"] = User.Identity.Name;
                        ls.Add(SelJJR);
                    }

                    strSQL = "select I_JYFMC,B_JSZHLX,J_JJRJSBH from AAA_DLZHXXB where B_DLYX='" + buyjjr + "'";
                    DataSet dsBuyJJR= DbHelperSQL.Query(strSQL);
                    if (dsBuyJJR != null && dsBuyJJR.Tables[0].Rows.Count > 0)
                    {
                        Hashtable BuyJJR = new Hashtable();
                        BuyJJR["type"] = "集合集合经销平台";
                        BuyJJR["提醒对象登陆邮箱"] = buyjjr;
                        BuyJJR["提醒对象用户名"] = dsBuyJJR.Tables[0].Rows[0]["I_JYFMC"].ToString().Trim();
                        BuyJJR["提醒对象结算账户类型"] = dsBuyJJR.Tables[0].Rows[0]["B_JSZHLX"].ToString().Trim();
                        BuyJJR["提醒对象角色编号"] = dsBuyJJR.Tables[0].Rows[0]["J_JJRJSBH"].ToString().Trim();
                        BuyJJR["提醒对象角色类型"] = "经纪人";
                        BuyJJR["提醒内容文本"] = "尊敬的" + dsBuyJJR.Tables[0].Rows[0]["I_JYFMC"].ToString().Trim() + "，您的交易账户于" + time + "新增经纪人收益" + je1304000043.ToString("#0.00") + "元，请继续努力！";
                        BuyJJR["创建人"] = User.Identity.Name;
                        ls.Add(BuyJJR);
                    }
                }
                Hashtable sellht = new Hashtable();
                sellht["type"] = "集合集合经销平台";
                sellht["提醒对象登陆邮箱"] = selleremail;
                sellht["提醒对象用户名"] = ds.Tables[0].Rows[0]["用户名"].ToString().Trim();
                sellht["提醒对象结算账户类型"] = ds.Tables[0].Rows[0]["结算账户类型"].ToString().Trim();
                sellht["提醒对象角色编号"] = ds.Tables[0].Rows[0]["买家角色编号"].ToString().Trim();
                sellht["提醒对象角色类型"] = "卖家";
                sellht["提醒内容文本"] = "尊敬的" + ds.Tables[0].Rows[0]["交易方名称"].ToString().Trim() + "，您" + divHTBH.InnerText.Trim() + "《电子购货合同》已根据您" + time + "确认的" + divCLYJ.InnerText + "清盘结束，您可在“交易账户”查看有关情况。";
                sellht["创建人"] = User.Identity.Name;
               
                ls.Add(sellht);

                //买方提醒
                DataSet db = DbHelperSQL.Query("SELECT  I_JYFMC 交易方名称,B_YHM AS 用户名,B_JSZHLX AS 结算账户类型,J_BUYJSBH AS 买家角色编号 FROM AAA_DLZHXXB WHERE B_DLYX='" + buyeremail + "'");

                Hashtable buyht = new Hashtable();
                buyht["type"] = "集合集合经销平台";
                buyht["提醒对象登陆邮箱"] = buyeremail;
                buyht["提醒对象用户名"] = db.Tables[0].Rows[0]["用户名"].ToString().Trim();
                buyht["提醒对象结算账户类型"] = db.Tables[0].Rows[0]["结算账户类型"].ToString().Trim();
                buyht["提醒对象角色编号"] = db.Tables[0].Rows[0]["买家角色编号"].ToString().Trim();
                buyht["提醒对象角色类型"] = "买家";
                buyht["提醒内容文本"] = "尊敬的" + db.Tables[0].Rows[0]["交易方名称"].ToString().Trim() + "，您" + divHTBH.InnerText.Trim() + "《电子购货合同》已根据您" + time + "确认的" + divCLYJ.InnerText + "清盘结束，您可在“交易账户”查看有关情况。";
                buyht["创建人"] = User.Identity.Name;
               
                ls.Add(buyht);

                JHJX_SendRemindInfor.Sendmes(ls);


                #region 交易方信用变化情况 edittime 2013-09 

                jhjx_JYFXYMX xymc = new jhjx_JYFXYMX();
                string[] buystrs = xymc.ZSZHMR_SFWQLV(buyeremail, divHTBH.InnerText.Trim(), "买家", User.Identity.Name);
                string[] sellstrs = xymc.ZSZHMC_SFWQLV(selleremail, divHTBH.InnerText.Trim(), "卖家", User.Identity.Name);
                string[] bjjrstrs = new string[2];
                string[] sjjrstrs = new string[2];

                if (xymc.CanDoing(divHTBH.InnerText.Trim(), buyeremail, "买家"))
                {
                    bjjrstrs = xymc.GLJYF_WQLY(buyjjr, buyeremail, divHTBH.InnerText.Trim(), "经纪人", User.Identity.Name);

                }
                else
                {
                    bjjrstrs = xymc.GLJYF_WWQLY(buyjjr, buyeremail, divHTBH.InnerText.Trim(), "经纪人", User.Identity.Name);
 
                }

                if (xymc.CanDoing(divHTBH.InnerText.Trim(),selleremail,"卖家"))
                {
                    sjjrstrs = xymc.GLJYF_WQLY(selljjr, selleremail, divHTBH.InnerText.Trim(), "经纪人", User.Identity.Name);
                  
                }
                else
                {
                    sjjrstrs = xymc.GLJYF_WWQLY(selljjr, selleremail, divHTBH.InnerText.Trim(), "经纪人", User.Identity.Name);
                   
 
                }
                List<string> jfl = new List<string>();
                jfl.Add(buystrs[0]);
                jfl.Add(buystrs[1]);
                jfl.Add(sellstrs[0]);
                jfl.Add(sellstrs[1]);
                jfl.Add(sjjrstrs[0]);
                jfl.Add(sjjrstrs[1]);
                jfl.Add(bjjrstrs[0]);
                jfl.Add(bjjrstrs[1]);
                try
                {
                    DbHelperSQL.ExecuteSqlTran(jfl);
                }
                catch
                {
                    MessageBox.Show(this, "信用信息未更新成功，请联系管理员手动更改！");
                    btnAdd.Enabled = false;
                    LoginAgain(this);
                }              

                #endregion

                MessageBox.Show(this,"清盘操作结束，数据更新成功！");

                LoginAgain(this);
            }
            else
            {
                MessageBox.Show(this, "数据更新失败");
            }

        } 
      
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        LoginAgain(this);
    }

    // 关闭当前页面并打开前一页面  
    public void LoginAgain(Page page)
    {
        string script = " <script language='javascript'>window.open('JHJX_QPCL.aspx','_self');</script>";
       
         page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CloseWindow", script);

    }

    //public void MonneyService(string dlyx, double change, ws2013 ws)
    //{       

    //    try
    //    {
    //        ws.AccontMoney(dlyx, change);
    //    }
    //    catch
    //    {
    //        MessageBox.Show(this, "服务调用失败！");
    //    }
 
    //}

    //public void MonneyService(string buydlyx, double buychange,string selldlyx,double sellchange, ws2013 ws)
    //{       

    //    try
    //    {
    //        ws.AccontsMoneys(buydlyx,buychange, selldlyx,sellchange);
    //    }
    //    catch
    //    {
    //        MessageBox.Show(this, "服务调用失败！");
    //    }

    //}
   
}