using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;
using Hesion.Brick.Core;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_fmemail_zhuanhuan : System.Web.UI.Page
{
    DataSet dsmain = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {


        string upsql = "select Number,Employee_Name,'pinyin1'='a','pinyin2'='a','pinyin3'='a',BM,GWMC from HR_Employees";
        DataSet ds = DbHelperSQL.Query(upsql);

        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                try
                {
                    IEnumerator OperandEnum = ds.Tables[0].Rows[i]["Employee_Name"].ToString().Replace(" ","").GetEnumerator();   
                    int CharCount = 0;   
                    string new_string = "";
                    while( OperandEnum.MoveNext( ) )   
                    {   
                         CharCount++;   
                         new_string = new_string + CHS2PinYin.Convert(OperandEnum.Current.ToString()) + "|";
                     }

                    if (CharCount == 3)
                    {
                        string[] new_str_arr = new_string.Split('|');
                        for(int p = 0; p < new_str_arr.Length;p++)
                        {
                            new_string = new_str_arr[0] + new_str_arr[1].Substring(0, 1) + new_str_arr[2].Substring(0, 1);
                        }
                    }
                    if (CharCount == 2)
                    {
                        new_string = new_string.Replace("|","");
                    }


                    ds.Tables[0].Rows[i]["pinyin1"] = CHS2PinYin.Convert(ds.Tables[0].Rows[i]["Employee_Name"].ToString().Replace(" ", ""));
                    ds.Tables[0].Rows[i]["pinyin2"] = new_string;
                    ds.Tables[0].Rows[i]["pinyin3"] = CharCount.ToString();
                }
                catch
                {
                }
            }
            GridView1.DataSource = ds.Tables[0].DefaultView;
            GridView1.DataBind();
            dsmain = ds;
        }
        else
        {
            /*
           //查找重复的rtx帐号(即全拼重复)
           SELECT Number '邮箱'=FMYXZHBBHHZ, 'RTX'=RTXZH FROM 
           YXGHYSB 
           WHERE RTXZH  
           in ( SELECT RTXZH 
           FROM YXGHYSB 
           GROUP BY RTXZH HAVING count(RTXZH) >1 )
           //查找重复帐号的具体工号
           select * from YXGHYSB where RTXZH = 'hanmin' 
           */
            ;
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        for(int y = 0; y < dsmain.Tables[0].Rows.Count; y++)
        {
            string number = getNumber("YXGHYSB");
            string str_YGGH = dsmain.Tables[0].Rows[y]["Number"].ToString();
            string str_FMYXZHBBHHZ = dsmain.Tables[0].Rows[y]["pinyin2"].ToString();
            string str_RTXZH = dsmain.Tables[0].Rows[y]["pinyin1"].ToString();
            string str_CreateUser = "admin";
            string upsql = "insert into YXGHYSB (Number, YGGH, FMYXZHBBHHZ, RTXZH, CreateUser) values ('" + number + "', '" + str_YGGH + "', '" + str_FMYXZHBBHHZ + "', '" + str_RTXZH + "', '" + str_CreateUser + "')";
            DataSet dss = DbHelperSQL.Query(upsql);
        }
    }

    /// <summary>
    /// 获得编号
    /// </summary>
    /// <param name="module"></param>
    /// <returns></returns>
    private string getNumber(string module)
    {
        string number = null;
        WorkFlowModule wf = new WorkFlowModule(module);
        number = wf.numberFormat.GetNextNumber();
        return number;
    }
}
