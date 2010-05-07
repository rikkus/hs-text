using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace HS.Text
{
    [TestFixture]
    public class FormatExtensionsMagicFormatTestFixture
    {
        [Test]
        public void FormattingEmptyStringGivesEmptyString()
        {
            Assert.AreEqual("", FormatExtensions.MagicFormat("", new {}));
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void FormattingNullGivesArgumentNullException()
        {
            FormatExtensions.MagicFormat(null, "");
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void FormattingWithNullAttributeHostGivesArgumentNullException()
        {
            FormatExtensions.MagicFormat("", null); 
        }

        [Test]
        public void FormattingWithNoUsableAttributesGivesSameStringBack()
        {
            const string helloWorld = "Hello, world!";

            Assert.AreEqual(helloWorld, FormatExtensions.MagicFormat(helloWorld, new {}));
        }

        [Test]
        public void FormattingWithNotQuiteSignificantBracesGivesSameStringBack()
        {
            const string notQuiteSignificantBraces = "Using { some braces } {- but not quite} significantly.";

            Assert.AreEqual
                (
                notQuiteSignificantBraces,
                FormatExtensions.MagicFormat(notQuiteSignificantBraces, new {})
                );
        }

        [Test, ExpectedException(typeof(FormatException))]
        public void FormattingWithMissingArgsGivesFormatException()
        {
            FormatExtensions.MagicFormat("{Format}", new {});
        }

        [Test]
        public void PlaceholderFilledAtStart()
        {
            Assert.AreEqual
                (
                "World, hello!",
                FormatExtensions.MagicFormat("{Planet}, hello!", new {Planet = "World"})
                );
        }

        [Test]
        public void PlaceholderFilledInMiddle()
        {
            Assert.AreEqual
                (
                "Hello, world!",
                FormatExtensions.MagicFormat("Hello, {Planet}!", new {Planet = "world"})
                );
        }

        [Test]
        public void PlaceholderFilledAtEnd()
        {
            Assert.AreEqual
                (
                "Hello, world",
                FormatExtensions.MagicFormat("Hello, {Planet}", new {Planet = "world"})
                );
        }

        [Test]
        public void TestMultipleDifferentPlaceholdersReplaced()
        {
            Assert.AreEqual
                (
                "Hello, world!",
                FormatExtensions.MagicFormat
                    (
                    "{Greeting}, {Planet}!",
                    new
                        {
                            Greeting = "Hello",
                            Planet = "world"
                        }
                    )
                );
        }

        [Test]
        public void TestMultipleIdenticalPlaceholdersReplaced()
        {
            Assert.AreEqual
                (
                "Hello! Hello!",
                FormatExtensions.MagicFormat
                    (
                    "{Greeting}! {Greeting}!",
                    new {Greeting = "Hello"}
                    )
                );
        }
    }
}
