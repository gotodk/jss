using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// SaveDepositary 的摘要说明
/// 定义节点控件的属性
/// </summary>
namespace FMOP.SD
{
    public class SaveDepositary
    { 
        /// <summary>
        /// 返回和设置节点名称
        /// </summary>
        private string name = string.Empty;
    
        /// <summary>
        /// 节点的名称
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
        /// 节点类型 
        /// </summary>
        private string type = string.Empty;

        /// <summary>
        /// 节点类型
        /// </summary>
        public string Type
        {
            set
            {
                type = value;
            }
            get
            {
                return type;
            }
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        private bool canNull;
        /// <summary>
        /// 是否可为空
        /// </summary>
        public bool CanNull
        {
            set
            {
                canNull = value;
            }
            get
            {
                return canNull;
            }
        }

        /// <summary>
        /// 内容
        /// </summary>
        private string text = string.Empty;
        
        /// <summary>
        /// 输入的内容
        /// </summary>
        public string Text
        {
            set
            {
                if (value == null)
                {
                    text = string.Empty;
                }
                else
                {
                    text = value;
                }
            }
            get
            {
                return text;
            }
        }

        /// <summary>
        /// 输入内容的最大长度
        /// </summary>
        private int length = 20;
        
        /// <summary>
        /// 输入内容的最大长度
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
        /// 若是子表，则保存子表名称
        /// </summary>
        private string subTable = string.Empty;
        /// <summary>
        /// 若为子表，则保存子表名称
        /// </summary>
        public string SubTable
        {
            set
            {
                subTable = value;
            }
            get
            {
                return subTable;
            }
        }

        public SaveDepositary()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
    }
}
