namespace Net4Log.ViewModel.Commands
{
    using System;
    using System.Windows.Input;
    using System.Windows.Media;

    using RoslynPad.Editor;

    public class ToogleColorCommand : ICommand
    {
        private bool isGray;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var codeEditor = (RoslynCodeEditor)parameter;
            codeEditor.Background = this.isGray ? new SolidColorBrush(Colors.White)
                                        : new SolidColorBrush(Colors.LightGray);

            this.isGray = !this.isGray;
        }

        protected virtual void OnCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}