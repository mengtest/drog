using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    private Vector3 deltaPos = Vector3.zero;

    public float timer = 0;

    public float keepTime = 0.3f;
    public float reduceTime = 0.2f;
    public float amplitude = 0.3f;
    public float drag = 0.1f;

    private void Start()
    {
        //keepTime = -1;
        //reduceTime = -1;
        //ShakeCameraFactor s = new ShakeCameraFactor(1,0.5f,1,1);
        //SetFactor(s);
    }


    void Update()
    {
        timer += Time.deltaTime;

        if(timer<reduceTime+keepTime)
        {
            transform.localPosition -= deltaPos;
            deltaPos = Random.insideUnitSphere * amplitude;
            if(timer>keepTime)
            {
                amplitude *= drag;
            }
            transform.localPosition += deltaPos;
        }
    }

    public void SetFactor(ShakeCameraFactor shakeCameraFactor)
    {
        timer = 0;
        amplitude = shakeCameraFactor._amplitude;
        drag = shakeCameraFactor._drag;
        keepTime = shakeCameraFactor._keepTime;
        reduceTime = shakeCameraFactor._reduceTime;
    }
}

public class ShakeCameraFactor
{
    /// <summary>
    /// 相机先在keepTime的时间里以amplitude的振幅震动，再在reduceTime时间内以以drag幅度衰减的amplitude振幅震动
    /// </summary>
    /// <param name="amplitude">振幅</param>
    /// <param name="drag">衰减系数</param>
    /// <param name="keepTime">震动持续时间</param>
    /// <param name="reduceTime">震动衰弱时间</param>
    public ShakeCameraFactor(float amplitude,float drag,float keepTime,float reduceTime)
    {
        _amplitude = amplitude;
        _drag = drag;
        _keepTime = keepTime;
        _reduceTime = reduceTime;
    }

    public float _amplitude;
    public float _drag;
    public float _keepTime;
    public float _reduceTime;
}