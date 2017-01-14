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
using System.IO;
using FMOP.DB;
using System.Data.SqlClient;
using System.Text;
using System.Data.Common;
using CrystalDecisions.CrystalReports.Engine;

using FMOP.DB;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public partial class Web_HR_YGXX_Print : System.Web.UI.Page
{
    protected ReportDocument m_Doc = new ReportDocument();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
 
                //string number = Request["Number"].ToString();
                //string sql1= "select Number,Employee_Name,Employee_Sex,LS,BM,EJBMZ,GWMC,XL1,ZY1,Employee_EthnicGroup,XX,Employee_Party,convert(varchar(10),Employee_BirthDay,120) as Employee_BirthDay,BYYX1,JG,Employee_IDCardNo,XZZ,JTZZ,HKSZD,HYZK,SYZKNXTX,GRDH,JTDH,JJLXR,JJLXRDH,WYYZ,WYSP,JSJSP,convert(varchar(10),RZQPXQBDSJ,120) as RZQPXQBDSJ,convert(varchar(10),RZQPXQBDSJZ,120) as RZQPXQBDSJZ,convert(varchar(10),KCQKSSJ,120) as KCQKSSJ,convert(varchar(10),KCQKSSJZ,120) as KCQKSSJZ,BZGZ,convert(varchar(10),BHTKSSJ,120) as BHTKSSJ,convert(varchar(10),BHTDQSJ,120) as BHTDQSJ,BHTQX from HR_Employees where Number='" + Request["Number"].ToString() + "'";
                //SqlDataSource1.SelectCommand = sql1.ToString();
                //string sql2 = "select * from YGXX_ALL where Number='" + Request["Number"].ToString() + "' order by f6";
                //SqlDataSource2.SelectCommand = sql2.ToString();
                //string sql3 = "select * from YGXX_Picture where Number='" + Request["Number"].ToString() + "'";
                //SqlDataSource3.SelectCommand = sql3.ToString();


                //加载报表模板
                m_Doc.Load(Server.MapPath("../HR/YGXX_Print1.rpt"));
                //设置数据集
                DataSet dsall = new DataSet();
                DataTable dt1 = new DataTable();
                
                DataTable dt2 = new DataTable();
                
                DataTable dt3 = new DataTable();
                
                //获取照片数据集
                string sql1 = "select * from YGXX_Picture where Number='" + Request["Number"].ToString() + "'";
                DataSet ds1 = DbHelperSQL.Query(sql1);
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    if (ds1.Tables[0].Rows[i]["localname"].ToString() == "" || ds1.Tables[0].Rows[i]["localname"].ToString() .IndexOf ("showzp.aspx?zk=")>=0)
                    {
                        ds1.Tables[0].Rows[i]["localname"] = "thumberror.gif";
                    }
                    ds1.Tables[0].Rows[i]["tupian"] = ReadImage(Server.MapPath("../upload/" + ds1.Tables[0].Rows[i]["localname"].ToString()));                 
                }
                dt1 = ds1.Tables[0].Copy();
                dt1.TableName = "YGXX_Picture";
                dsall.Tables.Add(dt1);

                //获取任职资格整数之类的
                string sql2 = "select * from YGXX_ALL where Number='" + Request["Number"].ToString() + "' order by f6";
                DataSet ds2 = DbHelperSQL.Query(sql2);
                dt2 = ds2.Tables[0].Copy();
                dt2.TableName = "YGXX_ALL";
                dsall.Tables.Add(dt2);

                //获取基本信息
                string sql3 = "select Number,Employee_Name,Employee_Sex,LS,BM,EJBMZ,GWMC,XL1,ZY1,Employee_EthnicGroup,XX,Employee_Party,convert(varchar(10),Employee_BirthDay,120) as Employee_BirthDay,BYYX1,JG,Employee_IDCardNo,XZZ,JTZZ,HKSZD,HYZK,SYZKNXTX,GRDH,JTDH,JJLXR,JJLXRDH,WYYZ,WYSP,JSJSP,convert(varchar(10),RZQPXQBDSJ,120) as RZQPXQBDSJ,convert(varchar(10),RZQPXQBDSJZ,120) as RZQPXQBDSJZ,convert(varchar(10),KCQKSSJ,120) as KCQKSSJ,convert(varchar(10),KCQKSSJZ,120) as KCQKSSJZ,BZGZ,convert(varchar(10),BHTKSSJ,120) as BHTKSSJ,convert(varchar(10),BHTDQSJ,120) as BHTDQSJ,BHTQX from HR_Employees where Number='" + Request["Number"].ToString() + "'";
                DataSet ds3 = DbHelperSQL.Query(sql3);
                dt3 = ds3.Tables[0].Copy();
                dt3.TableName = "HR_Employees_JBXX";
                dsall.Tables.Add(dt3);

                m_Doc.SetDataSource(dsall);
                CrystalReportViewer1.ReportSource = m_Doc;
            

            //SqlDataSource1.DataBind();            
            //SqlDataSource2.DataBind();
            //SqlDataSource3.DataBind();           
            //CrystalReportSource1.DataBind();
        
        
    }




    public static byte[] ReadImage(string path)
    {
        FileStream stream = null;
        try
        {
            stream = File.OpenRead(path);
            return ReadImage(stream);
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
            }
        }
    }

    /**/
    /// <summary>
    /// 从给定的流中读取数据到一个字节数组中，并返回此数组。
    /// 如果给定的流不是一个图像格式的流，将报异常。
    /// 返回的字节数组中，将非BMP和JEPG格式的图像数据流转换为JEPG格式输出，以支持大多数应用。
    /// 适用于直接从数据库中读取的二进制图像流的处理。
    /// </summary>
    /// <param name="stream">给定的图像数据流。</param>
    /// <returns>从流中读取的数据。</returns>
    public static byte[] ReadImage(Stream stream)
    {
        System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
        byte[] myImage = null;

        if (image.RawFormat.Guid != ImageFormat.Jpeg.Guid && image.RawFormat.Guid != ImageFormat.Bmp.Guid)
        {
            MemoryStream memStream = new MemoryStream();
            image.Save(memStream, ImageFormat.Jpeg);
            myImage = memStream.GetBuffer();
            memStream.Close();
        }
        else
        {
            stream.Position = 0;
            myImage = new byte[stream.Length];
            stream.Read(myImage, 0, (int)stream.Length);
        }
        return myImage;
    }
}
