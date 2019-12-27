using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesServer.SendPackets
{
    public class SpawnCharacterPacket : PacketBase
    {
        private bool local = false;

        public SpawnCharacterPacket(bool local, CharacterInfo character) : base()
        {
            writer.Write((byte)2);
            writer.Write(local);

            writer.Write(character.Character.Id);
            writer.Write(character.Character.BaseId);

            writer.Write(character.Character.Position.X);
            writer.Write(character.Character.Position.Y);
            writer.Write(character.Character.Position.Z);

            this.local = local;
        }

        public override void Read(Client client)
        {
        }

        public override void Write()
        {

        }
    }
}
