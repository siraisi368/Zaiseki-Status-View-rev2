using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 在席ソフトRev2
{
    public partial class SubWindow : Form
    {
        public SubWindow()
        {
            InitializeComponent();
        }

        public void SetImage(Image image)
        {
            pictureBox1.Image = image;
        }

        private void SubWindow_Load(object sender, EventArgs e)
        {
            this.Location = Properties.Settings.Default.subwindow;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
        }

        private void SubWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.subwindow = this.Location;
        }
    }
}
