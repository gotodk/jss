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
/// <summary>
///GetOldGw 的摘要说明
/// </summary>
namespace GWLEI
{
    public class GetOldGw
    {
        public GetOldGw()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        //得到旧公文文件的信息
        public static OldGw GetOldGwInfo(string id)
        {
            string sql;
            OldGw od = new OldGw();
            sql="select * from gwcgrecord where id='"+id.Trim()+"'";
            SqlDataReader sr = DbHelperSQL.ExecuteReader(sql);
            if (sr.Read())
            {
                od.Pid = sr["pid"].ToString();
                od.Pname = sr["pname"].ToString();
                od.Fildstate = sr["filestate"].ToString();
                od.Filezz = sr["filezz"].ToString();
                od.Filehqtype = sr["filehqtype"].ToString();
                od.Filepath = sr["filepath"].ToString();
                od.Oldfilename = sr["oldfilename"].ToString();
                od.Newfilename = sr["newfilename"].ToString();
                od.Fujianname = sr["fujianname"].ToString();
                if(sr["fujianfile"]==DBNull.Value)
                {
                    od.Fujianexit = false; //判断附件存在
                }
                else
                {
                    od.Fujianexit = true;
                  od.Fujianfile = (byte[])sr["fujianfile"];
                }
                od.Filetype = sr["filetype"].ToString();
                od.Dept = sr["dept"].ToString();
                od.Convertfilehqtype = sr["convertfilehqtype"].ToString();
                od.Converttype = sr["converttype"].ToString();
                od.Convertfileid=sr["convertfileid"].ToString();
                od.Boolconvert = sr["boolconvert"].ToString();
                od.Gwdinggao = sr["gwdinggao"].ToString();

            }
            sr.Close();
            return od;

        }
       //split函数
        public static string[] getsplit(string s)
        {
            string[] a;
            return s.Split('/');
        }

        /// <summary>
        /// 更改公文状态
        /// </summary>
        /// <param name="fileid"></param>
        /// <param name="typename"></param>
        /// <returns></returns>
        public static int ChangeGWState(string fileid,string typename)
        {
            int rcount = 0;
            string sql="update gwcgrecord set filestate='"+typename+"' where id='"+fileid.Trim()+"'";
            rcount = DbHelperSQL.ExecuteSql(sql);
            return rcount;     
        }
        //更改某字段
        public static int ChangeGWitem(string fileid, string items,string itemvalue)
        {
            int rcount = 0;
            string sql = "update gwcgrecord set "+ items.Trim()+"='" +itemvalue.Trim()+ "' where id='" + fileid.Trim() + "'";
            rcount = DbHelperSQL.ExecuteSql(sql);
            return rcount;
        }
      

    }
}
