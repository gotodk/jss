using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Collections;
using FMOP.XHelp;
using System.Text.RegularExpressions;
/// <summary>
/// CreateControl 的摘要说明
/// </summary>

namespace FMOP.MAKE
{
    public abstract class Controls
    {
        #region 产生控件的属性
        public Controls()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 重载构造函数，给属性赋值
        /// </summary>
        /// <param name="itemNode">xml节点</param>
        /// <param name="Flag">是子表还是主表的判断.Flag =0:子表，Flag＝1 主表</param>
        public Controls(XmlNode itemNode,int FlagParamer)
        {
            if (itemNode != null)
            {
                //共用的属性写值
                Name = XMLHelper.GetSingleString(itemNode, "name");
                CName = Name; //新算法 加入不受干扰的CName liujie 2010-09-27
                if (FlagParamer == 0)
                {
                    Caption = XMLHelper.GetSingleString(itemNode, "caption");
                }
                Flag = FlagParamer;
                CanNull = XMLHelper.GetSingleBool(itemNode, "canNull", false); //已做空字符处理，并返回false
                IsInListTable = XMLHelper.GetSingleBool(itemNode, "isInListTable", true);
                Style = XMLHelper.GetSingleString(itemNode, "Style", "");

                if (itemNode["OnBlur"] != null)
                {
                    OnBlur = XMLHelper.GetSingleString(itemNode, "OnBlur"); //例: validate();
                }

                ReadOnly = XMLHelper.GetSingleBool(itemNode, "readonly", false);

                if (itemNode["defaultValue"] != null)
                {
                    DefaultValue = XMLHelper.GetSingleString(itemNode, "defaultValue");
                }
                //前台控制显示 刘杰 2010-09-20
                if (itemNode["isDisplayF"] != null)
                {
                    isDisplayF = XMLHelper.GetSingleBool(itemNode, "isDisplayF", false);
                }
                //是否有弹出窗口，和弹出窗口的SQL语句
                HasModelButton = XMLHelper.GetSingleBool(itemNode, "hasModelButton", false);
            }
        }

        //当前操作的模块名称
        private string moduleName = string.Empty;
        //子表名称
        private string subTable = string.Empty;
        //弹出窗体的字段名
        private string popField = string.Empty;

        private int flag = 0; 

        //控件名称
        private string name = string.Empty;
        //控件名称2   刘杰2010-09-27 不干扰的控件名
        private string cname = string.Empty;
        //控件前面显示的标题
        private string caption = string.Empty;
        //控件类型
        private string type = string.Empty;
        //控件显示样式
        private string style = string.Empty;
        //控件默认值
        private string defaultValue = string.Empty;
        //DropDownList类型(是静态还是动态)
        private string comboType = string.Empty;  
        //DropDownList的项
        private ArrayList Items; 
        //DropDownList的查询语句       
        private string comboSql = string.Empty;
        //绑定DropDownList的值
        private string comboBoxValueField = string.Empty;
        //在TextBox后面的弹出页面的SQL语句
        private string modelSelectSQL = string.Empty;
        //事件名称
        private string onblur = string.Empty;
        //控件是否为空
        private bool canNull = true ;
        //控件是否显示在列表中
        private bool isInListTable = true;
        //控件是否只读
        private bool readOnly = false ;
        //在任一控件后是否有一个弹出视察按钮
        private bool hasModelButton;
        //控件值长度
        private int length = 50;
        //textArea 每行的字符数
        private int cols = 20;
        //textArea 总共多少行.
        private int rows;
        //控件最大上限值
        private double maxValue;
        //控件最小上限值
        private double minValue;
        //产生事件
        public string eventOP;

        //TD内容
        private string tdText = string.Empty;
        //TD宽度
        private string tdWidth = string.Empty;
        //TD占单元格数
        private string tdColsPan = string.Empty;

        /// <summary>
        /// 返回验证
        /// </summary>
        private string eventhanding = string.Empty;

         
        //长度类型
        private int lengType = 1;
        //前台显示控制 刘杰 2010-09-20
        private bool isDisplayF = false;

        private string jsClick = string.Empty;
        public bool IsDisPlayF
        {
            get
            {
                return isDisplayF;
            }
            set
            {
                isDisplayF = value;
            }
        }

        public int LengType
        {
            get
            {
                return lengType;
            }
            set
            {
                lengType = value;
            }

        }

        /// <summary>
        /// 页面状态，判断新增还是修改
        /// </summary>
        private string operatorFlag = string.Empty;
        public string OperatorFlag
        {
            set
            {
                operatorFlag = value;
            }
            get
            {
                return operatorFlag;
            }
        }
        #endregion

        #region 属性集合的调用
        /// <summary>
        /// 当前页面所处的模块名称
        /// </summary>
        public string ModuleName
        {
            get
            {
                return moduleName;
            }
            set
            {
                moduleName = value;
            }
        }
        /// <summary>
        /// TEXT -名称
        /// </summary>
        public string Name
        {
            set
            {
                name = value;
            }
            get
            {
                return name;
            }
        }
        /// <summary>
        /// TEXT -不干扰的名称  刘杰2010-09-27
        /// </summary>
        public string CName
        {
            set
            {
                cname = value;
            }
            get
            {
                return cname;
            }
        }

