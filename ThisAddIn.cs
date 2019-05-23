using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.Diagnostics;

namespace SentOnBehalfBCC
{
    public partial class ThisAddIn
    {
        private Properties.Settings   config = Properties.Settings.Default;
        private Outlook.Inspectors    inspectors;
        private Outlook.Explorers     explorers;
        private Outlook.Inspector     activeInspector;
        private Outlook.Explorer      activeExplorer;
        private Outlook.MailItem      mail;
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            
            // Get the Inspector object
            inspectors = this.Application.Inspectors;

            // Get the active Inspector object
            activeInspector = this.Application.ActiveInspector();
            if (activeInspector != null)
            {
                // Get the title of the active item when the Outlook start.
                Debug.WriteLine("Active inspector: {0}", activeInspector.Caption);
            }

            // Get the Explorer objects
            explorers = this.Application.Explorers;

            // Get the active Explorer object
            activeExplorer = this.Application.ActiveExplorer();
            if (activeExplorer != null)
            {
                // Get the title of the active folder when the Outlook start.
                Debug.WriteLine("Active explorer: {0}", activeExplorer.Caption);
            }

            config.PropertyChanged += Config_PropertyChanged;
            if (config.AddInEnabled)
                addComposingEventHandler();
        }

        private void Config_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "AddInEnabled":
                    changeComposingEventHandler();
                    break;
            }
        }

        private void changeComposingEventHandler()
        {
            if (config.AddInEnabled)
                addComposingEventHandler();
            else
                removeComposingEventHandler();
        }
        private void addComposingEventHandler()
        {
            activeExplorer.InlineResponse += ActiveExplorer_InlineResponse;
            inspectors.NewInspector += new Outlook.InspectorsEvents_NewInspectorEventHandler(Inspectors_NewInspector);
            explorers.NewExplorer += new Outlook.ExplorersEvents_NewExplorerEventHandler(Explorers_NewExplorer);
        }
        private void removeComposingEventHandler()
        {
            activeExplorer.InlineResponse -= ActiveExplorer_InlineResponse;
            inspectors.NewInspector -= Inspectors_NewInspector;
            explorers.NewExplorer -= Explorers_NewExplorer;
        }

        private void ActiveExplorer_InlineResponse(object item)
        {
            Outlook.MailItem mailItem = item as Outlook.MailItem;
            if (mailItem != null)
                newMail(ref mailItem);
        }
        private void Inspectors_NewInspector(Outlook.Inspector inspector)
        {
            Outlook.MailItem mailItem = inspector.CurrentItem as Outlook.MailItem;
            if (mailItem != null)
                newMail(ref mailItem);
        }
        private void MailItem_PropertyChange(string name)
        {
            Debug.WriteLine("##### {0}", name);
            switch (name)
            {
                case "SentOnBehalfOfName":
                    if (mail.Recipients.Contains(config.MailText))
                        return;
                    string smtp = mail.Sender?.GetSmtpAddress();
                    if (smtp != null && smtp.Equals(config.MailText, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Outlook.Recipient recipient = mail.Recipients.Add(config.MailText);
                        recipient.Type = (int)Outlook.OlMailRecipientType.olBCC;
                        mail.Recipients.ResolveAll();
                    }
                    break;
            }
        }
        private void newMail(ref Outlook.MailItem mail)
        {
            this.mail = mail;
            this.mail.PropertyChange += MailItem_PropertyChange;
            changeSendOnBehalf(ref mail);
        }

        private void changeSendOnBehalf(ref Outlook.MailItem mailItem)
        {
            Debug.WriteLine("changeSendOnBehalf");
            foreach (Outlook.Recipient recipient in mailItem.Recipients)
            {
                Debug.WriteLine(recipient.Address);
                string smtpAddress = recipient.GetSmtpAddress();
                Debug.WriteLine(smtpAddress);
                if (smtpAddress.Equals(config.MailText, StringComparison.InvariantCultureIgnoreCase))
                {
                    Debug.WriteLine("BCC and SentOnBehalf");
                    recipient.Type = (int)Outlook.OlMailRecipientType.olBCC;
                    mailItem.Sender = recipient.AddressEntry;
                    mailItem.Recipients.ResolveAll();
                    return;
                }
            }
        }
        private void Explorers_NewExplorer(Outlook.Explorer explorer)
        {
            System.Windows.Forms.MessageBox.Show("Explorers_NewExplorer");
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see https://go.microsoft.com/fwlink/?LinkId=506785
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
