using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPE.AlphaServer.Game;

internal enum EntityType : byte {
    Chicken = 0xA,
    Cow = 0xB,
    Pig = 0xC,
    Sheep = 0xD,

    Zombie = 0x20,
    Creeper = 0x21,
    Skeleton = 0x22,
    Spider = 0x23,
    PigZombie = 0x24,


    ItemEntity = 0x40,
    PrimedTnt = 0x41,
    FallingTile = 0x42,
    Arrow = 0x50,
    Snowball = 0x51,
    Egg = 0x52,
    Painting = 0x53,
    Minecart = 0x54,
}