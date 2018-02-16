using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform;

namespace Xamarin.Forms
{
	[RenderWith(typeof(_ImageRenderer))]
	public class Image : View, IImageController, IElementConfiguration<Image>, IImageElement
	{
		public static readonly BindableProperty SourceProperty = BindableProperty.Create("Source", typeof(ImageSource), typeof(Image), default(ImageSource),
			propertyChanging: OnSourcePropertyChanging,
			propertyChanged: OnSourcePropertyChanged);

		public static readonly BindableProperty AspectProperty = BindableProperty.Create("Aspect", typeof(Aspect), typeof(Image), Aspect.AspectFit);

		public static readonly BindableProperty IsOpaqueProperty = BindableProperty.Create("IsOpaque", typeof(bool), typeof(Image), false);

		internal static readonly BindablePropertyKey IsLoadingPropertyKey = BindableProperty.CreateReadOnly("IsLoading", typeof(bool), typeof(Image), default(bool));

		public static readonly BindableProperty IsLoadingProperty = IsLoadingPropertyKey.BindableProperty;

		readonly Lazy<PlatformConfigurationRegistry<Image>> _platformConfigurationRegistry;

		public Image()
		{
			_platformConfigurationRegistry = new Lazy<PlatformConfigurationRegistry<Image>>(() => new PlatformConfigurationRegistry<Image>(this));
			ImageElement.Initialize(this);
		}
		 

		public Aspect Aspect
		{
			get { return (Aspect)GetValue(AspectProperty); }
			set { SetValue(AspectProperty, value); }
		}

		public bool IsLoading
		{
			get { return (bool)GetValue(IsLoadingProperty); }
		}

		public bool IsOpaque
		{
			get { return (bool)GetValue(IsOpaqueProperty); }
			set { SetValue(IsOpaqueProperty, value); }
		}

		[TypeConverter(typeof(ImageSourceConverter))]
		public ImageSource Source
		{
			get { return (ImageSource)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		protected override void OnBindingContextChanged()
		{
			ImageElement.OnBindingContextChanged(this, this);
			base.OnBindingContextChanged();
		}

		[Obsolete("OnSizeRequest is obsolete as of version 2.2.0. Please use OnMeasure instead.")]
		protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
		{
			SizeRequest desiredSize = base.OnSizeRequest(double.PositiveInfinity, double.PositiveInfinity);
			return ImageElement.Measure(this, desiredSize, widthConstraint, heightConstraint);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SetIsLoading(bool isLoading)
		{
			SetValue(IsLoadingPropertyKey, isLoading);
		}

		public IPlatformElementConfiguration<T, Image> On<T>() where T : IConfigPlatform
		{
			return _platformConfigurationRegistry.Value.On<T>();
		}

		public event EventHandler<BindableValueChangedEventArgs> ImageSourceChanged;
		public event EventHandler<BindableValueChangedEventArgs> ImageSourceChanging;
		public event EventHandler ImageSourcesSourceChanged;

		void SourcePropertyChanged(ImageSource newValue) =>
			newValue.SourceChanged += onSourceChanged;

		void SourcePropertyChanging(ImageSource oldValue) =>
			oldValue.SourceChanged -= onSourceChanged;

		private void onSourceChanged(object sender, EventArgs e) =>
			ImageSourcesSourceChanged?.Invoke(this, EventArgs.Empty);

		static void OnSourcePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			var image = ((Image)bindable);

			if (newvalue != null)
			{
				image.SourcePropertyChanged((ImageSource)newvalue);
			}
			image.ImageSourceChanged?.Invoke(bindable, new BindableValueChangedEventArgs(bindable, oldvalue, newvalue));
			
		}

		static void OnSourcePropertyChanging(BindableObject bindable, object oldvalue, object newvalue)
		{
			var image = ((Image)bindable);
			if (oldvalue != null)
			{
				image.SourcePropertyChanging((ImageSource)oldvalue);
			}

			image.ImageSourceChanging?.Invoke(bindable, new BindableValueChangedEventArgs(bindable, oldvalue, newvalue));
		}

		


		public void RaiseImageSourcePropertyChanged() =>
			OnPropertyChanged(nameof(Source));
	}


	public class BindableValueChangedEventArgs : EventArgs
	{
		public BindableValueChangedEventArgs()
		{

		}

		public BindableValueChangedEventArgs(object owner, object oldValue, object newValue)
		{
			Owner = owner;
			OldValue = oldValue;
			NewValue = NewValue;
		}

		public object Owner { get;  }
		public object OldValue { get;  }
		public object NewValue { get; }
	}
}