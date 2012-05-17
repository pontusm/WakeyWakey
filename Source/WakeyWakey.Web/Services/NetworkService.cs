using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;

namespace WakeyWakey.Web.Services
{
    // Adapted version from:
    // http://blog.memos.cz/index.php/team/2008/06/12/wake-on-lan-in-csharp
    public class NetworkService
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

        public byte[] GetMACAddress(string hostNameOrAddress)
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(hostNameOrAddress);
            if (hostEntry.AddressList.Length == 0)
                return null; // We were not able to resolve given hostname / address 

            int ipaddr = BitConverter.ToInt32(hostEntry.AddressList.First().GetAddressBytes(), 0);

            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;
            if (SendARP(ipaddr, 0, macAddr, ref macAddrLen) != 0)
                return null; // The SendARP call failed 

            return macAddr;
        }

        public void SendMagicPacket(byte[] macAddress)
        {
            //var macAddress = new byte[] { 0x00, 0xe1, 0xff, 0x65, 0x23, 0x10 };

            //Construct the packet 
            var packet = new List<byte>();

            //Trailer of 6 FF packets 
            for (int i = 0; i < 6; i++)
                packet.Add(0xFF);

            //Repeat 16 time the MAC address (which is 6 bytes) 
            for (int i = 0; i < 16; i++)
                packet.AddRange(macAddress);

            //Send the packet to broadcast address 
            var client = new UdpClient();
            client.Connect(IPAddress.Broadcast, 0);
            client.Send(packet.ToArray(), packet.Count);
        }
    }
}