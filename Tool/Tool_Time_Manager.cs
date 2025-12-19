using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏中时间相关管理---游戏中只用一个时间线去处理所有的时间触发相关逻辑
/// 1.定时执行一次
/// 2.定时循环（手机停止）
/// 3.定时执行N次
/// 4.不定时执行N次
/// </summary>
public class Tool_Time_Manager:Singleton<Tool_Time_Manager>
{
    /// <summary>
    /// 所有定时触发的回调列表
    /// </summary>
    List<Time_Handle_Data> delay_One_List = new List<Time_Handle_Data>();
    /// <summary>
    /// 所有循环触发的回调列表
    /// </summary>
    List<Time_Handle_Data> delay_Most_List = new List<Time_Handle_Data>();
    /// <summary>
    /// 定时执行回调
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="h"></param>
    public void Delay_Handle_One(int dt, Action h)
    {
        //一个触发的结构
        Time_Handle_Data data = new Time_Handle_Data()
        {
            startTimes = DateTime.Now.Ticks,
            delayTimes = dt,
            handle = h
        };
        delay_One_List.Add(data);
    }
    /// <summary>
    /// 循环时间回调
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="h"></param>
    public void Delay_Handle_Most(int dt, Action h)
    {
        Time_Handle_Data data = new Time_Handle_Data()
        {
            startTimes = DateTime.Now.Ticks,
            delayTimes = dt,
            handle = h
        };
        delay_Most_List.Add(data);
    }

    public void Update()
    {

        for (int i = 0; i < delay_One_List.Count; i++)
        {
            Time_Handle_Data data = delay_One_List[i];

            int delay = (int)(DateTime.Now.Ticks - data.startTimes)/10000000;

            if (delay >= data.delayTimes)
            {
                data.handle?.Invoke();

                delay_One_List.Remove(data);
            }
        }

        for (int i = 0; i < delay_Most_List.Count; i++)
        {
            Time_Handle_Data data = delay_Most_List[i];

            int delay = (int)(DateTime.Now.Ticks - data.startTimes) / 10000000;

            if (delay >= data.delayTimes)
            {
                data.handle?.Invoke();

                data.startTimes = DateTime.Now.Ticks;
            }
        }

    }

}

/// <summary>
/// 一个触发回调的结构
/// </summary>
public class Time_Handle_Data
{
    /// <summary>
    /// 一个触发回调的起始时间
    /// </summary>
    public long startTimes;
    /// <summary>
    /// 一个触发回调的延迟时间
    /// </summary>
    public int delayTimes;
    /// <summary>
    /// 回调函数
    /// </summary>
    public Action handle;
}