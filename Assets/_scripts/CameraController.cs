using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera[] cameras;
    int current_num;
       

    // Start is called before the first frame update
    void Start()
    {
        SetCameraActive(current_num);
    }

    // Update is called once per frame
    void Update()
    {
        //SetCameraActive(current_num);
    }

    public void SetCameraActive(int num)
    {
        for (int i = 0; i < cameras.Length; i++) {
            cameras[i].gameObject.SetActive(num == i);
        }
    }
}
