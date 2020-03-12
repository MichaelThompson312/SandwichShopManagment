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
using DataObjects;
using LogicLayer;
using SaindWhichPresentationLayer;

namespace SaindWhich
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IUserManager _userManager;
        private User _user = null;
        private IOrderManager _orderManager;

        public MainWindow()
        {
            InitializeComponent();
            _userManager = new UserManager();
            _orderManager = new OrderManager();
        }

        private void hideAllUserTabs()
        {
            foreach (TabItem item in tabsetMain.Items)
            {
                item.Visibility = Visibility.Collapsed;
            }
            btnAddEmployee.Visibility = Visibility.Hidden;
            btnNewOrder.Visibility = Visibility.Hidden;
            dgUserList.Visibility = Visibility.Hidden;
            lbOrdeQueueComplete.Visibility = Visibility.Hidden;
            lbOrdeQueueInProgress.Visibility = Visibility.Hidden;
            lbOrdeQueueUnstarted.Visibility = Visibility.Hidden;
            labelOrderQueueComplete.Visibility = Visibility.Hidden;
            LabelOrderQueueInProgress.Visibility = Visibility.Hidden;
            LabelOrderQueueUnstarted.Visibility = Visibility.Hidden;
            CookInstructions.Visibility = Visibility.Hidden;
            ServerInstrcutions.Visibility = Visibility.Hidden;

        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var email = txtEmail.Text;
            var password = pwdPassword.Password;

            if (btnLogin.Content.ToString() == "Logout")
            {
                _user = null;

                txtEmail.Text = "";
                pwdPassword.Password = "";
                txtEmail.IsEnabled = true;
                pwdPassword.IsEnabled = true;
                btnLogin.Content = "Login";
                lblPassword.Visibility = Visibility.Visible;
                lblUsername.Visibility = Visibility.Visible;
                txtEmail.Focus();
                lblStatusMessage.Content = "You are not logged in. Please login to continue.";
                hideAllUserTabs();
                dgUserList.ItemsSource = null;
                return;
            }


            if (email.Length < 7 || password.Length < 7)
            {
                MessageBox.Show("Invalid Email or Password",
                    "Invalid Login!", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                txtEmail.Text = "";
                pwdPassword.Password = "";
                txtEmail.Focus();

                return;
            }

            try
            {
                _user = _userManager.AuthenticateUser(email, password);
                // code to set up the user interface
                string roles = "";
                for (int i = 0; i < _user.Roles.Count; i++)
                {
                    roles += _user.Roles[i];
                    if (i < _user.Roles.Count - 1)
                    {
                        roles += ", ";
                    }
                }

                lblStatusMessage.Content = "Hello, " + _user.FirstName
                    + ". You are logged in as: " + roles;

                if (pwdPassword.Password.ToString() == "newuser")
                {
                    // force a password reset
                    var resetPassword = new frmUpdatePassword(_user, _userManager);
                    if (resetPassword.ShowDialog() == true)
                    {
                        // code if the password was reset
                    }
                    else
                    {
                        // code to log the user out because reset failed
                    }

                }

                txtEmail.Text = "";
                pwdPassword.Password = "";
                txtEmail.IsEnabled = false;
                pwdPassword.IsEnabled = false;
                btnLogin.Content = "Logout";
                lblPassword.Visibility = Visibility.Hidden;
                lblUsername.Visibility = Visibility.Hidden;
                // show user tabs
                showUserTabs();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n"
                    + ex.InnerException.Message,
                    "Login Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void showUserTabs()
        {
            // loop through the user roles
            foreach (var role in _user.Roles)
            {
                // check for each role
                switch (role)
                {
                    case "Administrator":
                        tabAdmin.Visibility = Visibility.Visible;
                        tabAdmin.IsSelected = true;
                        dgUserList.Visibility = Visibility.Visible;
                        btnAddEmployee.Visibility = Visibility.Visible;
                        break;
                    
                    case "Server":
                        tabServer.Visibility = Visibility.Visible;
                        tabServer.IsSelected = true;
                        btnNewOrder.Visibility = Visibility.Visible;
                        lbOrdeQueueComplete.Visibility = Visibility.Visible;
                        labelOrderQueueComplete.Visibility = Visibility.Visible;
                        ServerInstrcutions.Visibility = Visibility.Visible;
                        break;

                    case "Manager":
                        tabServer.Visibility = Visibility.Visible;
                        tabCook.Visibility = Visibility.Visible;
                        lbOrdeQueueInProgress.Visibility = Visibility.Visible;
                        lbOrdeQueueUnstarted.Visibility = Visibility.Visible;
                        LabelOrderQueueInProgress.Visibility = Visibility.Visible;
                        LabelOrderQueueUnstarted.Visibility = Visibility.Visible;
                        btnNewOrder.Visibility = Visibility.Visible;
                        lbOrdeQueueComplete.Visibility = Visibility.Visible;
                        ServerInstrcutions.Visibility = Visibility.Visible;
                        ServerCompletedOrderQueueInstructions.Visibility = Visibility.Visible;
                        CookInstructions.Visibility = Visibility.Visible;
                        break;
                    case "Cook":
                        tabCook.Visibility = Visibility.Visible;
                        lbOrdeQueueInProgress.Visibility = Visibility.Visible;
                        lbOrdeQueueUnstarted.Visibility = Visibility.Visible;
                        LabelOrderQueueInProgress.Visibility = Visibility.Visible;
                        LabelOrderQueueUnstarted.Visibility = Visibility.Visible;
                        CookInstructions.Visibility = Visibility.Visible;
                        tabCook.IsSelected = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            hideAllUserTabs();
        }

        private void TabAdmin_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgUserList.ItemsSource == null)
                {
                    populateUserList();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n"
                   + ex.InnerException.Message,
                   "Error",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void populateOrderQueueComplete()
        {
            lbOrdeQueueComplete.Items.Clear();
            List<Order> orderQueue = _orderManager.GetOrderByStatus("Complete");
            foreach (var a in orderQueue)
            {
                ListBoxItem lbOrderItem = new ListBoxItem();
                lbOrderItem.Content = "Order: " + a.OrderID.ToString();
                lbOrdeQueueComplete.Items.Add(lbOrderItem);
                foreach (var b in a.StandardItem)
                {
                    ListBoxItem lbStandardItem = new ListBoxItem();
                    lbStandardItem.Content = "\tItem: " + b.StandardItemID;
                    lbOrdeQueueComplete.Items.Add(lbStandardItem);

                    foreach (var c in b.AddOns)
                    {
                        ListBoxItem lbAddOnItem = new ListBoxItem();
                        lbAddOnItem.Content = "\tAddOn: " + c.Name;
                        lbOrdeQueueComplete.Items.Add(lbAddOnItem);
                    }
                }
            }
        }

        private void populateOrderQueueUnstarted()
        {
            lbOrdeQueueUnstarted.Items.Clear();
            List<Order> orderQueue = _orderManager.GetOrderByStatus("UnStarted");
            foreach (var a in orderQueue)
            {
                ListBoxItem lbOrderItem = new ListBoxItem();
                lbOrderItem.Content = "Order: " + a.OrderID.ToString();
                lbOrdeQueueUnstarted.Items.Add(lbOrderItem);
                foreach (var b in a.StandardItem)
                {
                    ListBoxItem lbStandardItem = new ListBoxItem();
                    lbStandardItem.Content = "\tItem: " + b.StandardItemID;
                    lbOrdeQueueUnstarted.Items.Add(lbStandardItem);

                    foreach (var c in b.AddOns)
                    {
                        ListBoxItem lbAddOnItem = new ListBoxItem();
                        lbAddOnItem.Content = "\tAddOn: " + c.Name;
                        lbOrdeQueueUnstarted.Items.Add(lbAddOnItem);
                    }
                }
            }
        }

        private void populateOrderQueueInProgress()
        {
            lbOrdeQueueInProgress.Items.Clear();
            List<Order> orderQueue = _orderManager.GetOrderByStatus("InProgress");
            foreach (var a in orderQueue)
            {
                ListBoxItem lbOrderItem = new ListBoxItem();
                lbOrderItem.Content = "Order: " + a.OrderID.ToString();
                lbOrdeQueueInProgress.Items.Add(lbOrderItem);
                foreach (var b in a.StandardItem)
                {
                    ListBoxItem lbStandardItem = new ListBoxItem();
                    lbStandardItem.Content = "\tItem: " + b.StandardItemID;
                    lbOrdeQueueInProgress.Items.Add(lbStandardItem);

                    //lbOrdeQueueUnstarted.Items.Add("\t" + b.StandardItemID.ToString());
                    foreach (var c in b.AddOns)
                    {
                        ListBoxItem lbAddOnItem = new ListBoxItem();
                        lbAddOnItem.Content = "\tAddOn: " + c.Name;
                        lbOrdeQueueInProgress.Items.Add(lbAddOnItem);
                        //lbOrdeQueueUnstarted.Items.Add("\t" + c.Name.ToString() + "--" + c.Description.ToString());
                    }
                }
            }
        }

        private void populateUserList()
        {
            dgUserList.ItemsSource = _userManager.GetUserListByActive();
            dgUserList.Columns.RemoveAt(6);
            dgUserList.Columns.RemoveAt(5);
            dgUserList.Columns[0].Header = "Employee ID";
            dgUserList.Columns[1].Header = "First Name";
            dgUserList.Columns[2].Header = "Last Name";
            dgUserList.Columns[3].Header = "Phone Number";
        }

        private void DgUserList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            User user = (User)dgUserList.SelectedItem;

            var userForm = new frmEmployee(user, _userManager);
            if (userForm.ShowDialog() == true)
            {
                populateUserList();
            }
        }

        private void BtnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var addUserForm = new frmEmployee(_userManager);
            if (addUserForm.ShowDialog() == true)
            {
                populateUserList();
            }
        }


        private void BtnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderManager _orderManager = new OrderManager();
            AddOnsManager _addOnsManager = new AddOnsManager();
            StandardItemManager _standardItemManager = new StandardItemManager();
            var addOrderForm = new frmNewOrder(_addOnsManager, _user, _orderManager, _standardItemManager);
            addOrderForm.ShowDialog();
            
        }

        private void TabServer_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                btnNewOrder.Visibility = Visibility.Visible;
                lbOrdeQueueComplete.Visibility = Visibility.Visible;
                populateOrderQueueComplete();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n"
                   + ex.InnerException.Message,
                   "Error",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TabCook_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                populateOrderQueueInProgress();
                populateOrderQueueUnstarted();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n"
                   + ex.InnerException.Message,
                   "Error",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LbOrdeQueueUnstarted_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string orderStatus = "InProgress";
            //I was not able to move items from queue to queue from the programs side.The stored procedure 
            //was able to change the status(The queues get all of the orders based on the status of the order)
            try
            {
                int orderNumber = lbOrdeQueueUnstarted.SelectedIndex + 2;
                List<Order> orderQueue = _orderManager.GetOrderByStatus("UnStarted");

                int orderID = orderQueue[orderNumber].OrderID;

                if (_orderManager.UpdateOrderStatus(orderStatus, orderID))
                {
                    populateOrderQueueUnstarted();
                    populateOrderQueueInProgress();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n"
                   + ex.InnerException.Message,
                   "Update Failed",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LbOrdeQueueComplete_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //I understand that this is not the best way to do it. I had originally tried to add 
            //an event on the ListBoxItem dynamically but was not able to successfully do that
            string orderStatus = "Delivered";

            try
            {
                int orderNumber = lbOrdeQueueUnstarted.SelectedIndex + 2;
                List<Order> orderQueue = _orderManager.GetOrderByStatus("Complete");

                int orderID = orderQueue[orderNumber].OrderID;
                if (_orderManager.UpdateOrderStatus(orderStatus, orderID))
                {
                    MessageBox.Show("Order Delivered","Success", MessageBoxButton.OK);
                    populateOrderQueueComplete();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n"
                   + ex.InnerException.Message,
                   "Update Failed",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LbOrdeQueueInProgress_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string orderStatus = "Complete";

            try
            {
                int orderNumber = lbOrdeQueueUnstarted.SelectedIndex + 2;
                List<Order> orderQueue = _orderManager.GetOrderByStatus("InProgress");

                int orderID = orderQueue[orderNumber].OrderID;
                if (_orderManager.UpdateOrderStatus(orderStatus, orderID))
                {
                    populateOrderQueueUnstarted();
                    populateOrderQueueInProgress();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n"
                   + ex.InnerException.Message,
                   "Update Failed",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
