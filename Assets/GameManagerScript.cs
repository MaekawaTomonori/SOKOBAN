using System.Collections.Generic;
using UnityEngine;

struct MapData {
    private int[,] data_;

    public int[,] getData() {
        return data_;
    }

    public void setData(int[,] d) {
        data_ = d;
    }
}

public class GameManagerScript : MonoBehaviour {

    //private int[] map = new int[] { 0, 2, 1, 2, 0 };
    //public enum MapBlockType {
    //    AIR,    0
    //    PLAYER, 1
    //    BLOCK,  2
    //    GOAL,   3
    //}
    public GameObject playerPrefab;
    public GameObject blockPrefab;
    public GameObject goalPrefab;
    public GameObject particlePrefab;
    public GameObject wallPrefab;
    public GameObject clearText;
    public GameObject undoText;
    private GameObject[,] field;

    private MapData[] map;
    private int curStage = 0;


    private List<Vector2Int> goals = new List<Vector2Int>();

    // Start is called before the first frame update
    void Start() {
        //init
        Screen.SetResolution(1920, 1080, true);
        Initialize();
        
        //string txt = "";
        //for (int row = 0; row < map[curStage].getData().GetLength(0); ++row) {
        //    for (int column = 0; column < map[curStage].getData().GetLength(1); ++column) {
        //        txt += map[row, column] + ", ";
        //    }
        //    txt += "\n";
        //}
        //Debug.Log(txt);
        //PrintArray();


    }

    // Update is called once per frame
    void Update() {
        //undoText.SetActive(true);
        //if (Input.GetKeyDown(KeyCode.E)) {
        //    for (int row = 0; row < map[curStage].getData().GetLength(0); ++row) {
        //        for (int col = 0; col < map[curStage].getData().GetLength(1); ++col) {
        //            if (!field[row, col]) continue;
        //            if (field[row, col].tag != "Player") continue;
        //            Undo(new Vector2Int(col, row), field[row, col]);
        //        }
        //    }
        //}

        BlockColor();

        if (Input.GetKeyDown(KeyCode.S)) {
            curStage = 1;
            ClearField();
            Initialize();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            ClearField();
            Initialize();
            return;
        }

        if (isCleard()) {
            clearText.SetActive(true);
            //clearText.transform.position = Camera.main.transform.position;
            return;
        }


        Vector2Int playerIndex = getPlayerIndex();
        Vector2Int x = new Vector2Int(1,0);
        Vector2Int y = new Vector2Int(0,1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            MoveNumber( playerIndex, playerIndex + x);
            //    MoveNumber((int)BlockType.PLAYER, playerIndex, playerIndex + 1);
            //    PrintArray();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            MoveNumber(playerIndex, playerIndex - x);
            //    MoveNumber((int)BlockType.PLAYER, playerIndex, playerIndex - 1);
            //    PrintArray();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            MoveNumber(playerIndex, playerIndex - y);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            MoveNumber(playerIndex, playerIndex + y);
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
        for (int row = 0; row < map[curStage].getData().GetLength(0); ++row) {
            for (int column = 0; column < map[curStage].getData().GetLength(1); ++column) {
                if (field[row, column] == null) continue;

                if (field[row, column].tag == "Player") {
                    return new Vector2Int(column, row);
                }
            }
        }
        return new Vector2Int(-1,-1);
    }

    bool MoveNumber( Vector2Int moveFrom, Vector2Int moveTo) {
        Debug.Log("x : " + moveTo.x + ", y : " + moveTo.y);
        //MapRange detect
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1) || moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }

        //moveTo nullcheck
        if (field[moveTo.y, moveTo.x] != null) {
            Debug.Log("TAG : @" + field[moveTo.y, moveTo.x].tag);


            //destination is a block
            if (field[moveTo.y, moveTo.x].tag == "Block") {
                field[moveTo.y, moveTo.x].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                if (!MoveNumber(moveTo, moveTo + (moveTo - moveFrom))) {
                    return false;
                }
            }

            
        }
        if (field[moveFrom.y, moveFrom.x].tag == "Player") {
            for (int i = 0; i < 5; ++i) {
                //particles
                Instantiate(particlePrefab, field[moveFrom.y, moveFrom.x].transform.position, Quaternion.identity);
            }
        }
        field[moveTo.y,moveTo.x] = field[moveFrom.y, moveFrom.x];
        //field[moveTo.y, moveTo.x].transform.position = new Vector3(moveTo.x - 2, field.GetLength(0) - moveTo.y, 0);
        //field[moveFrom.y, moveFrom.x] = null;
        Vector3 moveToPosition = new Vector3(moveTo.x, map[curStage].getData().GetLength(0) - moveTo.y, 0);
        field[moveFrom.y, moveFrom.x].GetComponent<Move>().Moveto(moveToPosition, moveFrom, moveTo);
        field[moveFrom.y, moveFrom.x] = null;

