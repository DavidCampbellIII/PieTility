using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using MyBox;

namespace PieTility
{
    public class FollowBezierCurve : MonoBehaviour
    {
        [SerializeField]
        [MustBeAssigned]
        [Tooltip("Curves to follow, in order")]
        private List<BezierCurve> bezierCurves;
        [SerializeField]
        [PositiveValueOnly]
        [Tooltip("Time in seconds it takes to travel across curves one time")]
        private float travelTime = 1f;
        [SerializeField] 
        [Tooltip("Should we loop back to the first curve upon completion?")]
        private bool shouldRepeat = true;

        private List<Vector2> points;
        private float totalDistance = 0f;

        private void Start()
        {
            points = new List<Vector2>();
            CalculateCurvePoints();
            StartCoroutine(FollowCurve());
        }

        private void CalculateCurvePoints()
        {
            //go through all bezier curves
            for (int curveIndex = 0; curveIndex < bezierCurves.Count; curveIndex++)
            {
                BezierCurve curve = bezierCurves[curveIndex];
                //grab control points associated with this curve
                Vector2 pointOne = curve.handles[0].position;
                Vector2 pointTwo = curve.handles[1].position;
                Vector2 pointThree = curve.handles[2].position;
                Vector2 pointFour = curve.handles[3].position;

                //calculate all points on the current curve
                for (float time = 0; time <= 1; time += curve.step)
                {
                    Vector2 p1 = Mathf.Pow(1 - time, 3) * pointOne;
                    Vector2 p2 = 3 * Mathf.Pow(1 - time, 2) * time * pointTwo;
                    Vector2 p3 = 3 * (1 - time) * Mathf.Pow(time, 2) * pointThree;
                    Vector2 p4 = Mathf.Pow(time, 3) * pointFour;

                    Vector2 point = p1 + p2 + p3 + p4;
                    points.Add(point);

                    if (time != 0f)
                    {
                        Vector2 prevPoint = points[points.Count - 2];
                        totalDistance += Vector2.Distance(point, prevPoint);
                    }
                }
            }
        }

        private IEnumerator FollowCurve()
        {
            while (shouldRepeat)
            {
                transform.position = points[0];
                for (int i = 1; i < points.Count; i++)
                {
                    Vector2 currentPoint = points[i];
                    Vector2 prevPoint = points[i - 1];
                    float distance = Vector2.Distance(currentPoint, prevPoint);

                    //if two points are on top of each other, don't bother lerping between them
                    //this also prevents a divide by 0 exception
                    if (distance == 0f)
                    {
                        continue;
                    }

                    float distRatio = distance / totalDistance;
                    float lerpTime = travelTime * distRatio;

                    float currentLerpTime = 0f;
                    while (currentLerpTime < lerpTime)
                    {
                        currentLerpTime += Time.deltaTime;
                        if (currentLerpTime > lerpTime)
                        {
                            currentLerpTime = lerpTime;
                        }

                        float perc = currentLerpTime / lerpTime;
                        transform.position = Vector2.Lerp(prevPoint, currentPoint, perc);
                        yield return null;
                    }
                }
            }
        }
    }
}