namespace SelfHostedYoloScreenCapture
{
    partial class ActionBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._upload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _upload
            // 
            this._upload.Location = new System.Drawing.Point(0, 0);
            this._upload.Name = "_upload";
            this._upload.Size = new System.Drawing.Size(33, 23);
            this._upload.TabIndex = 0;
            this._upload.Text = "Up";
            this._upload.UseVisualStyleBackColor = true;
            // 
            // ActionBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._upload);
            this.Name = "ActionBox";
            this.Size = new System.Drawing.Size(33, 23);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _upload;
    }
}
