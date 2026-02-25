using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private float xstartPos;
    private float ystartPos;
    public GameObject cam;
    public float paralaxEffect;
    private void Start()
    {
        xstartPos = transform.position.x;
        ystartPos = transform.position.y;
    }
    void LateUpdate()
    {
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0);
    }
}