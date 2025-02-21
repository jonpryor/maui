﻿using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
using Microsoft.Maui.Controls.Compatibility.UITests;
#endif

namespace Microsoft.Maui.Controls.ControlGallery.Issues
{
#if UITEST
	[Category(UITestCategories.Frame)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 11132, "[Bug] [iOS] UpdateClip throws NullReferenceException when the Name of the Mask of the Layer is null", PlatformAffected.iOS)]
	public class Issue11132 : TestContentPage
	{
		const string InstructionsId = "instructions";

		public Issue11132()
		{

		}

		protected override void Init()
		{
			Title = "Issue 11132";

			var layout = new StackLayout();

			var instructions = new Label
			{
				AutomationId = InstructionsId,
				Padding = 12,
				BackgroundColor = Colors.Black,
				TextColor = Colors.White,
				Text = "If the test works without exceptions (an orange rectangle is rendered), the test has passed."
			};

			var issue11132Control = new Issue11132Control
			{
				HeightRequest = 100
			};

			layout.Children.Add(instructions);
			layout.Children.Add(issue11132Control);

			Content = layout;
		}

#if UITEST
		[Test]
		[Description("Verify that can use a CustomRenderer overriding the iOS View Layer properties")]
		public void Issue11132CustomRendererLayerAndClip()
		{
			RunningApp.WaitForElement(InstructionsId);
			RunningApp.Screenshot("No crash");
		}
#endif

	}

	[Preserve(AllMembers = true)]
	public class Issue11132Control : View
	{

	}
}