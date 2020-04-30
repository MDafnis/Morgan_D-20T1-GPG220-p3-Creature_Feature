using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static List<Obstacle> Obstacles = new List<Obstacle>();

    public static void RegisterObstacle(Obstacle obstacle)
    {
        Obstacles.Add(obstacle);
    }

    public static void DeregisterObstacle(Obstacle obstacle)
    {
        Obstacles.Remove(obstacle);
    }
}
