using System;
using System.Collections;
using System.ComponentModel;
using System.Data;

using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;



using System.IO;
using System.Net;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Web.Services.Description;
using System.Xml.Serialization;
using System.Web.Services.Discovery;
using System.Xml.Schema;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;


namespace FMipcClass
{



    public class InstanceCachesEx
    {
        public byte[] ConvertStreamToByteBuffer(System.IO.Stream theStream)
        {
            int b1;
            System.IO.MemoryStream tempStream = new System.IO.MemoryStream();
            while ((b1 = theStream.ReadByte()) != -1)
            {
                tempStream.WriteByte(((byte)b1));
            }
            return tempStream.ToArray();
        }
        private static readonly object obj = new object();


        private Hashtable htEx = new Hashtable();

        public Assembly InstanceCache(string key)
        {


            if (htEx.ContainsKey(key))
            {
                return (Assembly)(htEx[key]);
            }
            else
            {
                Byte[] content = null;
                FileStream f = new FileStream(key, FileMode.Open, FileAccess.Read, FileShare.Read);
                int b1;
                System.IO.MemoryStream tempStream = new System.IO.MemoryStream();
                while ((b1 = f.ReadByte()) != -1)
                {
                    tempStream.WriteByte(((byte)b1));
                }
                f.Close();

                content = tempStream.ToArray();
                Assembly valuenew = Assembly.Load(content);

                lock (obj)
                {
                    if (!htEx.ContainsKey(key))
                    {
                        htEx[key] = valuenew;
                    }

                }



                return valuenew;
            }
        }

    }

    public static class GetCache
    {
        static GetCache()
        {
            InstanceCacheEx = new InstanceCachesEx();
        }

        public static InstanceCachesEx InstanceCacheEx { get; set; }
    }


