using System.Windows;
using System.Windows.Controls;
using R_173.Handlers;
using Unity;

namespace R_173.SharedResources
{
	public class MessageBehavior
	{
		public static readonly DependencyProperty IsEnabledProperty =
			DependencyProperty.RegisterAttached(
				"IsEnabled",
				typeof(bool),
				typeof(MessageBehavior),
				new FrameworkPropertyMetadata(
					false,
					FrameworkPropertyMetadataOptions.Inherits,
					OnIsEnabledChanged));


		public static bool GetIsEnabled(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsEnabledProperty);
		}

		public static void SetIsEnabled(DependencyObject obj, bool value)
		{
			obj.SetValue(IsEnabledProperty, value);
		}


		private static void OnIsEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var keyboardHandler = App.ServiceCollection.Resolve<KeyboardHandler>();

			switch (obj)
			{
				case Button affirmativeButton when affirmativeButton.Name == "PART_AffirmativeButton":
					keyboardHandler.AffirmativeButton = args.NewValue is true
						? affirmativeButton
						: null;
					break;
				case Button negativeButton when negativeButton.Name == "PART_NegativeButton":
					keyboardHandler.NegativeButton = args.NewValue is true
						? negativeButton
						: null;
					break;
			}
		}
	}
}
