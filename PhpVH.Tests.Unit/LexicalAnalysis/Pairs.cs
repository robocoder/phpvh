using System.Collections.Generic;

namespace PhpVH.Tests.Unit.LexicalAnalysis
{
    public class Pairs<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }
    }
}
