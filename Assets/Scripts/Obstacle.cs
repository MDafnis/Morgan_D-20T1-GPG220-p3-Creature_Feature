using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float FieldStrength = 10f;
    public float FieldRange = 10f;

    // Start is called before the first frame update
    void Start()
    {
        ObstacleManager.RegisterObstacle(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        ObstacleManager.DeregisterObstacle(this);
    }

    public Vector3 GetFieldTo(Vector3 otherObject)
    {
        // calculate the vector to the other object and convert to 2D
        Vector3 vectorToOther = otherObject - transform.position;
        vectorToOther.y = 0;

        // get the distance to the other
        float distanceToOtherSq = vectorToOther.sqrMagnitude;

        // too far to have impact?
        if (distanceToOtherSq >= (FieldRange * FieldRange))
            return Vector3.zero;

        // determine proportion of the maximum range the other object is away
        float distanceToOther = Mathf.Sqrt(distanceToOtherSq);
        float distanceProportion = distanceToOther / FieldRange;

        // return the field vector
        return Vector3.Lerp(FieldStrength * (vectorToOther / distanceToOther), Vector3.zero, distanceProportion);
    }
}
