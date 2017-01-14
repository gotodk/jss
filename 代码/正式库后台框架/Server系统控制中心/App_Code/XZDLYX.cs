using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;


/// <summary>
/// Helper 的摘要说明
/// </summary>
public static class XZDLYX
{
    public static string GetOpen(string email)
    {
        string result="";
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable ht = new Hashtable();
            ht["@email"]=email;
            Hashtable returnHT = I_DBL.RunProc_CMD("GetOpen", "", ht);

            result = (returnHT["return_ds"] == null && ((DataSet)returnHT["return_ds"]).Tables.Count <= 0 && ((DataSet)returnHT["return_ds"]).Tables[0].Rows.Count>0) ? "ok" : ((DataSet)returnHT["return_ds"]).Tables[0].Rows[0][0].ToString();
            return result;
        }
        catch(Exception e)
        {
            return "ok";
        }
        
    }
}