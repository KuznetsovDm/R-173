﻿using R_173.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace R_173.Handlers
{
    public class KeyboardHandler
    {
        private readonly Dictionary<Key, Action<bool>> _onKeyDownActions;
        private RadioModel _currentRadioModel;
        private Key? _lastPressedKey;

        public KeyboardHandler(MainWindow _mainWindow)
        {
            _onKeyDownActions = new Dictionary<Key, Action<bool>>
            {
                { Key.Space, value => _currentRadioModel.Sending.Value = value ? SwitcherState.Enabled : SwitcherState.Disabled },
                { Key.RightCtrl, value => _currentRadioModel.Tone.Value = value ? SwitcherState.Enabled : SwitcherState.Disabled },
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
            //System.Diagnostics.Trace.WriteLine(e.Key);

            if (_currentRadioModel == null || !_onKeyDownActions.TryGetValue(e.Key, out var action))
                return;

            action(true);
        }

        private void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            _lastPressedKey = null;

            if (_currentRadioModel == null || !_onKeyDownActions.TryGetValue(e.Key, out var action))
                return;

            action(false);
        }
    }
}