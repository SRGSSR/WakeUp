namespace WakeUpGui
{
    partial class WakeUpForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WakeUpForm));
            this.WakeUpSleepIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.WakeNonSleepIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.WakeUpIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // WakeUpSleepIcon
            // 
            this.WakeUpSleepIcon.BalloonTipText = "Sleep mode is enabled";
            this.WakeUpSleepIcon.BalloonTipTitle = "Sleep Allowed";
            this.WakeUpSleepIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("WakeUpSleepIcon.Icon")));
            this.WakeUpSleepIcon.Text = "Sleeep";
            // 
            // WakeNonSleepIcon
            // 
            this.WakeNonSleepIcon.BalloonTipText = "Sleep disabled";
            this.WakeNonSleepIcon.BalloonTipTitle = "Sleep Off";
            this.WakeNonSleepIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("WakeNonSleepIcon.Icon")));
            this.WakeNonSleepIcon.Text = "Sleep switched off";
            // 
            // WakeUpIcon
            // 
            this.WakeUpIcon.BalloonTipText = "WakeUp";
            this.WakeUpIcon.BalloonTipTitle = "WakeUp";
            this.WakeUpIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("WakeUpIcon.Icon")));
            this.WakeUpIcon.Text = "WakeUpIcon";
            this.WakeUpIcon.Visible = true;
            // 
            // WakeUpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "WakeUpForm";
            this.Text = "WakeUp";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.WakeUpForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon WakeUpSleepIcon;
        private System.Windows.Forms.NotifyIcon WakeNonSleepIcon;
        private System.Windows.Forms.NotifyIcon WakeUpIcon;
    }
}

