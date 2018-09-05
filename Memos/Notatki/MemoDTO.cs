using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Notatki {
    [Serializable]

    public class MemoView {
        public string Text { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public MemoView(string title2, string text2, int id2, int userId2) {
            Title = title2;
            Text = text2;
            Id = id2;
            UserId = userId2;
            
        }
    }
}
