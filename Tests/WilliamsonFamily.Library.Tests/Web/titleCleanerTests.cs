using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WilliamsonFamily.Library.Web;

namespace WilliamsonFamily.Library.Tests.Web
{
    [TestClass]
    public class TitleCleanerTests
    {
        [TestMethod]
        public void TitleCleaner_RemoveColons()
        {
            string title = "this:s";
            var cleaner = GetCleaner();

            Assert.AreEqual("thiss", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_RemoveSlash()
        {
            string title = "this/s";
            var cleaner = GetCleaner();

            Assert.AreEqual("thiss", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_RemoveQuestionMarks()
        {
            string title = "this?s";
            var cleaner = GetCleaner();

            Assert.AreEqual("thiss", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_RemovePounds()
        {
            string title = "this#s";
            var cleaner = GetCleaner();

            Assert.AreEqual("thiss", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_RemoveLeftBracker()
        {
            string title = "this[s";
            var cleaner = GetCleaner();

            Assert.AreEqual("thiss", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_RemoveRightBracket()
        {
            string title = "this]s";
            var cleaner = GetCleaner();

            Assert.AreEqual("thiss", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_RemoveAtSign()
        {
            string title = "this@s";
            var cleaner = GetCleaner();

            Assert.AreEqual("thiss", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_RemoveStar()
        {
            string title = "this*s";
            var cleaner = GetCleaner();

            Assert.AreEqual("thiss", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_RemovePeriod()
        {
            string title = "this.s";
            var cleaner = GetCleaner();

            Assert.AreEqual("thiss", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_RemoveCommas()
        {
            string title = "this,";
            var cleaner = GetCleaner();

            Assert.AreEqual("this", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_RemoveBackSlash()
        {
            string title = "this\\s";
            var cleaner = GetCleaner();

            Assert.AreEqual("thiss", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_RemoveAmpersand()
        {
            string title = "this&s";
            var cleaner = GetCleaner();

            Assert.AreEqual("thiss", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_RemoveApostrophes()
        {
            string title = "this's";
            var cleaner = GetCleaner();

            Assert.AreEqual("thiss", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_RemoveQuotes()
        {
            string title = "this\"s";
            var cleaner = GetCleaner();

            Assert.AreEqual("thiss", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_ChangeSpacesToDashes()
        {
            string title = "this is a test";
            var cleaner = GetCleaner();

            Assert.AreEqual("this-is-a-test", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_RemoveDoubleDashes()
        {
            string title = "this -is a test";
            var cleaner = GetCleaner();

            Assert.AreEqual("this-is-a-test", cleaner.CleanTitle(title));
        }

        [TestMethod]
        public void TitleCleaner_ReturnsLowered()
        {
            string title = "Title";
            var cleaner = GetCleaner();

            Assert.AreEqual("title", cleaner.CleanTitle(title));
        }

        TitleCleaner GetCleaner()
        {
            return new TitleCleaner();
        }

    }
}
