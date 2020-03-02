using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;
using System.IO;
using Npgsql;
using NpgsqlTypes;

namespace examTest1
{
    class JobLogger
    {
        #region Attributos y Propiedades
        private static bool _logToFile;
        private static bool _logToConsole;
        private static bool _logMessage;
        private static bool _logWarning;
        private static bool _logError;
        private static bool _logToDatabase;
        #endregion

        
        #region Metodos
        public JobLogger(bool logToFile, bool logToConsole, bool logToDatabase, bool logMessage, bool logWarning, bool logError)
        {
            try
            {
                _logError = logError;
                _logMessage = logMessage;
                _logWarning = logWarning;

                _logToDatabase = logToDatabase;
                _logToFile = logToFile;
                _logToConsole = logToConsole;
                if (!_logToConsole && !_logToFile && !_logToDatabase)
                {
                    throw new Exception("Configuración inválida");
                }
                if (!_logError && !_logMessage && !_logWarning)
                {
                    throw new Exception("Debe Especificar el algun tipo de mensaje");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            
        }
        public  static void LogMessage(string messageText, bool message, bool warning, bool error)
        {
            try
            {
                messageText.Trim();
                if (messageText == null || messageText.Length == 0)
                {
                    throw new Exception("El mensaje no puede estar vacio.");
                }

                bool[] types = { message, warning, error };
                int trueFound = 0;
                int typeMessage = 0;
                for (int i = 0; i < types.Count(); i++)
                {
                    if (types[i] == true)
                    {
                        trueFound++;
                        typeMessage = (i+1);
                    }

                    if (trueFound > 1)
                    {
                        throw new Exception("Sólo debe especificar un tipo de mensaje.");
                    }
                }
                if (trueFound == 0)
                {
                    throw new Exception("Débe especificar el tipo de mensaje");
                }

                if (_logMessage && message)
                {
                    if (_logToConsole)
                    {
                        LogMessageToConsole(messageText, 1, "Mensaje");
                    }
                    if(_logToFile)
                    {
                        LogMessageToFile(messageText, 1, "Mensaje");
                    }
                    if(_logToDatabase)
                    {
                        LogMessageToDataBase(messageText, message, warning, error);
                    }
                }

                if (_logError && error)
                {
                    if (_logToConsole)
                    {
                        LogMessageToConsole(messageText, 2, "Error");
                    }
                    if (_logToFile)
                    {
                        LogMessageToFile(messageText, 2, "Error");
                    }
                    if (_logToDatabase)
                    {
                        LogMessageToDataBase(messageText, message, warning, error);
                    }
                }

                if (_logWarning && warning)
                {
                    if (_logToConsole)
                    {
                        LogMessageToConsole(messageText, 3, "Advertencia");
                    }
                    if (_logToFile)
                    {
                        LogMessageToFile(messageText, 3, "Advertencia");
                    }
                    if (_logToDatabase)
                    {
                        LogMessageToDataBase(messageText, message, warning, error);
                    }

                }
            }
            catch (Exception e )
            {
                Console.WriteLine(e.Message);
            }

        }


        public static void LogMessageToDataBase(string messageText, bool message, bool warning, bool error)
        {
            try
            {
                string strConexion = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
                NpgsqlConnection conn = new NpgsqlConnection("Host=localhost;Username=postgres;Password=postgres;Database=examTest1");
                NpgsqlCommand cmd = new NpgsqlCommand("insert into log(message, type_message, type_warning, type_error,created) VALUES('"+ messageText + "', "+ message +", "+ warning +", "+ error +", now())", conn);
                conn.Open();
                cmd.ExecuteNonQuery(); 
                conn.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static void LogMessageToFile(string messageText, int type, string typeName)
        {
            try
            {
                string folderPath = System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"];
                string archivoLog = System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".txt";
                if (!System.IO.File.Exists(archivoLog))
                {
                    if (!System.IO.Directory.Exists(folderPath))
                    {
                       System.IO.Directory.CreateDirectory(folderPath);
                    }
                    StreamWriter archivo = File.CreateText(archivoLog);
                    archivo.Close();
                    //System.IO.File.Create(archivoLog);
                }
                //string l = System.IO.File.ReadAllText(archivoLog);
                File.AppendAllText(archivoLog,  DateTime.Now.ToShortDateString() + " " + typeName + " " + messageText + Environment.NewLine);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        public static void LogMessageToConsole(string menssageText, int type, string typeName)
        {
            try
            {

                if (type==1)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (type == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                if (type == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                Console.WriteLine(DateTime.Now.ToShortDateString().Replace("/", "-")+ " [" + typeName + "]" + " " + menssageText );

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }

        }
        
        
        #endregion
    }

}
