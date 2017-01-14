using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FMOP.CollectionNode;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using FMOP.SD;
using FMOP.Check;
using FMOP.Execute;
using FMOP.XParse;
using FMOP.DB;
using FMOP.XHelp;
using FMOP.FILE;
using Hesion.Brick.Core;
using Hesion.Brick.Core.WorkFlow;

public partial class actionOrder : System.Web.UI.Page
{
    SaleOrder saleOrder = new SaleOrder();

    /// <summary>
    /// 提示消息
    /// </summary>
    string Message = string.Empty;

    /// <summary>
    /// 首页加载
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                if (Request["module"] != null && Request["number"] != null)
                {
                    ViewState["module"] = Request["module"].ToString();
                    ViewState["Number"] = Request["number"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }

    /// <summary>
    /// 比对数据，存库
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            //判断对比是否通过　
            if (compareOrders())
            {
                SaveToDB();
            }

            //提醒程序运行消息...
            AlertMessage();
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            AlertMessage();
        }
    }

    /// <summary>
    /// 比较
    /// </summary>
    /// <returns></returns>
    private bool compareOrders()
    {
        bool isSuccess = false;

        //销售单主表所使用的字段
        string cust_ID = string.Empty;  //客户编号
        string currentlyDate = System.DateTime.Now.ToString("yyyy-MM-dd");  //当前日期
        //销售单子表所使用的子表
        string SPBM = string.Empty;         //商品编码
        DataSet XSD = new DataSet();        //订单子表
        DataSet XSD_Sub = new DataSet();    //订单主表
        DataSet bosomSet = new DataSet();   //计划内
        DataSet besidesSet = new DataSet(); //计划外
        int SL = 0;
        string SPMC = string.Empty;
        try
        {
            //取销售单表
            XSD = this.QueryGNKH_Order();

            //取销售单子表
            XSD_Sub = QueryGNKH_Orders();

            //设置销售单主表
            cust_ID = XSD.Tables[0].Rows[0]["CUST_ID"].ToString();

            if (XSD_Sub.Tables[0] == null || XSD_Sub.Tables[0].Rows.Count < 1)
            {
                Message = "订单中无产品预订信息";
                isSuccess = false;
                return isSuccess;
            }
            //循环订单子表,比对计划内和计划外
            foreach (DataRow dr in XSD_Sub.Tables[0].Rows)
            {
                SPBM = dr["SPBM"].ToString();
                //订单中的数量
                if (dr["GOODS_COUNT"].ToString() != "")
                {
                    SL = Convert.ToInt32(dr["GOODS_COUNT"].ToString());
                }
                SPMC = dr["GOODS_TYPE"].ToString();
                
                //执行对比 :条件 : 当前月份,spbm,客户编号
            
                //查询计划内
                bosomSet = saleOrder.getPlan_bosomSum(cust_ID, SPBM, currentlyDate);

                //查询计划外
                besidesSet = saleOrder.getPlan_besidesSum(cust_ID, SPBM, currentlyDate);

                if ((bosomSet.Tables[0] == null && besidesSet.Tables[0] == null) || (bosomSet.Tables[0].Rows.Count < 1 && besidesSet.Tables[0].Rows.Count < 1))
                {
                    isSuccess = false;
                    Message = "编号为:" + ViewState["Number"].ToString() + "的订单：不能产生销售单.\\n失败原因:商品名称为"+SPMC+"[" + SPBM.Trim() + "]的产品,不存在于直销业务月度产品经营计划表或直销业务计划外产品需求计划表中!";
                    return isSuccess;
                }
                else
                {

                    string bosomSum = string.Empty;
                    string besidesSum = string.Empty;
                    //查询计划内
                    bosomSum = getFiledValue(bosomSet);

                    //查询计划外
                    besidesSum = getFiledValue(besidesSet);

                    //计划内生产的数量
                    int bosom = 0;
                    if (bosomSum.Split(':')[1].ToString() != "")
                    {
                        bosom = Convert.ToInt32(bosomSum.Split(':')[1].ToString());
                    }
                    //计划外生产的数量
                    int besides = 0;

                    if (besidesSum.Split(':')[1] != "")
                    {
                        besides = Convert.ToInt32(besidesSum.Split(':')[1].ToString());
                    }

                    //计划内已销的数量
                    int bosomsaled = 0;
                    if (bosomSum.Split(':')[2].ToString() != "")
                    {
                        bosomsaled = Convert.ToInt32(bosomSum.Split(':')[2].ToString());
                    }

                    //计划外已销的数量
                    int besidesed = 0;
                    if (besidesSum.Split(':')[2].ToString() != "")
                    {
                        besidesed = Convert.ToInt32(besidesSum.Split(':')[2].ToString());
                    }

                    if (bosom + besides >= SL)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        Message = "编号为:" + ViewState["Number"].ToString() + "的订单,所需名称为:" + SPMC + "["+SPBM.Trim()+"]的产品数量不足,无法生成销售单!";
                        return false;
                    }
                }
            }
        }
        catch
        {
            Message = "系统出现错误!";
            isSuccess = false;
        }

        return isSuccess;
    }


    /// <summary>
    /// 执行存库操作
    /// </summary>
    private string SaveToDB()
    {
        string module = ViewState["module"].ToString();
        string masterCmd = string.Empty;
        string subCmd = string.Empty;

        List<CommandInfo> saveToDb = new List<CommandInfo>();
        List<CommandInfo> middleInfo = new List<CommandInfo>();
        List<CommandInfo> GlobalCmdInfo = new List<CommandInfo>();
        SqlParameter[] ParamersArray = null;

        //销售单主表所使用的字段
        string DDH = ViewState["Number"].ToString();  //订单号
        string cust_ID = string.Empty;  //客户编号
        string KHJL = string.Empty;     //客户经理
        string FKFS = string.Empty;     //付款方式
        string KHMC = string.Empty;     //客户名称
        string DZ = string.Empty;       //地址
        string LXR = string.Empty;      //联系人
        string LXDH = string.Empty;     //联系电话
        int SLZJ = 0;                   //数量总计
        double JEZJ = 0;                //金额总计
        string BZ = string.Empty;       //备注
        string FHR = string.Empty;      //发货人
        string THSHR = string.Empty;    //提交/送货人
        string currentlyDate = System.DateTime.Now.ToString("yyyy-MM-dd");  //当前日期

        //销售单子表所使用的子表
        string SPBM = string.Empty;     //商品编码
        string SPMC = string.Empty;     //商品名称
        string GGXH = string.Empty;     //规格型号
        string DW = string.Empty;       //单位
        string DJ = string.Empty;       //单价
        int SL = 0;                     //数量
        double JE = 0.0;                //金额

        string bosomSum = "";           //计划内已销数量
        string besidesSum = "";         //计划外已销数量
        string KeyNumber = "";          //存放生成销售单的主键

        DataSet XSD = new DataSet();        //订单子表
        DataSet XSD_Sub = new DataSet();    //订单主表
        DataSet bosomSet = new DataSet();   //计划内
        DataSet besidesSet = new DataSet(); //计划外

        try
        {
            WorkFlowModule WFM = new WorkFlowModule(module);
            KeyNumber = WFM.numberFormat.GetNextNumber();

            //取销售单表
            XSD = QueryGNKH_Order();

            //取销售单子表
            XSD_Sub = QueryGNKH_Orders();

            //设置销售单主表
            cust_ID = XSD.Tables[0].Rows[0]["CUST_ID"].ToString();
            KHMC = XSD.Tables[0].Rows[0]["CUST_NAME"].ToString();
            DZ = XSD.Tables[0].Rows[0]["ORDER_ADDRESS"].ToString();
            LXR = XSD.Tables[0].Rows[0]["LINKMAN_NAME"].ToString();
            LXDH = XSD.Tables[0].Rows[0]["LINKMAN_TEL"].ToString();
            BZ = XSD.Tables[0].Rows[0]["ORDER_REMARK"].ToString();

            KHJL = this.KHJL.Text;
            FKFS = this.FKFS.Text;
            FHR = this.FHR.Text;
            THSHR = this.THSHR.Text;
            DDH = ViewState["Number"].ToString();
            SLZJ = 0;
            JEZJ = 0;

            //循环订单子表,比对计划内和计划外
            foreach (DataRow dr in XSD_Sub.Tables[0].Rows)
            {
                SPBM = dr["SPBM"].ToString();
                SPMC = dr["GOODS_TYPE"].ToString();
                GGXH = dr["PRINT_NEEDS_STANDARD"].ToString();
                DW = dr["GOODS_UNIT"].ToString();
                DJ = dr["GOODS_PRICE"].ToString();

                //订单中的数量
                if (dr["GOODS_COUNT"].ToString() != "")
                {
                    SL = Convert.ToInt32(dr["GOODS_COUNT"].ToString());
                }

                //订单中的金额
                if (dr["GOODS_MONEY"].ToString() != "")
                {
                    JE = Convert.ToDouble(dr["GOODS_MONEY"].ToString());
                }

                //汇总数量
                SLZJ = SLZJ + SL;

                //汇总金额
                JEZJ = JEZJ + JE;

                //查询计划内
                bosomSet = saleOrder.getPlan_bosomSum(cust_ID, SPBM, currentlyDate);
                besidesSet = saleOrder.getPlan_besidesSum(cust_ID, SPBM, currentlyDate);

                //查询计划内
                bosomSum = getFiledValue(bosomSet);

                //查询计划外
                besidesSum = getFiledValue(besidesSet);

                if (checkStr(bosomSum) || checkStr(besidesSum))
                {
                    //更新已销数量
                   saveToDb = UpdateYXSL(bosomSum, besidesSum, SL);
                }
                else
                {
                    Message = "编号为:" + ViewState["Number"].ToString() + "的订单：不能产生销售单,\r\n失败原因:商品编号为" + SPBM + "的产品，不存在于直销业务月度产品经营计划表或直销业务计划外产品需求计划表中！";
                    return Message;
                }
                
                //添加新增的子表语句,参数
                subCmd = "INSERT INTO CSZX_XSD_HWLB(parentNumber,SPBM,SPMC,GGXH,DW,DJ,SL,JE)VALUES(@parentNumber,@SPBM,@SPMC,@GGXH,@DW,@DJ,@SL,@JE)";
                ParamersArray = new SqlParameter[8];
                
                ParamersArray[0] = new SqlParameter("@parentNumber", SqlDbType.VarChar,50);
                ParamersArray[0].Value = KeyNumber;

                ParamersArray[1] = new SqlParameter("@SPBM", SqlDbType.VarChar, 50);
                ParamersArray[1].Value = SPBM;

                ParamersArray[2] = new SqlParameter("@SPMC", SqlDbType.VarChar, 50);
                ParamersArray[2].Value = SPMC;

                ParamersArray[3] = new SqlParameter("@GGXH", SqlDbType.VarChar, 50);
                ParamersArray[3].Value = GGXH;

                ParamersArray[4] = new SqlParameter("@DW", SqlDbType.VarChar, 50);
                ParamersArray[4].Value = DW;

                ParamersArray[5] = new SqlParameter("@DJ", SqlDbType.Float);
                ParamersArray[5].Value = Convert.ToDouble(DJ);

                ParamersArray[6] = new SqlParameter("@SL", SqlDbType.Int);
                ParamersArray[6].Value = Convert.ToInt32(SL);

                ParamersArray[7] = new SqlParameter("@JE", SqlDbType.Float);
                ParamersArray[7].Value = Convert.ToDouble(JE);

                //添加一个子表到泛型对象中
                middleInfo.Add(MakeObj(subCmd,ParamersArray));

                ParamersArray = null;
            }

            //添加审核
            string NextChecker = "";

            //取审核人
            Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
            if (wf.check != null)
            {
                NextChecker = wf.check.GetFirstCheckRole(KeyNumber, User.Identity.Name);
            }
            else
            {
                NextChecker = "";
            }

            int checkState = 1;
            if (NextChecker != "")
            {
                checkState = 0;
            }

            //新增到销售单主表
            masterCmd = @"insert into CSZX_XSD(Number,DDH,KHJL,FKFS,KHBH,KHMC,DZ,LXR,LXDH,SLZJ,JEZJ,BZ,FHR,THSHR,NextChecker,CheckState,CreateUser,CreateTime)Values
                          (@Number,@DDH,@KHJL,@FKFS,@KHBH,@KHMC,@DZ,@LXR,@LXDH,@SLZJ,@JEZJ,@BZ,@FHR,@THSHR,@NextChecker,@checkState,@CreateUser,getdate())";
            SqlParameter[] MasterParamersArray = new SqlParameter[17];

            MasterParamersArray[0] = new SqlParameter("@Number", SqlDbType.VarChar, 50);
            MasterParamersArray[0].Value = KeyNumber;

            MasterParamersArray[1] = new SqlParameter("@DDH", SqlDbType.VarChar, 50);
            MasterParamersArray[1].Value = DDH;

            MasterParamersArray[2] = new SqlParameter("@KHJL", SqlDbType.VarChar, 50);
            MasterParamersArray[2].Value = KHJL;

            MasterParamersArray[3] = new SqlParameter("@FKFS", SqlDbType.VarChar, 50);
            MasterParamersArray[3].Value = FKFS;

            MasterParamersArray[4] = new SqlParameter("@KHBH", SqlDbType.VarChar, 50);
            MasterParamersArray[4].Value = cust_ID;

            MasterParamersArray[5] = new SqlParameter("@KHMC", SqlDbType.VarChar, 50);
            MasterParamersArray[5].Value = KHMC;

            MasterParamersArray[6] = new SqlParameter("@DZ", SqlDbType.VarChar, 50);
            MasterParamersArray[6].Value = DZ;

            MasterParamersArray[7] = new SqlParameter("@LXR", SqlDbType.VarChar, 50);
            MasterParamersArray[7].Value = LXR;

            MasterParamersArray[8] = new SqlParameter("@LXDH", SqlDbType.VarChar, 50);
            MasterParamersArray[8].Value = LXDH;

            MasterParamersArray[9] = new SqlParameter("@SLZJ", SqlDbType.Int, 6);
            MasterParamersArray[9].Value = SLZJ;

            MasterParamersArray[10] = new SqlParameter("@JEZJ", SqlDbType.Float);
            MasterParamersArray[10].Value = JEZJ;

            MasterParamersArray[11] = new SqlParameter("@BZ", SqlDbType.Text);
            MasterParamersArray[11].Value = BZ;

            MasterParamersArray[12] = new SqlParameter("@FHR", SqlDbType.VarChar, 50);
            MasterParamersArray[12].Value = FHR;

            MasterParamersArray[13] = new SqlParameter("@THSHR", SqlDbType.VarChar, 50);
            MasterParamersArray[13].Value = THSHR;

            MasterParamersArray[14] = new SqlParameter("@NextChecker", SqlDbType.VarChar, 50);
            MasterParamersArray[14].Value = NextChecker;

            MasterParamersArray[15] = new SqlParameter("@checkState", SqlDbType.SmallInt, 1);
            MasterParamersArray[15].Value = checkState;

            MasterParamersArray[16] = new SqlParameter("@CreateUser", SqlDbType.VarChar, 50);
            MasterParamersArray[16].Value = User.Identity.Name;
            
            //添加一个主表到泛型对象中
            GlobalCmdInfo.Add(MakeObj(masterCmd, MasterParamersArray));

            //把子表的泛型对象添加到主表泛型对象中
            GlobalCmdInfo = XCHGObject(GlobalCmdInfo,middleInfo);

            //把需更新表的泛型对象添加到主表泛型对象中
            GlobalCmdInfo = XCHGObject(GlobalCmdInfo, saveToDb);

            //执行数据库操作
            int rowsAffect = DbHelperSQL.ExecuteSqlTranWithIndentity(GlobalCmdInfo);
            if (rowsAffect >= 0)
            {
                Message = "产生销售单,成功!";
               
                //添加提醒
                string dailiren = "无";
                if (Session["daililogin"] != null)
                {
                    dailiren = Session["daililogin"].ToString();
                }
                Hesion.Brick.Core.FMEvengLog.SaveToLog(User.Identity.Name, module, Request.UserHostAddress, "WorkFlow_Add_Save.aspx", KeyNumber, "insert", dailiren);

                //产生提醒信息
                wf.warning.CreateWarningToRole(KeyNumber, 1, User.Identity.Name);

                //添加审核提醒
                string msg = string.Empty;
                msg = "单号为:" + KeyNumber + "的" + wf.property.Title + "已经填写完成,请尽快审核!";

                if (NextChecker != "")
                {
                    //添加审核提醒
                    wf.warning.CreateWarningToJobName(KeyNumber, msg, module, User.Identity.Name, NextChecker, 1);
                }

                //转向报表页面
                Response.Redirect("HomeClient/SaleOrderReport.aspx?Number=" + KeyNumber);
             }
        }
        catch
        {
            Message = "产生销售单,失败!";
        }

        return Message;
    }

    /// <summary>
    /// 信息提示
    /// </summary>
    private void AlertMessage()
    {
        MessageBox.Show(this, Message);
    }

    /// <summary>
    /// 查询所选择纪录的订单子表内容
    /// </summary>
    private DataSet QueryGNKH_Orders()
    {
        DataSet result = null;
        if (!ViewState["Number"].ToString().Equals(""))
        {
            string cmdText = "select * from GNKH_Orders WHERE GNKH_Orders.parentNumber='" + ViewState["Number"].ToString() + "'";
            result = DbHelperSQL.Query(cmdText);
        }
        return result;
    }

    /// <summary>
    /// 查询所选择纪录的订单表内容
    /// </summary>
    private DataSet QueryGNKH_Order()
    {
        DataSet result = null;
        if (!ViewState["Number"].ToString().Equals(""))
        {
            string cmdText = "select * from GNKH_Order where Number='" + ViewState["Number"].ToString() + "'";
            result = DbHelperSQL.Query(cmdText);
        }
        return result;
    }

    /// <summary>
    /// 计划内更新已销数量
    /// </summary>
    /// <param name="saveToDb">事务泛型</param>
    /// <param name="bosomSum">计划内的数量</param>
    /// <param name="saleOrderSum">销售单的数量</param>
    /// <returns></returns>
    public List<CommandInfo> UpdatePlan_bosomSum(List<CommandInfo> saveToDb, string keys, int sum)
    {

        CommandInfo comInfo = new CommandInfo();
        if (keys != "")
        {
            string cmdText = "update CSZX_YDCPJYJHB_JH set YXSL=@YXSL where id=@Number";
            SqlParameter[] ParamArray = new SqlParameter[2];
            ParamArray[0] = new SqlParameter("@Number", SqlDbType.Int, 6);
            ParamArray[0].Value = Convert.ToInt32(keys);

            ParamArray[1] = new SqlParameter("@YXSL", SqlDbType.Int, 6);
            ParamArray[1].Value = sum;

            comInfo = MakeObj(cmdText, ParamArray);
            saveToDb.Add(comInfo);
        }
        return saveToDb;
    }

    /// <summary>
    /// 计划外更新已销数量
    /// </summary>
    /// <param name="saveToDb">事务泛型</param>
    /// <param name="besidesSum">计划外产生的数量</param>
    /// <param name="saleOrderSum">销售单的数量</param>
    /// <returns></returns>
    public List<CommandInfo> UpdatePlan_besidesSum(List<CommandInfo> saveToDb, string keys, int sum)
    {
        CommandInfo comInfo = new CommandInfo();

        if (keys != "")
        {
            string cmdText = "update ZXYWJHWCPXQJHB set YXSL=@YXSL where id=@Number";
            SqlParameter[] ParamArray = new SqlParameter[2];
            ParamArray[0] = new SqlParameter("@Number", SqlDbType.Int, 6);
            ParamArray[0].Value = Convert.ToInt32(keys);

            ParamArray[1] = new SqlParameter("@YXSL", SqlDbType.Int, 6);
            ParamArray[1].Value = sum;

            comInfo = MakeObj(cmdText, ParamArray);
            saveToDb.Add(comInfo);
        }
        return saveToDb;
    }
    
    /// <summary>
    /// 设置月份为两位数字
    /// </summary>
    /// <param name="number">1-9之间的数字</param>
    /// <returns></returns>
    private string setNumber(string number)
    {
        if (Int32.Parse(number) > 0 && Int32.Parse(number) < 10)
        {
            number = "0" + number;
        }

        return number;
    }

    /// <summary>
    /// 获取字段值
    /// </summary>
    /// <param name="result">数据集</param>
    /// <returns></returns>
    private string getFiledValue(DataSet result)
    {
        string returnValue ="::";
        
        if (result != null && result.Tables[0] != null && result.Tables[0].Rows.Count > 0)
        {
            returnValue = result.Tables[0].Rows[0][0].ToString() + ":" + result.Tables[0].Rows[0][1].ToString() + ":" + result.Tables[0].Rows[0][2].ToString();
        }
        return returnValue;
    }

    /// <summary>
    /// 更新已销数量
    /// </summary>
    /// <param name="bosomSum"></param>
    /// <param name="besidesSum"></param>
    /// <param name="SL"></param>
    private List<CommandInfo> UpdateYXSL(string bosomSum, string besidesSum, int SL)
    {
        List<CommandInfo> saveToDb = new List<CommandInfo>();
        //计划内生产的数量
        int bosom = 0;
        if (bosomSum.Split(':')[1].ToString() != "")
        {
           bosom = Convert.ToInt32(bosomSum.Split(':')[1].ToString());
        }

        //计划外生产的数量
        int besides = 0;

        if (besidesSum.Split(':')[1] != "")
        {
            besides = Convert.ToInt32(besidesSum.Split(':')[1].ToString());
        }

        //计划内已销的数量
        int bosomsaled = 0;
        if (bosomSum.Split(':')[2].ToString() != "")
        {
            bosomsaled = Convert.ToInt32(bosomSum.Split(':')[2].ToString());
        }

        //计划外已销的数量
        int besidesed = 0;
        if (besidesSum.Split(':')[2].ToString() != "")
        {
            besidesed = Convert.ToInt32(besidesSum.Split(':')[2].ToString());
        }

        //if ((bosom - bosomsaled) + (besides - besidesed) >= saleOrders)
        if (bosom + besides >= SL)
        {

            if (bosom > SL)
            {
                //更新计划内
                saveToDb = UpdatePlan_bosomSum(saveToDb, bosomSum.Split(':')[0].ToString(), SL);

                //更新计划外
               // saveToDb = UpdatePlan_besidesSum(saveToDb, besidesSum.Split(':')[0].ToString(), 0);
            }
            else if (bosom == SL)
            {
                //更新计划内
                saveToDb = UpdatePlan_bosomSum(saveToDb, bosomSum.Split(':')[0].ToString(), bosom);

                //更新计划外
               // saveToDb = UpdatePlan_besidesSum(saveToDb, besidesSum.Split(':')[0].ToString(), 0);
            }
            else
            {

                //更新计划内
                saveToDb = UpdatePlan_bosomSum(saveToDb, bosomSum.Split(':')[0].ToString(), bosom);

                //更新计划外
                saveToDb = UpdatePlan_besidesSum(saveToDb, besidesSum.Split(':')[0].ToString(), SL - bosom);
            }
        }
        return saveToDb;
    }

    /// <summary>
    /// 验证字符串
    /// </summary>
    /// <param name="str"></param>
    private bool checkStr(string str)
    {
        bool ischecked = true;
        string[] str1 = str.Split(':');

        for (int i = 0; i < str1.Length; i++)
        {
            if (str1[i].ToString().Equals(""))
            {
                ischecked = false;
            }
        }

        return ischecked;
    }

    /// <summary>
    /// 创建泛型对象
    /// </summary>
    /// <param name="strSql"></param>
    /// <param name="parameterArray"></param>
    /// <param name="cmdType"></param>
    /// <returns></returns>
    public CommandInfo MakeObj(string strSql, SqlParameter[] parameterArray)
    {
        CommandInfo commInfo = new CommandInfo();
        commInfo.CommandText = strSql;
        commInfo.Parameters = parameterArray;
        commInfo.cmdType = CommandType.Text;
        return commInfo;
    }

    /// <summary>
    /// 把source 中的内容　放到 dest 中
    /// </summary>
    /// <param name="dest"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    private List<CommandInfo> XCHGObject(List<CommandInfo> goal, List<CommandInfo> source)
    {
        foreach (CommandInfo myDE in source)
        {
            goal.Add(myDE);
        }

        return goal;
    }
}
