using System;
using System.Collections.Generic;

namespace PharmacyProject
{
	public class StockManager
	{
		private List<StockItem> _items = new List<StockItem>();
		private List<string> _catagories = new List<string>();

		public List<StockItem> Items
		{
			get { return _items; }
		}

		public List<string> Categories
		{
			get { return _catagories; }
		}

		//THIS IS PROBABLY NOT NEEDED
		/*public StockTypes//property type and return?
		{
			get{}
			set{}
		}*/

		//THIS ISN'T NEEDED
		public bool FindCatagory(string type)
		{
			foreach (string s in Categories)
			{
				if (type == s)
				{
					return true;
				}
			}
			return false;
		}
		//THIS ISN'T NEEDED
		public StockItem FindStock(string name)
		{
			foreach (StockItem s in _items)
			{
				if (s.StockName == name)
				{
					return s;
				}
			}
			return null;//This will never be returned.
		}

		public bool AddStock(string ItemName, string StockType, int Barcode, DataManager data)
		{
			//OLD CODE
			/*if (FindCatagory(ItemType))
			{
				Stock.Add(new StockItem(ItemName, ItemType, Barcode));
			}
			else
			{
				//Console.WriteLine("Item Type not found");
				return false;
			}*/

			//NEED TO INCORPORATE ITEM CODE HERE
			string query = 
				"INSERT INTO StockTable (Stock_Name, Stock_Type, Stock_Level)" +
				"VALUES('" + ItemName + "', '"+ StockType + "', " + nextStockID() + ");";
			if (data.PushData(query))
			{
				Items.Add(new StockItem(ItemName, StockType, 0));
				return true;
			}
			return false;
		}

		//USED TO GET THE NEXT UNUSED STOCK ID
		private int nextStockID()
		{
			int result = 0;
			foreach (StockItem s in _items)
			{
				if (s.StockId > result)
				{
					result = s.StockId;
				}
			}
			result++;
			return result;
		}

		public StockManager(List<StockItem> stockin)
		{
			_items = stockin;

			//--Catagories--
			Categories.Add("pain killer");
			Categories.Add("allergy");
			Categories.Add("Supliment");
		}
	}
}

