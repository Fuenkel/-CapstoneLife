using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace HJ.Manager
{
    public abstract class EventPageManager : MonoBehaviour
    {
        public string msg;
        public abstract void Play();

        public override string ToString()
        {
            return base.ToString();
        }

        private void Awake()
        {
            
        }
    }
}

