using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Map
{
#if UNITY_EDITOR

    [MenuItem("Tools/GenerateMap")]
    public static void GenerateMap()
    {
        GameObject stage = Resources.Load<GameObject>("Prefabs/Stage/Stage_001");
        Transform info = Helper.FindChild<Transform>(stage, "Info");
        List<Tilemap> collisions = Helper.FindChildren<Tilemap>(info.gameObject, "Collision");

        int zMax = collisions[0].cellBounds.yMax;
        int zMin = collisions[0].cellBounds.yMin;
        int xMax = collisions[0].cellBounds.xMax;
        int xMin = collisions[0].cellBounds.xMin;

        Directory.CreateDirectory($"Assets/Resources/StageData/{stage.name}");
        using (StreamWriter writer = File.CreateText($"Assets/Resources/StageData/{stage.name}/{stage.name}_Info.txt"))
        {
            writer.WriteLine(0);
            writer.WriteLine(collisions.Count - 1);
            writer.WriteLine(zMin);
            writer.WriteLine(zMax);
            writer.WriteLine(xMin);
            writer.WriteLine(xMax);

            writer.Close();
        }

        for (int y = 0; y < collisions.Count; y++)
        {
            using (StreamWriter writer =
                   File.CreateText($"Assets/Resources/StageData/{stage.name}/{stage.name}_Collision_{y}.txt"))
            {
                for (int z = zMax - 1; z > zMin; z--)
                {
                    for (int x = xMin; x < xMax; x++)
                    {
                        TileBase tile = collisions[y].GetTile(new Vector3Int(x, z, 0));
                        if (tile == null)
                        {
                            writer.Write('0');
                            continue;
                        }

                        switch (tile.name)
                        {
                            case "tileset_24":
                                writer.Write('1');
                                break;
                            case "rock":
                                writer.Write('2');
                                break;
                            case "tileset_4":
                                writer.Write('3');
                                break;
                        }
                    }

                    writer.WriteLine();
                }
            }
        }
    }

#endif
}