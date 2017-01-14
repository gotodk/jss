using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class pingtaiservices_moban_FMPTDZGHHT : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
            if (Request.Params["Number"] != null && Request.Params["Number"].ToString() != "")
            {
             //DataTable dataTable= DbHelperSQL.Query("select  ZZ.Z_HTBH '合同编号',DL_Seller.I_JYFMC '卖家交易方名称',DL_Buyer.I_JYFMC '买家交易方名称',DL_Seller.B_DLYX '卖家交易方账户',DL_Buyer.B_DLYX '买家交易方账户',DL_Seller.I_YYZZZCH '卖家营业执照注册号',DL_Buyer.I_YYZZZCH '买家营业执照注册号',DL_Buyer.I_SFZH '买家身份账号',DL_Buyer.I_ZCLB '买家注册类别',DL_Seller.I_XXDZ '卖家联系地址',DL_Buyer.I_XXDZ '买家联系地址',DL_Seller.I_LXRXM '卖家联系人',DL_Buyer.I_LXRXM '买家联系人',DL_Seller.I_JYFLXDH '卖家联系电话',DL_Buyer.I_JYFLXDH '买家联系电话',CONVERT(varchar(10),Z_ZBSJ,120) '合同签订时间',ZZ.Z_SPBH '商品编号',ZZ.Z_SPMC '商品名称',ZZ.Z_GG '规格',ZZ.Z_ZBJG '价格',ZZ.Z_ZBSL '数量',ZZ.Z_JJDW '计价单位',ZZ.Z_ZBJE '金额',ZZ.Z_GG '商品标准',convert(varchar(10),ZZ.Z_DBSJ,120) '合同开始时间',convert(varchar(10),(case ZZ.Z_HTQX when '三个月' then dateadd(DD,90,ZZ.Z_DBSJ) when '一年' then dateadd(DD,365,ZZ.Z_DBSJ) end),120) '合同结束时间',ZZ.Z_HTQX '合同期限' from AAA_ZBDBXXB as ZZ left join AAA_DLZHXXB as DL_Seller on ZZ.T_YSTBDMJJSBH=DL_Seller.J_SELJSBH left join AAA_DLZHXXB as DL_Buyer on ZZ.Y_YSYDDMJJSBH=DL_Buyer.J_BUYJSBH left join AAA_PTSPXXB as SP on ZZ.Z_SPBH=SP.SPBH   where ZZ.Number='" + Request.Params["Number"].ToString() + "'").Tables[0];
                DataTable dataTable = DbHelperSQL.Query("select  ZZ.Z_HTBH '合同编号','BH'+ZZ.Number '保证函编号',DL_Seller.I_JYFMC '卖家交易方名称',DL_Buyer.I_JYFMC '买家交易方名称',isnull(DL_Seller.B_ZHDQXYFZ,0) '卖家信用分值',isnull(DL_Buyer.B_ZHDQXYFZ,0) '买家信用分值',DL_Seller.B_DLYX '卖家交易方账户',DL_Buyer.B_DLYX '买家交易方账户',DL_Seller.I_YYZZZCH '卖家营业执照注册号',DL_Buyer.I_YYZZZCH '买家营业执照注册号',DL_Buyer.I_SFZH '买家身份账号',DL_Buyer.I_ZCLB '买家注册类别',DL_Seller.I_XXDZ '卖家联系地址',DL_Buyer.I_XXDZ '买家联系地址',DL_Seller.I_LXRXM '卖家联系人',DL_Buyer.I_LXRXM '买家联系人',DL_Seller.I_JYFLXDH '卖家联系电话',DL_Buyer.I_JYFLXDH '买家联系电话',CONVERT(varchar(10),Z_ZBSJ,120) '合同签订时间',ZZ.Z_SPBH '商品编号',ZZ.Z_SPMC '商品名称',ZZ.Z_GG '规格',ZZ.Z_ZBJG '价格',ZZ.Z_ZBSL '数量',ZZ.Z_JJDW '计价单位',ZZ.Z_ZBJE '金额',ZZ.Z_LYBZJJE '履约保证金',ZZ.Z_GG '商品标准',convert(varchar(10),ZZ.Z_DBSJ,120) '合同开始时间',convert(varchar(10),ZZ.Z_HTJSRQ,120) '合同结束时间',ZZ.Z_HTQX '合同期限' from AAA_ZBDBXXB as ZZ left join AAA_DLZHXXB as DL_Seller on ZZ.T_YSTBDMJJSBH=DL_Seller.J_SELJSBH left join AAA_DLZHXXB as DL_Buyer on ZZ.Y_YSYDDMJJSBH=DL_Buyer.J_BUYJSBH left join AAA_PTSPXXB as SP on ZZ.Z_SPBH=SP.SPBH   where ZZ.Number='" + Request.Params["Number"].ToString() + "'").Tables[0];
             string oldStr = "";
             string newStr = "";

             string strDYHM = "";
             if (dataTable.Rows[0]["买家注册类别"].ToString()=="单位")
             { 
             strDYHM=dataTable.Rows[0]["买家营业执照注册号"].ToString();
             }
             else 
             {
                 strDYHM = dataTable.Rows[0]["买家身份账号"].ToString();
             }

             DateTime dataTimeHTQD = Convert.ToDateTime(dataTable.Rows[0]["合同签订时间"]);
             DateTime dataTimeHTStart = Convert.ToDateTime(dataTable.Rows[0]["合同开始时间"]);
             DateTime dataTimeHTEnd = Convert.ToDateTime(dataTable.Rows[0]["合同结束时间"]);
             string strCS = "";
             if (dataTable.Rows[0]["合同期限"].ToString() == "三个月")
             {
                 strCS = "合同数量/90*120%";
             }
             else if (dataTable.Rows[0]["合同期限"].ToString() == "一年")
             {
                 strCS = "合同数量/365*120%";
             }
             else
             {
                 strCS = "一次性供货";
             }


             oldStr = BuildHtm(HttpContext.Current.Server.MapPath("FMPTDZGHHT.htm"));
             string strImage = "<img src='FMPTDZGHHT.files/xinxing.png' width='20px' height='17px' style='margin-top:2px;' />";


             newStr = oldStr.Replace("HT_BH", dataTable.Rows[0]["合同编号"].ToString()).Replace("HT_Seller_MC", dataTable.Rows[0]["卖家交易方名称"].ToString()).Replace("HT_Buyer_MC", dataTable.Rows[0]["买家交易方名称"].ToString()).Replace("HT_Seller_JYFZH", dataTable.Rows[0]["卖家交易方账户"].ToString()).Replace("HT_Buyer_JYFZH", dataTable.Rows[0]["买家交易方账户"].ToString()).Replace("HT_Seller_YYZZZCH", dataTable.Rows[0]["卖家营业执照注册号"].ToString()).Replace("HT_Buyer_DYHM", strDYHM).Replace("HT_Seller_DZ", dataTable.Rows[0]["卖家联系地址"].ToString()).Replace("HT_Buyer_DZ", dataTable.Rows[0]["买家联系地址"].ToString()).Replace("HT_Seller_LXR", dataTable.Rows[0]["卖家联系人"].ToString()).Replace("HT_Buyer_LXR", dataTable.Rows[0]["买家联系人"].ToString()).Replace("HT_Seller_LXDH", dataTable.Rows[0]["卖家联系电话"].ToString()).Replace("HT_Buyer_LXDH", dataTable.Rows[0]["买家联系电话"].ToString()).Replace("HT_NF", dataTimeHTQD.Year.ToString()).Replace("HT_YF", dataTimeHTQD.Month.ToString()).Replace("HT_DD", dataTimeHTQD.Day.ToString()).Replace("HT_Seller_JYFMC", dataTable.Rows[0]["卖家交易方名称"].ToString()).Replace("HT_Buyer_JYFMC", dataTable.Rows[0]["买家交易方名称"].ToString()).Replace("HT_SPBH", dataTable.Rows[0]["商品编号"].ToString()).Replace("HT_SPMC", dataTable.Rows[0]["商品名称"].ToString()).Replace("HT_GG", dataTable.Rows[0]["规格"].ToString()).Replace("HT_JG", dataTable.Rows[0]["价格"].ToString()).Replace("HT_SL", dataTable.Rows[0]["数量"].ToString()).Replace("HT_JJDW", dataTable.Rows[0]["计价单位"].ToString()).Replace("HT_JE", dataTable.Rows[0]["金额"].ToString()).Replace("HT_ZLBZ", dataTable.Rows[0]["商品标准"].ToString()).Replace("YYYY_Start", dataTimeHTStart.Year.ToString()).Replace("MM_Start", dataTimeHTStart.Month.ToString()).Replace("dd_Start", dataTimeHTStart.Day.ToString()).Replace("YYYY_End", dataTimeHTEnd.Year.ToString()).Replace("MM_End", dataTimeHTEnd.Month.ToString()).Replace("dd_End", dataTimeHTEnd.Day.ToString()).Replace("HT_RQCS", strCS).Replace("BZH_BH", dataTable.Rows[0]["保证函编号"].ToString()).Replace("LY_BZJ", dataTable.Rows[0]["履约保证金"].ToString());

                //设置交易双方的信用
             double SellerXY = Convert.ToDouble(dataTable.Rows[0]["卖家信用分值"]);//卖家信用分数
             double BuyerXY = Convert.ToDouble(dataTable.Rows[0]["买家信用分值"]);//买家信用分数
                 //根据分数得到图片资源
            string[] strSellerArray= JYFXYMX.GetXYImages(SellerXY);
            string[] strBuyerArray = JYFXYMX.GetXYImages(BuyerXY);
            if (strSellerArray == null)
            {
                newStr = newStr.Replace("SelerXY", "");
            }
            else
            {
                newStr = newStr.Replace("SelerXY", string.Join("", strSellerArray));
            }
            if (strBuyerArray == null)
            {
                newStr = newStr.Replace("BuyerXY", "");
            }
            else
            {
                newStr = newStr.Replace("BuyerXY", string.Join("", strBuyerArray));
            }
           


             Response.Write(newStr);
             Response.End();


            
            
            
            
            }





        
    }

    /// <summary>
    /// 根据模板读取数据库内容，无需创建其他列表，直接创建html
    /// </summary>
    /// <param name="strTmplPath">网页模板文件的路径</param>
    public string BuildHtm(string strTmplPath)
    {
        //取模板文件的内容
        System.Text.Encoding code = System.Text.Encoding.GetEncoding("gb2312");
        StreamReader sr = null;
        string str = "";
        try
        {
            sr = new StreamReader(strTmplPath, code);
            str = sr.ReadToEnd(); // 读取文件
            sr.Close();
        }
        catch (Exception exp)
        {
            return exp.ToString();
            sr.Close();
        }
        //string htmlfilename = this.GetFileSaveName(strEnName);//通过英文名获取保存后的文件名
        //替换变量标签

        //string strNew=str.Replace("[$nameChs$]", strChsName);
        //string str = str;
        //strNew = strNew.Replace("[$areaContect$]", "新的文本");//替换左侧页面导航
        return str;
    }
}


