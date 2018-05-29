CREATE TABLE [dbo].StockTable
(
	[Stock_Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Stock_Name] VARCHAR(50) NOT NULL, 
    [Stock_Type] CHAR(10) NOT NULL, 
    [Stock_Level] INT NOT NULL
)
