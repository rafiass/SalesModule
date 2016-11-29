use [Demo]
go


Drop table PluReqSale;
-- require product for a specific sale
CREATE TABLE [dbo].[PluReqSale](
	[SaleID] [int] NOT NULL,
	[pluID] [nvarchar](50) NOT NULL,
	[isPluno] [bit] NOT NULL, --is pluID represent [pluno] or [kind3]
	[qty] [real] NOT NULL,
 CONSTRAINT [PK_PluReqSale] PRIMARY KEY CLUSTERED 
(
	[SaleID] ASC,
	[pluID] ASC,
	[isPluno] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


Drop table PluOutSale;
-- products on which the sale is effective, with new price or discount
CREATE TABLE [dbo].[PluOutSale](
	[OutID] [int] NOT NULL,
	[SaleID] [int] NOT NULL,
	[pluID] [nvarchar](50) NOT NULL,
	[isPluno] [bit] NOT NULL,
	[MultiUnits] [real] default 1, --the amount on which the discount is effective, 0 - foreach unit
	[MaxRec] [real] default 1, --the amount of discounts allowed, 0 - unlimited
	[offPrice] [real] NOT NULL,
	[offType] [int] NOT NULL,
 CONSTRAINT [PK_PluOutSale] PRIMARY KEY CLUSTERED 
(
	[SaleID] ASC,
	[pluID] ASC,
	[isPluno] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

Drop table PluGiftedSale;
-- products which is gifted (or discounted) when a particular discounted product is effective
CREATE TABLE [dbo].[PluGiftedSale](
	[OutID] [int] NOT NULL,
	[pluID] [nvarchar](50) NOT NULL,
	[isPluno] [bit] NOT NULL,
	[MultiUnits] [real] default 1, --the amount on which the discount is effective, 0 - foreach unit
	[offPrice] [real] NOT NULL,
	[offType] [int] NOT NULL,
 CONSTRAINT [PK_PluGiftedSale] PRIMARY KEY CLUSTERED 
(
	[OutID] ASC,
	[pluID] ASC,
	[isPluno] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


Drop table Sales;
-- Sale object
CREATE TABLE [dbo].[Sales](
	[SaleID] [int] IDENTITY(1,1) NOT NULL,
	[SaleGroupID] [int] NOT NULL,
	[GroupIndex] [int] NOT NULL,
	[SaleType] [int] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[MinTotalPrice] [real] default 0, --minimum total price from which the sale is effective
	[MaxTotalPrice] [real] default NULL, --maximum total price to which the sale is effective, NULL - unlimited
	[TotalOffPrice] [real] default 0,
	[TotalOffType] [int] default 1,
	[AllowMultiple] [int] default 1, --effective once or multiple times (for each minimum receipt value), 0 - unlimited
	[Recurrences] [int] default 1, --number of recurrences for each instance
 CONSTRAINT [PK_Sales] PRIMARY KEY CLUSTERED 
(
	[SaleGroupID] ASC,
	[GroupIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


Drop table SalesGroup;
-- Gather series of sales to a united group
CREATE TABLE [dbo].[SalesGroup](
	[GroupID] [int] IDENTITY(1,1) NOT NULL,
	[Empno] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL DEFAULT GETDATE(),
	[isEnabled] [bit] NOT NULL default 1,
 CONSTRAINT [PK_SalesGroup] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


Drop table SalesPcid;
-- mix and match of sales and branches, including border dates and effective hours for each particular branch
CREATE TABLE [dbo].[SalesPcid](
	[SaleGroupID] [int] NOT NULL,
	[PCID] [int] NOT NULL,
	[isEnabled] [bit] NOT NULL default 1,
	[DateFrom] [date] NOT NULL,
	[DateTo] [date],
	[HourFrom] [time](0) NULL,
	[HourTo] [time](0) NULL,
 CONSTRAINT [PK_SalesPcid] PRIMARY KEY CLUSTERED 
(
	[SaleGroupID] ASC,
	[PCID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


Drop table SalesUser;
-- restricting each sale to a particular customer or club
CREATE TABLE [dbo].[SalesUser](
	[SaleGroupID] [int] NOT NULL,
	[VipID] [int] NOT NULL,
	[isVipno] [bit] NOT NULL,
 CONSTRAINT [PK_SalesUser] PRIMARY KEY CLUSTERED 
(
	[SaleGroupID] ASC,
	[VipID] ASC,
	[isVipno] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


Drop table SalesOffType;
-- off types
CREATE TABLE [dbo].[SalesOffType](
	[TypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SalesOffType] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

insert into SalesOffType (TypeName) values ('Nothing') -- 1
insert into SalesOffType (TypeName) values ('Percentage') -- 2
insert into SalesOffType (TypeName) values ('Fix Price') -- 3
insert into SalesOffType (TypeName) values ('Fix Discount') -- 4



Drop table SaleTypes;
-- off types
CREATE TABLE [dbo].SaleTypes(
	[TypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SaleType] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

insert into SaleTypes (TypeName) values ('מוצר בהנחה') -- 1
insert into SaleTypes (TypeName) values ('מוצר מוזל') -- 2
insert into SaleTypes (TypeName) values ('קנה וקבל פשוט') -- 3
insert into SaleTypes (TypeName) values ('קנה וקבל מתקדם') -- 4
insert into SaleTypes (TypeName) values ('חבילה במבצע מתקדם') -- 5
