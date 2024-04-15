using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    private int[] map = new int[]{0,0,1,0,0};

    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < map.Length; ++i) {
            Debug.Log(map[i] + ",");
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
