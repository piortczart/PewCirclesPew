namespace PewCircles
{
    partial class PewWindow
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
            this.SuspendLayout();
            // 
            // PewWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 747);
            this.Name = "PewWindow";
            this.Text = "Pewpewpew!";
            this.Click += new System.EventHandler(this.PewWindow_Click);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PewWindow_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PewWindow_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PewWindow_MouseDown);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

