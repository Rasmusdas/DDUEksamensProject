using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public string QuestName { get; protected set; }
    public int StatueFragmentReward { get; protected set; }
    public City Destination { get; protected set; }

    public GameObject questMarker;
    protected GameObject map;

    public Quest(string name, int fragmentAmount)
    {
        QuestName = name;
        StatueFragmentReward = fragmentAmount;
    }

    public virtual void StartQuest()
    {
        
    }

    public virtual void EndQuest()
    {

    }
    
    public Quest MakeCopy()
    {
        Quest q = Instantiate(this);
        q.QuestName = "owo";
        return q;
    }
}


public enum QuestType
{
    Fetch
}
