using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace vscli.Commands {

    internal sealed class FindReferencesCommand : ICommand {
        private const string CSPROJ_TYPENAME_REFERENCE = "Reference";

        public string Description { get; } = "Searches a csproj file for references like the one provided.";

        public int Run( string[] args ) {
            var csprojFile = args[0];
            var referenceToFind = args[1];

            FindReferences(
                csprojFile,
                referenceToFind
            );

            return 0;
        }

        private void FindReferences( string projectFilePath, string assemblyName ) {
            var proj = new Project( projectFilePath );
            var references = proj.GetItems( CSPROJ_TYPENAME_REFERENCE );

            var found = references.Where( r => r.UnevaluatedInclude.StartsWith( assemblyName ) );
            foreach( var @ref in found) {
                Console.WriteLine( $"Found '{@ref.UnevaluatedInclude}'" );
            }
        }
    }

}
