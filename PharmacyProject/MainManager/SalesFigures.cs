using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyProject
{
    public class SalesFigures
    {

        private SalesManager sales;
        private StockManager stock;
       
        private List<Report> _MonthlySales = new List<Report>();
        private List<Report> _WeeklySales = new List<Report>();

        public SalesFigures(SalesManager s, StockManager st)
        {
            sales = s;
            stock = st;

            //getFigures();

        }

		//void getAllPredictions()
		//private void getFigures()
		private void getFigures()
		{
            //Reset Predictions
            _MonthlySales.Clear();
            _WeeklySales.Clear();
            foreach (StockItem s in stock.Items)
            {
                _MonthlySales.Add(getLastMonthSales(s.StockId));
                _WeeklySales.Add(getLastWeekSales(s.StockId));
            }
		}

		//Return a list of weekly or monthly sales figures
		public List<Report> returnFigures(timeFrame frame)
		{
			getFigures();

			switch (frame)
			{
				case timeFrame.week:
					return _MonthlySales;
				case timeFrame.month:
					return _MonthlySales;
			}
			return new List<Report>();//This should never be reached, it's just here so it will compile
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

		//last months sales
		Report getLastMonthSales(int prod_num)
        {
            List<int> sales = getAllMonthSales(prod_num);
			return new Report(GetStockName(prod_num), sales[sales.Count-1]);
		}

		//get last weeks sales
		Report getLastWeekSales(int prod_num)
        {
            List<int> sales = getAllWeeklySales(prod_num);
            return new Report(GetStockName(prod_num), sales[sales.Count-1]);
        }

        //get each months total of sales
        List<int> getAllMonthSales(int prod_num)
        {
            List<int> result = new List<int>();
            DateTime Month = sales.SalesRecords[0].SalesDate;
           // Console.WriteLine("prod No:" + prod_num);
            int val = 0;

            //foreach(SalesRecord s in _sales)
            foreach (SalesRecord s in sales.SalesRecords)
            {
                if (s.StockItm.StockId == prod_num)
                {
                    if (SameMonth(Month, s.SalesDate))
                    {
                        //Console.WriteLine("adding:" + s.SaleQuantity + " to " + val);
                        val += s.SaleQuantity;
                        // Console.WriteLine("val=" + val);
                    }
                    else
                    {
                        //Console.WriteLine("end of month:" + val);
                        result.Add(val);
                        val = 0;
                        Month = s.SalesDate;
                    }
                }
            }
            //Console.WriteLine("total of months:" + result.Count);

			//Check if result is zero
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


        public List<Report> MonthlySales
        {
            get { return _MonthlySales; }
        }

        public List<Report> WeeklySales
        {
            get { return _WeeklySales; }
        }
    }

}
