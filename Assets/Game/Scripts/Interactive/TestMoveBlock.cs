using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveBlock : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    private Vector3 targetPos, startPos;
    private Vector3 t1, t2;
    private float animTime;
    private float timeToAnimate = 0.5f;

    private void Start()
    {
        targetPos = transform.position;
        t1 = targetPos;
        t2 = targetPos + new Vector3(0, 1, 0);
    }

    private void Update()
    {
        if (targetPos != transform.position)
        {
            animTime += Time.deltaTime;
            float t = animTime / timeToAnimate;
            transform.position = Vector3.Lerp(startPos, targetPos, curve.Evaluate(t));
        }
        else
            animTime = 0;
    }
    public void MoveBoxUp()
    {
        animTime = 0;
        startPos = transform.position;
        targetPos = t2;
    }
    public void MoveBoxDown()
    {
        animTime = 0;
        startPos = transform.position;
        targetPos = t1;
    }
}
