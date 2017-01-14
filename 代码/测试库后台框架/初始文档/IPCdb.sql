USE [yhbceshi]
GO

/****** Object:  Table [dbo].[AAA_ipcFF]    Script Date: 2014/2/28 22:27:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AAA_ipcFF](
	[FF_guid] [varchar](50) NOT NULL,
	[FF_JK_guid] [varchar](50) NOT NULL,
	[FF_yewuname] [varchar](100) NOT NULL,
	[FF_name] [varchar](100) NOT NULL,
	[FF_retype] [varchar](100) NOT NULL,
	[FF_canshu] [varchar](100) NOT NULL,
	[FF_shuoming] [varchar](100) NOT NULL,
	[FF_open] [int] NOT NULL,
	[FF_zt] [int] NOT NULL,
	[FF_CorE] [int] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[AAA_ipcGX]    Script Date: 2014/2/28 22:27:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AAA_ipcGX](
	[GX_guid] [varchar](50) NOT NULL,
	[GX_shibie] [varchar](100) NOT NULL,
	[GX_savelog] [int] NOT NULL,
	[GX_JK_guid] [varchar](50) NOT NULL,
	[GX_FF_guid] [varchar](50) NOT NULL,
	[GX_type] [int] NOT NULL,
	[GX_open] [int] NOT NULL,
	[GX_addtime] [datetime] NOT NULL,
	[GX_edittime] [datetime] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[AAA_ipcJK]    Script Date: 2014/2/28 22:27:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AAA_ipcJK](
	[JK_guid] [varchar](50) NOT NULL,
	[JK_host] [varchar](100) NOT NULL,
	[JK_path] [varchar](100) NOT NULL,
	[JK_shuoming] [varchar](100) NOT NULL,
	[JK_banben] [varchar](50) NOT NULL,
	[JK_open] [int] NOT NULL,
	[JK_zt] [int] NOT NULL,
	[JK_addtime] [datetime] NOT NULL,
	[JK_edittime] [datetime] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[AAA_ipcLOG]    Script Date: 2014/2/28 22:27:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AAA_ipcLOG](
	[LOG_guid] [varchar](50) NOT NULL,
	[LOG_GX_guid] [varchar](50) NOT NULL,
	[LOG_JK_host] [varchar](100) NOT NULL,
	[LOG_JK_path] [varchar](100) NOT NULL,
	[LOG_FF_yewuname] [varchar](100) NOT NULL,
	[LOG_FF_name] [varchar](100) NOT NULL,
	[LOG_FF_retype] [varchar](100) NOT NULL,
	[LOG_FF_canshu] [varchar](100) NOT NULL,
	[LOG_GX_type] [int] NOT NULL,
	[LOG_FF_CorE] [int] NOT NULL,
	[LOG_begintime] [datetime] NULL,
	[LOG_alltime] [int] NULL,
	[LOG_zt] [int] NOT NULL,
	[LOG_trytext1] [text] NULL,
	[LOG_trytext2] [text] NULL,
	[LOG_errbz] [text] NULL,
	[LOG_xingneng] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

