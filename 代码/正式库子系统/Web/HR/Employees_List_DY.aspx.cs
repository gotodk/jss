using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using Galaxy.ClassLib.DataBaseFactory;
using Hesion.Brick.Core.WorkFlow;
using Infragistics.WebUI.UltraWebGrid;

public partial class Web_HR_Employees_List_DY : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //判断是否有权限
            DefinedModule Dfmodule = new DefinedModule("Employees_List_DY");
            Authentication auth = Dfmodule.authentication;
            if (auth == null)
            {
                //MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
                return;
            }

            if (!auth.GetAuthByUserNumber(User.Identity.Name).CanView)
            {
                //MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
                return;
            }

            string upsql9 = " select '工号'=Number,'姓名'=Employee_Name,'性别'=Employee_Sex,'出生日期'=Convert(varchar(10),Employee_BirthDay,120),'毕业院校'=BYYX1,'专业'=ZY1,'学历'=XL1,'入司时间'=Convert(varchar(10),RZQPXQBDSJ,120),'任职岗位'=GWMC,'任职状态'=YGZT,'转正时间'=Convert(varchar(10),ZZSJ,120),'党员关系状态'=Employee_Party,'目前党员关系归属地'=MQDYGXGSD,'婚否'=HYZK,'身份证号'=Employee_IDCardNo,'联系电话'=GRDH,'籍贯'=JG,'家庭住址(通讯地址)'=JTZZ from HR_Employees where Employee_Party in ('党员','预备党员') and   YGZT not like '%离职%' order by createTime DESC ";
            DataSet ds_dy = DbHelperSQL.Query(upsql9);
            UltraWebGrid1.DataSource = ds_dy;
            UltraWebGrid1.DataBind();
            this.UltraWebGrid1.DisplayLayout.ColFootersVisibleDefault = ShowMarginInfo.Yes;
            this.UltraWebGrid1.Columns[0].FooterText = "合计：";
            UltraWebGrid1.Columns[0].Footer.Total = SummaryInfo.Count;
        }
        
        
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        this.UltraWebGridExcelExporter1.DownloadName = "report.xls";
        this.UltraWebGridExcelExporter1.Export(this.UltraWebGrid1);
    }
}
