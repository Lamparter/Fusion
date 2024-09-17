using System.Text.RegularExpressions;
using System.Text;

public class Patterns
{
    // I *may* have used AI to generate these regex patterns...
    public const string HeadingPattern = @"^(#{1,6}) (.+)$";
    public const string BoldPattern = @"\*\*(.+?)\*\*";
    public const string ItalicPattern = @"_(.+?)_";
    public const string LinkPattern = @"\[(.+?)\]\((.+?)\)";
    public const string ImagePattern = @"!\[(.+?)\]\((.+?)\)";
    public const string ListItemPattern = @"^(\*|-) (.+)$";
    public const string CodeBlockPattern = @"```([^`]*)```";
    public const string InlineCodePattern = @"`([^`]+)`";
    public const string BlockquotePattern = @"^> (.+)$";
    public const string HorizontalRulePattern = @"^---$";
    public const string TablePattern = @"^(\|.+\|)$";
    public const string TaskListPattern = @"^- \[([ x])\] (.+)$";
    public const string StrikethroughPattern = @"~~(.+?)~~";
}

public enum MarkdownTokenType
{
    Text,
    Heading,
    Bold,
    Italic,
    Link,
    Image,
    ListItem,
    CodeBlock,
    InlineCode,
    Blockquote,
    HorizontalRule,
    Table
}

public class MarkdownToken
{
    public MarkdownTokenType Type { get; }
    public string Content { get; }
    public MarkdownToken(MarkdownTokenType type, string content)
    {
        Type = type;
        Content = content;
    }
}

public class MarkdownLexer // I wanted to call this "MarkdownTokeniser" but a certain group of people wouldn't like it... :D Though for some reason Lexer isn't recognised as a word by the spell checker either...
{
    public List<MarkdownToken> Lex(string markdown)
    {
        var tokens = new List<MarkdownToken>();
        var lines = markdown.Split('\n');

        foreach (var line in lines)
        {
            var remainingLine = line.Trim();
            while (!string.IsNullOrEmpty(remainingLine))
            {
                if (Regex.IsMatch(remainingLine, Patterns.HeadingPattern))
                {
                    var match = Regex.Match(remainingLine, Patterns.HeadingPattern);
                    tokens.Add(new MarkdownToken(MarkdownTokenType.Heading, match.Value));
                    remainingLine = remainingLine.Substring(match.Length).Trim();
                }
                else if (Regex.IsMatch(remainingLine, Patterns.ImagePattern))
                {
                    var match = Regex.Match(remainingLine, Patterns.ImagePattern);
                    tokens.Add(new MarkdownToken(MarkdownTokenType.Image, match.Value));
                    remainingLine = remainingLine.Substring(match.Length).Trim();
                }
                else if (Regex.IsMatch(remainingLine, Patterns.LinkPattern))
                {
                    var match = Regex.Match(remainingLine, Patterns.LinkPattern);
                    tokens.Add(new MarkdownToken(MarkdownTokenType.Link, match.Value));
                    remainingLine = remainingLine.Substring(match.Length).Trim();
                }
                else if (Regex.IsMatch(remainingLine, Patterns.BoldPattern))
                {
                    var match = Regex.Match(remainingLine, Patterns.BoldPattern);
                    tokens.Add(new MarkdownToken(MarkdownTokenType.Bold, match.Value));
                    remainingLine = remainingLine.Substring(match.Length).Trim();
                }
                else if (Regex.IsMatch(remainingLine, Patterns.ItalicPattern))
                {
                    var match = Regex.Match(remainingLine, Patterns.ItalicPattern);
                    tokens.Add(new MarkdownToken(MarkdownTokenType.Italic, match.Value));
                    remainingLine = remainingLine.Substring(match.Length).Trim();
                }
                else if (Regex.IsMatch(remainingLine, Patterns.ListItemPattern))
                {
                    var match = Regex.Match(remainingLine, Patterns.ListItemPattern);
                    tokens.Add(new MarkdownToken(MarkdownTokenType.ListItem, match.Value));
                    remainingLine = remainingLine.Substring(match.Length).Trim();
                }
                else if (Regex.IsMatch(remainingLine, Patterns.CodeBlockPattern))
                {
                    var match = Regex.Match(remainingLine, Patterns.CodeBlockPattern);
                    tokens.Add(new MarkdownToken(MarkdownTokenType.CodeBlock, match.Value));
                    remainingLine = remainingLine.Substring(match.Length).Trim();
                }
                else if (Regex.IsMatch(remainingLine, Patterns.InlineCodePattern))
                {
                    var match = Regex.Match(remainingLine, Patterns.InlineCodePattern);
                    tokens.Add(new MarkdownToken(MarkdownTokenType.InlineCode, match.Value));
                    remainingLine = remainingLine.Substring(match.Length).Trim();
                }
                else if (Regex.IsMatch(remainingLine, Patterns.BlockquotePattern))
                {
                    var match = Regex.Match(remainingLine, Patterns.BlockquotePattern);
                    tokens.Add(new MarkdownToken(MarkdownTokenType.Blockquote, match.Value));
                    remainingLine = remainingLine.Substring(match.Length).Trim();
                }
                else if (Regex.IsMatch(remainingLine, Patterns.HorizontalRulePattern))
                {
                    var match = Regex.Match(remainingLine, Patterns.HorizontalRulePattern);
                    tokens.Add(new MarkdownToken(MarkdownTokenType.HorizontalRule, match.Value));
                    remainingLine = remainingLine.Substring(match.Length).Trim();
                }
                else if (Regex.IsMatch(remainingLine, Patterns.TablePattern))
                {
                    var match = Regex.Match(remainingLine, Patterns.TablePattern);
                    tokens.Add(new MarkdownToken(MarkdownTokenType.Table, match.Value));
                    remainingLine = remainingLine.Substring(match.Length).Trim();
                }
                else if (Regex.IsMatch(remainingLine, Patterns.TaskListPattern))
                {
                    var match = Regex.Match(remainingLine, Patterns.TaskListPattern);
                    tokens.Add(new MarkdownToken(MarkdownTokenType.ListItem, match.Value));
                    remainingLine = remainingLine.Substring(match.Length).Trim();
                }
                else if (Regex.IsMatch(remainingLine, Patterns.StrikethroughPattern))
                {
                    var match = Regex.Match(remainingLine, Patterns.StrikethroughPattern);
                    tokens.Add(new MarkdownToken(MarkdownTokenType.Text, match.Value));
                    remainingLine = remainingLine.Substring(match.Length).Trim();
                }
                else
                {
                    tokens.Add(new MarkdownToken(MarkdownTokenType.Text, remainingLine));
                    remainingLine = string.Empty;
                }
            }
        }
        return tokens;
    }
}

