using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL {
    public class Response {
        private string message;
        private bool status;

        public string Message { get => message; set => message = value; }
        public bool Status { get => status; set => status = value; }
    }


}
