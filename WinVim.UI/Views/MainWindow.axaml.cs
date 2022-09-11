using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using WinVim.BL;
using WinVim.BL.Common.Types;
using WinVim.BL.Windows;
using WinVim.UI.Views;

namespace WinVim.UI
{
    public partial class MainWindow : Window
    {
        static WinVim.BL.Windows.NativeFeatures.POINT point;
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos([In] ref WinVim.BL.Windows.NativeFeatures.POINT point);
        private Settings settings = Settings.GetInstance();

        private bool isSettingsShown = false;
        private SettingsWindow window = new SettingsWindow();
        public MainWindow()
        {
            InitializeComponent();
            settings.Controls = new List<WinVim.BL.Control> {
                new WinVim.BL.Control(Direction.GetDirection(Directions.TopLeft), Keys.Empty),
                new WinVim.BL.Control(Direction.GetDirection(Directions.Top), Keys.K),
                new WinVim.BL.Control(Direction.GetDirection(Directions.TopRight), Keys.Empty),
                new WinVim.BL.Control(Direction.GetDirection(Directions.Right), Keys.L),
                new WinVim.BL.Control(Direction.GetDirection(Directions.BottomRight), Keys.Empty),
                new WinVim.BL.Control(Direction.GetDirection(Directions.Bottom), Keys.J),
                new WinVim.BL.Control(Direction.GetDirection(Directions.BottomLeft), Keys.Empty),
                new WinVim.BL.Control(Direction.GetDirection(Directions.Left), Keys.H),
                new WinVim.BL.Control(Direction.GetDirection(Directions.None), Keys.B),
                new WinVim.BL.Control(Direction.GetDirection(Directions.None), Keys.N)
            };

            var arr = new Keys[] { Keys.LeftShift, Keys.LeftControl };
            settings.ToVimModeCombo = new Combination(
                arr,
                Mh_VimModeEnabled
            );
            
            Thread thread = new Thread(Update);
            thread.Start();
        }

        private void Update(object? obj) {
            var mh = new MessageHandler(Settings.GetInstance());
            var mb = new MessageBroker();

            mh.VimModeEnabled += Mh_VimModeEnabled;
            mh.MouseLeftClick += Mh_MouseLeftClick;
            mh.MouseRightClick += Mh_MouseRightClick;
            mh.MouseMove += Mh_MouseMove;

            mb.KeyDown += mh.KeyDown;
            mb.KeyUp += mh.KeyUp;
            while (true) {
                mb.ProcessMessages();
            }
        }

        private void Mh_MouseMove(Direction dir) {
            Console.WriteLine("Left");
            GetCursorPos(ref point);
            SetCursorPos(point.X + dir.dX * settings.SpeedX, point.Y - dir.dY * settings.SpeedY);
            Console.WriteLine(point.X + " " + point.Y);
            Console.WriteLine(dir);
        }

        private void Mh_MouseRightClick() {
            throw new NotImplementedException();
        }

        private void Mh_MouseLeftClick() {
            throw new NotImplementedException();
        }

        private void Mh_VimModeEnabled() {
            if (!settings.IsInVim) {
                Console.WriteLine("Enabled");
                settings.IsInVim = true;
                NativeFeatures.BlockInput(true);
            } else {
                Console.WriteLine("Disabled");
                settings.IsInVim = false;
                NativeFeatures.BlockInput(false);
            }
        }

        public void OnFocusLost(object sender, RoutedEventArgs e) {
            window?.Close();
        }

        public void OnSettingsEnter(object sender, PointerEventArgs e) {
            if (isSettingsShown) {
                window.Close();
                isSettingsShown = false;
                return;
            }

            window = new SettingsWindow();
            var curScreen = Screens.ScreenFromPoint(this.Position);
            if (curScreen == null) return;

            PixelPoint settingsWindowPoint;
            if(this.Position.Y < (curScreen.Bounds.Height / 2)) {
                settingsWindowPoint = new PixelPoint(this.Position.X, this.Position.Y + 40);
            } else {
                settingsWindowPoint = new PixelPoint(this.Position.X, this.Position.Y - 160);
            }

            window.Position = settingsWindowPoint;
            window.Show();
            isSettingsShown = true;
        }

        public void MouseLeftClickBox_KeyDown(object? sender, KeyEventArgs e) {

        }

        public void OnFocus(object sender, PointerEventArgs e) {
        }
    }
}