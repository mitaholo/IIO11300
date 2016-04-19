using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace Tehtava12
{
    public partial class Form1 : Form
    {
        private int language;

        public Form1()
        {
            InitializeComponent();
            cbLanguage.SelectedIndex = 0;
            cbType.SelectedIndex = 0;
        }

        private void ChangeLanguage(string language)
        {
            foreach (Control ctrl in this.Controls)
            {
                ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
                resources.ApplyResources(ctrl, ctrl.Name, new CultureInfo(language));
            }
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCurrentType.Text = cbType.Text;
        }

        private void cbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbLanguage.SelectedIndex)
            {
                case 0:
                    ChangeLanguage("fi-FI");
                    break;
                case 1:
                    ChangeLanguage("en-GB");
                    break;
                default:
                    ChangeLanguage("fi-FI");
                    break;
            }
        }
    }
}
