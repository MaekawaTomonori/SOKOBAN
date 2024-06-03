using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    private const float timeTaken = 0.2f;

    private float timeErapsed = 0;

    private Stack<Vector3> prePos = new Stack<Vector3>();
    private Stack<Vector2Int> preMPos = new Stack<Vector2Int>();

    private Vector3 destination;

    private Vector3 origin;

    public void Start() {
        destination = transform.position;
        origin = destination;
    }

    public void Init(Vector3 origin, Vector2Int mOrigin) {
        this.origin = origin;

        prePos.Push(origin);
        preMPos.Push(mOrigin);
    }

    public void Update() {
        if(origin == destination) return;

        timeErapsed += Time.deltaTime;

        float timeRate = timeErapsed / timeTaken;

        if(timeRate > 1) {timeRate=1;}

        float easing = timeRate;
        Vector3 currentPosition = Vector3.Lerp(origin, destination, easing);
        transform.position = currentPosition; 
    }

    public void Moveto(Vector3 destination, Vector2Int pre, Vector2Int mapCoordinate) {
        timeErapsed = 0;

        if (preMPos.Count == 0) {
            preMPos.Push(pre);
        }

        origin = transform.position;
        prePos.Push(origin);
        preMPos.Push(mapCoordinate);

        this.destination = destination;
    }

    public Vector2Int Undo() {
        if (prePos.Count <= 1) return preMPos.Peek();

        origin = destination;
        destination = prePos.Pop();

        return preMPos.Pop();
    }
}
