using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CMCalculater : SingletonHungry<CMCalculater>
{
    
    //获取主相机
    private Camera MainCamera
    {
        get
        {
            if (!_mainCamera)
            {
                _mainCamera = Camera.main;
                return _mainCamera;
            }
            else
            {
                return _mainCamera;
            }
        }
        set => _mainCamera = value;
    }
    private Camera _mainCamera;
    
    /// <summary>
    /// 获取鼠标坐标
    /// </summary>
    public Vector3 MousePosition => MainCamera.ScreenToWorldPoint(Input.mousePosition);

    /// <summary>
    /// 计算轨道位置
    /// </summary>
    /// <param name="xPosition">X</param>
    /// <returns>轨道下标</returns>
    public int CalculateTrack(float xPosition)
    {
        
        
        if (xPosition >= CMConstDatas.FirstTrackX - CMConstDatas.TrackXDistance / 2f &&
            xPosition < CMConstDatas.FirstTrackX + CMConstDatas.TrackXDistance / 2f)
        {
            return 0;
        }
        else if(xPosition >= CMConstDatas.FirstTrackX + CMConstDatas.TrackXDistance / 2f &&
                xPosition < CMConstDatas.FirstTrackX + 3f*CMConstDatas.TrackXDistance / 2f)
        {
            return 1;
        }
        else if (xPosition >= CMConstDatas.FirstTrackX + 3f*CMConstDatas.TrackXDistance / 2f &&
                 xPosition < CMConstDatas.FirstTrackX + 5f*CMConstDatas.TrackXDistance / 2f)
        {
            return 2;
        }
        else if (xPosition >= CMConstDatas.FirstTrackX + 5f * CMConstDatas.TrackXDistance / 2f &&
                 xPosition < CMConstDatas.FirstTrackX + 7f * CMConstDatas.TrackXDistance / 2f)
        {
            return 3;
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// 计算鼠标坐标轨道位置
    /// </summary>
    /// <returns></returns>
    public int CalculateTrack()
    {
        return CalculateTrack(MousePosition.x);
    }

    /// <summary>
    /// 计算时间位置
    /// </summary>
    /// <param name="yPosition">y</param>
    /// <param name="bpm">bpm</param>
    /// <param name="dividingCount">当前分割数</param>
    /// <returns></returns>
    public float CalculateTime(float yPosition, int bpm, int dividingCount)
    {
        int index;
        
        index = Mathf.RoundToInt(yPosition * bpm * dividingCount / 60f / CMConstDatas.DistancePerSecond);
        return index * 60f  / bpm / dividingCount;
    }

    /// <summary>
    /// 计算鼠标坐标时间位置
    /// </summary>
    /// <param name="bpm"></param>
    /// <param name="dividingCount"></param>
    /// <returns></returns>
    public float CalculateTime(int bpm, int dividingCount)
    {
        return CalculateTime(MousePosition.y, bpm, dividingCount);
    }

    
}
