

namespace System.Threading
{
    public static class CancellationTokenExtensions
    {
        public static bool WaitOnCancellationRequested(this CancellationToken cancellationToken, TimeSpan timeout)
        {
            return cancellationToken.WaitHandle.WaitOne(timeout);
        }
    }
}
