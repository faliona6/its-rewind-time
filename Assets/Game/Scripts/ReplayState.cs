using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Scripts
{
    class ReplayState : Replayable
    {
        // should be contructed with a reference to
        // record state.
        // replayer is referenced so clones can be modified
        private PropertyReplayer replayer;
        public ReplayState(RecordState recording, PropertyReplayer replayer)
        {
            this.replayer = replayer;
        }

        public void FixedAction()
        {

        }
    }
}
