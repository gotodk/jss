using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using FMOP.DB;
/// <summary>
///GetHqGw 的摘要说明
/// </summary>
namespace GWLEI
{
    public class GetHqGw
    {
        public GetHqGw()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 得到分配文件的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static HqGw GetHqGwInfo(string id)
        {
            string sql;
            HqGw hw = new HqGw();
    
            sql = "select * from gwcgsp where newfileid='" + id.Trim() + "'";
            SqlDataReader sr = DbHelperSQL.ExecuteReader(sql);
            if (sr.Read())
            {
                hw.Fileid = sr["fileid"].ToString();
                hw.Filehqtype = sr["filehqtype"].ToString();
                hw.Pid = sr["pid"].ToString();
                hw.Pname = sr["pname"].ToString();
                hw.Filezz = sr["filezz"].ToString();
                hw.Filestate = sr["filestate"].ToString();
                hw.Gwstate = sr["gwstate"].ToString();
                hw.Newfilepath = sr["newfilepath"].ToString();
                hw.Newfilename = sr["newfilename"].ToString();
                hw.Oldfilename = sr["oldfilename"].ToString();
                hw.Changemess = sr["changemess"].ToString();
                hw.Changetime = sr["changetime"].ToString();
                hw.Jielun = sr["jielun"].ToString();
                hw.Filetype = sr["filetype"].ToString();
                hw.Dept = sr["dept"].ToString();
            }
            sr.Close();
            return hw;

        }
        /// <summary>
        /// 更新分配文件文件状态
        /// </summary>
        /// <param name="fileid"></param>
        /// <param name="typename"></param>
        /// <returns></returns>
        public static int ChangeHQGWState(string fileid, string typename)
        {
            int rcount = 0;
            string sql = "update gwcgsp set filestate='" + typename + "' where newfileid='" + fileid.Trim() + "'";
            rcount = DbHelperSQL.ExecuteSql(sql);
            return rcount;
        }
        /// <summary>
        /// 更新分配文件公文状态，及意见结论
        /// </summary>
        /// <param name="fileid"></param>
        /// <param name="gwstate"></param>
        /// <param name="mess"></param>
        /// <param name="jl"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public static int   ChangeHQGWState(string fileid, string gwstate,string mess,string jl,string times)
        {
            int rcount = 0;
            string sql = "update gwcgsp set gwstate='" + gwstate + "',changemess='"+mess.Trim()+"',jielun='"+jl+"',changetime='"+times.Trim()+"' where newfileid='" + fileid.Trim() + "'";
            rcount = DbHelperSQL.ExecuteSql(sql);
           return rcount;
        }
        
        /// <summary>
        /// //判断审签具备条件
        /// </summary>
        /// <param name="oldfileid"></param>
        /// <returns></returns>
        public static bool getSQJB(string oldfileid)
        {
            string sql = "select count(*) from gwcgsp where fileid='" + oldfileid + "' and (jielun is NuLL or jielun='')";
            int rcount = Convert.ToInt32(DbHelperSQL.GetSingle(sql));

            if (rcount==0)
            {
                return true;//都参与了，具备使用审核通过检查条件可以使用getHQtgcount()方法了
            }
            else
            {
                return false;
            }

        }
        
        /// <summary>
        /// 判定审签通过与否
        /// </summary>
        /// <param name="oldfileid"></param>
        /// <returns></returns>
        public static bool getSQTG(string oldfileid)
        {
            int rcount = 0,rcount2=0;
            string sql= "select count(*) from gwcgsp where fileid='"+oldfileid+"'";
            string sql2="select count(*) from gwcgsp where fileid='"+oldfileid+"' and jielun='通过'";
            rcount= Convert.ToInt32(DbHelperSQL.GetSingle(sql));
            rcount2 = Convert.ToInt32(DbHelperSQL.GetSingle(sql2));
            if(rcount==rcount2&&rcount!=0)
            {
                 return true; //审核通过
            }
            else
            {
                return false; //审核未通过。

            }
  
        }
        /// <summary>
        /// 判断会签是否完成
        /// </summary>
        /// <param name="oldfileid"></param>
        /// <returns></returns>
        public static bool getHQTG(string oldfileid)
        {
            int rcount = 0, rcount2 = 0;
            string sql = "select count(*) from gwcgsp where fileid='" + oldfileid + "'";
            string sql2 = "select count(*) from gwcgsp where fileid='" + oldfileid + "' and jielun='' and gwstate='已提交'";
            rcount = Convert.ToInt32(DbHelperSQL.GetSingle(sql));
            rcount2 = Convert.ToInt32(DbHelperSQL.GetSingle(sql2));
            if (rcount == rcount2 && rcount != 0)
            {
                return true; //会签完
            }
            else
            {
                return false; //没有会签完

            }

        }
    
        /// <summary>
        /// 判断分配文件是否已经有人参与了修改
        /// </summary>
        /// <param name="oldfileid"></param>
        /// <returns></returns>
        public static bool ischange(string oldfileid)
        {
            int rcount = 0;
            string sql = "select count(*) from gwcgsp where fileid='" + oldfileid + "' and (filestate='已修改' or gwstate='已提交')";
             rcount = Convert.ToInt32(DbHelperSQL.GetSingle(sql));

            if (rcount>0)
            {
                return true; //有人参与了
            }
            else
            {
                return false;
            }
  
        }



    }
}
