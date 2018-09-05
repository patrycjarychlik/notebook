using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace SQL {
    [Serializable]
    [DataContract]
    public class MemoDto {
        private string text;
        private string title;
        private int id;
        private int userId;
        public MemoDto(string title2, string text2, int id2, int userId2) {
            title = title2;
            text = text2;
            id = id2;
            userId = userId2;
        }

        public MemoDto() { }

        [DataMember]
        public string Title { get => title; set => title = Title; }
        [DataMember]
        public string Text { get => text; set => text = Text; }
        [DataMember]
        public int Id { get => id; set => id = Id; }
        [DataMember]
        public int UserId { get => userId; set => Id = UserId; }

    }
}
