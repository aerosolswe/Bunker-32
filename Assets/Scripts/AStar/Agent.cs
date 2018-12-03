using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

    private Pathfinding pathfinding;
    private Rigidbody2D rb;

    private List<Vector3> points = new List<Vector3>();

    private Vector3 lastTargetPos = new Vector3(1000, 1000, 1000);
    public Vector3 velocity = new Vector2();
    
    public float maxSpeed = 3.5f;
    public float mass = 20;
    public float stoppingDistance = 0.1f;
    public float slowingDistance = 1;
    public float accelerationRate = 2;
    public float decelerationRate = 2;

    private float maxForce = 100;
    private float accelerationValue = 0;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        pathfinding = Pathfinding.instance;
	}

    public void Stop() {
        points.Clear();
        rb.velocity = Vector2.zero;
    }
	
    public void MoveTo(Vector3 targetPos) {
        if(targetPos == lastTargetPos) {
            return;
        }

        lastTargetPos = targetPos;
        points.Clear();

        List<Node> pathNodes = pathfinding.FindPath(transform.position, targetPos);

        pathfinding.grid.debugPath = pathNodes;

        for (int i = 0; i < pathNodes.Count; i++) {
            points.Add(pathNodes[i].worldPosition);
        }

        points.Add(targetPos);
    }

	void Update () {
        if (points.Count == 0) return;
        
        //calculate velocities
        if (RemainingDistance < slowingDistance) {
            accelerationValue = 0;
            velocity += Arrive(points[0]) / mass;
        } else {
            velocity += Seek(points[0]) / mass;
            accelerationValue += accelerationRate * Time.deltaTime;
            accelerationValue = Mathf.Clamp01(accelerationValue);
            velocity *= accelerationValue;
        }

        velocity = Truncate(velocity, maxSpeed);
        rb.velocity = velocity;

        if(ReachedPoint(points[0])) {
            points.RemoveAt(0);

            if(points.Count == 0) {
                rb.velocity = Vector2.zero;
            }
        }

        //ApplyRotation();
        CheckIfStuck();
	}

    Vector3 oldPos = new Vector3();
    float stuckInterval = 0.5f;
    float s = 0;

    void CheckIfStuck() {
        s += Time.deltaTime;

        if(s > stuckInterval) {
            s = 0;
        } else {
            return;
        }

        if(Vector3.Distance(oldPos, transform.position) < 0.1f) {
            //Debug.Log("Stuck?");
        }

        oldPos = transform.position;
    }

    //limit the magnitude of a vector
    Vector3 Truncate(Vector3 vec, float max) {
        if (vec.magnitude > max) {
            vec.Normalize();
            vec *= max;
        }
        return vec;
    }

    Vector3 Seek(Vector3 pos) {

        Vector3 desiredVelocity = (pos - transform.position).normalized * maxSpeed;
        Vector3 steer = desiredVelocity - velocity;
        steer = Truncate(steer, maxForce);
        return steer;
    }

    //slowing at target's arrival
    Vector3 Arrive(Vector3 pos) {

        Vector3 desiredVelocity = (pos - transform.position);
        float dist = desiredVelocity.magnitude;

        if (dist > 0) {
            var reqSpeed = dist / (decelerationRate * 0.3f);
            reqSpeed = Mathf.Min(reqSpeed, maxSpeed);
            desiredVelocity *= reqSpeed / dist;
        }

        Vector3 steer = desiredVelocity - velocity;
        steer = Truncate(steer, maxForce);
        return steer;
    }

    public float RemainingDistance {
        get {
            if (points.Count == 0) return 0;

            float dist = Vector2.Distance(transform.position, points[0]);
            for (int i = 0; i < points.Count; i++) {
                dist += Vector2.Distance(points[i], points[i == points.Count - 1 ? i : i + 1]);
            }

            return dist;
        }
    }

    void ApplyRotation() {
        float rot_z = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }
    
    bool ReachedPoint(Vector3 point) {
        return ComparePositions(point, transform.position);
    }

    bool ComparePositions(Vector3 posA, Vector3 posB) {
        float dst = Vector3.Distance(posA, posB);

        return dst < stoppingDistance;
    }
}
