using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;
using System.Data.SqlClient;
using System.Collections;
/// <summary>
///GetFwGw 的摘要说明
/// </summary>
namespace GWLEI
{
    public class GetFwGw
    {
        public GetFwGw()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        public static fwGw GetFWinfo(string cid)
        {
            string sql;
            fwGw fw = new fwGw();
            sql = "select * from gwdgfw where cid='" + cid.Trim() + "'";
            SqlDataReader sr = DbHelperSQL.ExecuteReader(sql);
            if (sr.Read())
            {
                fw.Cid = sr["cid"].ToString();
                fw.Sourcefileid = sr["sourcefileid"].ToString();
                fw.Pid = sr["pid"].ToString();
                fw.Fwpid = sr["fwpid"].ToString();
                fw.Fwtime = sr["fwtime"].ToString();
                fw.Fwstate = sr["fwstate"].ToString();
                fw.Filechangestate = sr["filechangestate"].ToString();
                fw.Dinggaotime = sr["dinggaotime"].ToString();
                fw.Boolconvert = sr["boolconvert"].ToString();

            }
            sr.Close();
            return fw;
        }
        /// <summary>
        /// 更改发文字段状态应用于表gwdgfw
        /// </summary>
        /// <param name="fileid"></param>
        /// <param name="items"></param>
        /// <param name="itemvalue"></param>
        /// <returns></returns>
        public static int Changefwitem(string cid, string items, string itemvalue)
        {
            int rcount = 0;
            string sql = "update gwdgfw set " + items.Trim() + "='" + itemvalue.Trim() + "' where cid='" + cid.Trim() + "'";
            rcount = DbHelperSQL.ExecuteSql(sql);
            return rcount;
        }
        //更改已发文
        public static int changefwtable(string cid,string fid,string ftime)
        {
            int rcount = 0;
            string sql = "update gwdgfw set fwpid='"+fid.Trim()+"',fwtime='"+ftime.Trim()+"',fwstate='已发文' where cid='" + cid.Trim() + "'";
            rcount = DbHelperSQL.ExecuteSql(sql);
            return rcount;
        }
    }
}


