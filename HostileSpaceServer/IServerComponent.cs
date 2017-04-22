using SFML.System;
using HostileSpaceNetLib;
using HostileSpaceNetLib.Packets;


namespace HostileSpaceServer
{
    interface IServerComponent
    {
        HostileSpaceServer Server { get; }

        void Update(Time Elapsed);
        void HandlePacket(Client Sender, IPacket Packet);


    }
}
