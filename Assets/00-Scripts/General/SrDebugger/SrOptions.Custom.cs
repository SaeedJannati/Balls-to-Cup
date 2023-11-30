using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using BallsToCup.General;
using UnityEngine;

public partial class SROptions
{
    public static readonly BaseEventController.SimpleEvent<int> onUnlockLevelRequest = new();
    [Category("General"), Sort(0)] public int Value 
    { get; set; }

    [Category("General"), Sort(1)]
    public void UnlockLevel()
    {
        onUnlockLevelRequest.Trigger(Value);
    }
}
