using UnityEngine;

namespace FallingStar.Expedition
{
    /// <summary>
    /// Holds reference to runtim-only expedition objects for the current
    /// star. This makes cleanup easy when we later implement the jump reset.
    /// </summary>
    public static class ExpeditionRuntimeContext
    {
        public static Transform FieldRoot { get; private set; }

        public static void SetFieldRoot(Transform root)
        {
            FieldRoot = root;
        }

        public static void Clear()
        {
            FieldRoot = null;
        }

    }
}
