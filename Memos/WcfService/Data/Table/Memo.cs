using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WcfService.Data.Table {
    [Serializable()]
    [DataContract]
    public  class Memo : SQL.Data.Element {
        public Memo(System.Data.DataRow dr) : base(dr) { }
        public Memo() { }

        public static class Column {
            public const string ID = "ID";
            public const string TEXT = "TEXT";
            public const string TITLE = "TITLE";
            public const string USER_ID = "USER_ID";
        }
        [DataMember]
        public static string TableName {
            get { return "Memo"; }
        }
        [DataMember]
        public int Id {
            get { return int.Parse(m_drRekord[Column.ID].ToString()); }
            set { m_drRekord[Column.ID] = value; }
        }
        [DataMember]
        public int UserId {
            get { return int.Parse(m_drRekord[Column.USER_ID].ToString()); }
            set { m_drRekord[Column.USER_ID] = value; }
        }
        [DataMember]
        public string Title {
            get { return m_drRekord[Column.TITLE].ToString(); }
            set { m_drRekord[Column.TITLE] = value; }
        }
        [DataMember]
        public string Text {
            get { return m_drRekord[Column.TEXT].ToString(); }
            set { m_drRekord[Column.TEXT] = value; }
        }
    }
}
