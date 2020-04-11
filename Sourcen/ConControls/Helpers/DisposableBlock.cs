/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;

namespace ConControls.Helpers
{
    sealed class DisposableBlock : IDisposable
    {
        readonly Action disposedHandler;
        internal DisposableBlock(Action onDisposeAction) =>
            disposedHandler = onDisposeAction ?? throw new ArgumentNullException(nameof(onDisposeAction));
        public void Dispose()
        {
            disposedHandler();
        }
    }
}
