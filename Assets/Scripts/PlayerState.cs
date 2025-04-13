using System;

namespace WAAS
{
    /// <summary>
    /// Class <c>PlayerState</c> is an enumeration that defines the various states a player can be in.
    /// </summary>
    [Flags]
    public enum PlayerState
    {
        None        = 0,
        Idle        = 1 << 0,
        Moving      = 1 << 1,
        Jumping     = 1 << 2,
        Attacking   = 1 << 3,
        Interacting = 1 << 4,
        Dashing     = 1 << 5
    }
}