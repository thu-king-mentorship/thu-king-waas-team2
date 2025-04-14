using UnityEngine;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>PlayerStateController</c> is a script that manages the player's state.
    /// </summary>
    public class PlayerStateController : MonoBehaviour
    {
        /// <value>Property <c>Current</c> returns the current state of the player.</value>
        private PlayerState Current { get; set; }

        /// <summary>
        /// Method <c>SetState</c> sets the player's state to the specified state.
        /// </summary>
        /// <param name="state">The state to set.</param>
        /// <param name="set">If true, sets the state; otherwise, unsets it.</param>
        public void SetState(PlayerState state, bool set = true)
        {
            if (set)
                Current |= state;
            else
                Current &= ~state;
        }

        /// <summary>
        /// Method <c>HasState</c> checks if the player has the specified state.
        /// </summary>
        /// <param name="state">The state to check.</param>
        /// <returns>True if the player has the specified state; otherwise, false.</returns>
        public bool HasState(PlayerState state)
        {
            return (Current & state) != 0;
        }

        /// <summary>
        /// Method <c>ClearAllStates</c> clears all states of the player.
        /// </summary>
        public void ClearAllStates()
        {
            Current = PlayerState.None;
        }
    }

}
