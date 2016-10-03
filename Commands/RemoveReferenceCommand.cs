using Microsoft.Build.Evaluation;
using System;
using System.Linq;

namespace vscli.Commands {

    internal sealed class RemoveReferenceCommand : ICommand {
        private const string CSPROJ_TYPENAME_REFERENCE = "Reference";

        public string Description { get; } = "Adds a reference to a csproj file.";

        public int Run( string[] args ) {
            var csprojFile = args[0];
            var referenceToRemove = args[1];

            RemoveReference(
                csprojFile,
                referenceToRemove
            );

            return 0;
        }

        private void RemoveReference( string projectFilePath, string assemblyName ) {
            var proj = new Project( projectFilePath );
            var references = proj.GetItems( CSPROJ_TYPENAME_REFERENCE );

            var reference = references.FirstOrDefault( r => r.UnevaluatedInclude == assemblyName );

            if( reference == null ) {
                Console.WriteLine( $"Project {proj.FullPath} does not have reference to {assemblyName}" );
                return;
            }

            proj.RemoveItem( reference );
            Console.WriteLine( $"Removing {assemblyName} as a reference from {proj.FullPath}" );

            proj.Save();
        }
    }

}
