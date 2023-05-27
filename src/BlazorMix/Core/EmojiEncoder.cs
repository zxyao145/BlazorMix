using System.Buffers;
using System.Text.Encodings.Web;

namespace BlazorMix;
public class EmojiEncoder: JavaScriptEncoder
{
    public override int MaxOutputCharactersPerInputCharacter => JavaScriptEncoder.Default.MaxOutputCharactersPerInputCharacter;

    public override unsafe int FindFirstCharacterToEncode(char* text, int textLength)
    {
        ReadOnlySpan<char> input = new ReadOnlySpan<char>(text, textLength);
        int idx = 0;

        // Enumerate until we're out of data or saw invalid input
        while (Rune.DecodeFromUtf16(input.Slice(idx), out Rune result, out int charsConsumed) == OperationStatus.Done)
        {
            if (WillEncode(result.Value)) { break; } // found a char that needs to be escaped
            idx += charsConsumed;
        }

        if (idx == input.Length) { return -1; } // walked entire input without finding a char which needs escaping
        return idx;
    }

    public override bool WillEncode(int unicodeScalar)
    {
        // Allow U+1F603 SMILING FACE WITH OPEN MOUTH ('😃'),
        // and for all other chars defer to the default escaper.

        if (unicodeScalar == 0x1F603) { return false; } // does not require escaping
        else { return JavaScriptEncoder.Default.WillEncode(unicodeScalar); }
    }

    public override unsafe bool TryEncodeUnicodeScalar(int unicodeScalar, char* buffer, int bufferLength, out int numberOfCharactersWritten)
    {
        // For anything that needs to be escaped, defer to the default escaper.
        return JavaScriptEncoder.Default.TryEncodeUnicodeScalar(unicodeScalar, buffer, bufferLength, out numberOfCharactersWritten);
    }
}
