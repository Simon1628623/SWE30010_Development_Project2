using System;
namespace PharmacyProject
{
	public class StockItem
	{
		private int stock_id;
		private string stock_name;
		private string stock_type;
		private int stock_level;


		public string StockName
		{
			get { return stock_name; }
		}

		public string StockType
		{
			get { return stock_type; }
		}

		public int StockId
		{
			get { return stock_id; }
		}

		public StockItem(string stockName, string stockType, int stockId)
		{
			stock_name = stockName;
			stock_type = stockType;
			stock_id = stockId;
			stock_level = 0;
		}
	}
}