public abstract class MarkdownNode
{
    public abstract string ToHtml();
}
public class TextNode : MarkdownNode
{
    public string Content { get; }
    public TextNode(string content) => Content = content;

    public override string ToHtml()
    {
        return Content;
    }
}

public class HeadingNode : MarkdownNode
{
    public string Content { get; }
    public HeadingNode(string content) => Content = content;

    public override string ToHtml()
    {
        var level = Content.TakeWhile(c => c == '#').Count();
        var text = Content.TrimStart('#').Trim();
        return $"<h{level}>{text}</h{level}>";
    }
}

public class BoldNode : MarkdownNode
{
    public string Content { get; }
    public BoldNode(string content) => Content = content;

    public override string ToHtml()
    {
        return $"<strong>{Regex.Replace(Content, @"\*\*(.+?)\*\*", "$1")}</strong>";
    }
}

public class ItalicNode : MarkdownNode
{
    public string Content { get; }
    public ItalicNode(string content) => Content = content;

    public override string ToHtml()
    {
        return $"<em>{Regex.Replace(Content, @"_(.+?)_", "$1")}</em>";
    }
}

public class ListItemNode : MarkdownNode
{
    public string Content { get; }
    public ListItemNode(string content) => Content = content;

    public override string ToHtml()
    {
        return $"<li>{Regex.Replace(Content, @"^(\*|-) ", "")}</li>";
    }
}

public class CodeBlockNode : MarkdownNode
{
    public string Content { get; }
    public CodeBlockNode(string content) => Content = content;

