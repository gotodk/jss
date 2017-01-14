using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Web.UI;
using System.Data;
using FMOP.DB;

/// <summary>
///ClassQX 的摘要说明
/// </summary>
public class ClassQX
{
	public ClassQX()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    static public Hashtable QX()
    {
        Hashtable ht = new Hashtable();
        ht["旧硒鼓业务"] = 1;
        ht["卖家超市"] = 2;
        ht["买家超市"] = 4;
        ht["景点管理部门"] = 8;
        ht["景点内商铺"] = 16;
        ht["农产品"] = 32;
        ht["社区管理部门"] = 64;
        ht["社区内商铺"] = 128;
        ht["工会管理部门"] = 256;
        ht["工会会员"] = 512;
        ht["xxx"] = 1024;
        ht["bbbb"] = 2048;
        return ht;
    }

    //后台未登陆，强制跳转到登陆页面 --2012-04 于海滨
    //考虑到方法已经添加到各个页面，现在该方法中添加权限验证方法  --2012-05-09  郭拓
    /// <summary>
    /// 验证用户是否登录、是否有足够的权限
    /// </summary>
    /// <param name="p">值为this即可</param>
    static public void Check_Login(Page p)
    {
        string a = CookieClass.GetCookie("FMCookie_DLZH");
        string b = CookieClass.GetCookie("FMCookie_KHBH");
        string c = CookieClass.GetCookie("FMCookie_DLZHLX");
        if (a == null || a == "" || b == null || b == "" || c == null || c == "")
        {
            //强制跳转
            HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/services/MMweb/yh_Login.aspx");
            HttpContext.Current.Response.End();
        }
        else
        {
            #region  ----
            //已经登录，则验证权限
            string KHBH = CookieClass.GetCookie("FMCookie_KHBH");
            string URL = HttpContext.Current.Request.Url.ToString(); //当前登录的url，用来判断是那个权限
            if ((URL.IndexOf("a0") > -1) && (!(URL.IndexOf("HT_SubInfo.aspx") > -1)))
            {
                string MMweb = "MMweb";
                int i = URL.IndexOf(MMweb);//MMweb的M在URL中的索引
                string URLSub = URL.Substring(i, URL.Length - i);
                string URLsubsub = URLSub.Substring(0, 11);
                string newUrl = URLsubsub.Replace("MMweb/a", "");   //得到0002
                int QX = Convert.ToInt32(newUrl);   //得到2 
                string QXName = "";     //得到2对应的权限名称
                switch (QX)
                {
                    case 2:
                        QXName = "卖家超市";
                        break;
                    case 4:
                        QXName = "买家超市";
                        break;
                    case 8:
                        QXName = "景点管理部门";
                        break;
                    case 16:
                        QXName = "景点内商铺";
                        break;
                    case 32:
                        QXName = "农产品";
                        break;
                    case 64:
                        QXName = "社区管理部门";
                        break;
                    case 128:
                        QXName = "社区内商铺";
                        break;
                    case 256:
                        QXName = "工会管理部门";
                        break;
                    case 512:
                        QXName = "工会会员";
                        break;
                }
               
                bool IsHasQX = ClassQX.IsHasQX(KHBH, QX); //判断用户是否开通了该权限
                bool IsExamQX = ClassQX.IsExamQX(KHBH, QX); //判断用户是否正在审核中
                bool IsCheckedAndSubmitQX = ClassQX.IsCheckedAndSubmitQX(KHBH, QX); //判断用户是否开通了但未填写资料
                bool IsCheckedQX = ClassQX.IsCheckedQX(KHBH, QX);  //判断用户是否勾选了该权限
                /*这四个方法，四个bool值是权限递减的，也是用户操作过程中从后往前的。*/
                if (IsHasQX)
                {
                    //如果用户开通了该项权限，则不执行任何操作。
                }
                else
                {
                    string SelectSHJG = "SELECT * FROM MM_JSKTSQSHB WHERE SQRKHBH = '" + KHBH + "' AND SQJSLX ='" + QXName + "'";
                    DataSet ds = DbHelperSQL.Query(SelectSHJG);
                    DataTable dt = ds.Tables[0];
                    string SHJG = "未审核";
                    string SHYJ = "";
                    if (dt.Rows.Count > 0)
                    {
                        SHJG = dt.Rows[0]["SHJG"].ToString();
                        SHYJ = dt.Rows[0]["SHYJ"].ToString();
                    }
                    /*else表示用户没有开通该项权限。
                      此时会发生的情况有：
                      该权限正在审核中
                      该项权限被驳回
                      该权限已经勾选，但没有提交资料
                      用户没有勾选该权限
                     */
                    //是不是正在审核,string SelectSHJG = "SELECT * FROM MM_JSKTSQSHB WHERE SQRKHBH = '" + KHBH + "'";
                    if (IsExamQX)
                    {
                        string myScript = "art.dialog({content: '您的该项权限正在审核中，暂时<br>不能使用此功能，请耐心等待',icon: 'warning', lock: true,button: [{name: '确定',callback: function () {window.location.href='/services/MMweb/yh_QXManager.aspx';},focus: true}]});";
                        p.ClientScript.RegisterStartupScript(p.GetType(), "MyScript", myScript, true);
                    }
                    else if (SHJG == "驳回")
                    {
                        string myScript = "art.dialog({content: '您的该项权限审核被驳回，<br>驳回原因是:<br>" + SHYJ + "',icon: 'warning', lock: true,button: [{name: '确定',callback: function () {window.location.href='/services/MMweb/yh_QXManager.aspx';},focus: true}]});";
                        p.ClientScript.RegisterStartupScript(p.GetType(), "MyScript", myScript, true);
                    }
                    else
                    {
                        /*else表示用户没有开通该项权限。
                          此时会发生的情况有：
                          该权限已经勾选，但没有提交资料
                          用户没有勾选该权限
                         */
                        //该权限已经勾选，但没有提交资料
                        if (IsCheckedAndSubmitQX)
                        {
                            string myScript = "art.dialog({content: '您尚未完善该权限页面的资料，将前往完善资料页面',icon: 'warning', lock: true,button: [{name: '确定',callback: function () {window.location.href='/services/MMweb/yh_QXManager.aspx';},focus: true}]});";
                            p.ClientScript.RegisterStartupScript(p.GetType(), "MyScript", myScript, true);
                        }
                        else
                        {
                            /*else表示用户没有开通该项权限。
                              此时会发生的情况有：
                              用户没有勾选该权限
                             */
                            //该权限有没有被勾选
                            if (!IsCheckedQX)
                            {
                                string myScript = "art.dialog({content: '您尚选择开通该项权限，将前往权限概览页面',icon: 'warning', lock: true,button: [{name: '确定',callback: function () {window.location.href='/services/MMweb/yh_QXManager.aspx';},focus: true}]});";
                                p.ClientScript.RegisterStartupScript(p.GetType(), "MyScript", myScript, true);
                            }
                        }
                    }
                }
            }
            #endregion
        }
    }


