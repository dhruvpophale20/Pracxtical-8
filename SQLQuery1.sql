
IF DB_ID(N'InventryDB') IS NULL
BEGIN
    CREATE DATABASE InventryDB;
END
GO

USE InventryDB;
GO

IF OBJECT_ID(N'dbo.Products', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Products
    (
        ProductId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
        ProductName NVARCHAR(100) NOT NULL,
        Quantity INT NOT NULL CONSTRAINT DF_Products_Quantity DEFAULT (0),
        Price INT NOT NULL,
        CONSTRAINT CK_Products_Quantity_NonNegative CHECK (Quantity >= 0),
        CONSTRAINT CK_Products_Price_NonNegative CHECK (Price >= 0)
    );
END
GO
