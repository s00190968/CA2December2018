using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using Newtonsoft.Json;
using System.IO;

namespace Accounts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Account> Accounts;//list holding the accounts
        ObservableCollection<Account> FilteredList;//list holding the accounts
        string []accountType;//array of account types for the combobox

        Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Accounts = GetData();//sets up the list to have some accounts ready
            accountsLbx.ItemsSource = Accounts;//list box shows the accounts

            //account type combobox
            accountType = new string[] { "Current", "Savings", "Student" };
            accountTypeCbx.ItemsSource = accountType;//set values for combobox
            accountTypeCbx.SelectedIndex = 0;//at first 0 index is selected

            //timer
            //create dispatcher timer object
            DispatcherTimer dt = new DispatcherTimer();

            //link this to  a method 
            dt.Tick += Dt_Tick;

            //set the interval
            dt.Interval = new TimeSpan(0, 0, 30);

            //start the thing
            dt.Start();

        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            string json = JsonConvert.SerializeObject(Accounts, Formatting.Indented);

            //write to file
            using (StreamWriter sw = new StreamWriter(@"Accounts.json"))//file should go to the bin folder of the solution
            {
                sw.Write(json);
            }
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            Account temp = new Account();//temporary account
            temp.AccountHolder = accountHolderTbx.Text;//set holder
            temp.AccountNumber = accountNumberTbx.Text;//set number
            temp.AccountType = accountTypeCbx.SelectedValue.ToString();//set type
            temp.Balance = decimal.Parse(openingBalanceTbx.Text);//set balance
            Accounts.Add(temp);//add to list of accounts
            accountsLbx.ItemsSource = Accounts;//list box shows the accounts
        }

        private ObservableCollection<Account> GetData()
        {
            ObservableCollection<Account> accounts = new ObservableCollection<Account>();

            Account acc1 = new Account()
            {
                AccountHolder = "Joe Bloggs",
                AccountNumber = "45670987",
                AccountType = "Current",
                Balance = 1000m
            };

            Account acc2 = new Account()
            {
                AccountHolder = "Sally Jones",
                AccountNumber = "43470959",
                AccountType = "Current",
                Balance = 500m
            };

            Account acc3 = new Account()
            {
                AccountHolder = "Jim Smith",
                AccountNumber = "72057823",
                AccountType = "Savings",
                Balance = 5000m
            };

            Account acc4 = new Account()
            {
                AccountHolder = "Joe Bloggs",
                AccountNumber = "90807652",
                AccountType = "Savings",
                Balance = 650.56m
            };

            Account acc5 = new Account()
            {
                AccountHolder = "Louise Dolan",
                AccountNumber = "87267575",
                AccountType = "Student",
                Balance = 100.05m
            };

            Account acc6 = new Account()
            {
                AccountHolder = "Milly Durcan",
                AccountNumber = "79530812",
                AccountType = "Student",
                Balance = 250m
            };

            accounts.Add(acc1);
            accounts.Add(acc2);
            accounts.Add(acc3);
            accounts.Add(acc4);
            accounts.Add(acc5);
            accounts.Add(acc6);

            return accounts;
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            Account selected = accountsLbx.SelectedItem as Account;//selected item

            if (selected != null) //if selected isn't null
            {
                Accounts.Remove(selected);//remove from list
            }
            detailsTxBlk.Text = "";
            accountsLbx.ItemsSource = Accounts;//list box shows the accounts
        }

        private void searchTbx_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            // read search term
            string search = searchTbx.Text;

            accountsLbx.ItemsSource = Accounts.Where(a => a.AccountHolder.ToLower().Contains(search));
        }

        private void filterCheckboxes_Click(object sender, RoutedEventArgs e)//filters list depending on the checkboxes selected
        {
            if (currentChBx.IsChecked==true)
            {
                FilteredList = FilterAccounts("Current");                
            }
            else if (savingsChBx.IsChecked == true)
            {
                FilteredList = FilterAccounts("Savings");
            }
            else if (studentChBx.IsChecked==true)
            {
                FilteredList = FilterAccounts("Student");
            }
            else//if nothing is selected show all
            {
                FilteredList = Accounts;
            }
            accountsLbx.ItemsSource = FilteredList;
        }

        ObservableCollection<Account> FilterAccounts(string variation)// make a temp list of the objects that have "variation" 
        {
            ObservableCollection<Account> temp = new ObservableCollection<Account>();

            foreach (Account a in Accounts)
            {
                if (a.AccountType.Equals(variation))
                {
                    temp.Add(a);
                }
            }

            return temp;
        }

        private void searchTbx_GotFocus(object sender, RoutedEventArgs e)//clear box when it gets focus
        {
            searchTbx.Clear();
        }

        private void accountsLbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Account selected = accountsLbx.SelectedItem as Account;//selected item
            if(selected != null)
            {
                detailsTxBlk.Text = selected.ToString();
            }
        }

        private void setReviewsBtn_Click(object sender, RoutedEventArgs e)
        {
            Account selected = accountsLbx.SelectedItem as Account;//selected item
            selected.reviewDate = randDate(365);
            detailsTxBlk.Text = selected.ToString();
        }

        public DateTime randDate(int timeSpan)
        {
            DateTime start = DateTime.Today;//today
            return start.AddDays(rand.Next(1, timeSpan));//add random amount of days to today and return that
        }

        private void accountHolderTbx_GotFocus(object sender, RoutedEventArgs e)//clear box when it gets focus
        {
            accountHolderTbx.Clear();
        }

        private void accountNumberTbx_GotFocus(object sender, RoutedEventArgs e)//clear box when it gets focus
        {
            accountNumberTbx.Clear();
        }

        private void openingBalanceTbx_GotFocus(object sender, RoutedEventArgs e)//clear box when it gets focus
        {
            openingBalanceTbx.Clear();
        }
    }
}
