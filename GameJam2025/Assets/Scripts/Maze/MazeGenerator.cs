using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using Random = Unity.Mathematics.Random;


public class MazeGenerator : MonoBehaviour
{
    public static MazeGenerator Instance { get; private set; }

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


    public int defaultSize = 20; // Size of the maze
    public int defaultRandSteps = 25; // Number of random steps to take
    public double defaultPerturbationChance = 0.5; // Chance of perturbation in the maze generation

    public GameObject mazePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Maze GenerateMaze(GameObject parent)
    {
        return GenerateMaze(parent, defaultSize, defaultRandSteps, defaultPerturbationChance);
    }

    public Maze GenerateMaze()
    {
        return GenerateMaze(gameObject, defaultSize, defaultRandSteps, defaultPerturbationChance);
    }

    public Maze GenerateMaze(GameObject parent, int size, int randSteps, double perturbationChance)
    {
        /*
         * Returns the start tile
         */
        GameObject mazeObject = Instantiate(mazePrefab, parent.transform);
        Maze maze = mazeObject.GetComponent<Maze>();
        maze.SetAttribute(size, randSteps, perturbationChance);
        return maze;
    }
}



public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
public static class Extensions
{
    public static Direction Opposite(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Invalid direction: {direction}")
        };
    }
}

