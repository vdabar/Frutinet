using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Services.Identity.Users
{
    public enum State
    {
        Inactive,
        Incomplete,
        Uncofirmed,
        Active,
        Locked,
        Deleted
    }
}