        /// <summary>
        /// 弹出窗体的字段名
        /// </summary>
        public string PopField
        {
            set
            {
                popField = value;
            }
            get
            {
                return popField;
            }
        }

        public int Flag
        {
            set
            {
                flag = value;
            }
            get
            {
                return flag;
            }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;
            }
        }
        /// <summary>
        /// 控件类型
        /// </summary>
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        /// <summary>
        /// TEXT - 所使用的样式
        /// </summary>
        public string Style
        {
            get
            {
                return style;
            }
            set
            {
                style = value;
            }
        }
        /// <summary>
        /// 按下后的脚本事件
        /// </summary>
        public string OnBlur
        {
            get
            {
                return onblur;
            }
            set
            {
                onblur = value;
            }
        }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue
        {
            get
            {
                return defaultValue;
            }
            set
            {
                defaultValue = value;
            }

        }
        /// <summary>
        /// DropDownList类型
        /// </summary>
        public string ComboType
        {
            get
            {
                return comboType;
            }
            set
            {
                comboType = value;
            }
        }
        /// <summary>
        /// 创建用于绑定下拉列表的各项的方法
        /// </summary>
        public ArrayList Choose
        {
            set
            {
                Items = value;
            }
            get
            {
                return Items;
            }
        }
        /// <summary>
        /// 绑定DropDownList执行的SQL语句
        /// </summary>
        public string ComboSql
        {
            get
            {
                return comboSql;
            }
            set
            {
                comboSql = value;
            }
        }
        /// <summary>
        /// 设定DropDownList的Value字段
        /// </summary>
        public string ComboBoxValueField
        {
            get
            {
                return comboBoxValueField;
            }
            set
            {
                comboBoxValueField = value;
            }
        }
        /// <summary>
        /// 弹出页面的SQL语句
        /// </summary>
        public string ModelSelectSQL
        {
            get
            {
                return modelSelectSQL;
            }
            set
            {
                modelSelectSQL = value;
            }
        }
        /// <summary>
        /// 是否为空
        /// </summary>
        public bool CanNull
        {
            get
            {
                return canNull;
            }
            set
            {
                if (value == false || value == true)
                {
                    canNull = value;
                }
            }
        }
        /// <summary>
        /// 是否在列表中显示
        /// </summary>
        public bool IsInListTable
        {
            get
            {
                return isInListTable;
            }
            set
            {
                if (value == false || value == true)
                {
                    isInListTable = value;
                }
            }
        }
        /// <summary>
        /// 是否存在弹出窗口
        /// </summary>
        public bool HasModelButton
        {
            get
            {
                return hasModelButton;
            }
            set
            {
                if (value == true || value || false)
                {
                    hasModelButton = value;
                }
            }
        }
        /// <summary>
        /// TEXT - 是否只读
        /// </summary>
        public bool ReadOnly
        {
            set
            {
                if (value == true || value == false)
                {
                    readOnly = value;
                }
            }
            get
            {
                return readOnly;
            }
        }
        /// <summary>
        /// TEXT - 输入内容的最大长度
        /// </summary>
        public int Length
        {
            set
            {
                length = value;
            }
            get
            {
                return length;
            }
        }
        /// <summary>
        /// 设定最大值
        /// </summary>
        public double MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
            }
        }
        /// <summary>
        /// 设定最小值
        /// </summary>
        public double MinValue
        {
            get
            {
                return minValue;
            }
            set
            {
                minValue = value;
            }
        }
        /// <summary>
        /// 设定TextArea的列个数
        /// </summary>
        public int Cols
        {
            get
            {
                return cols;
            }
            set
            {
                cols = value;
            }
        }
        /// <summary>
        /// 设定TextArea 的行数
        /// </summary>
        public int Rows
        {
            get
            {
                return rows;
            }
            set
            {
                rows = value;
            }
        }
        /// <summary>
        /// 子表名称
        /// </summary>
        /// <returns></returns>
        public string SubTable
        {
            get
            {
                return subTable;
            }
            set
            {
                subTable = value;
            }
        }
   
        /// <summary>
        /// 设定TD内容
        /// </summary>
        public string TDText
        {
            get
            {
                return tdText;
            }
            set
            {
                tdText = value;
            }
        }
        /// <summary>
        /// 设定长度类型
        /// </summary>

        public string TdWidth
        {
            set
            {
                tdWidth = value;
            }
            get
            {
                return tdWidth;
            }
        }

        /// <summary>
        /// 单元格所占格数
        /// </summary>
        public string TdColsPan
        {
            set
            {
                tdColsPan = value;
            }
            get
            {
                return tdColsPan;
            }
        }

        /// <summary>
        /// 设置按钮的前台事件
        /// </summary>
        public string JSClik
        {
            set
            {
                jsClick = value;
            }
            get
            {
                return jsClick;
            }
        }

        /// <summary>
        /// 返回Dreamwever的验证对象
        /// </summary>
        public string Eventhanding
        {
            set
            {
                eventhanding = value;
            }
            get
            {
                return eventhanding;
            }
        }
        #endregion

        #region 产生控件的方法
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
        /// 返回Html代码
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public abstract string GetHtml(string htmlStr,XmlNode controlNode);

        #endregion
    }
}
