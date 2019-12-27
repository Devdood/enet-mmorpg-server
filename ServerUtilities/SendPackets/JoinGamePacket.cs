using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesServer.SendPackets
{
    public class JoinGamePacket : PacketBase
    {
        public JoinGamePacket() : base()
        {
        }

        public override void Read(Client client)
        {
        }

        public override void Write()
        {
            writer.Write((byte)1);
        }
    }
}
