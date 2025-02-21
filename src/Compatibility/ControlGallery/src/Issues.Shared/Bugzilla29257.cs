﻿using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;
#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Microsoft.Maui.Controls.ControlGallery.Issues
{
	// Note that this test currently fails on UWP because of https://bugzilla.xamarin.com/show_bug.cgi?id=60478
#if UITEST
	[Category(Compatibility.UITests.UITestCategories.Bugzilla)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 29257, "CarouselPage.CurrentPage Does Not Work Properly When Used Inside a NavigationPage ")]
	public class Bugzilla29257 : TestContentPage
	{
		List<string> _menuItems = new List<string> {
			"Page 1", "Page 2", "Page 3", "Page 4", "Page 5"
		};

		ListView _menu;

		protected override void Init()
		{
			_menu = new ListView { ItemsSource = _menuItems };

			_menu.ItemSelected += PageSelected;

			Content = _menu;
		}

		async void PageSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var selection = e.SelectedItem as string;

			switch (selection)
			{
				case "Page 1":
					await Navigation.PushAsync(new TestPage(0));
					break;

				case "Page 2":
					await Navigation.PushAsync(new TestPage(1));
					break;

				case "Page 3":
					await Navigation.PushAsync(new TestPage(2));
					break;

				case "Page 4":
					await Navigation.PushAsync(new TestPage(3));
					break;

				case "Page 5":
					await Navigation.PushAsync(new TestPage(4));
					break;
			}
			_menu.SelectedItem = null;
		}

		internal class TestPage : CarouselPage
		{
			public TestPage()
			{
				Children.Add(new ContentPage { Content = new Label { Text = "This is page 1", BackgroundColor = Colors.Red } });
				Children.Add(new ContentPage { Content = new Label { Text = "This is page 2", BackgroundColor = Colors.Green } });
				Children.Add(new ContentPage { Content = new Label { Text = "This is page 3", BackgroundColor = Colors.Blue } });
				Children.Add(new ContentPage { Content = new Label { Text = "This is page 4", BackgroundColor = Colors.Pink } });
				Children.Add(new ContentPage { Content = new Label { Text = "This is page 5", BackgroundColor = Colors.Yellow } });

			}

			public TestPage(int page) : this()
			{
				CurrentPage = Children[page];
			}
		}

#if UITEST
		[Test]
		public void Bugzilla29257Test ()
		{
			RunningApp.Tap (q => q.Marked ("Page 1"));
#if __MACOS__
			System.Threading.Thread.Sleep(2000);
#endif
			RunningApp.WaitForElement (q => q.Marked ("This is page 1"));
			RunningApp.Back ();
#if __MACOS__
			System.Threading.Thread.Sleep(2000);
#endif
			RunningApp.Tap (q => q.Marked ("Page 2"));
#if __MACOS__
			System.Threading.Thread.Sleep(2000);
#endif
			RunningApp.WaitForElement (q => q.Marked ("This is page 2"));
			RunningApp.Back ();
			RunningApp.Tap (q => q.Marked ("Page 3"));
#if __MACOS__
			System.Threading.Thread.Sleep(2000);
#endif
			RunningApp.WaitForElement (q => q.Marked ("This is page 3"));
			RunningApp.Back ();
#if __MACOS__
			System.Threading.Thread.Sleep(2000);
#endif
			RunningApp.Tap (q => q.Marked ("Page 4"));
#if __MACOS__
			System.Threading.Thread.Sleep(2000);
#endif
			RunningApp.WaitForElement (q => q.Marked ("This is page 4"));
			RunningApp.Back ();
#if __MACOS__
			System.Threading.Thread.Sleep(2000);
#endif
			RunningApp.Tap (q => q.Marked ("Page 5"));
#if __MACOS__
			System.Threading.Thread.Sleep(2000);
#endif
			RunningApp.WaitForElement (q => q.Marked ("This is page 5"));
		}
#endif
	}
}
