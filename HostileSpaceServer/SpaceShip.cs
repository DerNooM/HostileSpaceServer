using System;
using System.Collections.Generic;
using System.Linq;
using SFML.System;
using HostileSpaceNetLib.Packets;

namespace HostileSpaceServer
{
    class SpaceShip
    {
        //UpdateShip.ShipData shipData = new UpdateShip.ShipData();
        Vector2f direction = new Vector2f(0, 0);
        /*

        Single acceleration = 0.0f;
        Single maxSpeed = 0.3f;
        Single turnRate = 0.002f;

        Single rotation = 0.0f;

        public SpaceShip()
        {

        }


        public void Update(Time Elapsed)
        {
            if (GetDistance(shipData.Position, shipData.Destination) < 50)
            {
                acceleration = 0;
                return;
            }


            if (GetDistance(shipData.Position, shipData.Destination) > 200)
            {
                if (acceleration < 1.0f)
                {
                    acceleration += 0.0005f * Elapsed.AsMilliseconds();
                }
            }
            else
            {
                if (acceleration > 0.2f)
                {
                    acceleration -= 0.0008f * Elapsed.AsMilliseconds();
                }
            }


            if (shipData.Rotation >= 360)
            {
                shipData.Rotation = 0;
            }
            if (shipData.Rotation <= -360)
            {
                shipData.Rotation = 0;
            }

            direction = shipData.Destination - shipData.Position;
            rotation = (float)Math.Atan2(direction.Y, direction.X);

            rotation = RadianToDegree(rotation);

            shipData.Rotation -= (turnRate * Elapsed.AsMilliseconds()) * ShortestRotation(rotation, shipData.Rotation);


            direction.Y = (Single)Math.Sin(DegreeToRadian(shipData.Rotation)) * maxSpeed * acceleration * Elapsed.AsMilliseconds();
            direction.X = (Single)Math.Cos(DegreeToRadian(shipData.Rotation)) * maxSpeed * acceleration * Elapsed.AsMilliseconds();


            shipData.Position += direction;

        }

        Single ShortestRotation(Single start, Single end)
        {
            Single diff = end - start;

            if (diff > 180.0f)
                diff -= 360.0f;
            else if (diff < -180.0f)
                diff += 360.0f;

            return diff;
        }


        Single DegreeToRadian(Single angle)
        {
            return (Single)Math.PI * angle / 180.0f;
        }

        Single RadianToDegree(Single angle)
        {
            return angle * (180.0f / (Single)Math.PI);
        }

        static Single GetDistance(Vector2f A, Vector2f B)
        {
            return (Single)Math.Sqrt(Math.Pow((B.X - A.X), 2) + Math.Pow((B.Y - A.Y), 2));
        }


        public Single Rotation
        {
            get { return shipData.Rotation; }
            set { shipData.Rotation = value; }
        }

        public Vector2f Position
        {
            get { return shipData.Position; }
            set { shipData.Position = value; }
        }

        public Vector2f Destination
        {
            get { return shipData.Destination; }
            set { shipData.Destination = value; }
        }

        public UpdateShip.ShipData ShipData
        {
            get { return shipData; }
        }

    */
    }
}
