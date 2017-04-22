using SFML.System;
using HostileSpaceNetLib;
using HostileSpaceNetLib.Packets;


namespace HostileSpaceServer
{
    class ServerComponent : IServerComponent
    {
        HostileSpaceServer server;


        public ServerComponent(HostileSpaceServer Server)
        {
            server = Server;
        }


        public virtual void Update(Time Elapsed)
        {
        }

        public virtual void HandlePacket(Client Client, IPacket Packet)
        {
        }
         
        
        public HostileSpaceServer Server
        {
            get { return server; }
        }


    }
}
