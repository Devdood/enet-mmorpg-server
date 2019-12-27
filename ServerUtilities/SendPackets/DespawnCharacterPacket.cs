using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesServer.SendPackets
{
    public class DespawnCharacterPacket : PacketBase
    {
        public DespawnCharacterPacket(CharacterInfo character) : base()
        {
            writer.Write((byte)3);

            writer.Write(character.Character.Id);
        }

        public override void Read(Client client)
        {
        }

        public override void Write()
        {

        }
    }
}
