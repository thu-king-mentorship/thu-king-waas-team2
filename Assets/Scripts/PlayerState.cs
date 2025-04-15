using System;

namespace WAAS
{
    /// <summary>
    /// Class <c>PlayerState</c> is an enumeration that defines the various states a player can be in.
    /// </summary>
    [Flags]
    public enum PlayerState
    {
        None            = 0,
        Moving          = 1 << 0,
        Jumping         = 1 << 1,
        AttackingMelee  = 1 << 2,
        AttackingRanged = 1 << 3
    }
}