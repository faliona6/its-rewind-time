using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Scripts
{
    class ReplayState : ReplayableState
    {
        // should be contructed with a reference to
        // record state.
        // replayer is referenced so clones can be modified
        private PropertyReplayer replayer;
        public ReplayState(RecordState recording, PropertyReplayer replayer)
        {
            this.replayer = replayer;
        }

        public void OnLoopReset()
        {
            // reset all values of player and index of iterator through
            // replay loop
        }

        public void FixedAction()
        {
            // interpolate between recorded values here.
        }
    }
}
