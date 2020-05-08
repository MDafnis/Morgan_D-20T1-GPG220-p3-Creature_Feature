using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTree : MonoBehaviour
{
    [SerializeField] private GameObject plotMesh, treeMesh;
    public bool treePlanted = false;
    public bool treeMaxHeight;

    public float defaultHungerTimer;
    public float hungerTimer = 20f;
    public bool treeHungry = false;
    public float treeHunger = 20f;
    public float treeFoodCapacity;

    public float defaultThirstTimer;
    public float thirstTimer = 20f;
    public bool treeThirsty = false;
    public float treeThirst = 20f;
    public float treeWaterCapacity;

    public float baseHeightGrowth = 2.5f;

    private void Start()
    {
        treeFoodCapacity = treeHunger;
        treeWaterCapacity = treeThirst;
        defaultHungerTimer = hungerTimer;
        defaultThirstTimer = thirstTimer;
    }

    private void Update()
    {
        if (treePlanted)
        {
            hungerTimer -= Time.deltaTime;
            if (hungerTimer <= 0)
            {
                treeHunger--;
                hungerTimer = 20f;
                if (!treeHungry && treeHunger <= (treeFoodCapacity / 4))
                {
                    treeHungry = true;
                    TerrainGenerator.instance.hungryTrees.Add(gameObject);
                }
            }

            thirstTimer -= Time.deltaTime;
            if (thirstTimer <= 0)
            {
                treeThirst--;
                thirstTimer = 20f;
                if (!treeThirsty && treeThirst <= (treeFoodCapacity / 4))
                {
                    treeThirsty = true;
                    TerrainGenerator.instance.thirstyTrees.Add(gameObject);
                }
            }

            if (treeHunger <= 0)
            {
                treeHungry = false;
                TerrainGenerator.instance.hungryTrees.Remove(gameObject);
                TerrainGenerator.instance.hungryTrees.Add(gameObject);
                treeMesh.SetActive(false);
                treePlanted = false;
            }
            if(treeThirst <= 0)
            {
                treeThirsty = false;
                TerrainGenerator.instance.thirstyTrees.Remove(gameObject);
                TerrainGenerator.instance.treePlot.Add(gameObject);
                treeMesh.SetActive(false);
                treePlanted = false;
            }
            Mathf.Clamp(treeHunger, 0f, treeFoodCapacity);
            Mathf.Clamp(treeThirst, 0f, treeWaterCapacity);
        }
    }

    public void plantTree()
    {
        TerrainGenerator.instance.plantedTrees.Add(gameObject);
        TerrainGenerator.instance.treePlot.Remove(gameObject);
        treeMesh.SetActive(true);
        treePlanted = true;
    }

    public void feedTree()
    {
        treeThirsty = true;
        treeHunger = 20f;
        treeHungry = false;
        hungerTimer = defaultHungerTimer;
        TerrainGenerator.instance.hungryTrees.Remove(gameObject);
        TerrainGenerator.instance.thirstyTrees.Add(gameObject);
    }

    public void waterTree()
    {
        treeThirst = 20f;
        treeThirsty = false;
        thirstTimer = defaultThirstTimer;
        TerrainGenerator.instance.thirstyTrees.Remove(gameObject);
        if (treeMesh.transform.localScale.y > 10f)
        {
            treeMaxHeight = true;
            return;
        }
        else if (treeMaxHeight != true)
        {
            treeMesh.transform.localScale = new Vector3(treeMesh.transform.localScale.x, baseHeightGrowth + (treeHunger / treeFoodCapacity), treeMesh.transform.localScale.z);
        }
    }
}
