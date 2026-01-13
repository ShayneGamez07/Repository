using System;
using System.Collections.Generic;
using System.Text;

namespace DinoPark.Core.Interface
{
    public interface IDangerous
    {
        int DangerLevel { get; }
        string GetWarningMessage();
    }
}
