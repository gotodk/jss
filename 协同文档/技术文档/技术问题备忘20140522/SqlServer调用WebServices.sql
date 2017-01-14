
go
sp_configure 'show advanced options', 1;  
GO  
RECONFIGURE;  
GO  
sp_configure 'Ole Automation Procedures', 1;  
GO  
RECONFIGURE;  
GO  
exec proc_CallWebServices




CREATE  PROCEDURE proc_CallWebServices

as

---Sql Server新建存储过程创建数据库


DECLARE @TimeLength INT=30  
DECLARE @Number varchar(200)='1310000001' 
DECLARE @obj INT     
DECLARE @hr INT       
DECLARE @url VARCHAR(200)      
DECLARE @response VARCHAR(5000)  

if((select YCFSSJ '异常发生时间' from dbo.AAA_JKXXSJB where Number=@Number )is NOT NULL)
BEGIN
  if((SELECT DATEDIFF(mi,(select YCFSSJ '异常发生时间' from dbo.AAA_JKXXSJB where Number=@Number),GETDATE()))>@TimeLength)
   BEGIN
   SET  @url='http://192.168.0.10:8010/WebService.asmx/SendMsg?strNumber='+@Number+'&strTagInfo=1'    
   EXEC sp_OACreate 'MSXML2.ServerXMLHttp',@obj out  
   EXEC sp_OAMethod @obj,'Open',null,'GET',@url,false  
   EXEC @hr= sp_OAMethod @obj,'send'  
   IF @hr <> 0
   BEGIN
		EXEC sp_OAGetErrorInfo @obj   
		select @obj '错误信息'
   END
   ELSE
   select @hr  '数据信息1'       
   EXEC @hr=sp_OAGetProperty @obj,'responseText',@response out          
   SELECT @response [response],@hr  '返回值'     
   EXEC sp_OADestroy @obj  
   update dbo.AAA_JKXXSJB set YCFSSJ=GETDATE() where Number=@Number
END
END

if((select YCFSSJ '软件运行时间' from dbo.AAA_JKXXSJB where Number=@Number )is NOT NULL)
BEGIN
   if((SELECT DATEDIFF(mi,(select CreateTime 'CreateTime' from dbo.AAA_JKXXSJB where Number=@Number ),GETDATE()))>@TimeLength)
   BEGIN
   SET  @url='http://192.168.0.10:8010/WebService.asmx/SendMsg?strNumber='+@Number+'&strTagInfo=2'  
   EXEC sp_OACreate 'MSXML2.ServerXMLHttp',@obj out  
   EXEC sp_OAMethod @obj,'Open',null,'GET',@url,false  
   EXEC @hr= sp_OAMethod @obj,'send'  
   IF @hr <> 0
   BEGIN
		EXEC sp_OAGetErrorInfo @obj   
		select @obj '错误信息'
   END
   ELSE
   select @hr  '数据信息1'       
   EXEC @hr=sp_OAGetProperty @obj,'responseText',@response out          
   SELECT @response [response],@hr  '返回值'     
   EXEC sp_OADestroy @obj  
   END
END


    