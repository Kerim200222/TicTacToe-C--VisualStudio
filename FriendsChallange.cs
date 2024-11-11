using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace X_O_Game
{
    public partial class FriendsChallange : Form
    {
        bool X = true;
        enum GameType { X, O };
        GameType curret_gt = GameType.X;
        int playerGames = 0;
        int nMyScore = 0;
        int nReturnScore = 0;
        Random rand = new Random();

        Button[,] btns = new Button[3, 3];

        public FriendsChallange()
        {
            InitializeComponent();
            pictureBox1.Visible = false;
            picBaemUp2.Visible = false;
            picBeamDown2.Visible = false;
            // Butonları tanımlıyoruz
            btns[0, 0] = btn00; btns[0, 1] = btn01; btns[0, 2] = btn02;
            btns[1, 0] = btn10; btns[1, 1] = btn11; btns[1, 2] = btn12;
            btns[2, 0] = btn20; btns[2, 1] = btn21; btns[2, 2] = btn22;
            foreach (Button btn in btns)
                btn.Click += new EventHandler(btn_Click);
        }

        #region Game Logic Methods

        // Buton tıklama olayını kontrol ediyoruz
        async void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Text != "") // Eğer butonun üzerinde yazı varsa, işlem yapma
                return;
            playerGames++; // Oyuncu oyunu oynadı
            btn.Text = curret_gt.ToString(); // Oyun tipini butona yaz
            if (IsWin()) // Eğer kazandıysa
            {
                IsBeam();
                UpdateScore(); // Skoru güncelle
                await Restarte(); // Oyunu sıfırla
                return;
            }
            if (playerGames == 9) // Eğer tüm kareler dolduysa
            {
                await Restarte(); // Oyunu sıfırla
            }

            SwitchGame(); // Oyuncuyu değiştir
        }

        // Oyun bitip kazanıp kazanmadığını kontrol ediyoruz
        bool IsWin()
        {
            // Yatay ve dikey kontroller
            for (int i = 0; i <= 2; i++)
            {
                if (btns[0, i].Text == curret_gt.ToString() &&
                    btns[0, i].Text == btns[1, i].Text &&
                    btns[1, i].Text == btns[2, i].Text)
                    return true;
                if (btns[i, 0].Text == curret_gt.ToString() &&
                    btns[i, 0].Text == btns[i, 1].Text &&
                    btns[i, 1].Text == btns[i, 2].Text)
                    return true;
            }
            // Çapraz kontroller
            if (btns[0, 0].Text == curret_gt.ToString() &&
                    btns[0, 0].Text == btns[1, 1].Text &&
                    btns[1, 1].Text == btns[2, 2].Text)
                return true;
            if (btns[0, 2].Text == curret_gt.ToString() &&
                btns[0, 2].Text == btns[1, 1].Text &&
                btns[1, 1].Text == btns[2, 0].Text)
                return true;

            return false;
        }

        // Oyuncu sırasını değiştir
        void SwitchGame()
        {
            if (curret_gt == GameType.X)
                curret_gt = GameType.O;
            else
                curret_gt = GameType.X;
        }

        // Skoru güncelle
        void UpdateScore()
        {
            if (curret_gt == GameType.X)
                nMyScore++;
            else if (curret_gt == GameType.O)
                nReturnScore++;
            if (nMyScore == 3)
            {
                panel2.Visible = false;
                picBaemUp2.Visible = false;
                picBeamDown2.Visible = false;
                pictureBox1.Image = Properties.Resources.WinGIF_;
                pictureBox1.Visible = true;
                pictureBox1.Location = new Point(350, 190);
                label1.Location = new Point(420, 340);
                pictureBox3.Visible = false;
                label4.Visible = false;
                pictureBox4.Visible = false;
                nMyScore = 0;
                nReturnScore = 0;
            }
            else if (nReturnScore == 3)
            {
                panel2.Visible = false;
                picBaemUp2.Visible = false;
                picBeamDown2.Visible = false;
                pictureBox1.Image = Properties.Resources.WinGIF_;
                pictureBox1.Visible = true;
                pictureBox1.Location = new Point(350, 190);
                label4.Location = new Point(430, 340);
                pictureBox3.Visible = false;
                label1.Visible = false;
                pictureBox4.Visible = false;
                nMyScore = 0;
                nReturnScore = 0;
            }

            lblMyScore.Text = nMyScore.ToString();
            lblFriend.Text = nReturnScore.ToString();
        }

        // Oyunu sıfırlama
        public async Task Restarte()
        {
            await Task.Delay(1000);
            foreach (Button btn in btns)
            {
                btn.Image = null;
                btn.Text = "";
            }
            playerGames = 0;
            X = true;
            curret_gt = GameType.X;
        }

        // Butonun görüntüsünü kontrol eder ve "X" veya "O" olarak değiştirir
        void ImageCheck(Button btn)
        {
            if (btn.Image == null)
            {
                if (X)
                {
                    btn.Image = Properties.Resources.X;
                    X = false;
                }
                else
                {
                    btn.Image = Properties.Resources.O;
                    X = true;
                }
            }
        }

        public async Task IsBeam()
        {
            async Task ShowBeam(PictureBox beam, Point location)
            {
                beam.Visible = true;
                beam.Location = location;
                await Task.Delay(1000);
                beam.Visible = false;
            }

            // Vertical beams
            if (btns[0, 0].Text == curret_gt.ToString() &&
                btns[0, 0].Text == btns[1, 0].Text &&
                btns[1, 0].Text == btns[2, 0].Text)
            {
                await ShowBeam(picBaemUp2, new Point(325, 190));
            }
            else if (btns[0, 1].Text == curret_gt.ToString() &&
                     btns[0, 1].Text == btns[1, 1].Text &&
                     btns[1, 1].Text == btns[2, 1].Text)
            {
                await ShowBeam(picBaemUp2, new Point(420, 190));
            }
            else if (btns[0, 2].Text == curret_gt.ToString() &&
                     btns[0, 2].Text == btns[1, 2].Text &&
                     btns[1, 2].Text == btns[2, 2].Text)
            {
                await ShowBeam(picBaemUp2, new Point(515, 190));
            }
            // Horizontal beams
            else if (btns[0, 0].Text == curret_gt.ToString() &&
                     btns[0, 0].Text == btns[0, 1].Text &&
                     btns[0, 1].Text == btns[0, 2].Text)
            {
                await ShowBeam(picBeamDown2, new Point(290, 215));
            }
            else if (btns[1, 0].Text == curret_gt.ToString() &&
                     btns[1, 0].Text == btns[1, 1].Text &&
                     btns[1, 1].Text == btns[1, 2].Text)
            {
                await ShowBeam(picBeamDown2, new Point(290, 295));
            }
            else if (btns[2, 0].Text == curret_gt.ToString() &&
                     btns[2, 0].Text == btns[2, 1].Text &&
                     btns[2, 1].Text == btns[2, 2].Text)
            {
                await ShowBeam(picBeamDown2, new Point(290, 380));
            }
        }

        #endregion

        #region GUI Evect

        // Formu değiştirme
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
        }

        // Oyunu başlatma butonuna tıklandığında
        private void button1_Click(object sender, EventArgs e)
        {
            Restarte();
            pictureBox1.Visible = false;
            panel2.Visible = true;
            lblMyScore.Text = "0";
            lblFriend.Text = "0";
            nReturnScore = 0;
            nMyScore = 0;
        }

        private void FriendsChallange_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        // Buton tıklamaları için gerekli eventler
        private void btn00_Click(object sender, EventArgs e) { ImageCheck(btn00); }
        private void btn01_Click(object sender, EventArgs e) { ImageCheck(btn01); }
        private void btn02_Click(object sender, EventArgs e) { ImageCheck(btn02); }
        private void btn10_Click(object sender, EventArgs e) { ImageCheck(btn10); }
        private void btn11_Click(object sender, EventArgs e) { ImageCheck(btn11); }
        private void btn12_Click(object sender, EventArgs e) { ImageCheck(btn12); }
        private void btn20_Click(object sender, EventArgs e) { ImageCheck(btn20); }
        private void btn21_Click(object sender, EventArgs e) { ImageCheck(btn21); }
        private void btn22_Click(object sender, EventArgs e) { ImageCheck(btn22); }
        #endregion

    }
}
