using System;
using System.Windows;
using System.Windows.Controls;

namespace R_173.Views.TrainingSteps
{
    class MouseBehavior
    {
        public static readonly DependencyProperty IsBlockProperty = DependencyProperty.RegisterAttached(
            "IsBlock",
            typeof(bool),
            typeof(MouseBehavior),
            new FrameworkPropertyMetadata(false));

        public static void SetIsBlock(UIElement element, bool value)
        {
            element.SetValue(IsBlockProperty, value);
        }

        public static bool GetIsBlock(UIElement element)
        {
            return (bool)element.GetValue(IsBlockProperty);
        }


        public MouseBehavior()
        {

        }

        public string GetToolTip => "hello";
    }
}
