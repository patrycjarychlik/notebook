using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SQL
{
    public enum eConnectionStatus
    {
        OK,
        SERVER_NOT_FOUND,
        WRONG_LOGIN_OR_PASSWORD,
        SERVER_ERROR,
        ERROR
    }

    public interface ISQL
    {
        eConnectionStatus ConnectionTest();
        eConnectionStatus ConnectionTest(cDbInfo dbInfo);
        ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        int ExecuteNoQuery(string query, Parameter[] parameters = null);
        DataSet ExecuteQuery(string query, Parameter[] parameters = null);
        DataTable GetTableStructure(string tableName);
        DataTable GetTable(string tableName);
        DataTable GetTable(string tableName, Parameter[] columnValue);
        void SetTableName(DataTable table, string name);
        void Delete(DataTable dt);
        void Insert(DataTable dt);
        void Update(DataTable dt);
        void Commit(DataTable dt, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        int GetNextId(string tableName);

    }
}
