using System.Collections.Generic;
using System.Text;
namespace HtmlAgilityPack.CssSelectors.NetCore;
public static class Tokenizer
{
    public static IEnumerable<Token> GetTokens(string cssFilter)
    {
        using var reader = new System.IO.StringReader(cssFilter);
        while (true)
        {
            var v = reader.Read();
            if (v < 0)
                yield break;
            var c = (char)v;
            if (c == '>')
            {
                yield return new Token(">");
                continue;
            }
            if (c == ' ' || c == '\t')
                continue;
            var word = c + ReadWord(reader);
            yield return new Token(word);
        }
    }
    private static string ReadWord(System.IO.StringReader reader)
    {
        var sb = new StringBuilder();
        while (true)
        {
            var v = reader.Read();
            if (v < 0)
                break;
            var c = (char)v;
            if (c == ' ' || c == '\t' || c == '>')
                break;
            sb.Append(c);
        }
        return sb.ToString();
    }
}
