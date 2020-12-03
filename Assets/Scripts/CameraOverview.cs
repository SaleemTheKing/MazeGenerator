using UnityEngine;

public class CameraOverview : MonoBehaviour
{
    public Camera camera;

    public GameObject _maze;

    void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void TryFindAMaze()
    {
        _maze = GameObject.FindWithTag("Maze");
    }

    private void FixedUpdate()
    {
        if (!_maze)
        {
            TryFindAMaze();
        }
        else
        {
            camera.transform.LookAt(_maze.transform);
        }
    }
}
