﻿using System;
using System.Threading.Tasks;
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
	[NUnit.Framework.Category(Compatibility.UITests.UITestCategories.Github5000)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 3390, "Crash/incorrect behavior with corner radius 5", PlatformAffected.All)]
	public class Issue3390 : TestContentPage
	{
		protected override void Init()
		{
			var btn = new Button()
			{
				Text = "Click me",
				WidthRequest = 50,
				HeightRequest = 50,
				CornerRadius = 25,
				//BackgroundColor = Colors.Accent
			};

			btn.Command = new Command(async () =>
			{
				btn.CornerRadius = 5;
				await Task.Delay(200);
				btn.Text = btn.CornerRadius == 5 ? "Success" : "Failed";
			});

			Content = new StackLayout()
			{
				Children =
				{
					btn,
					new Label
					{
						Text = $"When you click on the button, it will change the corner radius.{Environment.NewLine}" +
							$"[UWP] Application does not crash.{Environment.NewLine}" +
							$"[All] Сorner radius must be equal 5."
					}
				}
			};
		}

#if UITEST
		[Test]
		public void Issue3390Test()
		{
			RunningApp.Tap("Click me");
			RunningApp.WaitForElement("Success");
		}
#endif
	}
}
