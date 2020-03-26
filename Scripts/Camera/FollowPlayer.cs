using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowPlayer : MonoBehaviour
{
    public Transform player = null;
    public Vector3 initpos = new Vector3(0,5,-5);
    public Vector3 initrot = new Vector3(45,0,0);
    public Vector3 dirv;
    public Vector3 offset;
    private GameObject Camera;
    private EasyTouchMove touchR;
    public int dir = 0;
    void Start()
    {
        touchR = GameObject.Find("touchR").GetComponent<EasyTouchMove>();
        Camera = GameObject.Find("Main Camera");
    }


    Vector3 oldv,newv;
    void Update()
    {
        if(player==null) {
            return ;
        }
        if(touchR.Horizontal>10) dir=(dir+1)%24;
        else if(touchR.Horizontal<-10) dir=(dir-1+24)%24;
        if(Input.GetKeyDown("q")) {
            dir-=1;
            dir+=24;
            dir%=24;
        } else if(Input.GetKeyDown("e")) {
            dir+=1;
            dir%=24;
        }
        oldv = transform.position;
        transform.position = player.position+initpos;
        transform.RotateAround(player.position,player.up,dir*15f);
        newv = transform.position;
        transform.position = oldv;
        transform.position = Vector3.Lerp(oldv,newv,0.3f);
        //transform.eulerAngles = initrot;
        Camera.transform.forward = player.position-transform.position;
    }

    public int GetCameraDir() {
        return dir;
    }

}

/*
public class FollowPlayer : MonoBehaviour
{

    public Transform _Player;
    public Vector3 _offset;
    bool flag;

    public float angleSpeed = 1;
    public float r;
    public float angle;
    void Start()
    {
        flag = false;
        r = Mathf.Sqrt(_offset.x * _offset.x + _offset.z * _offset.z);
        angle = 0;

    }


    void Update()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            angle += angleSpeed * Time.deltaTime;
            _offset = new Vector3(r * Mathf.Cos(angle - Mathf.PI / 2), _offset.y, r * Mathf.Sin(angle - Mathf.PI / 2));
        }
        if(Input.GetKey(KeyCode.E))
        {
            angle -= angleSpeed * Time.deltaTime;
            _offset = new Vector3(r * Mathf.Cos(angle - Mathf.PI / 2), _offset.y, r * Mathf.Sin(angle - Mathf.PI / 2));
        }

        if (!flag)
        {
            if(_Player!=null)
            {
                //_offset = transform.position - _Player.position;
                flag = true;
            }
        }
        else
        {
            transform.position = _Player.position + _offset;
            transform.rotation = Quaternion.LookRotation(_Player.position + new Vector3(0, 2, 0) - transform.position);
        }

    }

}*/