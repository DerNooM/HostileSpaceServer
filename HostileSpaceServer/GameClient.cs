using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using HostileSpaceNetLib;
using HostileSpaceNetLib.Packets;
using SFML.System;

namespace HostileSpaceServer
{
    class GameClient : ServerComponent
    {
        Client client;

        PlayerData.AccountDataRow accountData;

        PlayerData.CharacterDataRow selectedCharacter;


        public GameClient(HostileSpaceServer Server, Client Client)
            : base (Server)
        {
            client = Client;
            client.PacketReceieved += Client_PacketReceieved;
        }


        public override void Update(Time Elapsed)
        {
        }

        private void Client_PacketReceieved(object sender, EventArgs e)
        {
            if (client.Packet.ID == PacketID.CharacterRequest)
            {
                HandleCharacterRequest();
            }
        }


        void HandleCharacterRequest()
        {
            CharacterResponse response = new CharacterResponse();
            DataRow[] tmp = accountData.GetChildRows("AccountData_CharacterData");

            if (tmp.Length == 0)
            {
                Console.WriteLine("0 characters");
            }
            else
            {
                for(int i = 0; i < tmp.Length; i++)
                {
                    response.Names[i] = (tmp[i] as PlayerData.CharacterDataRow).Name;
                    response.Images[i] = (tmp[i] as PlayerData.CharacterDataRow).Image;
                }
            }
            response.CharacterCount = (Byte)tmp.Length;
            client.BeginSend(response);
        }

        public Client Client
        {
            get { return client; }
        }

        public PlayerData.AccountDataRow AccountData
        {
            get { return accountData; }
            set { accountData = value; }
        }


    }
}
