﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

#if UITEST
using NUnit.Framework;
using Xamarin.UITest;
#endif

namespace Microsoft.Maui.Controls.ControlGallery.Issues
{
#if UITEST
	[NUnit.Framework.Category(Compatibility.UITests.UITestCategories.UwpIgnore)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.None, 0, "Carousel Async Add Page Issue", PlatformAffected.All, NavigationBehavior.PushModalAsync)]
	internal class CarouselAsync : TestCarouselPage
	{
		protected override void Init()
		{
			Children.Add(new ContentPage
			{
				BackgroundColor = Colors.Red,
				Content = new Label
				{
					Text = "Page One"
				}
			});
			Children.Add(new ContentPage
			{
				BackgroundColor = Colors.Green,
				Content = new Label
				{
					Text = "Page Two"
				}
			});
			Update(this);
		}

		static void Update(CarouselPage page)
		{
			Device.StartTimer(TimeSpan.FromSeconds(1), () =>
			{
				page.Children.Remove(page.Children.Skip(1).First() as ContentPage);
				Device.StartTimer(TimeSpan.FromSeconds(1), () =>
				{
					page.Children.Add(new ContentPage
					{
						BackgroundColor = Colors.Blue,
						Content = new Label
						{
							Text = "Page Two - Added"
						}
					});
					page.Children.Add(new ContentPage
					{
						BackgroundColor = Colors.Gray,
						Content = new Label
						{
							Text = "Page Three - Added"
						}
					});
					return false;
				});
				return false;
			});
		}

#if UITEST
		[Test]
		[Description ("All elements renderered")]
		public void CarouselAsyncTestsAllElementsPresent ()
		{
			RunningApp.WaitForElement (q => q.Marked ("Page One"));
			RunningApp.Screenshot ("All elements present");
		}

		[Test]
		[Description ("Async Pages inserted into a CarouselPage")]
		public void CarouselAsyncTestsAllPagesExistAfterAsyncAdding ()
		{
			RunningApp.WaitForElement (q => q.Marked ("Page One"));

			var window = RunningApp.Query (q => q.Raw ("*").Index (0));
			var width = window [0].Rect.Width;
			var height = window [0].Rect.Height;

			System.Threading.Thread.Sleep (3000);

			// TODO Implement swipe left and swipe right
			//App.DragFromTo (width - 10.0f, height / 2.0f, 10.0f, height / 2.0f);
			//App.WaitForElement (q => q.Marked ("Page Two - Added"));
			//App.Screenshot ("At Page 2");

			//Thread.Sleep (3000);

			//App.DragFromTo (width - 10.0f, height / 2.0f, 10.0f, height / 2.0f);
			//App.WaitForElement (q => q.Marked ("Page Three - Added"));
			//App.Screenshot ("At Page 3");
		}

/*******************************************************/
/**************** Landscape tests **********************/
/*******************************************************/

		[Test]
		[Description ("All elements renderered - landscape")]
		public void CarouselAsyncTestsAllElementsPresentLandscape ()
		{
			RunningApp.SetOrientationLandscape ();
			CarouselAsyncTestsAllElementsPresent ();
			RunningApp.SetOrientationPortrait ();
		}

		[Test]
		[Description ("Async Pages inserted into a CarouselPage - landscape")]
		public void CarouselAsyncTestsAllPagesExistAfterAsyncAddingLandscape ()
		{
			RunningApp.SetOrientationLandscape ();
			CarouselAsyncTestsAllPagesExistAfterAsyncAdding ();
			RunningApp.SetOrientationPortrait ();
		}
#endif
	}
}
