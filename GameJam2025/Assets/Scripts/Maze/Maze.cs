using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random = UnityEngine.Random;
public class Maze : MonoBehaviour
{
    public GameObject tilePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int size { get; set; }
    private Tile[,] tiles { get; set; }
    private Tile GetTile(int x, int y)
    {
        return this.tiles[x, y];
    }

    private Tile ApplyDirection(Tile tile, Direction direction)
    {
        int dx = 0, dy = 0;
        switch (direction)
        {
            case Direction.Up:
                dy = 1;
                break;
            case Direction.Down:
                dy = -1;
                break;
            case Direction.Left:
                dx = -1;
                break;
            case Direction.Right:
                dx = 1;
                break;
            default:
                break;
        }
        return this.GetTile(tile.x + dx, tile.y + dy);
    }

    private Tile start;
    private Tile end;
    public void SetAttribute(int size, int randSteps, double perturbationChance)
	{
		this.size = size;

        this.tiles = new Tile[this.size, this.size];

        for (int i = 0; i < this.size; i++)
        {
            for (int j = 0; j < this.size; j++)
            {
                GameObject tileObject = Instantiate(tilePrefab, this.transform);
                Tile tile = tileObject.GetComponent<Tile>();

                tile.SetAttributes(i, j, new Dictionary<Direction, bool>
                {
                    { Direction.Down, j == 0 },
                    { Direction.Up, j == this.size - 1 },
                    { Direction.Left, i == 0},
                    { Direction.Right, i == this.size - 1 }
                }, GetTile);

                this.tiles[i, j] = tile;

            }
        }
        this.start = this.tiles[0, this.size / 2];
        this.end = this.tiles[this.size - 1, this.size / 2];

        this.start.MakeWalkable(Direction.Left);
        this.end.MakeWalkable(Direction.Right);

        this.start.isMainPath = true;
        this.end.isMainPath = true;

        List<Tile> mainPath = new List<Tile> { this.start};

        Tile currentStart = this.start;
        for (int i = 0; i < randSteps; i++)
        {
            List<Direction> possibleDirections = currentStart.PossibleDirections();
            if (possibleDirections.Count == 0)
            {
                throw new Exception("No possible directions to move from the current tile.");
            }
            Dictionary<Tile, Direction> possibleTiles = possibleDirections.Select(direction => new { Tile = this.ApplyDirection(currentStart, direction), Direction = direction})
                .Where(entry => !entry.Tile.isMainPath && GetPath(entry.Tile, this.end) != null)
                .ToDictionary(x => x.Tile, x => x.Direction);

            if (possibleTiles.Count == 0)
            {
                throw new Exception("No possible tiles to move to from the current tile.");
            }
            Tile nextTile = possibleTiles.Keys.ToList<Tile>()[Random.Range(0, possibleTiles.Keys.Count)];


            currentStart.MakeWalkable(possibleTiles[nextTile]);
            nextTile.MakeWalkable(possibleTiles[nextTile].Opposite());
            nextTile.isMainPath = true;
            currentStart = nextTile;
            mainPath.Add(currentStart);
        }

        List<Tile> mainPathEnd = new List<Tile> { this.end };
        Tile currentEnd = this.end;
        for (int i = 0; i < randSteps; i++)
        {
            List<Direction> possibleDirections = currentEnd.PossibleDirections();
            if (possibleDirections.Count == 0)
            {
                throw new Exception("No possible directions to move from the current tile.");
            }
            Dictionary<Tile, Direction> possibleTiles = possibleDirections.Select(direction => new { Tile = this.ApplyDirection(currentEnd, direction), Direction = direction })
                .Where(entry => !entry.Tile.isMainPath && GetPath(entry.Tile, currentStart) != null)
                .ToDictionary(x => x.Tile, x => x.Direction);

            if (possibleTiles.Count == 0)
            {
                this.PrintMaze();
                Console.WriteLine($"Current End: {currentEnd.ToString()}");
                throw new Exception("No possible tiles to move to from the current tile.");
            }

            Tile nextTile = possibleTiles.Keys.ToList<Tile>()[Random.Range(0, possibleTiles.Keys.Count)];


            currentEnd.MakeWalkable(possibleTiles[nextTile]);
            nextTile.MakeWalkable(possibleTiles[nextTile].Opposite());
            nextTile.isMainPath = true;
            currentEnd = nextTile;
            mainPathEnd.Add(currentEnd);
        }
        List<Tile>? path = this.GetPath(currentStart, currentEnd);
        if (path == null)
        {
            throw new Exception("No path found between the start and end tiles.");
        }
        while (currentStart != currentEnd) {
            int currentStartIndex = 0;
            for (int i = 0; i < path.Count(); i++)
            {
                if (path[i] == currentEnd)
                {
                    this.MakePathWalkable(path);
                    try
                    {
                        mainPath.AddRange(path.GetRange(currentStartIndex, path.Count - currentStartIndex));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine($"{path.Count}, {currentStartIndex}");
                        throw;
                    }
                    currentStart = path[i];
                    currentStartIndex = i;
                    break;
                }
                if (Random.Range(0.0f, 1.0f) < perturbationChance)
                {
                    this.MakePathWalkable(path.GetRange(currentStartIndex,(i+1) - currentStartIndex));
                    mainPath.AddRange(path.GetRange(currentStartIndex,(i+1) - currentStartIndex));
                    currentStartIndex = i;
                    currentStart = path[i];
                    currentStart.isMainPath = true;
                    List<Direction> possibleDirections = currentStart.PossibleDirections();
                    Dictionary<Tile, Direction> possibleTiles = possibleDirections.Select(direction => new { Tile = this.ApplyDirection(currentStart, direction), Direction = direction })
                        .Where(entry => !entry.Tile.isMainPath && GetPath(entry.Tile, currentEnd) != null && path[i + 1] != entry.Tile)
                        .ToDictionary(x => x.Tile, x => x.Direction);
                    if (possibleTiles.Count != 0)
                    {
                        Tile nextTile = possibleTiles.Keys.ToList<Tile>()[Random.Range(0, possibleTiles.Keys.Count())];

                        currentStart.MakeWalkable(possibleTiles[nextTile]);
                        nextTile.MakeWalkable(possibleTiles[nextTile].Opposite());
                        nextTile.isMainPath = true;
                        currentStart = nextTile;
                        mainPath.Add(currentStart);
                        path = this.GetPath(currentStart, currentEnd);
                        if (path == null)
                        {
                            this.PrintMaze();
                            throw new Exception("No path found between the start and end tiles after perturbation.");
                        }
                        break;
                    }
                }
            }

        }
        mainPathEnd.Reverse();
        mainPath.AddRange(mainPathEnd);
        mainPath = mainPath.Distinct().ToList();

        foreach (Tile tile in mainPath)
        {
            if (Random.Range(0.0f, 1.0f) < 0.7)
            {
                this.PropagateNonMainPathTiles(tile);
            }
        }
        bool changed = true;
        while (changed) {
            changed = false;
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    if (!tiles[j, i].walkable.Values.Aggregate((x, y) => x || y))
                    {
                        List<Direction> possibleDirections = tiles[j, i].PossibleDirections();
                        Dictionary<Tile, Direction> possibleTiles = possibleDirections.Select(direction => new { Tile = this.ApplyDirection(tiles[j, i], direction), Direction = direction })
                            .Where(entry => entry.Tile.isMainPath || entry.Tile.offRoad)
                            .ToDictionary(x => x.Tile, x => x.Direction);
                        if (possibleTiles.Count > 0)
                        {
                            Tile nextTile = possibleTiles.Keys.ToList<Tile>()[Random.Range(0, possibleTiles.Keys.Count)];
                            tiles[j, i].MakeWalkable(possibleTiles[nextTile]);
                            nextTile.MakeWalkable(possibleTiles[nextTile].Opposite());
                            tiles[j, i].offRoad = true;
                            changed = true;
                        }
                    }
                }
            }
        }
    }

    private void PropagateNonMainPathTiles(Tile tile)
    {
        List<Tile> connectedTiles = new List<Tile>();
        {
            List<Direction> possibleDirections = tile.PossibleDirections();
            Dictionary<Tile, Direction> possibleTiles = possibleDirections.Select(direction => new { Tile = this.ApplyDirection(tile, direction), Direction = direction })
                .Where(entry => !entry.Tile.isMainPath && !entry.Tile.offRoad)
                .ToDictionary(x => x.Tile, x => x.Direction);

            double[] probabilities = new double[] { 0.95 };
            foreach (double prob in probabilities)
            {
                if (Random.Range(0.0f, 1.0f) < prob && possibleTiles.Count > 0)
                {
                    Tile nextTile = possibleTiles.Keys.ToList<Tile>()[Random.Range(0, possibleTiles.Keys.Count)];
                    connectedTiles.Add(nextTile);
                    tile.MakeWalkable(possibleTiles[nextTile]);
                    nextTile.MakeWalkable(possibleTiles[nextTile].Opposite());
                    possibleTiles.Remove(nextTile);

                }
                else
                {
                    break;
                }
            }
        }
        {
            List<Direction> possibleDirections = tile.PossibleDirections();
            Dictionary<Tile, Direction> possibleTiles = possibleDirections.Select(direction => new { Tile = this.ApplyDirection(tile, direction), Direction = direction })
                .ToDictionary(x => x.Tile, x => x.Direction);

            double[] probabilities = new double[] { 0.1, 0.1 };
            foreach (double prob in probabilities)
            {
                if (Random.Range(0.0f, 1.0f) < prob && possibleTiles.Count > 0)
                {
                    Tile nextTile = possibleTiles.Keys.ToList<Tile>()[Random.Range(0, possibleTiles.Keys.Count)];
                    connectedTiles.Add(nextTile);
                    tile.MakeWalkable(possibleTiles[nextTile]);
                    nextTile.MakeWalkable(possibleTiles[nextTile].Opposite());
                    possibleTiles.Remove(nextTile);

                }
                else
                {
                    break;
                }
            }
        }
        List<Tile> recursiveCalls = new List<Tile>();
        foreach (Tile connectedTile in connectedTiles)
        {
            if (!connectedTile.offRoad && !connectedTile.isMainPath)
            {
                connectedTile.offRoad = true;
                recursiveCalls.Add(connectedTile);
            }
        }

        foreach (Tile recursiveTile in recursiveCalls)
        {
            this.PropagateNonMainPathTiles(recursiveTile);
        }

    }

    private bool TileExists(int x, int y)
    {
        return x >= 0 && x < this.size && y >= 0 && y < this.size;
    }

    private List<Tile>? GetPath(Tile tile1, Tile tile2)
    {
        List<Tile> res = new List<Tile>();

        Queue<Tile> queue = new Queue<Tile>();

        Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();

        queue.Enqueue(tile1);


        while (queue.Count > 0)
        {
            Tile current = queue.Dequeue();
            if (current == tile2)
            {
                while (current != tile1)
                {
                    res.Add(current);
                    current = prev[current];
                }
                res.Add(tile1);
                res.Reverse();
                return res;
            }
            foreach (Direction direction in current.PossibleDirections().OrderBy(_ => Random.Range(0.0f, 1.0f)).ToList())
            {
                Tile nextTile = this.ApplyDirection(current, direction);
                if (nextTile != null && !prev.Keys.Contains(nextTile) && (!nextTile.isMainPath || nextTile==tile2))
                {
                    prev[nextTile] = current;
                    queue.Enqueue(nextTile);
                }
            }
        }

        return null;
    }

    private Direction MapDirection(int dx, int dy)
    {
        if (dx == 0 && dy == -1) return Direction.Down;
        if (dx == 0 && dy == 1) return Direction.Up;
        if (dx == -1 && dy == 0) return Direction.Left;
        if (dx == 1 && dy == 0) return Direction.Right;
        throw new ArgumentException($"Invalid direction: dx={dx}, dy={dy}");
    }

    public void MakePathWalkable(List<Tile> path)
    {
        for (int i = 0; i < path.Count() - 1; i++)
        {
            path[i].isMainPath = true;
            int dx = path[i + 1].x - path[i].x;
            int dy = path[i + 1].y - path[i].y;
            Direction direction = MapDirection(dx, dy);
            path[i].MakeWalkable(direction);
            path[i + 1].MakeWalkable(direction.Opposite());

        }
    }

    public void PrintMaze()
    {

        for (int i = 0; i < this.size; i++)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int j = 0; j < this.size; j++)
                    {
                        Console.Write(this.tiles[j, i].GetVisualRepresentation(row));
                    }
                Console.WriteLine();
            }
        }
    }
}
