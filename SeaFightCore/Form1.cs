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

namespace SeaFight
{
    public partial class Form1 : Form
    {
        /********************* Глобальные переменные **********************************/

        static MediaPlayer MainPlayer = new MediaPlayer(),
                           SoundsPlayer = new MediaPlayer(),
                           MusicPlayer = new MediaPlayer();
        bool game_started = false;
        IniFile settings = new IniFile("../../../Files/settings.ini");
        /******************************************************************************/








        public Form1()
        {
            InitializeComponent();
            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;

            InitialMenu();



        }











        /************************* Функционал кнопок ***********************/
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
        }

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
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
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
            PlaySound("../../../Sounds/Start.mp3");
            await Task.Delay(1000);

            MainLabel.Hide();
            LabelBox.Hide();
            PictureBox.Image = Image.FromFile("../../../Images/Loading.bmp");
            await Task.Delay(4000);
            InitialGameMenu();
            
        }

        /********************************************************************/




        /********************** Инициализация начала ************************/
        private async void InitialMenu()
        {
            MusicPlayer.Volume = (double)Int32.Parse(settings.Read("MusicVal", "Sound")) / 100;
            MainPlayer.Volume = (double)Int32.Parse(settings.Read("SoundVal", "Sound")) / 100;
            SoundsPlayer.Volume = MainPlayer.Volume;
            GameLoading();
            PlayMain();
            PlayMenuMusic();

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

        private async void InitialGameMenu()
        {
            game_started = true;
            PictureBox.Image = Image.FromFile("../../../Images/Game_menu.bmp");
            PlayGameMenuMain();
            PlayGameMenuMusic();

            SettingsButton.Size = new Size(200, 50);
            SettingsButton.Location = new Point(PictureBox.Width - SettingsButton.Width - 20, 20);
            SettingsButton.Text = "Настройки"; SettingsButton.Font = new Font("Monotype Corsiva", 20);
            SettingsButton.Refresh();
            SettingsButton.TextAlign = ContentAlignment.MiddleCenter;
            SettingsButton.Enabled = true;
        }
        /********************************************************************/




        /****************************** Звуки *******************************/
        private void PlaySound(string path)
        {
            FileInfo f = new FileInfo(path);
            string fullname = f.FullName;
            SoundsPlayer.Open(new Uri(fullname));
            SoundsPlayer.Play();
        }


        private async void PlayMain()
        {
            FileInfo f = new FileInfo("../../../Sounds/Main_Sound.mp3");
            string fullname = f.FullName;
            SoundsBar.Value = (int)(SoundsPlayer.Volume * 100);
            while (!game_started)
            {
                MainPlayer.Open(new Uri(fullname));
                MainPlayer.Play();
                await Task.Delay(1000 * 93);
            }
        }

        private async void PlayGameMenuMain()
        {
            FileInfo f = new FileInfo("../../../Sounds/GameMenuMain.mp3");
            string fullname = f.FullName;
            while (game_started)
            {
                MainPlayer.Open(new Uri(fullname));
                MainPlayer.Play();
                await Task.Delay(1000 * 55);
            }
        }

        private async void PlayGameMenuMusic()
        {
            FileInfo f = new FileInfo("../../../Sounds/GameMenuMusic.mp3");
            string fullname = f.FullName;
            while (game_started)
            {
                MusicPlayer.Open(new Uri(fullname));
                MusicPlayer.Play();
                await Task.Delay(1000 * 180);
            }
        }

        private void MusicBar_Scroll(object sender, EventArgs e)
        {
            MusicPlayer.Volume = (double)MusicBar.Value / 100;
            MusicValLabel.Text = $"Музыка: {MusicBar.Value}%";
        }

        private void SoundsBar_Scroll(object sender, EventArgs e)
        {
            MainPlayer.Volume = (double)SoundsBar.Value / 100;
            SoundsPlayer.Volume = (double)SoundsBar.Value / 100;
            SoundsValLabel.Text = $"Звуки: {SoundsBar.Value}%";
        }

        private async void PlayMenuMusic()
        {
            FileInfo f = new FileInfo("../../../Sounds/Menu_Music.mp3");
            string fullname = f.FullName;
            MusicBar.Value = (int)(MusicPlayer.Volume * 100);
            while (!game_started)
            {
                MusicPlayer.Open(new Uri(fullname));
                MusicPlayer.Play();
                await Task.Delay(1000 * 338);
            }
        }

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

        /*********************************************************************/










        /**************************** Анимации ******************************/
        private async void MoveButtonOut(Button l)
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

        private async void MoveButtonIn(Button l)
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

        private async void GameLoading()
        {
            ExitButton.Hide();
            SettingsButton.Hide();
            PlayButton.Hide();
            DevLabel.Hide();
            MainLabel.Hide();
            LabelBox.Hide();

            await Task.Delay(3000);
            PictureBox.Image = Image.FromFile("../../../Images/Main_Image.gif");

            DevLabel.Show();
            LabelBox.Show();
            MainLabel.Show();
            MoveButtonIn(PlayButton);
            await Task.Delay(100);
            MoveButtonIn(SettingsButton);
            await Task.Delay(100);
            MoveButtonIn(ExitButton);
        }


        /********************************************************************/




        /****************************   INI  ********************************/

        class IniFile   // revision 11
        {
            string Path;
            string EXE = Assembly.GetExecutingAssembly().GetName().Name;

            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

            public IniFile(string IniPath = null)
            {
                Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;
            }

            public string Read(string Key, string Section = null)
            {
                var RetVal = new StringBuilder(255);
                GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
                return RetVal.ToString();
            }

            public void Write(string Key, string Value, string Section = null)
            {
                WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
            }

            public void DeleteKey(string Key, string Section = null)
            {
                Write(Key, null, Section ?? EXE);
            }

            public void DeleteSection(string Section = null)
            {
                Write(null, null, Section ?? EXE);
            }

            public bool KeyExists(string Key, string Section = null)
            {
                return Read(Key, Section).Length > 0;
            }
        }


        /********************************************************************/
    }

}
