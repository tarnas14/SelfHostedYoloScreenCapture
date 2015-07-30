namespace SelfHostedYoloScreenCapture.PhotoUploading
{
    partial class PhotoUploaderPresentingResult
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
            this._close = new System.Windows.Forms.Button();
            this._copy = new System.Windows.Forms.Button();
            this._path = new System.Windows.Forms.TextBox();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._serverError = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this._serverError.SuspendLayout();
            this.SuspendLayout();
            // 
            // _close
            // 
            this._close.Location = new System.Drawing.Point(12, 13);
            this._close.Name = "_close";
            this._close.Size = new System.Drawing.Size(47, 23);
            this._close.TabIndex = 0;
            this._close.Text = "Close";
            this._close.UseVisualStyleBackColor = true;
            // 
            // _copy
            // 
            this._copy.Location = new System.Drawing.Point(65, 13);
            this._copy.Name = "_copy";
            this._copy.Size = new System.Drawing.Size(46, 23);
            this._copy.TabIndex = 1;
            this._copy.Text = "Copy";
            this._copy.UseVisualStyleBackColor = true;
            // 
            // _path
            // 
            this._path.Location = new System.Drawing.Point(117, 16);
            this._path.Name = "_path";
            this._path.Size = new System.Drawing.Size(255, 20);
            this._path.TabIndex = 2;
            // 
            // _progressBar
            // 
            this._progressBar.Location = new System.Drawing.Point(12, 13);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(360, 27);
            this._progressBar.TabIndex = 3;
            // 
            // _serverError
            // 
            this._serverError.Controls.Add(this.label1);
            this._serverError.Location = new System.Drawing.Point(65, 1);
            this._serverError.Name = "_serverError";
            this._serverError.Size = new System.Drawing.Size(317, 47);
            this._serverError.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server ERROR: could not upload, try again :(";
            // 
            // PhotoUploaderPresentingResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 51);
            this.ControlBox = false;
            this.Controls.Add(this._progressBar);
            this.Controls.Add(this._serverError);
            this.Controls.Add(this._path);
            this.Controls.Add(this._copy);
            this.Controls.Add(this._close);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PhotoUploaderPresentingResult";
            this.ShowInTaskbar = false;
            this.Text = "Link to your photo";
            this.TopMost = true;
            this._serverError.ResumeLayout(false);
            this._serverError.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _close;
        private System.Windows.Forms.Button _copy;
        private System.Windows.Forms.TextBox _path;
        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Panel _serverError;
        private System.Windows.Forms.Label label1;
    }
}