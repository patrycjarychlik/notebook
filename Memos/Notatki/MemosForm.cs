using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Główne okno managera notatek
namespace Notatki {

    public partial class MemosForm : Form {
        private const string EMPTY_STRING = "";
        private bool is_data_updating = false;

        public MemosForm() {
            InitializeComponent();

            SignInForm login = new SignInForm();
            if (login.ShowDialog() != DialogResult.OK) {
                Application.Exit();
                return;
            }
            base.Text = "Manager Notatek : " + Data.Config.User.Login;
        }

        private List<Data.Table.Memo> db_memos_list;
        private DataTable db_memo_table;

        public string Title {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public string Text {
            get { return richTextBox1.Text; }
            set { richTextBox1.Text = value; }
        }

        private Data.Table.Memo SelectedMemo {
            get {
                if (listBox1.SelectedIndex == -1)
                    return null;
                return db_memos_list[listBox1.SelectedIndex];
            }
        }

        private void LoadDData() {
            db_memo_table = Data.Config.sql.GetTable(
                Data.Table.Memo.TableName,
                new SQL.Parameter[]
                {
                    new SQL.Parameter(
                        Data.Table.Memo.Column.USER_ID,
                        Data.Config.User.Id)
                });

            UpdateData();
        }

        private void UpdateData() {
            is_data_updating = true;
            db_memos_list = SQL.Data.Elements<Data.Table.Memo>.CreateElements(db_memo_table).OrderBy(el => el.Title).ToList();
            try {
                listBox1.DataSource = db_memos_list.Select(el => el.Title).ToArray();
            } finally {
                is_data_updating = false;
            }
            memo_list_SelectedIndexChanged(this, new EventArgs());
        }

        private void new_memo(object sender, EventArgs e) {
            Text = EMPTY_STRING;
            Title = EMPTY_STRING;
            textBox1.Focus();
            is_data_updating = true;
            try {
                listBox1.SelectedItem = null;
            } finally {
                is_data_updating = false;
            }
        }

        private void load_list(object sender, EventArgs e) {
            if (Data.Config.User == null) {
                DialogResult = DialogResult.Cancel;
                return;
            }
            LoadDData();
        }

        private void memo_list_SelectedIndexChanged(object sender, EventArgs e) {
            if (is_data_updating) {
                return;
            }
            if (SelectedMemo != null) {
                Text = SelectedMemo.Text;
                Title = SelectedMemo.Title;
            } else {
                Text = EMPTY_STRING;
                Title = "";
            }
        }

        private void delete_active_memo(object sender, EventArgs e) {
            if (SelectedMemo != null) {
                SelectedMemo.ElementDataRow.Delete();
                Data.Config.sql.Commit(db_memo_table);
                UpdateData();
            }
        }

        private void delete_active_note(object sender, EventArgs e) {
            if (SelectedMemo == null) {
                var newRow = db_memo_table.NewRow();
                var row = SQL.Data.Elements<Data.Table.Memo>.CreateElement(newRow);
                row.Text = Text;
                row.Title = Title;
                row.UserId = Data.Config.User.Id;
                row.Id = Data.Config.sql.GetNextId(Data.Table.Memo.TableName);
                db_memo_table.Rows.Add(row.ElementDataRow);
            } else {
                SelectedMemo.Text = Text;
                SelectedMemo.Title = Title;
            }
            Data.Config.sql.Commit(db_memo_table);
            UpdateData();
        }

        private void list_showed(object sender, EventArgs e) {
            if (Data.Config.User == null) {
                DialogResult = DialogResult.Cancel;
                Application.Exit();
                return;
            }
        }

    }
}
