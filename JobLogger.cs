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
    public class JobLogger
    {
        #region Attributos y Propiedades
        private static bool _logToFile;
        private static bool _logToConsole;
        private static bool _logMessage;
        private static bool _logWarning;
        private static bool _logError;
        private static bool _logToDatabase;
        private static bool _initialized=false;
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
                    throw new Exception("Invalid Configuration");
                }
                if (!_logError && !_logMessage && !_logWarning)
                {
                    throw new Exception("Message Type not defined");
                }
                _initialized = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            
        }
        public  static void LogMessage(string messageText, bool message, bool warning, bool error)
        {
            try
            {
                if (!_initialized)
                {
                    throw new Exception("Not initialized");
                }
                if (messageText == null || messageText.Trim().Length == 0)
                {
                    //no puede dejar el mensaje vacio 
                    throw new Exception("Messsage can not be empty");
                }

                bool[] types = { message, warning, error };
                int trueFound = 0;
                int typeMessage = 0;
                for (int i = 0; i < types.Count(); i++)
                {
                    //detectar si hay un tipo establecido como True
                    if (types[i] == true)
                    {
                        trueFound++;
                        typeMessage = (i+1);
                    }
                    //si hay mas de un tipo de mensaje establecido retornar exception
                    if (trueFound > 1)
                    {
                        //no puede definir mas de un tipo de mensaje por envio
                        throw new Exception("No more than one type of message defined");
                    }
                }
                //si no hay ningun tipo definido de mensaje retornar exception
                if (trueFound == 0)
                {
                    //debe definir solo un tipo de mensaje
                    throw new Exception("you must define only one type of message");
                }

                //si se ha definido la captura de mensajes y se a capturado un mensaje.
                if (_logMessage && message)
                {
                    //si seestablece salida por consola = true
                    if (_logToConsole)
                    {
                        LogMessageToConsole(messageText, 1, "Mensaje");
                    }
                    //si se establece salida por archivo.txt = true
                    if(_logToFile)
                    {
                        LogMessageToFile(messageText, 1, "Mensaje");
                    }
                    //si se establece salida por base de datos = true
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
                throw new Exception(e.Message);
            }

        }

        /**
         * Envia el mensaje de Log a la base de datos
         * DB = examTest1
         * Table = log
         */
        public static void LogMessageToDataBase(string messageText, bool message, bool warning, bool error)
        {
            try
            {
                if (!_initialized)
                {
                    throw new Exception("Not initialized");
                }
                if (!message && !error && !warning) { throw new Exception("you must define only one type of message");  }
                //establecer conexion desde string de conexion en appsettings
                string strconexion = System.Configuration.ConfigurationManager.AppSettings["connectionstring"];
                //establecer la conexion
                NpgsqlConnection conn = new NpgsqlConnection(strconexion);
                //enviar sql Insert
                NpgsqlCommand cmd = new NpgsqlCommand("insert into log(message, type_message, type_warning, type_error,created) VALUES('"+ messageText + "', "+ message +", "+ warning +", "+ error +", now())", conn);
                //abrir la conexion 
                conn.Open();
                //ejecutar la query 
                cmd.ExecuteNonQuery(); 
                //cerrar la conexion
                conn.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        /**
         * Envia el mensaje de log a un archivo .txt
         */
        public  static void LogMessageToFile(string messageText, int type, string typeName)
        {
            try
            {
                if (!_initialized)
                {
                    throw new Exception("Not initialized");
                }
                //comprobar maximos y minimos
                if (!(type>0) || (type>3))
                {
                    throw new Exception("type >0 and <4");
                }
                //comprobar texto no vacio
                if (messageText.Trim().Length==0)
                {
                    throw new Exception("Message cant be empty");
                }
                //comprobar texto no vacio
                if (typeName.Trim().Length==0)
                {
                    throw new Exception("TypeName cant be empty");
                }
                //obtiene la direccion del directorio o carpeta donde se almacenaran los logs.txt
                string folderPath = System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"];
                //ubicacion del archivo .txt donde se escribiran los logs
                string archivoLog = System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".txt";
                //comprobar si no existe el archivo 
                if (!System.IO.File.Exists(archivoLog))
                {
                    //comprobar si no existe el directorio logs
                    if (!System.IO.Directory.Exists(folderPath))
                    {
                        //si no exite se crea el directorio
                       System.IO.Directory.CreateDirectory(folderPath);
                    }
                    //se crea el archivo de texto
                    StreamWriter archivo = File.CreateText(archivoLog);
                    //se cierra el archivo
                    archivo.Close();
                    //System.IO.File.Create(archivoLog);
                }
                //string l = System.IO.File.ReadAllText(archivoLog);
                //se agrega una nueva linea al archivo
                File.AppendAllText(archivoLog,  DateTime.Now.ToShortDateString() + " " + typeName + " " + messageText + Environment.NewLine);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        /**
         * envia el mensaje log por consola, cambiando el color segun el tipo de mensaje. 
         */
        public static void LogMessageToConsole(string menssageText, int type, string typeName)
        {
            try
            {
                if (!_initialized)
                {
                    throw new Exception("Not initialized");
                }
                //comrpobar type no es 0
                if (!(type > 0)) { throw new Exception("El tipo debe ser mayor a 0"); }
                //comprobar typename no esta vacio
                if (typeName.Trim().Length == 0) { throw new Exception("El el nombre no puede ser vacio"); }
                //establecer segun tipo, el color del texto que saldrá por consola
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
                //escribe por consola el mensaje
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
