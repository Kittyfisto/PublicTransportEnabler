using System;
using System.Globalization;
using PublicTransportEnabler.Enum;

namespace PublicTransportEnabler.Model
{
	public class Style
	{
		public static uint BLACK = 0xFF000000;
		public static uint DKGRAY = 0xFF444444;
		public static uint GRAY = 0xFF888888;
		public static uint LTGRAY = 0xFFCCCCCC;
		public static uint WHITE = 0xFFFFFFFF;
		public static uint RED = 0xFFFF0000;
		public static uint GREEN = 0xFF00FF00;
		public static uint BLUE = 0xFF0000FF;
		public static uint YELLOW = 0xFFFFFF00;
		public static uint CYAN = 0xFF00FFFF;
		public static uint MAGENTA = 0xFFFF00FF;
		public static uint TRANSPARENT = 0;
		public Shape shape;

		public Style(uint backgroundColor, uint foregroundColor)
		{
			shape = Shape.ROUNDED;
			BackgroundColor = backgroundColor;
			ForegroundColor = foregroundColor;
			BorderColor = 0;
		}

		public Style(Shape shape, uint backgroundColor, uint foregroundColor)
		{
			this.shape = shape;
			BackgroundColor = backgroundColor;
			ForegroundColor = foregroundColor;
			BorderColor = 0;
		}

		public Style(Shape shape, uint backgroundColor, uint foregroundColor, uint borderColor)
		{
			this.shape = shape;
			BackgroundColor = backgroundColor;
			ForegroundColor = foregroundColor;
			BorderColor = borderColor;
		}

		public Style(uint backgroundColor, uint foregroundColor, uint borderColor)
		{
			shape = Shape.ROUNDED;
			BackgroundColor = backgroundColor;
			ForegroundColor = foregroundColor;
			BorderColor = borderColor;
		}

		public uint BackgroundColor { get; private set; }
		public uint ForegroundColor { get; private set; }
		public uint BorderColor { get; private set; }

		public bool HasBorder()
		{
			return BorderColor != 0;
		}

		public static uint ParseColor(string colorstring)
		{
			if (colorstring.Length < 1)
				return 0;

			if (colorstring[0] == '#')
			{
				// Use a long to avoid rollovers on #ffXXXXXX
				long color = long.Parse(colorstring.Substring(1), NumberStyles.HexNumber);

				if (colorstring.Length == 7)
				{
					// Set the alpha value
					color |= 0x00000000ff000000;
				}
				else if (colorstring.Length != 9)
				{
					throw new ArgumentException("Unknown color");
				}
				return (uint) color;
			}
			throw new ArgumentException("Unknown color");
		}

		public static uint Rgb(int red, int green, int blue)
		{
			return (uint) ((0xFF << 24) | (red << 16) | (green << 8) | blue);
		}
	}
}