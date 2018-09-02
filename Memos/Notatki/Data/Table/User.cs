using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notatki.Data.Table {
    public class User : SQL.Data.Element {
        public User(System.Data.DataRow dr) : base(dr) { }

        public static class Column {
            public const string ID = "ID";
            public const string USERNAME = "USERNAME";
            public const string PASSWORD_MD5 = "PASSWORD_MD5";
        }

        public static string TableName {
            get { return "User"; }
        }

        public int Id {
            get { return int.Parse(m_drRekord[Column.ID].ToString()); }
            set { m_drRekord[Column.ID] = value; }
        }

        public string Login {
            get { return m_drRekord[Column.USERNAME].ToString(); }
            set { m_drRekord[Column.USERNAME] = value; }
        }

        public string passwordHash {
            get { return m_drRekord[Column.PASSWORD_MD5].ToString(); }
            set { m_drRekord[Column.PASSWORD_MD5] = value; }
        }
    }
}