    /// <summary>
    /// 判断用户是否已经开通某种权限
    /// </summary>
    /// <param name="KHBH">用户客户编号</param>
    /// <param name="IntQX">权限值</param>
    /// <returns>True or false</returns>
    public static bool IsHasQX(string KHBH, int IntQX)
    {
        string SelectSql = "SELECT * FROM MM_YHZCDLXXB WHERE KHBH = '" + KHBH + "'";
        DataSet ds = DbHelperSQL.Query(SelectSql);
        DataTable dt = ds.Tables[0];
        bool IsHasQX = false;
        if (dt.Rows.Count > 0)
        {
            string YKTQX = dt.Rows[0]["YKTQX"].ToString(); //获取当前用户的已开通权限
            int IntYKTQX = Convert.ToInt32(YKTQX);
            if ((IntYKTQX & IntQX) != IntQX)
            {
                IsHasQX = false;
            }
            if ((IntYKTQX & IntQX) == IntQX)
            {
                IsHasQX = true;
            }
        }
        return IsHasQX;
    }

    /// <summary>
    /// 判断某种权限是不是正在审核中
    /// </summary>
    /// <param name="KHBH">用户客户编号</param>
    /// <param name="IntQX">权限值</param>
    /// <returns>True or false</returns>
    public static bool IsExamQX(string KHBH, int IntQX)
    {
        string SelectSql = "SELECT * FROM MM_YHZCDLXXB WHERE KHBH = '" + KHBH + "'";
        DataSet ds = DbHelperSQL.Query(SelectSql);
        DataTable dt = ds.Tables[0];
        bool IsExamQX = false;
        if (dt.Rows.Count > 0)
        {
            string ZZSHZDQX = dt.Rows[0]["ZZSHZDQX"].ToString(); //获取当前用户的已开通权限
            int IntZZSHZDQX = Convert.ToInt32(ZZSHZDQX);
            if ((IntZZSHZDQX & IntQX) != IntQX)
            {
                IsExamQX = false;
            }
            if ((IntZZSHZDQX & IntQX) == IntQX)
            {
                IsExamQX = true;
            }
        }
        return IsExamQX;
    }

