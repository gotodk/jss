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
using System.IO;
using FMOP.XParse;
using FMOP.DB;
using FMOP.FACTORY;
using FMOP.TagCode;
using FMOP.BUTTON;

/// <summary>
/// Module 的摘要说明
/// </summary>
namespace FMOP.Module
{
    public class WorkFlowModule
    {
        /// <summary>
        /// 产生HTML代码
        /// </summary>
        private string htmlCode = string.Empty;
        private string moduleName = string.Empty;
        private string Number = string.Empty;
        public string pageNumer = string.Empty;

        /// <summary>
        /// 存放配置文件的Xml数据
        /// </summary>
        private string xmlDoc = string.Empty;

        /// <summary>
        /// 默认的模块构造函数
        /// </summary>
        public WorkFlowModule()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 重载模块构造函数
        /// </summary>
        /// <param name="module">模块名</param>
        public WorkFlowModule(string p_moduleName)
        {
            //查询该模块的配置文件
            moduleName = p_moduleName;
            xmlDoc = xmlParse.GetXml(p_moduleName);
        }
        /// <summary>
        /// 重载模块构造函数
        /// </summary>
        /// <param name="module">模块名</param>
        /// <param name="p_Number">单号</param>
        public WorkFlowModule(string p_moduleName,string p_Number)
        {
            //查询该模块的配置文件
            moduleName = p_moduleName;
            Number = p_Number;
            xmlDoc = xmlParse.GetXml(p_moduleName);
        }

        /// <summary>
        /// 返回html代码
        /// </summary>
        /// <returns></returns>
        public string GetHtml(string Type,string _PageTab)
        {
            XmlNodeList xlist = null;
            HtmlFile objFile = new HtmlFile();

            //设置模块名和单号
            objFile.ModuleName = moduleName;
            objFile.Number = Number;
            objFile.PageNumber = pageNumer;

            //产生Html引用的Js,CSS等文件代码
            objFile.PageTab = _PageTab;
            htmlCode = objFile.GetHtml(xmlDoc, Type);

            //产生Html主要控件代码
            xlist = xmlParse.GetXmlNodeList(moduleName, "//Data/Data-Field");

            FactoryControl Factory = new FactoryControl();
            Factory.OperatingFlag = Type;
            Factory.ModuleName = moduleName;
            switch (Type)
            {
                case "Add":
                    Factory.ParseConfigFiles(xlist, htmlCode);
                    htmlCode = Factory.GetHtml();
                    break;
                case "Update":
                    Factory.Number = Number;
                    Factory.ParseConfigFiles(xlist, htmlCode);
                    htmlCode = Factory.GetHtml();
                    break;
            }

            return htmlCode;
        }
    }
}
