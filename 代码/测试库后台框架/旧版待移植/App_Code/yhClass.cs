using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Data;
using FMOP.DB;
namespace ZTC
{
/// <summary>
///JsStarClass 的摘要说明
/// </summary>
    public class yhClass
    {
        private string userEmail = "";
        public yhClass()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
       

        }
        public yhClass(string emails)
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
            this.userEmail = emails;

        }
        /// <summary>
        /// 得到用户信息
        /// </summary>
        /// <returns></returns>
        public YhInfo GetYHInfo()
        {
            YhInfo u = new YhInfo();
            DataSet ds=DbHelperSQL.Query("select XM,DWMC,SSFWSBH,SSFWSMC,SSBSC from YHZTC_Users where DLYX='"+userEmail+"'");
            if(ds.Tables[0].Rows.Count>0)
            { 
               if(ds.Tables[0].Rows[0][0]!=null)
               {
                 u.UserName=ds.Tables[0].Rows[0][0].ToString().Trim();
               }
               else
               {
                 u.UserName="";
               }
               //
               if(ds.Tables[0].Rows[0][1]!=null)
               {
                 u.UserGzdw=ds.Tables[0].Rows[0][1].ToString().Trim();
               }
               else
               {
                  u.UserGzdw="";
               }
                //
               if (ds.Tables[0].Rows[0][2] != null)
               {
                   u.UserFwsID = ds.Tables[0].Rows[0][2].ToString().Trim();
               }
               else
               {
                   u.UserFwsID = "";
               }
                //
               if (ds.Tables[0].Rows[0][3] != null)
               {
                   u.UserFwsName= ds.Tables[0].Rows[0][3].ToString().Trim();
               }
               else
               {
                   u.UserFwsName = "";
               }
                //
               if (ds.Tables[0].Rows[0][4] != null)
               {
                   u.UserSSBSC = ds.Tables[0].Rows[0][4].ToString().Trim();
               }
               else
               {
                   u.UserSSBSC = "";
               }
              
          
            }
            else
            {
                  u.UserName="";
                  u.UserGzdw="";
                  u.UserFwsID = "";
                  u.UserFwsName = "";
                  u.UserSSBSC = "";
            }
            //得到用户点数
            u.UserDsNum = GetDianShu(userEmail.Trim());
      
            //得到用户星级与图片
            if (u.UserDsNum >= 100 && u.UserDsNum < 500)
            {
                u.UserXJ = 1;
                u.UserDJ = "1星";
                u.UserNextDs = Math.Round(500 - u.UserDsNum, 2);
                u.UserStarPath = "images/yh_star1.gif";

            }
            else if (u.UserDsNum >= 500 && u.UserDsNum < 1000)
            {
                u.UserXJ = 2;
                u.UserDJ = "2星";
                u.UserNextDs = Math.Round(1000 - u.UserDsNum, 2);
                u.UserStarPath = "images/yh_star2.gif";
            }
            else if (u.UserDsNum >= 1000 && u.UserDsNum < 3000)
            {
                u.UserXJ = 3;
                u.UserDJ = "3星";
                u.UserNextDs = Math.Round(3000 - u.UserDsNum, 2);
                u.UserStarPath = "images/yh_star3.gif";
            }
            else if (u.UserDsNum >= 3000 && u.UserDsNum < 5000)
            {
                u.UserXJ = 4;
                u.UserDJ = "4星";
                u.UserNextDs = Math.Round(5000 - u.UserDsNum, 2);
                u.UserStarPath = "images/yh_star4.gif";
            }
            else if (u.UserDsNum >= 5000 && u.UserDsNum < 10000)
            {
                u.UserXJ = 5;
                u.UserDJ = "5星";
                u.UserNextDs = Math.Round(10000 - u.UserDsNum, 2);
                u.UserStarPath = "images/yh_star5.gif";
            } //以上是星
            else if (u.UserDsNum >= 10000 && u.UserDsNum < 20000)
            {
                u.UserXJ = 6;
                u.UserDJ = "1钻";
                u.UserNextDs = Math.Round(20000 - u.UserDsNum, 2);
                u.UserStarPath = "images/yh_zs1.gif";
            }
            else if (u.UserDsNum >= 20000 && u.UserDsNum < 30000)
            {
                u.UserXJ = 7;
                u.UserDJ = "2钻";
                u.UserNextDs = Math.Round(30000 - u.UserDsNum, 2);
                u.UserStarPath = "images/yh_zs2.gif";
            }
            else if (u.UserDsNum >= 30000 && u.UserDsNum < 50000)
            {
                u.UserXJ = 8;
                u.UserDJ = "3钻";
                u.UserNextDs = Math.Round(50000 - u.UserDsNum, 2);
                u.UserStarPath = "images/yh_zs3.gif";
            }
            else if (u.UserDsNum >= 50000 && u.UserDsNum < 80000)
            {
                u.UserXJ = 9;
                u.UserDJ = "4钻";
                u.UserNextDs = Math.Round(80000 - u.UserDsNum, 2);
                u.UserStarPath = "images/yh_zs4.gif";
            }
            else if (u.UserDsNum >= 80000 && u.UserDsNum < 100000)
            {
                u.UserXJ = 10;
                u.UserDJ = "5钻";
                u.UserNextDs = Math.Round(100000 - u.UserDsNum, 2);
                u.UserStarPath = "images/yh_zs5.gif";
            } //以上是钻        
            else if (u.UserDsNum >= 100000 && u.UserDsNum < 150000)
            {
                u.UserXJ = 11;
                u.UserDJ = "1冠";
                u.UserNextDs = Math.Round(150000 - u.UserDsNum, 2);
                u.UserStarPath = "images/yh_hg1.gif";
            }
            else if (u.UserDsNum >= 150000 && u.UserDsNum < 200000)
            {
                u.UserXJ = 12;
                u.UserDJ = "2冠";
                u.UserNextDs = Math.Round(200000 - u.UserDsNum, 2);
                u.UserStarPath = "images/yh_hg2.gif";
            }
            else if (u.UserDsNum >= 200000 && u.UserDsNum < 250000)
            {
                u.UserXJ = 13;
                u.UserDJ = "3冠";
                u.UserNextDs = Math.Round(250000 - u.UserDsNum, 2);
                u.UserStarPath = "images/yh_hg3.gif";
            }
            else if (u.UserDsNum >= 250000 && u.UserDsNum < 300000)
            {
                u.UserXJ = 14;
                u.UserDJ = "4冠";
                u.UserNextDs = Math.Round(300000 - u.UserDsNum, 2);
                u.UserStarPath = "images/yh_hg4.gif";
            }
            else if (u.UserDsNum >= 300000)
            {
                u.UserXJ = 15;
                u.UserDJ = "5冠";
                u.UserNextDs = 0;
                u.UserStarPath = "images/yh_hg5.gif";
            }
            else
            {
                u.UserXJ = 0;
                u.UserDJ = "0星";
                u.UserNextDs = 0;
                u.UserStarPath = "images/yh_starhui.gif";
            }
            return u;

        }
        /// <summary>
        /// 设置用户点数
        /// </summary>
        /// <param name="dsNum">点数double</param>
        /// <param name="addType">类型名称</param>
        /// <returns></returns>
        private Boolean setUserPNum(double dsNum,string addType)
        {
            YhInfo y =GetYHInfo();
            string sqls = "INSERT INTO DSTJB(Number,DS,LX,CreateUser,FWSBH,FWSMC,SSBSC) values";
            sqls += " (dbo.getID('DSTJB')," + dsNum + ",'" + addType.Trim() + "','" + userEmail.Trim() + "','"+y.UserFwsID+"','"+y.UserFwsName+"','"+y.UserSSBSC+"')";
            
            
             int rcount = DbHelperSQL.ExecuteSql(sqls);
            try
            {   
                if (rcount== 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                return false;
            }
     
        }
        /// <summary>
        /// 指定用户email，设置用户点数
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="dsNum"></param>
        /// <param name="addType"></param>
        /// <returns></returns>
        private Boolean setUserPNum(string emailID,double dsNum, string addType)
        {
            YhInfo y = new YhInfo();
            y = GetYHInfo();
            string sqls = "INSERT INTO DSTJB(Number,DS,LX,CreateUser,FWSBH,FWSMC,SSBSC) values";
            sqls += " (dbo.getID('DSTJB')," + dsNum + ",'" + addType.Trim() + "','" +emailID.Trim()+ "','"+y.UserFwsID+"','"+y.UserFwsName+"','"+y.UserSSBSC+ "')";
            int rcount = DbHelperSQL.ExecuteSql(sqls);
            try
            {
                if (rcount == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                return false;
            }

        }
        /// <summary>
        /// 登录时添加用户10点
        /// </summary>
        /// <param name="dsNum">点数</param>
        /// <returns>1添加成功，2添加失败，3不需添加，成功</returns>
        public int AddDlPNum()
        {
            DataSet ds = DbHelperSQL.Query("select count(*) from DSTJB where LX='登录' and CreateUser='" + userEmail + "' and Convert(varchar(10),CreateTime,120)='" + System.DateTime.Now.ToString("yyyy-MM-dd") + "'");         
            if (ds.Tables[0].Rows[0][0].ToString()=="0")  //不存在需要添加
            {
                if (setUserPNum(10, "登录") ==true)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 0;
            }
            
        }
        /// <summary>
        /// 指令添加用户0点(不用了)
        /// </summary>
        ///<returns>true 成功，false 失败</returns>
        public Boolean AddZlPNum()
        {
            return setUserPNum(0, "指令");

        }
       /// <summary>
        /// 介绍，成功介绍一个用户注册，计200点。
       /// </summary>
       /// <param name="emailID">介绍人emailID</param>
       /// <returns>true 成功，false 失败</returns>
        public Boolean AddJSPNum(string emailID)
        {
            return setUserPNum(emailID,200, "介绍");
        }
        /// <summary>
        /// 注册，赠送100点启动点数；
        /// </summary>
        ///<returns>true 成功，false 失败</returns>
        public Boolean AddZCPNum()
        {
            return setUserPNum(100, "注册");
        }
        
       /// <summary>
       /// //得到所有用户的点数
       /// </summary>
       /// <returns></returns>
        public DataSet GetDianShu()
        {

            return DbHelperSQL.Query("select * from getYHDS");
        }
        
        /// <summary>
        /// //得到单个用户的点数
        /// </summary>
        /// <param name="emailID">用户EmailID账号</param>
        /// <returns></returns>
        public double GetDianShu(string emailID)
        {
            double r = 0.00;
            DataSet ds=DbHelperSQL.Query("select 用户账号,点数 from getYHDS  where 用户账号='" + emailID.Trim() + "'");
            if (ds!= null && ds.Tables[0].Rows.Count > 0)
            {
                r = Convert.ToDouble(ds.Tables[0].Rows[0][1]);
            }
            return r;
        }
       
         

    }
}