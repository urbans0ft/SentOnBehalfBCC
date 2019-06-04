using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Data;
using Microsoft.Office.Tools.Ribbon;

namespace SentOnBehalfBCC
{
    public partial class SentOnBehalfBCCRibbon
    {
        private void MyRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            this.EnableToggle.Image =
                Properties.Settings.Default.AddInEnabled ?
                    Properties.Resources.toggle_on_solid :
                    Properties.Resources.toggle_off_solid;
        }

        private void EnableToggle_Click(object sender, RibbonControlEventArgs e)
        {
            if (this.EnableToggle.Checked)
            {
                this.EnableToggle.Image = Properties.Resources.toggle_on_solid;
                this.MailText.Enabled = true;
            }
            else
            {
                this.EnableToggle.Image = Properties.Resources.toggle_off_solid;
                this.MailText.Enabled = false;
            }
            Properties.Settings.Default.AddInEnabled = this.EnableToggle.Checked;
        }

        private void MailText_TextChanged(object sender, RibbonControlEventArgs e)
        {
            Properties.Settings.Default.MailText = (sender as RibbonEditBox)?.Text;
        }
    }
}
