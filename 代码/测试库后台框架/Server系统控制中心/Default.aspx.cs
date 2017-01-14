using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            //DateTime PTCstartTime = DateTime.Now;
            DataSet state = Index_XTKZ.GetParameterInfo_XTKZ();
            string json = JsonConvert.SerializeObject(state);
            Response.Clear();
            Response.Write(json);
            //DateTime PTCSendTime2 = DateTime.Now;
            //string PTCStime2 = (PTCSendTime2 - PTCstartTime).ToString();
            //FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "json SQL-执行时间p：" + PTCStime2, null);  
            
        }
    }
}