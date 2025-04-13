using UnityEngine;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>PlayerStateController</c> is a script that manages the player's state.
    /// </summary>
    public class PlayerStateController : MonoBehaviour
    {
        /// <value>Property <c>Current</c> returns the current state of the player.</value>
        private PlayerState Current { get; set; } = PlayerState.Idle;

        /// <summary>
        /// Method <c>SetState</c> sets the player's state to the specified state.
        /// </summary>
        /// <param name="state">The state to set.</param>
        public void SetState(PlayerState state)
        {
            Current |= state;
        }

        /// <summary>
        /// Method <c>UnsetState</c> unsets the player's state to the specified state.
        /// </summary>
        /// <param name="state">The state to unset.</param>
        public void UnsetState(PlayerState state)
        {
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
