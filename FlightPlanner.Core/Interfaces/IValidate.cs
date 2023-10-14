using FlightPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.Core.Interfaces
{
    public interface IValidate
    {
        bool IsValid(Flight flight);
    }
}
