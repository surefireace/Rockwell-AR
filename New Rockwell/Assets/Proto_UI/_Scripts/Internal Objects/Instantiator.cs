using UnityEngine;
using System.Collections;
using System;

namespace Proto.Sbee
{
    public abstract class Instantiator : MonoBehaviour
    {
        public abstract IInstantiatable Instantiate(string name, HeadsetDevice headset, Transform root = null);
    }
}