using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeFactory : ScriptableObject
{
    private GameObject ParedeX;
    private GameObject ParedeZ;

    private Vector3 LastPosition;
    private Vector3 SecondLast;
    private Vector3 FirstPosition;
    private Vector2Int start;
    private Vector2Int end;
    private Vector2Int[] directions;

    private List<Vector2Int> MazePath;
    private List<Vector2Int> RandomMazePath;
    private int mazeWidth;
    private int mazeHeight;

    private Cell[,] grid;

    private List<GameObject> paredes;

    public class Cell
    {
        public bool[] walls = { true, true, true, true }; // Top, Right, Bottom, Left
        public bool visited = false;
        Vector3 coordinates;

        public Cell(Vector3 Coordinates)
        {
            coordinates = Coordinates;
        }

        public Vector3 GetCoordinates()
        {
            return coordinates;
        }
    }
    public void GenerateMaze(PlatformFactory spawner)
    {
        paredes = new List<GameObject>();

        ParedeZ = Resources.Load("Walls/Wall_Z", typeof(GameObject)) as GameObject;
        ParedeX = Resources.Load("Walls/Wall_X", typeof(GameObject)) as GameObject;

        //Starting variables
        MazePath = new List<Vector2Int>();
        SecondLast = new Vector3(0, 0, 0);
        FirstPosition = new Vector3(0, 0, 0);
        LastPosition = new Vector3(0, 0, 0);
        directions = new Vector2Int[]
        {
            new Vector2Int(0, 1), // Top
            new Vector2Int(1, 0), // Right
            new Vector2Int(0, -1), // Bottom
            new Vector2Int(-1, 0) // Left
        };
        mazeWidth = 10;
        mazeHeight = 10;
        start = new Vector2Int(0, 0);
        end = new Vector2Int(0, 0);
        RandomMazePath = new List<Vector2Int>();


        //Debug.Log("Number of Platforms = " + spawner.PlatformsPositions.Length);
        LastPosition = spawner.PlatformsPositions[spawner.GetPlatformIndex() - 1];
        LastPosition.y = 0;
        SecondLast = spawner.PlatformsPositions[spawner.GetPlatformIndex() - 2];
        //Debug.Log("LastPosition: " + LastPosition);

        FirstPosition = new Vector3(45 * -Math.Sign(LastPosition.x), 0, 45 * -Math.Sign(LastPosition.z));
        //Debug.Log("First Position: " + FirstPosition);



        grid = new Cell[mazeWidth, mazeHeight];
        InitializeGrid();

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = mazeHeight - 1; y >= 0; y--)
            {
                //Debug.Log("Coordenadas a testas = " + grid[x,y].GetCoordinates());
                if (FirstPosition == grid[x, y].GetCoordinates())
                {
                    start = new Vector2Int(x, y);
                }
                if (LastPosition == grid[x, y].GetCoordinates())
                {
                    end = new Vector2Int(x, y);
                }
            }
        }
        GenerateMazePositions(start, end);
        //PrintGrid();

        GenerateRandomPaths();


        //Creating the wall positions
        // Top, Right, Bottom, Left WALLS
        List<Vector3> WallPositions = new List<Vector3>();
        for (int i = 0; i < MazePath.Count; i++)
        {
            bool[] BuildWalls = grid[MazePath[i].x, MazePath[i].y].walls; // Top, Right, Bottom, Left
            //Debug.Log("The wall are: " + BuildWalls[0] + " " + BuildWalls[1] + " " + BuildWalls[2] + " " + BuildWalls[3]);

            //Getting the position of the tile to build the wall
            Vector3 TilePosition = grid[MazePath[i].x, MazePath[i].y].GetCoordinates();
            for (int j = 0; j < BuildWalls.Length; j++)
            {
                switch (j)
                {
                    case 0:
                        if (BuildWalls[j])
                        {
                            WallPositions.Add(TilePosition + new Vector3(0, 7.5f, 5));
                        }
                        break;
                    case 1:
                        if (BuildWalls[j])
                        {
                            WallPositions.Add(TilePosition + new Vector3(5, 7.5f, 0));
                        }
                        break;
                    case 2:
                        if (BuildWalls[j])
                        {
                            WallPositions.Add(TilePosition + new Vector3(0, 7.5f, -5));
                        }
                        break;
                    case 3:
                        if (BuildWalls[j])
                        {
                            WallPositions.Add(TilePosition + new Vector3(-5, 7.5f, 0));
                        }
                        break;
                }
            }
        }

        //Getting the positions of the walls of the random paths
        for (int i = 0; i < RandomMazePath.Count; i++)
        {
            bool[] BuildWalls = grid[RandomMazePath[i].x, RandomMazePath[i].y].walls; // Top, Right, Bottom, Left
            //Debug.Log("The wall are: " + BuildWalls[0] + " " + BuildWalls[1] + " " + BuildWalls[2] + " " + BuildWalls[3]);

            //Getting the position of the tile to build the wall
            Vector3 TilePosition = grid[RandomMazePath[i].x, RandomMazePath[i].y].GetCoordinates();
            for (int j = 0; j < BuildWalls.Length; j++)
            {
                switch (j)
                {
                    case 0:
                        if (BuildWalls[j])
                        {
                            WallPositions.Add(TilePosition + new Vector3(0, 7.5f, 5));
                        }
                        break;
                    case 1:
                        if (BuildWalls[j])
                        {
                            WallPositions.Add(TilePosition + new Vector3(5, 7.5f, 0));
                        }
                        break;
                    case 2:
                        if (BuildWalls[j])
                        {
                            WallPositions.Add(TilePosition + new Vector3(0, 7.5f, -5));
                        }
                        break;
                    case 3:
                        if (BuildWalls[j])
                        {
                            WallPositions.Add(TilePosition + new Vector3(-5, 7.5f, 0));
                        }
                        break;
                }
            }
        }
        //Instatiating the walls
        for (int i = 0; i < WallPositions.Count; i++)
        {
            if (WallPositions[i].x % 10 == 0)
            {
                paredes.Add(Instantiate(ParedeZ, WallPositions[i], Quaternion.identity));
            }
            else
            {
                paredes.Add(Instantiate(ParedeX, WallPositions[i], Quaternion.identity));
            }

        }

        //Vector3 SecondLast = spawner.PlatformsPositions[spawner.PlatformIndex - 2];
        Vector3 center = (LastPosition + SecondLast) / 2;
        Vector3 size = new Vector3(Math.Abs(LastPosition.x - SecondLast.x), Math.Abs(LastPosition.y - SecondLast.y), Math.Abs(LastPosition.z - SecondLast.z));
        size.x = size.x == 0 ? 1 : size.x;
        size.y = size.y == 0 ? 1 : size.y;
        size.z = size.z == 0 ? 1 : size.z;
        Collider[] hitColliders = Physics.OverlapBox(center, size / 2);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Wall"))
            {
                GameObject Wall = hitCollider.gameObject; // There is a wall between the platforms
                Wall.transform.position = new Vector3(Wall.transform.position.x, Wall.transform.position.y - 5f, Wall.transform.position.z);
            }
        }
    }

    //Initializes the grid with the cells 10*10
    private void InitializeGrid()
    {
        //Debug.Log("Im in InitializeGrid!");
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                grid[x, y] = new Cell(new Vector3(-45 + x * 10, 0, -45 + y * 10));
            }
        }
    }

    //Sets the start and end points of the maze
    public void GenerateMazePositions(Vector2Int start, Vector2Int end)
    {
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        stack.Push(start);
        //Debug.Log("Start: " + start + " End: " + end);
        grid[start.x, start.y].visited = true;
        while (stack.Count > 0)
        {
            Vector2Int current = stack.Peek();
            if (current == end)
            {
                while (stack.Count > 0)
                {
                    MazePath.Add(stack.Pop());
                }
                //Array.Clear(grid[MazePath[0].x, MazePath[0].y].walls,0, grid[MazePath[0].x, MazePath[0].y].walls.Length);
                //Debug.Log("End reached at current="+current+ " end="+end);
                return;
            }
            List<Vector2Int> unvisitedNeighbors = GetUnvisitedNeighbors(current);
            //Debug.Log("Unvisited Neighbors = " + unvisitedNeighbors.Count + " at current = " + current);
            if (unvisitedNeighbors.Count > 0)
            {
                Vector2Int neighbor = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                //Debug.Log("The choosen neighbor is = " + neighbor);
                RemoveWall(current, neighbor);
                stack.Push(neighbor);
                grid[neighbor.x, neighbor.y].visited = true;
            }
            else
            {
                Vector2Int aux = stack.Pop();
                //stack.Pop();
                for (int i = 0; i < 4; i++)
                {
                    if ((stack.Peek() + directions[i]) == aux)
                    {
                        aux = stack.Peek();
                        grid[aux.x, aux.y].walls[i] = true;
                    }
                }
            }
        }
    }
    //Removes the wall between the current cell and the neighbor cell
    void RemoveWall(Vector2Int current, Vector2Int neighbor)
    {
        if (neighbor.x == current.x)
        {
            if (neighbor.y < current.y)
            {
                grid[current.x, current.y].walls[2] = false; // Remove bottom wall
                grid[neighbor.x, neighbor.y].walls[0] = false; // Remove top wall
            }
            else if (neighbor.y > current.y)
            {
                grid[current.x, current.y].walls[0] = false; // Remove top wall
                grid[neighbor.x, neighbor.y].walls[2] = false; // Remove bottom wall
            }
        }
        else if (neighbor.y == current.y)
        {
            if (neighbor.x > current.x)
            {
                grid[current.x, current.y].walls[1] = false; // Remove right wall
                grid[neighbor.x, neighbor.y].walls[3] = false; // Remove left wall
            }
            else if (neighbor.x < current.x)
            {
                grid[current.x, current.y].walls[3] = false; // Remove left wall
                grid[neighbor.x, neighbor.y].walls[1] = false; // Remove right wall
            }
        }
    }

    //Gets the unvisited neighbors of a cell
    List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        //Debug.Log("Cell = " + cell);
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbor = cell + direction;
            //Debug.Log("The neighbor is = " + neighbor + " and the cell is = " + cell + " and the direction is = " + direction);
            //Debug.Log("The neighbor is valid = " + IsValid(neighbor) + " and the cell is visited value is " + grid[neighbor.x, neighbor.y].visited);
            if (IsValid(neighbor) && !grid[neighbor.x, neighbor.y].visited)
            {
                //Debug.Log("Its valid and unvisited");
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    //Checks if a cell is inside the grid
    bool IsValid(Vector2Int cell)
    {
        return cell.x >= 0 && cell.x < mazeWidth &&
               cell.y >= 0 && cell.y < mazeHeight;
    }

    void GenerateRandomPaths()
    {
        List<Vector2Int> RandomPoints = new List<Vector2Int>();
        //Debug.Log("The maze path is: ");
        //PrintGrid(MazePath);

        //Creates random points to start the randomPaths
        for (int i = 0; i < MazePath.Count / 5; i++)
        {
            //Debug.Log("Generation Random Path" + i + " of " + (int)(MazePath.Count / 5)+"paths");
            Vector2Int aux = (MazePath[Random.Range(0, MazePath.Count - 5)]);
            if (RandomPoints.Contains(aux))
            {
                i--;
            }
            else
            {
                RandomPoints.Add(aux);
            }
        }
        for (int i = 0; i < RandomPoints.Count; i++)
        {
            RestartVisited();
            GenerateRandomPath(RandomPoints[i]);
            //Debug.Log("We got the random paths:");
            //PrintGrid(RandomMazePath);
        }
    }
    void RestartVisited()
    {
        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeHeight; j++)
            {
                if (!MazePath.Contains(new Vector2Int(i, j)) && !RandomMazePath.Contains(new Vector2Int(i, j)))
                {
                    //Debug.Log("This cell"+ new Vector2Int(i,j)+" is not in either the path");
                    grid[i, j].visited = false;
                }
            }
        }
    }

    void GenerateRandomPath(Vector2Int start)
    {
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        stack.Push(start);
        grid[start.x, start.y].visited = true;
        while (stack.Count > 0)
        {
            Vector2Int current = stack.Peek();
            /*if (current == end)
            {
                while (stack.Count > 0)
                {
                    MazePath.Add(stack.Pop());
                }
                //Array.Clear(grid[MazePath[0].x, MazePath[0].y].walls,0, grid[MazePath[0].x, MazePath[0].y].walls.Length);
                //Debug.Log("End reached at current="+current+ " end="+end);
                return;
            }*/
            List<Vector2Int> unvisitedNeighbors = GetUnvisitedNeighbors(current);
            //Debug.Log("Unvisited Neighbors = " + unvisitedNeighbors.Count + " at current = " + current);
            if (unvisitedNeighbors.Count > 0)
            {
                Vector2Int neighbor = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                //Debug.Log("The choosen neighbor is = " + neighbor);
                RemoveWall(current, neighbor);
                stack.Push(neighbor);
                grid[neighbor.x, neighbor.y].visited = true;
            }
            else
            {
                /*Vector2Int aux = stack.Pop();
                //stack.Pop();
                for (int i = 0; i < 4; i++)
                {
                    if ((stack.Peek() + directions[i]) == aux)
                    {
                        aux = stack.Peek();
                        grid[aux.x, aux.y].walls[i] = true;
                    }
                }*/
                while (stack.Count > 0)
                {
                    RandomMazePath.Add(stack.Pop());
                }
            }
        }

    }
    public void DestroyMaze()
    {
        for (int i = 0; i < paredes.Count; i++)
        {
            Destroy(paredes[i]);
        }
    }
}
