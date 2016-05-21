using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Of_Life
{
    public partial class Size_Change : Form
    {
        private int x;
        private int y;

        public Size_Change()
        {
            InitializeComponent();
        }

        public int X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                X = Convert.ToInt32(textBox1.Text);
                Y = Convert.ToInt32(textBox2.Text);

            }
            catch (FormatException)
            {
                MessageBox.Show("ERROR", "DAT AINT NO NUMBAH", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            
        }
       
    }
}
