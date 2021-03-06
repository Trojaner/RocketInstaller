﻿using MetroFramework.Controls;

namespace RocketInstaller
{
    partial class InstallerForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
         private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallerForm));
            this.InstallButton = new MetroFramework.Controls.MetroButton();
            this.InstallDirectory = new MetroFramework.Controls.MetroTextBox();
            this.BrowseButton = new MetroFramework.Controls.MetroButton();
            this.InstallProgress = new MetroFramework.Controls.MetroProgressBar();
            this.StatusLabel = new MetroFramework.Controls.MetroLabel();
            this.UseSeparateCheckBox = new MetroFramework.Controls.MetroCheckBox();
            this.SuspendLayout();
            // 
            // InstallButton
            // 
            this.InstallButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InstallButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.InstallButton.Location = new System.Drawing.Point(83, 160);
            this.InstallButton.Name = "InstallButton ";
            this.InstallButton.Size = new System.Drawing.Size(146, 54);
            this.InstallButton.TabIndex = 2;
            this.InstallButton.Text = "Install";
            this.InstallButton.UseSelectable = true;
            this.InstallButton.UseVisualStyleBackColor = true;
            // 
            // InstallDirectory
            // 
            this.InstallDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InstallDirectory.Lines = new string[0];
            this.InstallDirectory.Location = new System.Drawing.Point(23, 63);
            this.InstallDirectory.MaxLength = 32767;
            this.InstallDirectory.Name = "InstallDirectory";
            this.InstallDirectory.PasswordChar = '\0';
            this.InstallDirectory.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.InstallDirectory.SelectedText = "";
            this.InstallDirectory.Size = new System.Drawing.Size(246, 20);
            this.InstallDirectory.TabIndex = 3;
            this.InstallDirectory.UseSelectable = true;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseButton.Location = new System.Drawing.Point(275, 63);
            this.BrowseButton.Name = "browseButton";
            this.BrowseButton.Size = new System.Drawing.Size(22, 20);
            this.BrowseButton.TabIndex = 4;
            this.BrowseButton.Text = "...";
            this.BrowseButton.UseSelectable = true;
            this.BrowseButton.UseVisualStyleBackColor = true;
            // 
            // InstallProgress
            // 
            this.InstallProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InstallProgress.Location = new System.Drawing.Point(24, 110);
            this.InstallProgress.Name = "InstallProgress";
            this.InstallProgress.Size = new System.Drawing.Size(274, 23);
            this.InstallProgress.TabIndex = 5;
            // 
            // StatusLabel
            // 
            this.StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.StatusLabel.Location = new System.Drawing.Point(6, 230);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(89, 19);
            this.StatusLabel.TabIndex = 6;
            this.StatusLabel.Text = "Status: Ready.";
            // 
            // UseSeparateCheckBox
            // 
            this.UseSeparateCheckBox.AutoSize = true;
            this.UseSeparateCheckBox.Checked = true;
            this.UseSeparateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseSeparateCheckBox.Location = new System.Drawing.Point(23, 87);
            this.UseSeparateCheckBox.Name = "UseSeparateCheckBox";
            this.UseSeparateCheckBox.Size = new System.Drawing.Size(164, 15);
            this.UseSeparateCheckBox.TabIndex = 7;
            this.UseSeparateCheckBox.Text = "Install in separate directory";
            this.UseSeparateCheckBox.UseSelectable = true;
            this.UseSeparateCheckBox.UseVisualStyleBackColor = true;
            // 
            // InstallerForm
            // 
            this.ApplyImageInvert = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackImage = global::RocketInstaller.Properties.Resources.logo;
            this.BackImagePadding = new System.Windows.Forms.Padding(210, 10, 0, 0);
            this.BackMaxSize = 50;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(320, 260);
            this.Controls.Add(this.UseSeparateCheckBox);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.InstallProgress);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.InstallDirectory);
            this.Controls.Add(this.InstallButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(320, 260);
            this.MinimumSize = new System.Drawing.Size(320, 260);
            this.Name = "InstallerForm";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.Text = "Rocket Installer";
            this.Load += new System.EventHandler(this.InstallerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

         private MetroButton InstallButton;
         private MetroButton BrowseButton;
         private MetroTextBox InstallDirectory;
         private MetroProgressBar InstallProgress;
         private MetroLabel StatusLabel;
         private MetroCheckBox UseSeparateCheckBox;
    }
}

