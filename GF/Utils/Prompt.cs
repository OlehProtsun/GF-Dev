using System.Windows.Forms;

namespace GF.Utils
{
    /// <summary>Найпростіше вікно вводу одного рядка.</summary>
    internal static class Prompt
    {
        public static string Show(string text, string caption)
        {
            using (var frm = new Form
            {
                Width = 400,
                Height = 150,
                Text = caption,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false
            })
            {
                var lbl = new Label { Left = 20, Top = 20, AutoSize = true, Text = text };
                var txt = new TextBox { Left = 20, Top = 45, Width = 340 };
                var btn = new Button
                {
                    Left = 280,
                    Top = 75,
                    Width = 80,
                    Text = "OK",
                    DialogResult = DialogResult.OK
                };

                frm.Controls.AddRange(new Control[] { lbl, txt, btn });
                frm.AcceptButton = btn;

                return frm.ShowDialog() == DialogResult.OK ? txt.Text : string.Empty;
            }
        }
    }
}
