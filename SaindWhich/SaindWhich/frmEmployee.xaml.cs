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
using System.Windows.Shapes;
using DataObjects;
using LogicLayer;

namespace SaindWhichPresentationLayer
{
    /// <summary>
    /// Interaction logic for frmEmployee.xaml
    /// </summary>
    public partial class frmEmployee : Window
    {
        private User _user = null;
        private IUserManager _userManager = null;
        private bool _addMode = true;

        public frmEmployee(IUserManager userManager)
        {
            InitializeComponent();

            _userManager = userManager;
        }
        public frmEmployee(User user, IUserManager userManager)
        {
            InitializeComponent();

            _user = user;
            _userManager = userManager;
            _addMode = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtEmployeeID.IsReadOnly = true;
            if (_addMode == false)
            {
                txtEmployeeID.Text = _user.EmployeeID.ToString();
                txtFirstName.Text = _user.FirstName;
                txtLastName.Text = _user.LastName;
                txtEmailAddress.Text = _user.Email;
                txtPhoneNumber.Text = _user.PhoneNumber;
                chkActive.IsChecked = _user.Active;

                txtEmployeeID.IsReadOnly = true;
                txtFirstName.IsReadOnly = true;
                txtEmailAddress.IsReadOnly = true;
                txtPhoneNumber.IsReadOnly = true;
                txtLastName.IsReadOnly = true;
                chkActive.IsEnabled = false;
            }
            else
            {
                chkActive.IsChecked = true;
                chkActive.IsEnabled = false;
                txtFirstName.Focus();
                btnEdit.Visibility = Visibility.Hidden;
                btnSave.Visibility = Visibility.Visible;
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            txtFirstName.IsReadOnly = false;
            txtEmailAddress.IsReadOnly = false;
            txtPhoneNumber.IsReadOnly = false;
            txtLastName.IsReadOnly = false;
            chkActive.IsEnabled = true;

            txtFirstName.Focus();
            btnEdit.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Visible;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtFirstName.Text.ToString() == "")
            {
                MessageBox.Show("Must enter a valid first name");
                txtFirstName.Focus();
                return;
            }
            if (txtLastName.Text.ToString() == "")
            {
                MessageBox.Show("Must enter a valid last name");
                txtLastName.Focus();
                return;
            }
            if (!(txtEmailAddress.Text.ToString().Length > 6
                && txtEmailAddress.Text.ToString().Contains("@")
                && txtEmailAddress.Text.ToString().Contains(".")))
            {
                MessageBox.Show("Must enter a valid Email");
                txtEmailAddress.Focus();
                return;
            }

            if (txtPhoneNumber.Text.ToString().Length < 10 || txtPhoneNumber.Text.ToString().Contains(" "))
            {
                MessageBox.Show("Must enter a valid Phone");
                txtPhoneNumber.Focus();
                return;
            }
            User user = new User()
            {
                FirstName = txtFirstName.Text.ToString(),
                LastName = txtLastName.Text.ToString(),
                PhoneNumber = txtPhoneNumber.Text.ToString(),
                Email = txtEmailAddress.Text.ToString()
            };

            if (_addMode)
            {
                try
                {
                    if (_userManager.AddEmployee(user))
                    {
                        this.DialogResult = true;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.InnerException);

                }
            }
            else
            {
                try
                {
                    if (_userManager.EditEmployee(_user, user))
                    {
                        this.DialogResult = true;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.InnerException);

                }
            }


        }
        
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
