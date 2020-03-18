﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExportTileMap : MonoBehaviour
{
    public int mapSize;
    public int randomVariation;
    public int sizeOfCity;
    public static Texture2D gameMap;
    public static Color grey;

    private void Awake()
    {
        randomVariation = UnityEngine.Random.Range(-1000000, 1000000);
        grey = Color.grey;
    }
    void Start()
    {
        Texture2D tileMap = CreateTileMap(mapSize, mapSize);

        tileMap = AddCitiesToMap(tileMap, sizeOfCity, Color.green, Color.magenta);

        tileMap = AddCitiesToMap(tileMap, sizeOfCity-2, Color.yellow, Color.black);

        gameMap = tileMap;

        SaveTextureToFile(tileMap);
    }


    /// <summary>
    /// Creates a base landscape
    /// </summary>
    /// <param name="xSize"> The size of the map on the x axis. </param>
    /// <param name="ySize"> The size of the map on the y axis. </param>
    /// <returns></returns>
    Texture2D CreateTileMap(int xSize, int ySize)
    {
        Texture2D texture = new Texture2D(xSize, ySize);


        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                float treeGeneration = Mathf.PerlinNoise((i + randomVariation) * 0.1f, (j + randomVariation) * 0.1f);
                float height = Mathf.PerlinNoise(i * 0.01f, j * 0.01f);
                Color c = Color.black;

                if (height > 0.90f)
                {
                    c = Color.white;
                }
                else if (height > 0.75f)
                {
                    c = Color.grey;
                }
                else if (height > 0.5f && treeGeneration > 0.7f)
                {
                    c = Color.cyan;
                }
                else if (height > 0.5f)
                {
                    c = Color.green;
                }
                else if (height > 0.45f)
                {
                    c = Color.yellow;
                }
                else if (height <= 0.45f)
                {
                    c = Color.blue;
                }
                texture.SetPixel(i, j, c);
            }
        }

        texture.Apply();

        return texture;
    }

    /// <summary>
    /// Adds cities to the tilemap.
    /// </summary>
    /// <param name="map"> The map to add cities to </param>
    /// <param name="citySize"> The size of the cities </param>
    /// <param name="landScapeColor"> The color that's defined as landscape </param>
    /// <param name="cityColor"> The color of the cities </param>
    /// <returns></returns>
    Texture2D AddCitiesToMap(Texture2D map, int citySize,Color landScapeColor, Color cityColor)
    {
        for (int x = 0; x < map.height - citySize; x++)
        {
            for (int y = 0; y < map.width - citySize; y++)
            {
                bool generateCity = true;
                for (int i = 0; i < citySize; i++)
                {
                    for (int j = 0; j < citySize; j++)
                    {
                        if(map.GetPixel(x+i,y+j) != landScapeColor)
                        {
                            generateCity = false;
                            break;
                        }
                    }
                    if(!generateCity)
                    {
                        break;
                    }
                }

                if(generateCity)
                {
                    for (int i = 0; i < citySize; i++)
                    {
                        for (int j = 0; j < citySize; j++)
                        {
                            map.SetPixel(x + i, y + j, cityColor);
                            if (i == 0 || i + 1 == citySize)
                            {
                                map.SetPixel(x + i, y + j, Color.red);
                            }
                            if(j == 0 || j + 1 == citySize)
                            {
                                map.SetPixel(x + i, y + j, new Color(0.8f,0.8f,0.8f));
                            }
                            if ((i == 0 && j == 0) || (i + 1 == citySize && j == 0) || (i == 0 && j + 1 == citySize) || (i + 1 == citySize && j + 1 == citySize))
                            {
                                map.SetPixel(x + i, y + j, grey);
                            }
                        }
                    }
                }
            }
        }

        map.Apply();

        return map;
    }

    /// <summary>
    /// Saves the tilemap as Tilemap.png to the documents folder in PNG format.
    /// </summary>
    /// <param name="map"> The tilemap to export </param>
    void SaveTextureToFile(Texture2D map)
    {
        File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tilemap.png", map.EncodeToPNG());
    }

}