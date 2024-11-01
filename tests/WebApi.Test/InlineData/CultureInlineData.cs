using System.Collections;

namespace WebApi.Test.InlineData;

public class CultureInlineData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return ["pt-BR"];
        yield return ["en"];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}