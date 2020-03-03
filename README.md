# Code Review

Please review the following code snippet. Assume that all referenced assemblies have been properly included.

The code is used to log different messages throughout an application. We want the ability to be able to log to a text file, the console and/or the database. Messages can be marked as message, warning or error. We also want the ability to selectively be able to choose what gets logged, such as to be able to log only errors or only errors and warnings.

1) If you were to review the following code, what feedback would you give? Please be specific and indicate any errors that would occur as well as other best practices and code refactoring that should be done.

2) Rewrite the code based on the feedback you provided in question 1. Please include unit tests on your code.


# Code

    using System;
	using System.Linq;
	using System.Text;


    namespace example
    {
        public class JobLogger
        {
            private static bool _logToFile;
            private static bool _logToConsole;
            private static bool _logMessage;
            private static bool _logWarning;
            private static bool _logError;
            //buena practica sería _logToDatabase para mentener la conveción
            private static bool LogToDatabase;
            private bool _initialized;

 


	        public JobLogger(bool logToFile, bool logToConsole, bool logToDatabase, bool 
	        logMessage, bool logWarning, bool logError)
	        {
			     
	            _logError = logError;
	            _logMessage = logMessage;
	            _logWarning = logWarning;
	            //atributo con el nombre _logToDatabase  para manterner convención
	            LogToDatabase = logToDatabase;
	            _logToFile = logToFile;
	            _logToConsole = logToConsole;
	            
	            /**
	            *Aqui Establecer Comprobaciones de: 
	            * 1 - comprobar que al menos un tipo de mensaje sea true.
	            * 2 - comprobar que al menos se establezca una salida para registrar (file,
	            *  console o database) 
	            * Si no se cunplen las reglas lanzar una excepción 
	            * Try - Catch
	            * para crear de forma correcta el constructor de la clase.
	            * si no se lanza ninguna excepción establecer _initialized como true
	            **/
	        }

	        public static void LogMessage(string message, bool message, bool warning, 
	        bool error)
	        {
		        /**
		        *Condicion para comprobar si _initialized es true
		        *los parámetros message (string)  y message (bool) son ambiguos por tener el 
		        *mismo nombre. Una mejor práctica es utilizar otro nombre 
		        *(PascalCase o CamelCase) como Message o messageText.
		        *
	            *usar trim en la misma linea del if como message.Trim().Length == 0
	            */
	            message.Trim();
	            if (message == null || message.Length == 0)
	            {
		            //cambiar return que al ser void no hace falta retorno.
		            //cambbiar reurn por lazar una excepción
	                return;
	            }
	            
	            //la siguiente condición se puede realizar en el constructor JobLogger
	            //lazando una excepción
	            if (!_logToConsole && !_logToFile && !LogToDatabase)
	            {
	                throw new Exception("Invalid configuration");
	            }
	            
	            //la siguiente condición se puede hacer en el constructor quitando la parte 
	            //de los tipos de mensajes  ~~(!message && !warning && !error)~~
	            if ((!_logError && !_logMessage && !_logWarning) || (!message && !warning && 
	            !error))
	            {
	                throw new Exception("Error or Warning or Message must be specified");
	            }
	            
	            /**
	            * agregar condición para determinar si no hay ningún tipo de mensaje establecido
	            * o si hay más de un mensaje establecido. Sólo se debe establecer uno.
	            */

				/**
				* Es recomendable crear un método para escribir el mensaje en un archivo de 
				* text y que el código sea más limpio
				**/
				
				/**
				* Utilizaré PostgreSQL para guardar el mensaje de log por temas de comidad en el
				*  entorno en el que se desarrollará.
				*/
				//se debe crear la key para almacenar el string de conexión en AppSettings
				
	            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.
	            SqlConnection(System.Configuration.ConfigurationManager
	            .AppSettings["ConnectionString"]);

	            connection.Open();
	            /**
	            * Las siguientes condiciones para saber el tipo de Mensaje 
	            * para almacenar por base de datos no es necesaria, porque no es una buena
	            * páctica almacenar el siguiente string en un campo: 
	            * (message + "', " + t.ToString) esto seria dificil de manipular en una 
	            * base de datos.
	            * Lo mejor sería:
	            * Tabla = log
	            * Campos = date Created, boolean t_warning, bool t_error, bool t_message, 
	            * string message.
	            * Almacenar los logs en base de datos no es conveniente por criterio de 
	            * rendimiento a menos que sea una situación que lo amerite.
	            */
	            int t;
	            if (message && _logMessage)
	            {
	                t = 1;
	            }
	            if (error && _logError)
	            {
	                t = 2;
	            }
	            if (warning && _logWarning)
	            {
	                t = 3;
	            }

	            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.
	            SqlCommand("Insert into Log Values('" + message + "', " + t.ToString() + ")");

	            command.ExecuteNonQuery();
	            
	            string l;
	            /**
	            * Crear un nuevo método para crear los archivos logs .txt 
	            * para código más limpio.
	            * 
	            * */
	            if(!System.IO.File.Exists(System.Configuration.ConfigurationManager.
	            AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + 
	            ".txt"))
	            {
	            /**
	            * No creo necesario leer todo el contenido del archivo de texto por criterio
	            * de rendimiento, se puede agreagar una linea nueva sin necesidad de hacerlo.
	            *
	            * Condiciones para detectar existencia de la carpeta logs y del archivo 
	            * para de no ser asi crearlos y escribir el mensaje.
	            *
	            */
	                l = System.IO.File.ReadAllText(System.Configuration.ConfigurationManager.
	                AppSettings["LogFileDirectory"] + "LogFile" + 
	                DateTime.Now.ToShortDateString() + ".txt");
	            }

				

				//estas condiciones no son necesarias sino hay que leer todo el 
				//contenido del archivo .txt
	            if (error && _logError)
	            {
	                l = l + DateTime.Now.ToShortDateString() + message;
	            }
	            if (warning && _logWarning)
	            {
	                l = l + DateTime.Now.ToShortDateString() + message;
	            }
	            if (message && _logMessage)
	            {
	                l = l + DateTime.Now.ToShortDateString() + message;
	            }

	            //el metodo WriteAllText no es necesario se puede utilizar AppendAllText 
	            //sin necesidad de leer todo el contenido del archivo.

				//al crear el archivo agregaría .Replace("/", "_")  a 
				//DateTime.Now.ToShortDateString() ya que Windows no permite la barrra /
				//como nombre de un archivo.
				
	            System.IO.File.WriteAllText(System.Configuration.ConfigurationManager.
	            AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + 
	            ".txt", l);
				/**
				* Crear la salida por consola en un nuevo método
				* /
	            if (error && _logError)
	            {
	                Console.ForegroundColor = ConsoleColor.Red;
	            }
	            if (warning && _logWarning)
	            {
	                Console.ForegroundColor = ConsoleColor.Yellow;
	            }
	            if (message && _logMessage) 
	            {
	                Console.ForegroundColor = ConsoleColor.White;
	            }
	            Console.WriteLine(DateTime.Now.ToShortDateString()  + message);
	        }
	    }
	}

**Data Base Create File:** DataBaseCreated.sql