using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermiteCurve : MonoBehaviour
{
    public GameObject start;
    public GameObject startTangent;
    public GameObject end;
    public GameObject endTangent;
    public GameObject LerpExample;
    public float t = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(start.transform.position, startTangent.transform.position, Color.yellow);
        Debug.DrawLine(end.transform.position, endTangent.transform.position, Color.yellow);
        t = (1 + Mathf.Sin(Time.realtimeSinceStartup)) * 0.5f;
        //if (t > 1) t = 2 - t; // t now goes from 0 to 1 and then 1 back to 0 again and repeats
        Vector3 tang0 = startTangent.transform.position - start.transform.position;
        tang0 *= 4;
        Vector3 tang1 = endTangent.transform.position - end.transform.position;
        tang1 *= 4;
        DebugHermite(start.transform.position, tang0, end.transform.position, tang1, 30);
        Vector3 interPos = HermiteCurveInterpolate(start.transform.position, tang0, end.transform.position, tang1, t);
        LerpExample.transform.position = interPos;
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

    Vector3 HermiteCurveInterpolate(Vector3 point0, Vector3 tangent0, Vector3 point1, Vector3 tangent1, float t)
    {
        float tsq = t * t;
        float tcub = tsq * t;
        //calc 4 basis functions
        float h00 = 2 * tcub - 3 * tsq + 1;
        float h01 = -2 * tcub + 3 * tsq;
        float h10 = tcub - 2 * tsq + t;
        float ht11 = tcub - tsq;
        //return combined result
        return h00 * point0 + h10 * tangent0 + h01 * point1 + ht11 * tangent1;
    }

    void DebugHermite(Vector3 point0, Vector3 tangent0, Vector3 point1, Vector3 tangent1, int segs)
    {
        Vector3 segmentStart = point0;
        float increment = 1 / (float)segs;
        for (float t = increment; t < 1; t += increment)
        {
            Vector3 segmentEnd = HermiteCurveInterpolate(start.transform.position, tangent0, end.transform.position, tangent1, t);
            Debug.DrawLine(segmentStart, segmentEnd, Color.white);
            segmentStart = segmentEnd;
        }
    }


}