    /// <summary>
    /// 判断用户该权限是否已勾选但未填写资料
    /// </summary>
    /// <param name="KHBH">用户客户编号</param>
    /// <param name="IntQX">权限值</param>
    /// <returns>True or false</returns>
    public static bool IsCheckedAndSubmitQX(string KHBH, int IntQX)
    {

        string SelectSql = "SELECT * FROM MM_YHZCDLXXB WHERE KHBH = '" + KHBH + "'";
        DataSet ds = DbHelperSQL.Query(SelectSql);
        DataTable dt = ds.Tables[0];
        bool IsCheckAndNotSub = false;  //是不是选择开通了，但是没填写资料
        if (dt.Rows.Count > 0)
        {
            string NKTQX = dt.Rows[0]["NKTQX"].ToString(); //获取当前用户的拟开通权限
            int IntNKTQX = Convert.ToInt32(NKTQX);  //拟开通权限Int
            string YKTQX = dt.Rows[0]["YKTQX"].ToString();//获取当前用户的已开通权限
            int IntYKTQX = Convert.ToInt32(YKTQX);  //已开通权限Int
            string ZZSHZDQX = dt.Rows[0]["ZZSHZDQX"].ToString(); //获取当前用户正在审核中的权限
            int IntZZSHZDQX = Convert.ToInt32(ZZSHZDQX); //正在审核中的权限Int
            if (IntZZSHZDQX == 0)
            {
                if (((IntNKTQX - IntYKTQX) & IntQX) == IntQX)
                {
                    IsCheckAndNotSub = true; //表示这种权限已经勾选，但没有填写资料
                }
            }
            if (IntZZSHZDQX != 0)
            {
                if (((IntNKTQX - IntYKTQX - IntZZSHZDQX) & IntQX) == IntQX)
                {
                    IsCheckAndNotSub = true;//表示这种权限已经勾选，但没有填写资料
                }
            }
        }
        return IsCheckAndNotSub;
    }

    /// <summary>
    /// 判断用户是否已经勾选该权限
    /// </summary>
    /// <param name="KHBH">用户客户编号</param>
    /// <param name="IntQX">权限值</param>
    /// <returns>True or false</returns>
    public static bool IsCheckedQX(string KHBH, int IntQX)
    {
        string SelectSql = "SELECT * FROM MM_YHZCDLXXB WHERE KHBH = '" + KHBH + "'";
        DataSet ds = DbHelperSQL.Query(SelectSql);
        DataTable dt = ds.Tables[0];
        bool IsCheckQX = false;
        if (dt.Rows.Count > 0)
        {
            string NKTQX = dt.Rows[0]["NKTQX"].ToString(); //获取当前用户的已开通权限
            int IntNKTQX = Convert.ToInt32(NKTQX);
            if ((IntNKTQX & IntQX) != IntQX)
            {
                IsCheckQX = false;
            }
            if ((IntNKTQX & IntQX) == IntQX)
            {
                IsCheckQX = true;
            }
        }
        return IsCheckQX;
    }

}