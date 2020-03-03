using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examTest1
{
    class Program
    {
        static void Main(string[] args)
        {
            //por defecto envia un mensaje de error

             JobLogger log = new JobLogger(true, true, true,      false, false,true);

             JobLogger.LogMessage("Error Message Test", false, false, true);
             

            
        }


    }

}
