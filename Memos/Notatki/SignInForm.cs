using System;
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
            try {
                ConnectToDB();
            } catch (Exception e) {
                ConnectToDB();
                try {
                    ConnectToDB();
                } catch {
                    System.Windows.Forms.MessageBox.Show("Wystąpił błąd, wyłączanie aplikacji");
                    Application.Exit();
                    return;
                }
            }
        }

        public string CalculateHash(string input) {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
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
            System.IO.File.Copy(Data.Config.EmptyDbFilePath, Data.Config.DbFilePath, true);
        }

        public void SignIn() {
            List<Data.Table.User> users = SQL.Data.Elements<Data.Table.User>.CreateElements(Data.Config.sql, Data.Table.User.TableName);

            string hashedWithMD5 = CalculateHash(Password);
            Data.Table.User user = users.FirstOrDefault(el => el.Login == Login && el.passwordHash == hashedWithMD5);
            if (user != null) {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Data.Config.User = user;
            } else {
                MessageBox.Show("Login lub hasło niepoprawne!");
            }
        }

        private bool Register() {
            try {
                DataTable dt = Data.Config.sql.GetTable(Data.Table.User.TableName);
                if (String.IsNullOrEmpty(Login)) {
                    MessageBox.Show("Błędny login, proszę podaj poprawnę wartość.");
                    return false;
                }

                if (SQL.Data.Elements<Data.Table.User>.CreateElements(dt).Any(el => el.Login == Login)) {
                    MessageBox.Show("Login zajęty, proszę wybierz inny login.");
                    return false;
                }

                Data.Table.User user = SQL.Data.Elements<Data.Table.User>.CreateElement(dt.NewRow());
                user.Login = Login;
                user.Id = Data.Config.sql.GetNextId(Data.Table.User.TableName);
                user.passwordHash = CalculateHash(Password);
                dt.Rows.Add(user.ElementDataRow);

                Data.Config.sql.Commit(dt);
                return true;
            } catch (Exception e) {
                System.Windows.Forms.MessageBox.Show("Wystąpił błąd, wyłączanie aplikacji");
                Console.WriteLine("Błąd podczas rejestracji" + e.Message);
                Application.Exit();
            }
            return false;
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
