using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class CreateCities : MonoBehaviour
{
    public House house;
    public City cy;
    public List<City> cityrino;
    void Start()
    {
        cityrino = ExportTileMap.cities;
        foreach (City city in ExportTileMap.cities)
        {
            int citySizeModify = 0;
            DiskSampling.GenerateDiskSamples(3, 5, city.size + citySizeModify, city.size + citySizeModify, out Vector2[,] grid);
            List<List<Vector2>> housePositions = DiskSampling.CleanDiskSampling(grid);
            foreach(List<Vector2> list in housePositions)
            {
                foreach(Vector2 pos in list)
                {
                    city.houses.Add(new House(house.housePrefab, new Vector3(pos.x - city.size / 2, pos.y-city.size/2), house.type, city));
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
                GenerateCity(c, ent);
            }
        }
    }

    void GenerateCity(City city, TileEntity ent)
    {
        foreach(House h in city.houses)
        {
            if(!h.generated)
            {
                if (Vector3.Distance(new Vector3(ent.gameObject.transform.position.x, 0, ent.gameObject.transform.position.z), new Vector3(city.xLocation+h.position.x, 0, city.yLocation + h.position.y)) < ent.dissolveRange-1)
                {
                    GameObject hus = ObjectPooling.objectPool.InstantiateFromPool(PoolType.House, new Vector3(city.xLocation + h.position.x, 3, city.yLocation + h.position.y));
                    hus.GetComponent<Tile>().house = h;
                    hus.GetComponent<Tile>().house.generated = true;
                }
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
