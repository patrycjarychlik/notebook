using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;


namespace WcfService {

    public partial class SignInForm {


        public void InitDatabase() {
            try {
                ConnectToDB();
            } catch (Exception e) {
                ConnectToDB();
                try {
                    ConnectToDB();
                } catch {
                    throw new Exception("Wystąpił błąd, wyłączanie aplikacji");
                }
            }
        }

        private string CalculateHash(string input) {
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

        public void InitDB() {
            System.IO.File.Copy(Data.Config.EmptyDbFilePath, Data.Config.DbFilePath, true);
        }

        public void SignIn(string Password, string Login) {
            List<Data.Table.User> users = SQL.Data.Elements<Data.Table.User>.CreateElements(Data.Config.sql, Data.Table.User.TableName);

            string hashedWithMD5 = CalculateHash(Password);
            Data.Table.User user = users.FirstOrDefault(el => el.Login == Login && el.passwordHash == hashedWithMD5);
            if (user != null) {
                Data.Config.User = user;
            } else {
                throw new Exception("Login lub hasło niepoprawne!");
            }
        }

        public bool Register(string Login, string Password) {
            try {
                DataTable dt = Data.Config.sql.GetTable(Data.Table.User.TableName);
                if (String.IsNullOrEmpty(Login)) {
                    throw new Exception("Błędny login, proszę podaj poprawnę wartość.");
                    return false;
                }

                if (SQL.Data.Elements<Data.Table.User>.CreateElements(dt).Any(el => el.Login == Login)) {
                    throw new Exception("Login zajęty, proszę wybierz inny login.");
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
                //System.Windows.Forms.MessageBox.Show("Wystąpił błąd, wyłączanie aplikacji");
                /*Console.WriteLine(*/
                throw new Exception("Wystąpił błąd, wyłączanie aplikacji");
                //Application.Exit();
            }
            return false;
        }


    }

}
