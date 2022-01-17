using KadoshShared.Commands;

namespace KadoshDomain.Commands
{
    public class CommandResult : ICommandResult
    {
        #region Constructors
        public CommandResult()
        {
        }

        public CommandResult(bool success, string messagem)
        {
            Success = success;
            Messagem = messagem;
        }
        #endregion Constructors

        #region Properties
        public bool Success { get; set; }
        public string? Messagem { get; set; }
        #endregion Properties
    }
}