    public override string ToHtml()
    {
        return $"<pre><code>{Regex.Replace(Content, @"```([^`]*)```", "$1")}</code></pre>";
    }
}

public class TableNode : MarkdownNode
{
    public string Content { get; }
    public TableNode(string content) => Content = content;

    public override string ToHtml()
    {
        var htmlBuilder = new StringBuilder("<table><tbody>");
        var rows = Content.Split('\n').Where(line => line.StartsWith("|"));
        foreach (var row in rows)
        {
            htmlBuilder.Append("<tr>");
            var columns = row.Trim('|').Split('|');
            foreach (var column in columns)
            {
                htmlBuilder.AppendFormat("<td>{0}</td>", column.Trim());
            }
            htmlBuilder.Append("</tr>");
        }
        htmlBuilder.Append("</tbody></table>");
        return htmlBuilder.ToString();
    }
}

public class TaskListNode : MarkdownNode
{
    public string Content { get; }
    public TaskListNode(string content) => Content = content;

    public override string ToHtml()
    {
        var match = Regex.Match(Content, Patterns.TaskListPattern);
        if (match.Success)
        {
            var isChecked = match.Groups[1].Value == "x" ? "checked" : "";
            var text = match.Groups[2].Value;
            return $"<li><input type='checkbox'{(isChecked == "checked" ? " checked" : "")}> {text}</li>"; // TODO: Add the ability to pass an argument to see if the consumer wants the checkbox to be disabled or not
        }
        return "";
    }
}

public class LinkNode : MarkdownNode
{
    public string Content { get; }
    public LinkNode(string content) => Content = content;

    public override string ToHtml()
    {
        var match = Regex.Match(Content, Patterns.LinkPattern);
        if (match.Success)
        {
            var text = match.Groups[1].Value;
            var url = match.Groups[2].Value;
            return $"<a href=\"{url}\">{text}</a>";
        }
        return "";
    }
}

public class ImageNode : MarkdownNode
{
    public string Content { get; }
    public ImageNode(string content) => Content = content;

    public override string ToHtml()
    {
        var match = Regex.Match(Content, Patterns.ImagePattern);
        if (match.Success)
        {
            var altText = match.Groups[1].Value;
            var url = match.Groups[2].Value;
            return $"<img src=\"{url}\" alt=\"{altText}\" />";
        }
        return "";
    }
}

public class StrikethroughNode : MarkdownNode
{
    public string Content { get; }
    public StrikethroughNode(string content) => Content = content;

    public override string ToHtml()
    {
        return $"<del>{Regex.Replace(Content, @"~~(.+?)~~", "$1")}</del>";
    }
}

public class MarkdownParser
{
    public List<MarkdownNode> Parse(List<MarkdownToken> tokens)
    {
        var nodes = new List<MarkdownNode>();

        foreach (var token in tokens)
        {
            switch (token.Type)
            {
                case MarkdownTokenType.Heading:
                    nodes.Add(new HeadingNode(token.Content));
                    break;
                case MarkdownTokenType.Bold:
                    nodes.Add(new BoldNode(token.Content));
                    break;
                case MarkdownTokenType.Italic:
                    nodes.Add(new ItalicNode(token.Content));
                    break;
                case MarkdownTokenType.ListItem:
                    if (Regex.IsMatch(token.Content, Patterns.TaskListPattern))
                    {
                        nodes.Add(new TaskListNode(token.Content));
                    }
                    else
                    {
                        nodes.Add(new ListItemNode(token.Content));
                    }
                    break;
                case MarkdownTokenType.CodeBlock:
                    nodes.Add(new CodeBlockNode(token.Content));
                    break;
                case MarkdownTokenType.Table:
                    nodes.Add(new TableNode(token.Content));
                    break;
                case MarkdownTokenType.Link:
                    nodes.Add(new LinkNode(token.Content));
                    break;
                case MarkdownTokenType.Image:
                    nodes.Add(new ImageNode(token.Content));
                    break;
                case MarkdownTokenType.Text:
                    if (Regex.IsMatch(token.Content, Patterns.StrikethroughPattern))
                    {
                        nodes.Add(new StrikethroughNode(token.Content));
                    }
                    else
                    {
                        nodes.Add(new TextNode(token.Content));
                    }
                    break;
            }
        }

        return nodes;
    }
}

public class HtmlRenderer
{
    public string Render(IEnumerable<MarkdownNode> nodes)
    {
        var sb = new StringBuilder();
        foreach (var node in nodes)
        {
            sb.AppendLine(node.ToHtml());
        }
        return sb.ToString();
    }
}

public class MarkdownProcessor
{
    private MarkdownLexer _lexer = new MarkdownLexer();
    private MarkdownParser _parser = new MarkdownParser();
    private HtmlRenderer _renderer = new HtmlRenderer();

    public string ConvertMarkdownToHtml(string markdown)
    {
        var tokens = _lexer.Lex(markdown);
        var ast = _parser.Parse(tokens);
        return _renderer.Render(ast);
    }
}