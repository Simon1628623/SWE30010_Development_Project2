using System;
using System.Collections.Generic;
using System.Globalization;

namespace PharmacyProject
{
    public class SalesPredictions
    {

        private SalesManager sales;
        private StockManager stock;
		//private List<int> _monthPredictions = new List<int>();
		//private List<int> _weekPredictions = new List<int>();
		private List<Report> _monthPredictions = new List<Report>();
		private List<Report> _weekPredictions = new List<Report>();

        private List<Report> _monthlySales = new List<Report>();
        private List<Report> _weeklySales = new List<Report>();

        public SalesPredictions(SalesManager s, StockManager st)
        {
            sales = s;
            stock = st;
            
            getAllPredictions();
            Console.WriteLine(ProductCount());
            for (int i = 0; i < ProductCount(); i++)
            {
                Console.WriteLine(i + " month: " + _monthPredictions[i].Stock + ":" + _monthPredictions[i].Amount);
                Console.WriteLine(i + " week: " + _weekPredictions[i].Stock + ":" + _weekPredictions[i].Amount);
            }          
        }

		//void getAllPredictions()
		private void getAllPredictions()
		{
            //Reset Predictions
            _monthPredictions.Clear();
            _weekPredictions.Clear();
            //for (int i = 0; i < ProductCount(); i++)
            foreach(StockItem s in stock.Items)
			{
				//MonthPredictions += getMonthPredictions(i);
				//WeekPredictions += getWeeklyPredictions(i);

				//Get New Predictions
				_monthPredictions.Add(getMonthPredictions(s.StockId));
				_weekPredictions.Add(getWeeklyPredictions(s.StockId));
			}
        }

		//Return prediction to be passed to GUI
		public List<Report> getPrediction(timeFrame frame)
		{
			//Reset the predictions
			getAllPredictions();

			//Return 
			switch (frame)
			{
				case timeFrame.week:
					return _weekPredictions;
				case timeFrame.month:
					return _monthPredictions;
			}

			return new List<Report>();//This should never be returned, it's just there for semantics. 
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

		//get individual product sales predictions
		Report getMonthPredictions(int prod_num)
        {
            int amount = 0;
            List<int> sales = getAllMonthSales(prod_num);
            foreach(int g in sales)
			//for (int i = 0; i < sales.Count; i++)
			{
				amount += g;
            }
            //result /= sales.Length;
            if(amount != 0)
                amount /= sales.Count;
			return new Report(GetStockName(prod_num), amount);
        }

        //individual product weekly predictions
        Report getWeeklyPredictions(int prod_num)
        {
            int amount = 0;
			//int[] sales = getAllWeeklySales(prod_num);

			//Get All Weekly Sales
			int[] sales = getAllWeeklySales(prod_num).ToArray();
			for (int i = 0; i < sales.Length; i++)
            {
				amount += sales[i];
            }
			amount /= sales.Length;

			return new Report(GetStockName(prod_num), amount);
        }

        //get each months total of sales
        List<int> getAllMonthSales(int prod_num)
        {
            List<int> result = new List<int>();
			//DateTime Month = _sales[0].Date;
			DateTime Month = sales.SalesRecords[0].SalesDate;
            Console.WriteLine("prod No:"+prod_num);
			int val = 0;
			//string query = "SELECT" + SalesQuanitity + "FROM " + salesTable + "WHERE" + prod_num + " = " + productNumber;
			
			//foreach(SalesRecord s in _sales)
			foreach (SalesRecord s in sales.SalesRecords)
			{
                if (s.StockItm.StockId == prod_num)
                {
                    if (s.StockItm.StockId == prod_num && SameMonth(Month, s.SalesDate))
                    {
                        Console.WriteLine("adding:" + s.SaleQuantity + " to " + val);
                        val += s.SaleQuantity;
                        Console.WriteLine("val=" + val);
                    }
                    else
                    {
                        Console.WriteLine("end of month:" + val);
                        result.Add(val);
                        val = 0;
                        val += s.SaleQuantity;
                        Month = s.SalesDate;
                    }
                }
                    
            }
            if (result.Count == 0)
            {
                result.Add(val);
            }
            Console.WriteLine("total of months:" + result.Count);

			return result;
        }

        //get each weeks total of sales
        List<int> getAllWeeklySales(int prod_num)
        {
            List<int> result = new List<int>();
			//string query = "SELECT" + SalesQuanitity + "FROM " + salesTable + "WHERE" + prod_num + " = " + productNumber;
			
			// DateTime Date = _sales[0].Date;
			DateTime Date = sales.SalesRecords[0].SalesDate;
			int val = 0;
            foreach (SalesRecord s in sales.SalesRecords)
            {
                if (s.StockItm.StockId == prod_num)
                {
                    if (SameWeek(Date, s.SalesDate))
                    {
                        val += s.SaleQuantity;
                    }
                    else
                    {
                        result.Add(val);
                        val = 0;
                        Date = s.SalesDate;
                    }
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
                Console.WriteLine("same month");
            }               
            return result;
        }

        bool SameWeek(DateTime dt, DateTime dt2)
        {
            bool result = false;
			//if (dt.AddDays(7 - dt.DayOfWeek.Date.Equals(dt2.AddDays(7 - dt2.DayOfWeek).Date))
			if ((dt.AddDays(7 - (int)dt.DayOfWeek).Date.Equals(dt2.AddDays(7 - (int)dt2.DayOfWeek).Date)))
			{
				result = true;
            }
			return result;
        }

        //calculate total number of products
        public int ProductCount()
        {
			//string sql = "SELECT COUNT(*) FROM " + stockTable;
			//return stock.StockCount();
			return stock.Items.Count;
		}


        public List<Report> MonthlyPredictions
        {
            get { return _monthPredictions; }
        }

        public List<Report> WeeklyPredictions
		{
            get { return WeeklyPredictions; }
        }

        
    }

}
