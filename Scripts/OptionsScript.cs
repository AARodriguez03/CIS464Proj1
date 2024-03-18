using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsScript : MonoBehaviour // use to toggle full screen or not. Not sure what else to implement as of right now
{

    public void SetScreen(bool trigger)         //will make the screen full or not depending on the boolean
    {
        Screen.fullScreen = trigger;

    }
   
}
