using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Judge
{
    // 圆心p(x, y), 半径r, 扇形圆心p1(x1, y1), 扇形正前方最远点p2(x2, y2), 扇形夹角弧度值theta(0,pi)
    public static bool IsCircleIntersectFan(float x, float y, float r, float x1, float y1, float x2, float y2, float theta)
    {
        // 计算扇形正前方向量 v = p1p2
        //Debug.Log(x + " " + y + " " + r + " " + x1 + " " + y1 + " " + x2 + " " + y2 + " " + theta);
        float vx = x2 - x1;
        float vy = y2 - y1;
    
        // 计算扇形半径 R = v.length()
        float R = (float)Math.Sqrt(vx * vx + vy * vy);
    
        // 圆不与扇形圆相交，则圆与扇形必不相交
        if ((x - x1) * (x - x1) + (y - y1) * (y - y1) > (R + r) * (R + r))
            return false;
    
    
        // 根据夹角 theta/2 计算出旋转矩阵，并将向量v乘该旋转矩阵得出扇形两边的端点p3,p4
        float h = theta * 0.5f;
        float c = (float)Math.Cos(h);
        float s = (float)Math.Cos(h);
        float x3 = x1 + (vx * c - vy * s);
        float y3 = y1 + (vx * s + vy * c);
        float x4 = x1 + (vx * c + vy * s);
        float y4 = y1 + (-vx * s + vy * c);
    
        // 如果圆心在扇形两边夹角内，则必相交
        float d1 = EvaluatePointToLine(x, y, x1, y1, x3, y3);
        float d2 = EvaluatePointToLine(x, y, x4, y4, x1, y1);
        if (d1 >= 0 && d2 >= 0)
            return true;
    
        // 如果圆与任一边相交，则必相交
        if (IsCircleIntersectLineSeg(x, y, r, x1, y1, x3, y3))
            return true;
        if (IsCircleIntersectLineSeg(x, y, r, x1, y1, x4, y4))
            return true;
    
        return false;
    }
    static float EvaluatePointToLine(float x, float y, float x1, float y1, float x2, float y2)
    {
        float a = y2 - y1;
        float b = x1 - x2;
        float c = x2 * y1 - x1 * y2;
    
        return a * x + b * y + c;
    }

    static bool IsCircleIntersectLineSeg(float x, float y, float r, float x1, float y1, float x2, float y2)
    {
        float vx1 = x - x1;
        float vy1 = y - y1;
        float vx2 = x2 - x1;
        float vy2 = y2 - y1;
    
    
        // len = v2.length()
        float len = (float)Math.Sqrt(vx2 * vx2 + vy2 * vy2);
    
        // v2.normalize()
        vx2 /= len;
        vy2 /= len;
    
        // u = v1.dot(v2)
        // u is the vector projection length of vector v1 onto vector v2.
        float u = vx1 * vx2 + vy1 * vy2;
    
        // determine the nearest point on the lineseg
        float x0 = 0f;
        float y0 = 0f;
        if (u <= 0)
        {
            // p is on the left of p1, so p1 is the nearest point on lineseg
            x0 = x1;
            y0 = y1;
        }
        else if (u >= len)
        {
            // p is on the right of p2, so p2 is the nearest point on lineseg
            x0 = x2;
            y0 = y2;
        }
        else
        {
            // p0 = p1 + v2 * u
            // note that v2 is already normalized.
            x0 = x1 + vx2 * u;
            y0 = y1 + vy2 * u;
        }
    
        return (x - x0) * (x - x0) + (y - y0) * (y - y0) <= r * r;
    }
}
