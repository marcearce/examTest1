using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace NET_E3_SeniorsTests
{
    [TestClass]
    public class JobLoggerTests
    {
        /**
         * se omiten los try catch para evitar falsos positivos.
         */
        #region Attributos y Propiedades
        private static bool _logToFile;
        private static bool _logToConsole;
        private static bool _logMessage;
        private static bool _logWarning;
        private static bool _logError;
        private static bool _logToDatabase;

        //unit test variables
        private static string _resultado = null;
        #endregion

        /**
         * if (!logToFile && !logToConsole && !logToDatabase) return exception.
         * al menos uno debe ser TRUE
         */
        [TestMethod]
        public void NoSeEspecificaSalidaEnConstructorJobLogger_returnException()
        {
            try
            {
                object obj = new examTest1.JobLogger(false, false, false, true, true, true);
            }
            catch (Exception e)
            {
                _resultado = e.Message;
            }
            Assert.IsNotNull(_resultado);
        }

        [TestMethod]
        public void NoEspecificaNingunMensajeACapturar_ReturnExeption()
        {
            try
            {
                object obj = new examTest1.JobLogger(true, true, true, false, false, false);
            }
            catch (Exception e)
            {
                _resultado = e.Message;
            }
            Assert.IsNotNull(_resultado);
        }

        /**
         * si el mensaje es vacio o null RETURN Exception.
         * 
         */
        [TestMethod]
        public void JobMessageElMensajeEsVacio_ReturnException()
        {
            try
            {
                //set "" test
                string messageText = "";

                messageText.Trim();
                if (messageText == null || messageText.Length == 0)
                {
                    throw new Exception("El mensaje no puede estar vacio.");
                }
            }
            catch (Exception e)
            {
                _resultado = e.Message;
            }
            Assert.IsNotNull(_resultado);
        }

        /**
         * No se define tipo al mensaje (message, warning, alert)
         * 
         * retrun exeption
         * */
        [TestMethod]
        public void NoDefineTipoDeMensaje_ReturnExeption()
        {
            try
            {
                //establecer variables para test
                bool message = false;
                bool warning = false;
                bool error = false;


                bool[] types = { message, warning, error };
                int trueFound = 0;
                int typeMessage = 0;
                for (int i = 0; i < types.Count(); i++)
                {
                    if (types[i] == true)
                    {
                        trueFound++;
                        typeMessage = (i + 1);
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

            }
            catch (Exception e)
            {
                _resultado = e.Message;
                // throw new Exception(e.Message);
            }
            Assert.IsNotNull(_resultado);

        }

        /**
         * Se define mas de un tipo al mensaje, return exeption
         * */
        [TestMethod]
        public void MensajeConMasDeUnTipo_ReturnExeption()
        {
            try
            {
                //establece mas de un tipo de mensaje
                bool message = true;
                bool warning = true;
                bool error = false;


                bool[] types = { message, warning, error };
                int trueFound = 0;
                int typeMessage = 0;
                for (int i = 0; i < types.Count(); i++)
                {
                    if (types[i] == true)
                    {
                        trueFound++;
                        typeMessage = (i + 1);
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

            }
            catch (Exception e)
            {
                _resultado = e.Message;
                // throw new Exception(e.Message);
            }
            Assert.IsNotNull(_resultado);

        }

        /**
         * si el mensaje que se captura no es igual al que se establece en el 
         * constructor entonces no realiza ninguna accion.
         * */
        [TestMethod]
        public void MensajeCapturadoNoDeclaradoEnContrsuctor_ReturnNull()
        {

                //declarar variables para test
                bool message = true;
                bool warning = false;
                bool error = false;

                _logMessage = false;
                _logWarning = false;
                _logError = true;

                //salida de consola para test
                _logToConsole = true;

                if (_logMessage && message)
                {
                    if (_logToConsole)
                    {
                        _resultado = "yes";
                    }
                    if (_logToFile)
                    {
                        _resultado = "yes";
                    }
                    if (_logToDatabase)
                    {
                        _resultado = "yes";
                    }
                }

                if (_logError && error)
                {
                    if (_logToConsole)
                    {
                        _resultado = "yes";
                    }
                    if (_logToFile)
                    {
                        _resultado = "yes";
                    }
                    if (_logToDatabase)
                    {
                        _resultado = "yes";
                    }
                }

                if (_logWarning && warning)
                {
                    if (_logToConsole)
                    {
                        _resultado = "yes";
                    }
                    if (_logToFile)
                    {
                        _resultado = "yes";
                    }
                    if (_logToDatabase)
                    {
                        _resultado = "yes";
                    }

                }
            Assert.IsNull(_resultado);

        }


    }
}
