using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{

    public Transform target;
    [SerializeField] float speed = 5;
    Vector2[] path;
    int targetIndex;

    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector2[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Debug.Log("Following path...");
        Vector2 currentWaypoint = path[0];
        while (true)
        {
            if ((Vector2)transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    Debug.Log("Reached end of path!");
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            Vector2 unitPos = transform.position;

            for (int i = targetIndex; i < path.Length; i++)
            {
                Vector2 waypoint = path[i];
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(new Vector3(waypoint.x, waypoint.y, 0), 0.1f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(new Vector3(unitPos.x, unitPos.y, 0), new Vector3(waypoint.x, waypoint.y, 0));
                }
                else
                {
                    Vector2 prevWaypoint = path[i - 1];
                    Gizmos.DrawLine(new Vector3(prevWaypoint.x, prevWaypoint.y, 0), new Vector3(waypoint.x, waypoint.y, 0));
                }
            }
        }
    }


}