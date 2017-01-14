using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.InteropServices;
using VBSDK;
using Hesion.Brick.Core;
using FMOP.DB;
using ShareLiu;
using System.Collections;
/// <summary>
///开发者：刘杰 2010-07-08
///实现短信发送的类，需要注册后使用。
///调用vbsdk.dll开发
/// </summary>
namespace ShareLiu.DXS
{

    public class SendDX
    {
        //如果是1，就代表使用新的，如果使用0代表使用旧的。

        public int oldNew = 1;
        public int jCount = 200;
        public ASPCom acp;
        public ASPCOMMLib.EUCPCom yme;
        private string sn = "SDK-CSL-010-00004"; //授权ID
        private string pass = "016189"; //授权密码
        //private string sn2 = "0SDK-EMY-0130-LBXRT";//亿美授权KEYtest
       // private string pass2 = "021368";//亿美授权密码test
        private string sn2 = "3SDK-EMY-0130-LFRPO";//亿美授权KEY
        private string pass2 = "951016";//亿美授权密码
        public SendDX()
        {
            if (oldNew == 0)
            {
                acp = new ASPCom();

            }
            else
            {
                yme = new ASPCOMMLib.EUCPCom();
            }
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 发送短信给手机
        /// </summary>
        /// <param name="mobileID">手机号</param>
        /// <param name="sendMess">发送信息</param>
        /// <returns>成功true,失败false</returns>
        public bool SendToM(string mobileID, string sendMess)
        {
            int j = 0;
            if (mobileID == "0" || mobileID == null || mobileID == "")
            {
                return false;
            }
            if (sendMess == "" || sendMess == null)
            {
                return false;
            }

            if (oldNew == 0) //老的
            {

                try
                {
                    ArrayList al = null;
                    
                    al = CheckALL.CLString(sendMess, jCount);
                    for (int i = 0; i < al.Count; i++)
                    {
                        if (acp.SendSMS(sn, mobileID, al[i].ToString()) == true)
                        {
                            j = j + 1;
                        }

                    }
                    if (j == al.Count)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception e1)
                {
                    return false;
                }
            }
            else
            {

                try
                {
                    ArrayList al = null;
                    sendMess +="【富美】";
                    al = CheckALL.CLString(sendMess, jCount);
                    for (int i = 0; i < al.Count; i++)
                    {
                        if (yme.get_SendSMS(sn2, mobileID, al[i].ToString(), "5") == 1)
                        {
                            try
                            {
                                //先不管成不成，但不阻挡数据
                                int rr = DbHelperSQL.ExecuteSql("insert into DXCOUNTINFO(sendprice) values('" + GetDJ() + "')");
                            }
                            catch (System.Exception e)
                            {

                            }
                            j = j + 1;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    if (j == al.Count)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                }
                catch (Exception e1)
                {
                    return false;
                }

            }

        }
        /// <summary>
        /// 直接测试发送 2012-02-10
        /// </summary>
        /// <param name="mobileID">手机号码</param>
        /// <returns></returns>
        public bool TestSendToM(string mobileID)
        {
            int j = 0;
            if (mobileID == "0" || mobileID == null || mobileID == "")
            {
                return false;
            }  
            try
            {
            
                if(yme.get_SendSMS(sn2, mobileID,"直接点击测试【富美】", "5") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
                           

        }
        /// <summary>
        /// 根据业务平台用户发送短信给手机
        /// </summary>
        /// <param name="pID">用户工号</param>
        /// <param name="SendMess">发送信息</param>
        /// <returns>成功true,失败false</returns>
        public bool SendToP(string pID, string SendMess)
        {
            string mobileID = "0";
            try
            {
                System.Data.SqlClient.SqlDataReader sr = DbHelperSQL.ExecuteReader("select GRDH from HR_Employees where Number='" + pID.Trim() + "' and YGZT<>'离职' ");
                if (sr.Read())
                {
                    mobileID = sr["GRDH"].ToString();
                    if (mobileID == "0" || mobileID == null || mobileID == "")
                    {
                        return false;  //发送不成功
                    }
                    else
                    {

                        return SendToM(mobileID, SendMess);  //正式发送



                    }

                }
                else
                {

                    return false;  //没有员工记录，返回空
                }

            }
            catch (Exception e2)
            {
                return false;
            }
        }
        /// <summary>
        /// 得到余额
        /// </summary>
        /// <returns></returns>
        public string GetBalance()
        {
            if (oldNew == 0)
            {
                return acp.QueryBalance(sn);
            }
            else
            {
                try
                {
                    object s;
                    int r = yme.get_GetBalance(sn2, out s);
                    return s.ToString();
                }
                catch (System.Exception e)
                {
                    return "0";
                }

            }
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <returns>成功true失败false</returns>
        public bool ZC()
        {
            if (oldNew == 0)
            {
                return acp.Register(sn, pass, "山东", "济南市", "生产业", "富美", "张三", "400-688-8844", "", "Ho@sina.com", "028-86868573", "成都", "100070");
            }
            else
            {
                try
                {
                    int r = yme.get_Register(sn2, pass2, "富美科技", "张三", "0531-87237017", "1", "1", "1", "1", "1");
                    if (r == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;

                    }
                }
                catch (System.Exception e)
                {
                    return false;
                }


            }
        }
        /// <summary>
        /// 注销
        /// </summary>
        /// <returns>是否成功</returns>
        public bool ZX()
        {
            if (oldNew == 0)
            {


                return acp.UnRegister(sn);
            }
            else
            {
                try
                {
                    int r = yme.get_UnRegister(sn2);
                    if (r == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;

                    }
                }
                catch (System.Exception e)
                {
                    return false;
                }


            }
        }
        /// <summary>
        /// 得到单价，只适用于亿美控件
        /// </summary>
        /// <returns></returns>
        public string GetDJ()
        {
            if (oldNew == 0)
            {
                return "0";
            }
            else
            {
                try
                {
                    object s;
                    int r = yme.get_GetPrice(sn2, out s);
                    return s.ToString();
                }
                catch (System.Exception e)
                {
                    return "0";
                }

            }

        }
        /// <summary>
        /// 测试注册返回值（亿)
        /// </summary>
        /// <returns></returns>
        //public int testZC()
        //{
        //   int aa=yme.get_Register(sn2, pass2, "富美科技", "李", "0531-87237010", "400-688-8844", "", "", "济南", "250000");
        //   return aa;
        //}

    }
}
