using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace PharmacyProject
{
	public class DataManager
	{
		//private SqlConnection connection;
		private string connectionString;
		//private int selected;

		//-username, password, host, sales_tbl, stock_tbl, login_tbl: string
		//+SalesTbl, StockTbl, LoginTbl <<read-only property>>

		//public bool OpenAccess()
		//public bool CloseAccess()

		private List<StockItem> StockList = new List<StockItem>();  //Stores Stocklist so stock items can be attached to sales items

		public List<StockItem> GetStock()
		{
			List<StockItem> result = new List<StockItem>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				//Form Query
				SqlCommand command = new SqlCommand("Select * from StockTable;", connection);

				//Execute Query
				connection.Open();
				using (SqlDataReader oReader = command.ExecuteReader())
				{
					while (oReader.Read())
					{
						//Add each entry to the stockItem List;
						result.Add(
							new StockItem(
								Convert.ToString(oReader["Stock_Name"]),
								Convert.ToString(oReader["Stock_Type"]),
								Convert.ToInt32(oReader["Stock_Id"])
							//, Convert.ToInt32(oReader["Stock_Level"])
							));
					}
					connection.Close();
					return result;
				}
			}


			//DUMMY DATA - FINISH THIS
			/*StockList = new List<StockItem>
			{
				new StockItem("panadol", "pain killer", 100001),
				new StockItem("Nurofen", "pain killer", 100002),
				new StockItem("Telfast", "allergy", 100003),
				new StockItem("Claretine", "allergy", 100004),
				new StockItem("Zink", "Supliment", 100005),
				new StockItem("Vitimin B", "Supliment", 100006)
			};
			return StockList;*/
		}

		//REDO - OR REMOVE???
		//Finds stock item to be added in sales item
		public StockItem FindStock(int barcode)
		{
			foreach (StockItem s in StockList)
			{
				if (barcode == s.StockId)
				{
					return s;
				}
			}

			return null;//NEED TO ADD CHECKS FOR NULL AND RETURN AN ERROR IF THEY APPEAR
		}

		public DateTime ParseDate(string date)
		{
			return DateTime.ParseExact(date, "dd/MM/yyyy", null);
		}

		public List<SalesRecord> GetSales(List<StockItem> StockItems)
		{
			List<SalesRecord> result = new List<SalesRecord>();

			//using (connection)
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				//Form Query
				SqlCommand command = new SqlCommand("Select * from SalesTable;", connection);

				//Execute Query
				connection.Open();
				using (SqlDataReader oReader = command.ExecuteReader())
				{
					while (oReader.Read())
					{
						int StockId = Convert.ToInt32(oReader["Stock_Id"]);
						StockItem stockLink = new StockItem(null, null, 0);//This will always be initiated properly bellow.

						//Find Stock Item
						foreach (StockItem s in StockItems)
						{
							if (s.StockId == StockId)
							{
								stockLink = s;
							}
						}
						//Add each entry to the stockItem List;
						//public SalesRecord(int salesId, StockItem item, int salesQuantity, DateTime salesDate)
						result.Add(
							new SalesRecord(
								Convert.ToInt32(oReader["Sales_Id"]),
								stockLink,
								Convert.ToInt32(oReader["Sales_Quantity"]),
								Convert.ToDateTime(oReader["Sales_Date"])
							));
					}
					connection.Close();
					return result;
				}
			}



			//DUMMY DATA - FINISH THIS
			/*
			return new List<SalesRecord>
			{
				new SalesRecord(ParseDate("11/11/2011"), FindStock(100001), 1, 1),
				new SalesRecord(ParseDate("11/11/2011"), FindStock(100002), 1, 2),
				new SalesRecord(ParseDate("11/11/2011"), FindStock(100003), 1, 3),
				new SalesRecord(ParseDate("11/11/2011"), FindStock(100004), 1, 4),
				new SalesRecord(ParseDate("11/11/2011"), FindStock(100005), 1, 5),
				new SalesRecord(ParseDate("11/11/2011"), FindStock(100006), 1, 6)
			};
			*/
		}
		//public List<User> LoginIn()

		//Push Data Methods
		public bool PushData(string query)  //OVERLOADED TO TAKE A STRING
		{
			SqlCommand command = new SqlCommand(query);

			return PushData(command);
		}
		public bool PushData(SqlCommand command)
		{
			//Initialising connection and command
			//SqlConnection connection = new SqlConnection(connectionString);

			int r;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command.Connection = connection;

				//using (connection)
				using (command)
				{
					r = command.ExecuteNonQuery();
					//r = (int)command.ExecuteScalar();
				}

				connection.Close();
			}
			//if wanted refresh put code here
			//PopulateTable("StockTable");

			//Check if change successful.
			if (r == 0)
				return false;
			else
				return true;

		}

		//Pull Data Methods
		public DataTable PullData(string query)//Returns a DataTable filled with data from the database, based on the input query;
		{
			SqlCommand command = new SqlCommand(query);
			return PullData(command);
		}
		public DataTable PullData(SqlCommand command) 
		{
			DataTable result = new DataTable();
			
			//Open Connection
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				
				//Set Command
				using (command)
				{
					//Fill Data Table Using Command
					using (SqlDataReader dr = command.ExecuteReader())
					{
						result.Load(dr);
						connection.Close();
					}
				}
			}
			return result;
		}

		public void exportReport(string path, string col1, string col2, List<Report> reports)
		{
			StringBuilder output = new StringBuilder();
			output.Append(col1).Append(",").Append(col2).AppendLine();

			foreach (Report r in reports)
			{
				output.Append(r.Stock).Append(",").Append(r.Amount).AppendLine();
			}

			File.WriteAllText(path, output.ToString());
		}

		//THIS NEEDS FINISHING
		public DataManager()
		{
			connectionString = ConfigurationManager.ConnectionStrings["PharmacyProject.Properties.Settings.PharmacyDatabaseConnectionString"].ConnectionString;
			//connection = new SqlConnection(connectionString);
		}
	}
}

