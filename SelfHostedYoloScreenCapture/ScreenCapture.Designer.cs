namespace SelfHostedYoloScreenCapture
{
    partial class ScreenCapture
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
            this._canvas = new System.Windows.Forms.PictureBox();
            this._actionBox = new SelfHostedYoloScreenCapture.ActionBox();
            ((System.ComponentModel.ISupportInitialize)(this._canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // _canvas
            // 
            this._canvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._canvas.Location = new System.Drawing.Point(0, 0);
            this._canvas.Margin = new System.Windows.Forms.Padding(0);
            this._canvas.Name = "_canvas";
            this._canvas.Size = new System.Drawing.Size(283, 261);
            this._canvas.TabIndex = 0;
            this._canvas.TabStop = false;
            // 
            // _actionBox
            // 
            this._actionBox.Location = new System.Drawing.Point(12, 12);
            this._actionBox.Name = "_actionBox";
            this._actionBox.Size = new System.Drawing.Size(33, 23);
            this._actionBox.TabIndex = 1;
            this._actionBox.Visible = false;
            // 
            // ScreenCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.ControlBox = false;
            this.Controls.Add(this._actionBox);
            this.Controls.Add(this._canvas);
            this.Name = "ScreenCapture";
            this.ShowInTaskbar = false;
            this.Text = "ScreenCapture";
            ((System.ComponentModel.ISupportInitialize)(this._canvas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox _canvas;
        private ActionBox _actionBox;
    }
}