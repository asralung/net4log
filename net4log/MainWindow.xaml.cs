namespace Net4Log
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Xml.Serialization;

    using LogfileReader.Entries;

    using Net4Log.RoslynHost;
    using Net4Log.ViewModel;

    using RoslynPad.Editor;
    using RoslynPad.Roslyn;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RoslynPad.Roslyn.RoslynHost host;

        public MainWindow()
        {
            this.InitializeComponent();

            var additionalAssemblies = new[]
                                           {
                                               Assembly.Load("RoslynPad.Roslyn.Windows"),
                                               Assembly.Load("RoslynPad.Editor.Windows"),
                                               Assembly.Load("LogfileReader"),
                                           };

            var hostReferences = RoslynHostReferences
                                .Default
                                .With(typeNamespaceImports: new[]
                                                            {
                                                                typeof(LogEntry),
                                                                typeof(DocumentViewModel), // ???
                                                                typeof(Globals),
                                                                typeof(XmlSerializer)
                                                            });

            this.host = new RoslynHostWithGlobals<Globals>(
                                 additionalAssemblies: additionalAssemblies, 
                                 references: hostReferences);

            this.DataContext = new DocumentViewModel(this.host);
        }

        private void OnItemLoaded(object sender, EventArgs e)
        {
            var editor = (RoslynCodeEditor)sender;
            editor.Loaded -= this.OnItemLoaded;
            editor.Focus();

            var viewModel = (DocumentViewModel)editor.DataContext;
            var workingDirectory = Directory.GetCurrentDirectory();

            var documentId = editor.Initialize(
                                                this.host,
                                                new ClassificationHighlightColors(),
                                                workingDirectory,
                                                HintProvider.GetInitalScriptText());

            viewModel.Initialize(documentId);
        }

        private void OnPreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }
    }
}
