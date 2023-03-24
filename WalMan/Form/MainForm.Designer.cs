using System.Windows.Forms;

namespace WalMan
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            selectWallpaperButton = new Button();
            intervalComboBox = new ComboBox();
            label2 = new Label();
            label1 = new Label();
            currentWallpaperLabel = new Label();
            disableButton = new Button();
            skipListBox = new ListBox();
            label3 = new Label();
            openLogButton = new Button();
            openFileDialog = new OpenFileDialog();
            SuspendLayout();
            // 
            // selectWallpaperButton
            // 
            selectWallpaperButton.Location = new System.Drawing.Point(20, 50);
            selectWallpaperButton.Margin = new Padding(5, 6, 5, 6);
            selectWallpaperButton.Name = "selectWallpaperButton";
            selectWallpaperButton.Size = new System.Drawing.Size(208, 42);
            selectWallpaperButton.TabIndex = 1;
            selectWallpaperButton.Text = "Select Wallpaper";
            selectWallpaperButton.UseVisualStyleBackColor = true;
            selectWallpaperButton.Click += SelectWallpaperButtonClick;
            // 
            // intervalComboBox
            // 
            intervalComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            intervalComboBox.FormattingEnabled = true;
            intervalComboBox.Location = new System.Drawing.Point(20, 137);
            intervalComboBox.Margin = new Padding(5, 6, 5, 6);
            intervalComboBox.Name = "intervalComboBox";
            intervalComboBox.Size = new System.Drawing.Size(206, 40);
            intervalComboBox.TabIndex = 3;
            intervalComboBox.SelectedIndexChanged += IntervalComboBoxSelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(20, 99);
            label2.Margin = new Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(153, 32);
            label2.TabIndex = 4;
            label2.Text = "Time interval";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(20, 12);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(212, 32);
            label1.TabIndex = 5;
            label1.Text = "Current Wallpaper:";
            // 
            // currentWallpaperLabel
            // 
            currentWallpaperLabel.AutoSize = true;
            currentWallpaperLabel.Location = new System.Drawing.Point(242, 12);
            currentWallpaperLabel.Margin = new Padding(5, 0, 5, 0);
            currentWallpaperLabel.Name = "currentWallpaperLabel";
            currentWallpaperLabel.Size = new System.Drawing.Size(88, 32);
            currentWallpaperLabel.TabIndex = 6;
            currentWallpaperLabel.Text = "not set";
            // 
            // disableButton
            // 
            disableButton.Location = new System.Drawing.Point(20, 192);
            disableButton.Margin = new Padding(5, 6, 5, 6);
            disableButton.Name = "disableButton";
            disableButton.Size = new System.Drawing.Size(208, 49);
            disableButton.TabIndex = 7;
            disableButton.Text = "Disable WalMan";
            disableButton.UseVisualStyleBackColor = true;
            disableButton.Click += UnregisterButtonClick;
            // 
            // skipListBox
            // 
            skipListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            skipListBox.FormattingEnabled = true;
            skipListBox.IntegralHeight = false;
            skipListBox.ItemHeight = 32;
            skipListBox.Location = new System.Drawing.Point(237, 86);
            skipListBox.Margin = new Padding(5, 4, 5, 4);
            skipListBox.Name = "skipListBox";
            skipListBox.Size = new System.Drawing.Size(380, 358);
            skipListBox.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(237, 50);
            label3.Margin = new Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(101, 32);
            label3.TabIndex = 9;
            label3.Text = "Skip list:";
            // 
            // openLogButton
            // 
            openLogButton.Location = new System.Drawing.Point(20, 253);
            openLogButton.Margin = new Padding(5, 6, 5, 6);
            openLogButton.Name = "openLogButton";
            openLogButton.Size = new System.Drawing.Size(208, 49);
            openLogButton.TabIndex = 10;
            openLogButton.Text = "Open Log";
            openLogButton.UseVisualStyleBackColor = true;
            openLogButton.Click += OpenLogButtonClick;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(783, 580);
            Controls.Add(openLogButton);
            Controls.Add(label3);
            Controls.Add(skipListBox);
            Controls.Add(disableButton);
            Controls.Add(currentWallpaperLabel);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(intervalComboBox);
            Controls.Add(selectWallpaperButton);
            Margin = new Padding(5, 6, 5, 6);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(796, 620);
            Name = "MainForm";
            Text = "WalMan";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button selectWallpaperButton;
        private System.Windows.Forms.ComboBox intervalComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label currentWallpaperLabel;
        private System.Windows.Forms.Button disableButton;
        private ListBox skipListBox;
        private Label label3;
        private Button openLogButton;
        private OpenFileDialog openFileDialog;
    }
}

