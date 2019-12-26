using System.Threading;

namespace Common.Threading
{
    public static class CancellationTokens
    {
        public static readonly CancellationToken CancelledToken = new CancellationToken(true);
        public static readonly CancellationToken InfiniteToken = new CancellationToken(false);
    }
}
