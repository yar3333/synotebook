using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SyNotebook
{
    public partial class NoteForm : Form
    {
	    List<RadioButton> imageButtons = new List<RadioButton>();

        public NoteForm(ImageList imageList)
        {
            InitializeComponent();
            
            for (var i=0;i<imageList.Images.Count;i++)
            {
				var y = i/8;
				var x = i%8;
				
				var cb = new RadioButton();
				cb.Appearance = Appearance.Button;
				cb.FlatStyle = FlatStyle.Flat;
				cb.FlatAppearance.CheckedBackColor = Color.White;
				cb.ImageList = imageList;
				cb.ImageIndex = i;
				cb.Left = 70 + x*28;
				cb.Top  = 98 + y*28;
				cb.Width = 25;
				cb.Height = 25;
				
				imageButtons.Add(cb);
				Controls.Add(cb);
            }
        }

        public int ImageIndex
        {
            get
            {
                for (var i=0;i<imageButtons.Count;i++)
                {
                    if (imageButtons[i].Checked) return i;
                }
                return 0;
            }
            set
            {
                imageButtons[value].Checked = true;
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}