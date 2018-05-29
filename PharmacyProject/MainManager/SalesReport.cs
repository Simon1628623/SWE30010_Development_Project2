using System;
using System.Collections.Generic;
using System.Globalization;


namespace PharmacyProject
{
    public class SalesReport
    {
    	private SalesManager sales;
        private StockManager stock;
		private List<SalesReport> _monthSales = new List<SalesReport>();
		private List<SalesReport> _weekSales = new List<SalesReport>();

		public SalesReport(SalesManager s, StockManager st)
		{
			sales = s;
            stock = st;
		}

    	//void getAllSales()
		private void getAllSales()
		{
			for (int i = 1; i <= ProductCount(); i++)
			{
				//Reset Sales
				_monthSales = new List<SalesReport>();
				_weekSales = new List<SalesReport>();

				//Get New Sales
				_monthSales.Add(getAllMonthSales(i));
				_weekSales.Add(getAllWeeklySales(i));
			}
        }

    	//Return sales to be passed to GUI
		public List<SalesReport> getSales(timeFrame frame)
		{
			//Reset the sales
			getAllSales();

			//Return 
			switch (frame)
			{
				case timeFrame.week:
					return _weekSales;
				case timeFrame.month:
					return _monthSales;
			}

			return new List<SalesReport>();
		}

		//Get a stock products name
		string GetStockName(int stockID)
		{
			foreach (StockItem s in stock.Items)
			{
				if (s.StockId == stockID)
				{
					return s.StockName;
				}
			}
			return "ERROR: Name Not Found";
		}

		/*
		//get individual product sales
		SalesReport getMonthSales(int prod_num)
        {
            int amount = 0;
            List<int> sales = getAllMonthSales(prod_num);
			for (int i = 0; i < sales.Count; i++)
			{
				amount += sales[i];
            }
			//amount /= sales.Count;
			return new SalesReport(GetStockName(prod_num), amount);
        }

		//individual product weekly sales
        SalesReport getWeeklySales(int prod_num)
        {
            int amount = 0;

			//Get All Weekly Sales
			int[] sales = getAllWeeklySales(prod_num).ToArray();
			for (int i = 0; i < sales.Count; i++)
            {
				amount += sales[i];
            }
			//amount /= sales.Count;
			return new SalesReport(GetStockName(prod_num), amount);
        }
		*/

		//get each months total of sales
        List<int> getAllMonthSales(int prod_num)
        {
            List<int> result = new List<int>();
			DateTime Month = sales.SalesRecords[0].SalesDate;  

			int val = 0;
		
			foreach (SalesRecord s in sales.SalesRecords)
			{               
                if (s.SalesId == prod_num && SameMonth(Month, s.SalesDate))
                {
					val += s.SaleQuantity;
                }
				else if (s.SalesId == prod_num)
				{
                    result.Add(val);
                    val = 0;
                    Month = s.SalesDate;
                }
            }
			//Check if result has zero entries
			if (result.Count == 0)
			{
				result.Add(val);
			}

			return result;
        }

 		//get each weeks total of sales
        List<int> getAllWeeklySales(int prod_num)
        {
            List<int> result = new List<int>();
			
			DateTime Date = sales.SalesRecords[0].SalesDate;
			int val = 0;
            foreach (SalesRecord s in sales.SalesRecords)
            {
                if (s.SalesId == prod_num && SameWeek(Date, s.SalesDate))
                {
                    val += s.SaleQuantity;
                }
                else if (s.SalesId == prod_num)
                {
                    result.Add(val);
                    val = 0;
                    Date = s.SalesDate;
                }
            }
			
			//Check if result has zero entries
			if (result.Count == 0)
			{
				result.Add(val);
			}

			return result;
        }

    	bool SameMonth(DateTime dt, DateTime dt2)
        {
            bool result = false;
            if (dt.Month == dt2.Month)
            {
                result = true;
            }               
            return result;
        }

        bool SameWeek(DateTime dt, DateTime dt2)
        {
            bool result = false;
	
			if ((dt.AddDays(7 - (int)dt.DayOfWeek).Date.Equals(dt2.AddDays(7 - (int)dt2.DayOfWeek).Date)))
			{
				result = true;
            }
			return result;
        }

        //mysql to calculate total number of products
        public int ProductCount()
        {
			return stock.Items.Count;
		}

    	public List<SalesReport> MonthlySales
        {
            get { return _monthSales; }
        }

        public List<SalesReport> WeeklySales
		{
            get { return WeeklySales; }
        }
    }
}