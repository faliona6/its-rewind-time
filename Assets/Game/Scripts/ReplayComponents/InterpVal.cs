using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;

namespace Assets.Game.Scripts
{
    public class InterpVal
    {
        // animation data (abiguously stored currently)
        // position data
        // rotation data

        // whatever other data we want to interpolate
        // if these variables get unweildy, we might have to figure
        // out a better way of storing data.

        // if we want better security or something to happen
        // when we mutate these variables, these shouldn't be
        // public, but whatever for now.
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Quaternion camRotation;
        public InterpVal(Vector3 position, Quaternion rotation, Quaternion camRotation)
        {
            this.localPosition = position;
            this.localRotation = rotation;
            this.camRotation = camRotation;
        }
    }
}
