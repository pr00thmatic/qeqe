using UnityEngine;
using System.Collections;

namespace Lvl {
    public class LevelLink {
        public LevelStatus initial;
        public LevelStatus final;
        public int index; // index of the level the link comes from.

        public LevelLink () {
            initial = LevelController.GetCurrentLevelStatus();
            index = LevelController.instance.currentLevel;
        }

        public void SetFinal (Level lvl) {
            final = new LevelStatus(lvl);
        }

        public void Undo () {
            final = null;
        }
    }
}
