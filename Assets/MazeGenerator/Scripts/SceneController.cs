using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneController : MonoBehaviour {

    private Dictionary<object, object> memory;
    private List<Vector3> savedMaze;
    private float lastSection = 0;
    private float time = 0;
    [SerializeField] private Rigidbody ball = null;
    
    private int finishX, finishY;

    private void Start() {
        var args = File.ReadAllText("input.txt").Split(' ');
        finishY = int.Parse(args[3]) * 4;
        finishX = int.Parse(args[4]) * 4;
        //File.WriteAllLines("input.txt", new List<string>{ "" });
        savedMaze = new List<Vector3>();
        memory = new Dictionary<object, object>();
        for (int i = 0; i < transform.childCount; i++) {
            savedMaze.Add(Vector3.zero);
        }
    }

    private void FixedUpdate() {
        if (finishX <= ball.transform.position.x && ball.transform.position.x + 2 <= finishX + 4 && 
            finishY <= ball.transform.position.z && ball.transform.position.z + 2 <= finishY + 4) {
            Debug.Log("Win!");
            File.WriteAllLines("output.txt", new List<string>{ "WIN!" });
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                     Application.Quit();
            #endif
        }
        time += Time.fixedDeltaTime;
        if (time >= lastSection) {
            makeTurn();
            time = 0;
        }
    }

    private void makeTurn() {
        KillMaze();
        Solution sol = new Solution(this);
        Vector3 ballPos = ball.transform.position;
        Vector4 data = sol.getMove();
        Vector3 turn = new Vector3(data.x, 0, data.z);
        lastSection = data.w;
        CreateMaze();
        sol.controller = null;
        ball.velocity = turn;
        ball.transform.position = ballPos;
    }

    private void KillMaze() {
        for (int i = 0; i < transform.childCount; i++) {
            savedMaze[i] = transform.GetChild(i).position;
        }
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).transform.position = new Vector3(1e4f, 1e4f, 1e4f);
        }
    }

    private void CreateMaze() {
        for (int i = 0; i < savedMaze.Count; i++) {
            transform.GetChild(i).transform.position = savedMaze[i];
        }
    }
    
    public void Add(object key, object value) {
        if (!memory.ContainsKey(key)) {
            memory.Add(key, value);
        }
    }

    public void Set(object key, object value) {
        Add(key, value);
        memory[key] = value;
    }

    public object Get(object key) {
        return memory[key];
    }

    public Vector3 GetPosition() {
        return ball.position;
    }
}
