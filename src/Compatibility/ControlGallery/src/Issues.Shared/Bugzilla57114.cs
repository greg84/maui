﻿using System;
using System.Diagnostics;

using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

#if UITEST
using Microsoft.Maui.Controls.Compatibility.UITests;
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Microsoft.Maui.Controls.ControlGallery.Issues
{
#if UITEST
	[Category(UITestCategories.Gestures)]
	[NUnit.Framework.Category(UITestCategories.UwpIgnore)]
	[NUnit.Framework.Category(Compatibility.UITests.UITestCategories.Bugzilla)]
#endif

	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 57114, "Forms gestures are not supported on UIViews that have native gestures", PlatformAffected.iOS)]
	public class Bugzilla57114 : TestContentPage
	{
		public static string _57114NativeGestureFiredMessage = "_57114NativeGestureFiredMessage";

		Label _results;
		bool _nativeGestureFired;
		bool _formsGestureFired;

		const string Testing = "Testing...";
		const string Success = "Success";
		const string ViewAutomationId = "_57114View";

		protected override void Init()
		{
			var instructions = new Label
			{
				Text = $"Tap the Aqua View below. If the label below changes from '{Testing}' to '{Success}', the test has passed."
			};

			_results = new Label { Text = Testing };

			var view = new _57114View
			{
				AutomationId = ViewAutomationId,
				HeightRequest = 200,
				WidthRequest = 200,
				BackgroundColor = Colors.Aqua,
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.Fill
			};

			var tap = new TapGestureRecognizer
			{
				Command = new Command(() =>
				{
					_formsGestureFired = true;
					UpdateResults();
				})
			};

			MessagingCenter.Subscribe<object>(this, _57114NativeGestureFiredMessage, NativeGestureFired);

			view.GestureRecognizers.Add(tap);

			var layout = new StackLayout()
			{
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.Fill,
				Children =
				{
					instructions, _results, view
				}
			};

			Content = layout;
		}

		void NativeGestureFired(object obj)
		{
			_nativeGestureFired = true;
			UpdateResults();
		}

		void UpdateResults()
		{
			if (_nativeGestureFired && _formsGestureFired)
			{
				_results.Text = Success;
			}
			else
			{
				_results.Text = Testing;
			}
		}

		[Preserve(AllMembers = true)]
		public class _57114View : View
		{
		}

#if UITEST
[Microsoft.Maui.Controls.Compatibility.UITests.FailsOnMauiAndroid]
		[Test]
		public void _57114BothTypesOfGesturesFire()
		{
			RunningApp.WaitForElement(Testing);
			RunningApp.Tap(ViewAutomationId);
			RunningApp.WaitForElement(Success);
		}
#endif
	}
}