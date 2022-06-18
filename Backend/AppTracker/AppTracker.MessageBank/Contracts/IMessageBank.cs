using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTracker.MessageBank.Contracts
{
    public interface IMessageBank
    {
        public enum Responses
        {
            // Essential
            success,
            unhandledException,
            operationCancelled,
            operationTimeExceeded,
            principalNotSet,

            // Database
            databaseConnectionFail,

            // Account Verification
            accountVerificationSuccess,
            accountNotEnabled,
            accountNotConfirmed,
            accountNotFound,

            // Account Authentication
            authenticationSuccess
        }
        public Task<IMessageResponse> GetMessageAsync(Responses response, CancellationToken cancellationToken = default(CancellationToken));
    }
}
