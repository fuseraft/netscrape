namespace Netscrape.Helpers;

using System.Text;

public class WordWrap
{
    public static string Wrap(string text, int maxWidth)
    {
        const string padding = "  ";
        
        if (text == null)
        {
            return string.Empty;
        }
        
        if (maxWidth < 1)
        {
            throw new ArgumentException("maxWidth must be greater than 0.");
        }

        var words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var currentLineLength = 0;
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(padding);

        for (var i = 0; i < words.Length; ++i)
        {
            var word = words[i];

            if (currentLineLength + word.Length + 1 > maxWidth)
            {
                if (currentLineLength > 0)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.Append(padding);
                    currentLineLength = 0;
                }

                while (word.Length > maxWidth)
                {
                    stringBuilder.AppendLine(word[..maxWidth]);
                    word = word.Substring(maxWidth);
                }
            }

            if (currentLineLength > 0)
            {
                stringBuilder.Append(' ');
                currentLineLength++;
            }

            stringBuilder.Append(word);
            currentLineLength += word.Length;
        }

        return stringBuilder.ToString();
    }
}