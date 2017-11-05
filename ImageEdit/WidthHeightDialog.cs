using System;
using System.Windows.Forms;

namespace ImageEdit
{
    public partial class WidthHeightDialog : Form
    {
        public WidthHeightDialog(int defaultWidth, int defaultHeight)
        {
            InitializeComponent();
            WidthUpDown.Value = defaultWidth;
            HeightUpDown.Value = defaultHeight;
        }

        public int SelectedWidth => (int)WidthUpDown.Value;
        public int SelectedHeight => (int)HeightUpDown.Value;

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
