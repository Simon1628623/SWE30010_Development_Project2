using System;
using System.Collections.Generic;

namespace PharmacyProject
{
	public class SalesView
	{
		private string _product;
		private int _quantity;
		private DateTime _date;

		public string Product
		{
			get { return _product; }
		}

		public int Quantity
		{
			get { return _quantity; }
		}

		public DateTime Date
		{
			get { return _date; }
		}

		public SalesView(string product, int quantity, DateTime date)
		{
			_product = product;
			_quantity = quantity;
			_date = date;
		}
	}
}
