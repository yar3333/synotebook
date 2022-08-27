using System;
using System.Windows.Forms;

namespace SyNotebook
{
	public partial class ParamForm : Form
	{
		public ParamForm()
		{
			InitializeComponent();
		}
		
		void updateSamples()
		{
			propFontSample.SelectAll();
			propFontSample.SelectionFont = propFontDialog.Font;
			propFontSample.DeselectAll();

			monoFontSample.SelectAll();
			monoFontSample.SelectionFont = monoFontDialog.Font;
			monoFontSample.DeselectAll();
		}

		private void btSelectPropFont_Click(object sender, EventArgs e)
		{
			if (propFontDialog.ShowDialog(this)==DialogResult.OK)
			{
				updateSamples();
			}
		}

		private void btSelectMonoFont_Click(object sender, EventArgs e)
		{
			if (monoFontDialog.ShowDialog(this)==DialogResult.OK)
			{
				updateSamples();
			}
		}

		private void ParamForm_Load(object sender, EventArgs e)
		{
 			updateSamples();
		}
	}
}