using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRGazeInteraction
{
    [System.Serializable]
    public class GazeEvent : UnityEvent<GameObject> { }

    public class GazeEvents : MonoBehaviour
    {
        public GazeState gazeState;

        [Range(0, 6)]
        public float activationTime;

        public GazeEvent OnGazeEnter;
        public GazeEvent OnGazeActivated;
        public GazeEvent OnGazeLeft;
    }
}