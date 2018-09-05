using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfService.Data.Table;
using static WcfService.IService1;

namespace WcfService {

    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę klasy „Service1” w kodzie i pliku konfiguracji.
    public class Service1 : IService1 {
        private SignInForm signInForm = new SignInForm();
        private MemosForm memosForm = new MemosForm();

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

        public SQL.Response SignIn(string Login, string Password) {
            SQL.Response response = new SQL.Response();
            try {
                signInForm.SignIn(Password, Login);
                response.Status = true;
            } catch (Exception e) {
                response.Message = e.Message;
                response.Status = false;
            }
            return response;
        }

        public SQL.Response Register(string Login, string Password) {
            SQL.Response response = new SQL.Response();
            try {
                signInForm.Register(Login, Password);
                response.Status = true;
            } catch (Exception e) {
                response.Message = e.Message;
                response.Status = false;
            }
            return response;
        }

        public string getLogin() {
            return memosForm.getLogin();
        }

        public DataTable LoadData() {
            return memosForm.LoadData();
        }

        public List<SQL.MemoDto> UpdateData(DataTable table) {
            List<Memo> list = memosForm.UpdateData(table);
            List<SQL.MemoDto> memos = new List<SQL.MemoDto>();
            foreach (Memo m in list) {
                memos.Add(new SQL.MemoDto(m.Title, m.Text, m.Id, m.UserId));
            }
            return memos;
        }

        public void CommitAndUpdate(DataTable table) {
            memosForm.CommitAndUpdate(table);
        }

        public void DeleteNote(DataTable table, string Title, string Text) {
            memosForm.DeleteNote(table, Title, Text);
        }
    }
}
