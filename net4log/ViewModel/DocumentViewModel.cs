namespace Net4Log.ViewModel
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using LogfileReader.Entries;

    using Microsoft.CodeAnalysis;

    using Net4Log.ViewModel.Commands;

    using RoslynPad.Roslyn;

    public class DocumentViewModel : INotifyPropertyChanged
    {
        private readonly RoslynHost host;

        private string result;

        private string statusLine;

        private bool isBusy;

        public DocumentViewModel(RoslynHost host)
        {
            this.host = host;
            this.SxFy = new List<SxFyLogEntry>();
            this.Result = HintProvider.GetUsageHint();
            this.StatusLine = HintProvider.NoLogFilesLoadedYetText();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DocumentId Id { get; private set; }

        public string Result
        {
            get => this.result;
            set => this.SetProperty(ref this.result, value);
        }

        public string StatusLine
        {
            get => this.statusLine;
            set => this.SetProperty(ref this.statusLine, value);
        }

        public bool IsBusy
        {
            get => this.isBusy;
            set => this.SetProperty(ref this.isBusy, value);
        }

        public IEnumerable<SxFyLogEntry> SxFy { get; set; }

        public IEnumerable<LogEntry> Entries { get; set; }

        public ICommand RunScript => new RunScriptCommand(this, this.host.DefaultReferences, this.host.DefaultImports);

        public ICommand ToogleColor => new ToogleColorCommand();

        public ICommand ParseFiles => new ParseFilesCommand(this);

        internal void Initialize(DocumentId id)
        {
            this.Id = id;
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;

                // ReSharper disable once ExplicitCallerInfoArgument
                this.OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}