using Outlook = Microsoft.Office.Interop.Outlook;
namespace SentOnBehalfBCC
{
    public static class Extension
    {
        /// <summary>See: https://docs.microsoft.com/de-de/office/vba/outlook/concepts/address-book/obtain-the-e-mail-address-of-a-recipient</summary>
        private const string PR_SMTP_ADDRESS = "http://schemas.microsoft.com/mapi/proptag/0x39FE001E";
        public static string GetSmtpAddress(this Outlook.Recipient r)
        {
            return r.PropertyAccessor.GetProperty(PR_SMTP_ADDRESS);
        }
        public static string GetSmtpAddress(this Outlook.AddressEntry a)
        {
            return a.PropertyAccessor.GetProperty(PR_SMTP_ADDRESS);
        }
        public static bool Contains(this Outlook.Recipients recipients, string smtpAddress)
        {
            foreach (Outlook.Recipient r in recipients)
            {
                if (r.GetSmtpAddress().Equals(smtpAddress))
                    return true;
            }
            return false;
        }
    }
}
