using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Scripts
{

    struct FrameActions
    {
        // Given frame the action(s) take place in
        // This may need to store larger values if rewinds
        // go on for much longer
        uint frame;
        List<Action> actionsOnFrame;
    }

    class RecordState : Replayable
    {
        private PropertyReplayer replayer;
        private List<FrameActions> frameActions;
        public RecordState(PropertyReplayer replayer)
        {
            this.replayer = replayer;
        }

        // store the desired properties of the player
        // every framesPerSave into an arrayList.
        public void FixedAction()
        {

        }
    }
}
