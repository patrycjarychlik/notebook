using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService {
    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę interfejsu „IService1” w kodzie i pliku konfiguracji.
    [ServiceContract]
    public interface IService1 {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        bool InitDatabase();

        [OperationContract]
        void InitDB();

        [OperationContract]
        SQL.Response SignIn(string Password, string Login);

        [OperationContract]
        SQL.Response Register(string Login, string Password);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        string getLogin();

        [OperationContract]
        DataTable LoadData();

        [OperationContract]
        List<SQL.MemoDto> UpdateData(DataTable table);

        [OperationContract]
        void CommitAndUpdate(DataTable table);

        [OperationContract]
        void DeleteNote(DataTable table, string Title, string Text);
    }

        // Użyj kontraktu danych, jak pokazano w poniższym przykładzie, aby dodać typy złożone do operacji usługi.
        // Możesz dodać pliki XSD do projektu. Po skompilowaniu projektu możesz bezpośrednio użyć zdefiniowanych w nim typów danych w przestrzeni nazw „WcfService.ContractType”.
        [DataContract]
        public class CompositeType {
            bool boolValue = true;
            string stringValue = "Hello ";

            [DataMember]
            public bool BoolValue {
                get { return boolValue; }
                set { boolValue = value; }
            }

            [DataMember]
            public string StringValue {
                get { return stringValue; }
                set { stringValue = value; }
        }
    }
}
