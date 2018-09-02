using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SQL.SqLite
{
    public class Transaction : ITransaction
    {
        private string m_savePointName;

        private bool m_commited = false;
        private Transaction m_parentTransaction = null;
        private SQL m_conn;

        public bool Commited { get { return m_commited; } }

        private void CreateTransaction(SQL conn, IsolationLevel isolationLevel)
        {
            m_conn = conn;
            m_parentTransaction = conn.CurrentTransction;

            if (conn.Connection.State == ConnectionState.Closed)
                conn.Connection.Open();
            conn.Transaction = (System.Data.SQLite.SQLiteTransaction2)conn.Connection.BeginTransaction(isolationLevel);

            conn.CurrentTransction = this;
        }

        public Transaction(SQL conn)
        {
            if (conn.Transaction != null)
                CreateTransaction(conn, conn.Transaction.IsolationLevel);
        }

        public Transaction(SQL conn, IsolationLevel isolationLevel)
        {
            CreateTransaction(conn, isolationLevel);
        }

        public void Commit()
        {
            m_conn.Transaction.Commit();
            m_commited = true;
        }

        public void Dispose()
        {
            if (!m_commited)
            {
                m_conn.Transaction.Rollback();
            }
            m_conn.CurrentTransction = m_parentTransaction;
        }
    }
}
