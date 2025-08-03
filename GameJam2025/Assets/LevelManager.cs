using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Maze1Container;
    public GameObject Maze2Container;
    public GameObject MazePlayer1;
    public GameObject MazePlayer2;

    public Tile StartingTilePlayer1 { get; private set; }
    public Tile StartingTilePlayer2 { get; private set; }
    public Tile FinishingTilePlayer1 { get; private set; }
    public Tile FinishingTilePlayer2 { get; private set; }

    public GameObject FinishTriggerPrefab;

    public int currentMazeSize = 5;
    public int currentMazeRandStep = 5;
    public int mazeModifier = 2;
    public int currentItemAmount = 5;

    public List<GameObject> CollectiblePrefabs;

    public delegate void FinishReachedDelegate();

    private int _numberOfMazes = 4;


    private void CleanUp()
    {
        foreach (Transform child in Maze1Container.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in Maze2Container.transform)
        {
            Destroy(child.gameObject);
        }

        GameManager.Instance.player1.ResetStats();
        GameManager.Instance.player2.ResetStats();
    }

    public IEnumerator StartRound()
    {
        CleanUp();
        currentMazeSize += mazeModifier;
        currentMazeRandStep += mazeModifier;
        currentItemAmount += mazeModifier;
        MazeGenerator.Instance.defaultSize = currentMazeSize;
        MazeGenerator.Instance.defaultRandSteps = currentMazeRandStep;

        List<Maze> mazes = new List<Maze>();
        Dictionary<Maze, int> mazeLengths = new Dictionary<Maze, int>();
        for (int i = 0; i < _numberOfMazes; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                try
                {
                    mazes.Add(MazeGenerator.Instance.GenerateMaze(gameObject));
                    break;
                }
                catch (Exception e)
                {
                    Debug.LogWarning("Something went wrong while generating maze " + i + ": " + e.Message);
                    if (j == 4)
                    {
                        Debug.LogError("Failed to generate maze after 5 attempts. Terminating.");
                        throw e;
                    }
                }
            }

            yield return null;
            mazeLengths.Add(mazes[i], mazes[i].GetBestSolutionLength());
        }

        mazes.Sort((x, y) => mazeLengths[x].CompareTo(mazeLengths[y]));

        int minDiff = Int32.MaxValue;
        int minDiffIndex = -1;
        for (int i = 0; i < _numberOfMazes - 1; i++)
        {
            int diff = Mathf.Abs(mazeLengths[mazes[i]] - mazeLengths[mazes[i + 1]]);
            if (diff < minDiff)
            {
                minDiff = diff;
                minDiffIndex = i;
            }
        }

        var m1 = mazes[minDiffIndex];
        var m2 = mazes[minDiffIndex + 1];
        Maze maze1, maze2;
        if (Random.Range(0, 2) == 0)
        {
            maze1 = m1;
            maze2 = m2;
        }
        else
        {
            maze1 = m2;
            maze2 = m1;
        }

        maze1.gameObject.transform.SetParent(Maze1Container.transform);
        maze2.gameObject.transform.SetParent(Maze2Container.transform);
        maze1.gameObject.transform.localPosition = Vector3.zero;
        maze2.gameObject.transform.localPosition = Vector3.zero;
        mazes.Remove(m1);
        mazes.Remove(m2);

        foreach (var maze in mazes)
        {
            Destroy(maze.gameObject);
        }

        foreach (Transform child in transform)
        {
            Destroy(child.transform.gameObject);
        }

        this.StartingTilePlayer1 = maze1.GetStartTile();
        this.StartingTilePlayer2 = maze2.GetStartTile();
        MazePlayer1.transform.position = this.StartingTilePlayer1.transform.position;
        MazePlayer2.transform.position = this.StartingTilePlayer2.transform.position;

        Debug.Log(maze1.GetBestSolutionLength());
        Debug.Log(maze2.GetBestSolutionLength());

        FinishReachedDelegate delegate1 = null;
        FinishReachedDelegate delegate2 = null;

        delegate1 += GameManager.Instance.Player1FinishesMaze;
        delegate2 += GameManager.Instance.Player2FinishesMaze;

        this.FinishingTilePlayer1 = maze1.GetFinishTile();
        this.FinishingTilePlayer2 = maze2.GetFinishTile();

        GameObject trigger1 = Instantiate(FinishTriggerPrefab, maze1.GetFinishTile().transform.position + Vector3.right,
            Quaternion.identity,
            Maze1Container.transform);
        GameObject trigger2 = Instantiate(FinishTriggerPrefab, maze2.GetFinishTile().transform.position + Vector3.right,
            Quaternion.identity,
            Maze2Container.transform);
        trigger1.GetComponent<FinishReachedScript>().SetDelegate(delegate1);
        trigger2.GetComponent<FinishReachedScript>().SetDelegate(delegate2);

        List<GameObject> collectibles = DetermineCollectiblesInMaze(currentItemAmount);

        maze1.SpawnCollectibles(collectibles);
        maze2.SpawnCollectibles(collectibles);

        maze1.AddVisualBorder();
        maze2.AddVisualBorder();

        GameManager.Instance.FreezePlayers(false);


    }

    private GameObject GetRandomCollectiblePrefab()
    {
        List<CollectibleController> controllers =
            CollectiblePrefabs.Select(x => x.GetComponent<CollectibleController>()).ToList();
        float weightSum = controllers.Select(x => x.GetFrequency()).Aggregate((x, y) => (x + y));
        float randomValue = Random.Range(0.0f, weightSum);
        float currentWeight = 0.0f;
        for (int i = 0; i < CollectiblePrefabs.Count; i++)
        {
            CollectibleController controller = controllers[i];
            currentWeight += controller.GetFrequency();
            if (currentWeight >= randomValue)
            {
                return CollectiblePrefabs[i];
            }
        }
        throw new Exception("No collectible found with the given frequency distribution.");
    }


    private List<GameObject> DetermineCollectiblesInMaze(int amount)
    {
        List<GameObject> collectibles = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            collectibles.Add(GetRandomCollectiblePrefab());
        }
        return collectibles;
    }

    void Start()
    {
        GameManager.Instance.FreezePlayers(true);
        StartCoroutine(StartRound());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
