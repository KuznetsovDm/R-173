using System;
using System.Collections.Generic;
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

namespace R_173.Views
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : UserControl
    {
        public Test()
        {
            InitializeComponent();
        }
    }

    public class Task
    {
        public int Number { get; set; }
        public string Description { get; set; }
        public Orientation Orientation { get; set; }
    }

    public class Line
    {
        public Orientation Orientation { get; set; }
    }
}
