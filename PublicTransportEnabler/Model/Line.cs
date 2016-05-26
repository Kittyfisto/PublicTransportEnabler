using System;
using System.Collections.Generic;
using System.Text;
using PublicTransportEnabler.Enum;

namespace PublicTransportEnabler.Model
{
	public class Line : EquatableBase<Line>
	{
		private static string PRODUCT_ORDER = "IRSUTBPFC?";

		public Line(string id, string label, Style style)
			: this(id, label, style, null, null)
		{
		}

		public Line(string id, string label, Style style, string message)
			: this(id, label, style, null, message)
		{
		}

		public Line(string id, string label, Style style, HashSet<Attr> attrs)
			: this(id, label, style, attrs, null)
		{
		}

		public Line(string id, string label, Style style, HashSet<Attr> attrs, string message)
		{
			Id = id;
			Label = label;
			Style = style;
			Attrs = attrs;
			Message = message;

			Product = label != null ? label[0] : '?';
		}

		public string Id { get; private set; }
		private char Product { get; set; } // TODO make true field
		public string Label { get; private set; }
		public Style Style { get; private set; }
		public HashSet<Attr> Attrs { get; private set; }
		public string Message { get; private set; }

		public bool HasAttr(Attr attr)
		{
			return Attrs != null && Attrs.Contains(attr);
		}

		public override string ToString()
		{
			var builder = new StringBuilder("Line(");
			builder.Append(Label);
			builder.Append(")");
			return builder.ToString();
		}

		public override bool Equals(Line other)
		{
			return NullSafeEquals(Label, other.Label);
		}

		public override int InstanceGetHashCode()
		{
			return NullSafeHashCode(Label);
		}

		public override int CompareTo(Line other)
		{
			int productThis = PRODUCT_ORDER.IndexOf(Product);
			int productOther = PRODUCT_ORDER.IndexOf(other.Product);

			if (productThis < 0)
			{
				productThis = int.MaxValue;
			}

			if (productOther < 0)
			{
				productOther = int.MaxValue;
			}

			int compareProduct = productThis.CompareTo(productOther);

			if (compareProduct != 0)
				return compareProduct;

			return String.Compare(Label, other.Label, StringComparison.Ordinal);
		}
	}
}