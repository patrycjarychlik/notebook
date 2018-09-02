using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SQL.Data
{
    public class Elements<T> where T : Element
    {
        #region Public
        public static T CreateElement(DataRow row)
        {
            return Activator.CreateInstance(typeof(T), new object[] { row }) as T;
        }

        public static List<T> CreateElements(DataTable table)
        {
            List<T> wynik = new List<T>();

            foreach (DataRow wiersz in table.Rows)
            {
                if (wiersz.RowState != DataRowState.Deleted)
                    wynik.Add(CreateElement(wiersz));
            }
            return wynik;
        }

        public static List<T> CreateElements(DataRow[] rows)
        {
            List<T> wynik = new List<T>();

            foreach (DataRow wiersz in rows)
            {
                if (wiersz.RowState != DataRowState.Deleted)
                    wynik.Add(CreateElement(wiersz));
            }
            return wynik;
        }

        public static List<T> CreateElements(DataRowCollection rows)
        {
            List<T> wynik = new List<T>();

            foreach (DataRow wiersz in rows)
            {
                if (wiersz.RowState != DataRowState.Deleted)
                    wynik.Add(CreateElement(wiersz));
            }
            return wynik;
        }

        public static List<T> CreateElements(ISQL sql, string tableName)
        {
            var ds = sql.ExecuteQuery("SELECT * FROM " + tableName);
            sql.SetTableName(ds.Tables[0], tableName);
            return Elements<T>.CreateElements(ds.Tables[0].Rows);
        }

        public static List<T> CreateElements(ISQL sql, string tableName, string columnName, string columnValue)
        {
            var ds = sql.ExecuteQuery("SELECT * FROM " + tableName + " WHERE " + columnName + " = '" + columnValue + "'");
            sql.SetTableName(ds.Tables[0], tableName);
            return Elements<T>.CreateElements(ds.Tables[0].Rows);
        }

        #endregion Public

    }
}
