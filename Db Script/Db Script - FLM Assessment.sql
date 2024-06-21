-- Creatde Db is it does Not exist
USE master;
GO

DECLARE @DatabaseName nvarchar(50)
SET @DatabaseName = N'FLM'

DECLARE @SQL varchar(max)

SELECT @SQL = COALESCE(@SQL,'') + 'KILL ' + CONVERT(VARCHAR, SPID) + ';'
FROM MASTER..SysProcesses
WHERE DBID = DB_ID(@DatabaseName) AND SPId <> @@SPID

--SELECT @SQL 
EXEC(@SQL)

-- ALTER DATABASE [FLM] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
-- GO
	
DROP DATABASE IF EXISTS FLM;

CREATE DATABASE FLM;
GO

USE FLM;
GO

-- Create the Branch table if it does not exist.
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'FLM' AND  TABLE_NAME = 'Branch')
BEGIN
	CREATE TABLE Branch (
		ID INT NOT NULL
		,[Name] VARCHAR(100)
		,TelephoneNumber VARCHAR(15)
		,OpenDate DATETIME
		,CONSTRAINT PK_BranchID PRIMARY KEY(ID)
	);
END

-- Create the Product table if it does not exist.
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'FLM' AND  TABLE_NAME = 'Product')
BEGIN
	CREATE TABLE Product (
		ID INT NOT NULL
		,[Name] VARCHAR(100) NOT NULL
		,WeightedItem BIT
		,SuggestedSellingPrice DECIMAL(18, 2)
		,CONSTRAINT PK_ProductID PRIMARY KEY(ID)
	);
END

-- Create the BranchProduct table if it does not exist.
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'FLM' AND  TABLE_NAME = 'BranchProduct')
BEGIN
	CREATE TABLE BranchProduct (
		BranchProductID INT IDENTITY NOT NULL
		,BranchID INT
		,ProductID INT
		,CONSTRAINT PK_BranchProductID PRIMARY KEY(BranchProductID)
		,CONSTRAINT FK_Branch_BranchID FOREIGN KEY (BranchID) REFERENCES Branch(ID)
		,CONSTRAINT FK_Product_ProductID FOREIGN KEY (ProductID) REFERENCES Product(ID)
	);
END






SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteBranch')
	DROP PROCEDURE DeleteBranch
GO

CREATE PROCEDURE DeleteBranch     
(        
   @BranchId INT       
)        
AS      
BEGIN        
   DELETE FROM Branch WHERE Id=@BranchId        
END;



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetProductsByBranch')
	DROP PROCEDURE GetProductsByBranch
GO

CREATE PROCEDURE GetProductsByBranch AS
BEGIN
	SELECT b.Name AS BranchName, STRING_AGG(p.Name, ', ') AS ProductNames
	FROM BranchProduct bp
	INNER JOIN Branch b ON bp.BranchId = b.ID
	INNER JOIN Product p ON bp.ProductId = p.ID
	GROUP BY b.Name
	ORDER BY b.Name;
END;