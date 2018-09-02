using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;


namespace SQL.SqLite
{
    public class SQL : ISQL
    {
        cDbInfo m_dbInfo;
        SQLiteConnection m_connection;
        internal Transaction CurrentTransction = null;
        internal SQLiteTransaction2 Transaction = null;

        public cDbInfo DbInfo { get { return m_dbInfo; } }
        internal SQLiteConnection Connection { get { return m_connection; } }

        public SQL(cDbInfo dbInfo)
        {
            m_dbInfo = dbInfo;
            m_connection = SQL.CreateConnection(dbInfo);
        }

        internal static SQLiteConnection CreateConnection(cDbInfo dbInfo)
        {
            SQLiteConnection c = new SQLiteConnection(SQL.CreateConnectionString(dbInfo).ConnectionString);
            c.Flags = System.Data.SQLite.SQLiteConnectionFlags.AllowNestedTransactions;
            return c;
        }

        internal static SQLiteConnectionStringBuilder CreateConnectionString(cDbInfo dbInfo)
        {
            SQLiteConnectionStringBuilder c = new SQLiteConnectionStringBuilder();
            c.DataSource = dbInfo.IP;
            c.Password = dbInfo.Password;
            return c;
        }

        #region Public

        public eConnectionStatus ConnectionTest()
        {
            return ConnectionTest(m_dbInfo);
        }

        public eConnectionStatus ConnectionTest(cDbInfo dbInfo)
        {
            try
            {
                SQLiteConnection c = new SQLiteConnection(SQL.CreateConnectionString(dbInfo).ConnectionString);
                c.Open();
                if (c.State == System.Data.ConnectionState.Open)
                {
                    c.Close();
                    return eConnectionStatus.OK;
                }
            }
            catch (SQLiteException e)
            {
                return eConnectionStatus.ERROR;
            }
            return eConnectionStatus.ERROR;
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            CurrentTransction = new Transaction(this, isolationLevel);
            return CurrentTransction;
        }

        public int ExecuteNoQuery(string query, Parameter[] parameters = null)
        {
            if (m_connection.State == ConnectionState.Closed)
                m_connection.Open();

            if (CurrentTransction != null && CurrentTransction.Commited)
                throw new Exception("Current transaction was alredy commited.");

            SQLiteCommand c = new SQLiteCommand(query, m_connection);

            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    c.Parameters.Add(new SQLiteParameter(p.Name, p.Value));
                }
            }

