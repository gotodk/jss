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
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;

/// <summary>
///GetSwFatherInfohuiqian 的摘要说明
/// </summary>
/// 
namespace ShareLiu.SwClasses
{
    public class GetSwFatherInfohuiqian
    {
        public GetSwFatherInfohuiqian()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }


        /// <summary>
        /// 得到事务对象信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>事务表对象</returns>
        public static SwFatherObj GetSw(string id)
        {
            string sql;
            SwFatherObj sf = new SwFatherObj();
            sql = "select * from SWCreatehuiqian where smid='" + id + "'";
            SqlDataReader sr = DbHelperSQL.ExecuteReader(sql);
            if (sr.Read())
            {
                sf.Modetype = sr["modetype"].ToString();
                sf.Modename = sr["modename"].ToString();
                sf.Smdoctitle = sr["smdoctitle"].ToString();
                sf.Advices = sr["advices"].ToString();

                sf.Userid = sr["userid"].ToString();
                sf.Username = sr["username"].ToString();

                sf.Deptname = sr["deptname"].ToString();
                sf.Newfilename = sr["newfilename"].ToString();
                sf.NewFilepath = sr["newfilepath"].ToString();
                sf.Oldfilename = sr["oldfilename"].ToString();
                sf.Oldfilepath = sr["oldfilepath"].ToString();
                // sf.Spid = sr["spid"].ToString();
                //  sf.Spname = sr["spname"].ToString();
                sf.Smstates = sr["smstates"].ToString();
                if (sr["createtime"] == DBNull.Value)
                {
                    //sf.Createtime = Convert.ToDateTime("1900-01-01");
                }
                else
                {
                    sf.Createtime = Convert.ToDateTime(sr["createtime"]);
                }
                //if (sr["sptime"] == DBNull.Value)
                //{
                //   // sf.sptime = Convert.ToDateTime("1900-01-01");
                //}
                //else
                //{
                //    sf.Sptime = Convert.ToDateTime(sr["sptime"]);
                //}

            }
            sr.Close();
            return sf;
        }
        /// <summary>
        /// 更改文件状态
        /// </summary>
        /// <param name="SmId">关键字值</param>
        /// <param name="Fstates">字段值</param>
        /// <returns>影响记录行数</returns>
        public static int ChangeFileStates(string SmId, string Fstates)
        {
            int rcount = 0;
            string sql = "update SWCreatehuiqian set smstates='" + Fstates.Trim() + "' where smid='" + SmId.Trim() + "'";
            rcount = DbHelperSQL.ExecuteSql(sql);
            return rcount;
        }
        /// <summary>
        /// 删除《分配表》SwSqlRecord（从表）记录，处理回滚
        /// </summary>
        /// <param name="Smid">主表外键，从表主键1</param>
        /// <param name="page">所属页面</param>
        public static void DeleteSonRecord(string Smid, System.Web.UI.Page page)
        {
            string delsql = "delete from SwSqRecordhuiqian where smid='" + Smid + "'";
            int rdelcount = DbHelperSQL.ExecuteSql(delsql);
            if (rdelcount > 0)
            {
                MessageBox.Show(page, "分配不成功,记录回滚");
            }
            else
            {
                MessageBox.Show(page, "分配不成功,记录不能回滚，请联系系统管理员");
            }
        }
        /// <summary>
        /// 删除分配表2，同DeleteSonRecord功能，处理【反分配】
        /// </summary>
        /// <param name="Smid">主表外键</param>
        /// <param name="page">所属页面</param>
        public static void DeleteSonRecordTwo(string Smid, System.Web.UI.Page page)
        {
            string delsql = "delete from SwSqRecordhuiqian where smid='" + Smid + "'";
            int rdelcount = DbHelperSQL.ExecuteSql(delsql);
            MessageBox.Show(page, "删除成功,删除行数" + rdelcount.ToString());

        }
        /// <summary>
        /// 根据一个关键字修改字段数值，支持一个字段的修改
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Kid">主键</param>
        /// <param name="kValues">主键值</param>
        /// <param name="items">字段项目</param>
        /// <param name="itemvalue">字段值</param>
        /// <returns>影响记录行数</returns>
        public static int ChangeItemsValue(string TableName, string Kid, string kValues, string items, string itemvalue)
        {
            int rcount = 0;
            string sql = "update " + TableName.Trim() + " set " + items.Trim() + "='" + itemvalue.Trim() + "' where" + Kid.Trim() + "='" + kValues.Trim() + "'";
            rcount = DbHelperSQL.ExecuteSql(sql);
            return rcount;
        }
    }
}
