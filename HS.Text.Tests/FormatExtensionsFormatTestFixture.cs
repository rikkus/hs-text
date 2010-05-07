using System;
using NUnit.Framework;

namespace HS.Text
{
    [TestFixture]
    public class FormatExtensionsFormatTestFixture
    {
        [Test]
        public void FormattingEmptyStringGivesEmptyString()
        {
            Assert.AreEqual("", FormatExtensions.Format(""));
        }

        [Test]
        public void FormattingEmptyStringWithArgsGivesEmptyString()
        {
            Assert.AreEqual("", FormatExtensions.Format("", "a:b"));
        }
        
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void FormattingNullGivesArgumentNullException()
        {
            FormatExtensions.Format(null);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void FormattingNullArgsGivesArgumentNullException()
        {
            FormatExtensions.Format(null, null);
        }

        [Test]
        public void FormattingWithNoArgsGivesSameStringBack()
        {
            const string helloWorld = "Hello, world!";

            Assert.AreEqual(helloWorld, FormatExtensions.Format(helloWorld));
        }

        [Test]
        public void FormattingWithNotQuiteSignificantBracesGivesSameStringBack()
        {
            const string notQuiteSignificantBraces = "Using { some braces } {- but not quite} significantly.";

            Assert.AreEqual
                (
                notQuiteSignificantBraces,
                FormatExtensions.Format(notQuiteSignificantBraces)
                );
        }

        [Test, ExpectedException(typeof(FormatException))]
        public void FormattingWithMissingArgsGivesFormatException()
        {
            FormatExtensions.Format("{format}");
        }

        [Test]
        public void PlaceholderFilledAtStart()
        {
            Assert.AreEqual
                (
                "World, hello!",
                FormatExtensions.Format("{planet}, hello!", "planet:World")
                );
        }

        [Test]
        public void PlaceholderFilledInMiddle()
        {
            Assert.AreEqual
                (
                "Hello, world!",
                FormatExtensions.Format("Hello, {planet}!", "planet:world")
                );
        }

        [Test]
        public void PlaceholderFilledAtEnd()
        {
            Assert.AreEqual
                (
                "Hello, world",
                FormatExtensions.Format("Hello, {planet}", "planet:world")
                );
        }

        [Test]
        public void TestMultipleDifferentPlaceholdersReplaced()
        {
            Assert.AreEqual("Hello, world!", FormatExtensions.Format("{greeting}, {planet}!", "greeting:Hello", "planet:world"));
        }

        [Test]
        public void TestMultipleIdenticalPlaceholdersReplaced()
        {
            Assert.AreEqual("hello, hello", FormatExtensions.Format("{greeting}, {greeting}", "greeting:hello"));
        }
    }
}
