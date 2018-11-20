﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;

namespace R_173.Views
{
    /// <summary>
    /// Interaction logic for Appointment.xaml
    /// </summary>
    public partial class Appointment : UserControl, ITabView
    {
        public Appointment()
        {
            InitializeComponent();
            var document = new XpsDocument(Properties.Resources.XpsDescriptionPath, FileAccess.Read);
            docViewer.Document = document.GetFixedDocumentSequence();
        }
    }
}
