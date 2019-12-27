using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace HeroesServer.ReceivedPackets
{
    public class PlayerStatePacket : PacketBase
    {
        public PlayerStatePacket() : base()
        {
        }

        public override void Read(Client client)
        {
            Vector3 pos = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

            client.CharacterInfo.Character.Position = pos;
        }

        public override void Write()
        {
        }
    }
}
