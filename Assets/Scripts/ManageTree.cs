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

            // If the treeHunger is less than 0 and currentObjective is false, set treeHungry to false, run RemoveTree Function.
            if (treeHunger <= 0 && currentObjective != true)
            {
                treeHungry = false;
                RemoveTree();
            }

            // If the treeThirst is less than 0 and currentObjective is false, set treeThirsty to false, run RemoveTree Function.
            if (treeThirst <= 0 && currentObjective != true)
            {
                treeThirsty = false;
                RemoveTree();
            }
            // Clamp the hunger and thirst to the max default capacity(20).
            Mathf.Clamp(treeHunger, 0f, treeFoodCapacity);
            Mathf.Clamp(treeThirst, 0f, treeWaterCapacity);
        }
    }

    // PlantTree adds the tree to the planted tree list, removes the tree from the treePlot list(pots that don't contain trees), enable the pot mesh, enable
    // sappling mesh, set treePlanted bool to true.
    public void PlantTree()
    {
        TerrainGenerator.instance.plantedTrees.Add(gameObject);
        TerrainGenerator.instance.treePlot.Remove(gameObject);
        plotMesh.SetActive(true);
        treeMesh.SetActive(true);
        treePlanted = true;
    }

    // FeedTree function sets the tree's thirsty bool to true, refills the plants hunger, disables the hungry bool, resets the hungerTimer, removes the tree
    // from the hungry list.
    public void FeedTree()
    {
        treeThirsty = true;
        treeHunger = treeFoodCapacity;
        treeHungry = false;
        hungerTimer = defaultHungerTimer;
        TerrainGenerator.instance.hungryTrees.Remove(gameObject);
    }

    // WaterTree function refills the plants thirst, disables the thirsty bool, resets the thirstTimer, removes the tree from the thirsty list,
    // checks the tree if it has met max height and if it has grown more than twice, otherwise the sappling will add to it's growCount, and will grow in height
    // until the sappling has grown twice, once it grows more than twice; the sappling mesh will swap out with the evolvedTreeMesh, disable the plot mesh, and
    // scale itself each time the tree is watered.
    public void WaterTree()
    {
        treeThirst = treeWaterCapacity;
        treeThirsty = false;
        thirstTimer = defaultThirstTimer;
        TerrainGenerator.instance.thirstyTrees.Remove(gameObject);
        if (treeMesh.transform.localScale.y > 3f && growthCount > 2)
        {
            treeMaxHeight = true;
            return;
        }
        else if (treeMaxHeight != true)
        {
            if (growthCount <= 2)
            {
                treeMesh.transform.localScale = new Vector3((treeMesh.transform.localScale.x + baseHeightGrowth + treeHunger) / treeFoodCapacity,
                                                            (treeMesh.transform.localScale.y + baseHeightGrowth + treeHunger) / treeFoodCapacity,
                                                            (treeMesh.transform.localScale.z + baseHeightGrowth + treeHunger) / treeFoodCapacity);
            } else
            {
                plotMesh.SetActive(false);
                treeMesh.SetActive(false);
                evolvedTreeMesh.SetActive(true);
                evolvedTreeMesh.transform.localScale = new Vector3(evolvedTreeMesh.transform.localScale.x + (baseHeightGrowth / treeFoodCapacity),
                                                            evolvedTreeMesh.transform.localScale.y + (baseHeightGrowth / treeFoodCapacity),
                                                            evolvedTreeMesh.transform.localScale.z + (baseHeightGrowth / treeFoodCapacity));
            }
            growthCount++;
        }
    }

    // This function checks the tree if it's within the list of hungry/thirsty, if so it will remove itself from the list, disables the mesh of the plot and tree
    // meshes no matter what type it is, and sets whether the tree is planted to false, reset the treeThirst; treeHunger values, reset the treeThirstTimer and
    // treeHungerTimer, remove the tree from the planted list, lastly checks if the tree is within the treePlot list, if not the function will add the tree back
    // to the treePlot list ready to be planted again.
    public void RemoveTree()
    {
        if (TerrainGenerator.instance.hungryTrees.Contains(gameObject))
        {
            TerrainGenerator.instance.hungryTrees.Remove(gameObject);
        }
        if (TerrainGenerator.instance.thirstyTrees.Contains(gameObject))
        {
            TerrainGenerator.instance.thirstyTrees.Remove(gameObject);
        }
        plotMesh.SetActive(false);
        treeMesh.SetActive(false);
        evolvedTreeMesh.SetActive(false);
        treePlanted = false;
        treeThirst = treeWaterCapacity;
        treeHunger = treeFoodCapacity;
        thirstTimer = defaultThirstTimer;
        hungerTimer = defaultHungerTimer;
        TerrainGenerator.instance.plantedTrees.Remove(gameObject);
        if (TerrainGenerator.instance.treePlot.Contains(gameObject) == false)
        {
            TerrainGenerator.instance.treePlot.Add(gameObject);
        }
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
