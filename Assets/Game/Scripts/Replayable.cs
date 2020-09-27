using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Scripts
{
    interface Replayable
    {
        // either loads or saves the state if
        //
        void FixedAction();
    }
}