    /// <summary>
    /// 动态调用webservice类。根据网址和方法名调用。
    /// </summary>
    [WebService(Namespace = "http://ipc.ipc.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class FMWScenter : System.Web.Services.WebService
    {

        #region	私有变量和属性定义
        ///	<summary>					
        ///	web服务地址							
        ///	</summary>							
        private string _wsdlUrl = string.Empty;
        ///	<summary>					
        ///	web服务名称							
        ///	</summary>							
        private string _wsdlName = string.Empty;
        ///	<summary>					
        ///	代理类命名空间							
        ///	</summary>							
        private string _wsdlNamespace = "FMWSDL__{0}__";
        ///	<summary>					
        ///	代理类类型名称							
        ///	</summary>							
        private Type _typeName = null;
        ///	<summary>					
        ///	程序集名称							
        ///	</summary>							
        private string _assName = string.Empty;
        ///	<summary>					
        ///	代理类所在程序集路径							
        ///	</summary>							
        private string _assPath = string.Empty;
        ///	<summary>					
        ///	代理类的实例							
        ///	</summary>							
        private object _instance = null;

        #endregion


        /// <summary>
        /// 重新生成代理类。先删除原来的文件，再根据传入的地址重新生成。
        /// </summary>
        /// <param name="WSDLurls">要重新生成的接口地址列表</param>
        /// <returns>ok是成功，其他都是失败。</returns>
        public string GetNewWSDL(string[] WSDLurls)
        {
            try
            {


                //删除所有代理类的代码。
                DirectoryInfo di = new DirectoryInfo(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/ForIPC/");
                FileInfo[] ff = di.GetFiles("FMWSDL__*.dll");
                if (ff.Length != 0)
                {
                    foreach (FileInfo fi in ff)
                    {
                        if (fi.IsReadOnly)
                        {
                            fi.IsReadOnly = false; //更改文件的只读属性
                        }
                        fi.Delete();
                    }
                }

                foreach (string url in WSDLurls)
                {
                    FMWScenter_temp(url);
                }

                return "ok";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        #region	构造函数

        public FMWScenter()
        {
            ;
        }

        /// <summary>
        /// 初始化远程调用，若本地存在dll文件，则不会获取新的代理类。 不存在dll文件，才会获取。
        /// url格式：http://网址/路径/xxx.asmx?wsdl
        /// </summary>
        /// <param name="wsdlUrl">远程url</param>
        public FMWScenter(string wsdlUrl)
        {
            FMWScenter_temp(wsdlUrl);
        }

        private void FMWScenter_temp(string wsdlUrl)
        {
            this._wsdlUrl = wsdlUrl;
            string wsdlName = FMWScenter.getWsclassName(wsdlUrl);
            this._wsdlName = wsdlName;
            this._assName = string.Format(_wsdlNamespace, wsdlName);
            this._assPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/ForIPC/" + this._assName + getMd5Sum(this._wsdlUrl) + ".dll";
            this.CreateServiceAssembly();
        }




        #endregion




        #region	得到WSDL信息，生成本地代理类并编译为DLL，构造函数调用，类生成时加载
        ///	<summary>							
        ///	得到WSDL信息，生成本地代理类并编译为DLL							
        ///	</summary>							
        private void CreateServiceAssembly()
        {
            if (this.checkCache())
            {
                this.initTypeName();
                return;
            }
            if (string.IsNullOrEmpty(this._wsdlUrl))
            {
                return;
            }
            try
            {
                //使用WebClient下载WSDL信息						
                WebClient web = new WebClient();
                Stream stream = web.OpenRead(this._wsdlUrl);
                ServiceDescription description = ServiceDescription.Read(stream);//创建和格式化WSDL文档
                stream.Close();
                web.Dispose();

                ServiceDescriptionImporter importer = new ServiceDescriptionImporter();//创建客户端代理代理类
                importer.ProtocolName = "Soap";
                importer.Style = ServiceDescriptionImportStyle.Client;	//生成客户端代理						
                importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
                importer.AddServiceDescription(description, null, null);//添加WSDL文档
                //使用CodeDom编译客户端代理类					
                CodeNamespace nmspace = new CodeNamespace(_assName);	//为代理类添加命名空间				
                CodeCompileUnit unit = new CodeCompileUnit();
                unit.Namespaces.Add(nmspace);
                this.checkForImports(this._wsdlUrl, importer);
                ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                CompilerParameters parameter = new CompilerParameters();
                parameter.ReferencedAssemblies.Add("System.dll");
                parameter.ReferencedAssemblies.Add("System.XML.dll");
                parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
                parameter.ReferencedAssemblies.Add("System.Data.dll");
                parameter.GenerateExecutable = false;
                parameter.GenerateInMemory = false;
                parameter.IncludeDebugInformation = false;
                CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);
                provider.Dispose();
                if (result.Errors.HasErrors)
                {
                    string errors = string.Format(@"编译错误:{0}错误！", result.Errors.Count);
                    foreach (CompilerError error in result.Errors)
                    {
                        errors += error.ErrorText;
                    }
                    throw new Exception(errors);
                }
                this.copyTempAssembly(result.PathToAssembly);
                this.initTypeName();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region	执行Web服务方法
        ///	<summary>							
        ///	执行代理类指定方法，有返回值。不支持没有返回值的远程方法。					
        ///	</summary>								
        ///	<param	name="methodName">方法名称</param>							
        ///	<param	name="param">参数数组，数组中每个成员，代表一个实际参数，没有参数就直接传入null</param>							
        ///	<returns>远程方法返回值</returns>								
        public object ExecuteQuery(string methodName, object[] param)
        {




            object rtnObj = null;
            string[] args = new string[2];
            List<string> list = new List<string>();
            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();
            object[] obj = new object[3];

            try
            {
                if (this._typeName == null)
                {
                    //记录Web服务访问类名错误日志代码位置
                    throw new TypeLoadException("Web服务访问类名【" + this._wsdlName + "】不正确，请检查！");
                }
                //调用方法



                MethodInfo mi = this._typeName.GetMethod(methodName);
                FastInvoke.FastInvokeHandler fastInvoker = FastInvoke.GetMethodInvoker(mi);


                if (mi == null)
                {
                    //记录Web服务方法名错误日志代码位置
                    throw new TypeLoadException("Web服务访问方法名【" + methodName + "】不正确，请检查！");
                }
                try
                {
                    if (param == null)
                    {
                        //另一个方式，出多，实际不用
                        rtnObj = fastInvoker(_instance, null);
                    }
                    else
                    {
                        rtnObj = mi.Invoke(_instance, param);
                    }
                }
                catch (TypeLoadException tle)
                {
                    //记录Web服务方法参数个数错误日志代码位置
                    throw new TypeLoadException("Web服务访问方法【" + methodName + "】参数个数不正确，请检查！", new TypeLoadException(tle.StackTrace));
                }

         
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, new Exception(ex.StackTrace));
            }
            return rtnObj;
        }


        #endregion

        #region	私有方法
        ///	<summary>								
        ///	得到代理类类型名称								
        ///	</summary>								
        private void initTypeName()
        {
            Assembly serviceAsm = GetCache.InstanceCacheEx.InstanceCache(this._assPath);
            Type[] types = serviceAsm.GetTypes();
            string objTypeName = "";
            foreach (Type t in types)
            {
                if (t.BaseType == typeof(SoapHttpClientProtocol))
                {
                    objTypeName = t.Name;
                    break;
                }
            }
            _typeName = serviceAsm.GetType(this._assName + "." + objTypeName);
            _instance = serviceAsm.CreateInstance(this._assName + "." + objTypeName);


        }

        ///	<summary>						
        ///	根据web	service文档架构向代理类添加ServiceDescription和XmlSchema							
        ///	</summary>								
        ///	<param	name="baseWSDLUrl">web服务地址</param>							
        ///	<param	name="importer">代理类</param>							
        private void checkForImports(string baseWsdlUrl, ServiceDescriptionImporter importer)
        {
            DiscoveryClientProtocol dcp = new DiscoveryClientProtocol();
            dcp.DiscoverAny(baseWsdlUrl);
            dcp.ResolveAll();
            foreach (object osd in dcp.Documents.Values)
            {
                if (osd is ServiceDescription) importer.AddServiceDescription((ServiceDescription)osd, null, null); ;
                if (osd is XmlSchema) importer.Schemas.Add((XmlSchema)osd);
            }
        }
        ///	<summary>							
        ///	复制程序集到指定路径								
        ///	</summary>								
        ///	<param	name="pathToAssembly">程序集路径</param>							
        private void copyTempAssembly(string pathToAssembly)
        {
                File.Copy(pathToAssembly, this._assPath);
        }

        private string getMd5Sum(string str)
        {
            Encoder enc = System.Text.Encoding.Unicode.GetEncoder();
            byte[] unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }
            return sb.ToString();
        }

        ///	<summary>							
        ///	是否已经存在该程序集								
        ///	</summary>								
        ///	<returns>false:不存在该程序集,true:已经存在该程序集</returns>								
        private bool checkCache()
        {
            if (File.Exists(this._assPath))
            {
                return true;
            }
            return false;
        }

        //私有方法，默认取url入口的文件名为类名
        private static string getWsclassName(string wsdlUrl)
        {
            string[] parts = wsdlUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }
        #endregion
    }
}