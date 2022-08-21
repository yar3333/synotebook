namespace SyNotebook
{
	partial class ParamForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParamForm));
			this.propFontDialog = new System.Windows.Forms.FontDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.propFontSample = new System.Windows.Forms.RichTextBox();
			this.btSelectPropFont = new System.Windows.Forms.Button();
			this.monoFontDialog = new System.Windows.Forms.FontDialog();
			this.btSelectMonoFont = new System.Windows.Forms.Button();
			this.monoFontSample = new System.Windows.Forms.RichTextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btOK = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// propFontDialog
			// 
			this.propFontDialog.ShowEffects = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(143, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Пропорциональный шрифт";
			// 
			// propFontSample
			// 
			this.propFontSample.Location = new System.Drawing.Point(16, 29);
			this.propFontSample.Name = "propFontSample";
			this.propFontSample.ReadOnly = true;
			this.propFontSample.Size = new System.Drawing.Size(223, 42);
			this.propFontSample.TabIndex = 1;
			this.propFontSample.Text = "AZ az АЯ ая\nПример шрифта";
			// 
			// btSelectPropFont
			// 
			this.btSelectPropFont.Location = new System.Drawing.Point(245, 28);
			this.btSelectPropFont.Name = "btSelectPropFont";
			this.btSelectPropFont.Size = new System.Drawing.Size(75, 23);
			this.btSelectPropFont.TabIndex = 2;
			this.btSelectPropFont.Text = "Выбрать";
			this.btSelectPropFont.UseVisualStyleBackColor = true;
			this.btSelectPropFont.Click += new System.EventHandler(this.btSelectPropFont_Click);
			// 
			// monoFontDialog
			// 
			this.monoFontDialog.FixedPitchOnly = true;
			this.monoFontDialog.ShowEffects = false;
			// 
			// btSelectMonoFont
			// 
			this.btSelectMonoFont.Location = new System.Drawing.Point(246, 102);
			this.btSelectMonoFont.Name = "btSelectMonoFont";
			this.btSelectMonoFont.Size = new System.Drawing.Size(75, 23);
			this.btSelectMonoFont.TabIndex = 5;
			this.btSelectMonoFont.Text = "Выбрать";
			this.btSelectMonoFont.UseVisualStyleBackColor = true;
			this.btSelectMonoFont.Click += new System.EventHandler(this.btSelectMonoFont_Click);
			// 
			// monoFontSample
			// 
			this.monoFontSample.Location = new System.Drawing.Point(17, 103);
			this.monoFontSample.Name = "monoFontSample";
			this.monoFontSample.ReadOnly = true;
			this.monoFontSample.Size = new System.Drawing.Size(223, 42);
			this.monoFontSample.TabIndex = 4;
			this.monoFontSample.Text = "AZ az АЯ ая\nПример шрифта";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(14, 87);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(122, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Моноширинный шрифт";
			// 
			// btOK
			// 
			this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOK.Location = new System.Drawing.Point(63, 175);
			this.btOK.Name = "btOK";
			this.btOK.Size = new System.Drawing.Size(84, 23);
			this.btOK.TabIndex = 6;
			this.btOK.Text = "ОК";
			this.btOK.UseVisualStyleBackColor = true;
			// 
			// btCancel
			// 
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(184, 175);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(86, 23);
			this.btCancel.TabIndex = 7;
			this.btCancel.Text = "Отмена";
			this.btCancel.UseVisualStyleBackColor = true;
			// 
			// ParamForm
			// 
			this.AcceptButton = this.btOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(335, 215);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.btOK);
			this.Controls.Add(this.btSelectMonoFont);
			this.Controls.Add(this.monoFontSample);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btSelectPropFont);
			this.Controls.Add(this.propFontSample);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ParamForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Настройки";
			this.Load += new System.EventHandler(this.ParamForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RichTextBox propFontSample;
		private System.Windows.Forms.Button btSelectPropFont;
		public System.Windows.Forms.FontDialog propFontDialog;
		public System.Windows.Forms.FontDialog monoFontDialog;
		private System.Windows.Forms.Button btSelectMonoFont;
		private System.Windows.Forms.RichTextBox monoFontSample;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btOK;
		private System.Windows.Forms.Button btCancel;
	}
}