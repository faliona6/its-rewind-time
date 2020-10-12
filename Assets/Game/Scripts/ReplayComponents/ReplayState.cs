using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts
{
    public class ReplayState : ReplayableState
    {
        // should be contructed with a reference to
        // record state.
        // replayer is referenced so clones can be modified
        private PropertyReplayer replayer;
        private List<InterpVal> interpVals;
        private List<FrameAction> frameActions;
        // amount of fixedupdates total since beginning of replay
        private int fixedCount = 0;
        // curent index of interpVals the replayer is on
        private int fixedIndex = 0;
        // last index in frameActions that was replayed, used for O(1) searching
        private int lastActionIndex = 0;

        public ReplayState(RecordState recording, PropertyReplayer replayer)
        {
            this.replayer = replayer;
            interpVals = recording.GetInterpVals();
            frameActions = recording.GetFrameActions();
        }

        public void OnLoopReset()
        {
            // reset all values of player and index of iterator through
            // replay loop
            // Note: need to think of a more abstract way of setting/getting values from structures/animating properties
            fixedIndex = fixedCount = 0;
            SetProperties();
        }

        private void SetProperties()
        {
            replayer.GetOrientation().position = interpVals[fixedIndex].position;
            replayer.GetOrientation().rotation = interpVals[fixedIndex].rotation;
        }

        private void InterpolateProperties()
        {
            // stop uneccessary calculations because this will be called a lot
            if (fixedIndex >= (interpVals.Count - 3))
                return;
            fixedCount++;
            int pctThroughUpdate = (fixedCount % replayer.GetFramesPerSave());
            if (pctThroughUpdate == 0)
                fixedIndex++;
            // get percentage to lerp this frame by
            // remove method calls if it is slow here
            float pct = ((float)pctThroughUpdate) / replayer.GetFramesPerSave();
            // interpolate towards target values
            // might need a more general way of getting values, but this is just a prototype for now.
            // have a method that asks for a particular thing to animate
            replayer.GetOrientation().position = Vector3.Lerp(
                interpVals[fixedIndex].position,
                interpVals[fixedIndex + 1].position,
                pct);
            replayer.GetOrientation().rotation = Quaternion.Lerp(
                interpVals[fixedIndex].rotation,
                interpVals[fixedIndex + 1].rotation,
                pct);
        }

        public void FixedAction()
        {
            // interpolate between recorded values here/check if any actions on frame
            InterpolateProperties();
        }
    }
}
