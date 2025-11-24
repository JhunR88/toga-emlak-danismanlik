using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TOGA_EMLAK_VE_DANIŞMANLIK
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 ucuncuForm = new Form3();
            ucuncuForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 dorduncuForm = new Form4();
            dorduncuForm.Show();
        }
    }
}
