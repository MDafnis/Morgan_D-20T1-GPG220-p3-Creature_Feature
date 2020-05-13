using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTree : MonoBehaviour
{
    [SerializeField] private GameObject plotMesh, treeMesh, evolvedTreeMesh; // Creates the input for the meshes.
    [SerializeField] private bool treePlanted = false; // Allows us to recognise what tree is planted.
    private bool treeMaxHeight; // Allows us to recognise if the tree is at the Max Height.
    [SerializeField] private int growthCount = 0; // This will tell us how many times the tree will grow.
    public bool currentObjective = false; // If this is active the tree will not be deleted even if a tree's hunger/thirst is 0.

    private float defaultHungerTimer; // This timer will set itself to the hunger timer, doing this allows us to reference to the max timer.
    [SerializeField] private float hungerTimer = 20f; // The hunger time will tick while the scene is active, once it hits 0; tree hunger will drop by 1.
    [SerializeField] private bool treeHungry = false; // Sets the tree to be hungry or not.
    [SerializeField] private float treeHunger = 20f; // This represents the tree's hunger.
    private float treeFoodCapacity; // This is similar to being a default hunger, it will will be used to reference the max hunger capacity.

    private float defaultThirstTimer; // This timer will set itself to the thirst timer, doing this allows us to reference to the max timer.
    [SerializeField] private float thirstTimer = 20f; // The thirst time will tick while the scene is active, once it hits 0; tree thirst will drop by 1.
    [SerializeField] private bool treeThirsty = false; // Sets the tree to be thirsty or not.
    [SerializeField] private float treeThirst = 20f; // This represents the tree's thirst.
    private float treeWaterCapacity; // This is similar to being a default water, it will will be used to reference the max water capacity.

    [SerializeField] private float baseHeightGrowth = 2.5f; // How much the tree will grow by, used within the formula for growing trees.

    private void Start()
    {
        // Sets the default variables / max variables.
        treeFoodCapacity = treeHunger;
        treeWaterCapacity = treeThirst;
        defaultHungerTimer = hungerTimer;
        defaultThirstTimer = thirstTimer;
    }

    private void Update()
    {
        // Checks if the tree is planted.
        if (treePlanted)
        {
            // Makes the hungerTimer decay, if the hunger timer is below 0; the treehunger will drop by one and reset the hunger timer, checks if the tree isn't
            // hungry and treehunger is below a quarter of treeFoodCapacity, if it is; tree hungry is set to true and it adds the tree to the hungry tree list.
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

            // Makes the thirstTimer decay, if the thirst timer is below 0; the treeThirst will drop by one and reset the thirst timer, checks if the tree isn't
            // thirst and treeThirst is below a quarter of treeWaterCapacity, if it is; tree thirst is set to true and it adds the tree to the thirst tree list.
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

            // If the treeHunger is less than 0, set treeHungry to false, if the tree is in the thirsty list, remove the tree from that list, remove the tree 
            // from the hungryTrees list, add the treePlot back so a tree can be planted again, remove the tree meshes and unregister the tree as planted.
            if (treeHunger <= 0 && currentObjective != true)
            {
                treeHungry = false;
                if (TerrainGenerator.instance.thirstyTrees.Contains(gameObject))
                {
                    TerrainGenerator.instance.thirstyTrees.Remove(gameObject);
                }
                TerrainGenerator.instance.hungryTrees.Remove(gameObject);
                TerrainGenerator.instance.treePlot.Add(gameObject);
                RemoveTree();
            }

            // If the treeThirst is less than 0, set treeThirst to false, if the tree is in the hungry list, remove the tree from that list, remove the tree from 
            // the waterTrees list, add the treePlot back so a tree can be planted again, remove the tree meshes and unregister the tree as planted.
            if (treeThirst <= 0 && currentObjective != true)
            {
                treeThirsty = false;
                if (TerrainGenerator.instance.hungryTrees.Contains(gameObject))
                {
                    TerrainGenerator.instance.hungryTrees.Remove(gameObject);
                }
                TerrainGenerator.instance.thirstyTrees.Remove(gameObject);
                TerrainGenerator.instance.treePlot.Add(gameObject);
                treeMesh.SetActive(false);
                treePlanted = false;
            }
            // Clamp the hunger and thirst to the max default capacity(20).
            Mathf.Clamp(treeHunger, 0f, treeFoodCapacity);
            Mathf.Clamp(treeThirst, 0f, treeWaterCapacity);
        }
    }

    public void PlantTree()
    {
        TerrainGenerator.instance.plantedTrees.Add(gameObject);
        TerrainGenerator.instance.treePlot.Remove(gameObject);
        treeMesh.SetActive(true);
        treePlanted = true;
    }

    // FeedTree function sets the tree's thirsty bool to true, refills the plants hunger, disables the hungry bool, resets the hungerTimer, removes the tree
    // from the hungry list, adds the tree to the thirsty list.
    public void FeedTree()
    {
        treeThirsty = true;
        treeHunger = 20f;
        treeHungry = false;
        hungerTimer = defaultHungerTimer;
        TerrainGenerator.instance.thirstyTrees.Add(gameObject);
    }

    // WaterTree function refills the plants thirst, disables the thirsty bool, resets the thirstTimer, removes the tree from the thirsty list,
    // checks the tree whether or not it has met max tree height, otherwise the tree will grow in height.
    public void WaterTree()
    {
        treeThirst = 20f;
        treeThirsty = false;
        thirstTimer = defaultThirstTimer;
        if (treeMesh.transform.localScale.y > 10f)
        {
            treeMaxHeight = true;
            return;
        }
        else if (treeMaxHeight != true)
        {
            if (growthCount < 2)
            {
                treeMesh.transform.localScale = new Vector3(treeMesh.transform.localScale.x + baseHeightGrowth + (treeHunger / treeFoodCapacity),
                                                            treeMesh.transform.localScale.y + baseHeightGrowth + (treeHunger / treeFoodCapacity),
                                                            treeMesh.transform.localScale.z + baseHeightGrowth + (treeHunger / treeFoodCapacity));
            } else
            {
                treeMesh.SetActive(false);
                evolvedTreeMesh.SetActive(true);
                evolvedTreeMesh.transform.localScale = new Vector3(evolvedTreeMesh.transform.localScale.x,
                                                            evolvedTreeMesh.transform.localScale.y + baseHeightGrowth + (treeHunger / treeFoodCapacity),
                                                            evolvedTreeMesh.transform.localScale.z);
            }
            growthCount++;
        }
    }

    // This function disables the mesh of the tree no matter what type it is, and sets whether the tree is planted to false.
    public void RemoveTree()
    {
        treeMesh.SetActive(false);
        evolvedTreeMesh.SetActive(false);
        treePlanted = false;
    }

    // Returns the boolean of treePlanted.
    public bool GetTreePlanted()
    {
        return treePlanted;
    }

    // Sets the boolean of treePlanted.
    public void SetTreePlanted(bool result)
    {
        treePlanted = result;
    }

    // Returns the value of of treeHunger.
    public float GetTreeHunger()
    {
        return treeHunger;
    }

    // Sets the float of the treeHunger variable.
    public void SetTreeHunger(float result)
    {
        treeHunger = result;
    }

    // Returns the value of of treeThirst.
    public float GetTreeThirst()
    {
        return treeThirst;
    }

    // Sets the float of the treeHunger variable.
    public void SetTreeThirst(float result)
    {
        treeThirst = result;
    }
}
