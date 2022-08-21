namespace SyNotebook
{
    partial class PasswordNewForm
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
			this.btOK = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.labText = new System.Windows.Forms.Label();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.tbRepeatPassword = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btOK
			// 
			this.btOK.Location = new System.Drawing.Point(102, 155);
			this.btOK.Margin = new System.Windows.Forms.Padding(4);
			this.btOK.Name = "btOK";
			this.btOK.Size = new System.Drawing.Size(95, 28);
			this.btOK.TabIndex = 2;
			this.btOK.Text = "ОК";
			this.btOK.UseVisualStyleBackColor = true;
			this.btOK.Click += new System.EventHandler(this.btOK_Click);
			// 
			// btCancel
			// 
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(287, 155);
			this.btCancel.Margin = new System.Windows.Forms.Padding(4);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(95, 28);
			this.btCancel.TabIndex = 3;
			this.btCancel.Text = "Отмена";
			this.btCancel.UseVisualStyleBackColor = true;
			// 
			// labText
			// 
			this.labText.AutoSize = true;
			this.labText.Location = new System.Drawing.Point(20, 31);
			this.labText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.labText.Name = "labText";
			this.labText.Size = new System.Drawing.Size(158, 16);
			this.labText.TabIndex = 0;
			this.labText.Text = "Введите новый пароль";
			// 
			// tbPassword
			// 
			this.tbPassword.Location = new System.Drawing.Point(22, 51);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(446, 22);
			this.tbPassword.TabIndex = 0;
			// 
			// tbRepeatPassword
			// 
			this.tbRepeatPassword.Location = new System.Drawing.Point(23, 102);
			this.tbRepeatPassword.Name = "tbRepeatPassword";
			this.tbRepeatPassword.PasswordChar = '*';
			this.tbRepeatPassword.Size = new System.Drawing.Size(446, 22);
			this.tbRepeatPassword.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(20, 83);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(130, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Повторите пароль";
			// 
			// PasswordNewForm
			// 
			this.AcceptButton = this.btOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(491, 211);
			this.Controls.Add(this.tbRepeatPassword);
			this.Controls.Add(this.tbPassword);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.labText);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.btOK);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PasswordNewForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Пароль";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        public System.Windows.Forms.TextBox tbPassword;
        public System.Windows.Forms.Label labText;
		public System.Windows.Forms.TextBox tbRepeatPassword;
		public System.Windows.Forms.Label label1;
    }
}