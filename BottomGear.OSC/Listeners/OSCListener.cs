using Rug.Osc.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace BottomGear.OSC.Listeners
{
    public class OSCListener : IDisposable
    {
        public event EventHandler<OscMessage> MessageReceived;

        private OscReceiver oscReceiver;

        public IPAddress IPAddress { get; private set; }

        public int Port { get; private set; }

        public OSCListener(IPAddress address, int port)
        {
            IPAddress = address;
            Port = port;
        }

        public void Start()
        {
            oscReceiver = new OscReceiver(IPAddress, Port);
            oscReceiver.Connect();

            try
            {
                while (oscReceiver.State != OscSocketState.Closed)
                {
                    // Check if we are connected yet
                    if (oscReceiver.State != OscSocketState.Connected)
                    {
                        Thread.Sleep(500);
                        continue;
                    }

                    // This is blocking, waits for the next message
                    OscPacket packet = oscReceiver.Receive();

                    var message = packet as OscMessage;
                    if (message == null)
                    {
                        continue;
                    }

                    OnOscMessageReceived(message);
                }
            }
            catch (Exception ex)
            {
                if (!ex.ToString().Contains("The receiver socket has been disconnected"))
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void OnOscMessageReceived(OscMessage message)
        {
            if (MessageReceived != null)
            {
                MessageReceived.Invoke(this, message);
            }
        }

        public void Dispose()
        {
            if(oscReceiver != null && (oscReceiver.State == OscSocketState.Connected || oscReceiver.State == OscSocketState.NotConnected))
            {
                oscReceiver.Close();
            }
        }
    }
}
