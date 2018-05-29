using System;
//using System.Collections.Generic;

namespace PharmacyProject
{
	public class Report
	{
		private string _stock;
		private int _amount;

		public string Stock
		{
			get { return _stock; }
		}

		public int Amount
		{
			get { return _amount; }
		}

		public Report(string stock, int amount)
		{
			_stock = stock;
			_amount = amount;
		}
	}
}
