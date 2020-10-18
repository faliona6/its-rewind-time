using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Scripts
{

    public struct FrameAction
    {
        // Given frame the action(s) take place in
        // This may need to store larger values if rewinds
        // go on for much longer (uint val)
        int frame;
        List<Action> actionsOnFrame;
    }

    public class RecordState : ReplayableState
    {
        private PropertyReplayer replayer;
        // method calls on the player (shoot, jump)
        // replicated exactly by the frame unlike properties
        private List<FrameAction> frameActions;

        // storage of all non-frame-specific attributes
        // (position/velocity/animstate) that can be safely approximated
        private List<InterpVal> interpVals;

        // frame counter
        private byte fCount = 0;

        public RecordState(PropertyReplayer replayer)
        {
            // back reference to the replayer object
            this.replayer = replayer;
            frameActions = new List<FrameAction>();
            interpVals = new List<InterpVal>();
            // this class sets itself to the only recorder in the scene
        }

        public List<FrameAction> GetFrameActions()
        {
            return frameActions;
        }

        public List<InterpVal> GetInterpVals()
        {
            return interpVals;
        }

        public void OnLoopReset()
        {
            // record state should call a context switch on
            // the replayer component to replay state
            replayer.SwitchToReplay(this);
        }

        // this method takes the current state of the player
        // and saves it into 
        private void SaveState()
        {
            // if it has been a full frames per save since last save
            if((++fCount) % replayer.GetFramesPerSave() == 0)
            {
                // save the data
                interpVals.Add(new InterpVal(replayer.GetPosition().position,
                    replayer.GetRotation().localRotation));
            }
        }

        // store the desired properties of the player
        // every framesPerSave into an arrayList.
        public void FixedAction()
        {
            // every x frames, the state should be saved
            SaveState();
        }
    }
}
