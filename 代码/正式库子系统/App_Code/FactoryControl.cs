using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using FMOP.DATE;
using FMOP.DDL;
using FMOP.FU;
using FMOP.FB;
using FMOP.IB;
using FMOP.TA;
using FMOP.TB;
using FMOP.STABLE;
using FMOP.MAKE;
using FMOP.TagCode;
using FMOP.BUTTON;
using FMOP.XHelp;
using FMOP.Execute;
/// <summary>
/// FactoryControl 的摘要说明
/// </summary>
namespace FMOP.FACTORY
{
    public class FactoryControl
    {

        /// <summary>
        /// 操作方式
        /// </summary>
        private string operatingFlag;
        
        /// <summary>
        /// 操作方式
        /// </summary>
        public string OperatingFlag
        {
            set
            {
                operatingFlag = value;
            }
            get
            {
                return operatingFlag;
            }
        }
        /// <summary>
        /// 换行符
        /// </summary>
        private int CrLine = 0;
        /// <summary>
        /// 存放生成的HTML代码
        /// </summary>
        private string htmlCode = string.Empty;

        /// <summary>
        /// 添加的模块名
        /// </summary>
        private string moduleName;
        
        /// <summary>
        /// 模块名
        /// </summary>
        private string number = string.Empty;
        
        /// <summary>
        /// 模块名
        /// </summary>
        public string ModuleName
        {
            set
            {
                moduleName = value;
            }
            get
            {
                return moduleName;
            }
        }

        /// <summary>
        /// 单号
        /// </summary>
        public string Number
        {
            set
            {
                number = value;
            }
            get
            {
                return number;
            }
        }

        /// <summary>
        /// Dreamwever的验证对象
        /// </summary>
        private string validate = string.Empty;
 
