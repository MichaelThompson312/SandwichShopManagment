using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DataObjects;
using LogicLayer;
using Button = System.Windows.Controls.Button;

namespace SaindWhichPresentationLayer
{
    /// <summary>
    /// Interaction logic for frmNewOrder.xaml
    /// </summary>
    public partial class frmNewOrder : Window
    {
        private User _user = null;
        private Order _order = null;
        private OrderManager _orderManager = null;
        private StandardItemManager _standardItemManager = null;
        private AddOnsManager _addOnsManager = null;

        public frmNewOrder(AddOnsManager addOnsManager, User user,
            OrderManager orderManager, StandardItemManager standardItemManager)
        {
            InitializeComponent();
            _orderManager = orderManager;
            _addOnsManager = addOnsManager;
            _standardItemManager = standardItemManager;
            _user = user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Order order = new Order();
            _order = order;
            ShowCurrentOrder(_order);
            PopulateAddOnList(_addOnsManager);
        }

        private void ShowCurrentOrder(Order order)
        {
            foreach (var a in order.StandardItem)
            {
                lbCrtOrder.Items.Add(a.Name);
            }
        }

        private void AddStandardItemToOrder(Order order)
        { 
            order.StandardItem.Add(new StandardItem());
            int orderSize = order.StandardItem.Count;
            int itemNumber = orderSize - 1;
            order.StandardItem[orderSize- 1].Name = ("Item " + (orderSize)).ToString();
            lbCrtOrder.Items.Add(order.StandardItem[itemNumber].Name);
                        
        }

        private void PopulateAddOnList(AddOnsManager _addOns)
        {
            AddOnsManager _addOnsManager = new AddOnsManager();
            _addOnsManager = _addOns;
            try
            {
                var eAddons = _addOnsManager.GetAllAddOns();
                foreach (var a in eAddons)
                {
                    lstAddOns.Items.Add(a.Name);
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n\n\n" + ex.InnerException.Message);
            }
        }

        private void LstAddOns_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            try
            {
                if (lbCrtOrder.HasItems)
                {
                    int itemCount = lbCrtOrder.Items.Count;
                    lbCrtOrderAddons.Items.Add((string)lstAddOns.SelectedItem);
                    string addOnItem = (string)lstAddOns.SelectedItem;
                    int itemToAddToo = lbCrtOrder.SelectedIndex;
                    StandardItem currentItem = _order.StandardItem[itemToAddToo];                    
                    AddAddOnsToItem(currentItem, addOnItem);
                }

            }
            catch (Exception ex) 
            {
                System.Windows.MessageBox.Show(ex.Message + "\n\n\n" + ex.InnerException.Message);
            }        
        }

        private void AddAddOnsToItem(StandardItem currentItem, string addOnItem)
        {
            int orderItem = lbCrtOrder.SelectedIndex;
            int ingredientID = 0;
            string ingredientName;
            AddOn newAddOn = new AddOn();
            List<AddOn> tempAddOnList = new List<AddOn>();
            var eAddons = _addOnsManager.GetAllAddOns();
            foreach (var a in eAddons)
            {
                if (a.Name == addOnItem)
                {
                    ingredientID = a.IngredientID;
                    ingredientName = a.Name;
                    newAddOn.Name = ingredientName;
                    newAddOn.IngredientID = ingredientID;
                    currentItem.AddOns.Add(newAddOn);
                }
            }
        }

        private void BtnAdditionalItem_Click(object sender, RoutedEventArgs e)
        {
            AddStandardItemToOrder(_order);
            if (lstAddOns.IsEnabled == false)
            {
                btnSubmitOrder.IsEnabled = true;
                lstAddOns.IsEnabled = true;
            }
        }

      

        private void LbCrtOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedItemIndex = lbCrtOrder.SelectedIndex;
            lbCrtOrderAddons.Items.Clear();
            foreach (var a in _order.StandardItem[selectedItemIndex].AddOns)
            {
                lbCrtOrderAddons.Items.Add(a.Name);
            }
        }

        private void BtnSubmitOrder_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure?", "Submit Order", MessageBoxButton.YesNo,
                        MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    int orderID = _orderManager.AddOrder(_order, _user);//Creates an order

                    foreach (var a in _order.StandardItem)
                    {
                        a.StandardItemID = _standardItemManager.CreateStandardItem();//returns an int of the standarditemid, SP takes no parameters
                        if (_orderManager.CreateOrderItem(a.StandardItemID, orderID))
                        {
                            foreach (var b in a.AddOns)
                            {
                                _addOnsManager.AddAddOn(orderID, a.StandardItemID, b.IngredientID);//SP takes the ingredient ID, orderID and StandardItemID
                            }
                        }  
                        
                    }

                    System.Windows.MessageBox.Show("Success!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message + "\n\n" + ex.InnerException);
                }
            }
            
        }
        
    }
}
