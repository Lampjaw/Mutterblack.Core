namespace Mutterblack.Core.Commands
{
    public class CommandResult
    {
        public CommandResult() { }
        public CommandResult(object result)
        {
            Success = true;
            Result = result;
        }

        public bool Success { get; set; }
        public string Error { get; set; }
        public object Result { get; set; }
    }
}
