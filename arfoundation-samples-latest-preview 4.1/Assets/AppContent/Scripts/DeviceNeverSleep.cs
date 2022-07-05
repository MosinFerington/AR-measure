using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceNeverSleep : MonoBehaviour
{
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

}
