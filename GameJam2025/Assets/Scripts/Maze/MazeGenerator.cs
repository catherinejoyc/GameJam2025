using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using Random = Unity.Mathematics.Random;


public class MazeGenerator : MonoBehaviour
{

    public int defaultSize = 30; // Size of the maze
    public int defaultRandSteps = 40; // Number of random steps to take
    public double defaultPerturbationChance = 0.5; // Chance of perturbation in the maze generation
    public Vector3 defaultPosition = new Vector3(0, 0, 0); // Default position to spawn the maze

    public GameObject mazePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMaze()
    {
        GenerateMaze(defaultPosition, defaultSize, defaultRandSteps, defaultPerturbationChance);
    }

    public void GenerateMaze(Vector3 position, int size, int randSteps, double perturbationChance)
    {
        GameObject mazeObject = Instantiate(mazePrefab,  position, Quaternion.identity, this.transform);
        Maze maze = mazeObject.GetComponent<Maze>();
        maze.SetAttribute(size, randSteps, perturbationChance);
        maze.PrintMaze();
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

