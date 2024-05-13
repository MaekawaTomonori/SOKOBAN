using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
    private const float lifeTime = 0.6f;
    private float leftLifeTime;
    private Vector3 velocity;
    private Vector3 defaultScale;

    void Start() {
        leftLifeTime = lifeTime;
        defaultScale = transform.localScale;
        float maxVelocity = 5;
        velocity = new Vector3(Random.Range(-maxVelocity, maxVelocity), Random.Range(-maxVelocity, maxVelocity), 0);
    }

    void Update() {
        leftLifeTime -= Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), defaultScale, leftLifeTime / lifeTime);

        if (leftLifeTime <= 0) {
            Destroy(gameObject);
        }
    }
}
