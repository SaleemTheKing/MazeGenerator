using UnityEngine;
using UnityEngine.UI;

public class CameraOverview : MonoBehaviour
{
    public Slider _widthSlider;
    public Slider _heightSlider;

    private Vector3 _distance = new Vector3(0, 9, -11);
    
    void Start()
    {
        SetDistance();
    }

    public void SetDistance()
    {
        /*
         * max width and heigth = 50
         * min width and height = 10
         * 50 - 10 = 40 steps
         * 33 steps in Y and Z coordinates are needed for the camera (tested by trial and error) to display a full maze
         * 40 / 33 = 1.12
         */
        float distanceFactor = ((_widthSlider.value + _heightSlider.value) / 2) - 10;
        transform.position = new Vector3(0, 
                                        _distance.y + (1.12f * distanceFactor),
                                        _distance.z - (1.12f * distanceFactor));
    }
}
