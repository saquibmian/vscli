using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace vscli.Commands {

    internal sealed class AddReferenceCommand : ICommand {
        private const string CSPROJ_TYPENAME_REFERENCE = "Reference";

        public string Description { get; } = "Adds a reference to a csproj file.";

        public int Run( string[] args ) {
            var csprojFile = args[0];
            var referenceToAdd = args[1];
            var specificVersion = false;

            AddReference(
                csprojFile,
                referenceToAdd,
                specificVersion
            );

            return 0;
        }

        private void AddReference( string projectFilePath, string assemblyName, bool specificVersion = false ) {
            var proj = new Project( projectFilePath );
            var references = proj.GetItems( CSPROJ_TYPENAME_REFERENCE );

            if( references.Any( r => r.UnevaluatedInclude == assemblyName ) ) {
                Console.WriteLine( $"Project {proj.FullPath} already has a reference to {assemblyName}" );
                return;
            }

            var item = proj.AddItem(
                CSPROJ_TYPENAME_REFERENCE,
                assemblyName,
                new Dictionary<string, string> {
                    ["SpecificVersion"] = specificVersion ? "True" : "False"
                }
            );
            Console.WriteLine( $"Adding {assemblyName} as a reference from {proj.FullPath}" );

            proj.Save();
        }
    }

}
