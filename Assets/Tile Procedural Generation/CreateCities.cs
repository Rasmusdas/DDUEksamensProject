using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class CreateCities : MonoBehaviour
{
    public House house;
    public City cy;
    void Start()
    {
        foreach(City city in ExportTileMap.cities)
        {
            Debug.Log(city.size);
            int citySizeModify = -2;
            DiskSampling.GenerateDiskSamples(Mathf.Sqrt(city.size-3), 15, city.size + citySizeModify, city.size + citySizeModify, out Vector2[,] grid);
            List<List<Vector2>> housePositions = DiskSampling.CleanDiskSampling(grid);
            foreach(List<Vector2> list in housePositions)
            {
                foreach(Vector2 pos in list)
                {
                    city.houses.Add(new House(house.housePrefab, new Vector3(pos.x + city.xLocation - 500 - (city.size + citySizeModify) / 2, pos.y + city.yLocation - 500 - (city.size + citySizeModify) / 2), house.type, city));
                }
            }
        }
    }

    private void Update()
    {
       foreach(TileEntity ent in CreateTiles.tileEntities)
       {
            foreach(City c in ExportTileMap.cities)
            {
                cy = c;
                if(Vector3.Distance(ent.gameObject.transform.position,new Vector3(c.xLocation-500,c.yLocation-500)) < ent.dissolveRange)
                {
                    GenerateCity(c);
                }
            }
        }
    }

    void GenerateCity(City city)
    {
        foreach(House h in city.houses)
        {
            if(!h.generated)
            {
                GameObject hus = ObjectPooling.objectPool.InstantiateFromPool(PoolType.House,new Vector3(h.position.x,3,h.position.y));
                hus.transform.Rotate(0,Random.Range(0, 4) * 90,0);
                hus.GetComponent<Tile>().house = h;
                hus.GetComponent<Tile>().heightTarget = 1.15f;
                hus.GetComponent<Tile>().house.generated = true;
            }
        }
    }
}

[System.Serializable]
public class House
{
    public GameObject housePrefab;
    public HouseType type;
    public Vector2 position;
    public bool generated;
    private City city;

    public House(GameObject housePrefab, Vector2 position, HouseType type, City city)
    {
        this.housePrefab = housePrefab;
        this.position = position;
        this.type = type;
        this.city = city;
        generated = false;
    }
    
}

public enum HouseType
{
    small,
    medium,
    big
}
