using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfService.Data.Table {
    class Memo : SQL.Data.Element {
        public Memo(System.Data.DataRow dr) : base(dr) { }

        public static class Column {
            public const string ID = "ID";
            public const string TEXT = "TEXT";
            public const string TITLE = "TITLE";
            public const string USER_ID = "USER_ID";
        }

        public static string TableName {
            get { return "Memo"; }
        }

        public int Id {
            get { return int.Parse(m_drRekord[Column.ID].ToString()); }
            set { m_drRekord[Column.ID] = value; }
        }

        public int UserId {
            get { return int.Parse(m_drRekord[Column.USER_ID].ToString()); }
            set { m_drRekord[Column.USER_ID] = value; }
        }

        public string Title {
            get { return m_drRekord[Column.TITLE].ToString(); }
            set { m_drRekord[Column.TITLE] = value; }
        }

        public string Text {
            get { return m_drRekord[Column.TEXT].ToString(); }
            set { m_drRekord[Column.TEXT] = value; }
        }
    }
}
