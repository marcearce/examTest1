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

            JobLogger log = new JobLogger(false, true, false,      true, true,true);

            //JobLogger.LogMessage("Error Message Test", false, true,false);

            //JobLogger.LogMessage("mensaje", true,false, false );


            int valor1 = 50;
            int valor2 = 0;

            try
            {
                Console.WriteLine(valor1/valor2);

            }
            catch (Exception e)
            {
                JobLogger.LogMessage("No se puede dividir por 0", false, false, true);
                Console.WriteLine(e.Message);
            }

            
          


            
        }


    }

}