        /// <summary>
        /// 默认的构造函数
        /// </summary>
        public FactoryControl()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 创建控件对象，并返回所有创建对象的HTML代码
        /// </summary>
        /// <param name="controlXml">所有控件的XML内容</param>
        /// <returns>HTML 控件代码</returns>
        public void ParseConfigFiles(XmlNodeList controlXml,string Html)
        {
            XmlNode nodeType =null;
            SubTable makeTable = new SubTable();
            string mouseColor = "";
            bool visable = true;
            int twidth = 0;
            //添加表头
            htmlCode = Html;
      //    htmlCode = makeTable.TableHead(htmlCode);
            CrLine = 0;
            int Columns = 0;
            //统计子表有多少个控件
            int subTotal = 0;
            for (int i = 0; i < controlXml.Count; i++)
            {
                if (controlXml[i].SelectSingleNode("type").InnerXml == "SubTable")
                 {
                     subTotal = subTotal + 1;
                 }
            }

            for (int index = 0; index < controlXml.Count; index++)
            {
                nodeType = controlXml[index].SelectSingleNode("type");
                if (nodeType.InnerXml == "SubTable")
                {
                    //加入子表的Caption
                    //MkCaption(controlXml[index], makeTable);s

                    //是否可见
                    visable = XMLHelper.GetSingleBool(controlXml[index], "visable", true);

                    if (controlXml[index].SelectSingleNode("caption")!= null && controlXml[index].SelectSingleNode("caption").InnerXml != "")
                    {
                        twidth = XMLHelper.GetSingleInt(controlXml[index], "twidth", 0);
                        //若可见,则显示
                        if (visable == true)
                        {
                            htmlCode = makeTable.TRAdd(htmlCode);
                            makeTable.LengType = 2;
                            htmlCode = makeTable.TDAdd(htmlCode);

                            //加入对宽度的解析 2011-03-16 刘杰,
                            htmlCode = makeTable.CaptionTable2(htmlCode, controlXml[index].SelectSingleNode("caption").InnerXml,twidth);
                            //原算法 2011-03-16
                            // htmlCode = makeTable.CaptionTable(htmlCode, controlXml[index].SelectSingleNode("caption").InnerXml);
                            htmlCode = makeTable.TDEnd(htmlCode);
                            htmlCode = makeTable.TREnd(htmlCode);
                        }
                        else
                        {
                            htmlCode = makeTable.TRAdd(htmlCode);
                            makeTable.LengType = 2;
                            htmlCode = makeTable.TDAdd(htmlCode);
                            //加入对宽度的解析 2011-03-16 刘杰
                            htmlCode = makeTable.CaptionTable2(htmlCode, controlXml[index].SelectSingleNode("caption").InnerXml, "display:none",twidth);
                            //原算法 2011-03-16
                            //htmlCode = makeTable.CaptionTable(htmlCode, controlXml[index].SelectSingleNode("caption").InnerXml,"display:none");
                            htmlCode = makeTable.TDEnd(htmlCode);
                            htmlCode = makeTable.TREnd(htmlCode);
                        }
                    }

                    makeTable.SubTable = controlXml[index].SelectSingleNode("name").InnerXml;

                    //加入嵌入表格
                    AddNestingTableStart(makeTable,visable);

                    //添加表头部分
                    AddSubTableHead(controlXml[index]);

                    //添加具体内容
                    AddSubTableMaster(controlXml[index], makeTable.SubTable);

                    //产生Dreamwever验证方法
                    CreateVlidate(validate, makeTable.SubTable);

                    //判断是否为修改页面
                    if (OperatingFlag == "Update")
                    {
                        //判断子表是否存在内容,若是存在，则显示
                        SetSubTableValue(controlXml[index], makeTable.SubTable);
                    }

                    //结束嵌入表格
                    AddNestingTableEnd(makeTable);
                    
                    //添加子表的鼠标移动变色
                    mouseColor = "<script type=\"text/javascript\" language=\"JavaScript\">tigra_tables('" + makeTable.SubTable + "', 1, 0, '#F3F3F3', '#ffffff', '#F1F6F9', '#E0EFF2');</script>";
                    htmlCode = setAddressChar("</form>", mouseColor, htmlCode);

                    Columns = 2;
                }
                else
                {
                    //判断类型,若为TextArea,则一行只显示一个
                    if (nodeType.InnerXml == "MutiLineTextBox" || nodeType.InnerXml == "AddOnFiles" || CrLine % 2 == 0 || nodeType.InnerXml == "CheckBoxs")
                    {
                        if (CrLine != 0)
                        {
                            if (!isExistsEndTR("/tr>\r\n"))
                            {
                                if (nodeType.InnerXml == "MutiLineTextBox" || nodeType.InnerXml == "AddOnFiles" || Columns % 2 == 0 || nodeType.InnerXml == "CheckBoxs")
                                {
                                    htmlCode = makeTable.TREnd(htmlCode);
                                }
                            }
                        }

                        if (nodeType.InnerXml == "MutiLineTextBox" || nodeType.InnerXml == "AddOnFiles" || Columns % 2 == 0)
                        {
                            htmlCode = makeTable.TRAdd(htmlCode);
                        }

                        if (nodeType.InnerXml == "MutiLineTextBox" || nodeType.InnerXml == "AddOnFiles" || nodeType.InnerXml == "CheckBoxs")
                        {
                            Columns = Columns + 2;
                            makeTable.LengType = 2;
                        }
                        else
                        {
                            Columns = Columns + 1;
                            makeTable.LengType = 1;
                        }
                    }
                    else
                    {
                        if (isExistsEndTR("/tr>\r\n"))
                        {
                            if (Columns % 2 == 0 && Columns != 0 || nodeType.InnerXml == "MutiLineTextBox" || nodeType.InnerXml == "AddOnFiles" || nodeType.InnerXml == "CheckBoxs")
                            {
                                htmlCode = makeTable.TRAdd(htmlCode);
                            }
                            if (nodeType.InnerXml == "MutiLineTextBox" || nodeType.InnerXml == "AddOnFiles" || nodeType.InnerXml == "CheckBoxs")
                            {
                                Columns = Columns + 2;
                            }
                            else
                            {
                                Columns = Columns + 1;
                            }
                        }
                        else
                        {
                            if (nodeType.InnerXml == "MutiLineTextBox" || nodeType.InnerXml == "AddOnFiles" || nodeType.InnerXml == "CheckBoxs")
                            {
                                Columns = Columns + 2;
                            }
                            else
                            {
                                Columns = Columns + 1;
                            }
                        }
                        makeTable.LengType = 1;
                        makeTable.TdWidth = "50%";
                    }

                    //设定单元格宽度
                    #region 隐藏
                    if (CrLine != controlXml.Count - subTotal)
                    {
                        makeTable.TdColsPan = "1";
                    }
                    htmlCode = makeTable.TDAdd(htmlCode);
                    distributeControl(nodeType.InnerXml, controlXml[index], htmlCode, 0);
                    htmlCode = makeTable.TDEnd(htmlCode);
                    #endregion
                    CrLine += 1; //计数器
                    //判断，若满足以下三个条件即添加</TR>,1:一行有两个基本控件时. 或当类型为TextArea,或类型为File时, 3:是循环中最后一个控件时,4 若
                    if (((CrLine % 2 == 0 && CrLine != 0)||(Columns % 2 == 0 && Columns != 0)) || ((nodeType.InnerXml == "MutiLineTextBox") || (CrLine == controlXml.Count - subTotal) || (nodeType.InnerXml == "CheckBoxs")|| (nodeType.InnerXml == "AddOnFiles")))
                    {
                        if ((Columns % 2 == 0 && Columns != 0) || (nodeType.InnerXml == "MutiLineTextBox" || nodeType.InnerXml == "AddOnFiles"))
                        {
                            htmlCode = makeTable.TREnd(htmlCode);
                            Columns = 0;
                        }
                    }
                }
            }

            //如果为修改状态，则设主键编号为只读
            if (operatingFlag == "Update")
            {
                htmlCode = htmlCode.Replace("//设为只读", "setReadOnlyNumber();\r\n");
            }

            //添加控件
            AddSubMit(makeTable);

            if (moduleName == "ZS_DLSDD")
            {
                StringBuilder str = new StringBuilder("");
                str.Append("if(document.getElementById('sprytextfieldZS_DLSDDLBHJJE')!= null && document.getElementById('sprytextfieldZKHJEZJ')!=null)\r\n");
                str.Append("{");
                str.Append("document.getElementById('sprytextfieldZS_DLSDDLBHJJE').style.display='none';\r\n");
                str.Append("document.getElementById('sprytextfieldZKHJEZJ').style.display='none';\r\n");
                str.Append("}\r\n");

                htmlCode = htmlCode.Replace("//设为隐藏", str.ToString());
            }

        }

