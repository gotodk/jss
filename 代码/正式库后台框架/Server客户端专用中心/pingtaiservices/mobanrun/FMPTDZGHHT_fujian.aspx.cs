using FMipcClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pingtaiservices_mobanrun_FMPTDZGHHT_fujian : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string number = "";
        if (Request.Params["Number"] != null && Request.Params["Number"].ToString() != "" && Request.Params["Number"].ToString() != "xxxxxxxxxx")
        {
            number = Request.Params["Number"].ToString();
        }
        else
        {
            return;
        }



        DataTable dtcs = new DataTable("参数");
        dtcs.Columns.Add("中标定标_Number");
        dtcs.Rows.Add(number);
        object[] re = IPC.Call("电子购货合同信息", new object[] { dtcs });
        DataSet reFF = new DataSet();
        if (re[0].ToString() == "ok")
        {
            reFF = (DataSet)(re[1]);
        }
        else
        {
            Response.Write(re[1].ToString());
        }

        DataTable dataTableSeller = reFF.Tables["卖家资质"];
        DataTable dataTableBuyer = reFF.Tables["买家资质"];
        DataTable dataTableTBD = reFF.Tables["投标单"];
        DataTable dataTableYDD = reFF.Tables["预订单"];


        DataTable dataTableTBDZZ = reFF.Tables["投标单资质"];
        DataTable dataTableTHYFHXX = reFF.Tables["提货单发货单"];
        DataTable dataTableQTZZ = reFF.Tables["其他资质"];


        //开始显示数据
        //卖家资质
        this.lab_Seller_YYZZ.Attributes["showtupian"] = dataTableSeller.Rows[0]["营业执照扫描件"].ToString();
        this.lab_Seller_ZZJGDMZ.Attributes["showtupian"] = dataTableSeller.Rows[0]["组织机构代码证扫描件"].ToString();
        this.lab_Seller_SWDJZ.Attributes["showtupian"] = dataTableSeller.Rows[0]["税务登记证扫描件"].ToString();
        this.lab_Seller_KHXKZ.Attributes["showtupian"] = dataTableSeller.Rows[0]["开户许可证扫描件"].ToString();
        this.lab_Seller_YLYJK.Attributes["showtupian"] = dataTableSeller.Rows[0]["预留印鉴卡扫描件"].ToString();
        this.lab_Seller_FDDBRSFZ.Attributes["showtupian"] = dataTableSeller.Rows[0]["法定代表人身份证扫描件"].ToString();
        this.lab_Seller_FDDBRSFZ_FM.Attributes["showtupian"] = dataTableSeller.Rows[0]["法定代表人身份证反面扫描件"].ToString();
        this.lab_Seller_FDDBRSQS.Attributes["showtupian"] = dataTableSeller.Rows[0]["法定代表人授权书"].ToString();
        this.lab_Seller_SFZ.Attributes["showtupian"] = dataTableSeller.Rows[0]["身份证扫描件"].ToString();
        this.lab_Seller_SFZ_FM.Attributes["showtupian"] = dataTableSeller.Rows[0]["身份证反面扫描件"].ToString();
        //买家资质
        this.lab_Buyer_YYZZ.Attributes["showtupian"] = dataTableBuyer.Rows[0]["营业执照扫描件"].ToString();
        this.lab_Buyer_ZZJGDMZ.Attributes["showtupian"] = dataTableBuyer.Rows[0]["组织机构代码证扫描件"].ToString();
        this.lab_Buyer_SWDJZ.Attributes["showtupian"] = dataTableBuyer.Rows[0]["税务登记证扫描件"].ToString();
        this.lab_Buyer_KHXKZ.Attributes["showtupian"] = dataTableBuyer.Rows[0]["开户许可证扫描件"].ToString();
        this.lab_Buyer_YLYJK.Attributes["showtupian"] = dataTableBuyer.Rows[0]["预留印鉴卡扫描件"].ToString();
        this.lab_Buyer_FDDBRSFZ.Attributes["showtupian"] = dataTableBuyer.Rows[0]["法定代表人身份证反面扫描件"].ToString();
        this.lab_Buyer_FDDBRSFZ_FM.Attributes["showtupian"] = dataTableBuyer.Rows[0]["法定代表人身份证扫描件"].ToString();
        this.lab_Buyer_FDDBRSQS.Attributes["showtupian"] = dataTableBuyer.Rows[0]["法定代表人授权书"].ToString();
        this.lab_Buyer_SFZ.Attributes["showtupian"] = dataTableBuyer.Rows[0]["身份证扫描件"].ToString();
        this.lab_Buyer_SFZ_FM.Attributes["showtupian"] = dataTableBuyer.Rows[0]["身份证反面扫描件"].ToString();

        //隐藏和显示资质
        if (dataTableSeller.Rows[0]["注册类别"].ToString() == "单位")
        {
            s1.Visible = true;
            s2.Visible = true;
            s3.Visible = true;
            s4.Visible = false;
        }
        else
        {
            s1.Visible = false;
            s2.Visible = false;
            s3.Visible = false;
            s4.Visible = true;
        }

        if (dataTableBuyer.Rows[0]["注册类别"].ToString() == "单位")
        {
            b1.Visible = true;
            b2.Visible = true;
            b3.Visible = true;
            b4.Visible = false;
        }
        else
        {
            b1.Visible = false;
            b2.Visible = false;
            b3.Visible = false;
            b4.Visible = true;

        }


        //投标单
        tbdxz.Attributes["showkjdy"] = number;

        Label1.Text = dataTableTBD.Rows[0]["卖家名称"].ToString();
        Label2.Text = dataTableTBD.Rows[0]["发布时间"].ToString();
        Label3.Text = dataTableTBD.Rows[0]["投标单号"].ToString();
        Label13.Text = dataTableTBD.Rows[0]["拟卖出商品名称"].ToString();
        Label14.Text = dataTableTBD.Rows[0]["规格"].ToString();

        Label4.Text = dataTableTBD.Rows[0]["计价单位"].ToString();
        Label5.Text = dataTableTBD.Rows[0]["投标拟售量"].ToString();
        Label6.Text = dataTableTBD.Rows[0]["投标价格"].ToString();
        Label7.Text = dataTableTBD.Rows[0]["投标金额"].ToString();
        Label8.Text = dataTableTBD.Rows[0]["冻结的投标保证金"].ToString();
        Label9.Text = dataTableTBD.Rows[0]["平台设定最大经济批量"].ToString();
        Label10.Text = dataTableTBD.Rows[0]["卖家设定的经济批量"].ToString();
        Label11.Text = dataTableTBD.Rows[0]["合同期限"].ToString();
        string ghqy = dataTableTBD.Rows[0]["可供货区域"].ToString().Replace("|","，");
        Label12.Text = ghqy.Substring(1, ghqy.Length-2);

        //预订单
        yddxz.Attributes["showkjdy"] = number;

        Label15.Text = dataTableYDD.Rows[0]["买家名称"].ToString();
        Label16.Text = dataTableYDD.Rows[0]["下单时间"].ToString();
        Label17.Text = dataTableYDD.Rows[0]["预订单号"].ToString();
        Label18.Text = dataTableYDD.Rows[0]["拟买入商品名称"].ToString();
        Label19.Text = dataTableYDD.Rows[0]["规格"].ToString();
        Label20.Text = dataTableYDD.Rows[0]["计价单位"].ToString();
        Label21.Text = dataTableYDD.Rows[0]["拟订购数量"].ToString();
        Label22.Text = dataTableYDD.Rows[0]["拟买入价格"].ToString();
        Label23.Text = dataTableYDD.Rows[0]["拟订购金额"].ToString();
        Label24.Text = dataTableYDD.Rows[0]["冻结的订金"].ToString();
        Label25.Text = dataTableYDD.Rows[0]["合同期限"].ToString();
        string shqy = dataTableYDD.Rows[0]["收货区域"].ToString().Replace("|", "，");
        Label26.Text = shqy.Substring(1, shqy.Length - 2);

        //投标单资质
        this.lab_ZLBZYZM.Attributes["showtupian"] = dataTableTBDZZ.Rows[0]["质量标准与证明"].ToString();
        this.lab_CPJCBG.Attributes["showtupian"] = dataTableTBDZZ.Rows[0]["产品检测报告"].ToString();
        this.lab_PGFZRFLCNS.Attributes["showtupian"] = dataTableTBDZZ.Rows[0]["品管总负责人法律承诺书"].ToString();
        this.lab_FDDBRCNS.Attributes["showtupian"] = dataTableTBDZZ.Rows[0]["法定代表人承诺书"].ToString();
        this.lab_SHFWGDYCN.Attributes["showtupian"] = dataTableTBDZZ.Rows[0]["售后服务规定与承诺"].ToString();
        this.lab_CPSJSQS.Attributes["showtupian"] = dataTableTBDZZ.Rows[0]["产品送检授权书"].ToString();
        this.lab_SLZM.Attributes["showtupian"] = dataTableTBDZZ.Rows[0]["税率证明"].ToString();
        string[] strArray = dataTableTBDZZ.Rows[0]["资质01"].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("特殊资质名称", typeof(System.String));
        dataTable.Columns.Add("特殊资质路径", typeof(System.String));
        if (strArray.Length > 0)//说明存在特殊资质
        {
            for (int i = 0; i < strArray.Length; i++)
            {
                string[] strSingle = strArray[i].Split('*');
                string strTSZZMC = strSingle[0].ToString();//特殊资质名称
                string strTSZZLJ = strSingle[1].ToString().Trim();//特殊资质路径
                dataTable.Rows.Add(new object[] { strTSZZMC, strTSZZLJ });
            }
            for (int j = 0; j < 10 - strArray.Length; j++)//为了凑够10个资质
            {
                dataTable.Rows.Add(new object[] { "", "" });
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                dataTable.Rows.Add(new object[] { "", "" });
            }
        }
        this.lab_ZZ01.Attributes["showtupian"] = dataTable.Rows[0]["特殊资质路径"].ToString();
        this.lab_ZZ02.Attributes["showtupian"] = dataTable.Rows[1]["特殊资质路径"].ToString();
        this.lab_ZZ03.Attributes["showtupian"] = dataTable.Rows[2]["特殊资质路径"].ToString();
        this.lab_ZZ04.Attributes["showtupian"] = dataTable.Rows[3]["特殊资质路径"].ToString();
        this.lab_ZZ05.Attributes["showtupian"] = dataTable.Rows[4]["特殊资质路径"].ToString();
        this.lab_ZZ06.Attributes["showtupian"] = dataTable.Rows[5]["特殊资质路径"].ToString();
        this.lab_ZZ07.Attributes["showtupian"] = dataTable.Rows[6]["特殊资质路径"].ToString();
        this.lab_ZZ08.Attributes["showtupian"] = dataTable.Rows[7]["特殊资质路径"].ToString();
        this.lab_ZZ09.Attributes["showtupian"] = dataTable.Rows[8]["特殊资质路径"].ToString();
        this.lab_ZZ10.Attributes["showtupian"] = dataTable.Rows[9]["特殊资质路径"].ToString();
 

        if (dataTable.Rows[0]["特殊资质路径"].ToString() == "")
        {
            ;
        }
        else
        {
            this.lab_ZZ01.InnerText = dataTable.Rows[0]["特殊资质名称"].ToString();
        }
        if (dataTable.Rows[1]["特殊资质路径"].ToString() == "")
        {
            ;
        }
        else
        {
            this.lab_ZZ02.InnerText = dataTable.Rows[1]["特殊资质名称"].ToString();
        }
        if (dataTable.Rows[2]["特殊资质路径"].ToString() == "")
        {
            ;
        }
        else
        {
            this.lab_ZZ03.InnerText = dataTable.Rows[2]["特殊资质名称"].ToString();
        }
        if (dataTable.Rows[3]["特殊资质路径"].ToString() == "")
        {
            ;
        }
        else
        {
            this.lab_ZZ04.InnerText = dataTable.Rows[3]["特殊资质名称"].ToString();
        }
        if (dataTable.Rows[4]["特殊资质路径"].ToString() == "")
        {
             ;
        }
        else
        {
            this.lab_ZZ05.InnerText = dataTable.Rows[4]["特殊资质名称"].ToString();
        }
        if (dataTable.Rows[5]["特殊资质路径"].ToString() == "")
        {
            ;
        }
        else
        {
            this.lab_ZZ06.InnerText = dataTable.Rows[5]["特殊资质名称"].ToString();
        }
        if (dataTable.Rows[6]["特殊资质路径"].ToString() == "")
        {
            ;
        }
        else
        {
            this.lab_ZZ07.InnerText = dataTable.Rows[6]["特殊资质名称"].ToString();
        }
        if (dataTable.Rows[7]["特殊资质路径"].ToString() == "")
        {
            ;
        }
        else
        {
            this.lab_ZZ08.InnerText = dataTable.Rows[7]["特殊资质名称"].ToString();
        }
        if (dataTable.Rows[8]["特殊资质路径"].ToString() == "")
        {
            ;
        }
        else
        {
            this.lab_ZZ09.InnerText = dataTable.Rows[8]["特殊资质名称"].ToString();
        }
        if (dataTable.Rows[9]["特殊资质路径"].ToString() == "")
        {
            ;
        }
        else
        {
            this.lab_ZZ10.InnerText = dataTable.Rows[9]["特殊资质名称"].ToString();
        }

        if (dataTableQTZZ.Rows.Count > 0)
        {
            this.linkBZH.Attributes["showkjdy"] = dataTableQTZZ.Rows[0]["保函编号"].ToString();
            if (dataTableQTZZ.Rows[0]["履约争议证明文件路径"].ToString() != "")
            {
                this.linkLVZMWJ.Attributes["showtupian"] = dataTableQTZZ.Rows[0]["履约争议证明文件路径"].ToString().Replace(@"\", "/");
                this.linkLVZMWJ.Visible = true;
            }
            else
            {
                this.linkLVZMWJ.Visible = false;
            }

        }
        else
        {
            this.linkBZH.Visible = false;
            this.linkLVZMWJ.Visible = false;


        }

        //提货单与发货单信息
        if (dataTableTHYFHXX != null && dataTableTHYFHXX.Rows.Count > 0)
        {
            thyfhxxtxt.InnerHtml = "已提货数量：" + dataTableTHYFHXX.Rows[0]["已提货数量"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;" + "已提货金额：" + dataTableTHYFHXX.Rows[0]["已提货金额"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;" + "已发货数量：" + dataTableTHYFHXX.Rows[0]["已发货数量"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;" + "无异议收货数量： " + dataTableTHYFHXX.Rows[0]["无异议收货数量"].ToString();
            this.dataGridView3.AutoGenerateColumns = false;
            this.dataGridView3.DataSource = dataTableTHYFHXX.DefaultView;
            this.dataGridView3.DataBind();
        }


    }
}