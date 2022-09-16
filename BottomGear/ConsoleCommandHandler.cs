using BottomGear.PiShock.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace BottomGear
{
    sealed class ConsoleCommandHandler
    {
        public void HandleCommand(string input)
        {
            if(input.StartsWith("strength", StringComparison.OrdinalIgnoreCase))
            {
                var desiredStrengthStr = input.Substring("strength ".Length);
                if(int.TryParse(desiredStrengthStr, out int newStrength))
                {
                    PiShockConfigProvider.Config.ShockStrengthOverride = newStrength;
                }
            }

            if (input.StartsWith("duration", StringComparison.OrdinalIgnoreCase))
            {
                var desiredDurationStr = input.Substring("duration ".Length);
                if (int.TryParse(desiredDurationStr, out int newDuration))
                {
                    PiShockConfigProvider.Config.ShockDurationOverride = newDuration;
                }
            }

            if (input.Equals("test", StringComparison.OrdinalIgnoreCase))
            {
                
            }
        }
    }
}
