using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class ParameterizedSignalEmitter<T> : SignalEmitter
{
    public T parameter;
}

public class IntegerSignalEmitter : ParameterizedSignalEmitter<int> { }
