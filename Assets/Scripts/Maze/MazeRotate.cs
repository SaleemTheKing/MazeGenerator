using UnityEngine;

public class MazeRotate : MonoBehaviour
{
    private void FixedUpdate()
    {
        gameObject.transform.Rotate(0,0.25f,0);
    }
}
