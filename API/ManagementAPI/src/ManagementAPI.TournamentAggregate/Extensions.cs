using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.TournamentAggregate
{
    internal static class Extensions
    {
        public static Int32 RoundOff (this Int32 i)
        {
            return ((Int32)Math.Round(i / 10.0)) * 10;
        }
    }
}
