using System;
//using PHP;

namespace PharmacyProject
{
	public enum timeFrame { week, month };

	public class MainManager
	{
		private StockManager _stock;
		private SalesManager _sales;
		private DataManager _data;
        private SalesPredictions _predictions;
		private SalesFigures _figures;

		public StockManager Stock
		{
			get { return _stock; }
		}

		public SalesManager Sales
		{
			get { return _sales; }
		}

		public DataManager Data
		{
			get { return _data; }
		}

		public SalesPredictions Predictions
		{
			get { return _predictions; }
		}


		public SalesFigures Figures
        {
            get { return _figures; }
        }

		public MainManager()
		{
			_data = new DataManager();
			_stock = new StockManager(Data.GetStock());
			_sales = new SalesManager(Data.GetSales(_stock.Items));
			_predictions = new SalesPredictions(_sales, _stock);
			_figures = new SalesFigures(_sales, _stock);
		}

		/*
		public void Exit()
		{
			Data.Exit();
		}
		*/
	}
}

