﻿using System;
using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Microsoft.Maui.Graphics;
using AbsoluteLayoutFlags = Microsoft.Maui.Layouts.AbsoluteLayoutFlags;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
using Microsoft.Maui.Controls.Compatibility.UITests;
#endif

namespace Microsoft.Maui.Controls.ControlGallery.Issues
{
#if UITEST
	[Category(UITestCategories.BoxView)]
	[Category(UITestCategories.InputTransparent)]
	[NUnit.Framework.Category(Compatibility.UITests.UITestCategories.Bugzilla)]
#endif

	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 39331, "[Android] BoxView Is InputTransparent Even When Set to False")]
	public class Bugzilla39331 : TestContentPage
	{
		View _busyBackground;
		Button _btnLogin;

		protected override void Init()
		{
			AbsoluteLayout layout = new AbsoluteLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			BackgroundColor = Color.FromUint(0xFFDBDBDB);

			_btnLogin = new Button
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,

				Text = "Press me",
				BackgroundColor = Color.FromUint(0xFF6E932D),
				TextColor = Colors.White,
			};
			_btnLogin.Clicked += BtnLogin_Clicked;
			layout.Children.Add(_btnLogin, new Rect(0.5f, 0.5f, 0.25f, 0.25f), AbsoluteLayoutFlags.All);

			_busyBackground = new BoxView
			{
				BackgroundColor = new Color(0, 0, 0, 0.5f),
				IsVisible = false,
				InputTransparent = false
			};

			// Bump up elevation on Android to cover FastRenderer Button
			((BoxView)_busyBackground).On<Android>().SetElevation(10f);

			layout.Children.Add(_busyBackground, new Rect(0, 0, 1, 1), AbsoluteLayoutFlags.SizeProportional);

			Content = layout;
		}

		void BtnLogin_Clicked(object sender, EventArgs e)
		{

			if (!_busyBackground.IsVisible)
			{
				_btnLogin.Text = "Blocked?";
				_busyBackground.IsVisible = true;
			}
			else
			{
				_btnLogin.Text = "Guess Not";
				_busyBackground.IsVisible = false;
			}
		}

#if UITEST
[Microsoft.Maui.Controls.Compatibility.UITests.FailsOnMauiAndroid]
		[Test]
		public void Bugzilla39331Test()
		{
			RunningApp.Tap (q => q.Marked ("Press me"));
			RunningApp.WaitForElement (q => q.Marked ("Blocked?"));
			RunningApp.Tap (q => q.Marked ("Blocked?"));
			RunningApp.WaitForNoElement (q => q.Marked ("Guess Not"));
		}
#endif
	}
}
