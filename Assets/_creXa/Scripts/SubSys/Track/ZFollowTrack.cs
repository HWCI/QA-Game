using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creXa.GameBase
{
    public class ZFollowTrack : MonoBehaviour
    {
        public GameObject targetObject;
        public ZTrack track;

        public  AnimationCurve timeCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public bool play = false;
        public bool pause = false;
        public bool playOnAwake = false;

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
