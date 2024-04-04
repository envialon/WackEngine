using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WackEditor.Utilities
{

    //TODO: This is disconnected from the id_type of the engine, 
    //if id_type changes to a different byte size number, this will create a bug.
    internal class IdUtils
    {
        public static int INVALID_ID = -1;

        public static bool IsValid(int id) => id == INVALID_ID;

    }
}
