using System;
using System.Collections.Generic;
using System.Text;

namespace BottomGear.OSC.Messages
{
    public class VRCAnimatorParam
    {
        public string Name { get; private set; }
        public bool BoolValue
        {
            get
            {
                return Value == 1f;
            }
        }

        public float FloatValue
        {
            get
            {
                return Value;
            }
        }

        public int IntValue
        {
            get
            {
                return (int)Value;
            }
        }

        private float Value { get; set; }

        public VRCAnimatorParam(string path, bool value)
        {
            this.Name = path;
            this.Value = value ? 1.0f : 0.0f;
        }

        public VRCAnimatorParam(string path, float value)
        {
            this.Name = path;
            this.Value = value;
        }

        public VRCAnimatorParam(string path, int value)
        {
            this.Name = path;
            this.Value = (float)value;
        }
    }
}
