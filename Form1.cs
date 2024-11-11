using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace X_O_Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
             Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            SoloChallange soloChallange = new SoloChallange();
            soloChallange.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            FriendsChallange friendsChallange = new FriendsChallange();
            friendsChallange.Show();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
