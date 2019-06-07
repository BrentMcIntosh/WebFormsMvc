using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Util;

namespace Web.Framework
{
	public static class HtmlHelperExtensions
	{
		public static MvcCard Card(this HtmlHelper helper)
		{
			var response = helper.ViewContext.HttpContext.Response;
			response.Write("<table class=\"card\" border=\"0\"><tr><td class=\"top-left-corner\"></td><td class=\"top\"></td><td class=\"top-right-corner\"></td></tr><tr><td class=\"left\"></td><td class=\"content\">");
			return new MvcCard(response);
		}

		public static MvcHtmlString DropDownList<T>(this HtmlHelper htmlHelper, string name, IEnumerable<T> values, T value, Func<T, string> key, Func<T, string> label, string optionLabel)
		{
			IEnumerable<SelectListItem> items = values.Select(x => new SelectListItem
			                                                      	{
			                                                      		Selected = x.Equals(value),
																		Text = label(x),
																		Value = key(x)
			                                                      	});
			return htmlHelper.DropDownList(name, items, optionLabel);
		}

		public static MvcHtmlString EnumDropDownList<T>(this HtmlHelper htmlHelper, string name, string optionLabel, object htmlAttributes)
		{
			IEnumerable<SelectListItem> items = Enum.GetValues(typeof (T))
				.Cast<T>()
				.Select(v => new SelectListItem
				             	{
				             		Selected = false,
				             		Value = v.ToString(),
				             		Text = EnumUtils.GetEnumDescription(v)
				             	});

			return htmlHelper.DropDownList(name, items, optionLabel, htmlAttributes);
		}

		public static MvcHtmlString EnumDropDownList<T>(this HtmlHelper htmlHelper, string name, T value, object htmlAttributes)
		{
			IEnumerable<SelectListItem> items = Enum.GetValues(typeof(T))
				.Cast<T>()
				.Select(v => new SelectListItem
				{
					Selected = v.Equals(value),
					Value = v.ToString(),
					Text = EnumUtils.GetEnumDescription(v)
				});

			return htmlHelper.DropDownList(name, items, htmlAttributes);
		}

		public static MvcHtmlString EnumDropDownList<T>(this HtmlHelper htmlHelper, string name, T value)
		{
			IEnumerable<SelectListItem> items = Enum.GetValues(typeof (T))
				.Cast<T>()
				.Select(v => new SelectListItem
				             	{
									Selected = v.Equals(value),
									Value = v.ToString(),
									Text = EnumUtils.GetEnumDescription(v)
				             	});

			return htmlHelper.DropDownList(name, items);
		}

		public static MvcHtmlString EnumDropDownList<T>(this HtmlHelper htmlHelper, string name, T value, string optionLabel, object htmlAttributes)
		{
			IEnumerable<SelectListItem> items = Enum.GetValues(typeof(T))
				.Cast<T>()
				.Select(v => new SelectListItem
				{
					Selected = v.Equals(value),
					Value = v.ToString(),
					Text = EnumUtils.GetEnumDescription(v)
				});

			return htmlHelper.DropDownList(name, items, optionLabel, htmlAttributes);
		}

		public static MvcHtmlString EnumDropDownList<T>(this HtmlHelper htmlHelper, string name, T? value, string optionLabel, object htmlAttributes) where T : struct 
		{
			IEnumerable<SelectListItem> items = Enum.GetValues(typeof(T))
				.Cast<T>()
				.Select(v => new SelectListItem
				{
					Selected = v.Equals(value),
					Value = v.ToString(),
					Text = EnumUtils.GetEnumDescription(v)
				});

			return htmlHelper.DropDownList(name, items, optionLabel, htmlAttributes);
		}

		public static MvcHtmlString YesNoDropDownList(this HtmlHelper htmlHelper, string name, bool value)
		{
			return htmlHelper.DropDownList(name, new[]
			                             	{
			                             		new SelectListItem {Selected = value, Text = "Yes", Value = bool.TrueString},
			                             		new SelectListItem {Selected = !value, Text = "No", Value = bool.FalseString},
			                             	});
		}

		public static MvcHtmlString YesNoMaybeDropDownList(this HtmlHelper htmlHelper, string name, bool? value)
		{
			return htmlHelper.DropDownList(name, new[]
			                             	{
			                             		new SelectListItem {Selected = value.HasValue && value.Value, Text = "Yes", Value = bool.TrueString},
			                             		new SelectListItem {Selected = value.HasValue && !value.Value, Text = "No", Value = bool.FalseString},
			                             	}, "");
		}
	}

	public class MvcCard : IDisposable
	{
		// Fields
		private bool _disposed;
		private readonly HttpResponseBase _httpResponse;

		// Methods
		public MvcCard(HttpResponseBase httpResponse)
		{
			if (httpResponse == null)
				throw new ArgumentNullException("httpResponse");
			_httpResponse = httpResponse;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				_disposed = true;
				_httpResponse.Write("</td><td class=\"right\"></td></tr><tr><td class=\"bottom-left-corner\"></td><td class=\"bottom\"></td><td class=\"bottom-right-corner\"></td></tr></table>");
			}
		}
	}
}
