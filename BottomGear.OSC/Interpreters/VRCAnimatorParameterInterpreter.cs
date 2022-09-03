using BottomGear.OSC.Messages;
using Rug.Osc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BottomGear.OSC.Interpreters
{
    public class VRCAnimatorParameterInterpreter
    {
        public VRCAnimatorParam InterpretOscMessage(OscMessage message)
        {
            //return new VRCAnimatorParamMessage(message.Address, message[0]);
            object value = message[0];
            Type type = value.GetType();

            string address = message.Address;
            if(!address.StartsWith("/avatar/parameters/"))
            {
                throw new ArgumentException("Message address must start with \"/avatar/parameters\" to be interpreted as an animator parameter.", nameof(message));
            }

            var paramName = address.Substring("/avatar/parameters/".Length);

            if(type == typeof(float))
            {
                return new VRCAnimatorParam(paramName, (float)value);
            }
            else if (type == typeof(bool))
            {
                return new VRCAnimatorParam(paramName, (bool)value);
            }
            else if (type == typeof(int))
            {
                return new VRCAnimatorParam(paramName, (int)value);
            }
            else
            {
                throw new ArgumentException("Unknown parameter type in OSC Message", nameof(message));
            }
        }
    }
}
