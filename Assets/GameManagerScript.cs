using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    //private int[] map = new int[] { 0, 2, 1, 2, 0 };
    public GameObject playerPrefab;
    public GameObject blockPrefab;
    private int[,] map = {
        { 0, 0, 1, 0, 0 },
        { 0, 0, 2, 0, 0 },
        { 0, 0, 0, 2, 0 }
    };

    private GameObject[,] field;

    // Start is called before the first frame update
    void Start() {
        //init
        field = new GameObject[map.GetLength(0), map.GetLength(1)];

        Debug.Log("Field Size x : " + field.GetLength(1) + ", y : " + field.GetLength(0));

        for (int row = 0; row < map.GetLength(0); ++row) {
            for (int column = 0; column < map.GetLength(1); ++column) {
                if (map[row, column] == 0) continue;

                if (map[row, column] == 1) {
                    field[row, column] = Instantiate(
                        playerPrefab,
                        new Vector3(column - 2, map.GetLength(0) - row, 0),
                        Quaternion.identity
                    );
                    continue;
                }

                if (map[row, column] == 2) {
                    field[row, column] = Instantiate(
                        blockPrefab,
                        new Vector3(column - 2, map.GetLength(0) - row, 0),
                        Quaternion.identity
                    );
                }
            }
        }
        
        //string txt = "";
        //for (int row = 0; row < map.GetLength(0); ++row) {
        //    for (int column = 0; column < map.GetLength(1); ++column) {
        //        txt += map[row, column] + ", ";
        //    }
        //    txt += "\n";
        //}
        //Debug.Log(txt);
        //PrintArray();


    }

    // Update is called once per frame
    void Update() {
        Vector2Int playerIndex = getPlayerIndex();
        Vector2Int x = new Vector2Int(1,0);
        Vector2Int y = new Vector2Int(0,1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            MoveNumber(playerPrefab.tag, playerIndex, playerIndex + x);
            //    MoveNumber((int)BlockType.PLAYER, playerIndex, playerIndex + 1);
            //    PrintArray();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            MoveNumber(playerPrefab.tag, playerIndex, playerIndex - x);
            //    MoveNumber((int)BlockType.PLAYER, playerIndex, playerIndex - 1);
            //    PrintArray();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            MoveNumber(playerPrefab.tag, playerIndex, playerIndex - y);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            MoveNumber(playerPrefab.tag,playerIndex, playerIndex + y);
        }
    }
    //void PrintArray() {
    //    string output = "";
    //    for (int i = 0; i < map.Length; ++i)
    //    {
    //        output += map[i].ToString() + ", ";
    //    }

    //    Debug.Log(output);
    //}

    Vector2Int getPlayerIndex() {
        for (int row = 0; row < map.GetLength(0); ++row) {
            for (int column = 0; column < map.GetLength(1); ++column) {
                if (field[row, column] == null) continue;

                if (field[row, column].tag == "Player") {
                    return new Vector2Int(column, row);
                }
            }
        }
        return new Vector2Int(-1,-1);
    }

    bool MoveNumber(string objectiveTag, Vector2Int moveFrom, Vector2Int moveTo) {
        Debug.Log("x : " + moveTo.x + ", y : " + moveTo.y);
        //MapRange detect
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1) || moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }

        //moveTo nullcheck
        if (field[moveTo.y, moveTo.x] != null) {
            Debug.Log("TAG : @" + field[moveTo.y, moveTo.x].tag);

            //distination is a block
            if (field[moveTo.y, moveTo.x].tag == "Block") {
                if (!MoveNumber("Block", moveTo, moveTo + (moveTo - moveFrom))) {
                    return false;
                }
            }
        }

        field[moveTo.y,moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveTo.y,moveTo.x].transform.position = new Vector3(moveTo.x -2, field.GetLength(0)- moveTo.y,0);
        field[moveFrom.y, moveFrom.x] = null;
        
        Debug.Log("Success");
        return true;
    }
}
