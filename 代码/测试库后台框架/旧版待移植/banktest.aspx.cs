using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class banktest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //真实环境调用测试：平安

        BankPA.Service bs = new BankPA.Service();
        DataSet ds = new DataSet();
        ds.ReadXml(Server.MapPath(@"~/App_Code/Bank/XMLPingan/Us_Send_1330.xml"));
        ds.Tables[0].Rows[0]["FuncFlag"] = "1";
        ds.Tables[0].Rows[0]["TxDate"] = DateTime.Now.ToString("yyyyMMdd");
        ds.Tables[0].Rows[0]["Reserve"] = "发送测试";
        DataSet result = bs.Reincarnation(ds);
        
        this.GridView1.DataSource = result.Tables["结果"];
        this.GridView1.DataBind();
        this.GridView2.DataSource = result.Tables["包头"];
        this.GridView3.DataSource = result.Tables["包体"];
        this.GridView2.DataBind();
        this.GridView3.DataBind();


        //string invokeState = string.Empty;

        //IBank IB = new IBank("j1@j1.com", "出金");
        //DataSet ds = new DataSet();
        //DataSet dsResult = IB.Invoke(ds, ref invokeState);

        //this.GridView1.DataSource = dsResult;
        //this.GridView1.DataBind();

        ////guotuo@hotmail.com  n1@n1.com
        //IBank IB2 = new IBank("guotuo@hotmail.com", "出金");
        //DataSet ds2 = new DataSet();
        //DataSet dsResult2 = IB2.Invoke(ds2, ref invokeState);
        //if (dsResult2 == null)
        //    Response.Write(invokeState);

        //this.GridView2.DataSource = dsResult2;
        //this.GridView2.DataBind();

        //string xx = Helper.Convert("变更银行账户").ToLower();
    
        //    xx = xx.Replace("yinxing", "yinhang");
        //string yy = Helper.Convert("chujin").ToLower();
        //string zz = Helper.Convert("CHUJIN").ToLower();
        //string zz01 = Helper.Convert("GetKey").ToLower();
        //Response.Write(xx + "|" + yy + "|" + zz + "|" + zz01);

        ////DataSet ds = new DataSet("参数表");

        //string ssss= "";
        //DataTable Configs = BankHelper.GetConfig("浦发银行");
        //this.GridView3.DataSource = Configs;
        //this.GridView3.DataBind();
        //Response.Write(ssss);

        ////第一种调用方式
        //BankRoute bk = new BankRoute("action@action.com");
        //string bankTypes = bk.GetUserBank();
        //Assembly asm = Assembly.GetExecutingAssembly();
        //Type tp = asm.GetType(bankTypes, false);
        //object instance = asm.CreateInstance(bankTypes, false);
        //MethodInfo method = tp.GetMethod("出金");
        //object[] _params = { ds };
        //DataSet result = (DataSet)method.Invoke(instance, _params);

        ////第二种调用方式
        //IBank bank = new IBank("action@action.com", "出金");
        //string info = string.Empty;
        //bank.Invoke(ds, ref info);
    }
}