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
            this.selectFolderButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.intervalComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.wallpaperFolderLabel = new System.Windows.Forms.Label();
            this.unregisterButton = new System.Windows.Forms.Button();
            this.skipListBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.openLogButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // selectFolderButton
            // 
            this.selectFolderButton.Location = new System.Drawing.Point(12, 39);
            this.selectFolderButton.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.selectFolderButton.Name = "selectFolderButton";
            this.selectFolderButton.Size = new System.Drawing.Size(128, 33);
            this.selectFolderButton.TabIndex = 1;
            this.selectFolderButton.Text = "Select folder";
            this.selectFolderButton.UseVisualStyleBackColor = true;
            this.selectFolderButton.Click += new System.EventHandler(this.SelectFolderButtonClick);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Select wallpapers folder";
            // 
            // intervalComboBox
            // 
            this.intervalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.intervalComboBox.FormattingEnabled = true;
            this.intervalComboBox.Location = new System.Drawing.Point(12, 107);
            this.intervalComboBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.intervalComboBox.Name = "intervalComboBox";
            this.intervalComboBox.Size = new System.Drawing.Size(128, 33);
            this.intervalComboBox.TabIndex = 3;
            this.intervalComboBox.SelectedIndexChanged += new System.EventHandler(this.IntervalComboBoxSelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Change picture every";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Current folder:";
            // 
            // wallpaperFolderLabel
            // 
            this.wallpaperFolderLabel.AutoSize = true;
            this.wallpaperFolderLabel.Location = new System.Drawing.Point(119, 9);
            this.wallpaperFolderLabel.Name = "wallpaperFolderLabel";
            this.wallpaperFolderLabel.Size = new System.Drawing.Size(52, 25);
            this.wallpaperFolderLabel.TabIndex = 6;
            this.wallpaperFolderLabel.Text = "not set";
            // 
            // unregisterButton
            // 
            this.unregisterButton.Location = new System.Drawing.Point(12, 150);
            this.unregisterButton.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.unregisterButton.Name = "unregisterButton";
            this.unregisterButton.Size = new System.Drawing.Size(128, 38);
            this.unregisterButton.TabIndex = 7;
            this.unregisterButton.Text = "Disable WalMan";
            this.unregisterButton.UseVisualStyleBackColor = true;
            this.unregisterButton.Click += new System.EventHandler(this.UnregisterButtonClick);
            // 
            // skipListBox
            // 
            this.skipListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skipListBox.FormattingEnabled = true;
            this.skipListBox.IntegralHeight = false;
            this.skipListBox.ItemHeight = 25;
            this.skipListBox.Location = new System.Drawing.Point(146, 67);
            this.skipListBox.Name = "skipListBox";
            this.skipListBox.Size = new System.Drawing.Size(224, 274);
            this.skipListBox.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(146, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 25);
            this.label3.TabIndex = 9;
            this.label3.Text = "Skip list:";
            // 
            // openLogButton
            // 
            this.openLogButton.Location = new System.Drawing.Point(12, 198);
            this.openLogButton.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.openLogButton.Name = "openLogButton";
            this.openLogButton.Size = new System.Drawing.Size(128, 38);
            this.openLogButton.TabIndex = 10;
            this.openLogButton.Text = "Open Log";
            this.openLogButton.UseVisualStyleBackColor = true;
            this.openLogButton.Click += new System.EventHandler(this.OpenLogButtonClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 353);
            this.Controls.Add(this.openLogButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.skipListBox);
            this.Controls.Add(this.unregisterButton);
            this.Controls.Add(this.wallpaperFolderLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.intervalComboBox);
            this.Controls.Add(this.selectFolderButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "MainForm";
            this.Text = "WalMan";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button selectFolderButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ComboBox intervalComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label wallpaperFolderLabel;
        private System.Windows.Forms.Button unregisterButton;
        private ListBox skipListBox;
        private Label label3;
        private Button openLogButton;
    }
}

