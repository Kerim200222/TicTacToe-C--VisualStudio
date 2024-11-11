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
    public partial class SoloChallange : Form
    {
        bool X = true;
        enum GameType { X, O };
        GameType curret_gt = GameType.X;
        int playerGames = 0;
        int nMyScore = 0;
        int nReturnScore = 0;
        Random rand = new Random();

        Button[,] btns = new Button[3, 3];

        public SoloChallange()
        {
            InitializeComponent();
            pictureBox1.Visible = false;
            picBaemUp.Visible = false;
            picBeamDown.Visible = false;
            btns[0, 0] = btn00; btns[0, 1] = btn01; btns[0, 2] = btn02;
            btns[1, 0] = btn10; btns[1, 1] = btn11; btns[1, 2] = btn12;
            btns[2, 0] = btn20; btns[2, 1] = btn21; btns[2, 2] = btn22;
            foreach (Button btn in btns)
                btn.Click += new EventHandler(btn_Click);
        }

        #region Game Logic Methods (Oyun Mantığı Yöntemleri)

        // Bu metot, butona tıklanma olayını işleyerek oyunun akışını kontrol eder.
        async void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Text != "")
                return;
            playerGames++;
            btn.Text = curret_gt.ToString();
            btn.Image = Properties.Resources.X;  // X işaretini ekler.
            if (IsWin()) // Kazanma durumu kontrol edilir.
            {
                UpdateScore();
                IsBeam();
                await Restarte();
                return;
            }

            if (playerGames == 9) // Tüm kareler dolduysa yeniden başlat.
            {
                await Restarte();
            }
            SwitchGame();

            if (curret_gt == GameType.O)
                ComputerMove(); // Bilgisayar hamlesini yap.
        }

        // Bu metot, her butonun üzerine resim ekler.
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

        // Bu metot, oyunu sıfırlar (başlangıç durumuna döner).
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

        // Bu metot, bilgisayarın hamlesini yapar.
        void ComputerMove()
        {
            List<Button> availableButtons = new List<Button>();

            foreach (Button btn in btns)
            {
                if (btn.Text == "")
                    availableButtons.Add(btn);
            }

            if (availableButtons.Count > 0)
            {
                Button btn = availableButtons[rand.Next(availableButtons.Count)];
                btn.Text = GameType.O.ToString();
                btn.Image = Properties.Resources.O;
            }

            if (IsWin())
            {
                UpdateScore();
                Restarte();
            }
            else
            {
                playerGames++;
                SwitchGame();
            }
        }

        // Kazanma durumu kontrolü
        bool IsWin()
        {
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

        // Oyuncular arasında geçiş yapar.
        void SwitchGame()
        {
            if (curret_gt == GameType.X)
                curret_gt = GameType.O;
            else
                curret_gt = GameType.X;
        }

        // Skorları günceller ve gösterir.
        void UpdateScore()
        {
            if (curret_gt == GameType.X)
                nMyScore++;
            else if (curret_gt == GameType.O)
                nReturnScore++;
            if (nMyScore == 3)
            {
                panel2.Visible = false;
                pictureBox1.Image = Properties.Resources.WinGIF_;
                pictureBox1.Visible = true;
                pictureBox1.Location = new Point(350, 190);
                nMyScore = 0;
                nReturnScore = 0;
            }
            else if (nReturnScore == 3)
            {
                panel2.Visible = false;
                pictureBox1.Image = Properties.Resources.GameOverGIF1;
                pictureBox1.Visible = true;
                pictureBox1.Location = new Point(350, 190);
                nMyScore = 0;
                nReturnScore = 0;
            }

            lblMyScore.Text = nMyScore.ToString();
            lblReturn.Text = nReturnScore.ToString();
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
                await ShowBeam(picBaemUp, new Point(13, 55));
            }
            else if (btns[0, 1].Text == curret_gt.ToString() &&
                     btns[0, 1].Text == btns[1, 1].Text &&
                     btns[1, 1].Text == btns[2, 1].Text)
            {
                await ShowBeam(picBaemUp, new Point(150, 10));
            }
            else if (btns[0, 2].Text == curret_gt.ToString() &&
                     btns[0, 2].Text == btns[1, 2].Text &&
                     btns[1, 2].Text == btns[2, 2].Text)
            {
                await ShowBeam(picBaemUp, new Point(255, 15));
            }
            // Horizontal beams
            else if (btns[0, 0].Text == curret_gt.ToString() &&
                     btns[0, 0].Text == btns[0, 1].Text &&
                     btns[0, 1].Text == btns[0, 2].Text)
            {
                await ShowBeam(picBeamDown, new Point(12, 35));
            }
            else if (btns[1, 0].Text == curret_gt.ToString() &&
                     btns[1, 0].Text == btns[1, 1].Text &&
                     btns[1, 1].Text == btns[1, 2].Text)
            {
                await ShowBeam(picBeamDown, new Point(22, 115));
            }
            else if (btns[2, 0].Text == curret_gt.ToString() &&
                     btns[2, 0].Text == btns[2, 1].Text &&
                     btns[2, 1].Text == btns[2, 2].Text)
            {
                await ShowBeam(picBeamDown, new Point(22, 200));
            }
        }

        #endregion

        #region GUI Event Methods (GUI Olay Yöntemleri)

        // Menüye dönme butonu tıklanırsa bu metot çalışır
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();

        }

        // Skorları sıfırlama butonu tıklanırsa bu metot çalışır.
        private void button1_Click(object sender, EventArgs e)
        {
            Restarte();
            pictureBox1.Visible = false;
            panel2.Visible = true;
            lblMyScore.Text = "0";
            lblReturn.Text = "0";
            nReturnScore = 0;
            nMyScore = 0;
        }

        private void SoloChallange_Load(object sender, EventArgs e)
        {

        }

        // Buton tıklama olaylarını işleyen metotlar
        private void button3_Click(object sender, EventArgs e) { ImageCheck(btn02); }
        private void button5_Click(object sender, EventArgs e) { ImageCheck(btn01); }
        private void button10_Click(object sender, EventArgs e) { ImageCheck(btn00); }
        private void button7_Click(object sender, EventArgs e) { ImageCheck(btn12); }
        private void button4_Click(object sender, EventArgs e) { ImageCheck(btn11); }
        private void button9_Click(object sender, EventArgs e) { ImageCheck(btn10); }
        private void button8_Click(object sender, EventArgs e) { ImageCheck(btn22); }
        private void button6_Click(object sender, EventArgs e) { ImageCheck(btn21); }
        private void button11_Click(object sender, EventArgs e) { ImageCheck(btn20); }
        #endregion
    }
}