/// <summary>
/// 交易方信用明细
/// </summary>
public static class JYFXYMX
{

    /// <summary>
    /// 用户信用等级图片
    /// </summary>
    /// <param name="userScore"></param>
    /// <returns></returns>
    public static string[] GetXYImages(double userScore)
    {
        if (userScore >= 1 && userScore < 4)//1-3分  “一心”
        {
            return new string[] { UserYXImage.XinXing };
        }
        else if (userScore >= 4 && userScore < 7)//4-6分  “二心”
        {
            return new string[] { UserYXImage.XinXing, UserYXImage.XinXing };
        }
        else if (userScore >= 7 && userScore < 10)//7-9分  “三心”
        {
            return new string[] { UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing };
        }
        else if (userScore >= 10 && userScore < 13)//10-12分  “四心”
        {
            return new string[] { UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing };
        }
        else if (userScore >= 13 && userScore < 16)//13-15分  “五心”
        {
            return new string[] { UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing };
        }
        else if (userScore >= 16 && userScore < 21)//13-15分  “一钻”
        {
            return new string[] { UserYXImage.ZuanXing };
        }
        else if (userScore >= 21 && userScore < 26)//21-25分  “二钻”
        {
            return new string[] { UserYXImage.ZuanXing, UserYXImage.ZuanXing };
        }
        else if (userScore >= 26 && userScore < 31)//26-30分  “三钻”
        {
            return new string[] { UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing };
        }
        else if (userScore >= 31 && userScore < 36)//31-25分  “四钻”
        {
            return new string[] { UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing };
        }
        else if (userScore >= 36 && userScore < 41)//36-40分  “五钻”
        {
            return new string[] { UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing };
        }
        else if (userScore >= 41 && userScore < 46)//41-45分  “一皇冠”
        {
            return new string[] { UserYXImage.HuanGuanXing };
        }
        else if (userScore >= 46 && userScore < 51)//46-50分  “二皇冠”
        {
            return new string[] { UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing };
        }
        else if (userScore >= 51 && userScore < 56)//46-50分  “三皇冠”
        {
            return new string[] { UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing };
        }
        else if (userScore >= 56 && userScore < 61)//56-60分  “四皇冠”
        {
            return new string[] { UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing };
        }
        else if (userScore >= 61 && userScore < 100)//61-99分  “五皇冠”
        {
            return new string[] { UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing };
        }
        else if (userScore >= 100) //100分以上 “VIP”
        {
            return new string[] { UserYXImage.VipXing };

        }
        return null;
    }




}


public static class UserYXImage
{

    /// <summary>
    /// 返回“心”形状图片
    /// </summary>
    public static string XinXing
    {
        get
        {
            return "<img src='FMPTDZGHHT.files/xinxing.png' width='16px' height='13px' style='margin-top:2px;' />";
        }
    }

    /// <summary>
    /// 返回“钻”形状图片
    /// </summary>
    public static string ZuanXing
    {

        get
        {
            return "<img src='FMPTDZGHHT.files/zuanxing.png' width='16px' height='13px' style='margin-top:2px;' />";
        
        }
    }

    /// <summary>
    /// 返回“皇冠”形状图片
    /// </summary>
    public static string HuanGuanXing
    {
        get
        {
            return "<img src='FMPTDZGHHT.files/huangguanxing.png' width='16px' height='13px' style='margin-top:2px;' />";
        }

    }

    /// <summary>
    /// 返回“VIP”形状图片
    /// </summary>
    public static string VipXing
    {
        get
        {
            return "<img src='FMPTDZGHHT.files/vipxing.png' width='16px' height='13px' style='margin-top:2px;' />";
        }

    }



}
