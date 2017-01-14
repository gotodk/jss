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
using System.IO;
using FMOP.XHelp;
/// <summary>
/// addHtml 的摘要说明
/// </summary>
namespace FMOP.TagCode
{
    public class HtmlFile
    {
        private string addOnCssFile = string.Empty;
        private string addOnJsFile = string.Empty;
        private string sP_OnAdd = string.Empty;
        private string jS_AfterAdd = string.Empty;
        private string jS_AfterDelete = string.Empty;
        private string jS_AfterModify = string.Empty;
        private string redirect_AfterModify = string.Empty;
        private string moduleName = string.Empty;
        private string number = string.Empty;
        private string pagenumber = string.Empty;


        public string PageNumber
        {
            set
            {
                pagenumber = value;
            }
            get
            {
                return pagenumber;
            }
        }
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
        /// 页面要添加的CSS文件
        /// </summary>
        public string AddOnCssFile
        {
            get
            {
                return addOnCssFile;
            }
            set
            {
                addOnCssFile = value;
            }
        }

        /// <summary>
        /// 页面要添加的前台JS文件
        /// </summary>
        public string AddOnJsFile
        {
            get
            {
                return addOnJsFile;
            }
            set
            {
                addOnJsFile = value;
            }
        }

        /// <summary>
        /// 页面操作执行的存储过程 
        /// </summary>
        public string SP_OnAdd
        {
            get
            {
                return sP_OnAdd;
            }
            set
            {
                sP_OnAdd = value;
            }
        }

        /// <summary>
        /// 添加的表单验证
        /// </summary>
        public string JS_AfterAdd
        {
            get
            {
                return jS_AfterAdd;
            }
            set
            {
                jS_AfterAdd = value;
            }
        }

        public string JS_AfterModify
        {
            get
            {
                return jS_AfterModify;
            }
            set
            {
                jS_AfterModify = value;
            }
        }

        /// <summary>
        /// 删除时的提示信息
        /// </summary>
        public string JS_AffterDelete
        {
            get
            {
                return jS_AfterDelete;
            }
            set
            {
                jS_AfterDelete = value;
            }
        }

        /// <summary>
        /// 执行操作完成后，所转向的页面
        /// </summary>
        public string Redirect_AfterModify
        {
            get
            {
                return redirect_AfterModify;
            }
            set
            {
                redirect_AfterModify = value;
            }
        }

        private string pageTab = string.Empty;
        public string PageTab
        {
            set
            {
                pageTab = value;
            }
            get
            {
                return pageTab;
            }
        }

