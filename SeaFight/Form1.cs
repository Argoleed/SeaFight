using System;
using System.Windows.Media;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Shapes;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace SeaFight
{
    public partial class Form1 : Form
    {
        /************************************** Глобальные переменные **********************************/

        public struct MouseSquare
        {
            public int i;
            public int j;
            public void getPos(int x, int y)
            {
                i = (x - 1) / 52;
                if (i > 9) i = 9;
                if (i < 0) i = 0;
                j = (y - 1) / 52;
                if (j > 9) j = 9;
                if (j < 0) j = 0;
            }
        }

        static MediaPlayer MainPlayer = new MediaPlayer(),
                           SoundsPlayer = new MediaPlayer(),
                           MusicPlayer = new MediaPlayer();
        bool game_started = false;
        IniFile settings;
        Graphics mainImage;
        private System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Black);
        int mode = 0;
        Sea AISea = new Sea();
        Sea mySea = new Sea();
        Random R = new Random();
        MouseSquare prevSquare = new MouseSquare { i = 0, j = 0 };
        MouseSquare thisSquare;
        MouseSquare AISquare;
        Graphics AISeaGrid;
        Graphics mySeaGrid;
        bool loseNear_played = false;
        StreamWriter protocolWriter;
        string adder_str = "..\\..\\";
        bool round_started = false;
        /************************************************************************************************/


        /***************************************** Окна *************************************************/

        // ЗАПУСК ФОРМЫ
        public Form1()
        {
            Directory.CreateDirectory("C:\\SeaFightData");

            if (!File.Exists($"C:\\SeaFightData\\settings.ini"))
            {
                File.Create($"C:\\SeaFightData\\settings.ini").Close();
                settings = new IniFile("C:\\SeaFightData\\settings.ini");
                settings.Write("SoundVal", "100", "Sound");
                settings.Write("MusicVal", "100", "Sound");
            }
            else
            {
                settings = new IniFile("C:\\SeaFightData\\settings.ini");
            }

            InitializeComponent();
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            InitialMenu();

            
            
        }


        // ИНИЦИАЛИЗАЦИЯ КОМПОНЕНТОВ ПРИ ЗАПУСКЕ
        private async void InitialMenu()
        {
            MusicPlayer.Volume = (double)Int32.Parse(settings.Read("MusicVal", "Sound")) / 100;
            MainPlayer.Volume = (double)Int32.Parse(settings.Read("SoundVal", "Sound")) / 100;
            SoundsPlayer.Volume = MainPlayer.Volume;
            GameLoading();

            LabelBox.Parent = PictureBox;
            LabelBox.Location = new Point(
                (LabelBox.Parent.Size.Width - LabelBox.Width) / 2 + 60,
                LabelBox.Parent.Size.Height / 8
            );

            MainLabel.Parent = LabelBox;
            MainLabel.Location = new Point(
                (MainLabel.Parent.Size.Width - MainLabel.Width) / 2 - 60,
                MainLabel.Parent.Size.Height / 2 - 80
            );

            PlayButton.Parent = PictureBox;
            PlayButton.Location = new Point((
                this.Size.Width - PlayButton.Width) / 2,
                this.Size.Height / 2
            );

            SettingsButton.Parent = PictureBox;
            SettingsButton.Location = new Point(
                (this.Size.Width - SettingsButton.Width) / 2,
                PlayButton.Location.Y + 75
                );

            ExitButton.Parent = PictureBox;
            ExitButton.Location = new Point(
                (this.Size.Width - ExitButton.Width) / 2,
                SettingsButton.Location.Y + 75
            );

            DevLabel.Parent = PictureBox;
            DevLabel.Location = new Point(0, this.Size.Height + 20);

            MusicBar.Size = PlayButton.Size;
            MusicBar.Location = PlayButton.Location;
            MusicBar.Parent = PictureBox;
            MusicBar.Hide();

            SoundsBar.Size = SettingsButton.Size;
            SoundsBar.Parent = PictureBox;
            SoundsBar.Location = SettingsButton.Location;
            SoundsBar.Hide();

            BackButton.Parent = PictureBox;
            BackButton.Location = ExitButton.Location;
            BackButton.Hide();

            MusicValLabel.Parent = MusicBar;
            MusicValLabel.Location = new Point(MusicBar.Size.Width / 2 - 70, 20);
            MusicValLabel.Hide();

            SoundsValLabel.Parent = SoundsBar;
            SoundsValLabel.Location = new Point(SoundsBar.Size.Width / 2 - 60, 20);
            SoundsValLabel.Hide();
        }


        // ЗАПУСК ИГРЫ, ГЛАВНОЕ МЕНЮ
        private async void GameLoading()
        {
            ExitButton.Hide();
            SettingsButton.Hide();
            PlayButton.Hide();
            DevLabel.Hide();
            MainLabel.Hide();
            LabelBox.Hide();

            mainImage = Graphics.FromImage(PictureBox.Image);
            await Task.Delay(1000);
            PlayMain();
            PlayMenuMusic();

            double vol = MainPlayer.Volume;
            double mus_vol = MusicPlayer.Volume;
            MainPlayer.Volume = 0;

            for (int i = 0; i < 30; i++)
            {
                MainPlayer.Volume = (vol / 30) * i;
                MusicPlayer.Volume = (mus_vol / 30) * i;

                myPen.Color = System.Drawing.Color.FromArgb((0xFF * i / 100), myPen.Color);
                mainImage.FillRectangle(myPen.Brush, 0, 0, PictureBox.Width, PictureBox.Height);
                PictureBox.Invalidate();
                PictureBox.Update();
                await Task.Delay(20);
            }
            PictureBox.Image = Image.FromFile(adder_str + "Images/Main_Image.gif");


            DevLabel.Show();
            LabelBox.Show();
            MainLabel.Show();
            MoveButtonIn(PlayButton);
            await Task.Delay(100);
            MoveButtonIn(SettingsButton);
            await Task.Delay(100);
            MoveButtonIn(ExitButton);
        }


        // МЕНЮ ВЫБОРА РЕЖИМА ИГРЫ
        private async void InitialGameMenu()
        {
            PictureBox.Image = Image.FromFile(adder_str + "Images/Game_menu.bmp");
            PlayGameMenuMain();
            PlayGameMenuMusic();


            SettingsButton.Size = new Size(200, 50);
            SettingsButton.Location = new Point(PictureBox.Width - SettingsButton.Width - 20, 20);
            SettingsButton.Text = "Настройки"; SettingsButton.Font = new Font("Monotype Corsiva", 20);
            SettingsButton.Refresh();
            SettingsButton.TextAlign = ContentAlignment.MiddleCenter;
            SettingsButton.Enabled = true;
            SettingsButton.Show();

            difficult1Button.Parent = PictureBox;
            difficult2Button.Parent = PictureBox;
            difficult3Button.Parent = PictureBox;
            difficult1Button.Show();
            difficult2Button.Show();
            difficult3Button.Show();

            difficult1Button.Location = new Point((PictureBox.Width / 3 - difficult1Button.Width) / 2 + 300,
                                                  (PictureBox.Height - difficult1Button.Height) / 2);
            difficult2Button.Location = new Point((PictureBox.Width / 3 - difficult1Button.Width) / 2 + PictureBox.Width / 3,
                                                  (PictureBox.Height - difficult1Button.Height) / 2);
            difficult3Button.Location = new Point((PictureBox.Width / 3 - difficult1Button.Width) / 2 + PictureBox.Width / 3 * 2 - 300,
                                                  (PictureBox.Height - difficult1Button.Height) / 2);
        }



        // МЕНЮ БОЯ
        async private void Game()
        {
            difficult1Button.Enabled = false;
            difficult2Button.Enabled = false;
            difficult3Button.Enabled = false;
            MoveButtonOut(difficult3Button);
            SoundsDown();
            await Task.Delay(100);
            MoveButtonOut(difficult2Button);
            await Task.Delay(100);
            MoveButtonOut(difficult1Button);
            await Task.Delay(100);
            MoveButtonOut(SettingsButton);
            await Task.Delay(100);
            PlaySound(adder_str + "Sounds/Horn.mp3");
            await Task.Delay(1000);

            mainImage = Graphics.FromImage(PictureBox.Image);
            for (int i = 0; i < 30; i++)
            {
                myPen.Color = System.Drawing.Color.FromArgb((0xFF * i / 100), myPen.Color);
                mainImage.FillRectangle(myPen.Brush, 0, 0, PictureBox.Width, PictureBox.Height);
                PictureBox.Invalidate();
                PictureBox.Update();
                await Task.Delay(20);
            }
            await Task.Delay(3000);
            PictureBox.Image = Image.FromFile(adder_str + "Images/fight.png");
            PlayFightMusic();
            PlayFightMain();

            MenuButton.Hide();
            MenuButton.Parent = PictureBox;

            SettingsButton.Location = new Point(PictureBox.Width - SettingsButton.Width - 20, 20);
            SettingsButton.Enabled = true;
            SettingsButton.Show();

            panel1.Parent = PictureBox;
            panel2.Parent = PictureBox;
            panel1.Location = new Point((PictureBox.Width / 2 - panel1.Width) / 2,
                                        (PictureBox.Height - panel1.Height) / 2);
            panel2.Location = new Point((PictureBox.Width / 2 - panel2.Width) / 2 + PictureBox.Width / 2,
                                        (PictureBox.Height - panel2.Height) / 2);
            panel1.Show();
            panel2.Show();
            button1.Show();
            button2.Show();

            pictureBox1.Enabled = false;
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox2.Image = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            AISeaGrid = Graphics.FromImage(pictureBox1.Image);
            mySeaGrid = Graphics.FromImage(pictureBox2.Image);
            drawGrid();

            AISea.Generate(R);
            mySea.Generate(R);
            reShowMyShips();

            pictureBox1.Enabled = false;
        }

        /************************************************************************************************/


        /************************************* Функционал кнопок ****************************************/


        // НАСТРОЙКИ
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            MusicValLabel.Text = $"Музыка: {MusicBar.Value}%";
            SoundsValLabel.Text = $"Звуки: {SoundsBar.Value}%";
            SettingsButton.Hide();
            PlayButton.Hide();
            ExitButton.Hide();
            BackButton.Show();
            MusicBar.Show();
            MusicValLabel.Show();
            SoundsBar.Show();
            SoundsValLabel.Show();

            if (game_started && mode == 0)
            {
                ExitButton.Show();
                ExitButton.Enabled = true;
                ExitButton.Location = new Point(
                    (BackButton.Location.X),
                     BackButton.Location.Y + 75
                );
                difficult1Button.Hide();
                difficult2Button.Hide();
                difficult3Button.Hide();
            }
            else if (game_started)
            {
                panel1.Hide();
                panel2.Hide();
                MenuButton.Enabled = true;
                MenuButton.Location = new Point(
                    (BackButton.Location.X),
                     BackButton.Location.Y + 75
                );
                MenuButton.Show();
            }
        }


        // НАЗАД
        private void BackButton_Click(object sender, EventArgs e)
        {
            settings.Write("SoundVal", $"{(int)(SoundsPlayer.Volume * 100)}", "Sound");
            settings.Write("MusicVal", $"{(int)(MusicPlayer.Volume * 100)}", "Sound");
            BackButton.Hide();
            MusicBar.Hide();
            MusicValLabel.Hide();
            SoundsBar.Hide();
            SoundsValLabel.Hide();
            PlayButton.Show();
            SettingsButton.Show();
            ExitButton.Show();

            if (game_started && mode == 0)
            {
                ExitButton.Hide();
                difficult1Button.Show();
                difficult2Button.Show();
                difficult3Button.Show();
            }
            else if (game_started)
            {
                panel1.Show();
                panel2.Show();
                MenuButton.Hide();
                ExitButton.Hide();
            }
        }

        // ВЫХОД
        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ИГРАТЬ
        private async void PlayButton_Click(object sender, EventArgs e)
        {
            PlayButton.Enabled = false;
            ExitButton.Enabled = false;
            SettingsButton.Enabled = false;

            MoveButtonOut(PlayButton);
            SoundsDown();
            await Task.Delay(100);
            MoveButtonOut(SettingsButton);
            await Task.Delay(100);
            MoveButtonOut(ExitButton);
            await Task.Delay(100);
            PlaySound(adder_str + "Sounds/Start.mp3");
            await Task.Delay(1000);

            MainLabel.Hide();
            LabelBox.Hide();

            PictureBox.Image = Image.FromFile(adder_str + "Images/Loading.bmp");
            await Task.Delay(1000);
            mainImage = Graphics.FromImage(PictureBox.Image);
            for (int i = 0; i < 30; i++)
            {
                myPen.Color = System.Drawing.Color.FromArgb((0xFF * i / 100), myPen.Color);
                mainImage.FillRectangle(myPen.Brush, 0, 0, PictureBox.Width, PictureBox.Height);
                PictureBox.Invalidate();
                PictureBox.Update();
                await Task.Delay(20);
            }

            game_started = true;
            InitialGameMenu();
            
        }


        // ГОТОВ
        async private void button2_Click(object sender, EventArgs e)
        {
            button1.Hide();
            button2.Hide();
            PlaySound(adder_str+"Sounds/Maxim_start.m4a");
            stepLabel.Location = new Point((PictureBox.Width - stepLabel.Width) / 2, 50);
            stepLabel.Parent = PictureBox;
            stepLabel.Show();
            round_started = true;

            DateTime protocolDate = DateTime.Now;
            string dateString = protocolDate.ToString("yyyy-MM-dd HH.mm.ss");
            if (!File.Exists($"C:\\SeaFightData\\Protocol {dateString} game.txt"))
                File.Create($"C:\\SeaFightData\\Protocol {dateString} game.txt").Close();
            protocolWriter = new StreamWriter($"C:\\SeaFightData\\Protocol {dateString} game.txt", true, Encoding.ASCII);
            protocolWriter.WriteLine($"User Sea:\n{mySea.ToString()}\nAI Sea:\n{AISea.ToString()}\nSteps:\n");

            await Task.Delay(5000);
            pictureBox1.Enabled = true;
        }

        // РАССТАВИТЬ
        private void button1_Click(object sender, EventArgs e)
        {
            mySea.Generate(R);
            reShowMyShips();
        }

        
        // ЛЁГКИЙ РЕЖИМ
        async private void difficult1Button_Click(object sender, EventArgs e)
        {
            mode = 1;
            Game();
        }


        // СРЕДНИЙ РЕЖИМ
        async private void difficult2Button_Click(object sender, EventArgs e)
        {
            mode = 2;
            Game();
        }


        // ТЯЖЁЛЫЙ РЕЖИМ
        async private void difficult3Button_Click(object sender, EventArgs e)
        {
            mode = 3;
            Game();
        }


        // В МЕНЮ
        async private void MenuButton_Click(object sender, EventArgs e)
        {

            BackButton_Click(sender, e);
            SettingsButton.Hide();
            mode = 0;
            loseNear_played = false;
            stepLabel.Hide();
            panel1.Hide();
            panel2.Hide();
            MusicPlayer.Stop();
            MainPlayer.Stop();

            PictureBox.Image = Image.FromFile(adder_str+"Images/Loading.bmp");
            await Task.Delay(1000);
            mainImage = Graphics.FromImage(PictureBox.Image);

            if (round_started)
            {
                round_started = false;
                protocolWriter.Write("\nGame stopped");
                protocolWriter.Close();
            }

            myPen.Color = System.Drawing.Color.Black;
            for (int i = 0; i < 30; i++)
            {
                myPen.Color = System.Drawing.Color.FromArgb((0xFF * i / 100), myPen.Color);
                mainImage.FillRectangle(myPen.Brush, 0, 0, PictureBox.Width, PictureBox.Height);
                PictureBox.Invalidate();
                PictureBox.Update();
                await Task.Delay(20);
            }
            InitialGameMenu();

            difficult1Button.Enabled = true;
            difficult2Button.Enabled = true;
            difficult3Button.Enabled = true;
        }

        /**************************************************************************************************/


        /******************************************** Звуки ***********************************************/

        // ПРОИГРЫВАНИЕ ЗВУКА
        private void PlaySound(string path)
        {
            FileInfo f = new FileInfo(path);
            string fullname = f.FullName;
            SoundsPlayer.Open(new Uri(fullname));
            SoundsPlayer.Play();
        }


        // ЧАЙКИ И ВОЛНЫ (ГЛАВНОЕ МЕНЮ)
        private async void PlayMain()
        {
            FileInfo f = new FileInfo(adder_str+"Sounds/Main_Sound.mp3");
            string fullname = f.FullName;
            SoundsBar.Value = (int)(SoundsPlayer.Volume * 100);
            while (!game_started)
            {
                MainPlayer.Open(new Uri(fullname));
                MainPlayer.Play();
                await Task.Delay(1000 * 93);
            }
        }


        // МУЗЫКА ГЛАВНОГО МЕНЮ
        private async void PlayMenuMusic()
        {
            FileInfo f = new FileInfo(adder_str+"Sounds/Menu_Music.mp3");
            string fullname = f.FullName;
            MusicBar.Value = (int)(MusicPlayer.Volume * 100);
            while (!game_started)
            {
                MusicPlayer.Open(new Uri(fullname));
                MusicPlayer.Play();
                await Task.Delay(1000 * 338);
            }
        }


        // ТРЕСК ДОСОК (МЕНЮ ВЫБОРА РЕЖИМА)
        private async void PlayGameMenuMain()
        {
            FileInfo f = new FileInfo(adder_str + "Sounds/GameMenuMain.mp3");
            string fullname = f.FullName;
            while (game_started && mode == 0)
            {
                MainPlayer.Open(new Uri(fullname));
                MainPlayer.Play();
                await Task.Delay(1000 * 55);
            }
        }


        // МУЗЫКА МЕНЮ ВЫБОРА РЕЖИМА
        private async void PlayGameMenuMusic()
        {
            FileInfo f = new FileInfo(adder_str+"Sounds/GameMenuMusic.mp3");
            string fullname = f.FullName;
            while (game_started && mode == 0)
            {
                MusicPlayer.Open(new Uri(fullname));
                MusicPlayer.Play();
                await Task.Delay(1000 * 180);
            }
        }


        // ЗВУКИ БОЯ (МЕНЮ БОЯ)
        private async void PlayFightMain()
        {
            FileInfo f = new FileInfo(adder_str + "Sounds/fight_main.mp3");
            string fullname = f.FullName;
            while (game_started && mode != 0)
            {
                MainPlayer.Open(new Uri(fullname));
                MainPlayer.Play();
                await Task.Delay(1000 * 90);
            }
        }


        // МУЗЫКА МЕНЮ БОЯ
        private async void PlayFightMusic()
        {
            FileInfo f = new FileInfo(adder_str + "Sounds/fight_music.mp3");
            string fullname = f.FullName;
            MusicBar.Value = (int)(MusicPlayer.Volume * 100);
            while (game_started && mode != 0)
            {
                MusicPlayer.Open(new Uri(fullname));
                MusicPlayer.Play();
                await Task.Delay(1000 * 163);
            }
        }


        // ИЗМЕНЕНИЕ ГРОМКОСКИ МУЗЫКИ
        private void MusicBar_Scroll(object sender, EventArgs e)
        {
            MusicPlayer.Volume = (double)MusicBar.Value / 100;
            MusicValLabel.Text = $"Музыка: {MusicBar.Value}%";
        }


        // ИЗМЕНЕНИЕ ГРОМКОСКИ ЗВУКОВ
        private void SoundsBar_Scroll(object sender, EventArgs e)
        {
            MainPlayer.Volume = (double)SoundsBar.Value / 100;
            SoundsPlayer.Volume = (double)SoundsBar.Value / 100;
            SoundsValLabel.Text = $"Звуки: {SoundsBar.Value}%";
        }

        // ЗАТИХАНИЕ ЗВУКОВ
        private async void SoundsDown()
        {
            double start_music = MusicPlayer.Volume,
                   start_sound = MainPlayer.Volume;
            while (MusicPlayer.Volume > 0 || MainPlayer.Volume > 0)
            {
                if (MusicPlayer.Volume > 0) MusicPlayer.Volume -= 0.01;
                if (MainPlayer.Volume > 0) MainPlayer.Volume -= 0.01;
                await Task.Delay(20);
            }
            MusicPlayer.Stop();
            MainPlayer.Stop();
            MusicPlayer.Volume = start_music;
            MainPlayer.Volume = start_sound;
        }

        /***************************************************************************************************/


        /**************************************** Анимации ************************************************/

        // КНОПКА УЕЗЖАЕТ ВПРАВО
        private async void MoveButtonOut(System.Windows.Forms.Button l)
        {
            int min_point = l.Location.X - 100;
            while (l.Location.X > min_point)
            {
                await Task.Delay(1);
                l.Location = new Point(l.Location.X - Math.Abs(min_point - l.Location.X) / 3 - 1, l.Location.Y);
            }
            await Task.Delay(100);
            while (l.Location.X < 5000)
            {
                await Task.Delay(1);
                l.Location = new Point(l.Location.X + Math.Abs(min_point - l.Location.X) / 3 + 1, l.Location.Y);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        // ПЛАВНОЕ ПОЯВЛЕНИЕ КНОПКИ
        private async void MoveButtonIn(System.Windows.Forms.Button l)
        {
            l.Location = new Point(-5000, l.Location.Y);
            l.Show();
            int max_point = (PictureBox.Width - l.Width) / 2;
            while (l.Location.X < max_point)
            {
                await Task.Delay(1);
                l.Location = new Point(l.Location.X + Math.Abs(max_point - l.Location.X) / 3 + 1, l.Location.Y);
            }
        }


        // ПЕРЕЗАПУСК ОТОБРАЖЕНИЯ КОРАБЛЕЙ ПОЛЬЗОВАТЕЛЯ
        public void reShowMyShips()
        {
            for (int i = 0; i < 10; i++) for (int j = 0; j < 10; j++)
                {
                    myPen.Color = System.Drawing.Color.DarkSlateGray;
                    mySeaGrid.FillRectangle(myPen.Brush, (52 * i) + 1, (52 * j) + 1, 51, 51);
                    if (mySea[i, j].status != 0)
                    {
                        myPen.Color = System.Drawing.Color.Silver;
                        mySeaGrid.FillRectangle(myPen.Brush, (52 * i) + 1, (52 * j) + 1, 51, 51);
                    }
                }
            pictureBox2.Invalidate();
            pictureBox2.Update();
        }


        // ОТРИСОВКА СЕТКИ ПОЛЯ
        void drawGrid()
        {
            myPen.Color = System.Drawing.Color.Black;
            for (int i = 0; i < 10; i++)
            {
                AISeaGrid.DrawLine(myPen, new Point(pictureBox1.Width / 10 * i, 0),
                                          new Point(pictureBox1.Width / 10 * i, pictureBox1.Height));
                AISeaGrid.DrawLine(myPen, new Point(0, pictureBox1.Height / 10 * i),
                                          new Point(pictureBox1.Width, pictureBox1.Height / 10 * i));
                mySeaGrid.DrawLine(myPen, new Point(pictureBox1.Width / 10 * i, 0),
                                          new Point(pictureBox1.Width / 10 * i, pictureBox1.Height));
                mySeaGrid.DrawLine(myPen, new Point(0, pictureBox1.Height / 10 * i),
                                          new Point(pictureBox1.Width, pictureBox1.Height / 10 * i));
            }
            AISeaGrid.DrawLine(myPen, new Point(pictureBox1.Width - 1, 0),
                                      new Point(pictureBox1.Width - 1, pictureBox1.Height));
            AISeaGrid.DrawLine(myPen, new Point(0, pictureBox1.Height - 1),
                                      new Point(pictureBox1.Width, pictureBox1.Height - 1));
            mySeaGrid.DrawLine(myPen, new Point(pictureBox1.Width - 1, 0),
                                     new Point(pictureBox1.Width - 1, pictureBox1.Height));
            mySeaGrid.DrawLine(myPen, new Point(0, pictureBox1.Height - 1),
                                      new Point(pictureBox1.Width, pictureBox1.Height - 1));
        }


        // ПОПАДАНИЕ МЫШИ В ПОЛЕ ПРОТИВНИКА
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            thisSquare.getPos(Control.MousePosition.X % 10, Control.MousePosition.Y % 10);
            if (AISea[thisSquare.i, thisSquare.j].status < 100)
            {
                myPen.Color = System.Drawing.Color.Silver;
                AISeaGrid.FillRectangle(myPen.Brush, (52 * thisSquare.i) + 1, (52 * thisSquare.j) + 1, 51, 51);
                pictureBox1.Invalidate();
                pictureBox1.Update();
            }

            prevSquare.i = thisSquare.i;
            prevSquare.j = thisSquare.j;
        }


        // ПЕРЕДВИЖЕНИЕ МЫШИ В ПОЛЕ ПРОТИВНИКА
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            thisSquare.getPos(e.X, e.Y);

            if (thisSquare.i != prevSquare.i || thisSquare.j != prevSquare.j)
            {
                if (AISea[thisSquare.i, thisSquare.j].status < 100)
                {
                    myPen.Color = System.Drawing.Color.Silver;
                    AISeaGrid.FillRectangle(myPen.Brush, (52 * thisSquare.i) + 1, (52 * thisSquare.j) + 1, 51, 51);
                }

                if (AISea[prevSquare.i, prevSquare.j].status < 100)
                {
                    myPen.Color = System.Drawing.Color.DarkSlateGray;
                    AISeaGrid.FillRectangle(myPen.Brush, (52 * prevSquare.i) + 1, (52 * prevSquare.j) + 1, 51, 51);
                }
                pictureBox1.Invalidate();
                pictureBox1.Update();

                prevSquare.i = thisSquare.i;
                prevSquare.j = thisSquare.j;
            }
        }


        // УХОД МЫШИ ИЗ ПОЛЯ ПРОТИВНИКА
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            if (AISea[thisSquare.i, thisSquare.j].status < 100)
            {
                myPen.Color = System.Drawing.Color.DarkSlateGray;
                AISeaGrid.FillRectangle(myPen.Brush, (52 * prevSquare.i) + 1, (52 * prevSquare.j) + 1, 51, 51);
                pictureBox1.Invalidate();
                pictureBox1.Update();
            }
        }

        /****************************************************************************************************/


        /***************************************** Бой ******************************************************/

        // ХОД ПОЛЬЗОВАТЕЛЯ
        async private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && AISea[thisSquare.i, thisSquare.j].status < 100)
            {
                bool got = AISea.Fire(thisSquare.i, thisSquare.j);
                protocolWriter.Write($"User\t-\t{mySea[thisSquare.i, thisSquare.j]} ");
                if (got)
                {
                    myPen.Color = System.Drawing.Color.Maroon;
                }
                else
                {
                    myPen.Color = System.Drawing.Color.DarkSlateGray;
                    protocolWriter.Write("(away)\n");
                    PlaySound(adder_str + "Sounds/miss.mp3");
                }
                myPen.Width = 3;
                AISeaGrid.FillRectangle(myPen.Brush, (52 * thisSquare.i) + 1, (52 * thisSquare.j) + 1, 51, 51);
                myPen.Color = System.Drawing.Color.Silver;
                AISeaGrid.DrawLine(myPen, new Point(pictureBox1.Width / 10 * thisSquare.i + 10, pictureBox1.Height / 10 * thisSquare.j + 10),
                                          new Point(pictureBox1.Height / 10 * (thisSquare.i + 1) - 10, pictureBox1.Height / 10 * (thisSquare.j + 1) - 10));
                AISeaGrid.DrawLine(myPen, new Point(pictureBox1.Width / 10 * thisSquare.i + 10, pictureBox1.Height / 10 * (thisSquare.j + 1) - 10),
                                          new Point(pictureBox1.Height / 10 * (thisSquare.i + 1) - 10, pictureBox1.Height / 10 * thisSquare.j + 10));

                if (got && AISea.isDeath(thisSquare.i, thisSquare.j))
                {
                    for (int i = 0; i < 10; i++) for (int j = 0; j < 10; j++) if (AISea[i, j].status == AISea[thisSquare.i, thisSquare.j].status)
                    {
                        for (int x = i - 1; x <= i + 1; x++) for (int y = j - 1; y <= j + 1; y++) if (x >= 0 && x <= 9 && y >= 0 && y <= 9)
                        {
                            AISeaGrid.DrawLine(myPen, new Point(pictureBox1.Width / 10 * x + 10, pictureBox1.Height / 10 * y + 10),
                                                        new Point(pictureBox1.Height / 10 * (x + 1) - 10, pictureBox1.Height / 10 * (y + 1) - 10));
                            AISeaGrid.DrawLine(myPen, new Point(pictureBox1.Width / 10 * x + 10, pictureBox1.Height / 10 * (y + 1) - 10),
                                                        new Point(pictureBox1.Height / 10 * (x + 1) - 10, pictureBox1.Height / 10 * y + 10));

                            if (AISea[x, y].status < 100) AISea[x, y].status += 100;

                        }
                    }
                    PlaySound(adder_str + "Sounds/kill.mp3");
                    protocolWriter.Write("(kill)\n");

                    if (AISea.isWin())
                    {
                        SoundsDown();
                        PlaySound(adder_str + "Sounds/win.mp3");
                        stepLabel.Text = "Победа!";
                        round_started = false;
                        protocolWriter.Write("\nUser wins");
                        protocolWriter.Close();
                        stepLabel.Location = new Point((PictureBox.Width - stepLabel.Width) / 2, 50);
                        pictureBox1.Enabled = false;
                        SettingsButton.Hide();
                        MenuButton.Show();
                        MenuButton.Location = new Point((PictureBox.Width - MenuButton.Width) / 2, PictureBox.Width - 200);
                    }
                }
                else if (got)
                {
                    PlaySound(adder_str + "Sounds/got.mp3");
                    protocolWriter.Write("(hit)\n");
                }

                if (!got)
                {
                    pictureBox1.Enabled = false;
                    AIStep();
                }
                pictureBox1.Invalidate();
                pictureBox1.Update();
                myPen.Width = 1;
            }
        }


        // ХОД КОМПЬЮТЕРА
        async public void AIStep()
        {
            stepLabel.Text = "Ход Компьютера";
            stepLabel.Location = new Point((PictureBox.Width - stepLabel.Width) / 2, 50);
            bool got = true;
            await Task.Delay(2000);
            Point stepPoint = new Point(0, 0);

            do
            {
                if (mode == 1) stepPoint = mySea.lowAIstep();
                else if (mode == 2) stepPoint = mySea.mediumAIstep();
                else if (mode == 3) stepPoint = mySea.highAIstep();

                protocolWriter.Write($"AI\t-\t{mySea[stepPoint.X, stepPoint.Y]} ");

                AISquare.i = (int)stepPoint.X;
                AISquare.j = (int)stepPoint.Y;
                got = mySea.Fire(AISquare.i, AISquare.j);
                if (got)
                {
                    myPen.Color = System.Drawing.Color.Maroon;
                }
                else
                {
                    myPen.Color = System.Drawing.Color.DarkSlateGray;
                    protocolWriter.Write("(away)\n");
                    PlaySound(adder_str + "Sounds/miss.mp3");
                }
                myPen.Width = 3;
                mySeaGrid.FillRectangle(myPen.Brush, (52 * AISquare.i) + 1, (52 * AISquare.j) + 1, 51, 51);
                myPen.Color = System.Drawing.Color.Silver;
                mySeaGrid.DrawLine(myPen, new Point(pictureBox1.Width / 10 * AISquare.i + 10, pictureBox1.Height / 10 * AISquare.j + 10),
                                          new Point(pictureBox1.Height / 10 * (AISquare.i + 1) - 10, pictureBox1.Height / 10 * (AISquare.j + 1) - 10));
                mySeaGrid.DrawLine(myPen, new Point(pictureBox1.Width / 10 * AISquare.i + 10, pictureBox1.Height / 10 * (AISquare.j + 1) - 10),
                                          new Point(pictureBox1.Height / 10 * (AISquare.i + 1) - 10, pictureBox1.Height / 10 * AISquare.j + 10));

                if (got && mySea.isDeath(AISquare.i, AISquare.j))
                {
                    for (int i = 0; i < 10; i++) for (int j = 0; j < 10; j++) if (mySea[i, j].status == mySea[AISquare.i, AISquare.j].status)
                    {
                        for (int x = i - 1; x <= i + 1; x++) for (int y = j - 1; y <= j + 1; y++) if (x >= 0 && x <= 9 && y >= 0 && y <= 9)
                        {
                            mySeaGrid.DrawLine(myPen, new Point(pictureBox1.Width / 10 * x + 10, pictureBox1.Height / 10 * y + 10),
                                                        new Point(pictureBox1.Height / 10 * (x + 1) - 10, pictureBox1.Height / 10 * (y + 1) - 10));
                            mySeaGrid.DrawLine(myPen, new Point(pictureBox1.Width / 10 * x + 10, pictureBox1.Height / 10 * (y + 1) - 10),
                                                        new Point(pictureBox1.Height / 10 * (x + 1) - 10, pictureBox1.Height / 10 * y + 10));

                            if (mySea[x, y].status < 100) mySea[x, y].status += 100;

                        }
                    }
                    protocolWriter.Write("(kill)\n");
                    PlaySound(adder_str + "Sounds/kill.mp3");

                    if (mySea.isWin())
                    {
                        PlaySound(adder_str+"Sounds/lose.mp3");
                        SoundsDown();
                        protocolWriter.Write("\nAI wins\n");
                        protocolWriter.Close();
                        stepLabel.Text = "Поражение!";
                        round_started = false;
                        stepLabel.Location = new Point((PictureBox.Width - stepLabel.Width) / 2, 50);
                        SettingsButton.Hide();
                        MenuButton.Show();
                        MenuButton.Location = new Point((PictureBox.Width - MenuButton.Width) / 2, PictureBox.Height - 200);
                    }
                }
                else if (got)
                {
                    protocolWriter.Write("(hit)\n");
                    PlaySound(adder_str + "Sounds/got.mp3");
                }
                pictureBox2.Invalidate();
                pictureBox2.Update();
                myPen.Width = 1;

                await Task.Delay(2000);
            } while (got && !mySea.isWin());

            if (!mySea.isWin())
            {
                stepLabel.Text = "Ваш Ход";
                stepLabel.Location = new Point((PictureBox.Width - stepLabel.Width) / 2, 50);
                if (!loseNear_played && mySea.oneShip())
                {
                    loseNear_played = true;
                    PlaySound(adder_str + "Sounds/Maxim_loseNear.m4a");
                    await Task.Delay(3000);
                }
                pictureBox1.Enabled = true;
            }
        }

        /*****************************************************************************************************/
    }

}
