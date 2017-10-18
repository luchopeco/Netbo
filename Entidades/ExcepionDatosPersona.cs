using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ExcepionDatosPersona : Exception
    {
        #region CONSTRUCTORES
        public ExcepionDatosPersona()
        {
            // = "Error...";
        }
        public ExcepionDatosPersona(string message): base(message)
        {
            End = false;
        }
        public ExcepionDatosPersona(string message, Exception inner): base(message, inner)
        {
            End = false;
        }
        public ExcepionDatosPersona(string message, bool termina): base(message)           
        {
            end = termina;
        }
        #endregion CONSTRUCTORES

        #region PROPIEDADES
        private bool end;
        public bool End
        {
            get { return end; }
            set { end = value; }
        }
        #endregion PROPIEDADES
    }
}
