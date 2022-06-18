using static AppTracker.MessageBank.Contracts.IMessageBank;

namespace AppTracker.MessageBank.Contracts
{
    public interface IMessageResponse
    {
        public string Message { get; }
        public int Code { get; }
    }
}
