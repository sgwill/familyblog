using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WilliamsonFamily.Library.Tests
{
	[TestClass]
	public class ExtensionMethodsTests
	{
		[TestMethod]
		public void HasValue_String_HasValue()
		{
			Assert.AreEqual(true, "value".HasValue());
		}

		[TestMethod]
		public void HasValue_EmptyString_DoesNotHasValue()
		{
			Assert.AreEqual(false, "".HasValue());
		}

		[TestMethod]
		public void HasValue_NullString_DoesNotHasValue()
		{
			string s = null;
			Assert.AreEqual(false, s.HasValue());
		}
	}
}