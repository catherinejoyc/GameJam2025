using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public Dictionary<Direction, bool> border { get; set; }
	public Dictionary<Direction, bool> walkable { get; private set; }

	public Func<int, int, Tile> getTile { get; set; }
	public int x { get; set; }
	public int y { get; set; }
	public GameObject leftWall;
	public GameObject rightWall;
	public GameObject bottomWall;
	public GameObject topWall;

	public bool isMainPath { get; set; } = false;
	public bool offRoad { get; set; } = false;

    public void SetAttributes(int x, int y, Dictionary<Direction, bool> border, Func<int, int, Tile> getTile)
	{
		this.x = x;
		this.y = y;
        this.border = border;
		this.getTile = getTile;
		this.walkable = new Dictionary<Direction, bool>
		{
			{ Direction.Up, false },
			{ Direction.Down, false },
			{ Direction.Left, false },
			{ Direction.Right, false }
		};
		UpdateSelf();
    }

    private void UpdateSelf()
    {
	    if (!Mathf.Approximately(transform.position.x, this.x) || !Mathf.Approximately(transform.position.y, this.y))
	    {
		    transform.position = new Vector3(this.x, this.y, transform.position.z);
	    }

	    topWall.SetActive(!walkable[Direction.Up]);
	    leftWall.SetActive(!walkable[Direction.Left]);
	    rightWall.SetActive(!walkable[Direction.Right]);
	    bottomWall.SetActive(!walkable[Direction.Down]);

    }

	public List<Direction> PossibleDirections()
	{
		return this.border.Where(x => !x.Value).Select(x => x.Key).ToList();
    }

    public override string? ToString()
    {
        return $"Tile({this.x},{this.y})";
    }

	public void MakeWalkable(Direction direction)
	{
		if (this.walkable.ContainsKey(direction))
		{
			this.walkable[direction] = true;
		}
		else
		{
			throw new ArgumentException($"Direction {direction} is not valid for Tile ({this.x}, {this.y})");
		}
		UpdateSelf();
    }

    public String GetVisualRepresentation(int row)
	{
        StringBuilder sb = new StringBuilder();
        switch (row)
		{
			case 0:
                sb.Append("■");
                sb.Append(this.walkable[Direction.Up] ? "O" : "■");
                sb.Append("■");

                break;

			case 1:
				sb.Append(this.walkable[Direction.Left] ? "O" : "■");
                if (this.walkable.Values.Aggregate((x, y) => x || y))
                {
					if (isMainPath)
					{
						sb.Append("M");
                    }
					else {
						sb.Append("O");
                    }
                } else
				{
					sb.Append("■");
                }
                sb.Append(this.walkable[Direction.Right] ? "O" : "■");
				break;

			case 2:
				sb.Append("■");
				sb.Append(this.walkable[Direction.Down] ? "O" : "■");
				sb.Append("■");
				break;

            default:
				throw new Exception($"Invalid row {row} for Tile ({this.x}, {this.y})");
		}

		return sb.ToString();

    }
}
