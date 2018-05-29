CREATE TABLE [dbo].SalesTable
(
	[Sales_Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Stock_Id] INT NOT NULL, 
    [Sales_Quantity] INT NOT NULL, 
    [Sales_Date] DATE NOT NULL, 
    CONSTRAINT StockFK FOREIGN KEY (Stock_Id) REFERENCES StockTable(Stock_Id) 
)
