using Simple.Common;

namespace System.Collections.Generic
{
    public static class LinkedListExtensions
    {
        public static void AddRange<T>(this LinkedList<T> linkedList, IEnumerable<T> items)
        {
            ArgCheck.NotNull(nameof(linkedList), linkedList);
            if (items != null)
                foreach (var item in items)
                    linkedList.AddLast(item);
        }
    }
}
