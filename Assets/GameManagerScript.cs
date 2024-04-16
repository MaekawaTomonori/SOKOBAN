using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    private int[] map = new int[] { 0, 2, 1, 2, 0 };

    private enum BlockType {
        AIR,
        PLAYER,
        BLOCK
    }

    // Start is called before the first frame update
    void Start()
    {
        PrintArray();
    }

    // Update is called once per frame
    void Update() {
        int playerIndex = getPlayerIndex();
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            MoveNumber((int)BlockType.PLAYER, playerIndex, playerIndex + 1);
            PrintArray();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            MoveNumber((int)BlockType.PLAYER, playerIndex, playerIndex - 1);
            PrintArray();
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
        if (moveTo < 0 || moveTo > map.Length -1) { return false; }

        //distination is a block
        if (map[moveTo] == (int)BlockType.BLOCK) {
            if (!MoveNumber((int)BlockType.BLOCK, moveTo, moveTo + (moveTo - moveFrom))) {
                return false;
            }
        }
        map[moveFrom] = (int)BlockType.AIR;
        map[moveTo] = objectiveNum;
        return true;
    }
}
