using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Win32.Interop;
using System;
using WinVim.BL;
using WinVim.BL.Common.Types;
using WinVim.UI.Models;
using WinVim.UI.ViewModels;

namespace WinVim.UI.Views {
    public partial class SettingsWindow : Window {
        public SettingsWindow() {
            InitializeComponent();
            DataContext = new SettingsViewModel(Settings.GetInstance());
        }

        public void OnFocusLost(object? sender, EventArgs e) {
            this.Close();
        }

        public void MouseLeftClickBox_KeyDown(object sender, KeyEventArgs e) {
            LeftClickComboTextBox.Text = e.Key.ToString();
            System.Diagnostics.Debug.WriteLine(KeyConverter.VirtualKeyFromKey(e.Key));
        }

        public void MouseLeftClickBox_Click(object sender, GotFocusEventArgs e) {
            Settings.GetInstance().MouseLeft = Keys.B;
        }

        public void MouseRightClickBox_KeyDown(object sender, KeyEventArgs e) {
            LeftClickComboTextBox.Text = e.Key.ToString();
            System.Diagnostics.Debug.WriteLine(KeyConverter.VirtualKeyFromKey(e.Key));
        }

        public void MouseRightClickBox_Click(object sender, GotFocusEventArgs e) {
            Settings.GetInstance().MouseLeft = Keys.N;
        }
    }
}
