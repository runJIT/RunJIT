namespace RunJit.Cli.RunJit.Generate.DotNetTool.Models
{
    internal abstract class DisposableBase : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                DisposeManagedResources();
            }

            _disposed = true;
        }

        protected abstract void DisposeManagedResources();

        ~DisposableBase()
        {
            Dispose(false);
        }
    }
}
