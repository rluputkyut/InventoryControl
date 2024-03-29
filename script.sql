USE [InventoryControl]
GO
/****** Object:  Table [dbo].[Brand]    Script Date: 4/12/2021 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brand](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Brand] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customer]    Script Date: 4/12/2021 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[NickName] [nvarchar](500) NOT NULL,
	[PhoneNo] [nvarchar](50) NULL,
	[Address] [nvarchar](500) NULL,
	[AccountInformation] [nvarchar](500) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PreOrderHeader]    Script Date: 4/12/2021 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PreOrderHeader](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[WaitingDay] [int] NOT NULL,
	[Deposit] [money] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Remark] [nvarchar](500) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_PreOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PreOrderItems]    Script Date: 4/12/2021 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PreOrderItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HeaderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[OrderPrice] [money] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_PreOrderItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product]    Script Date: 4/12/2021 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](1000) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Photo] [image] NULL,
	[BrandId] [int] NOT NULL,
	[ProductTypeId] [int] NOT NULL,
	[Size] [decimal](18, 2) NULL,
	[Price] [money] NULL,
	[ManufactureDate] [datetime] NULL,
	[ExpiredDate] [datetime] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProductType]    Script Date: 4/12/2021 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_ProductType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PurchaseOrderHeader]    Script Date: 4/12/2021 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOrderHeader](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Source] [nvarchar](500) NOT NULL,
	[WarehouseId] [int] NOT NULL,
	[Remark] [nvarchar](500) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_PurchaseOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PurchaseOrderItems]    Script Date: 4/12/2021 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOrderItems](
	[Id] [int] NOT NULL,
	[HeaderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[BuyingPrice] [money] NOT NULL,
	[Currency] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_PurchaseOrderItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SaleOrderHeader]    Script Date: 4/12/2021 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaleOrderHeader](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[WarehouseId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[CashOnDelivery] [bit] NOT NULL,
	[AccountTransfer] [bit] NOT NULL,
	[TransferInformation] [nvarchar](500) NULL,
	[Remark] [nvarchar](500) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_SaleOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SaleOrderItems]    Script Date: 4/12/2021 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaleOrderItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HeaderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[SellingPrice] [money] NOT NULL,
	[OtherExpense] [money] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_SaleOrderItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TransferProductHeader]    Script Date: 4/12/2021 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransferProductHeader](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[FromWarehouseId] [int] NOT NULL,
	[ToWarehouseId] [int] NOT NULL,
	[Cost] [money] NOT NULL,
	[Remark] [nvarchar](500) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_TransferProductHeader] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TransferProductItems]    Script Date: 4/12/2021 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransferProductItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HeaderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_TransferProducts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Warehouse]    Script Date: 4/12/2021 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Warehouse](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Warehouse] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WarehouseProducts]    Script Date: 4/12/2021 5:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WarehouseProducts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WarehouseId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_WarehouseProducts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[PreOrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_PreOrders_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[PreOrderHeader] CHECK CONSTRAINT [FK_PreOrders_Customer]
GO
ALTER TABLE [dbo].[PreOrderItems]  WITH CHECK ADD  CONSTRAINT [FK_PreOrderItems_PreOrderHeader] FOREIGN KEY([HeaderId])
REFERENCES [dbo].[PreOrderHeader] ([Id])
GO
ALTER TABLE [dbo].[PreOrderItems] CHECK CONSTRAINT [FK_PreOrderItems_PreOrderHeader]
GO
ALTER TABLE [dbo].[PreOrderItems]  WITH CHECK ADD  CONSTRAINT [FK_PreOrderItems_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[PreOrderItems] CHECK CONSTRAINT [FK_PreOrderItems_Product]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Brand] FOREIGN KEY([BrandId])
REFERENCES [dbo].[Brand] ([Id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Brand]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductType] FOREIGN KEY([ProductTypeId])
REFERENCES [dbo].[ProductType] ([Id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_ProductType]
GO
ALTER TABLE [dbo].[PurchaseOrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseOrders_Product] FOREIGN KEY([WarehouseId])
REFERENCES [dbo].[Warehouse] ([Id])
GO
ALTER TABLE [dbo].[PurchaseOrderHeader] CHECK CONSTRAINT [FK_PurchaseOrders_Product]
GO
ALTER TABLE [dbo].[PurchaseOrderItems]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseOrderItems_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[PurchaseOrderItems] CHECK CONSTRAINT [FK_PurchaseOrderItems_Product]
GO
ALTER TABLE [dbo].[PurchaseOrderItems]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseOrderItems_PurchaseOrderHeader] FOREIGN KEY([HeaderId])
REFERENCES [dbo].[PurchaseOrderHeader] ([Id])
GO
ALTER TABLE [dbo].[PurchaseOrderItems] CHECK CONSTRAINT [FK_PurchaseOrderItems_PurchaseOrderHeader]
GO
ALTER TABLE [dbo].[SaleOrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_SaleOrders_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[SaleOrderHeader] CHECK CONSTRAINT [FK_SaleOrders_Customer]
GO
ALTER TABLE [dbo].[SaleOrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_SaleOrders_Warehouse] FOREIGN KEY([WarehouseId])
REFERENCES [dbo].[Warehouse] ([Id])
GO
ALTER TABLE [dbo].[SaleOrderHeader] CHECK CONSTRAINT [FK_SaleOrders_Warehouse]
GO
ALTER TABLE [dbo].[SaleOrderItems]  WITH CHECK ADD  CONSTRAINT [FK_SaleOrderItems_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[SaleOrderItems] CHECK CONSTRAINT [FK_SaleOrderItems_Product]
GO
ALTER TABLE [dbo].[SaleOrderItems]  WITH CHECK ADD  CONSTRAINT [FK_SaleOrderItems_SaleOrderHeader] FOREIGN KEY([HeaderId])
REFERENCES [dbo].[SaleOrderHeader] ([Id])
GO
ALTER TABLE [dbo].[SaleOrderItems] CHECK CONSTRAINT [FK_SaleOrderItems_SaleOrderHeader]
GO
ALTER TABLE [dbo].[TransferProductHeader]  WITH CHECK ADD  CONSTRAINT [FK_TransferProductHeader_Warehouse] FOREIGN KEY([FromWarehouseId])
REFERENCES [dbo].[Warehouse] ([Id])
GO
ALTER TABLE [dbo].[TransferProductHeader] CHECK CONSTRAINT [FK_TransferProductHeader_Warehouse]
GO
ALTER TABLE [dbo].[TransferProductHeader]  WITH CHECK ADD  CONSTRAINT [FK_TransferProductHeader_Warehouse1] FOREIGN KEY([ToWarehouseId])
REFERENCES [dbo].[Warehouse] ([Id])
GO
ALTER TABLE [dbo].[TransferProductHeader] CHECK CONSTRAINT [FK_TransferProductHeader_Warehouse1]
GO
ALTER TABLE [dbo].[TransferProductItems]  WITH CHECK ADD  CONSTRAINT [FK_TransferProductItems_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[TransferProductItems] CHECK CONSTRAINT [FK_TransferProductItems_Product]
GO
ALTER TABLE [dbo].[TransferProductItems]  WITH CHECK ADD  CONSTRAINT [FK_TransferProductItems_TransferProductHeader] FOREIGN KEY([HeaderId])
REFERENCES [dbo].[TransferProductHeader] ([Id])
GO
ALTER TABLE [dbo].[TransferProductItems] CHECK CONSTRAINT [FK_TransferProductItems_TransferProductHeader]
GO
ALTER TABLE [dbo].[WarehouseProducts]  WITH CHECK ADD  CONSTRAINT [FK_WarehouseProducts_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[WarehouseProducts] CHECK CONSTRAINT [FK_WarehouseProducts_Product]
GO
ALTER TABLE [dbo].[WarehouseProducts]  WITH CHECK ADD  CONSTRAINT [FK_WarehouseProducts_Warehouse] FOREIGN KEY([WarehouseId])
REFERENCES [dbo].[Warehouse] ([Id])
GO
ALTER TABLE [dbo].[WarehouseProducts] CHECK CONSTRAINT [FK_WarehouseProducts_Warehouse]
GO
