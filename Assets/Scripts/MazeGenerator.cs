using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour
{
    [Serializable]
    public class Cell
    {
        public bool isVisited;
        public GameObject north;    // 1    
        public GameObject east;     // 2
        public GameObject west;     // 3
        public GameObject south;    // 4    
    }

    [Header("Maze Settings")]
    public GameObject wall;
    public Slider widthSlider;
    public Slider heightSlider;

    // Generate a grid with the maze settings
    private float wallLength = 1f;
    private Vector3 _initPos;
    private GameObject _wallFolder;
    private int xSize;
    private int ySize;
    
    // Cells
    private Cell[] _cells;
    private int _currentCell;
    private int _totalCells;
    private int _visitedCells;
    
    // DFS
    private List<int> _lastCells;
    private bool _startedBuilding;
    private int _currentNeighbour;
    private int _backingUp;
    private int _wallToBreak;

    private void Start()
    {
        xSize = (int)widthSlider.value;
        ySize = (int)heightSlider.value;
        _totalCells = xSize * ySize;
        _visitedCells = 0;
        _startedBuilding = false;
        CreateGrid();
    }

    
    void CreateGrid()
    {
        _wallFolder = new GameObject();
        _wallFolder.name = "Maze";
        _wallFolder.tag = "Maze";
        _initPos = new Vector3((-xSize / 2) + (wallLength / 2), 0, (-ySize / 2) + (wallLength / 2));
        Vector3 myPos = _initPos;
        GameObject tempWall;

        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                myPos = new Vector3(_initPos.x + (j * wallLength) - (wallLength / 2),
                    0,
                    _initPos.z + (i * wallLength) - (wallLength / 2));
                tempWall = Instantiate(wall, myPos, Quaternion.identity);
                tempWall.transform.SetParent(_wallFolder.transform);
            }
        }

        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(_initPos.x + (j * wallLength),
                                    0,
                                    _initPos.z + (i * wallLength) - wallLength);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(0, 90, 0));
                tempWall.transform.SetParent(_wallFolder.transform);
            }
        }

        AssignCells();
    }

    void AssignCells()
    {
        _lastCells = new List<int>();
        _lastCells.Clear();
        int children = _wallFolder.transform.childCount;
        GameObject[] allWalls = new GameObject[children];
        _cells = new Cell[_totalCells];
        int eastWestProcess = 0;
        int childProcess = 0;
        int termCount = 0;

        for (int i = 0; i < children; i++)
        {
            allWalls[i] = _wallFolder.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < _cells.Length; i++)
        {
            if (termCount == xSize)
            {
                eastWestProcess++;
                termCount = 0;
            }
            
            _cells[i] = new Cell();
            _cells[i].east = allWalls[eastWestProcess];
            _cells[i].south = allWalls[childProcess + (xSize + 1) * ySize];
            
            eastWestProcess++;
            termCount++;
            childProcess++;
            
            _cells[i].west = allWalls[eastWestProcess];
            _cells[i].north = allWalls[childProcess + ((xSize + 1) * ySize) + xSize - 1];
        }
        CreateMaze();
    }

    void CreateMaze()
    {
        while (_visitedCells < _totalCells)
        {
            if (_startedBuilding)
            {
                GetAndAssignNeighbours();
                if (!_cells[_currentNeighbour].isVisited && _cells[_currentCell].isVisited)
                {
                    BreakWall();
                    _cells[_currentNeighbour].isVisited = true;
                    _visitedCells++;
                    _lastCells.Add(_currentCell);
                    _currentCell = _currentNeighbour;
                    
                    if (_lastCells.Count > 0)
                    {
                        _backingUp = _lastCells.Count - 1;
                    }
                }
            }
            else
            {
                _currentCell = Random.Range(0, _totalCells);
                _cells[_currentCell].isVisited = true;
                _visitedCells++;
                _startedBuilding = true;
            }
        }
        
        GetAndAssignNeighbours();
    }

    void BreakWall()
    {
        switch (_wallToBreak)
        {
            case 1:
                Destroy(_cells[_currentCell].north);
                break;
            case 2:
                Destroy(_cells[_currentCell].east);
                break;
            case 3:
                Destroy(_cells[_currentCell].west);
                break;
            case 4:
                Destroy(_cells[_currentCell].south);
                break;
        }
    }

    void GetAndAssignNeighbours()
    {
        int length = 0;
        int[] neighbours = new int[4];
        int[] connectingWall = new int[4];
        int check = (_currentCell + 1) / xSize;
        check -= 1;
        check *= xSize;
        check += xSize;
        
        /*
         * We count the maze like this:
         * [ 7 | 8 | 9 ]
         * [ 4 | 5 | 6 ]
         * [ 1 | 2 | 3 ]
         * Where every numbered cell has 4 walls a.k.a. neighbours 
         */
        
        
        // West wall
        if (_currentCell + 1 < _totalCells && (_currentCell + 1) != check)
        {
            if (!_cells[_currentCell + 1].isVisited)
            {
                neighbours[length] = _currentCell + 1;
                connectingWall[length] = 3;
                length++;
            }
        }
        
        // East wall
        if (_currentCell - 1 >= 0 && (_currentCell + 1) != check)
        {
            if (!_cells[_currentCell - 1].isVisited)
            {
                neighbours[length] = _currentCell - 1;
                connectingWall[length] = 2;
                length++;
            }
        }
        
        // North wall
        if (_currentCell + xSize < _totalCells)
        {
            if (!_cells[_currentCell + xSize].isVisited)
            {
                neighbours[length] = _currentCell + xSize;
                connectingWall[length] = 1;
                length++;
            }
        }
        
        // South wall
        if (_currentCell - xSize >= 0)
        {
            if (!_cells[_currentCell - xSize].isVisited)
            {
                neighbours[length] = _currentCell - xSize;
                connectingWall[length] = 4;
                length++;
            }
        }

        if (length != 0)
        {
            int randomNeighbour = Random.Range(0, length);
            _currentNeighbour = neighbours[randomNeighbour];
            _wallToBreak = connectingWall[randomNeighbour];
        }
        else
        {
            if (_backingUp > 0)
            {
                _currentCell = _lastCells[_backingUp];
                _backingUp--;
            }
        }
    }

    public void RegenerateMaze()
    {
        Destroy(GameObject.FindWithTag("Maze"));
        Start();
    }
}