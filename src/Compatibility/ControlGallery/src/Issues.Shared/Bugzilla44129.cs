﻿using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Internals;

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
	[Issue(IssueTracker.Bugzilla, 44129, "Crash when adding tabbed page after removing all pages using DataTemplates")]
	public class Bugzilla44129 : TestTabbedPage // or TestFlyoutPage, etc ...
	{
		protected override void Init()
		{
			// Initialize ui here instead of ctor
			var viewModels = new ObservableCollection<string>();
			viewModels.Add("First");
			viewModels.Add("Second");
			var template = new DataTemplate(() =>
			{
				ContentPage page = new ContentPage();
				var crashMe = new Button { Text = "Crash Me" };
				crashMe.Clicked += (sender, args) =>
				{
					viewModels.Clear();
					viewModels.Add("Third");
				};

				page.Content = crashMe;
				page.SetBinding(ContentPage.TitleProperty, ".");

				return page;
			});

			ItemTemplate = template;
			ItemsSource = viewModels;
		}

#if UITEST
		[Test]
		public void Issue44129Test()
		{
			RunningApp.Screenshot("I am at Issue 1");
			RunningApp.WaitForElement(q => q.Marked("First"));
			RunningApp.Screenshot("I see the Label");
			RunningApp.Tap(q => q.Marked("Second"));
			RunningApp.Tap(q => q.Marked("Crash Me"));
		}
#endif
	}
}