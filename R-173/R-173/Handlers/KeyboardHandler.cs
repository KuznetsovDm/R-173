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
                { Key.Space, () => _currentRadioModel.Tone.Value = SwitcherState.Enabled },
                { Key.NumPad0, () => _currentRadioModel.Frequency.Value = _currentRadioModel.Frequency.Value * 10 + 0 },
                { Key.NumPad1, () => _currentRadioModel.Frequency.Value = _currentRadioModel.Frequency.Value * 10 + 1 },
                { Key.NumPad2, () => _currentRadioModel.Frequency.Value = _currentRadioModel.Frequency.Value * 10 + 2 },
                { Key.NumPad3, () => _currentRadioModel.Frequency.Value = _currentRadioModel.Frequency.Value * 10 + 3 },
                { Key.NumPad4, () => _currentRadioModel.Frequency.Value = _currentRadioModel.Frequency.Value * 10 + 4 },
                { Key.NumPad5, () => _currentRadioModel.Frequency.Value = _currentRadioModel.Frequency.Value * 10 + 5 },
                { Key.NumPad6, () => _currentRadioModel.Frequency.Value = _currentRadioModel.Frequency.Value * 10 + 6 },
                { Key.NumPad7, () => _currentRadioModel.Frequency.Value = _currentRadioModel.Frequency.Value * 10 + 7 },
                { Key.NumPad8, () => _currentRadioModel.Frequency.Value = _currentRadioModel.Frequency.Value * 10 + 8 },
                { Key.NumPad9, () => _currentRadioModel.Frequency.Value = _currentRadioModel.Frequency.Value * 10 + 9 },
            };
            _onKeyUpActions = new Dictionary<Key, Action>
            {
                { Key.Space, () => _currentRadioModel.Tone.Value = SwitcherState.Disabled },
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
