using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Text;
using System.Threading;

namespace HeroesServer
{
    public class CharactersManager
    {
        public static CharactersManager Instance;
        public Dictionary<int, CharacterInfo> characters = new Dictionary<int, CharacterInfo>();
        public Dictionary<int, Dictionary<int, CharacterInfo>> zonesBucket = new Dictionary<int, Dictionary<int, CharacterInfo>>();

        public static int lastId = 1;

        private Thread updateThread;

        public CharactersManager()
        {
            Instance = this;

            zonesBucket.Add(0, new Dictionary<int, CharacterInfo>());
            LoadMonstersDatabase();

            updateThread = new Thread(new ThreadStart(UpdateCharacters));
            updateThread.Start();
        }

        private void LoadMonstersDatabase()
        {
            DataTable table = DatabaseUtils.ReturnQuery("SELECT * FROM mobs_spawn");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                int baseId = (int)table.Rows[i]["mob_id"];
                Vector3 pos = new Vector3((float)(double)table.Rows[i]["pos_x"], (float)(double)table.Rows[i]["pos_y"], (float)(double)table.Rows[i]["pos_z"]);
                float respawnTime = (int)table.Rows[i]["respawn_time"];
                int zoneId = (int)table.Rows[i]["zone_id"];

                AddCharacter(new CharacterInfo()
                {
                    Character = new Character()
                    {
                        Id = lastId++,
                        BaseId = (short)baseId,
                        Position = pos
                    },
                    ZoneId = zoneId
                });
            }
        }

        private void UpdateCharacters()
        {
            while (true)
            {
                foreach (var zone in zonesBucket)
                {
                    foreach (var info in zone.Value.Values)
                    {
                        info.Update();
                    }
                }

                Thread.Sleep(100);
            }
        }

        public CharacterInfo AddCharacter(CharacterInfo info)
        {
            int newId = lastId++;
            info.Character.Id = newId;

            characters.Add(info.Character.Id, info);
            zonesBucket[info.ZoneId].TryAdd(info.Character.Id, info);

            foreach (var item in zonesBucket[info.ZoneId])
            {
                if (item.Value.Character.Id != info.Character.Id)
                {
                    if (item.Value.Client != null)
                    {
                        item.Value.Client.SendData(new SendPackets.SpawnCharacterPacket(false, info));
                    }
                }
            }

            return info;
        }

        public void RemoveCharacter(CharacterInfo info)
        {
            characters.Remove(info.Character.Id);
            zonesBucket[info.ZoneId].Remove(info.Character.Id);

            foreach (var item in zonesBucket[info.ZoneId])
            {
                if (item.Value.Client != null)
                {
                    item.Value.Client.SendData(new SendPackets.DespawnCharacterPacket(info));
                }
            }
        }
        public CharacterInfo GetCharacterInfo(int id)
        {
            return characters[id];
        }

        public bool GetCharacterInfo(int id, out CharacterInfo character)
        {
            if(characters.ContainsKey(id))
            {
                character = characters[id];
                return true;
            }
            else
            {
                character = null;
                return false;
            }
        }
    }

    public class CharacterInfo
    {
        public int ZoneId { get; set; }
        public Client Client { get; set; }
        public Character Character { get; set; }

        public void Update()
        {
            if(Client != null)
            {
                UpdateClient();
            }
        }

        private void UpdateClient()
        {
            List<SendPackets.SetPositionsPacket.CharacterSnapshot> snapshots = new List<SendPackets.SetPositionsPacket.CharacterSnapshot>();
            foreach (var item in CharactersManager.Instance.zonesBucket[ZoneId])
            {
                if (item.Value.Character.Id != Character.Id)
                {
                    snapshots.Add(new SendPackets.SetPositionsPacket.CharacterSnapshot()
                    {
                        id = item.Value.Character.Id,
                        pos = item.Value.Character.Position
                    });
                }
            }

            Client.SendData(new SendPackets.SetPositionsPacket(snapshots));
        }
    }
}