        public HtmlFile()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 产生Html文件的整个结构代码
        /// </summary>
        public string HtmlCode(string Type)
        {
            string page = string.Empty;
            StringBuilder htmStruct = new StringBuilder();
            htmStruct.Append("<html>\r\n");
            htmStruct.Append("<head>\r\n");
            htmStruct.Append("<title>FMOP</title>\r\n");
            htmStruct.Append("<Meta http-equiv=\"Content-Type\" content=\"text/html; Charset=utf-8\">\r\n");
            htmStruct.Append("<link href=\"../css/SpryValidationTextField.css\" rel=\"stylesheet\" type=\"text/css\"/>\r\n");
            htmStruct.Append("<link href=\"../css/SpryValidationTextarea.css\" rel=\"stylesheet\" type=\"text/css\"/>\r\n");
            htmStruct.Append("<link href=\"../css/SpryValidationSelect.css\" rel=\"stylesheet\" type=\"text/css\"/>\r\n");
            htmStruct.Append("<link href=\"../css/style.css\" rel=\"stylesheet\" type=\"text/css\"/>\r\n");
            htmStruct.Append("<script src=\"../js/SpryValidationTextField.js\" type=\"text/javaScript\"></script>\r\n");
            htmStruct.Append("<script src=\"../js/SpryValidationTextarea.js\" type=\"text/javaScript\"></script>\r\n");
            htmStruct.Append("<script src=\"../js/setday.js\" type=\"text/javaScript\"></script>\r\n");
            htmStruct.Append("<script type=\"text/javascript\" src=\"../js/tigra_tables.js\"></script>\r\n");
            htmStruct.Append("<script src=\"../js/common-private.js\" type=\"text/javaScript\"></script>\r\n");
            htmStruct.Append("<script src=\"../js/SpryValidationSelect.js\" type=\"text/javaScript\"></script>\r\n");

            if (AddOnCssFile != "")
            {
                htmStruct.Append("<link href=\"../css/"+AddOnCssFile+"\" rel=\"stylesheet\" type=\"text/css\"/>\r\n");
            }

            if (AddOnJsFile != "")
            {
                htmStruct.Append("<script src=\"../js/" + AddOnJsFile + "\" type=\"text/javaScript\"></script>\r\n");
            }

            htmStruct.Append("<script language=\"javaScript\"event=\"onkeydown\" for=\"document\">\r\n");
            htmStruct.Append("if(event.keyCode==13||event.keyCode==9)\r\n");
            htmStruct.Append("{\r\n");
            htmStruct.Append("var objName;\r\n");
            htmStruct.Append("objName = event.srcElement.name;\r\n");
            htmStruct.Append("if(event.srcElement.type == \"textarea\")");
            htmStruct.Append("return true;\r\n");
            htmStruct.Append("frm = document.getElementById('frm1');\r\n");
            htmStruct.Append("for(var i=0;i<frm.elements.length;i++)\r\n");
            htmStruct.Append("{\r\n");
            htmStruct.Append("\tif(objName == frm.elements[i].name)\r\n");
            htmStruct.Append("\tfor(var j=i+1;j<frm.elements.length;j++)\r\n");
            htmStruct.Append("\t{\r\n");
            htmStruct.Append("\t\tif(frm.elements[j].type != \"button\" && frm.elements[j].type !=\"File\" && frm.elements[j].type !=\"hidden\")");
            htmStruct.Append("\t\t{\r\n");
            htmStruct.Append("\t\t\tfrm.elements[j].focus();return false;\r\n");
            htmStruct.Append("\t\t}\r\n");
            htmStruct.Append("\t}\r\n");
            htmStruct.Append("}\r\n");
            htmStruct.Append("}\r\n");
            htmStruct.Append("</script>\r\n");
            htmStruct.Append("</head>\r\n");
            htmStruct.Append("<body>\r\n");
            htmStruct.Append("<table style=\"width:100%\" class=\"FormView\">");
            if (pagenumber != "")
            {
                page = "&pagenumber=" + pagenumber;
            }
            else
            {
                page = "";
            }
            switch (Type)
            {
                case "Add":
                    if (JS_AfterAdd != "")
                    {
                        htmStruct.Append("<form id='frm1' method='post' action='WorkFlow_Add_Save.aspx?module=" + moduleName + "' enctype=\"multipart/form-data\" onsubmit=\"" + JS_AfterAdd + "()\">\r\n");
                    }
                    else
                    {
                        //htmStruct.Append("<form id='frm1' method='post' action='WorkFlow_Add_Save.aspx?module=" + moduleName + "' enctype=\"multipart/form-data\" onsubmit=\"return submitForm();\">\r\n");
                        htmStruct.Append("<form id='frm1' method='post' action='WorkFlow_Add_Save.aspx?module=" + moduleName + "' enctype=\"multipart/form-data\">\r\n");
                    }
                    break;

                case "Update":
                    if (JS_AfterModify != "")
                    {
                        htmStruct.Append("<form id='frm1' method='post' action='WorkFlow_Update_Save.aspx?module=" + moduleName + "&KeyNumber=" + Number + page + "' enctype=\"multipart/form-data\" onsubmit=\"" + JS_AfterModify + "()\">\r\n");
                    }
                    else
                    {
                        htmStruct.Append("<form id='frm1' method='post' action='WorkFlow_Update_Save.aspx?module=" + moduleName + "&KeyNumber=" + Number + page + "' enctype=\"multipart/form-data\">\r\n");
                        //htmStruct.Append("<form id='frm1' method='post' action='WorkFlow_Update_Save.aspx?module=" + moduleName + "&number=" + Number + "' enctype=\"multipart/form-data\" onsubmit=\"return submitForm();\">\r\n");
                    }
                    break;
            }
            htmStruct.Append(PageTab);
            htmStruct.Append("</form>\r\n");
            htmStruct.Append("</table>\r\n");
            htmStruct.Append("<script type=\"text/javascript\">\r\n");
            htmStruct.Append("<!--\r\n");
            htmStruct.Append("function submitForm()\r\n");
            htmStruct.Append("{\r\n");
            htmStruct.Append("\tif(!ValidateFile())\r\n");
            htmStruct.Append("\t{\r\n");
            htmStruct.Append("\treturn false;\r\n");
            htmStruct.Append(" \t}\r\n");
            htmStruct.Append("\telse\r\n");
            htmStruct.Append("\t\t{\r\n");
            htmStruct.Append("\t\tif(!Validate())\r\n");
            htmStruct.Append("\t\t{\r\n");
            htmStruct.Append("\t\treturn false;\r\n");
            htmStruct.Append("\t\t}\r\n");
            htmStruct.Append("\t}\r\n");
            htmStruct.Append("return this.frm1.onsubmit;\r\n");
          //  htmStruct.Append("return this.frm1.onsubmit();\r\n");
            htmStruct.Append("}\r\n");
            htmStruct.Append("//设为只读\r\n");
            htmStruct.Append("//-->\r\n");
            htmStruct.Append("</script>\r\n");
            htmStruct.Append("</body>\r\n");
            htmStruct.Append("<script type=\"text/javascript\">\r\n");
            htmStruct.Append("//设为隐藏\r\n");
            htmStruct.Append("</script>\r\n");
            htmStruct.Append("</html>\r\n");
            //函数返回
            return htmStruct.ToString();
        }

        /// <summary>
        /// 返回Html 文件结构代码
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public string GetHtml(string xmldoc, string Type)
        {
            //设置属性
            SettingAttribute(xmldoc);

            //产生HtmlHead代码
            return HtmlCode(Type);
        }

        /// <summary>
        /// 分析配置文件，并给属性传值
        /// </summary>
        /// <param name="xmldoc"></param>
        private void SettingAttribute(string xmlStr)
        {
            string xmlAssert = "<?xml version=\"1.0\" encoding=\"GB2312\"?>";
            Stream stream = new MemoryStream(System.Text.UTF8Encoding.Default.GetBytes(xmlAssert + xmlStr));
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(stream);
            XmlNode node = null;

            node = xmldoc.SelectSingleNode("//WorkFlowModule");
            AddOnCssFile = XMLHelper.GetSingleString(node, "AddOnCssFile", "");

            node = xmldoc.SelectSingleNode("//WorkFlowModule");
            AddOnJsFile = XMLHelper.GetSingleString(node, "AddOnJsFile", "");

            node = xmldoc.SelectSingleNode("//WorkFlowModule");
            JS_AfterAdd = XMLHelper.GetSingleString(node, "JS_AfterAdd", "");


            node = xmldoc.SelectSingleNode("//WorkFlowModule");
            JS_AfterModify = XMLHelper.GetSingleString(node, "JS_AfterModify", "");
            
        }
    }
}
