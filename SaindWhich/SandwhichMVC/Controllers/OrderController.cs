using DataObjects;
using LogicLayer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SandwhichMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SandwhichMVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private IOrderManager _orderManager = new OrderManager();
        private IStandardItemManager _standardItemManager = new StandardItemManager();
        private IAddOnsManager _addOnsManager = new AddOnsManager();


        // GET: Order
        public ActionResult Index()
        {
            var email = User.Identity.GetUserName();

            string userEmail = email;
            var order = _orderManager.GetOrderByEmail(userEmail);
            return View(order);           
        }

        // GET: Order/Details/5
        public ActionResult Details(string email)
        {
            var orders = _orderManager.GetOrderByEmailAndActive(email);

            return View(orders);
        }

        // GET: Order/Create
        [Authorize]
        public ActionResult Create()
        {
            Order order = new Order();
            User user = new User();
            var email = User.Identity.GetUserName();
            order.OrderEmail = email;
            order.OrderFirstName = "";
            order.OrderLastName = "";

            return View();
        }

        // POST: Order/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                //This code is pretty weird. I stuggled with letting a user create a custom sandwich on the web, 
                //the way that I was doing it on the desktop I just couldn't figure out here. So this code
                //Generates a random sandwhich based on all the possible add ons and it shouldn';t actually be bad. It shouldn't 
                //Have more than 1 kind of bread or any repeating toppings. What I am planning on doing this summer is letting a 
                //user rate a sandwich after they pick it up and eat it. Then over time I can actually create good sanwiches. 
                //I could create a random sanwich, check if it has already been made and already has sy 30 reviews and if those 
                //reviews are less than 60/100, I could create them a new sandwich and repeat that process untill I get a 
                //satisfactory sandwich. Over time I would be able to have a base menu of sanwiches that the customers have
                //considered to be actually good

                Order order = new Order();
                
                var email = User.Identity.GetUserName();
                order.OrderEmail = email;
                order.OrderFirstName = Request.Form["OrderFirstName"];
                order.OrderLastName = Request.Form["OrderLastName"];
                string quantity = Request.Form["OrderQuantity"];

                int numItems = Int32.Parse(quantity);

                User user = new User();
                user.EmployeeID = 1000001;

                for (int i = 0; i < numItems; i++)
                {
                    List<StandardItem> standardItem = new List<StandardItem>();
                    StandardItem standardItem1 = new StandardItem();
                    standardItem1.StandardItemID = _standardItemManager.CreateStandardItem();
                    List<AddOn> addons = new List<AddOn>();
                    var possibleAddons = _addOnsManager.GetAllAddOns();

                    var rand = new Random();
                    var rtnlist = new List<int>();

                    int possibleOptions = possibleAddons.Count/4;

                    for (int k = 0; k < possibleOptions; k++)
                    {
                      rtnlist.Add(rand.Next(0, 4));
                    }

                    List<int> uniqueItems = rtnlist.Distinct<int>().ToList();
                    for (int j = 0; j < uniqueItems.Count; j++)
                    {
                        addons.Add(possibleAddons[uniqueItems[j]*4]);
                    }
                    standardItem1.AddOns = addons;
                    order.StandardItem.Add(standardItem1);
                }
                int orderID = _orderManager.AddOrder(order, user);//Creates an order
                foreach (var a in order.StandardItem)
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
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                _orderManager.UpdateOrderStatus("Canceled", id);
            }
            catch (Exception)
            {
                RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}




