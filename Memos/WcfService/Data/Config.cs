using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfService.Data {
    public static class Config {
        public const string DbFilePath = "db.sqlite";
        public const string EmptyDbFilePath = "db_empty.sqlite";
        public const string DbPassword = "";
        public static SQL.ISQL sql;
        public static Data.Table.User User;
    }
}
