using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class Helper
    {

        public enum ModosActivacionTarjeta
        {
            UsuarioInterno = 1,
            UsuarioExterno=2
        }
        
       
     

        public enum TiposDocumentos
        {
            LC = 1,
            DNIM = 2,
            DNIF = 3,
            CED = 4,
            PASS = 5,
            CUIT = 10,
            LE = 11,
            IND = 99,
            CUIL = 100
        }

        public enum Empresas
        {
            WebApiComunidad=1,
            SCSF=2
            
        }

        public int getEdad(DateTime fechaNacimiento)
        {
            int edad = DateTime.Today.AddTicks(-fechaNacimiento.Ticks).Year - 1;
            return edad;
        }

        /// <summary>
        /// Redondea un numero
        /// </summary>
        /// <param name="valorARedondear"></param>
        /// <param name="posicionRedondeo">2:decena|3:centena|4:miles</param>
        /// <returns></returns>
        public static int Redondear(int valorARedondear, int posicionRedondeo)
        {
            int retorno = 0;
            double min = Math.Pow(10, posicionRedondeo - 1);
            retorno = Convert.ToInt32(Math.Ceiling((Convert.ToDouble(valorARedondear) / min)) * min);
            return retorno;
        }

        /// <summary>
        /// Valida Si el cuit tiene el formato correcto
        /// </summary>
        /// <param name="cuit"></param>
        /// <returns></returns>
        public static bool ValidarCUIT(decimal cuit)
        {
            int mk_suma = 0;
            bool mk_valido = true;

            if (cuit.ToString().Length != 11)
            {
                return false;
            }
            else
            {
                mk_suma = 0;
                mk_suma += Convert.ToInt32(cuit.ToString().Substring(0, 1)) * 5;
                mk_suma += Convert.ToInt32(cuit.ToString().Substring(1, 1)) * 4;
                mk_suma += Convert.ToInt32(cuit.ToString().Substring(2, 1)) * 3;
                mk_suma += Convert.ToInt32(cuit.ToString().Substring(3, 1)) * 2;
                mk_suma += Convert.ToInt32(cuit.ToString().Substring(4, 1)) * 7;
                mk_suma += Convert.ToInt32(cuit.ToString().Substring(5, 1)) * 6;
                mk_suma += Convert.ToInt32(cuit.ToString().Substring(6, 1)) * 5;
                mk_suma += Convert.ToInt32(cuit.ToString().Substring(7, 1)) * 4;
                mk_suma += Convert.ToInt32(cuit.ToString().Substring(8, 1)) * 3;
                mk_suma += Convert.ToInt32(cuit.ToString().Substring(9, 1)) * 2;
                mk_suma += Convert.ToInt32(cuit.ToString().Substring(10, 1)) * 1;
            }

            double redondeo = Math.Round((double)((double)mk_suma / 11), 0);
            double division = (double)((double)mk_suma / 11);
            if (redondeo == division)
            {
                mk_valido = true;
            }
            else
            {
                mk_valido = false;
            }

            return mk_valido;
        }

        /// <summary>
        /// Devuelve en letras un numero
        /// </summary>
        /// <param name="num">Numero</param>
        /// <returns></returns>
        public static string NumerosEnLetras(string num)

        {

            string res, dec = "";

            int entero;

            int decimales;

            double nro;

            try

            {

                nro = Convert.ToDouble(num);

            }

            catch

            {

                return "";

            }

            entero = Convert.ToInt32(Math.Truncate(nro));

            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));

            if (decimales > 0)

            {

                dec = " CON " + decimales.ToString() + "/ 100";

            }

            res = toText(Convert.ToDouble(entero)) + dec;

            return res.ToLowerInvariant();

        }

        private static string toText(double value)

        {

            string Num2Text = "";

            value = Math.Truncate(value);

            if (value == 0) Num2Text = "CERO";

            else if (value == 1) Num2Text = "UNO";

            else if (value == 2) Num2Text = "DOS";

            else if (value == 3) Num2Text = "TRES";

            else if (value == 4) Num2Text = "CUATRO";

            else if (value == 5) Num2Text = "CINCO";

            else if (value == 6) Num2Text = "SEIS";

            else if (value == 7) Num2Text = "SIETE";

            else if (value == 8) Num2Text = "OCHO";

            else if (value == 9) Num2Text = "NUEVE";

            else if (value == 10) Num2Text = "DIEZ";

            else if (value == 11) Num2Text = "ONCE";

            else if (value == 12) Num2Text = "DOCE";

            else if (value == 13) Num2Text = "TRECE";

            else if (value == 14) Num2Text = "CATORCE";

            else if (value == 15) Num2Text = "QUINCE";

            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);

            else if (value == 20) Num2Text = "VEINTE";

            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);

            else if (value == 30) Num2Text = "TREINTA";

            else if (value == 40) Num2Text = "CUARENTA";

            else if (value == 50) Num2Text = "CINCUENTA";

            else if (value == 60) Num2Text = "SESENTA";

            else if (value == 70) Num2Text = "SETENTA";

            else if (value == 80) Num2Text = "OCHENTA";

            else if (value == 90) Num2Text = "NOVENTA";

            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);

            else if (value == 100) Num2Text = "CIEN";

            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);

            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";

            else if (value == 500) Num2Text = "QUINIENTOS";

            else if (value == 700) Num2Text = "SETECIENTOS";

            else if (value == 900) Num2Text = "NOVECIENTOS";

            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);

            else if (value == 1000) Num2Text = "MIL";

            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);

            else if (value < 1000000)

            {

                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";

                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);

            }

            else if (value == 1000000) Num2Text = "UN MILLON";

            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);

            else if (value < 1000000000000)

            {

                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";

                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);

            }

            else if (value == 1000000000000) Num2Text = "UN BILLON";

            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else

            {

                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";

                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            }

            return Num2Text;

        }

        public static string GetNombreMes(int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return dtinfo.GetMonthName(month);
        }


        public static bool EnviarMail(string destinatarios, string asunto, string body)
        {
            SmtpClient SmtpServer = new SmtpClient();
            SmtpServer.Credentials = new System.Net.NetworkCredential("contacto@palomacruzshop.com.ar", "Paloma2141");
            SmtpServer.Port = 25;
            SmtpServer.Host = "mail.palomacruzshop.com.ar";
            SmtpServer.EnableSsl = false;
            MailMessage mail = new MailMessage();
            String[] addr = "luchopeco@gmail.com".Split(',');
            foreach (var dest in addr)
            {
                mail.To.Add(dest);
            }
            mail.From = new MailAddress("contacto@palomacruzshop.com.ar", "Luciano Pecovich", System.Text.Encoding.UTF8);
            mail.Subject = "Hola";
            mail.Body = "Correcto";
            SmtpServer.Send(mail);
            return true;

        }

        /// <summary>
        /// Devuelve al fecha recibida para utilizar como fecha desde, le pone la hora 00:00:00
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static DateTime FechaHoraDesde(DateTime fechaHora)
        {
            return new DateTime(fechaHora.Year, fechaHora.Month, fechaHora.Day, 0, 0, 0);
        }

        /// <summary>
        /// Devuelve al fecha recibida para utilizar como fecha Hasta, le pone la hora 23:59:59
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static DateTime FechaHoraHasta(DateTime fechaHora)
        {            
            return new DateTime(fechaHora.Year, fechaHora.Month, fechaHora.Day, 23, 59, 59);
        }

        /// <summary>
        /// Devuelve  mas de un minuto, mas de un mes, mas de 1 ano, etc
        /// </summary>
        /// <param name="fechaACalcular"></param>
        /// <returns></returns>
        public static string FechaAmigable(DateTime fechaACalcular)
        {
            string fecha = string.Empty;
            DateTime fechaActual = DateTime.Now;
            TimeSpan dif = fechaActual - fechaACalcular;
            if (dif.Days > 365)
            {
                decimal ano = dif.Days / 365;
                return Math.Truncate(ano).ToString() + " años";

            }
            else if (dif.Days > 30)
            {
                decimal mes = dif.Days / 30;
                return Math.Truncate(mes).ToString() + " mes/meses";
            }
            else if (dif.Days >= 1)
            {
                int dias = dif.Days;
                return dias.ToString() + " días";
            }
            else if (dif.Hours >= 1)
            {
                return dif.Hours + " horas";
            }
            else
            {
                return dif.Minutes + " minutos";
            }

        }
    }
}
