﻿using System;

namespace R_173.Interfaces
{
    interface IMessageBox
    {
        void ShowDialog(Action ok, Action cancel, string message, string okText, string cancelText);
    }
}