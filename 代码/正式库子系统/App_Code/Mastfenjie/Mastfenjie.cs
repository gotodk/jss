using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Collections;
using FMOP.DB;
/// <summary>
///Mastfenjie 的摘要说明
/// </summary>
public class Mastfenjie
{
	public Mastfenjie()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 插入新数据
    /// </summary>
    /// <param name="ht">一级任务主题</param>
    /// <param name="ds1">成果数据集</param>
    /// <param name="ds2">子任务数据集</param>
    /// <param name="issend">是否发布</param>
    /// <param name="iscreate">是否第一次创建0：根任务1：子任务</param>
    /// <returns> false :失败 ;true :成功</returns>
    public bool InsertData(Hashtable ht, DataSet ds1, DataSet ds2 , string issend ,string iscreate)
    {
        ArrayList al = new ArrayList();
        bool ispass = false;
        string StrRMast = "";
        string strchengguo = "";
        string strCmast = "";
        if (iscreate == "0")
        { 
           
            if (ht != null)
            {
                string dtrnn = ht["ID"].ToString();
                StrRMast = "insert into dbo.SW_YUNXINGKANBAN (ID,PID,ProjTitle,Projtype,ProjStarttime,ProjOvertime,ProjSouece,ProjecsouceName,IsInkaohe,ProjCheDepartment,ProjCheckerID,ProjCheckName,ProjCreatime,ProjCreateID, ProiState,ProjMiaoshu,ProjLable,Isyifenpei) values ('" + ht["ID"].ToString() + "','0','" + ht["工作事务名称"].ToString().Trim() + "','" + ht["事务类别"].ToString() + "','" + ht["任务开始时间"].ToString() + "','" + ht["任务结束时间"].ToString() + "','" + ht["任务来源ID"].ToString() + "','" + ht["任务来源名称"].ToString() + "','" + ht["是否列入考核"].ToString() + "','" + ht["主导部门"].ToString() + "','" + ht["主导人ID"].ToString() + "','" + ht["主导人"].ToString() + "','" + DateTime.Now.ToString() + "','" + ht["创建人"].ToString() + "','" + issend + "','" + ht["任务描述"].ToString() + "','进行中','是')";


                al.Add(StrRMast);
           
            if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    strchengguo = " insert into SW_Chengguoyanshou (PID,chengguotype,jiaofutime,tibaobeirong,Tibaotime ) values ('" + ht["ID"].ToString() + "','" + dr["成果类型"].ToString() + "','" + dr["交付时间"].ToString() + "','" + dr["成果描述及标准"].ToString() + "','"+DateTime.Now.ToString()+"')";

                    al.Add(strchengguo);
                }

                string StrChange = " Update  SW_YUNXINGKANBAN set Isyifenpei = '是' where ID = '" + ht["ID"].ToString().Trim() + "'";
                al.Add(StrChange);

            
            }

            if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
            {
                string StrID = (int.Parse(dtrnn.Trim())+1).ToString();
                foreach (DataRow dr in ds2.Tables[0].Rows)
                {
                    strCmast = "insert into dbo.SW_YUNXINGKANBAN (ID,PID,ProjTitle,Projtype,ProjStarttime,ProjOvertime,ProjSouece,ProjecsouceName,IsInkaohe,ProjCheDepartment,ProjCheckerID,ProjCheckName,ProjCreatime,ProjCreateID, ProiState,ProjMiaoshu,ProjLable,Isyifenpei) values ('" + StrID + "','" + ht["ID"].ToString() + "','" + dr["任务名称"].ToString().Trim() + "','" + dr["任务类别"].ToString() + "','" + dr["任务开始时间"].ToString() + "','" + dr["任务结束时间"].ToString() + "','" + ht["创建人"].ToString() + "','" + ht["创建人名称"].ToString() + "','" + dr["是否列入考核"].ToString() + "','" + dr["任务主导部门"].ToString() + "','" + dr["主导人ID"].ToString() + "','" + dr["任务主导人"].ToString() + "','" + DateTime.Now.ToString() + "','" + ht["创建人"].ToString() + "','" + issend + "','" + dr["任务详情描述"].ToString() + "','进行中','否')";

                    StrID = (int.Parse(StrID.Trim()) + 1).ToString();
                    al.Add(strCmast);
                }

            }

             
            DbHelperSQL.ExecuteSqlTran(al);

            ispass = true;


        }
        }

      
        

        return ispass;
    }



    public bool InsChildMastFenjie( Hashtable ht ,DataSet ds1 , DataSet ds2)
    {
        bool ispass = false;

       
            ArrayList al = new ArrayList();
            if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
            {

                string strchengguo = "";
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    strchengguo = " insert into SW_Chengguoyanshou (PID,chengguotype,jiaofutime,tibaobeirong,Tibaotime ) values ('" + ht["PID"].ToString().Trim() + "','" + dr["成果类型"].ToString() + "','" + dr["交付时间"].ToString() + "','" + dr["成果描述及标准"].ToString() + "','" + DateTime.Now.ToString() + "')";

                    al.Add(strchengguo);
                  
                }

                string StrChange = " Update  SW_YUNXINGKANBAN set Isyifenpei = '是' where ID = '" + ht["PID"].ToString().Trim() + "'";
                al.Add(StrChange);

            }

            if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
            {
                string strCmast = "";
                string StrID = GetNum().Trim();
                foreach (DataRow dr in ds2.Tables[0].Rows)
                {
                    strCmast = "insert into dbo.SW_YUNXINGKANBAN (ID,PID,ProjTitle,Projtype,ProjStarttime,ProjOvertime,ProjSouece,ProjecsouceName,IsInkaohe,ProjCheDepartment,ProjCheckerID,ProjCheckName,ProjCreatime,ProjCreateID, ProiState,ProjMiaoshu,ProjLable,Isyifenpei) values ('" + StrID + "','" + ht["PID"].ToString().Trim() + "','" + dr["任务名称"].ToString().Trim() + "','" + dr["任务类别"].ToString() + "','" + dr["任务开始时间"].ToString() + "','" + dr["任务结束时间"].ToString() + "','" + ht["任务来源ID"].ToString() + "','" + ht["任务来源名称"].ToString().Trim() + "','" + dr["是否列入考核"].ToString() + "','" + dr["任务主导部门"].ToString() + "','" + dr["主导人ID"].ToString() + "','" + dr["任务主导人"].ToString() + "','" + DateTime.Now.ToString() + "','" + ht["任务来源ID"].ToString() + "','" + ht["iseng"].ToString().Trim() + "','" + dr["任务详情描述"].ToString() + "','进行中','否')";

                    StrID = (int.Parse(StrID.Trim()) + 1).ToString();
                    al.Add(strCmast);
                }


            }

            DbHelperSQL.ExecuteSqlTran(al);

            ispass = true;
            return ispass;
      
        
    }


    private string GetNum()
    {
        string sql;
        sql = "select dbo.getSWchakanMaxid() as maxs ";
        DataSet ds = DbHelperSQL.Query(sql);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            return dr["maxs"].ToString();
        }
        return "999999";
    }


    public bool NewMastAdd(Hashtable htMast,Hashtable htchild)
    {
        ArrayList al = new ArrayList();
        bool ispass = false;
        string StrRMast = "";
        string StrChild = "";
        if (htMast != null)
        {
            StrRMast = "insert into dbo.SW_YUNXINGKANBAN (ID,PID,ProjTitle,Projtype,ProjStarttime,ProjOvertime,ProjecsouceName,ProjCheDepartment,ProjCheckerID,ProjCheckName,ProjCreatime,ProjCreateID, ProjCreateName,ProiState,ProjMiaoshu,ProjLable,Isyifenpei,ProjxiebanBm,ProjxiebanNum) values ('" + htMast["ID"].ToString() + "','0','" + htMast["工作事务名称"].ToString().Trim() + "','" + htMast["事务类别"].ToString() + "','" + htMast["任务开始时间"].ToString() + "','" + htMast["任务结束时间"].ToString() + "','" + htMast["任务来源"].ToString() + "','" + htMast["主导部门"].ToString() + "','" + htMast["主导人ID"].ToString() + "','" + htMast["主导人"].ToString() + "','" + DateTime.Now.ToString() + "','" + htMast["创建人"].ToString() + "','" + htMast["创建人名称"] + "','0','" + htMast["任务描述"].ToString() + "','进行中','是','" + htMast["协办部门"].ToString() + "','" + htMast["协办人员"].ToString().Trim() + "')";

            al.Add(StrRMast);
        }

        if (htchild != null)
        {
            StrChild = " insert into SW_Chengguoyanshou (PID,zhudaoren,yanshourenName,yanshourenID ) values ('" + htchild["PID"].ToString().Trim() + "','" + htchild["主导人ID"].ToString() + "','" + htchild["分管领导"].ToString() + "','" + htchild["分管领导ID"].ToString() +  "')";
            al.Add(StrChild);
        }

        if (al != null && al.Count > 0)
        {
            DbHelperSQL.ExecuteSqlTran(al);
            ispass = true;
        }
        return ispass;
    }





  
}