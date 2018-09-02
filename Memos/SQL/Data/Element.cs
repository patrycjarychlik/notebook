using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SQL.Data
{
    public class Element
    {

        #region Zmienne

        /// <summary>
        /// Rekord dla tej klasy
        /// </summary>
        protected DataRow m_drRekord;

        #endregion Zmienne

        #region Kontruktory

        /// <summary>
        /// Prywatny konstruktor
        /// </summary>
        protected Element() { }

        /// <summary>
        /// Publiczny kontruktor
        /// </summary>
        /// <param name="drRekord">Rekord obiektu</param>
        public Element(DataRow drRekord)
        {
            this.m_drRekord = drRekord;
        }
        #endregion Konstruktory

        #region Wlasciwosci

        /// <summary>
        /// DataRow powiazany z elementem
        /// </summary>
        public DataRow ElementDataRow
        {
            get { return m_drRekord; }
        }

        public object this[int id]
        {
            get { return m_drRekord[id]; }
            set { m_drRekord[id] = value; }
        }

        public object this[string columnName]
        {
            get { return m_drRekord[columnName]; }
            set { m_drRekord[columnName] = value; }
        }

        public string[] ColumnName
        {
            get
            {
                List<string> name = new List<string>();
                foreach (System.Data.DataColumn column in m_drRekord.Table.Columns)
                {
                    name.Add(column.ColumnName);
                }
                return name.ToArray();
            }
        }

        #endregion

    }

}