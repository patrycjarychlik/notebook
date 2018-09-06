using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadania {
    interface Generator {

        List<double> GetData();
        void PassParameter(Object obj);
    }
}
