using UnityEngine;

namespace FallingStar.Run
{
    /// <summary>
    /// Tracks run progression across star system.
    /// Each jump increases StarIndex (difficulty level)
    /// </summary>
    public class RunProgress : MonoBehaviour
    {
        public int StartIndex { get; private set; }

        public void AdvanceStar()
        {
            StartIndex += 1;
        }

        public void ResetRun()
        {
            StartIndex = 1;
        }
    }
}
