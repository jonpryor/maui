﻿using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.ControlGallery.Issues
{
#if UITEST
	[NUnit.Framework.Category(Compatibility.UITests.UITestCategories.Github5000)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 2598, "Replacing page in CarouselPage does not work the first time", PlatformAffected.All)]
	internal class GitHub2598 : TestCarouselPage
	{
		private ContentPage CreatePage(string labelText, Color bg)
		{
			return new ContentPage
			{
				Content = new Label { Text = labelText },
				BackgroundColor = bg
			};
		}

		protected override void Init()
		{
			var firstPage = new ContentPage()
			{
				Content = new StackLayout()
				{
					Children =
					{
						new Button()
						{
							Text = "Replace Page2. It should be green",
							TextColor = Colors.White,
							BackgroundColor = Colors.Green,
							Command =  new Command(() =>
							{
								var newPage = CreatePage("This is the new Page 2", Colors.Green);
								Children[1] = newPage;
							})
						}
					}
				},
				BackgroundColor = Colors.Blue
			};
			Children.Add(firstPage);

			var secondPage = CreatePage("Page 2", Colors.Red);
			Children.Add(secondPage);

			CurrentPage = firstPage;
		}
	}
}