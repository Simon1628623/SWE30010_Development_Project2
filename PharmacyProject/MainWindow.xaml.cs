using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Text.RegularExpressions;


namespace Pharmacy_GUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>

	//aliasing the Enum from MainManager, for easy access
	using timeFrame = PharmacyProject.timeFrame; 

	public partial class MainWindow : Window
	{
		//Regular Expression
		private Regex num_pattern = new Regex("[0-9]{1,20}");
		private Regex alpha_num_pattern = new Regex("[A-Za-z0-9]{1,20}");

		//Default Dropdown Result
		string defaultDropdown = "Please Select";

		//Time Period List
		private List<string> timeFrameList = new List<String>()
		{"Weekly", "Monthly"};

		//CurrentTimeFrame
		private timeFrame currentTimeFrame;

		PharmacyProject.MainManager Manager = new PharmacyProject.MainManager();

		//private List<string> catagories;
		private List<string> products;

		/*private List<string> test_dropdown = new List<string>()	//TESTING ONLY - REMOVE LATER
		{
			"Test1",
			"Test 2"
		};*/
		//Constructor
		public MainWindow()
		{
			InitializeComponent();

			//View Sales Visible by Default
			HidePanels();
			LoadViewSales();
			Panel_ViewSales.Visibility = Visibility.Visible;
		}

		//Populate ComboBoxes
		private void Populate(ComboBox Dropdown, List<string> options)
		{
			Dropdown.Items.Clear();
			Dropdown.Items.Add(defaultDropdown);
			foreach (string s in options)
			{
				Dropdown.Items.Add(s);
			}
		}

//----------------
//TOP MENU BUTTONS
//----------------

		private void AddSales_Click(object sender, RoutedEventArgs e)
		{
			LoadAddSales();
		}
		private void AddStock_Click(object sender, RoutedEventArgs e)
		{
			LoadAddStock();
		}
		private void ViewSales_Click(object sender, RoutedEventArgs e)
		{
			LoadViewSales();
		}

		private void Button_WeeklyPrediction_Clicked(object sender, RoutedEventArgs e)
		{
			LoadPredictions(timeFrame.week);
		}
		private void button_MonthlyPredictions_Click(object sender, RoutedEventArgs e)
		{
			LoadPredictions(timeFrame.month);
		}

		private void Button_WeeklySales_Click(object sender, RoutedEventArgs e)
		{
			LoadReport(timeFrame.week);
		}

		private void Button_MonthlySales_Click(object sender, RoutedEventArgs e)
		{
			LoadReport(timeFrame.month);
		}

		//-------------
		//OTHER BUTTONS
		//-------------	

		//Export Predictions
		private void PredictionExport_Click(object sender, RoutedEventArgs e)
		{
			if (currentTimeFrame == timeFrame.month)
			{
				Manager.Data.exportReport("monthlyPrediction.csv", "item", "quantity", Manager.Predictions.getPrediction(timeFrame.month));
			}
			else if (currentTimeFrame == timeFrame.week)
			{
				Manager.Data.exportReport("weeklyPrediction.csv", "item", "quantity", Manager.Predictions.getPrediction(timeFrame.week));
			}
		}

		//Export Sales Report
		private void Button_SalesExport_Click(object sender, RoutedEventArgs e)
		{
			if (currentTimeFrame == timeFrame.month)
			{
				Manager.Data.exportReport("monthlyReport.csv", "item", "quantity", Manager.Figures.returnFigures(timeFrame.month));
			}
			else if (currentTimeFrame == timeFrame.week)
			{
				Manager.Data.exportReport("weeklyReport.csv", "item", "quantity", Manager.Figures.returnFigures(timeFrame.week));
			}
		}

		//Edit Sales
		private void SalesRecord_Edit1_Click(object sender, RoutedEventArgs e)
		{
			LoadEditSales();
		}

	//ADD STOCK OK
		private void AddStock_Ok_Click(object sender, RoutedEventArgs e)
		{
			//OLD CODE
			/*
			if (
				string.IsNullOrEmpty(AddStock_Name_TB.Text)
				|| string.IsNullOrEmpty(AddStock_Barcode_TB.SText)
				//|| Dropdown menu equals default choice
				)
			*/

			if (alpha_num_pattern.IsMatch(AddStock_Name_TB.Text) &&
				num_pattern.IsMatch(AddStock_Barcode_TB.Text) &&
				AddStock_Catagory_DB.SelectedIndex != 0)
				//!AddStock_Catagory_DB.Equals(defaultDropdown))
			{


				if (Manager.Stock.AddStock(AddStock_Name_TB.Text, AddStock_Catagory_DB.Text, Convert.ToInt32(AddStock_Barcode_TB.Text), Manager.Data))
				{
					LoadViewSales();
				}
				else
				{
					AddStock_NetworkError.Visibility = Visibility.Visible;
				}		
			}
			else//Flash warning on screen
			{
				AddStock_Warning.Visibility = Visibility.Hidden;
				//System.Threading.Thread.Sleep(500); //Wait for .5 Secs
				AddStock_Warning.Visibility = Visibility.Visible;
			}
		}
	//ADDSTOCK CANCEL
		private void AddStock_Cancel_Click(object sender, RoutedEventArgs e)
		{
			LoadViewSales();
		}

	//ADD SALES OK
		private void Button_AddSales_Ok_Click(object sender, RoutedEventArgs e)
		{
			DateTime dt;

			if (num_pattern.IsMatch(AddSales_Amount.Text) &&
				(AddSales_Date.SelectedDate != null) &&
				//!(AddSales_Product.Equals(defaultDropdown)))
				AddSales_Product.SelectedIndex != 0)
			{
				//AddSales_Date.SelectedDate
				dt = AddSales_Date.SelectedDate.Value;//Convert from nullable date-time to normal date-time

				//Send info to Database
				if (Manager.Sales.AddSalesRecord(dt, Manager.Stock.FindStock(AddSales_Product.Text), Convert.ToInt32(AddSales_Amount.Text), Manager.Data))
				{
					LoadViewSales();
				}
				else
				{
					AddSales_NetworkError.Visibility = Visibility.Visible;
				}

			}
			else//Flash warning on screen
			{
				AddSales_Warning.Visibility = Visibility.Hidden;
				//System.Threading.Thread.Sleep(500); //Wait for .5 Secs
				AddSales_Warning.Visibility = Visibility.Visible;
			}
		}
	//ADD SALES CANCEL
		private void AddSales_Cancel_Click(object sender, RoutedEventArgs e)
		{
			LoadViewSales();
		}

	//EDIT SALES OK
		private void Button_EditSales_Ok_Click(object sender, RoutedEventArgs e)
		{
				DateTime dt;//This is just a precaution for the compiler, it will never actually get passed

			if (num_pattern.IsMatch(EditSales_Amount.Text) &&
				EditSales_Date.SelectedDate != null &&
				//!(EditSales_Product.Equals(defaultDropdown)))
				EditSales_Product.SelectedIndex != 0)
			{
				dt = AddSales_Date.SelectedDate.Value;

				if (Manager.Sales.EditSalesRecord(1, dt, Manager.Stock.FindStock(AddSales_Product.Text), Convert.ToInt32(AddSales_Amount.Text), Manager.Data))//FINISH THIS
					//if (Manager.Sales.EditSalesRecord(SalesRecordId: Int, date:int, item:stockItem, quantity:int, Manager.Data))//FINISH THIS
					LoadViewSales();
				//Else
				//EditSales_NetworkError.Visibility = Visibility.Visible;
			}
			else//Flash warning on screen
			{
				EditSales_Warning.Visibility = Visibility.Hidden;
				//System.Threading.Thread.Sleep(500); //Wait for .5 Secs
				EditSales_Warning.Visibility = Visibility.Visible;
			}
		}
	//EDIT SALES CANCEL
		private void EditSales_Cancel_Click(object sender, RoutedEventArgs e)
		{
			LoadViewSales();
		}


		private void ReportGenerate_Button_Click(object sender, RoutedEventArgs e)
		{
			//FINISH THIS
		}

		//-------------
		// LOAD PANNELS
		//-------------
		//Add Sales
		private void LoadAddSales()
		{
			if (Panel_AddSales.Visibility == Visibility.Hidden)
			{
				HidePanels();
				cleartext();
				HideWarnings();

				//Put entries in dropdown list
				products = new List<string>();
				foreach (PharmacyProject.StockItem s in Manager.Stock.Items)
				{
					products.Add(s.StockName);
				}
				Populate(AddSales_Product, products);

				//Prefill
				AddSales_Date.SelectedDate = DateTime.Today;

				Panel_AddSales.Visibility = Visibility.Visible;
			}
		}

	//View Sales
		private void LoadViewSales()
		{
			if (Panel_ViewSales.Visibility == Visibility.Hidden)
			{
				HidePanels();
				cleartext();
				HideWarnings();

				SalesRecordGrid.ItemsSource = Manager.Sales.displaySales();

				Panel_ViewSales.Visibility = Visibility.Visible;
			}
		}

	//Load Predictions
		private void LoadPredictions(timeFrame time)
		{
			//SET THE CURRENT TIMEFRAME - USED FOR EXPORTING
			currentTimeFrame = time;
			
			//Hide other panels and show Predictions panel
			if (Panel_SalesPredictions.Visibility == Visibility.Hidden)
			{
				HidePanels();
				cleartext();
				HideWarnings();

				Panel_SalesPredictions.Visibility = Visibility.Visible;
			}

			//Set Weekly or Monthly Sales Predictions
			if (time == timeFrame.week)
			{
				//Set Data Source to Weekly
				//PredictionsGrid.ItemsSource = Manager.Sales.SalesRecords;
				PredictionsGrid.ItemsSource = Manager.Predictions.getPrediction(timeFrame.week);

				//Show the correct title
				WeeklySalesPredictionTitle.Visibility = Visibility.Visible;
				MonthlySalesPredictionTitle.Visibility = Visibility.Hidden;
			}
			else if (time == timeFrame.month)
			{
				//Set Data Source to Monthly - FINISH THIS
				PredictionsGrid.ItemsSource = Manager.Predictions.getPrediction(timeFrame.month);

				//Show the correct title
				MonthlySalesPredictionTitle.Visibility = Visibility.Visible;
				WeeklySalesPredictionTitle.Visibility = Visibility.Hidden;
			}

		}

		//Load Report
		private void LoadReport(timeFrame time)
		{
			//Set the current timeframe for exporting
			currentTimeFrame = time;

			//Hide other panels and show this one
			if (Panel_SalesReport.Visibility == Visibility.Hidden)
			{
				HidePanels();
				cleartext();
				HideWarnings();

				Panel_SalesReport.Visibility = Visibility.Visible;
			}

			//Set correct data source and title
			if (time == timeFrame.week)
			{
				//Set Data Source to Weekly - FINISH THIS
				ReportGrid.ItemsSource = Manager.Figures.returnFigures(timeFrame.week);

				//Show the correct title
				WeeklySalesReportTitle.Visibility = Visibility.Visible;
				MonthlySalesReportTitle.Visibility = Visibility.Hidden;
			}
			else if (time == timeFrame.month)
			{
				//Set Data Source to Monthly - FINISH THIS
				ReportGrid.ItemsSource = Manager.Figures.returnFigures(timeFrame.month);

				//Show the correct title
				MonthlySalesReportTitle.Visibility = Visibility.Visible;
				WeeklySalesReportTitle.Visibility = Visibility.Hidden;
			}

			//Populate(Combo_ReportPeriod, timeFrameList);
		}

		//Add Stock
		private void LoadAddStock()
		{
			if (Panel_AddStock.Visibility == Visibility.Hidden)
			{
				HidePanels();
				cleartext();
				HideWarnings();

				Populate(AddStock_Catagory_DB, Manager.Stock.Categories);

				Panel_AddStock.Visibility = Visibility.Visible;
			}
		}

	//Edit Sales
		private void LoadEditSales()
		{
			if (Panel_EditSales.Visibility == Visibility.Hidden)
			{
				HidePanels();
				cleartext();
				HideWarnings();

				//Put entries in dropdown list
				products = new List<string>();
				foreach (PharmacyProject.StockItem s in Manager.Stock.Items)
				{
					products.Add(s.StockName);
				}
				Populate(EditSales_Product, products);	//FINISH THIS

				//Prefill
				EditSales_Date.SelectedDate = DateTime.Today;//FINISH THIS
				EditSales_Amount.Text = "1";//FINISH THIS
				/*
				for(int i = 0, i > Index.length, i++)
				{
					if Index[i] == catagory
						AddSales_Product.SelectedIndex = i;
				}
				*/

				Panel_EditSales.Visibility = Visibility.Visible;
			}
		}
		
//--------------
// SHOW AND HIDE
//--------------

		//--Hides all panels, except the top menu--
		private void HidePanels()
		{
			Panel_ViewSales.Visibility = Visibility.Hidden;
			Panel_AddSales.Visibility = Visibility.Hidden;
			Panel_AddStock.Visibility = Visibility.Hidden;
			Panel_EditSales.Visibility = Visibility.Hidden;
			Panel_SalesPredictions.Visibility = Visibility.Hidden;
			Panel_SalesReport.Visibility = Visibility.Hidden;
		}

		//Clear all text boxes
		private void cleartext()
		{
			//EDIT SALES
			//EditSales_Date.Text = " ";
			//EditSales_Date.SelectedDate = null;
			EditSales_Amount.Clear();
			EditSales_Product.SelectedIndex =0;

			//ADD SALES
			//EditSales_Date.SetValue(DateTime.Now);
			//EditSales_Date.SelectedDate = new DateTime(2001, 10, 20);
			//AddSales_Date.SelectedDate = new DateTime(2001, 10, 20);
			AddSales_Amount.Clear();
			AddSales_Product.SelectedIndex = 0;

			//AddStock
			AddStock_Name_TB.Clear();
			AddStock_Catagory_DB.SelectedIndex = 0;
			AddStock_Barcode_TB.Clear();
		}

		//Hide All Warning Labels
		private void HideWarnings()
		{
			AddStock_Warning.Visibility = Visibility.Hidden;
			AddSales_Warning.Visibility = Visibility.Hidden;
			EditSales_Warning.Visibility = Visibility.Hidden;

			EditSales_NetworkError.Visibility = Visibility.Hidden;
			AddSales_NetworkError.Visibility = Visibility.Hidden;
			AddStock_NetworkError.Visibility = Visibility.Hidden;
		}

//------------------------------------------------------
//DON'T KNOW WHAT THESE DO, BUT WON'T COMPILE WITHOUT IT
//------------------------------------------------------
		private void button_Copy_Click(object sender, RoutedEventArgs e)
		{
		}

		/*private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void AddSales_Date_TextChanged(object sender, TextChangedEventArgs e)
		{

		}*/
		private void textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
		}

		private void PHP_Sales_Manager_Loaded(object sender, RoutedEventArgs e)
		{

			PharmacyProject.PharmacyDatabaseDataSet pharmacyDatabaseDataSet = ((PharmacyProject.PharmacyDatabaseDataSet)(this.FindResource("pharmacyDatabaseDataSet")));
			// Load data into the table SalesTable. You can modify this code as needed.
			PharmacyProject.PharmacyDatabaseDataSetTableAdapters.SalesTableTableAdapter pharmacyDatabaseDataSetSalesTableTableAdapter = new PharmacyProject.PharmacyDatabaseDataSetTableAdapters.SalesTableTableAdapter();
			pharmacyDatabaseDataSetSalesTableTableAdapter.Fill(pharmacyDatabaseDataSet.SalesTable);
			System.Windows.Data.CollectionViewSource salesTableViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("salesTableViewSource")));
			salesTableViewSource.View.MoveCurrentToFirst();
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void AddStock_Catagory_DB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}//Class End
}//Namespace End
	