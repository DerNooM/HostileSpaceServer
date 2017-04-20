using System;
using System.Collections.Generic;
using System.Linq;
using SFML.System;
using HostileSpaceNetLib;
using HostileSpaceNetLib.Packets;

namespace HostileSpaceServer
{
    class Game
    {
        HostileSpaceServer server;

        SpaceShip playerShip;

        Client client;

        public Game(HostileSpaceServer Server, Client Client)
        {
            server = Server;
            client = Client;
            client.PacketReceieved += Client_PacketReceieved;

            playerShip = new SpaceShip();

        }

        private void Client_PacketReceieved(object sender, EventArgs e)
        {
            switch (client.Packet.ID)
            {
                case PacketID.SetDestination:
                    {
                        SetDestination packet = new SetDestination(client.Packet);

                        playerShip.Destination = packet.Destination;

                    }
                    break;
            }
        }


        public void Update(Time Elapsed)
        {
            playerShip.Update(Elapsed);

            UpdateShip updateShip = new UpdateShip(playerShip.ShipData);
            if (client.Connected)
            {
                client.BeginSend(updateShip.Packet);
                
            }
            else
            {
                
            }
        }


        public Client Client
        {
            get { return client; }
        }


    }
}
