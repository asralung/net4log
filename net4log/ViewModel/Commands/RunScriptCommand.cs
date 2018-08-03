namespace Net4Log.ViewModel.Commands
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.CSharp.Scripting.Hosting;
    using Microsoft.CodeAnalysis.Scripting;

    using Net4Log.RoslynHost;

    using RoslynPad.Editor;

    public class RunScriptCommand : ICommand
    {
        private readonly DocumentViewModel viewModel;

        private readonly ImmutableArray<MetadataReference> defaultReferences;

        private readonly ImmutableArray<string> defaultImports;

        public RunScriptCommand(DocumentViewModel viewModel, ImmutableArray<MetadataReference> defaultReferences, ImmutableArray<string> defaultImports)
        {
            this.viewModel = viewModel;
            this.defaultReferences = defaultReferences;
            this.defaultImports = defaultImports;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var codeEditor = (RoslynCodeEditor)parameter;

            await this.TrySubmit(codeEditor.Text ?? string.Empty);
        }

        protected virtual void OnCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task TrySubmit(string script)
        {
            try
            {
                this.viewModel.IsBusy = true;

                this.viewModel.Result = (await this.ExecuteScriptAsync(script)).ToString();
            }
            catch (Exception e)
            {
                this.viewModel.Result = CSharpObjectFormatter.Instance.FormatException(e);
            }
            finally
            {
                this.viewModel.IsBusy = false;
            }
        }

        private Task<object> ExecuteScriptAsync(string script)
        {
            return Task.Run(() => this.ExecuteScript(script));
        }

        private object ExecuteScript(string text)
        {
            this.viewModel.Result = null;

            var scriptOptions = ScriptOptions.Default.WithReferences(this.defaultReferences)
                .WithImports(this.defaultImports)
                .AddReferences("LogfileReader")
                .AddReferences(typeof(Globals).Assembly)
                .AddImports("LogfileReader")
                .AddImports("Net4Log");

            var script = CSharpScript.Create(text, scriptOptions, typeof(Globals));

            var diagnostics = script.Compile();
            if (diagnostics.Any(t => t.Severity == DiagnosticSeverity.Error))
            {
                return string.Join(Environment.NewLine, diagnostics.Select(x => x.ToString()));
            }

            var scriptResult = script.RunAsync(new Globals
                                                   {
                                                       SxFy = this.viewModel.SxFy,
                                                       Entries = this.viewModel.Entries
                                                   });

            if (scriptResult.Exception != null)
            {
                return scriptResult.Exception;
            }

            return scriptResult.Result.ReturnValue ?? string.Empty;
        }
    }
}