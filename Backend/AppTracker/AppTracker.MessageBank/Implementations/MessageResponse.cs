using AppTracker.MessageBank.Contracts;

namespace AppTracker.MessageBank.Implementations
{
    public class MessageResponse: IMessageResponse, IEquatable<MessageResponse>
    {
        public string Message { get; }
        public int Code { get;  }
        public bool Equals(MessageResponse? obj)
        {
            if (obj != null)
            {
                return Message.Equals(obj.Message) && Code.Equals(obj.Code);
            }
            return false;
        }

        public MessageResponse(string message, int code)
        {
            Message = message;
            Code = code;
        }
    }
}
