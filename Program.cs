using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using vscli.Commands;

namespace vscli {
    internal sealed class Program {

        private static ImmutableDictionary<string, ICommand> s_commands = new Dictionary<string, ICommand> {

            ["add-reference"] = new AddReferenceCommand(),
            ["remove-reference"] = new RemoveReferenceCommand(),
            ["find-reference"] = new FindReferencesCommand()

        }.ToImmutableDictionary();

        static void Main( string[] args ) {
            try {
                Environment.Exit( ThrowableMain( args ) );
            } catch ( Exception e ) {
                while( e != null ) {
                    Console.Error.WriteLine( $"error: {e.Message}" );
                    e = e.InnerException;
                }
            }
        }

        private static int ThrowableMain( string[] args ) {
            if( args.Length == 0 ) {
                ShowUsage();
                return -1;
            }

            var cmdName = args[0];
            args = args.Skip( 1 ).ToArray();

            if( s_commands.ContainsKey( cmdName ) ) {
                var cmd = s_commands[cmdName];
                return cmd.Run( args );
            }

            ShowUsage();
            return -1;
        }

        private static void ShowUsage() {
            Console.WriteLine( "Usage:\n" );
            foreach( var cmd in s_commands ) {
                Console.WriteLine( $"\t{cmd.Key}\t\t{cmd.Value.Description}" );
            }
        }

    }

}
