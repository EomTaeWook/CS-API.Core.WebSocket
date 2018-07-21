using System;
using System.Collections.Generic;
using System.Text;

namespace API.Core.WebSocket.InternalStructure
{
    public interface IProtectedData
    {
        string Unprotect(string connectionToken);
        string Protect(string connectionToken);
    }
}
