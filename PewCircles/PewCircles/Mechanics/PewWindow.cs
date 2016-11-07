using HelloGame.Client;
using PewCircles.Mechanics;
using System;
using System.Threading;
using System.Windows.Forms;

namespace PewCircles
{
    public partial class PewWindow : Form
    {
        private bool _isVisible = true;
        private readonly GraphicsBuffer _buffer;
        private Action<TimeSpan> _updateSutffAction;
        private InputManager _inputManager;
        private TimeSource _timeSource;
        private ClientNetwork _clientNetwork;

        public PewWindow(GraphicsBuffer buffer, Pewness pewness, InputManager inputManager, TimeSource timeSource, ClientNetwork clientNetwork)
        {
            InitializeComponent();
            Show();

            ResizeEnd += new EventHandler(FormResizeEnd);
            SizeChanged += new EventHandler(FormSizeChanged);
            FormClosed += new FormClosedEventHandler(FormFormClosed);

            _buffer = buffer;
            _buffer.Initialize(this, pewness.Render);

            _updateSutffAction = pewness.Update;

            _inputManager = inputManager;
            _inputManager.SetMousePointFunction(() => PointToClient(Cursor.Position));

            _timeSource = timeSource;

            SetStyle(ControlStyles.StandardDoubleClick, false);

            _clientNetwork = clientNetwork;
        }

        public void PewMadness()
        {
            _clientNetwork.StartConnection("localhost", 9999, "Client", new CancellationTokenSource());

            var time = new TimeCounter(_timeSource);
            while (_isVisible)
            {
                _updateSutffAction(time.GetTimeSinceLastCall());
                _buffer.Render(CreateGraphics());
                Application.DoEvents();
            }
        }

        private void FormFormClosed(object sender, EventArgs e)
        {
            _isVisible = false;
        }

        private void FormResizeEnd(object sender, EventArgs e)
        {
            _buffer.CreateBuffer();
        }

        private void FormSizeChanged(object sender, EventArgs e)
        {
            _buffer.CreateBuffer();
        }

        private void PewWindow_KeyDown(object sender, KeyEventArgs e)
        {
            _inputManager.KeyPressed(e.KeyCode);
        }

        private void PewWindow_KeyUp(object sender, KeyEventArgs e)
        {
            _inputManager.KeyReleased(e.KeyCode);
        }

        private void PewWindow_Click(object sender, EventArgs e)
        {
            _inputManager.MouseClicked((MouseEventArgs)e);
        }
    }
}

