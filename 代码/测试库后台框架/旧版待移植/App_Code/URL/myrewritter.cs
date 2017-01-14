using System;
using System.Collections.Generic;
using System.Web;
using URLRewriter;

namespace URL
{ 
    public class myrewritter : URLRewriter.BaseModuleRewriter 
    { 
        protected override void Rewrite(string requestedPath, HttpApplication app)
        {
            if (requestedPath.Contains("/ceshi.html"))
                app.Context.RewritePath("/services/FWPTZS/YHZTC/login_w.aspx");
            else
            { }
                //app.Context.RewritePath("/services/FWPTZS/login.aspx"); 
        } 
    }
}