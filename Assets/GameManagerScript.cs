using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    private int[] map = new int[] { 0, 0, 1, 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        PrintArray();
    }

    // Update is called once per frame
    void Update() {
        int playerIndex = getPlayerIndex();
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            MoveNumber(1, playerIndex, playerIndex + 1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            MoveNumber(1, playerIndex, playerIndex - 1);
        }
    }

    void PrintArray() {
        string output = "";
        for (int i = 0; i < map.Length; ++i)
        {
            output += map[i].ToString() + ", ";
        }

        Debug.Log(output);

    }

    int getPlayerIndex()
    {
        int playerIndex = -1;
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == 1)
            {
                playerIndex = i;
                break;
            }
        }
        return playerIndex;
    }

    bool MoveNumber(int objectiveNum, int moveFrom, int moveTo)
    {
        if (moveTo < 0 || moveTo > map.Length) { return false; }
        map[moveTo] = objectiveNum;
        map[moveFrom] = map[moveTo];
        return true;
    }
}
