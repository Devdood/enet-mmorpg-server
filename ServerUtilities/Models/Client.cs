using ENet;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesServer
{
    public class Client
    {
        public Peer Peer { get; set; }
        public CharacterInfo CharacterInfo { get; set; }

        public void SendData(PacketBase packet)
        {
            packet.Write();

            byte[] data = packet.GetWriteData();
            Packet p = new Packet();
            p.Create(data);

            Peer.Send(0, ref p);
        }
    }
}
