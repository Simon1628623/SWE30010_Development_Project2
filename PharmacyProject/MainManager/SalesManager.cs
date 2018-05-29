using System;
using System.Collections.Generic;

namespace PharmacyProject
{
	public class SalesManager
	{
		private List<SalesRecord> _sales_record = new List<SalesRecord>();

		public List<SalesRecord> SalesRecords
		{
			get { return _sales_record; }
		}

		//Cycles sales records to look for an unused ID
		public int GetUnusedID()
		{
			bool finished = true;
			int result = 1;
            result = _sales_record.Count;
            /*
			while (!finished)
			{
				finished = true;
				foreach (SalesRecord s in _sales_record)//Checks the sales records for current result
				{
					if (s.SalesId == result)
					{
						result++;
						finished = false;
					}
				}
			}*/

			return result;
		}

		public bool AddSalesRecord(DateTime date, StockItem item, int quantity, DataManager data)
		{
			int id = GetUnusedID();
			string dateString = date.ToString("dd/MM/yyyy");

			string command =
				"INSERT INTO SALESTABLE (Stock_Id, Sales_Quantity, Sales_Date)" +
				"VALUES(" + item.StockId + ", " + quantity +", '" + dateString + "');"
				;

			if (data.PushData(command))
			{
				//public SalesRecord(int salesId, StockItem item, int salesQuantity, DateTime salesDate)
				_sales_record.Add(new SalesRecord(GetUnusedID(), item, quantity, date));
				return true;
			}
			return false;
		}

		public bool EditSalesRecord(int id, DateTime date, StockItem item, int quantity, DataManager data)
		{

			foreach (SalesRecord s in _sales_record)
			{
				if (s.SalesId == id)
				{
					string command = "UPDATE SalesTable "
						+ "SET Stock_Id = " + item.StockId
						+ ", Sales_Quantity = " + quantity
						+ ", Sales_Date= " + date
						+ " Where Sales_Id = " + id + ";";
					if (data.PushData(command))
					{
						_sales_record.Remove(s);
						_sales_record.Add(new SalesRecord(id, item, quantity, date));
						return true;
					}
				}
			}
			return false;
		}

		public List<SalesView> displaySales()
		{
			List<SalesView> result = new List<SalesView>();

			foreach (SalesRecord s in SalesRecords)
			{
				result.Add(new SalesView(s.StockItm.StockName, s.SaleQuantity, s.SalesDate));
			}

			return result;
		}

		public SalesManager(List<SalesRecord> salesIn)
		{
			_sales_record = salesIn;
		}
	}
}

