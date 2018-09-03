using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService {

    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę klasy „Service1” w kodzie i pliku konfiguracji.
    public class Service1 : IService1 {
        private SignInForm signInForm = new SignInForm();

        public string GetData(int value) {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite) {
            if (composite == null) {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue) {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public bool InitDatabase() {
            try {
                signInForm.InitDatabase();
                return true;
            } catch (Exception e) {
                return false;
            }
        }

        public void InitDB() {
            signInForm.InitDB();
        }

        public void SignIn(string Password, string Login) {
            signInForm.SignIn(Password, Login);
        }

        public bool Register(string Login, string Password) {
            return signInForm.Register(Login, Password);
        }

    }
}
