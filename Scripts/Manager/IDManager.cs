using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ID管理器，用于分配唯一ID
/// </summary>
public class IDManager
{
    public static int curID = 0;

    /// <summary>
    /// 获得唯一ID
    /// </summary>
    /// <returns>唯一ID</returns>
    public static int GetID() {
        return curID++;
    }
}
