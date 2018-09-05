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
            listBox1.DataSource = null;

           ServiceReference1.Service1Client proxy = new ServiceReference1.Service1Client();
            base.Text = "Manager Notatek : " + proxy.getLogin();
            proxy.Close();
        }

        private List<MemoView> db_memos_list;
        private DataTable db_memo_table;

        public string Title {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public string Text {
            get { return richTextBox1.Text; }
            set { richTextBox1.Text = value; }
        }

        private MemoView SelectedMemo {
            get {
                if (listBox1.SelectedIndex == -1)
                    return null;
                return db_memos_list[listBox1.SelectedIndex];
            }
        }

        private void LoadData() {
            ServiceReference1.Service1Client proxy = new ServiceReference1.Service1Client();
            db_memo_table = proxy.LoadData();
            UpdateData();
            proxy.Close();
        }

        private void UpdateData() {
            ServiceReference1.Service1Client proxy = new ServiceReference1.Service1Client();

            is_data_updating = true;
            Notatki.ServiceReference1.MemoDto[] dto = proxy.UpdateData(db_memo_table);
            db_memos_list = new List<MemoView>();

            foreach (Notatki.ServiceReference1.MemoDto memoDto in dto) {
                db_memos_list.Add(new MemoView(memoDto.Title, memoDto.Text, memoDto.Id, memoDto.UserId));
            }
            try {
                listBox1.DataSource = null;
                listBox1.DataSource = db_memos_list.Select(s => (string)s.Title).ToArray();
            } finally {
                is_data_updating = false;
            }
            memo_list_SelectedIndexChanged(this, new EventArgs());
            proxy.Close();
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
            ServiceReference1.Service1Client proxy = new ServiceReference1.Service1Client();

            if (proxy.getLogin() == null) {
                DialogResult = DialogResult.Cancel;
                return;
            }
            LoadData();
            proxy.Close();
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
                ServiceReference1.Service1Client proxy = new ServiceReference1.Service1Client();

                for (int i = db_memo_table.Rows.Count - 1; i >= 0; i--) {
                    DataRow dr = db_memo_table.Rows[i];
                    if (dr["Id"].ToString() == SelectedMemo.Id.ToString())
                        dr.Delete();
                }
                proxy.CommitAndUpdate(db_memo_table);
                UpdateData();
            }
        }

        private void save_active_note(object sender, EventArgs e) {
            ServiceReference1.Service1Client proxy = new ServiceReference1.Service1Client();

            if (SelectedMemo == null) {
                proxy.DeleteNote(db_memo_table, Title, Text);
            } else {
                SelectedMemo.Text = Text;
                SelectedMemo.Title = Title;

                for (int i = db_memo_table.Rows.Count - 1; i >= 0; i--) {
                    DataRow dr = db_memo_table.Rows[i];
                    if (dr["Id"].ToString() == SelectedMemo.Id.ToString()) {
                        dr.SetField(3, Text);
                        dr.SetField(2, Title);
                    }
                }
                proxy.CommitAndUpdate(db_memo_table);
            }
            proxy.Close();
            LoadData();
        }

        private void list_showed(object sender, EventArgs e) {
            ServiceReference1.Service1Client proxy = new ServiceReference1.Service1Client();

            if (proxy.getLogin() == null) {
                DialogResult = DialogResult.Cancel;
                Application.Exit();
                return;
            }
            proxy.Close();
        }

    }
}
