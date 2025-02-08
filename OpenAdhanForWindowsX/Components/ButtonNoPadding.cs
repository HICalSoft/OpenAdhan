using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OpenAdhanForWindowsX.Components
{
    public class ButtonNoPadding : Button
    {
        private string _textCurrent;

        private string _Text;
        [Category("Appearance")]
        public override string Text
        {
            get { return _Text; }
            set
            {
                if (value != _Text)
                {
                    _Text = value;
                    Invalidate();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _textCurrent = Text;
            _Text = string.Empty;
            base.OnPaint(e);
            _Text = _textCurrent;

            using (var brush = new SolidBrush(ForeColor))
            {
                using (var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    e.Graphics.DrawString(Text, Font, brush, Rectangle.Inflate(ClientRectangle, -2, -2), stringFormat);
                }
            }

        }

    }
}
