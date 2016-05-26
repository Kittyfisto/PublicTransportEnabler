using System.Collections.Generic;
using PublicTransportEnabler.Enum;

namespace PublicTransportEnabler.Model
{
	public class StandardColors
	{
		public static Dictionary<char, Style> LINES = new Dictionary<char, Style>();

		static StandardColors()
		{
			LINES.Add('I', new Style(Shape.RECT, Style.WHITE, Style.RED, Style.RED));
			LINES.Add('R', new Style(Shape.RECT, Style.GRAY, Style.WHITE));
			LINES.Add('S', new Style(Shape.CIRCLE, Style.ParseColor("#006e34"), Style.WHITE));
			LINES.Add('U', new Style(Shape.RECT, Style.ParseColor("#003090"), Style.WHITE));
			LINES.Add('T', new Style(Shape.RECT, Style.ParseColor("#cc0000"), Style.WHITE));
			LINES.Add('B', new Style(Style.ParseColor("#993399"), Style.WHITE));
			LINES.Add('F', new Style(Shape.CIRCLE, Style.BLUE, Style.WHITE));
			LINES.Add('?', new Style(Style.DKGRAY, Style.WHITE));
		}
	}
}