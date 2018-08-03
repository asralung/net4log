namespace Net4Log.ViewModel.Commands
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using LogfileReader;

    public class ParseFilesCommand : ICommand
    {
        private readonly DocumentViewModel viewModel;

        public ParseFilesCommand(DocumentViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var e = (DragEventArgs)parameter;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);

                await this.OnNewFiles(files);
            }
        }

        protected virtual void OnCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task OnNewFiles(string[] files)
        {
            try
            {
                this.viewModel.IsBusy = true;

                await this.OnNewFilesInternally(files);
            }
            catch (Exception e)
            {
                this.viewModel.StatusLine = e.ToString();
            }
            finally
            {
                this.viewModel.IsBusy = false;
            }
        }

        private async Task OnNewFilesInternally(string[] files)
        {
            var start = DateTime.Now;
            var parser = await new LogfileParser().Parse(files);

            this.viewModel.Entries = parser.Entries;
            this.viewModel.SxFy = parser.SxFy;
            this.viewModel.StatusLine = string.Join(Environment.NewLine, files) + Environment.NewLine
                              + $"Parsed {this.viewModel.Entries.Count()} logfile entries from {files.Length} files ({(DateTime.Now - start).TotalMilliseconds:F0}ms)";
        }
    }
}