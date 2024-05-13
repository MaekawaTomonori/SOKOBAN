using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    private const float timeTaken = 0.2f;

    private float timeErapsed = 0;

    private Vector3 destination;

    private Vector3 origin;

    public void Start() {
        destination = transform.position;
        origin = destination;
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

    public void Moveto(Vector3 destination) {
        timeErapsed = 0;

        origin = transform.position;

        this.destination = destination;
    }
}
