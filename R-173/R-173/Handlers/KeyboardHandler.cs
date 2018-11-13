using R_173.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace R_173.Handlers
{
    public class KeyboardHandler
    {
        private readonly Dictionary<Key, Action<bool>> _onKeyDownActions;
        private RadioModel _currentRadioModel;
        private Key? _lastPressedKey;
        public event EventHandler<SharedResources.KeyEventArgs> OnKeyDown;

        public KeyboardHandler()
        {
            _onKeyDownActions = new Dictionary<Key, Action<bool>>
            {
                { Key.Space, value => _currentRadioModel.Sending.Value = value ? SwitcherState.Enabled : SwitcherState.Disabled },
                { Key.RightCtrl, value => _currentRadioModel.Tone.Value = value ? SwitcherState.Enabled : SwitcherState.Disabled },
                { Key.N, value => _currentRadioModel.Board.Value = value ? SwitcherState.Enabled : SwitcherState.Disabled },
                { Key.C, value => _currentRadioModel.Reset.Value = value ? SwitcherState.Enabled : SwitcherState.Disabled },
            };

            Enumerable.Range(0, 10)
                .Select(i => new { i, key = Key.NumPad0 + i })
                .ToList()
                .ForEach(n => 
                    _onKeyDownActions.Add(
                        n.key, 
                        value => _currentRadioModel.Numpad[n.i].Value = 
                            value ? SwitcherState.Enabled : SwitcherState.Disabled));

        }


        public void ActivateRadio(RadioModel model)
        {
            _currentRadioModel = model;
        }

        public void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown?.Invoke(this, new SharedResources.KeyEventArgs { Key = e.Key });
            e.Handled = true;
            //if (e.Key == _lastPressedKey)
            //    return;
            //_lastPressedKey = e.Key;
            //System.Diagnostics.Trace.WriteLine(e.Key);

            if (_currentRadioModel == null || !_onKeyDownActions.TryGetValue(e.Key, out var action))
                return;

            action(true);
        }

        public void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            _lastPressedKey = null;

            if (_currentRadioModel == null || !_onKeyDownActions.TryGetValue(e.Key, out var action))
                return;

            action(false);
        }
    }
}
