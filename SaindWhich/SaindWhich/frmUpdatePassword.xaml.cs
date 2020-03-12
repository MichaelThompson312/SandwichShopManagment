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
    /// Interaction logic for frmUpdatePassword.xaml
    /// </summary>
    public partial class frmUpdatePassword : Window
    {
        User _user = null;
        IUserManager _userManager = null;

        public frmUpdatePassword(User user, IUserManager userManager)
        {
            InitializeComponent();

            _user = user;
            _userManager = userManager;
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string oldPassword = pwdCurrentPassword.Password;
            string newPassword = pwdNewPassword.Password;
            string checkPassword = pwdRetypePassword.Password;

            if (oldPassword.Length < 7)
            {
                MessageBox.Show("Invalid password entry");
                pwdCurrentPassword.Password = "";
                pwdCurrentPassword.Focus();
                return;
            }
            if (newPassword.Length < 7)
            {
                MessageBox.Show("Invalid password entry");
                pwdNewPassword.Password = "";
                pwdNewPassword.Focus();
                return;
            }
            if (newPassword != checkPassword)
            {
                MessageBox.Show("New password and retype must match.");
                pwdNewPassword.Password = "";
                pwdRetypePassword.Password = "";
                pwdNewPassword.Focus();
                return;
            }

            // try to update password
            try
            {
                if (_userManager.ResetPassword(_user.EmployeeID,
                    pwdCurrentPassword.Password.ToString(),
                    pwdNewPassword.Password.ToString()))
                {
                    MessageBox.Show("Password Successfully Reset");
                    this.DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Reset Failed.");
                    this.DialogResult = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" +
                    ex.InnerException.Message);
                this.DialogResult = false;
            }

        }
    }
}
