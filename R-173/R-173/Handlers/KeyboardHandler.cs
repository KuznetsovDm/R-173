using R_173.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace R_173.Handlers
{
    public class KeyboardHandler
    {
        private readonly Dictionary<Key, Action> _onKeyDownActions;
        private readonly Dictionary<Key, Action> _onKeyUpActions;
        private RadioModel _currentRadioModel;
        private Key? _lastPressedKey;

        public KeyboardHandler(Window _mainWindow)
        {
            _onKeyDownActions = new Dictionary<Key, Action>
            {
                { Key.Space, () => _currentRadioModel.Tone.Value = SwitcherState.Enabled }
            };
            _onKeyUpActions = new Dictionary<Key, Action>
            {
                { Key.Space, () => _currentRadioModel.Tone.Value = SwitcherState.Disabled }
            };

            _mainWindow.PreviewKeyDown += OnPreviewKeyDown;
            _mainWindow.PreviewKeyUp += OnPreviewKeyUp;
        }


        public void ActivateRadio(RadioModel model)
        {
            _currentRadioModel = model;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (e.Key == _lastPressedKey)
                return;
            _lastPressedKey = e.Key;
            System.Diagnostics.Trace.Write("Down: ");
            System.Diagnostics.Trace.WriteLine(e.Key);

            if (_currentRadioModel == null || !_onKeyDownActions.TryGetValue(e.Key, out var action))
                return;

            action();
        }

        private void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            _lastPressedKey = null;
            System.Diagnostics.Trace.Write("Up: ");
            System.Diagnostics.Trace.WriteLine(e.Key);

            if (_currentRadioModel == null || !_onKeyDownActions.TryGetValue(e.Key, out var action))
                return;

            action();
        }
    }
}
