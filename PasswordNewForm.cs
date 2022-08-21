using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SyNotebook
{
    public partial class PasswordNewForm : Form
    {
        public PasswordNewForm()
        {
            InitializeComponent();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (tbPassword.Text!=tbRepeatPassword.Text)
            {
				MessageBox.Show("Пароли не совпадают!","Изменение пароля");
				tbPassword.Text = "";
				tbRepeatPassword.Text = "";
				tbPassword.Focus();
            }
            else
            {
				DialogResult = DialogResult.OK;
				Close();
			}
        }
    }
}