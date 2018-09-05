using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfService {

    public partial class MemosForm  {
        private const string EMPTY_STRING = "";
       
        public string getLogin(){
            return Data.Config.User.Login;
        }

        public DataTable LoadData() {
            DataTable  db_memo_table = Data.Config.sql.GetTable(
                Data.Table.Memo.TableName,
                new SQL.Parameter[]
                {
                    new SQL.Parameter(
                        Data.Table.Memo.Column.USER_ID,
                        Data.Config.User.Id)
                });

            return db_memo_table;
        }

        public List<Data.Table.Memo> UpdateData(DataTable table) {
            return SQL.Data.Elements<Data.Table.Memo>.CreateElements(table).OrderBy(el => el.Title).ToList();
        }

        public void CommitAndUpdate(DataTable table) {
            Data.Config.sql.Commit(table);
            UpdateData(table);
        }


        public void DeleteNote(DataTable table, string Title, string Text) {
                var newRow = table.NewRow();
                var row = SQL.Data.Elements<Data.Table.Memo>.CreateElement(newRow);
                row.Text = Text;
                row.Title = Title;
                row.UserId = Data.Config.User.Id;
                row.Id = Data.Config.sql.GetNextId(Data.Table.Memo.TableName);
                table.Rows.Add(row.ElementDataRow);

            Data.Config.sql.Commit(table);
            UpdateData(table);
        }
    }
}
