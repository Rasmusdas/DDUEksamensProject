using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMap : MonoBehaviour
{
    public List<City> cities;
    public Image map;
    public GameObject cityTextPrefab;
    public GameObject playerPin;
    public RectTransform rectTrans;
    public GameObject player;
    void Start()
    {
        StartCoroutine(Initialize());
        rectTrans = playerPin.GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTrans.localPosition = new Vector2(player.transform.position.x, player.transform.position.z);
        rectTrans.localEulerAngles = player.transform.eulerAngles.y < 0 ?  new Vector3(0, 0, -player.transform.eulerAngles.y) : new Vector3(0, 0, -player.transform.eulerAngles.y + 360);
    }

    IEnumerator Initialize()
    {
        Texture2D gameMap = ExportTileMap.gameMap;
        if (gameMap != null)
        {
            cities = ExportTileMap.cities;
            gameMap = ExportTileMap.gameMap;
            map.sprite = Sprite.Create(gameMap, new Rect(0, 0, gameMap.width, gameMap.height), new Vector2(0.5f, 0.5f));
            foreach (City c in cities)
            {
                GameObject cityText = Instantiate(cityTextPrefab,new Vector3(0,0,-1),Quaternion.identity,map.transform);
                RectTransform rT = cityText.GetComponent<RectTransform>();
                rT.sizeDelta = new Vector2(500,250);
                rT.localPosition = new Vector3(c.xLocation, c.yLocation, -1);
                cityText.GetComponent<Text>().text = c.name;
                c.text = cityText.GetComponent<Text>();
            }
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(Initialize());
        }
    }
}
