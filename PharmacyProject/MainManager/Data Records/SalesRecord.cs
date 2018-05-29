using System;
namespace PharmacyProject
{
	public class SalesRecord
	{
		private int sales_id;
		private DateTime sales_date;
		private StockItem stock_item;
		private int sales_quantity;

		public int SalesId
		{
			get { return sales_id; }
			set { sales_id = value; }
		}

		public DateTime SalesDate
		{
			get { return sales_date; }
			set { sales_date = value; }
		}

		public int SaleQuantity
		{
			get { return sales_quantity; }
			set { sales_quantity = value; }
		}

		public StockItem StockItm
		{
			get { return stock_item; }
			set { stock_item = value; }
		}

		//public SalesRecord(DateTime date, StockItem item, int quantity, int id)
		public SalesRecord(int salesId, StockItem item, int salesQuantity, DateTime salesDate)
		{
			//_date = Convert.ToDateTime(date);
			sales_date = salesDate;
			stock_item = item;
			sales_quantity = salesQuantity;
			sales_id = salesId;
		}
	}
}