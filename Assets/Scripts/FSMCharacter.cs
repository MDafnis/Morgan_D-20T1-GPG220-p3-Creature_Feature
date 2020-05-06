using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMCharacter : MonoBehaviour
{
    // Not ideal - Recommend using a blackboard system instead.
    public Vector2Int LocationToRequest;
    protected bool HasDestination = false;
    protected Vector3 Destination = Vector3.zero;
    protected Vector3 refDestination = Vector3.zero;
    protected List<PathdataNode> Path = null;
    protected int CurrentPoint = -1;
    protected Rigidbody CharacterRB;
    public int curCharge = 100;
    public bool stateActive = false;

    [Header("Reached Destination")]
    public float DestinationThreshold = 0.1f;

    [Header("Advancing Point")]
    public float PointReachedThreshold = 1f;

    [Header("Steering Controls")]
    public float MaximumSpeed = 1f;
    [Range(0f, 1f)] public float Aggressiveness = 0.5f;

    [Header("Avoidance Controls")]
    public bool EnableAvoidance = true;
    [Range(0f, 1f)] public float AvoidancePriority = 0.5f;

    [Header("Debug Controls")]
    public bool DEBUG_DrawPath = true;

    void Start()
    {
        CharacterRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // do we have no destination?
        if (!HasDestination && TerrainGenerator.instance.sphereList.Count > 0 && !stateActive)
        {
            //SetDestination(new Vector3(TerrainGenerator.instance.waterShelter.transform.position.x, transform.position.y, TerrainGenerator.instance.waterShelter.transform.position.z)); // TerrainGenerator.instance.WaterShelter.transform.position.x, transform.position.y, TerrainGenerator.instance.WaterShelter.transform.position.z
        }

        // can and should draw path?
        if (DEBUG_DrawPath && HasDestination)
        {
            // draw the path
            Debug.DrawLine(transform.position, Path[CurrentPoint].WorldLocation, Color.green);
            for (int index = CurrentPoint; index < Path.Count - 1; ++index)
            {
                Debug.DrawLine(Path[index].WorldLocation, Path[index + 1].WorldLocation, Color.blue);
            }
        }
    }

    void FixedUpdate()
    {
        // What we have
        //  - Path (list of points)
        //  - Which point we're heading to (CurrentPoint)
        //  - List of obstacles
        //  - Movement stats (speed etc)

        // no destination?
        if (!HasDestination)
        {
            CharacterRB.velocity = Vector3.zero;
            return;
        }

        // are we on the final point?
        if (CurrentPoint == (Path.Count - 1))
        {
            // have we reached the destination?
            if (ReachedDestination)
            {
                HasDestination = false;
                CharacterRB.velocity = Vector3.zero;

                return;
            }
        }
        else
        {
            // reached the next point?
            if (CanAdvanceToNextPoint)
            {
                ++CurrentPoint;
            }
        }

        // TODO - detect if we're stuck
        //      - Has it been trying to move but not moved much for a set time?

        // Get our desired movement vector
        Vector3 desiredVector = Path[CurrentPoint].WorldLocation - transform.position;
        float desiredSpeed = MaximumSpeed;
        desiredVector.y = 0;
        desiredVector.Normalize();

        // Obstacle avoidance
        Vector3 avoidanceVector = Vector3.zero;
        if (EnableAvoidance)
        {
            // work out the total avoidance vector
            foreach (Obstacle obstacle in ObstacleManager.Obstacles)
            {
                avoidanceVector += obstacle.GetFieldTo(transform.position);
            }

            // clamp the magnitude of the avoidance vector
            float avoidanceMag = avoidanceVector.magnitude;
            avoidanceVector = avoidanceVector.normalized * Mathf.Min(avoidanceMag, MaximumSpeed);
        }

        Vector3 steeringVector = Vector3.Lerp(desiredVector * desiredSpeed, avoidanceVector,
                                              EnableAvoidance ? AvoidancePriority : 0f);

        // Update the character's velocity
        CharacterRB.velocity = Vector3.Lerp(CharacterRB.velocity, steeringVector.normalized * desiredSpeed, Aggressiveness);
    }

    public bool ReachedDestination
    {
        get
        {
            // Typically for this we do a 2D check
            float distance2DSquared = Mathf.Pow(Destination.x - transform.position.x, 2) +
                                      Mathf.Pow(Destination.z - transform.position.z, 2);

            return distance2DSquared < (DestinationThreshold * DestinationThreshold);
        }
    }

    public bool CanAdvanceToNextPoint
    {
        get
        {
            // Typically for this we do a 2D check
            float distance2DSquared = Mathf.Pow(Path[CurrentPoint].WorldLocation.x - transform.position.x, 2) +
                                      Mathf.Pow(Path[CurrentPoint].WorldLocation.z - transform.position.z, 2);

            return distance2DSquared < (PointReachedThreshold * PointReachedThreshold);
        }
    }

    public void SetDestination(Vector3 newDestination)
    {
        // is this the same as our current one?
        if (HasDestination && ((newDestination - Destination).sqrMagnitude < float.Epsilon))
        {
            // nothing to do as we already have this as a destination
            return;
        }

        // set the new destination
        Destination = newDestination;
        refDestination = newDestination;
        HasDestination = true;
        CurrentPoint = 0;

        // TODO - Call to pathfinding would go here. 
        var myPos = new Vector2(transform.position.x, transform.position.z);

        //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //sphere.GetComponent<SphereCollider>().enabled = false;
        //sphere.transform.localScale = new Vector3(5, 5, 5);
        //sphere.transform.position = new Vector3(myPos.x, transform.position.y, myPos.y);

        var destPos = new Vector2(newDestination.x, newDestination.z);
        Path = PathFinding.instance.FindPath(Pathdata.instance.FindNode(myPos), Pathdata.instance.FindNode(destPos));

        if (Path == null || Path.Count == 0)
        {
             HasDestination = false;
             return;
        }
    }
}