            return c.ExecuteNonQuery();
        }

        public DataSet ExecuteQuery(string query, Parameter[] parameters = null)
        {
            if (m_connection.State == ConnectionState.Closed)
                m_connection.Open();

            if (CurrentTransction != null && CurrentTransction.Commited)
                throw new Exception("Current transaction was alredy commited.");

            SQLiteCommand c = new SQLiteCommand(query, m_connection);
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    c.Parameters.Add(new SQLiteParameter(p.Name, p.Value));
                }
            }
            SQLiteDataAdapter da = new SQLiteDataAdapter(c);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public void SetTableName(DataTable table, string name)
        {
            table.TableName = name;
            string[] primaryKeys = PrimaryKey(name);

            List<DataColumn> dc = new List<DataColumn>();
            foreach (var el in primaryKeys)
            {
                table.Columns[el].Unique = true;
                dc.Add(table.Columns[el]);
            }
            table.PrimaryKey = dc.ToArray();
        }

        public void Delete(DataTable dt)
        {
            if (dt.PrimaryKey.Length == 0)
                throw new ArgumentNullException("Primary key not found.");

            DataTable mod = dt.GetChanges(DataRowState.Deleted);
            if (mod == null)
                return;
            foreach (DataRow dr in mod.Rows)
            {
                string query = "DELETE FROM " + dt.TableName + " WHERE ";
                List<Parameter> param = new List<Parameter>();
                int i = 1;
                foreach (DataColumn dc in dt.PrimaryKey)
                {
                    query += dc.ColumnName + " = :P" + i.ToString();
                    param.Add(new Parameter("P" + i.ToString(), dr[dc.ColumnName, DataRowVersion.Original]));
                    i++;
                    if (dc != dt.PrimaryKey[dt.PrimaryKey.Length - 1])
                    {
                        query += " AND ";
                    }
                }

                ExecuteNoQuery(query, param.ToArray());
            }
        }

        public void Insert(DataTable dt)
        {
            DataTable mod = dt.GetChanges(DataRowState.Added);
            if (mod == null)
                return;
            foreach (DataRow dr in mod.Rows)
            {
                int i = 1;

                string query = "INSERT INTO " + dt.TableName + " (";
                string query2 = " ) VALUES ( ";


                List<Parameter> param = new List<Parameter>();
                foreach (DataColumn dc in dt.Columns)
                {
                    query += dc.ColumnName;
                    query2 += ":P" + i.ToString();
                    param.Add(new Parameter("P" + i.ToString(), dr[dc.ColumnName]));
                    i++;
                    if (dc != dt.Columns[dt.Columns.Count - 1])
                    {
                        query += ", ";
                        query2 += ", ";
                    }
                }
                query += query2 + ")";
                ExecuteNoQuery(query, param.ToArray());
            }
        }

        public void Update(DataTable dt)
        {
            DataTable mod = dt.GetChanges(DataRowState.Modified);
            if (mod == null)
                return;
            if (dt.PrimaryKey.Length == 0)
                throw new ArgumentNullException("Primary key not found.");

            foreach (DataRow dr in mod.Rows)
            {
                int i = 1;

                string query = "UPDATE " + dt.TableName + " SET ";
                List<Parameter> param = new List<Parameter>();
                foreach (DataColumn dc in dt.Columns)
                {
                    query += dc.ColumnName + " = :P" + i.ToString() + " ";
                    param.Add(new Parameter("P" + i.ToString(), dr[dc.ColumnName]));
                    i++;
                    if (dc != dt.Columns[dt.Columns.Count - 1])
                    {
                        query += ",";
                    }
                }
                query += " WHERE ";

                foreach (DataColumn dc in dt.PrimaryKey)
                {
                    query += dc.ColumnName + " = :P" + i.ToString() + " ";
                    param.Add(new Parameter("P" + i.ToString(), dr[dc.ColumnName]));
                    i++;
                    if (dc != dt.PrimaryKey[dt.PrimaryKey.Length - 1])
                    {
                        query += " AND ";
                    }
                }
                ExecuteNoQuery(query, param.ToArray());
            }
        }

        public void Commit(DataTable dt, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var t = BeginTransaction(isolationLevel))
            {
                Delete(dt);
                Update(dt);
                Insert(dt);
                t.Commit();
                dt.AcceptChanges();
            }
        }

        public DataTable GetTableStructure(string tableName)
        {
            DataTable dt = ExecuteQuery("SELECT * FROM " + tableName + " WHERE 1=2").Tables[0];
            SetTableName(dt, tableName);
            return dt;
        }

        public DataTable GetTable(string tableName)
        {
            DataTable dt = ExecuteQuery("SELECT * FROM " + tableName).Tables[0];
            SetTableName(dt, tableName);
            return dt;
        }

        public DataTable GetTable(string tableName, Parameter[] columnValue)
        {
            List<Parameter> param = new List<Parameter>();
            string q = "SELECT * FROM " + tableName;
            if(columnValue != null && columnValue.Length > 0)
            {
                q += " WHERE";
                for(int i = 0; i < columnValue.Length; i++)
                {
                    if(i > 0)
                        q += " AND ";
                    q += " " + columnValue[i].Name + " = :p" + i.ToString();
                    param.Add(new Parameter("p" + i.ToString(), columnValue[i].Value));
                }
            }
            DataTable dt = ExecuteQuery(q, param.ToArray()).Tables[0];
            SetTableName(dt, tableName);
            return dt;
        }
    
        public int GetNextId(string tableName)
        {
            var ds = ExecuteQuery("SELECT MAX(id) FROM " + tableName);
            string strId = ds.Tables[0].Rows[0][0].ToString();
            if (string.IsNullOrWhiteSpace(strId))
                return 0;
            int id = int.Parse(strId);
            id++;
            return id;
        }

        #endregion Public

        private string[] PrimaryKey(string tableName)
        {
            string sql = "pragma table_info(" + tableName + ")";
            List<string> list = new List<string>();
            foreach (DataRow dr in ExecuteQuery(sql).Tables[0].Rows)
            {
                if (dr["pk"].ToString() != "1")
                    continue;
                list.Add(dr["name"].ToString());
            }
            return list.ToArray();
        }
    }
}
