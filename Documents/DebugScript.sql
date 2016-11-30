
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
