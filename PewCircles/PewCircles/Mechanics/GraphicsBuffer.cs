using System;
using System.Drawing;
using System.Windows.Forms;

namespace PewCircles
{
    public class GraphicsBuffer
    {
        private BufferedGraphics _buffer;
        private Control _control;
        private Action<Graphics> _renderAction;

        /// <summary>
        /// Make a constructor out of this if it does not need to be instaniated by IoC.
        /// </summary>
        public void Initialize(Control control, Action<Graphics> renderAction)
        {
            _control = control;
            _renderAction = renderAction;
            CreateBuffer();
        }

        public void CreateBuffer()
        {
            var graphicsContext = BufferedGraphicsManager.Current;
            _buffer = graphicsContext.Allocate(_control.CreateGraphics(), _control.DisplayRectangle);
        }

        public void Render(Graphics finalCanvas)
        {
            if (_renderAction != null)
            {
                _buffer.Graphics.Clear(Color.DarkSlateBlue);
                _renderAction(_buffer.Graphics);
            }

            _buffer.Render(finalCanvas);
        }
    }
}
