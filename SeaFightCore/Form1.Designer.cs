namespace SeaFight
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.PlayButton = new System.Windows.Forms.Button();
            this.SettingsButton = new System.Windows.Forms.Button();
            this.MainLabel = new System.Windows.Forms.Label();
            this.ExitButton = new System.Windows.Forms.Button();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.LabelBox = new System.Windows.Forms.PictureBox();
            this.DevLabel = new System.Windows.Forms.Label();
            this.BackButton = new System.Windows.Forms.Button();
            this.MusicBar = new System.Windows.Forms.TrackBar();
            this.SoundsBar = new System.Windows.Forms.TrackBar();
            this.MusicValLabel = new System.Windows.Forms.Label();
            this.SoundsValLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LabelBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MusicBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SoundsBar)).BeginInit();
            this.SuspendLayout();
            // 
            // PlayButton
            // 
            this.PlayButton.BackColor = System.Drawing.Color.LightGreen;
            this.PlayButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PlayButton.Font = new System.Drawing.Font("Monotype Corsiva", 19.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PlayButton.Location = new System.Drawing.Point(436, 387);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(602, 53);
            this.PlayButton.TabIndex = 0;
            this.PlayButton.Text = "Играть";
            this.PlayButton.UseVisualStyleBackColor = false;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // SettingsButton
            // 
            this.SettingsButton.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.SettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SettingsButton.Font = new System.Drawing.Font("Monotype Corsiva", 19.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SettingsButton.Location = new System.Drawing.Point(436, 469);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(602, 53);
            this.SettingsButton.TabIndex = 1;
            this.SettingsButton.Text = "Настройки";
            this.SettingsButton.UseVisualStyleBackColor = false;
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // MainLabel
            // 
            this.MainLabel.AutoSize = true;
            this.MainLabel.BackColor = System.Drawing.Color.Transparent;
            this.MainLabel.Font = new System.Drawing.Font("Monotype Corsiva", 64.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainLabel.ForeColor = System.Drawing.Color.Black;
            this.MainLabel.Location = new System.Drawing.Point(249, 106);
            this.MainLabel.Name = "MainLabel";
            this.MainLabel.Size = new System.Drawing.Size(597, 134);
            this.MainLabel.TabIndex = 2;
            this.MainLabel.Text = "Морской Бой";
            // 
            // ExitButton
            // 
            this.ExitButton.BackColor = System.Drawing.Color.IndianRed;
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ExitButton.Font = new System.Drawing.Font("Monotype Corsiva", 19.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ExitButton.ForeColor = System.Drawing.Color.Black;
            this.ExitButton.Location = new System.Drawing.Point(436, 545);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(602, 53);
            this.ExitButton.TabIndex = 3;
            this.ExitButton.Text = "Выход";
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // PictureBox
            // 
            this.PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBox.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox.Image")));
            this.PictureBox.Location = new System.Drawing.Point(0, 0);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(1902, 1033);
            this.PictureBox.TabIndex = 4;
            this.PictureBox.TabStop = false;
            // 
            // LabelBox
            // 
            this.LabelBox.BackColor = System.Drawing.Color.Transparent;
            this.LabelBox.Image = ((System.Drawing.Image)(resources.GetObject("LabelBox.Image")));
            this.LabelBox.Location = new System.Drawing.Point(41, 66);
            this.LabelBox.Name = "LabelBox";
            this.LabelBox.Size = new System.Drawing.Size(1100, 239);
            this.LabelBox.TabIndex = 5;
            this.LabelBox.TabStop = false;
            // 
            // DevLabel
            // 
            this.DevLabel.AutoSize = true;
            this.DevLabel.BackColor = System.Drawing.Color.Transparent;
            this.DevLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DevLabel.ForeColor = System.Drawing.Color.LightGray;
            this.DevLabel.Location = new System.Drawing.Point(12, 997);
            this.DevLabel.Name = "DevLabel";
            this.DevLabel.Size = new System.Drawing.Size(258, 23);
            this.DevLabel.TabIndex = 6;
            this.DevLabel.Text = "Сироткин Илья, ГУАП 2023";
            // 
            // BackButton
            // 
            this.BackButton.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.BackButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BackButton.Font = new System.Drawing.Font("Monotype Corsiva", 19.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BackButton.Location = new System.Drawing.Point(272, 618);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(602, 53);
            this.BackButton.TabIndex = 7;
            this.BackButton.Text = "Назад";
            this.BackButton.UseVisualStyleBackColor = false;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // MusicBar
            // 
            this.MusicBar.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.MusicBar.Location = new System.Drawing.Point(248, 284);
            this.MusicBar.Maximum = 100;
            this.MusicBar.Name = "MusicBar";
            this.MusicBar.Size = new System.Drawing.Size(368, 56);
            this.MusicBar.TabIndex = 8;
            this.MusicBar.TickFrequency = 5;
            this.MusicBar.Scroll += new System.EventHandler(this.MusicBar_Scroll);
            // 
            // SoundsBar
            // 
            this.SoundsBar.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.SoundsBar.Location = new System.Drawing.Point(767, 488);
            this.SoundsBar.Maximum = 100;
            this.SoundsBar.Name = "SoundsBar";
            this.SoundsBar.Size = new System.Drawing.Size(368, 56);
            this.SoundsBar.TabIndex = 9;
            this.SoundsBar.TickFrequency = 5;
            this.SoundsBar.Scroll += new System.EventHandler(this.SoundsBar_Scroll);
            // 
            // MusicValLabel
            // 
            this.MusicValLabel.AutoSize = true;
            this.MusicValLabel.BackColor = System.Drawing.Color.Transparent;
            this.MusicValLabel.Font = new System.Drawing.Font("Monotype Corsiva", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MusicValLabel.Location = new System.Drawing.Point(256, 292);
            this.MusicValLabel.Name = "MusicValLabel";
            this.MusicValLabel.Size = new System.Drawing.Size(170, 37);
            this.MusicValLabel.TabIndex = 10;
            this.MusicValLabel.Text = "Музыка: 50%";
            // 
            // SoundsValLabel
            // 
            this.SoundsValLabel.AutoSize = true;
            this.SoundsValLabel.BackColor = System.Drawing.Color.Transparent;
            this.SoundsValLabel.Font = new System.Drawing.Font("Monotype Corsiva", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SoundsValLabel.Location = new System.Drawing.Point(371, 292);
            this.SoundsValLabel.Name = "SoundsValLabel";
            this.SoundsValLabel.Size = new System.Drawing.Size(144, 37);
            this.SoundsValLabel.TabIndex = 11;
            this.SoundsValLabel.Text = "Звуки: 50%";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.DevLabel);
            this.Controls.Add(this.PictureBox);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.SettingsButton);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.LabelBox);
            this.Controls.Add(this.MainLabel);
            this.Controls.Add(this.BackButton);
            this.Controls.Add(this.MusicBar);
            this.Controls.Add(this.SoundsBar);
            this.Controls.Add(this.SoundsValLabel);
            this.Controls.Add(this.MusicValLabel);
            this.Name = "Form1";
            this.Text = "Морской Бой";
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LabelBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MusicBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SoundsBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Button SettingsButton;
        private System.Windows.Forms.Label MainLabel;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.PictureBox LabelBox;
        private System.Windows.Forms.Label DevLabel;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.TrackBar MusicBar;
        private System.Windows.Forms.TrackBar SoundsBar;
        private System.Windows.Forms.Label MusicValLabel;
        private System.Windows.Forms.Label SoundsValLabel;
    }
}

