using HeroesServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerUtilities.ReceivedPackets
{
    public class JoinGamePacket : PacketBase
    {
        public override void Read(Client client)
        {
            //string username = reader.ReadString();
            //string password = reader.ReadString();

            //Console.WriteLine("Joined game: " + username + "/" + password);
            client.SendData(new HeroesServer.SendPackets.JoinGamePacket());
            client.SendData(new HeroesServer.SendPackets.SpawnCharacterPacket(true, client.CharacterInfo));

            foreach (var character in CharactersManager.Instance.zonesBucket[0].Values)
            {
                if (character.Character.Id == client.CharacterInfo.Character.Id)
                {
                    continue;
                }

                client.SendData(new HeroesServer.SendPackets.SpawnCharacterPacket(false, character));
            }
        }

        public override void Write()
        {
        }
    }
}
