using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [Header("Config")]
    public LayerMask pathBlockMask;
    public float aStarScanDist = 1;
    //public bool spawnMarkers = true;

    [Header("Trackers")]
    public bool findingPath = false;
    public bool reachedDestination = false;

    [Header("Prefabs")]
    //public GameObject waypointMarker;

    [Header ("Objects")]
    public Transform goal;
    Character myChar;
    
    // Start is called before the first frame update
    void Start()
    {
        myChar = GetComponent<Character>();
        goal = DungeonManager.singleton.GetPC().transform;
        //Pathfind();
    }

    public void Pathfind() {
        StartCoroutine(PathfindRoutine());
    }

    IEnumerator PathfindRoutine() {
        bool needPath = true;
        while(!reachedDestination) {

            //Generate the path
            GetAStarToTarget(goal.transform.position);
            yield return new WaitUntil(()=>findingPath == false);
            needPath = false;

            //Move creature along path
            foreach(Vector2 v in aStarPath) {
                while(Vector3.Distance(transform.position, v) > myChar.GetSpeed() * Time.fixedDeltaTime) {

                    //See if we can reach the next node in the sequence
                    if(CanReachPos(transform.position, v)) {
                        myChar.MoveToward(v);
                    } else {
                        needPath = true;
                        break;
                    }
                    yield return new WaitForFixedUpdate();
                }
                if(needPath) {
                    break;
                }
            }

            if(!needPath) {
                reachedDestination = true;
            }

            yield return null;
        }
    }

    void GetAStarToTarget(Vector3 targetPos) {
        findingPath = true;
        aStarPath = new List<Vector2>();
        StartCoroutine(AStarToTargetRoutine(targetPos));
    }

    IEnumerator AStarToTargetRoutine(Vector3 targetPos) {
        
        Vector3 myPos = transform.position;

        HashSet<Vector2> closedSet = new HashSet<Vector2>(); //Blocks already evaluated

        HashSet<Vector2> openSet = new HashSet<Vector2>(); //Currently discovered nodes that are not evaluated yet
        openSet.Add(myPos);

        cameFrom = new Dictionary<Vector2, Vector2>();

        //gScore is the cost of getting to a position
        Dictionary <Vector2, float> gScore = new Dictionary<Vector2, float>();
        gScore.Add(myPos, 0);

        //fScore is the gScore, plus the heuristic
        Dictionary <Vector2, float> fScore = new Dictionary<Vector2, float>();
        fScore.Add(myPos, HeuristicCostEstimate(myPos, targetPos));

        int maxAttempts = 40; //Prevent us from looking forever if path gets too large

        while(openSet.Count > 0 && maxAttempts > 0) {
            maxAttempts -= 1;

            //Find the vector in open set with smallest f, set it as our current
            float minF = int.MaxValue;
            Vector2 minFVector = new Vector2(-1,-1);

            //This is lazy, priority queue is better
            foreach(Vector2 v in openSet) {
                if(fScore[v] < minF) {
                    minF = fScore[v];
                    minFVector = v;
                }
            }

            Vector2 current = minFVector;

            yield return null; //Time slice

            if(Vector2.Distance(current, targetPos) <= aStarScanDist && CanReachPos(current,targetPos)) { //We've made it to target
                ReconstructPath(current);
            } else {
                openSet.Remove(current); //Remove our current space from the evaluation pool
                closedSet.Add(current); //Add to the closed pool

                //Neighbors are the spaces in our imaginary grid to the north, south, east, and west
                List<Vector2> neighbors = new List<Vector2>();
                for(float x = -1; x < 2; x++) {
                    for(float y = -1; y < 2; y++) {
                        if(x == 0 || y == 0) {
                            if(CanReachPos(current, current + new Vector2(x*aStarScanDist, y*aStarScanDist))) {
                                neighbors.Add(current + new Vector2(x,y)*aStarScanDist);
                            }
                        }

                    }
                }

                foreach(Vector2 neighbor in neighbors) {
                    if(!closedSet.Contains(neighbor)) {
                        float new_gScore = gScore[current] + aStarScanDist;

                        bool worsePath = false;
                        if(!openSet.Contains(neighbor)) {
                            openSet.Add(neighbor);
                        } else if(new_gScore >= gScore[neighbor]) {
                            worsePath = true;
                        }

                        if(!worsePath) {
                            cameFrom[neighbor] = current;
                            gScore[neighbor] = new_gScore;
                            fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, targetPos);
                        }
                    }
                }
            }

        }

        findingPath = false;
    }

    public bool CanReachPos(Vector3 startPos, Vector3 endPos) {
        return !Physics2D.Linecast(startPos, endPos, pathBlockMask);
    }

    float HeuristicCostEstimate(Vector2 start, Vector2 goal) {
        return Mathf.Abs(start.x - goal.x) + Mathf.Abs(start.y - goal.y);
    }
    public List<Vector2> aStarPath;
    Dictionary<Vector2, Vector2> cameFrom;
    void ReconstructPath(Vector2 current) {
        
        aStarPath = new List<Vector2>();

        aStarPath.Add(current);
        while(cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            aStarPath.Add(current);
        }

        aStarPath.Reverse();

        aStarPath.Add(goal.transform.position);
    }
}