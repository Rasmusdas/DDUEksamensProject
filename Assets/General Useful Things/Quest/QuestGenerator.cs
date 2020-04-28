using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    public List<QuestPair> quests;

    Dictionary<QuestType, Quest> dict = new Dictionary<QuestType, Quest>();

    private void Start()
    {
        foreach(var v in quests)
        {
            dict.Add(v.type,v.quest);
        }
    }

    public Quest GenerateQuest(QuestType type)
    {
        return dict[type];
    }
}

[System.Serializable]
public struct QuestPair
{
    public QuestType type;
    public Quest quest;
}
