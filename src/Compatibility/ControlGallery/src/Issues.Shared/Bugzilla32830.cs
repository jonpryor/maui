﻿using System;
using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Microsoft.Maui.Controls.ControlGallery.Issues
{
	// Uses a custom renderer on Android to override SetupPageTransition.
	// While these transitions are often desired, they can appear to cause the "flash"
	// at the top and bottom of the screen that could be confused with the bug we're fixing.
	public class NoFlashTestNavigationPage : TestNavigationPage
	{
		protected override void Init()
		{

		}
	}
#if UITEST
	[Category(Compatibility.UITests.UITestCategories.Bugzilla)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 32830, "Hiding navigation bar causes layouts to shift during navigation", PlatformAffected.iOS)]
	public class Bugzilla32830 : NoFlashTestNavigationPage
	{
		const string Button1 = "button1";
		const string Button2 = "button2";
		const string BottomLabel = "I am visible at the bottom of the page";

		[Preserve(AllMembers = true)]
		class Page1 : ContentPage
		{
			public Page1()
			{
				Title = "Page 1";
				BackgroundColor = Colors.Gray;

				var relativeLayout = new Compatibility.RelativeLayout { };

				relativeLayout.Children.Add(new StackLayout
				{
					VerticalOptions = LayoutOptions.Center,
					Children = {
						new Label {
							HorizontalTextAlignment = TextAlignment.Center,
							Text = "Page 1",
							TextColor = Colors.White
						},
						new Button {
							Text = "Go to page 2",
							Command = new Command(async () => await Navigation.PushAsync(new Page2())),
							AutomationId = Button1,
							TextColor = Colors.White
						},
						new Button {
							Text = "Toggle Nav Bar",
							Command = new Command(() => NavigationPage.SetHasNavigationBar(this, !NavigationPage.GetHasNavigationBar(this))),
							TextColor = Colors.White
						}
					}
				}, yConstraint: Compatibility.Constraint.RelativeToParent(parent => { return parent.Y; }));

				relativeLayout.Children.Add(new Label
				{
					Text = BottomLabel,
					TextColor = Colors.White
				}, yConstraint: Compatibility.Constraint.RelativeToParent(parent => { return parent.Height - 30; }));

				Content = relativeLayout;

				NavigationPage.SetHasNavigationBar(this, false);
			}
		}

		[Preserve(AllMembers = true)]
		class Page2 : ContentPage
		{
			public Page2()
			{
				Title = "Page 2";
				BackgroundColor = Colors.Gray;
				var relativeLayout = new Compatibility.RelativeLayout { };
				relativeLayout.Children.Add(new StackLayout
				{
					VerticalOptions = LayoutOptions.Center,
					Children = {
							new Label {
								HorizontalTextAlignment = TextAlignment.Center,
								Text = "Page 2",
									TextColor = Colors.White
							},
							new Button {
								Text = "Go to tabs",
								AutomationId = Button2,
								Command = new Command(async () => await Navigation.PushAsync(new MyTabs())),
								TextColor = Colors.White
							},
							new Button {
								Text = "Toggle Nav Bar",
								Command = new Command(() => NavigationPage.SetHasNavigationBar(this, !NavigationPage.GetHasNavigationBar(this))),
								TextColor = Colors.White
							}
						}
				}, yConstraint: Compatibility.Constraint.RelativeToParent(parent => { return parent.Y; }));

				relativeLayout.Children.Add(new Label
				{
					Text = BottomLabel,
					TextColor = Colors.White
				}, yConstraint: Compatibility.Constraint.RelativeToParent(parent => { return parent.Height - 30; }));

				Content = relativeLayout;
			}
		}

		class MyTabs : TabbedPage
		{
			public MyTabs()
			{
				Children.Add(new NavigationPage(new Page1()));
				Children.Add(new Page2());
			}
		}

		protected override void Init()
		{
			Navigation.PushAsync(new Page1());

		}

#if UITEST
		[Test]
		public void Bugzilla32830Test()
		{
			RunningApp.WaitForElement(q => q.Marked(BottomLabel));
			RunningApp.WaitForElement(q => q.Marked(Button1));
			RunningApp.Tap(q => q.Marked(Button1));
			RunningApp.WaitForElement(q => q.Marked(Button2));
			RunningApp.Tap(q => q.Marked(Button2));
			RunningApp.WaitForElement(q => q.Marked(BottomLabel));
		}
#endif
	}
}