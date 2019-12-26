using Simple.Common;

namespace System.Collections.Generic
{
    public static class QueueExtensions
    {
        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            ArgCheck.NotNull(nameof(queue), queue);
            if (queue != null && items != null)
                foreach (var item in items)
                    queue.Enqueue(item);
        }
    }
}
