namespace vscli {

    internal interface ICommand {
        string Description { get; }
        int Run( string[] args );
    }

}
