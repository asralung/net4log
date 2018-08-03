namespace Net4Log.RoslynHost
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Reflection;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    using RoslynPad.Roslyn;

    /// <summary>This class is intended to inject the global variables (stored in <see cref="Globals"/>) into roslyns IntelliSense</summary>
    /// <typeparam name="T">The type that contains the global vaiable.</typeparam>
    public class RoslynHostWithGlobals<T> : RoslynHost
    {
        public RoslynHostWithGlobals(NuGetConfiguration nuGetConfiguration = null, IEnumerable<Assembly> additionalAssemblies = null, RoslynHostReferences references = null)
            : base(nuGetConfiguration, additionalAssemblies, references)
        {
        }

        protected override Project CreateProject(Solution solution, DocumentCreationArgs args, CompilationOptions compilationOptions, Project previousProject = null)
        {
            var name = args.Name ?? "Program";
            var id = ProjectId.CreateNewId(name);

            var parseOptions = new CSharpParseOptions(kind: SourceCodeKind.Script, languageVersion: LanguageVersion.Latest);

            compilationOptions = compilationOptions.WithScriptClassName(name);

            solution = solution.AddProject(ProjectInfo.Create(
                id,
                VersionStamp.Create(),
                name,
                name,
                LanguageNames.CSharp,
                isSubmission: true,
                parseOptions: parseOptions,
                hostObjectType: typeof(T),
                compilationOptions: compilationOptions,
                metadataReferences: previousProject != null ? ImmutableArray<MetadataReference>.Empty : this.DefaultReferences,
                projectReferences: previousProject != null ? new[] { new ProjectReference(previousProject.Id) } : null));

            var project = solution.GetProject(id);

            return project;
        }
    }
}