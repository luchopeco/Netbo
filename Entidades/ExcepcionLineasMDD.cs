using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ExcepcionLineasMDD : Exception
    {
        #region CONSTRUCTORES
        public ExcepcionLineasMDD()
        {
            // = "Error...";
        }
        public ExcepcionLineasMDD(string message): base(message)
        {
            End = false;
        }
        public ExcepcionLineasMDD(string message, Exception inner): base(message, inner)
        {
            End = false;
        }
        public ExcepcionLineasMDD(string message, bool termina): base(message)           
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
