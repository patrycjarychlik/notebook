﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Logowanie i rejestracja użytkowników
namespace Notatki {

    public partial class SignInForm : Form {
        public SignInForm() {
            InitializeComponent();
        }

        public string Login {
            get { return textBox1.Text; }
        }

        public string Password {
            get { return textBox2.Text; }
        }

        private void InitDatabase() {
            ServiceReference1.Service1Client proxy = new ServiceReference1.Service1Client();
            bool init = proxy.InitDatabase();
            if (!init) {
                System.Windows.Forms.MessageBox.Show("Wystąpił błąd, wyłączanie aplikacji");
                Application.Exit();
            }
        }

        private void ConnectToDB() {
            if (!System.IO.File.Exists(Data.Config.DbFilePath))
                throw new Exception();
            Data.Config.sql = new SQL.SqLite.SQL(new SQL.cDbInfo(Data.Config.DbFilePath, Data.Config.DbPassword));
            if (Data.Config.sql.ConnectionTest() != SQL.eConnectionStatus.OK) {
                throw new Exception();
            }
        }

        private void InitDB() {
            ServiceReference1.Service1Client proxy = new ServiceReference1.Service1Client();
            proxy.InitDB();
        }

        public void SignIn() {
            ServiceReference1.Service1Client proxy = new ServiceReference1.Service1Client();
            proxy.SignIn(Password, Login);
        }

        private bool Register() {
            ServiceReference1.Service1Client proxy = new ServiceReference1.Service1Client();
            return proxy.Register(Login, Password);
        }

        private void button1_Click(object sender, EventArgs e) {
            SignIn();
        }

        private void register_button(object sender, EventArgs e) {
            if (Register()) {
                SignIn();
            }
        }

        private void load_button(object sender, EventArgs e) {
            InitDatabase();
        }
    }
}
