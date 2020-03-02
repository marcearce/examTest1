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
            //DataBase db = new DataBase();
            //Console.WriteLine("Hola");
            //Console.WriteLine(db.Open().ToString());


            
             JobLogger log = new JobLogger(true, true, true,      true, true, true);

             JobLogger.LogMessage("Este es un mensaje de Error", false, true, false);
             
            //string archivoLog = System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".txt";

            //System.IO.Directory.CreateDirectory("C:/Logs/" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".txt");
            //System.IO.File.AppendText("C:/Logs/" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".txt");

            
        }

        //string carpeta = System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".txt";
        //Console.WriteLine(System.IO.File.Exists(carpeta));


        //Console.WriteLine("se ha guardado correctamente");

    }

}
