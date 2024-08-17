
CREATE TABLE [Cart] (
	[CartId] NVARCHAR(255) NOT NULL UNIQUE,
	[UserId] NVARCHAR(255),
	[DateCreated] DATETIME,
	[User]  DEFAULT  public virtual User User { get; set; },
	[CartItems]  DEFAULT public virtual ICollection<CartItem> CartItems { get; set; }
);
GO

CREATE INDEX [id_index]
ON [Cart] ([id]);
GO

CREATE TABLE [CartItems] (
	[CartItemId] NVARCHAR(255) NOT NULL UNIQUE,
	[CartId] NVARCHAR(255),
	[ProductId] NVARCHAR(255),
	[Quantity] INT,
	[Cart]  DEFAULT public Cart Cart { get; set; },
	[Product]  DEFAULT   public Product Product { get; set; },
	PRIMARY KEY([CartItemId])
);
GO

CREATE TABLE [User] (
	[UserId] NVARCHAR(255),
	PRIMARY KEY([UserId])
);
GO

CREATE TABLE [Products] (
	[ProductId] NVARCHAR(255) NOT NULL UNIQUE,
	[ProductName] NVARCHAR(255),
	[Description] NVARCHAR(255),
	[ImageUrl] NVARCHAR(255),
	[CategoryId] NVARCHAR(255),
	[Price] DECIMAL,
	[BrandId] INT,
	[Vote] INT,
	[Quantity] INT,
	PRIMARY KEY([ProductId])
);
GO

CREATE TABLE [Category] (
	[CategoryId] NVARCHAR(255) NOT NULL UNIQUE,
	[CategoryName] NVARCHAR(255),
	--         public virtual ICollection<Product> Products { get; set; }

	[Products] ,
	PRIMARY KEY([CategoryId])
);
GO

CREATE TABLE [ProductDetail] (
	[ProductDetailId] NVARCHAR(255) NOT NULL UNIQUE,
	[ProductId] NVARCHAR(255),
	[ColorId] NVARCHAR(255),
	[SizeId] NVARCHAR(255),
	[Product] ,
	[Color] ,
	[Size] ,
	PRIMARY KEY([ProductDetailId])
);
GO

CREATE TABLE [Order] (
	[OrderId] INT NOT NULL IDENTITY UNIQUE,
	[UserId] NVARCHAR(255),
	[TotalPrice] DECIMAL,
	[OrderDate] DATETIME,
	-- public enum OrderStatus
    {
        Processing,
        Completed,
        Canceled
    }

	[Status]  DEFAULT    public OrderStatus Status { get; set; },
	--         public ICollection<OrderDetail> OrderDetails { get; set; }

	[OrderDetails] ,
	[Product]  DEFAULT  public virtual Product Product { get; set; },
	[Order]  DEFAULT    public virtual Order Order { get; set; },
	PRIMARY KEY([OrderId])
);
GO

CREATE TABLE [OrderDetail] (
	[OrderDetailId] INT NOT NULL IDENTITY UNIQUE,
	[OrderId] INT,
	[ProductId] NVARCHAR(255),
	[Quantity] INT,
	[Price] DECIMAL,
	--  public enum OrderDetailStatus
    {
        Pending,
        Shipped,
        Delivered,
        Returned
    }
	[Status]  DEFAULT  public OrderDetailStatus Status { get; set; },
	PRIMARY KEY([OrderDetailId])
);
GO

CREATE TABLE [Size] (
	[SizeId] NVARCHAR(255) NOT NULL UNIQUE,
	[SizeName] NVARCHAR(255),
	--         public virtual ICollection<ProductDetail> ProductDetails { get; set; }

	[ProductDetails] ,
	PRIMARY KEY([SizeId])
);
GO

CREATE TABLE [Color] (
	[ColorId] NVARCHAR(255) NOT NULL UNIQUE,
	[ColorName] NVARCHAR(255),
	--  public virtual ICollection<ProductDetail> ProductDetails { get; set; }
	[ProductDetails] ,
	PRIMARY KEY([ColorId])
);
GO

CREATE TABLE [Brand] (
	[BrandId] INT NOT NULL IDENTITY UNIQUE,
	[BrandName] NVARCHAR(255),
	[ImageUrl] NVARCHAR(255),
	--  public virtual ICollection<Product> Products { get; set; }
	[Products] ,
	PRIMARY KEY([BrandId])
);
GO

ALTER TABLE [ProductDetail]
ADD FOREIGN KEY([SizeId]) REFERENCES [Size]([SizeId])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [ProductDetail]
ADD FOREIGN KEY([ProductId]) REFERENCES [Products]([ProductId])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [Cart]
ADD FOREIGN KEY([UserId]) REFERENCES [User]([UserId])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [CartItems]
ADD FOREIGN KEY([CartId]) REFERENCES [Cart]([CartId])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [Order]
ADD FOREIGN KEY([UserId]) REFERENCES [User]([UserId])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [Products]
ADD FOREIGN KEY([ProductId]) REFERENCES [CartItems]([ProductId])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [Products]
ADD FOREIGN KEY([BrandId]) REFERENCES [Brand]([BrandId])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [ProductDetail]
ADD FOREIGN KEY([ColorId]) REFERENCES [Color]([ColorId])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [Products]
ADD FOREIGN KEY([CategoryId]) REFERENCES [Category]([CategoryId])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [OrderDetail]
ADD FOREIGN KEY([OrderId]) REFERENCES [Order]([OrderId])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO