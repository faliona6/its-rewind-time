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
        private List<FrameAction> actions;
        // amount of fixedupdates total since beginning of replay
        private int fixedCount = 0;
        // curent index of interpVals the replayer is on
        // last index in frameActions that was replayed, used for O(1) searching
        private int lastActionIndex = 0;

        public ReplayState(RecordState recording, PropertyReplayer replayer)
        {
            this.replayer = replayer;
            interpVals = recording.InterpVals;
            actions = recording.PlayerActions;
        }

        public void OnLoopReset()
        {
            // reset all values of player and index of iterator through
            // replay loop
            // Note: need to think of a more abstract way of setting/getting values from structures/animating properties
            fixedCount = 0;
            SetProperties();
        }

        private void SetProperties()
        {
            replayer.GetPositionTransform().localPosition = interpVals[fixedCount].localPosition;
            replayer.GetRotationTransform().localRotation = interpVals[fixedCount].localRotation;
            replayer.GetCamTransform().localRotation = interpVals[fixedCount].camRotation;
        }

        private void InterpolateProperties()
        {
            // stop uneccessary calculations because this will be called a lot
            if (fixedCount >= (interpVals.Count - 3))
                return;
            fixedCount++;
            // replay the data every physics update
            // might need a more general way of getting values, but this is just a prototype for now.
            // have a method that asks for a particular thing to animate
            replayer.GetPositionTransform().position = interpVals[fixedCount].localPosition;
            replayer.GetRotationTransform().localRotation = interpVals[fixedCount].localRotation;
            replayer.GetCamTransform().localRotation = interpVals[fixedCount].camRotation;
        }

        public void FixedAction()
        {
            // interpolate between recorded values here/check if any actions on frame
            InterpolateProperties();
        }
    }
}
