using System.ComponentModel;
using System.Threading.Tasks;
using Android.Widget;

namespace Xamarin.Forms.Platform.Android.FastRenderers
{
	public static class ImageElementManager
	{
		public static void Init(IVisualElementRenderer renderer)
		{
			renderer.ElementPropertyChanged += OnElementPropertyChanged;
			renderer.ElementChanged += OnElementChanged;
		}

		private async static void OnElementChanged(object sender, VisualElementChangedEventArgs e)
		{
			var renderer = (sender as IVisualElementRenderer);
			var view = (ImageView)renderer.View;
			var newImageElement = (IImageElement)e.NewElement;
			var oldImageElement = (IImageElement)e.OldElement;

			await TryUpdateBitmap(view, newImageElement, oldImageElement);
			UpdateAspect(view, newImageElement, oldImageElement);
			ElevationHelper.SetElevation(view, renderer.Element);
		}

		private async static void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == ImageButton.SourceProperty.PropertyName)
			{
				var renderer = (sender as IVisualElementRenderer);
				await TryUpdateBitmap((ImageView)renderer.View, (IImageElement)renderer.Element);
			}
			else if (e.PropertyName == ImageButton.AspectProperty.PropertyName)
			{
				var renderer = (sender as IVisualElementRenderer);
				UpdateAspect((ImageView)renderer.View, (IImageElement)renderer.Element);
			}
		}


		async static Task TryUpdateBitmap(ImageView Control, IImageElement newImage, IImageElement previous = null)
		{
			if (newImage == null)
			{
				return;
			}

			await Control.UpdateBitmap(newImage, previous);
		}

		static void UpdateAspect(ImageView Control, IImageElement newImage, IImageElement previous = null)
		{
			if (newImage == null)
			{
				return;
			}

			ImageView.ScaleType type = newImage.Aspect.ToScaleType();
			Control.SetScaleType(type);
		}
	}
}