namespace Fusion.Tests
{
    [TestClass]
    public class TestHtmlProcessing
    {
        [TestMethod]
        public void TestHeadingConversion()
        {
            var processor = new MarkdownProcessor(MarkdownStandard.CommonMark);
            var markdown = "# Heading 1";
            var expectedHtml = "<h1>Heading 1</h1>";
            var actualHtml = processor.ConvertMarkdownToHtml(markdown).Trim();
            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [TestMethod]
        public void TestBoldConversion()
        {
            var processor = new MarkdownProcessor(MarkdownStandard.CommonMark);
            var markdown = "**Bold Text**";
            var expectedHtml = "<strong>Bold Text</strong>";
            var actualHtml = processor.ConvertMarkdownToHtml(markdown).Trim();
            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [TestMethod]
        public void TestItalicConversion()
        {
            var processor = new MarkdownProcessor(MarkdownStandard.CommonMark);
            var markdown = "_Italic Text_";
            var expectedHtml = "<em>Italic Text</em>";
            var actualHtml = processor.ConvertMarkdownToHtml(markdown).Trim();
            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [TestMethod]
        public void TestTaskListConversion()
        {
            var processor = new MarkdownProcessor(MarkdownStandard.CommonMark);
            var markdown = "- [x] Checked item\n- [ ] Unchecked item";
            var expectedHtml = "<li><input type='checkbox' checked disabled> Checked item</li>\n<li><input type='checkbox' disabled> Unchecked item</li>";
            var actualHtml = processor.ConvertMarkdownToHtml(markdown).Trim();

            // Normalise whitespace
            expectedHtml = NormalizeWhitespace(expectedHtml);
            actualHtml = NormalizeWhitespace(actualHtml);

            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [TestMethod]
        public void TestLinkConversion()
        {
            var processor = new MarkdownProcessor(MarkdownStandard.CommonMark);
            var markdown = "[Link Text](http://example.com)";
            var expectedHtml = "<a href=\"http://example.com\">Link Text</a>";
            var actualHtml = processor.ConvertMarkdownToHtml(markdown).Trim();
            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [TestMethod]
        public void TestImageConversion()
        {
            var processor = new MarkdownProcessor(MarkdownStandard.CommonMark);
            var markdown = "![Alt Text](http://example.com/image.png)";
            var expectedHtml = "<img src=\"http://example.com/image.png\" alt=\"Alt Text\" />";
            var actualHtml = processor.ConvertMarkdownToHtml(markdown).Trim();
            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [TestMethod]
        public void TestStrikethroughConversion()
        {
            var processor = new MarkdownProcessor(MarkdownStandard.CommonMark);
            var markdown = "~~Strikethrough Text~~";
            var expectedHtml = "<del>Strikethrough Text</del>";
            var actualHtml = processor.ConvertMarkdownToHtml(markdown).Trim();
            Assert.AreEqual(expectedHtml, actualHtml);
        }

        private string NormalizeWhitespace(string input)
        {
            return string.Join(" ", input.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
