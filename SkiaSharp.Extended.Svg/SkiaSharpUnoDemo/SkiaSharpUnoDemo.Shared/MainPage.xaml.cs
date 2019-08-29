using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using SkiaSharp;
using SkiaSharp.Extended.Svg;
using SkiaSharp.Views.UWP;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SkiaSharpUnoDemo
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		private SkiaSharp.Extended.Svg.SKSvg svg;

		public MainPage()
		{
			this.InitializeComponent();
		}

		private static Stream GetImageStream(string svgName)
		{
			var type = typeof(MainPage).GetTypeInfo();
			var assembly = type.Assembly;

			return assembly.GetManifestResourceStream($"SkiaSharpUnoDemo.images.{svgName}");
		}

		private void LoadSvg(string svgName)
		{
			// create a new SVG object
			svg = new SkiaSharp.Extended.Svg.SKSvg();

			// load the SVG document from a stream
			using (var stream = GetImageStream(svgName))
				svg.Load(stream);
		}

		private void OnPainting(object sender, SKPaintSurfaceEventArgs e)
		{
			var page = (SKXamlCanvas)sender;
			LoadSvg(page.Tag as string);

			var surface = e.Surface;
			var canvas = surface.Canvas;

			var width = e.Info.Width;
			var height = e.Info.Height;

			// clear the surface
			canvas.Clear(SKColors.White);

			// the page is not visible yet
			if (svg == null)
				return;

			// calculate the scaling need to fit to screen
			float canvasMin = Math.Min(width, height);
			float svgMax = Math.Max(svg.Picture.CullRect.Width, svg.Picture.CullRect.Height);
			float scale = canvasMin / svgMax;
			var matrix = SKMatrix.MakeScale(scale, scale);

			// draw the svg
			canvas.DrawPicture(svg.Picture, ref matrix);
		}
	}
}
