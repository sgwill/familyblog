using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WilliamsonFamily.Web.Web;

namespace WilliamsonFamily.Web.Tests.Web
{
    [TestClass]
    public class ExtensionTests
    {
        #region EntrySummary Tests
        [TestMethod]
        public void Extensions_EntrySummary_EmptyString_ReturnsEmptyString()
        {
            string entry = "";
            Assert.AreEqual("", entry.EntrySummary());
        }

        [TestMethod]
        public void Extensions_EntrySummary_EntryLessThan1000Chars_ReturnsEntry()
        {
            string entry = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In interdum enim vitae nunc facilisis sed vestibulum erat aliquet. Pellentesque vel tellus semper massa vehicula convallis. Donec leo sapien, ultrices id laoreet id, semper ut quam. In hac habitasse platea dictumst. Etiam in fermentum leo. Quisque pretium fermentum nisl fringilla posuere. Donec ipsum lacus, placerat at dignissim quis, tincidunt vitae lacus. Donec pellentesque lobortis ante, hendrerit commodo eros congue imperdiet. Sed euismod ultrices magna, in aliquam enim tristique quis. Phasellus convallis consequat ante, a sodales mi laoreet in. Sed et velit augue. Donec faucibus, enim sed sodales vulputate, mauris ante pulvinar nibh, sed ultrices ante risus ut ipsum. Sed lobortis lobortis vestibulum. Donec aliquet aliquet rutrum. Phasellus lobortis mi non turpis ultricies id pretium ante ornare. In leo tortor, consectetur sed vulputate et, ultrices sed velit. Maecenas purus nisl, volutpat nec fringilla ut, viverra in neque.";
            Assert.AreEqual(entry, entry.EntrySummary());
        }

        [TestMethod]
        public void Extensions_EntrySummary_EntryGreaterThan1000Chars_ReturnsUntilFirstWordAfter1000()
        {
            string entry = "Lorem ips dolor sit amet, consectetur adipiscing elit. In interdum enim vitae nunc facilisis sed vestibulum erat aliquet. Pellentesque vel tellus semper massa vehicula convallis. Donec leo sapien, ultrices id laoreet id, semper ut quam. In hac habitasse platea dictumst. Etiam in fermentum leo. Quisque pretium fermentum nisl fringilla posuere. Donec ipsum lacus, placerat at dignissim quis, tincidunt vitae lacus. Donec pellentesque lobortis ante, hendrerit commodo eros congue imperdiet. Sed euismod ultrices magna, in aliquam enim tristique quis. Phasellus convallis consequat ante, a sodales mi laoreet in. Sed et velit augue. Donec faucibus, enim sed sodales vulputate, mauris ante pulvinar nibh, sed ultrices ante risus ut ipsum. Sed lobortis lobortis vestibulum. Donec aliquet aliquet rutrum. Phasellus lobortis mi non turpis ultricies id pretium ante ornare. In leo tortor, consectetur sed vulputate et, ultrices sed velit. Maecenas purus nisl, volutpat nec fringilla ut, viverra in nequels and the only way.";
            string result = "Lorem ips dolor sit amet, consectetur adipiscing elit. In interdum enim vitae nunc facilisis sed vestibulum erat aliquet. Pellentesque vel tellus semper massa vehicula convallis. Donec leo sapien, ultrices id laoreet id, semper ut quam. In hac habitasse platea dictumst. Etiam in fermentum leo. Quisque pretium fermentum nisl fringilla posuere. Donec ipsum lacus, placerat at dignissim quis, tincidunt vitae lacus. Donec pellentesque lobortis ante, hendrerit commodo eros congue imperdiet. Sed euismod ultrices magna, in aliquam enim tristique quis. Phasellus convallis consequat ante, a sodales mi laoreet in. Sed et velit augue. Donec faucibus, enim sed sodales vulputate, mauris ante pulvinar nibh, sed ultrices ante risus ut ipsum. Sed lobortis lobortis vestibulum. Donec aliquet aliquet rutrum. Phasellus lobortis mi non turpis ultricies id pretium ante ornare. In leo tortor, consectetur sed vulputate et, ultrices sed velit. Maecenas purus nisl, volutpat nec fringilla ut, viverra in nequels and";
            Assert.AreEqual(result, entry.EntrySummary());
        }
        #endregion

        #region FriendyDateTime Tests
        [TestMethod]
        public void Extensions_FriendlyDate_Today_Returns_Today()
        {
            Assert.AreEqual("Today", DateTime.Today.FriendlyDate());
        }

        [TestMethod]
        public void Extensions_FriendlyDate_Yesterday_Returns_Yesterday()
        {
            Assert.AreEqual("Yesterday", DateTime.Today.AddDays(-1).FriendlyDate());
        }

        [TestMethod]
        public void Extensions_FriendyDate_ThisWeek_Returns_DayNameWithMonthDay()
        {
            Assert.AreEqual(string.Format("{0:dddd}, {0:MMMM} {0:dd}", DateTime.Today.AddDays(-3)), DateTime.Today.AddDays(-3).FriendlyDate());
            Assert.AreEqual(string.Format("{0:dddd}, {0:MMMM} {0:dd}", DateTime.Today.AddDays(-7)), DateTime.Today.AddDays(-7).FriendlyDate());
        }

        [TestMethod]
        public void Extensions_FriendlyDate_ThisYear_Returns_MonthDay()
        {
            Assert.AreEqual(string.Format("{0:M}", DateTime.Today.AddDays(-8)), DateTime.Today.AddDays(-8).FriendlyDate());
        }

        [TestMethod]
        public void Extension_FriendlyDate_NotThisYear_Returns_MonthDayYear()
        {
            Assert.AreEqual(string.Format("{0:MMMM} {0:dd}, {0:yyyy}", DateTime.Today.AddYears(-1)), DateTime.Today.AddYears(-1).FriendlyDate());
        }
        #endregion
    }
}
