using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Scripts
{
    public interface ReplayableState
    {
        // either loads or saves the state based on the fixed update loop
        void FixedAction();

        // what each state should do given that the loop has ended, assuming
        // the character continues on to the next loop
        // record -> replay
        // replay -> start from beginning
        void OnLoopReset();
    }
}
