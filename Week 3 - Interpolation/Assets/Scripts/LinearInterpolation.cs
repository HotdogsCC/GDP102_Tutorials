using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearInterpolation : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public GameObject LerpExample;
    public float t = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t = (1 + Mathf.Sin(Time.realtimeSinceStartup)) * 0.5f;
        if (t > 1) t = 2 - t; // t now goes from 0 to 1 and then 1 back to 0 again and repeats
        Debug.DrawLine(start.transform.position, end.transform.position, Color.blue);
        Vector3 interpolatedPosition = MyVector3Lerp(start.transform.position, end.transform.position, t);
        LerpExample.transform.position = interpolatedPosition;
    }

    float MyFloatLerp(float start, float end, float t)
    {
        return start + (end - start) * t;
    }

    Vector3 MyVector3Lerp(Vector3 start, Vector3 end, float t)
    {
        float x = MyFloatLerp(start.x, end.x, t);
        float y = MyFloatLerp(start.y, end.y, t);
        float z = MyFloatLerp(start.z, end.z, t);
        return new Vector3(x, y, z);
    }



}
