
using System;
using System.Collections.Generic;

/// <summary>
/// 时间管理器 -- 游戏只用一个时间线
/// </summary>
public class TimeManager:Singleton<TimeManager>
{
    
    List<Time_Handle_data> delay_one_list = new List<Time_Handle_data>();
    
    List<Time_Handle_data> delay_most_list = new List<Time_Handle_data>();
    
    /// <summary>
    /// 添加延迟执行事件
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="handle"></param>
    public void Delay_Hadnle_One(int dt, Action handle)
    {
        Time_Handle_data data = new Time_Handle_data
        {
            startsTimes = DateTime.Now.Ticks,
            delayTimes = dt,
            handle =  handle
        };
        delay_one_list.Add(data);
    }
    
    /// <summary>
    /// 添加定时循环事件
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="handle"></param>
    public void Delay_Hadnle_Most(int dt, Action handle)
    {
        Time_Handle_data data = new Time_Handle_data
        {
            startsTimes = DateTime.Now.Ticks,
            delayTimes = dt,
            handle =  handle
        };
        delay_most_list.Add(data);
    }

    public void Update()
    {
        for (int i = 0; i < delay_one_list.Count; i++)
        { 
             Time_Handle_data data = delay_one_list[i];
            int delay = (int)(DateTime.Now.Ticks - data.startsTimes)/10000000;
            if (delay >= data.delayTimes)
            {
                data.handle?.Invoke();
                delay_one_list.Remove(data);
            }
        }
        
        for (int i = 0; i < delay_most_list.Count; i++)
        {
            Time_Handle_data data = delay_one_list[i];
            int delay = (int)(DateTime.Now.Ticks - data.startsTimes)/10000000;
            if (delay >= data.delayTimes)
            {
                data.handle?.Invoke();
                
                data.startsTimes = DateTime.Now.Ticks;
            }
        } 
        
    }
}

public class Time_Handle_data
{
    public long startsTimes;
    
    /// <summary>
    /// 一个触发回调的延迟时间
    /// </summary>
    public int delayTimes;
    public Action handle;
}