using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FetchQuest : Quest
{




    public FetchQuest(string name, int fragmentReward, City destination) : base(name,fragmentReward)
    {
        Destination = destination;
    }

    public override void StartQuest()
    {
        map = GameObject.FindGameObjectWithTag("Map");
        //questMarker = GameObject.Instantiate();
    }
}