        Debug.Log("Success");
        return true;
    }

    private bool isCleard() {
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int row = 0; row < map[curStage].getData().GetLength(0); ++row) {
            for (int col = 0; col < map[curStage].getData().GetLength(1); ++col) {
                if (map[curStage].getData()[row, col] == 3) {
                    goals.Add(new Vector2Int(col, row));
                }
            }
        }

        for (int i = 0; i < goals.Count; ++i) {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Block") {
                return false;
            }
        }


        return true;
    }

    void Initialize() {
        GenerateMap();
        field = new GameObject[map[curStage].getData().GetLength(0), map[curStage].getData().GetLength(1)];

        Debug.Log("Field Size x : " + field.GetLength(1) + ", y : " + field.GetLength(0));

        for (int row = 0; row < map[curStage].getData().GetLength(0); ++row)  {
            for (int column = 0; column < map[curStage].getData().GetLength(1); ++column) {
                if (map[curStage].getData()[row, column] == 0) continue;

                switch (map[curStage].getData()[row, column]) {
                    case 1:
                        field[row, column] = Instantiate(
                            playerPrefab,
                            new Vector3(column, map[curStage].getData().GetLength(0) - row, 0),
                            Quaternion.identity
                        );
                        break;
                    case 2:
                        field[row, column] = Instantiate(
                            blockPrefab,
                            new Vector3(column, map[curStage].getData().GetLength(0) - row, 0),
                            Quaternion.identity
                        );
                        break;
                    case 3:
                        Instantiate(
                            goalPrefab,
                            new Vector3(column, map[curStage].getData().GetLength(0) - row, 0.01f),
                            Quaternion.identity
                        );
                        break;
                    case 4:
                        field[row, column] = Instantiate(
                            wallPrefab,
                            new Vector3(column, map[curStage].getData().GetLength(0) - row, 0),
                            Quaternion.identity
                        );
                        break;
                }
            }
        }
        // 
        // x (0 ~) : (0 ~ map size)
        // y (map size ~ ) : (0 ~ map size)  size = 10 row = 0
        //
        Vector3 cameraPos = new Vector3(map[curStage].getData().GetLength(1) / 2, map[curStage].getData().GetLength(0) / 2 + 1, -10);
        Camera.main.transform.position = cameraPos;

        clearText.SetActive(false);
        undoText.SetActive(false);

        for (int row = 0; row < map[curStage].getData().GetLength(0); ++row)
        {
            for (int col = 0; col < map[curStage].getData().GetLength(1); ++col)
            {
                if (map[curStage].getData()[row, col] == 3)
                {
                    goals.Add(new Vector2Int(col, row));
                }
            }
        }
    }

    void ClearField() {
        for (int r = 0; r < field.GetLength(0); ++r) {
            for (int c = 0; c < field.GetLength(1); ++c) {
                Destroy(field[r, c]);
            }
        }
    }

    void GenerateMap() {
        map = new MapData[2];
        map[0]
            .setData(
                new int[,] {
                    { 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                    { 4, 0, 0, 0, 2, 3, 0, 0, 4 },
                    { 4, 0, 0, 3, 2, 0, 0, 0, 4 },
                    { 4, 0, 0, 0, 2, 3, 0, 0, 4 },
                    { 4, 0, 0, 0, 1, 0, 0, 0, 4 },
                    { 4, 0, 0, 0, 2, 3, 0, 0, 4 },
                    { 4, 0, 0, 3, 2, 0, 0, 0, 4 },
                    { 4, 0, 0, 0, 2, 3, 0, 0, 4 },
                    { 4, 4, 4, 4, 4, 4, 4, 4, 4 }
                }
            );
        map[1]
            .setData(
                new int[,] {
                    { 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                    { 4, 0, 0, 0, 0, 0, 0, 0, 4 },
                    { 4, 0, 1, 0, 2, 3, 0, 0, 4 },
                    { 4, 3, 0, 2, 2, 0, 0, 0, 4 },
                    { 4, 0, 2, 0, 3, 0, 0, 0, 4 },
                    { 4, 0, 0, 0, 0, 0, 0, 0, 4 },
                    { 4, 0, 3, 0, 0, 0, 0, 0, 4 },
                    { 4, 0, 0, 0, 0, 0, 0, 0, 4 },
                    { 4, 4, 4, 4, 4, 4, 4, 4, 4 }
                }
            );
    }

    void Undo(Vector2Int mPos,GameObject obj) {
        Vector2Int mapPos = obj.GetComponent<Move>().Undo();
        field[mapPos.y, mapPos.x] = field[mPos.y, mapPos.x];
        field[mPos.y, mPos.x] = null;
    }

    private void BlockColor() {
        //block color
        foreach (Vector2Int goal in goals) {
            GameObject obj = field[goal.y, goal.x];
            if (obj == null) { continue; }

            if (obj.tag == "Block") {
                obj.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
            }
        }
    }
}
