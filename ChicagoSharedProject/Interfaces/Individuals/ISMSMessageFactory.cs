using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tabs.Mobile.Shared.Interfaces.Individuals
{
    public interface ISMSMessageFactory
    {

        Task SendInvitation(string phoneNumber, string senderName, string receiverName);

    }
}
