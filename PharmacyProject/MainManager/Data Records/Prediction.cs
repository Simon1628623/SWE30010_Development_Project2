using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyProject
{
	public class Prediction
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

		public Prediction(string stock, int amount)
		{
			_stock = stock;
			_amount = amount;
		}
	}
}
