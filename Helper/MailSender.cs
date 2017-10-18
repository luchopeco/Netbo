using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class MailSender
    {
        #region"Send Mail"

        public static string SendMail(SmtpClient smtpClient,
                                    MailAddress from,
                                    MailAddress[] to,
                                    string subject,
                                    string body,
                                    bool isBodyHtml)
        {
            return SendMail(smtpClient, from, to, null, null, null, subject, body, isBodyHtml);
        }

        public static string SendMail(SmtpClient smtpClient,
                                    MailAddress from,
                                    MailAddress[] to,
                                    MailAddress[] cc,
                                    string subject,
                                    string body,
                                    bool isBodyHtml)
        {
            return SendMail(smtpClient, from, to, cc, null, null, subject, body, isBodyHtml);
        }

        public static string SendMail(SmtpClient smtpClient,
                                    MailAddress from,
                                    MailAddress[] to,
                                    MailAddress[] cc,
                                    MailAddress[] cco,
                                    string subject,
                                    string body,
                                    bool isBodyHtml)
        {
            return SendMail(smtpClient, from, to, cc, cco, null, subject, body, isBodyHtml);
        }

        public static string SendMail(SmtpClient smtpClient,
                                    MailAddress from,
                                    MailAddress[] to,
                                    Attachment[] toAttach,
                                    string subject,
                                    string body,
                                    bool isBodyHtml)
        {
            return SendMail(smtpClient, from, to, null, null, toAttach, subject, body, isBodyHtml);
        }

        public static string SendMail(SmtpClient smtpClient,
                                    MailAddress from,
                                    MailAddress[] to,
                                    MailAddress[] cc,
                                    Attachment[] toAttach,
                                    string subject,
                                    string body,
                                    bool isBodyHtml)
        {
            return SendMail(smtpClient, from, to, cc, null, toAttach, subject, body, isBodyHtml);
        }

        public static string SendMail(SmtpClient smtpClient,
                                    MailAddress from,
                                    MailAddress[] to,
                                    MailAddress[] cc,
                                    MailAddress[] cco,
                                    Attachment[] toAttach,
                                    string subject,
                                    string body,
                                    bool isBodyHtml)
        {
            string error = string.Empty;
            using (MailMessage mail = new MailMessage())
            {
                try
                {
                    if (to == null || (to != null && to.Length == 0)) throw new NullReferenceException("Al menos debe existir un destinatario para el envío de email");

                    mail.From = from;
                    to.ToList().ForEach(x => mail.To.Add(x));
                    if (cc != null && cc.Length > 0) cc.ToList().ForEach(x => mail.CC.Add(x));
                    if (cco != null && cco.Length > 0) cco.ToList().ForEach(x => mail.Bcc.Add(x));
                    if (toAttach != null && toAttach.Length > 0) toAttach.ToList().ForEach(x => mail.Attachments.Add(x));
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = isBodyHtml;
                    smtpClient.Send(mail);
                    smtpClient.Dispose();
                }
                catch (NullReferenceException nullEx)
                {
                    error = string.Format("{0}", nullEx);
                }
                catch (Exception ex)
                {
                    error = string.Format("{0}", ex);
                }
            }
            return error;
        }
        #endregion

        #region "statics methods"
        public static SmtpClient GetSmtpClient()
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Timeout = SmtpClientSettings.Settings.Timeout;
            smtpClient.DeliveryMethod = SmtpClientSettings.Settings.DeliveryMethod;
            smtpClient.EnableSsl = SmtpClientSettings.Settings.EnableSsl;
            smtpClient.Credentials = new NetworkCredential(SmtpClientSettings.Settings.EmailFrom, SmtpClientSettings.Settings.Password);
            smtpClient.Host = SmtpClientSettings.Settings.SmtpAddress;
            smtpClient.Port = SmtpClientSettings.Settings.PortNumber;
            return smtpClient;
        }

        public static MailAddress GetFrom(string display="No-Reply")
        {
            return new MailAddress(SmtpClientSettings.Settings.EmailFrom, display);
        }

        public static MailAddress[] GetAddresses(AddresseType type)
        {
            switch (type)
            {
                case AddresseType.To:
                    if (SmtpClientSettings.AddressesTo != null) return SmtpClientSettings.AddressesTo.Addressees.Cast<Addressee>().Select(x => new MailAddress(x.Email)).ToArray();
                    break;
                case AddresseType.Cc:
                    if (SmtpClientSettings.AddressesCc != null) return SmtpClientSettings.AddressesCc.Addressees.Cast<Addressee>().Select(x => new MailAddress(x.Email)).ToArray();
                    break;
                case AddresseType.Cco:
                    if (SmtpClientSettings.AddressesCco != null) return SmtpClientSettings.AddressesCco.Addressees.Cast<Addressee>().Select(x => new MailAddress(x.Email)).ToArray();
                    break;
            }
            
            return null;
        }
        #endregion

        #region "enum"
        public enum AddresseType
        {
            To,
            Cc,
            Cco
        }
        #endregion
    }

    #region "Settings"
    public class SmtpClientSettings : ConfigurationSection
    {
        private static SmtpClientSettings smptClientsettings = ConfigurationManager.GetSection("SmtpClientSettings") as SmtpClientSettings;
        private static AddresseeCollectionConfig addressesTo = ConfigurationManager.GetSection("AddresseeCollectionTo") as AddresseeCollectionConfig;
        private static AddresseeCollectionConfig addressesCc = ConfigurationManager.GetSection("AddresseeCollectionCc") as AddresseeCollectionConfig;
        private static AddresseeCollectionConfig addressesCco = ConfigurationManager.GetSection("AddresseeCollectionCco") as AddresseeCollectionConfig;

        public static SmtpClientSettings Settings
        {
            get
            {
                return smptClientsettings;
            }
        }

        public static AddresseeCollectionConfig AddressesTo
        {
            get
            {
                return addressesTo;
            }
        }

        public static AddresseeCollectionConfig AddressesCc
        {
            get
            {
                return addressesCc;
            }
        }

        public static AddresseeCollectionConfig AddressesCco
        {
            get
            {
                return addressesCco;
            }
        }

        [ConfigurationProperty("Timeout", DefaultValue = 100000000, IsRequired = false)]
        [IntegerValidator(MinValue = 100000000, MaxValue = 300000000)]
        public int Timeout
        {
            get { return (int)this["Timeout"]; }
            set { this["Timeout"] = value; }
        }


        [ConfigurationProperty("DeliveryMethod", IsRequired = false)]
        public SmtpDeliveryMethod DeliveryMethod
        {
            get
            {
                SmtpDeliveryMethod defaultValue = SmtpDeliveryMethod.Network;
                Enum.TryParse<SmtpDeliveryMethod>(this["DeliveryMethod"] as string, out defaultValue);
                return defaultValue;
            }
            set { this["DeliveryMethod"] = value; }
        }

        [ConfigurationProperty("EnableSsl", DefaultValue = false, IsRequired = false)]
        public bool EnableSsl
        {
            get { return (bool)this["EnableSsl"]; }
            set { this["EnableSsl"] = value; }
        }

        [ConfigurationProperty("smtpAddress", DefaultValue = "mail.sancristobalcaja.com.ar", IsRequired = false)]
        public string SmtpAddress
        {
            get { return this["smtpAddress"] as string; }
            set { this["smtpAddress"] = value; }
        }

        [ConfigurationProperty("portNumber", DefaultValue = 25, IsRequired = false)]
        public int PortNumber
        {
            get { return (int)this["portNumber"]; }
            set { this["portNumber"] = value; }
        }

        [ConfigurationProperty("password", DefaultValue = "nelnuevoxRAN", IsRequired = false)]
        public string Password
        {
            get { return this["password"] as string; }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("emailFrom", DefaultValue = "procesos-sbi@sancristobalcaja.com.ar", IsRequired = false)]
        public string EmailFrom
        {
            get { return this["emailFrom"] as string; }
            set { this["emailFrom"] = value; }
        }
    }

    public class Addressee : ConfigurationElement
    {
        [ConfigurationProperty("Email")]
        public string Email
        {
            get
            {
                return this["Email"] as string;
            }
            set
            {
                this["Email"] = value;
            }
        }
    }

    public class AddresseeCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Addressee();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Addressee)element).Email;
        }

        protected override string ElementName
        {
            get
            {
                return "Addressee";
            }
        }

        protected override bool IsElementName(string elementName)
        {
            return !String.IsNullOrEmpty(elementName) && elementName == "Addressee";
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        public new Addressee this[string key]
        {
            get
            {
                return base.BaseGet(key) as Addressee;
            }
        }
    }

    public class AddresseeCollectionConfig : ConfigurationSection
    {
        [ConfigurationProperty("AddresseeCollection", IsDefaultCollection = true, IsKey = false, IsRequired = false)]
        public AddresseeCollection Addressees
        {
            get
            {
                return base["AddresseeCollection"] as AddresseeCollection;
            }

            set
            {
                base["AddresseeCollection"] = value;
            }
        }
    }
    #endregion
}
