class Program
{
    static void Main(string[] args)
    {
        var processor = new MarkdownProcessor(MarkdownStandard.CommonMark);
        var markdown = @"Change this to whatever you want to test on demand!";
        string html = processor.ConvertMarkdownToHtml(markdown);
        Console.WriteLine(html);
    }
}
