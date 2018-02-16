﻿using System;
using System.ComponentModel; 
using Xamarin.Forms.Internals;
using AView = Android.Views.View;

namespace Xamarin.Forms.Platform.Android
{
	internal static class BackgroundManager
	{
		public static void Init(IVisualElementRenderer renderer)
		{ 
			renderer.ElementPropertyChanged += OnElementPropertyChanged;
			renderer.ElementChanged += OnElementChanged;
		}

		static void UpdateBackgroundColor(AView Control, VisualElement Element, Color? color = null)
		{
			if (Element == null || Control == null)
				return;
			
			Control.SetBackgroundColor((color ?? Element.BackgroundColor).ToAndroid());
		}


		static void OnElementChanged(object sender, VisualElementChangedEventArgs e)
		{
			var reference = Guid.NewGuid().ToString();
			Performance.Start(reference);
			if (e.OldElement != null)
			{
				e.OldElement.PropertyChanged -= OnElementPropertyChanged;
			}

			if (e.NewElement != null)
			{
				var renderer = (sender as IVisualElementRenderer);
				e.NewElement.PropertyChanged += OnElementPropertyChanged;
				UpdateBackgroundColor(renderer?.View, renderer?.Element);
			}

			Performance.Stop(reference);
		}


		static void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
			{
				var renderer = (sender as IVisualElementRenderer);
				UpdateBackgroundColor(renderer?.View, renderer?.Element);
			}
		}
	}
}