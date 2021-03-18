using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestConsole
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
            
        }

        private void fillComponents()
        {
            throw new NotImplementedException();
        }

        private void cmbDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("It Changed");
        }
    }
}
