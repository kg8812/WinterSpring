using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ParameterizedSignalReciever<T> : MonoBehaviour, INotificationReceiver
{
    public SignalAssetEventPair[] signalAssetEventPairs;

    [Serializable]
    public class SignalAssetEventPair
    {
        public SignalAsset signalAsset;
        public ParameterizedEvent events;

        [Serializable]
        public class ParameterizedEvent : UnityEvent<T> { }
    }


    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if(notification is ParameterizedSignalEmitter<T> emitter)
        {
            var matches = signalAssetEventPairs.Where(x => ReferenceEquals(x.signalAsset, emitter.asset));
            foreach(var m in matches)
            {
                m.events.Invoke(emitter.parameter);
            }
        }
    }
}
