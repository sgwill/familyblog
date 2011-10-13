using System;

namespace WilliamsonFamily.Library
{
	public static class ExtensionMethods
	{
		/// <summary>
		/// Answers true if this String is neither null or empty.
		/// </summary>
		/// <remarks>I'm also tired of typing !String.IsNullOrEmpty(s)</remarks>
		public static bool HasValue(this string s)
		{
			return !string.IsNullOrEmpty(s);
		}
	}
}