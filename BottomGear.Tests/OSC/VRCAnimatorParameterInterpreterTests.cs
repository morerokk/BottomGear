using BottomGear.OSC.Interpreters;
using Rug.Osc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BottomGear.Tests.OSC
{
    public class VRCAnimatorParameterInterpreterTests
    {
        [Fact]
        public void TestInterpretOscMessage_Given05f_ShouldBe05f()
        {
            var interpreter = new VRCAnimatorParameterInterpreter();
            var message = new OscMessage("/avatar/parameters/TestParameter", 0.5f);

            var interpretedParam = interpreter.InterpretOscMessage(message);

            Assert.Equal(0.5f, interpretedParam.FloatValue);
        }

        [Fact]
        public void TestInterpretOscMessage_GivenTrue_ShouldBeTrue()
        {
            var interpreter = new VRCAnimatorParameterInterpreter();
            var message = new OscMessage("/avatar/parameters/TestParameter", true);

            var interpretedParam = interpreter.InterpretOscMessage(message);

            Assert.True(interpretedParam.BoolValue);
            Assert.Equal(1.0f, interpretedParam.FloatValue);
            Assert.Equal(1, interpretedParam.IntValue);
        }

        [Fact]
        public void TestInterpretOscMessage_Given78_ShouldBe78()
        {
            var interpreter = new VRCAnimatorParameterInterpreter();
            var message = new OscMessage("/avatar/parameters/TestParameter", 78);

            var interpretedParam = interpreter.InterpretOscMessage(message);

            Assert.Equal(78, interpretedParam.IntValue);
        }
    }
}
