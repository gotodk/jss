using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;

namespace 客户端主程序.DataControl
{
    /// <summary>
    /// 远程调用对应类库
    /// </summary>
    class WebServicesCenter
    {
        /// <summary>
        /// 配置文件webservice动态类名称
        /// </summary>
        string sjk;
        /// <summary>
        /// 程序集
        /// </summary>
        Assembly asm;
        /// <summary>
        /// 类型
        /// </summary>
        Type type;
        /// <summary>
        /// 激活器
        /// </summary>
        object instance;

        public WebServicesCenter()
        {
            //初始化动态配置
            sjk = DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[0];
            asm = Assembly.GetExecutingAssembly();
            type = asm.GetType(sjk);
            instance = asm.CreateInstance(sjk);
            //带参数列子return (DataSet)method.Invoke(instance, new object[] { "hello word!" } );
        }




        /// <summary>
        /// 测试获得一个分页数据
        /// </summary>
        /// <param name="HTwhere">条件参数哈希表</param>
        /// <returns></returns>
        public DataSet GetPagerDB(Hashtable HTwhere)
        {
            try
            {
                MethodInfo method = type.GetMethod("GetPagerDB");
                //处理默认值
                string GetCustomersDataPage_NAME = "";
                if (HTwhere.ContainsKey("GetCustomersDataPage_NAME"))
                {
                    GetCustomersDataPage_NAME = HTwhere["GetCustomersDataPage_NAME"].ToString();
                }
                string this_dblink = "";
                if (HTwhere.ContainsKey("this_dblink"))
                {
                    this_dblink = HTwhere["this_dblink"].ToString();
                }
                string page_index = "";
                if (HTwhere.ContainsKey("page_index"))
                {
                    page_index = HTwhere["page_index"].ToString();
                }
                string page_size = "";
                if (HTwhere.ContainsKey("page_size"))
                {
                    page_size = HTwhere["page_size"].ToString();
                }
                string serach_Row_str = "";
                if (HTwhere.ContainsKey("serach_Row_str"))
                {
                    serach_Row_str = HTwhere["serach_Row_str"].ToString();
                }
                string search_tbname = "";
                if (HTwhere.ContainsKey("search_tbname"))
                {
                    search_tbname = HTwhere["search_tbname"].ToString();
                }
                string search_mainid = "";
                if (HTwhere.ContainsKey("search_mainid"))
                {
                    search_mainid = HTwhere["search_mainid"].ToString();
                }
                string search_str_where = "";
                if (HTwhere.ContainsKey("search_str_where"))
                {
                    search_str_where = HTwhere["search_str_where"].ToString();
                }
                string search_paixu = "";
                if (HTwhere.ContainsKey("search_paixu"))
                {
                    search_paixu = HTwhere["search_paixu"].ToString();
                }
                string search_paixuZD = "";
                if (HTwhere.ContainsKey("search_paixuZD"))
                {
                    search_paixuZD = HTwhere["search_paixuZD"].ToString();
                }
                string count_float = "";
                if (HTwhere.ContainsKey("count_float"))
                {
                    count_float = HTwhere["count_float"].ToString();
                }
                string count_zd = "";
                if (HTwhere.ContainsKey("count_zd"))
                {
                    count_zd = HTwhere["count_zd"].ToString();
                }
                string cmd_descript = "";
                if (HTwhere.ContainsKey("cmd_descript"))
                {
                    cmd_descript = HTwhere["cmd_descript"].ToString();
                }
                DataSet ds =  (DataSet)method.Invoke(instance, new object[] { GetCustomersDataPage_NAME, this_dblink, page_index, page_size, serach_Row_str, search_tbname, search_mainid, search_str_where, search_paixu, search_paixuZD, count_float, count_zd, cmd_descript });
                return ds;
 
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }



        /// <summary>
        /// 验证登陆
        /// </summary>
        /// <returns></returns>
        public DataSet checkLoginIn(string user, string pass, string IP)
        {
            try
            {
                MethodInfo method = type.GetMethod("checkLoginIn");
                DataSet dsLogin = (DataSet)method.Invoke(instance, new object[] { user, pass, IP });
                return dsLogin;
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 激活帐户(注册第三步)
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public DataSet AccountID(string email,string valNum)
        {
            try
            {
                MethodInfo method = type.GetMethod("AccountID");
                DataSet ds = (DataSet)method.Invoke(instance, new object[] { email,valNum });
                return ds;
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用WebServices错误：" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 用户注册时再次发送验证码
        /// </summary>
        /// <param name="user">用户email</param>
        /// <param name="ValNum">验证码</param>
        /// <returns></returns>
        public DataSet reSendValNum(string user, string ValNum)
        {
            try
            {
                MethodInfo method = type.GetMethod("reSendValNum");
                DataSet ds = (DataSet)method.Invoke(instance, new object[] { user, ValNum });
                return ds;
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用WebServices错误：" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 再次发送短信验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="valNum"></param>
        /// <returns></returns>
        public DataSet reSendSMsg(string phone, string valNum)
        {
            try
            {
                MethodInfo method = type.GetMethod("reSendSMsg");
                DataSet ds = (DataSet)method.Invoke(instance, new object[] { phone, valNum });
                return ds;
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用WebServices错误：" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 验证是否验证码
        /// </summary>
        /// <param name="user">用户邮箱</param>
        /// <param name="typeToSet">重置密码方式</param>
        /// <returns></returns>
        public DataSet SendValNumber(string user, string typeToSet,string RandowNumber)
        {
            try
            {
                MethodInfo method = type.GetMethod("SendValNumber");
                DataSet dsResetPwd = (DataSet)method.Invoke(instance, new object[] { user, typeToSet, RandowNumber });
                return dsResetPwd;
            }
            catch(Exception ex)
            {
                Support.StringOP.WriteLog("调用WebServices错误：" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="uid">用户名(登录邮箱)</param>
        /// <param name="uname">昵称</param>
        /// <param name="pwd">密码</param>
        /// <param name="valnum">验证码</param>
        /// <returns></returns>
        public DataSet UserRegister(string uid, string uname, string pwd, string valnum)
        {
            try
            {
                MethodInfo method = type.GetMethod("UserRegister");
                DataSet dsReg = (DataSet)method.Invoke(instance, new object[] { uid, uname, pwd, valnum });
                return dsReg;
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用WebServices错误:" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 用户注册（修改邮箱（和用户名等））
        /// </summary>
        /// <param name="pk">主键</param>
        /// <param name="uid"></param>
        /// <param name="uname"></param>
        /// <param name="pwd"></param>
        /// <param name="valnum"></param>
        /// <returns></returns>
        public DataSet UserChangeRegister(string pk,string uid, string uname, string pwd, string valnum)
        {
            try
            {
                MethodInfo method = type.GetMethod("UserChangeRegister");
                DataSet dsReg = (DataSet)method.Invoke(instance, new object[] { pk, uid, uname, pwd, valnum });
                return dsReg;
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用WebServices错误:" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="uid">用户登录邮箱</param>
        /// <param name="types">邮箱或手机号码</param>
        /// <param name="pwd">重置后的密码</param>
        /// <param name="valNum">验证码</param>
        /// <param name="typeToSet">找回密码的方式</param>
        /// <returns>DataSet</returns>
        public DataSet ChangePassword(string uid,string types, string pwd, string valNum, string typeToSet)
        {
            try
            {
                MethodInfo method = type.GetMethod("ChangePassword");
                DataSet dsReSetPwd = (DataSet)method.Invoke(instance, new object[] { uid, types, pwd, valNum, typeToSet });
                return dsReSetPwd;
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用WebServices错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 可以提前获取的一些数据或参数
        /// </summary>
        /// <returns></returns>
        public DataSet GetPublisDsData()
        {
            try
            {
                MethodInfo method = type.GetMethod("GetPublisDsData");
                DataSet dsPublisDsData = (DataSet)method.Invoke(instance, null);
                return dsPublisDsData;
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// 测试获得一个字符串
        /// </summary>
        /// <returns></returns>
        public string test_str()
        {
            try
            {
                MethodInfo method = type.GetMethod("test_str");
                return (string)method.Invoke(instance, null);

            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }



        /// <summary>
        /// 测试获得一个多维数组，模拟首页列表显示
        /// </summary>
        /// <returns></returns>
        public string[][] indexTJSJ()
        {
            try
            {
                MethodInfo method = type.GetMethod("indexTJSJ");
                return (string[][])method.Invoke(instance, null);

            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// 测试获得一个数据集
        /// </summary>
        /// <returns></returns>
        public DataSet test_ds(string sp)
        {
            try
            {
                MethodInfo method = type.GetMethod("test_ds");
                return (DataSet)method.Invoke(instance, new object[] { sp });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// 测试获得一个分类数据集
        /// </summary>
        /// <returns></returns>
        public DataSet test_dsfl(string dbtablename)
        {
            try
            {
                MethodInfo method = type.GetMethod("test_dsfl");
                return (DataSet)method.Invoke(instance, new object[] { dbtablename });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 测试获得一个数据集
        /// </summary>
        /// <returns></returns>
        public DataSet test_moni()
        {
            try
            {
                MethodInfo method = type.GetMethod("test_moni");
                return (DataSet)method.Invoke(instance, null);
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 测试处理一个表单
        /// </summary>
        /// <returns></returns>
        public string test_save_or_update(string str1)
        {
            try
            {
                MethodInfo method = type.GetMethod("test_save_or_update");
                return (string)method.Invoke(instance, new object[] { str1 });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 删除服务器上的图片
        /// </summary>
        /// <param name="path"></param>
        public void DelPicture(string path)
        {
            try
            {

                MethodInfo method = type.GetMethod("DelPicture");
                method.Invoke(instance, new object[] { path });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());

                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
              
            }        
        }
        
        /// <summary>
        /// 数据更新
        /// </summary>
        /// <param name="array">对象名称</param>
        /// <param name="methodName">方法名称</param>
        /// <returns></returns>
        public string DataUpdate(object obj,string methodName)
        {
            try
            {
                
                MethodInfo method = type.GetMethod(methodName);
                return (string)method.Invoke(instance, new object[] { obj });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }        
        }

        /// <summary>
        /// 根据条件返回DataSet
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataSet GetDataSet(object obj)
        {
            try
            {
                MethodInfo method = type.GetMethod("GetDataSet");
                DataSet ds = (DataSet)method.Invoke(instance, new object[] { obj });
                return ds;
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 设置默认收货地址
        /// </summary>
        /// <returns></returns>
        public string Set_MrAddress(string number,string dlyx)
        {
            try
            {
                MethodInfo method = type.GetMethod("Set_MrAddress");
                return (string)method.Invoke(instance, new object[] { number,dlyx });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 删除收货地址
        /// </summary>
        /// <returns></returns>
        public string Del_Address(string number, string dlyx)
        {
            try
            {
                MethodInfo method = type.GetMethod("Del_Address");
                return (string)method.Invoke(instance, new object[] { number, dlyx });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 判断是否在营业时间内
        /// </summary>
        /// <returns></returns>
        public string IsOnOpenTime()
        {
            try
            {
                MethodInfo method = type.GetMethod("IsOnOpenTime");
                return (string)method.Invoke(instance,null);
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return "";
            }
        }
        /// <summary>
        /// 判断是否在营业日期内
        /// </summary>
        /// <returns></returns>
        public string IsOnOpenDay()
        {
            try
            {
                MethodInfo method = type.GetMethod("IsOnOpenDay");
                return (string)method.Invoke(instance,null);
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return "";
            }
        }

        /// <summary>
        /// 保存收货地址
        /// </summary>
        /// <returns></returns>
        public string[] Save_Address(string[][]  inputDT)
        {
            try
            {
                MethodInfo method = type.GetMethod("Save_Address");
                return (string[])method.Invoke(instance, new object[] { inputDT }); 
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 保存收货地址
        /// </summary>
        /// <returns></returns>
        public DataSet Get_EditAddress(string number,string dlyx)
        {
            try
            {
                MethodInfo method = type.GetMethod("Get_EditAddress");
                return (DataSet)method.Invoke(instance, new object[] { number,dlyx });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        ///  根据登录邮箱，判断当前账户是否已开通结算账户
        /// </summary>
        /// <returns></returns>
        public DataSet GetSelSHZT(string user_mail)
        {
            try
            {
                MethodInfo method = type.GetMethod("GetSelSHZT");
                return (DataSet)method.Invoke(instance, new object[] { user_mail });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 根据登陆账号，判断卖家结算账户默认的经纪人是否暂停新业务
        /// </summary>
        /// <param name="user_mail">用户邮箱</param>
        /// <returns></returns>
        public string GetIsZTXYW(string user_mail)
        {
            try
            {
                MethodInfo method = type.GetMethod("GetIsZTXYW");
                return (string)method.Invoke(instance, new object[] { user_mail });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return "";
            }
        }
        /// <summary>
        /// 根据登陆账号，判断卖家结算账户默认的经纪人是否暂停新业务
        /// </summary>
        /// <param name="user_mail">用户邮箱</param>
        /// <returns></returns>
        public string GetZHDQKYYE(string user_mail)
        {
            try
            {
                MethodInfo method = type.GetMethod("GetZHDQKYYE");
                return (string)method.Invoke(instance, new object[] { user_mail });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return "";
            }
        }
        /// <summary>
        ///  获取所有一级商品分类
        /// </summary>
        /// <returns></returns>
        public DataSet GetSatrtSPFL()
        {
            try
            {
                MethodInfo method = type.GetMethod("GetSatrtSPFL");
                return (DataSet)method.Invoke(instance,null);
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 保存结算信息 (wyh 2013.1.18)
        /// </summary>
        /// <returns></returns>
        public string jszhzx(DataTable dt,string Type)
        {
            try
            {
                MethodInfo method = type.GetMethod("jszhzx");
                return (string)method.Invoke(instance, new object[] { dt, Type });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 根据选中的商品名称，获取子类商品
        /// </summary>
        /// <param name="sortName">商品名称</param>
        /// <returns></returns>
        public DataSet GetChildSPFL(string sortName)
        {
            try
            {
                MethodInfo method = type.GetMethod("GetChildSPFL");
                return (DataSet)method.Invoke(instance, new object[] { sortName });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 根据商品分类的名称，得到此商品分类下的SortID
        /// </summary>
        /// <param name="StartSPFL">一级商品名称</param>
        /// <param name="SecondSPFL">二级商品名称</param>
        /// <param name="ThreeSPFL">三级商品名称</param>
        /// <returns></returns>
        public string GetSPXXSortID(string StartSPFL, string SecondSPFL, string ThreeSPFL)
        {
            try
            {
                MethodInfo method = type.GetMethod("GetSPXXSortID");
                return (string)method.Invoke(instance, new object[] { StartSPFL, SecondSPFL, ThreeSPFL });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return "";
            }
        }

        /// <summary>
        /// 获取经纪人信息 (wyh 2013.1.21)
        /// </summary>
        /// <returns></returns>
        public DataSet Getjjrjinfo(string jjjsbh)
        {
            try
            {
                MethodInfo method = type.GetMethod("GetjjrState");
                return (DataSet)method.Invoke(instance, new object[] { jjjsbh });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// 获取基本资料及资质的相关数据
        /// </summary>
        /// <returns></returns>
        public DataSet Get_BasicInfo(string dlyx,string jszhlx)
        {
            try
            {
                MethodInfo method = type.GetMethod("Get_BasicInfo");
                return (DataSet)method.Invoke(instance, new object[] {dlyx, jszhlx });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }

        #region//审核买家、卖家基础数据

        /// <summary>
        /// 返回买家、卖家基础基础数据
        /// </summary>
        /// <param name="jslx"></param>
        /// <param name="jsbh"></param>
        /// <returns></returns>
        public DataSet ReturnMMJBascData(string jslx, string jsbh,string jjrjsbh)
        {
            try
            {
                MethodInfo method = type.GetMethod("ReturnMMJBascData");
                return (DataSet)method.Invoke(instance, new object[] { jslx, jsbh, jjrjsbh });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 审核买家基本信息【审核通过】
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public string[] VerifyBuyerDataPass(DataTable dataTable)
        {
            try
            {
                MethodInfo method = type.GetMethod("VerifyBuyerDataPass");
                return (string[])method.Invoke(instance, new object[] { dataTable });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        
        }
        /// <summary>
        /// 审核买家基本信息 【驳回】
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public string[] VerifyBuyerDataReject(DataTable dataTable)
        {
            try
            {
                MethodInfo method = type.GetMethod("VerifyBuyerDataReject");
                return (string[])method.Invoke(instance, new object[] { dataTable });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        
        
        }

        /// <summary>
        /// 审核卖家基本信息【审核通过】
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public string[] VerifySellerDataPass(DataTable dataTable)
        {
            try
            {
                MethodInfo method = type.GetMethod("VerifySellerDataPass");
                return (string[])method.Invoke(instance, new object[] { dataTable });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        
        }
        /// <summary>
        /// 审核卖家基本信息 【驳回】
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public string[] VerifySellerDataReject(DataTable dataTable)
        {
            try
            {
                MethodInfo method = type.GetMethod("VerifySellerDataReject");
                return (string[])method.Invoke(instance, new object[] { dataTable });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        
        }
        #endregion

        #region//结算账户审核后、发送数据
        /// <summary>
        /// 审核成功后发送邮件  发送短信
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public string[] SendVerifyPassEmailPhone(DataTable dataTable)
        {
            try
            {
                MethodInfo method = type.GetMethod("SendVerifyPassEmailPhone");
                return (string[])method.Invoke(instance, new object[] { dataTable });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
          
        }


        /// <summary>
        /// 审核失败后发送邮件
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public string[] SendVerifyRejectEmailPhone(DataTable dataTable)
        {
            try
            {
                MethodInfo method = type.GetMethod("SendVerifyRejectEmailPhone");
                return (string[])method.Invoke(instance, new object[] { dataTable });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }






        #endregion

        /// <summary>
        /// 得到所有商品分类，有卖家、买家区分
        /// </summary>
        /// <param name="jszh">"卖家"、"买家"</param>
        /// <returns></returns>
        public DataSet GetSPFL(string jszh)
        {
            try
            {
                MethodInfo method = type.GetMethod("GetSPFL");
                return (DataSet)method.Invoke(instance, new object[] { jszh });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }



        /// <summary>
        /// 添加一个新的投标信息
        /// </summary>
        /// <param name="DStbxx">投标信息</param>
        /// <returns></returns>
        public DataSet FBTHXXSaveADD(DataSet DStbxx)
        {
            try
            {
                MethodInfo method = type.GetMethod("FBTHXXSaveADD");
                return (DataSet)method.Invoke(instance, new object[] { DStbxx });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 撤销或修改一个投标信息
        /// </summary>
        /// <param name="Number">主表编号</param>
        /// <param name="id">子表编号</param>
        /// <param name="sp">撤销/修改</param>
        /// <param name="sl">修改的数量</param>
        /// <param name="jg">修改的单价</param>
        /// <returns></returns>
        public string DEltbxx(string Number, string id, string sp, string sl, string jg,string jsbh)
        {
            try
            {
                MethodInfo method = type.GetMethod("DEltbxx");
                return (string)method.Invoke(instance, new object[] { Number, id, sp, sl, jg, jsbh });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }
        //下达预订单
        public DataSet XDYDDSaveADD(DataSet ds)
        {
            try
            {
                MethodInfo method = type.GetMethod("XDYDDSaveADD");
                return (DataSet)method.Invoke(instance, new object[] { ds });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 修改、撤销预订单
        /// </summary>
        /// <param name="dt">修改的数据集，必须有的字段"修改的数量"、"修改的价格"、"预订单号"、"商品编号"、"合同周期"、"操作[修改、撤销]"</param>
        /// <param name="cz"></param>
        /// <returns>返回修改或撤销的结果</returns>
        public string UpdateOrDeleteYDD(DataSet ds)
        {
            try
            {
                MethodInfo method = type.GetMethod("UpdateOrDeleteYDD"); 
                return (string)method.Invoke(instance, new object[] { ds });

            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return "";
            }
        }



        /// <summary>
        /// 缴纳履约保证金
        /// </summary>
        /// <param name="zhubiao">投标信息主表编号</param>
        /// <param name="zibiao">投标信息子表编号</param>
        /// <param name="zibiao">角色编号</param>
        /// <returns>返回结果</returns>
        public string PayLYHZJ(string zhubiao, string zibiao, string jsbh)
        {
            try
            {
                MethodInfo method = type.GetMethod("PayLYHZJ");
                return (string)method.Invoke(instance, new object[] { zhubiao,  zibiao,  jsbh });

            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return "";
            }
        }



        /// <summary>
        /// 获取提货单的一些基本信息，用于提交提货单
        /// </summary>
        /// <param name="Number">合同编号</param>
        /// <param name="jsbh">买家角色编号</param>
        /// <returns></returns>
        public DataSet GetTHinfo(string Number, string jsbh)
        {

            try
            {
                MethodInfo method = type.GetMethod("GetTHinfo");
                return (DataSet)method.Invoke(instance, new object[] { Number, jsbh });

            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 保存提货单
        /// </summary>
        /// <param name="dsinfo">提货单提交参数</param>
        /// <returns></returns>
        public string AddTHD(DataSet dsinfo)
        {
            try
            {
                MethodInfo method = type.GetMethod("AddTHD");
                return (string)method.Invoke(instance, new object[] { dsinfo });

            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return "";
            }
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="dsinfo">发货提交参数</param>
        /// <returns></returns>
        public string UpdateFaHuo(DataSet dsinfo)
        {
            try
            {
                MethodInfo method = type.GetMethod("UpdateFaHuo");
                return (string)method.Invoke(instance, new object[] { dsinfo });

            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return "";
            }
        }



        /// <summary>
        /// 签收
        /// </summary>
        /// <param name="Number">提货单号</param>
        /// <param name="jsbh">买家角色编号</param>
        /// <returns></returns>
        public string UpdateQianShou(string Number, string jsbh)
        {
            try
            {
                MethodInfo method = type.GetMethod("UpdateQianShou");
                return (string)method.Invoke(instance, new object[] { Number, jsbh });

            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return "";
            }
        }



        /// <summary>
        /// 获取未读提醒
        /// </summary>
        /// <param name="DLYX">登陆邮箱</param>
        /// <returns></returns>
        public string GetTrayMsg(string DLYX)
        {
            try
            {
                MethodInfo method = type.GetMethod("GetTrayMsg");
                return (string)method.Invoke(instance, new object[] { DLYX });

            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return "";
            }
        }


        /// <summary>
        /// 获取用户的可用额度
        /// </summary>
        /// <param name="DLYX">登陆邮箱</param>
        /// <returns></returns>
        public string GetMyMoney(string DLYX, string bankinfo)
        {
            try
            {
                MethodInfo method = type.GetMethod("GetMyMoney");
                return (string)method.Invoke(instance, new object[] { DLYX, bankinfo });

            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return "";
            }
        }

        /// <summary>
        /// 加入或删除自选商品
        /// </summary>
        /// <param name="DLYX">登录邮箱</param>
        /// <param name="SPBH">商品编号</param>
        /// <param name="edit">加入,删除</param>
        /// <returns></returns>
        public string ZXSPedit(string DLYX, string SPBH,string edit)
        {
            try
            {
                MethodInfo method = type.GetMethod("ZXSPedit");
                return (string)method.Invoke(instance, new object[] { DLYX, SPBH, edit });

            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return "";
            }
        }

        /// <summary>
        /// 根据以及分类获取二级商品分类
        /// </summary>
        /// <param name="SortParentID">父分类id</param>
        /// <returns></returns>
        public DataSet GetSPFLerji(string SortParentID)
        {

            try
            {
                MethodInfo method = type.GetMethod("GetSPFLerji");
                return (DataSet)method.Invoke(instance, new object[] { SortParentID });

            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 停止或开启审核，或暂停与恢复选中用户的新业务，或仅获取状态而已。
        /// </summary>
        /// <param name="JJRjsbh">经纪人角色编号</param>
        /// <param name="JJRjsbh">经纪人登陆邮箱</param>
        /// <param name="type">操作类型， 新注册、新业务</param>
        /// <param name="BeSetDLYX">操作对象的登陆邮箱</param>
        /// <returns></returns>
        public string[] JJRset(string JJRjsbh, string JJRDLYX, string sptype, string BeSetDLYX)
        {

            try
            {
                MethodInfo method = type.GetMethod("JJRset");
                return (string[])method.Invoke(instance, new object[] { JJRjsbh, JJRDLYX, sptype, BeSetDLYX });

            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 维护经济人
        /// </summary>
        /// <param name="array">对象名称</param>
        /// <param name="methodName">方法名称</param>
        /// <returns></returns>
        public DataSet UpdateJJR(DataSet obj,string Method)
        {
            try
            {

                MethodInfo method = type.GetMethod(Method);
                return (DataSet)method.Invoke(instance, new object[] { obj });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
                return null;
            }
        }

    }
}
