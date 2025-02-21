﻿using System.Collections.Generic;
using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Microsoft.Maui.Controls.ControlGallery.Issues
{
#if UITEST
	[NUnit.Framework.Category(Compatibility.UITests.UITestCategories.Bugzilla)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 60563, "ActivityIndicator in ListView causes SIGSEGV crash in iOS 8", PlatformAffected.iOS)]
	public class Bugzilla60563 : TestNavigationPage
	{
		const string btnGoToList = "btnGoToList";
		const string spinner = "spinner";

		protected override void Init()
		{
			Navigation.PushAsync(new NavigationPage(new StartPage()));
		}

		[Preserve(AllMembers = true)]
		class ListPage : ContentPage
		{
			public ListPage()
			{
				Title = "List";
				Content = new ListView
				{
					HasUnevenRows = false,
					RowHeight = 50,
					ItemTemplate = new DataTemplate(() => { return new SpinnerViewCell(); }),
					ItemsSource = new List<int> { 1, 2, 3, 4, 5 },
				};
			}
		}

		[Preserve(AllMembers = true)]
		class SpinnerViewCell : ViewCell
		{
			public SpinnerViewCell()
			{
				var indicator = new ActivityIndicator
				{
					IsRunning = true,
					AutomationId = spinner
				};
				var layout = new Compatibility.RelativeLayout();
				layout.Children.Add(indicator, x: () => 0, y: () => 0);
				View = indicator;
			}
		}

		[Preserve(AllMembers = true)]
		class StartPage : ContentPage
		{
			public StartPage()
			{
				var button = new Button
				{
					Text = "Go To List",
					BackgroundColor = Colors.Beige,
					HeightRequest = 40,
					WidthRequest = 100,
					VerticalOptions = LayoutOptions.Center,
					HorizontalOptions = LayoutOptions.Center,
					AutomationId = btnGoToList
				};
				button.Clicked += (sender, e) => Navigation.PushAsync(new ListPage());

				Title = "Home";
				Content = new StackLayout { Children = { new Label { Text = "Click the button to go to a ListView with an ActivityIndicator, then go back to this page. If the app does not crash, this test has passed." }, button } };
			}
		}

#if UITEST && __IOS__
		[Test]
		public void Bugzilla60563Test()
		{
			RunningApp.WaitForElement(q => q.Marked(btnGoToList));
			RunningApp.Tap(q => q.Marked(btnGoToList));
			RunningApp.WaitForElement(q => q.Marked(spinner));
			RunningApp.Back();
			RunningApp.WaitForElement(q => q.Marked(btnGoToList));
			RunningApp.Tap(q => q.Marked(btnGoToList));
			RunningApp.WaitForElement(q => q.Marked(spinner));
		}
#endif
	}
}