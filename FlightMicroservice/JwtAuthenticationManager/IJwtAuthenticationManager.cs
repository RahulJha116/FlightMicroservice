using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMicroservice.IJwtAuthentication
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string AdminEmail, string AdminPasskey);
    }
}