        /// <summary>
        /// 主表控件生成
        /// </summary>
        /// <param name="Type">类型</param>
        /// <param name="controlNode">节点</param>
        /// <param name="makeHtml">Html文档</param>
        /// <returns></returns>
        private void distributeControl(string Type, XmlNode controlNode, string makeHtml,int Flag)
        {
            Controls control = null;
            switch (Type)
            {
                case "TextBox":
                    control = new Textbox(controlNode,Flag);
                    if (operatingFlag == "Update")
                    {
                        control.DefaultValue = GetControlDefault(controlNode, "Master", "");
                    }
                    control.ModuleName = moduleName;
                    htmlCode = control.GetHtml(makeHtml, controlNode);
                    break;
                case "DropDownList":
                    control = new Dropdownlist(controlNode, Flag);
                    if (operatingFlag == "Update")
                    {
                        control.DefaultValue = GetControlDefault(controlNode, "Master", "");
                    }
                    control.ModuleName = moduleName;
                    htmlCode = control.GetHtml(makeHtml, controlNode);
                    break;
                case "DateSelectBox":
                    control = new DateControl(controlNode,Flag);
                    if (operatingFlag == "Update")
                    {
                        control.DefaultValue = GetControlDefault(controlNode, "Master", "");
                    }
                    control.ModuleName = moduleName;
                    htmlCode = control.GetHtml(makeHtml, controlNode);
                    break;
                case "FloatBox":
                    control = new FloatBox(controlNode, Flag);
                    if (operatingFlag == "Update")
                    {
                        control.DefaultValue = GetControlDefault(controlNode, "Master", "");
                    }
                    control.ModuleName = moduleName;
                    htmlCode = control.GetHtml(makeHtml, controlNode);
                    break;
                case "IntBox":
                    control = new IntBox(controlNode, Flag);
                    if (operatingFlag == "Update")
                    {
                        control.DefaultValue = GetControlDefault(controlNode, "Master", "");
                    }
                    control.ModuleName = moduleName;
                    htmlCode = control.GetHtml(makeHtml, controlNode);
                    break;
                case "AddOnFiles":
                    control = new FileUp(controlNode, Flag);
                    control.ModuleName = moduleName;
                    htmlCode = control.GetHtml(makeHtml, controlNode);
                    if (operatingFlag == "Update")
                    {
                        htmlCode = CreateIFrame(htmlCode);
                    }
                    break;
                case "MutiLineTextBox":
                    control = new TextArea(controlNode, Flag);
                    control.LengType = 2;
                    if (operatingFlag == "Update")
                    {
                        control.DefaultValue = GetControlDefault(controlNode, "Master", "");
                    }
                    control.ModuleName = moduleName;
                    control.OperatorFlag = operatingFlag;
                    htmlCode = control.GetHtml(makeHtml, controlNode);
                    break;

                case "CheckBoxs":
                    control = new Checkboxlist(controlNode, Flag);
                    control.LengType = 2;
                    if (operatingFlag == "Update")
                    {
                        control.DefaultValue = GetControlDefault(controlNode, "Master", "");
                    }
                    control.ModuleName = moduleName;
                    control.OperatorFlag = operatingFlag;
                    htmlCode = control.GetHtml(makeHtml, controlNode);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 子表控件的生成
        /// </summary>
        /// <param name="Type">类型</param>
        /// <param name="controlNode">节点</param>
        /// <param name="makeHtml">传入Html代码</param>
        /// <param name="Flag">区分主表与子表的标志</param>
        /// <param name="field">传递弹出窗口的字段名</param>
        /// <param name="module">传递弹出窗口的模块名</param>
        /// <param name="subName">传递弹出窗口的子表名</param>
        private void distributeSubControl(string Type, XmlNode controlNode, string makeHtml, int Flag,string subName)
        {
            Controls control = null;
            switch (Type)
            {
                case "TextBox":
                    control = new Textbox(controlNode, Flag);
                    refControl(control, makeHtml, controlNode, subName);
                    break;
                case "DropDownList":
                    control = new Dropdownlist(controlNode, Flag);
                    refControl(control, makeHtml, controlNode, subName);
                    break;
                case "DateSelectBox":
                    control = new DateControl(controlNode, Flag);
                    refControl(control, makeHtml, controlNode, subName);
                    break;
                case "FloatBox":
                    control = new FloatBox(controlNode, Flag);
                    refControl(control, makeHtml, controlNode, subName);
                    break;
                case "IntBox":
                    control = new IntBox(controlNode, Flag);
                    refControl(control, makeHtml, controlNode, subName);
                    break;
                case "AddOnFiles":
                    control = new FileUp(controlNode, Flag);
                    refControl(control, makeHtml, controlNode, subName);
                    break;
                case "MutiLineTextBox":
                    control = new TextArea(controlNode, Flag);
                    control.OperatorFlag = operatingFlag;
                    control.LengType = 2;
                    refControl(control, makeHtml, controlNode, subName);
                    break;
                default:
                    break;
            }
        }
       
        /// <summary>
        /// 返回组件所产生HTML代码
        /// </summary>
        /// <returns></returns>
        public string GetHtml()
        {
            return htmlCode;
        }

        /// <summary>
        /// 添加子表的表头 : 例:<tr><td>订单号</td><td>客户名称</td><td>公司地址</td><td>联系电话</td></tr>
        /// </summary>
        /// <returns></returns>
        public void AddSubTableHead(XmlNode itemNode)
        { 
            SubTable tb = new SubTable();
            htmlCode = tb.subTRAdd(htmlCode);

            //设置表格宽度
            tb.TdWidth = GetTdWidth(itemNode);
            foreach (XmlNode subNode in itemNode.SelectNodes("subTable/Sub-Field"))
            {
                tb.TDText = XMLHelper.GetSingleString(subNode, "caption");
                htmlCode = tb.subTdHead(htmlCode, tb.TDText);
            }
            //添加操作列
            tb.TDText = "执行操作";
            htmlCode = tb.subTdHead(htmlCode, tb.TDText);
            htmlCode = tb.TREnd(htmlCode);
        }

        /// <summary>
        /// 添加子表的主体部分，产生子表内容
        /// </summary>
        /// <param name="itemNode"></param>
        /// <returns></returns>
        public void AddSubTableMaster(XmlNode itemNode,string subName)
        {
            SubTable tb = new SubTable();
            ClientButton btn = new ClientButton();
            string  nodeType = string.Empty;
            string paramStr = string.Empty;
            string checkRule = string.Empty;    //控件的检查规则
            string controlName = string.Empty;  //控件的名称 
            bool  isEmpty = false;               //是否为空
            string maxValue = string.Empty;     //最大值
            string minValue = string.Empty;     //最小值
            string length = string.Empty;       //长度
            htmlCode = tb.TRAdd(htmlCode);
            
            //设置表格中每个单元格的宽度
            tb.TdWidth = GetTdWidth(itemNode);
            //创建子表控件 （子表中的节点在Data-Field下还有一层Sub-Field)
            foreach (XmlNode subNode in itemNode.SelectNodes("subTable/Sub-Field"))
            {
                tb.Style = XMLHelper.GetSingleString(subNode, "Style", "");

                //*****添加子表的检验规则*****/
                controlName = subName + XMLHelper.GetSingleString(subNode, "name");
                nodeType = XMLHelper.GetSingleString(subNode, "type");
                maxValue = XMLHelper.GetSingleString(subNode, "maxValue","");
                minValue = XMLHelper.GetSingleString(subNode, "minValue","");
                isEmpty = XMLHelper.GetSingleBool(subNode, "canNull",false);
                length = XMLHelper.GetSingleString(subNode, "length","");

                checkRule = checkRule + controlName + ":" + nodeType + ":" + isEmpty + ":" + maxValue + ":" + minValue + ":" + length + ".";
                /****************************/
                //指定添加TD中的内容
                htmlCode = tb.TDAdd(htmlCode);
                paramStr = paramStr + controlName + ",";
                distributeSubControl(nodeType, subNode, htmlCode, 1, subName);
                htmlCode = tb.TDEnd(htmlCode);
            }
            checkRule = checkRule.Substring(0, checkRule.LastIndexOf("."));
            //去掉最后多余的,
            paramStr = paramStr.Substring(0, paramStr.LastIndexOf(","));
            
            //添加确定按钮
            htmlCode = tb.TDAdd(htmlCode);

            //产生子表的处理操作(galxy 修改2010.10.19==========================添加checkRule=======)
            htmlCode = createSubJsCode(paramStr, subName, htmlCode, checkRule);
            btn.JSClik = subName+"addTableRow('"+paramStr+"','"+ subName +"','"+checkRule+"');";    //传递参数: paramStr:传递到前台的各控件名称， 最后一个参数:存放Table的div名称,添加验证规则  
            htmlCode = btn.GetButton(htmlCode);// htmlCode = btn.GetHtml(htmlCode,null);
            htmlCode = tb.TDEnd(htmlCode);
            //子表结束
            htmlCode = tb.TREnd(htmlCode);
        }

        /// <summary>
        /// 添加Submit控件
        /// </summary>
        private void AddSubMit(SubTable makeTable)
        {
            htmlCode = makeTable.TRAdd(htmlCode);
            makeTable.LengType = 2;
            htmlCode = makeTable.TDAdd(htmlCode);
            ClientButton btn = new ClientButton();
            btn.Name = "btnSubmit";
            btn.JSClik = "return submitForm();";
            btn.DefaultValue = "确定";
            htmlCode = btn.MakeSubmitButton(htmlCode);
            htmlCode = makeTable.TDEnd(htmlCode);
            htmlCode = makeTable.TREnd(htmlCode);
        }

        /// <summary>
        /// 添加内嵌表格开始部分
        /// </summary>
        private void AddNestingTableStart(SubTable tbl,bool visable)
        {
            htmlCode = tbl.TRAdd(htmlCode);   //添加 <TR>
            tbl.TdColsPan = "2";
            htmlCode = tbl.TDAdd(htmlCode);   //添加 <TD>
            htmlCode = tbl.StartDiv(htmlCode);

            if (visable == true)
            {
                htmlCode = tbl.TableHead(htmlCode); //原
            }
            else
            {
                htmlCode = tbl.TableHead(htmlCode,"display:none");
            }
        }

        /// <summary>
        /// 添加内嵌表格结束部分
        /// </summary>
        /// <param name="tbl"></param>
        private void AddNestingTableEnd(SubTable tbl)
        {
            htmlCode = tbl.TableEnd(htmlCode);
            htmlCode = tbl.EndDiv(htmlCode);
            htmlCode = tbl.TDEnd(htmlCode);   //添加 </TD>
            htmlCode = tbl.TREnd(htmlCode);   //结束 </TR>
        }

        /// <summary>
        /// 在指定位置添加字符串
        /// </summary>
        /// <param name="strSplit">分隔符</param>
        /// <param name="strChild">子串</param>
        /// <param name="strSource">源串</param>
        /// <returns></returns>
        public string setAddressChar(string strSplit, string strChild, string strSource)
        {
            string[] strArray = Regex.Split(strSource, strSplit, RegexOptions.IgnoreCase);
            strSource = strArray[0];
            strSource += strChild + strSplit;
            strSource += strArray[1];
            return strSource;
        }

        /// <summary>
        /// 产生动态处理子表的JS代码
        /// </summary>
        /// <param name="paramlist">子表控件名称的列表</param>
        /// <param name="subName">子表名</param>
        /// <returns></returns>
        public string createSubJsCode(string paramStr, string subName, string html, string createSubJsCode)
        {   int index = 0;
            StringBuilder subCode = new StringBuilder();
            //产生javaScript动态数组,存放子表控件的值
            if (paramStr != "")
            {
                index = paramStr.Split(',').Length;
                string[] subControl = new string[index];
                for (int i = 0; i < index; i++)
                {
                    subControl[i] = paramStr.Split(',')[i].ToString()+"Array";
                }

                subCode.Append("<script type=\"text/javascript\">\r\n");
                subCode.Append("<!--\\\\存放子表:" + subName + "中控件数据的数组\r\n");

                //定义数组
                for (int i = 0; i < subControl.Length; i++)
                {
                    subCode.Append("\tvar " + subControl[i] +" = new Array();\r\n");
                }

                //添加保存子表表结构的变量
                subCode.Append(jsCreateTableHead(subName));
                //添加新增子表时的处理方法
                subCode.Append(jsrefreshDiv(subName));
                subCode.Append(jsGetHJJE(subName));
                subCode.Append(jsSetHiddenValue(subControl, subName));
                subCode.Append(jsDeleteRow(subControl,subName));
                //添加两个方法调用，用于动态生成保存按钮和保存事件==========by galaxy 2010.10.19==================
                subCode.Append(jsEditSubRow(subControl, subName, paramStr));
                subCode.Append(jssavemysubedit(subControl, subName, paramStr, createSubJsCode));
                //===============================================================================
                subCode.Append(jsGetDataHtml(subControl,subName));
                subCode.Append(jsAddTableRow(subControl, subName, paramStr));
                subCode.Append("--></script>\r\n");

                //把产生的js代码添加在</body>之前
                htmlCode = setAddressChar("</body>", subCode.ToString(), htmlCode);
            }
            return htmlCode;
        }

        /// <summary>
        /// 计算子表中存在合计金额的情况
        /// </summary>
        /// <param name="subName"></param>
        /// <returns></returns>
        public string jsGetHJJE(string subName)
        {
            StringBuilder tableJs = new StringBuilder();
            string strFieldHid = subName + "JE_hid"; 
            tableJs.Append("\tfunction "+ subName+"getHJJE(tbName)\r\n");
            tableJs.Append("\t{\r\n");
            tableJs.Append("\t\tvar fieldName=\"\";\r\n");
            tableJs.Append("\t\tvar strJE;\r\n");
            tableJs.Append("\t\tvar HJJE=0;\r\n");
            tableJs.Append("\t\tvar JEarray = new Array();\r\n");

            //判断合计是否存在
            tableJs.Append("\t\t var strfield =\"" + subName +"\" + \"HJ\";\r\n");
            tableJs.Append("\t\tif(document.getElementById(strfield) != null)\r\n");
            tableJs.Append("\t\t{\r\n");
            tableJs.Append("\t\t\tfieldName = strfield;\r\n");
            tableJs.Append("\t\t}\r\n");
            tableJs.Append("\t\telse{\r\n");
            tableJs.Append("\t\t\tfieldName=\"\";\r\n");
            tableJs.Append("\t\t}\r\n");

            //判断合计金额是否存在
            tableJs.Append("\t\tif(fieldName == \"\")\r\n");
            tableJs.Append("\t\t{\r\n");
            tableJs.Append("\t\t\tstrfield =\"" + subName + "\" + \"HJJE\";\r\n");
            tableJs.Append("\t\t\tif(document.getElementById(strfield) != null)\r\n");
            tableJs.Append("\t\t\t{\r\n");
            tableJs.Append("\t\t\t\tfieldName = strfield;\r\n");
            tableJs.Append("\t\t\t}\r\n");
            tableJs.Append("\t\t}\r\n");
            
            tableJs.Append("\t\tif(fieldName != \"\")\r\n");
            tableJs.Append("\t\t{\r\n");
            tableJs.Append("\t\t\tif(document.getElementById('"+strFieldHid+"') != null)\r\n");
            tableJs.Append("\t\t\t{\r\n");
            tableJs.Append("\t\t\t\tstrJE = document.getElementById('" + strFieldHid + "').value;\r\n");

            tableJs.Append("\t\t\t\tJEarray = strJE.split(',');\r\n");
            tableJs.Append("\t\t\t\tfor(var i=0;i<JEarray.length;i++)\r\n");
            tableJs.Append("\t\t\t\t{\r\n");
            tableJs.Append("\t\t\t\t\tif(JEarray[i] != \"\")\r\n");
            tableJs.Append("\t\t\t\t\t{\r\n");
            tableJs.Append("\t\t\t\t\t\tHJJE += parseFloat(JEarray[i]);\r\n");
            tableJs.Append("\t\t\t\t\t}\r\n");
            tableJs.Append("\t\t\t\t}\r\n");
            tableJs.Append("\t\t\t\tdocument.getElementById(fieldName).value = HJJE;\r\n");
            tableJs.Append("\t\t\t}\r\n");
            tableJs.Append("\t\t}\r\n");
            tableJs.Append("\t}\r\n");
            return tableJs.ToString();
        }

        /// <summary>
        /// 在前台JS中保存Table 结构
        /// </summary>
        /// <param name="subName">子表名</param>
        /// <returns></returns>
        public string jsCreateTableHead(string subName)
        {
            StringBuilder tableHead = new StringBuilder();
            //产生一个保存表头和控件的前台变量
            tableHead.Append("\tvar " + subName + "StartLine;\r\n");
            tableHead.Append("\t" + subName + "StartLine=\"\";\r\n");
            tableHead.Append("\t" + subName + "StartLine=GetTableTag('" + subName + "');\r\n");  //产生表头
            tableHead.Append("\t" + subName + "StartLine +=\"<tr>\";\r\n");
            tableHead.Append("\t" + subName + "StartLine +=document.getElementById(\"" + subName + "\").rows(0).innerHTML;\r\n");
            tableHead.Append("\t" + subName + "StartLine +=\"</tr>\";\r\n");
            tableHead.Append("\t" + subName + "StartLine +=\"<tr>\";\r\n");
            tableHead.Append("\t" + subName + "StartLine +=document.getElementById(\"" + subName + "\").rows(1).innerHTML;\r\n");
            tableHead.Append("\t" + subName + "StartLine +=\"</tr>\";\r\n");
            return tableHead.ToString();
        }

        /// <summary>
        /// 执行新增时Table添加一行，并保存值
        /// </summary>
        /// <param name="subControl">子表控件集合</param>
        /// <param name="subName">子表名</param>
        /// <param name="paramStr">参数集合</param>
        /// <returns></returns>
        public string jsAddTableRow(string[] subControl, string subName, string paramStr)
        {

            StringBuilder jsCode = new StringBuilder();
            jsCode.Append(" function "+subName+"addTableRow(paramStr,subName,checkRule)\r\n");
            jsCode.Append("{\r\n");
            jsCode.Append("\tif (checkControl(checkRule) == true)\r\n");
            jsCode.Append("\t{\r\n");
            //把控件中的值加到数组中
            for (int i = 0; i < subControl.Length; i++)
            {

                jsCode.Append("\t\t" + subControl[i] + "[" + subControl[i] + ".length]=document.getElementById(\"" + paramStr.Split(',')[i] + "\").value.replaceAllts();\r\n");
            }

            jsCode.Append("\t\t" + subName + "refreshDiv();\r\n");
            jsCode.Append("\t\t"+ subName + "setHidden();\r\n");
            jsCode.Append("\t}\r\n");
            jsCode.Append("\t"+subName + "DWvalidate();\r\n");
            jsCode.Append("\t" + subName + "getHJJE(" + subName + ");\r\n"); //new
            jsCode.Append("}\r\n");
            return jsCode.ToString();
        }

        /// <summary>
        /// 组合子表内容，显示在页面中
        /// </summary>
        /// <param name="subName">子表名</param>
        /// <returns></returns>
        public string jsrefreshDiv(string subName)
        {
            StringBuilder jsCode = new StringBuilder();
            jsCode.Append("function "+subName+"refreshDiv()\r\n");
            jsCode.Append("{\r\n");
            jsCode.Append("\tvar divObj = document.getElementById('" + subName + "_div');\r\n");
            jsCode.Append("\tvar subCode;\r\n");
            jsCode.Append("\tsubCode =\"\";\r\n");
            jsCode.Append("\tsubCode=" + subName + "StartLine;\r\n");
            jsCode.Append("\tsubCode+= " + subName + "GetDataHtml();\r\n");
            jsCode.Append("\tsubCode +=\"</table>\";\r\n");
            jsCode.Append("\tdivObj.innerHTML = subCode;\r\n");
            jsCode.Append("\ttigra_tables('" + subName + "', 1, 0, '#F3F3F3', '#ffffff', '#F1F6F9', '#E0EFF2');");
            jsCode.Append("}\r\n");
            return jsCode.ToString();
        }

        /// <summary>
        /// 当新增或删除一行子表的数据时，更新子表<TR>内容
        /// </summary>
        /// <param name="subControl">子表控件集合</param>
        /// <param name="subName">子表名</param>
        /// <returns></returns>
        public string jsGetDataHtml(string[] subControl,string subName)
        {
            StringBuilder jsCode = new StringBuilder();
            jsCode.Append("function "+subName+"GetDataHtml()\r\n");
            jsCode.Append("{\r\n");

            jsCode.Append("\tvar result;\r\n");
            jsCode.Append("\tresult=\"\";\r\n");
            jsCode.Append("\tfor( i=0;i<" + subControl[0] + ".length; i++)\r\n");
            jsCode.Append("\t{\r\n");
            jsCode.Append("\t\tresult += \"<tr>\";\r\n");
            for (int i = 0; i < subControl.Length; i++)
            {
                jsCode.Append("\t\tresult += \"<td>\";\r\n");
                jsCode.Append("\t\tresult +=" + subControl[i] + "[i];\r\n");
                jsCode.Append("\t\tresult +=\"</td>\";\r\n");
            }

            jsCode.Append("\t\tresult += \"<td>\";\r\n");
            //===================galaxy添加2010.10.18，用于前台编辑子表
            jsCode.Append("\t\tresult += \"<input type=\\\"image\\\" id=\\\"" + subName + "jsEditRowBtn\\\" onclick=\\\"" + subName + "EditSubRow(\"+i+\");\\\" value=\\\"编辑\\\" src=\\\"images/edit_16x16.gif\\\"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;\";\r\n");
            //====================

            jsCode.Append("\t\tresult += \"<input type=\\\"image\\\" id=\\\"" + subName + "delBtn\\\" onclick=\\\"" + subName + "del(\"+i+\");\\\" value=\\\"删除\\\" src=\\\"images/deleteFile.gif\\\"/>\";\r\n");

            jsCode.Append("\t\tresult +=\"</td>\";\r\n");
            jsCode.Append("\t\tresult += \"</tr>\";\r\n");
            jsCode.Append("\t}\r\n");
            jsCode.Append("\treturn result;\r\n");
            jsCode.Append("}\r\n");
            return jsCode.ToString();
        }

        /// <summary>
        /// 处理子表的前台删除行的事件操作
        /// </summary>
        /// <param name="subControl">子表控件集合</param>
        /// <param name="subName">子表名</param>
        /// <returns></returns>
        public string jsDeleteRow(string[] subControl,string subName)
        {
            StringBuilder jsCode = new StringBuilder();
            jsCode.Append("function "+subName+"del(index)\r\n");
            jsCode.Append("{\r\n");
            for (int i = 0; i < subControl.Length; i++)
            {
                jsCode.Append("\t"+subControl[i] + ".splice(index,1);\r\n");
            }
            jsCode.Append("\t" + subName +"refreshDiv();\r\n");
            jsCode.Append("\t" + subName + "setHidden();\r\n");
            jsCode.Append("\t" + subName + "DWvalidate();\r\n");
            jsCode.Append("\t" + subName + "getHJJE(" + subName + ");\r\n"); //new
            jsCode.Append("}\r\n");
            return jsCode.ToString();
        }
        //========================================================================================
        /// <summary>
        /// 处理子表的前台编辑行的事件操作 galaxy添加  2010.10.18 用于添加编辑的js处理方法
        /// </summary>
        /// <param name="subControl">子表控件集合</param>
        /// <param name="subName">子表名</param>
        /// <returns></returns>
        public string jsEditSubRow(string[] subControl, string subName, string paramStr)
        {
            StringBuilder jsCode = new StringBuilder();
            jsCode.Append("function " + subName + "EditSubRow(index)\r\n");
            jsCode.Append("{\r\n");
            //for (int i = 0; i < subControl.Length; i++)
            //{
            //    jsCode.Append("\t" + subControl[i] + ".splice(index,1);\r\n");
            //}
            

            
            

            jsCode.Append("\t" + subName + "refreshDiv();\r\n");
            jsCode.Append("\t" + subName + "setHidden();\r\n");
            jsCode.Append("\t" + subName + "DWvalidate();\r\n");
            jsCode.Append("\t" + subName + "getHJJE(" + subName + ");\r\n"); //new

            //更改编辑框相关内容
            //把控件中的值加到数组中
            for (int i = 0; i < subControl.Length; i++)
            {
                //jsCode.Append("\t\t" + subControl[i] + "[" + subControl[i] + ".length]=document.getElementById(\"" + paramStr.Split(',')[i] + "\").value.replaceAllts();\r\n");

                jsCode.Append("\t\t" + "document.getElementById(\"" + paramStr.Split(',')[i] + "\").value = " + subControl[i] + "[index];" + "\r\n");
            }

            
            jsCode.Append("\t" + "var subbigtable = document.getElementById('" + subName + "');" + "\r\n");
            jsCode.Append("\t" + "var subbigtable_tbody = subbigtable.getElementsByTagName(\"tbody\")[0];" + "\r\n");
            jsCode.Append("\t" + "var edit_tr = subbigtable_tbody.rows[index+2];" + "\r\n");
            jsCode.Append("\t" + "var edit_tr_cell = edit_tr.cells(" + subControl.Length + ");" + "\r\n");
            jsCode.Append("\t" + "edit_tr_cell.innerHTML=edit_tr_cell.innerHTML + \"<div style=color:#FF0000>正在编辑中……</div>\";" + "\r\n");

            jsCode.Append("\t" + "var edit_tr_bnt = subbigtable_tbody.rows[1];" + "\r\n");
            jsCode.Append("\t" + "var edit_tr_cell_bnt = edit_tr_bnt.cells(" + subControl.Length + ");" + "\r\n");
            jsCode.Append("\t" + "edit_tr_cell_bnt.innerHTML = \"<input type='button' name='btn_edit' id='btn_edit' value='保存编辑' class='button' style='font-size:12px;color:#FF0000;width:60px;' onclick='" + subName + "savemysubedit(\"+index+\",1);' />\";" + "\r\n");
            jsCode.Append("\t" + "edit_tr_cell_bnt.innerHTML = edit_tr_cell_bnt.innerHTML + \"<input type='button' name='btn_edit_no' id='btn_edit_no' value='取消' class='button' style='font-size:12px;color:#FF0000;width:40px;' onclick='" + subName + "savemysubedit(\"+index+\",0);' />\";" + "\r\n");

            //下面三句话如果开启，编辑区域将移动到被编辑的条目下一行
            //jsCode.Append("\t" + "var newTr = subbigtable_tbody.insertRow(index+3);" + "\r\n");
            //jsCode.Append("\t" + "subbigtable_tbody.insertBefore(edit_tr_bnt.cloneNode(true),newTr);" + "\r\n");
            //jsCode.Append("\t" + "var newTr = subbigtable_tbody.deleteRow(1);" + "\r\n");
            /*
            jsCode.Append("\t" + "subbigtable_tbody.appendChild(add_tr.cloneNode(true));" + "\r\n");
            //jsCode.Append("\t" + "alert(add_tr.cloneNode(true).innerHTML);" + "\r\n");
            //jsCode.Append("\t" + "alert(document.getElementById('" + subName + "').rows(1).innerHTML);" + "\r\n");
            jsCode.Append("\t" + "alert(subbigtable_tbody.rows(3).innerHTML);" + "\r\n");
             * */
            
            jsCode.Append("}\r\n");
            return jsCode.ToString();
        }


        /// <summary>
        /// 执行保存编辑一行，并保存值
        /// </summary>
        /// <param name="subControl">子表控件集合</param>
        /// <param name="subName">子表名</param>
        /// <param name="paramStr">参数集合</param>
        /// <returns></returns>
        public string jssavemysubedit(string[] subControl, string subName, string paramStr, string createSubJsCode)
        {

            StringBuilder jsCode = new StringBuilder();
            jsCode.Append(" function " + subName + "savemysubedit(index,yeorno)\r\n");
            jsCode.Append("{\r\n");


            jsCode.Append("\tif (yeorno == 1)\r\n");
            jsCode.Append("\t{\r\n");
            jsCode.Append("\tif (checkControl(\"" + createSubJsCode + "\") == true)\r\n");
            jsCode.Append("\t{\r\n");
            //把控件中的值加到数组中
            for (int i = 0; i < subControl.Length; i++)
            {

                jsCode.Append("\t\t" + subControl[i] + "[index]=document.getElementById(\"" + paramStr.Split(',')[i] + "\").value.replaceAllts();\r\n");
            }

            jsCode.Append("\t\t" + subName + "refreshDiv();\r\n");
            jsCode.Append("\t\t" + subName + "setHidden();\r\n");
            jsCode.Append("\t" + subName + "DWvalidate();\r\n");
            jsCode.Append("\t" + subName + "getHJJE(" + subName + ");\r\n"); //new

            jsCode.Append("\t" + "var subbigtable = document.getElementById('" + subName + "');" + "\r\n");
            jsCode.Append("\t" + "var subbigtable_tbody = subbigtable.getElementsByTagName(\"tbody\")[0];" + "\r\n");
            jsCode.Append("\t" + "var edit_tr = subbigtable_tbody.rows[index+2];" + "\r\n");
            jsCode.Append("\t" + "var edit_tr_cell = edit_tr.cells(" + subControl.Length + ");" + "\r\n");
            jsCode.Append("\t" + "edit_tr_cell.innerHTML=edit_tr_cell.innerHTML + \"<div style=color:#FF0000>编辑完成</div>\";" + "\r\n");

            jsCode.Append("\t}\r\n");

            jsCode.Append("}\r\n");
            jsCode.Append("else\r\n");
            jsCode.Append("\t{\r\n");
            jsCode.Append("\t\t" + subName + "refreshDiv();\r\n");
            jsCode.Append("\t\t" + subName + "setHidden();\r\n");
            jsCode.Append("\t" + subName + "DWvalidate();\r\n");
            jsCode.Append("\t" + subName + "getHJJE(" + subName + ");\r\n"); //new
            jsCode.Append("}\r\n");


            jsCode.Append("}\r\n");
            return jsCode.ToString();
        }


        //========================================================================================

        /// <summary>
        /// 给隐藏控件赋值
        /// </summary>
        /// <param name="subControl">数组名称前端</param>
        /// <returns></returns>
        public string jsSetHiddenValue(string[] subControl,string subName)
        {
            StringBuilder jsCode = new StringBuilder();
            string hidName = "";
            jsCode.Append("function "+subName+"setHidden()\r\n");
            jsCode.Append("{\r\n");
            jsCode.Append("\tfor( i=0;i<" + subControl[0] + ".length; i++)\r\n");
            jsCode.Append("\t{\r\n");
            for (int i = 0; i < subControl.Length; i++)
            {
                hidName = subControl[i].ToString().Replace("Array","").ToString();
                hidName = hidName + "_hid";
                jsCode.Append("\t\tdocument.getElementById(\""+hidName+"\").value +=" + subControl[i] + "[i]+\",\";\r\n");
            }
            jsCode.Append("\t}\r\n");
            for (int i = 0; i < subControl.Length; i++)
            {
                hidName = subControl[i].ToString().Replace("Array", "").ToString();
                hidName = hidName + "_hid";
                jsCode.Append("\thidName =document.getElementById(\"" + hidName + "\");\r\n");
                //去掉最后一个字符
                jsCode.Append("\thidName.value = hidName.value.substring(0,hidName.value.length-1);\r\n");
            }
            jsCode.Append("}\r\n");
            return jsCode.ToString();
        }

        /// <summary>
        /// 取节点的控件名称,从 DB中查找相对应的值
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string GetControlDefault(XmlNode node, string Type, string subName)
        {
            string name = XMLHelper.GetSingleString(node, "name");
            string type = XMLHelper.GetSingleString(node, "type");
            string value = string.Empty;
            DataSet result = null;
            ExecuteDB.KeyNumber = Number;
            switch (Type)
            {
                case "Master":
                    ExecuteDB.modeName = moduleName;
                    result = ExecuteDB.GetSaleOrdersRecordset();
                    if (type != "AddOnFiles" && result != null)
                    {
                        foreach (DataRow rowIndex in result.Tables[0].Rows)
                        {
                            if (rowIndex[name] != null)
                            {
                                value = rowIndex[name].ToString();
                                return value;
                            }
                        }
                    }
                    break;
            }
            return value;
        }

        /// <summary>
        /// 若子表存在值，则给子表添加相应的行
        /// </summary>
        /// <param name="SubName"></param>
        private void SetSubTableValue(XmlNode itemNode, string SubName)
        {
            DataSet result = null;
            string columname = string.Empty;
            string controlName = string.Empty;
            string value = string.Empty;
            StringBuilder jsCode = new StringBuilder();

            result = ExecuteDB.GetSubRecordset(SubName);
            if (result != null && result.Tables[0].Rows.Count > 0)
            {
                jsCode.Append("<script type=\"text/javascript\">\r\n");
                jsCode.Append("<!--\r\n");
                jsCode.Append("function js" + SubName + "()\r\n");
                jsCode.Append("{\r\n");

                for (int i = 0; i < result.Tables[0].Rows.Count; i++)
                {
                    foreach (XmlNode subNode in itemNode.SelectNodes("subTable/Sub-Field"))
                    {
                        columname = XMLHelper.GetSingleString(subNode, "name");
                        controlName = SubName + columname + "Array";
                        jsCode.Append("\t" + controlName + "[" + controlName + ".length]='" + result.Tables[0].Rows[i][columname].ToString() + "';\r\n");
                    }
                }
                jsCode.Append("}\r\n");
                jsCode.Append("js" + SubName + "();\r\n");
                jsCode.Append(SubName + "refreshDiv();\r\n");
                jsCode.Append(SubName + "setHidden();\r\n");
                jsCode.Append(SubName + "DWvalidate();\r\n");
                jsCode.Append("--></script>\r\n");
                htmlCode = setAddressChar("</body>", jsCode.ToString(), htmlCode);
            }
        }

        /// <summary>
        /// 添加上传控件
        /// </summary>
        /// <param name="htmlCode">返回HTML代码</param>
        private string CreateIFrame(string htmlCode)
        {
            string Iframe = string.Empty;
            Iframe = @"<br/><iframe width='100%' height='45' frameborder='0' scrolling='no' src='UpFile.aspx?module="+ moduleName+"&number="+ Number +"'></iframe>";
            htmlCode = setAddressChar("</form>", Iframe, htmlCode);
            return htmlCode;
        }

        /// <summary>
        /// 产生DreamweverValidate验证
        /// </summary>
        /// <param name="validate"></param>
        /// <returns></returns>
        private void CreateVlidate(string validate,string SubName)
        {
            StringBuilder jsValidate = new StringBuilder();
            jsValidate.Append("<script type =\"text/javascript\">\r\n");
            jsValidate.Append("function " + SubName + "DWvalidate()\r\n");
            jsValidate.Append("{\r\n");
            jsValidate.Append(validate);
            jsValidate.Append("}\r\n");
            jsValidate.Append("</script>");
            htmlCode = setAddressChar("</form>", jsValidate.ToString(), htmlCode);
        }

        /// <summary>
        /// 创建control对象属性
        /// </summary>
        /// <param name="htmlControl"></param>
        /// <returns></returns>
        private void refControl(Controls control, string makeHtml, XmlNode controlNode, string subName)
        {
            control.SubTable = subName;
            control.Name = subName + control.Name;
            control.ModuleName = moduleName;
            htmlCode = control.GetHtml(makeHtml, controlNode);
            if (control.Eventhanding != "")
            {
                validate = validate + control.Eventhanding;
            }
        }

        /// <summary>
        ///  添加标题
        /// </summary>
        /// <param name="xnode">子表结构内容</param>
        /// <param name="subName">子表对象</param>
        private void MkCaption(XmlNode xnode, SubTable makeTable)
        {
            /**********************************/
            htmlCode = makeTable.TableHead(htmlCode);
            htmlCode = makeTable.TRAdd(htmlCode);
            makeTable.LengType = 2;
            htmlCode = makeTable.TDAdd(htmlCode, xnode.SelectSingleNode("caption").InnerXml);
            htmlCode = makeTable.TREnd(htmlCode);
            htmlCode = makeTable.TableEnd(htmlCode);
            /************************************/
        }

        /// <summary>
        /// 获取子表的每个单元格宽度+
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private string GetTdWidth(XmlNode node)
        {
            int Total = 0;
            string cellWidth = string.Empty;
            Total = node.SelectNodes("//subTable/Sub-Field").Count;
            if (Total != 0)
            {
                cellWidth = Convert.ToString((100 / (Total + 1))) + "%";
            }
            else
            {
                cellWidth = "25%";
            }
            return cellWidth;
        }

        /// <summary>
        /// 判断是否存在TR结束标记
        /// </summary>
        /// <returns></returns>
        private bool isExistsEndTR(string exstr)
        {
            string strSource = "";
            string[] strArray = Regex.Split(htmlCode, "</form>", RegexOptions.IgnoreCase);
            strSource = strArray[0];

            if (strSource.Substring(strSource.Length - 6, 6) == exstr)
            { 
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
