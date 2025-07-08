using System.Windows.Forms;

namespace GF.UI
{
    /// <summary>
    /// Utility wrapper around <see cref="MessangeForm"/> providing simplified
    /// message and input dialogs.
    /// </summary>
    public static class MyMessageBox
    {
        /// <summary>
        /// Shows a message dialog.
        /// </summary>
        public static DialogResult Show(
            string text,
            string caption = "Message",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            IWin32Window owner = null)
        {
            DialogResult result;
            using (var dlg = new MessangeForm(text, buttons))
            {
                dlg.Text = caption;
                result = owner == null ? dlg.ShowDialog() : dlg.ShowDialog(owner);
            }
            return result;
        }

        /// <summary>
        /// Shows an input dialog and returns the entered value or <c>null</c>
        /// when cancelled.
        /// </summary>
        public static string Input(
            string placeholder = "",
            string caption = "Input",
            string defaultText = "",
            IWin32Window owner = null)
        {
            string userValue = null;
            using (var dlg = new MessangeForm(
                placeholder,
                MessageBoxButtons.OKCancel,
                askInput: true,
                defaultInput: defaultText))
            {
                dlg.Text = caption;
                DialogResult dr = owner == null ? dlg.ShowDialog() : dlg.ShowDialog(owner);
                if (dr == DialogResult.OK)
                    userValue = dlg.UserInput;
            }
            return userValue;
        }
    }
}
