using UnityEngine;
using Common.BFB;

namespace Common.Game
{
    public static class GameUtils
    {
        public static IBFBGameProvider FindBFBGameProvider()
        {
            var bfb = GameObject.FindObjectOfType<BFBGameProvider>();
            if (bfb == null)
            {
                Debug.LogError("IGameProvider go not found!");
            }

            return bfb;
        }
    }
}