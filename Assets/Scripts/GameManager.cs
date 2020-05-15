using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour
{
    public void GenerateNodes()
    {
        
        TerrainGenerator tg = TerrainGenerator.instance;
        var robots = FindObjectsOfType<FSMCharacter>();
        foreach (FSMCharacter character in robots)
        {
            tg.robotList.Remove(character.gameObject);
            Destroy(character.gameObject);
        }
        if (tg.sphereList.Count > 0)
        {
            foreach (GameObject gameObjects in tg.sphereList)
            {
                Destroy(gameObjects);
            }
            tg.sphereList.Clear();
        }

        tg.GenerateTerrain();
    }

    public void Save()
    {
        SaveData SD = new SaveData();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.OpenOrCreate);
        var robots = FindObjectsOfType<FSMCharacter>();
        var trees = FindObjectsOfType<ManageTree>();
        foreach(FSMCharacter character in robots)
        {
            SD.robotLocationx.Add(character.transform.position.x);
            SD.robotLocationy.Add(character.transform.position.y);
            SD.robotLocationz.Add(character.transform.position.z);
            SD.charge.Add(character.curCharge);
        }
        foreach (ManageTree tree in trees)
        {
            SD.treeLocationx.Add(tree.transform.position.x);
            SD.treeLocationy.Add(tree.transform.position.y);
            SD.treeLocationz.Add(tree.transform.position.z);
            SD.isTree.Add(tree.GetTreePlanted());
            SD.hunger.Add(tree.GetTreeHunger());
            SD.thirst.Add(tree.GetTreeThirst());
        }
        bf.Serialize(file, SD);
        file.Close();
        Debug.Log("Saved Info");
    }

    public void Load()
    {
        if(File.Exists (Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SaveData SD = (SaveData)bf.Deserialize(file);
            file.Close();
            var robots = FindObjectsOfType<FSMCharacter>();
            var trees = FindObjectsOfType<ManageTree>();
            for (int i = 0; i < robots.Length; i++)
            {
                FSMCharacter character = robots[i];
                character.transform.position = new Vector3(SD.robotLocationx[i], SD.robotLocationy[i], SD.robotLocationz[i]);
                character.curCharge = SD.charge[i];
            }

            for (int i = 0; i < trees.Length; i++)
            {
                ManageTree tree = trees[i];
                tree.transform.position = new Vector3(SD.treeLocationx[i], SD.treeLocationy[i], SD.treeLocationz[i]);
                tree.SetTreePlanted(SD.isTree[i]);
                tree.SetTreeHunger(SD.hunger[i]);
                tree.SetTreeThirst(SD.thirst[i]);
            }
            Debug.Log("Loaded Info");
        }
    }
}
