using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    City destination;
    public GameObject player;
    public GameObject woohoo;
    bool finished;
    void Start()
    {
        destination = ExportTileMap.cities[Random.Range(0, ExportTileMap.cities.Count - 1)];
        StartCoroutine(StartQuest());
        woohoo.SetActive(false);
    }

    void Update()
    {
        if (Vector3.Distance(new Vector3(player.transform.position.x, 0, player.transform.position.z), new Vector3(destination.xLocation, 0, destination.yLocation)) < 10)
        {
            StartCoroutine(Finished());
        }
    }

    IEnumerator Finished()
    {
        if(!finished)
        {
            finished = true;
            woohoo.SetActive(true);
            yield return new WaitForSeconds(10);
            woohoo.SetActive(false);
            destination.text.color = Color.green;
        }
    }

    IEnumerator StartQuest()
    {
        while(destination.text == null)
        {
            yield return new WaitForEndOfFrame();
        }
        destination.text.text = "Hamstervale";
        destination.text.transform.position += Vector3.forward * 10;
        destination.name = "Hamstervale";
        destination.text.color = Color.red;
    }
}
