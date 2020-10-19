using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Timer
{
    public float timeRemaining;
    public bool IsRunning;
    float lifeTime;
    
    void Update()
    {

    }

    public Timer(float lifeTime)
    {
        this.lifeTime = lifeTime;
        timeRemaining = lifeTime;
        IsRunning = false;
    }

    public void timerUpdate(){
        if (IsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                IsRunning = false;
                GameMaster.GM.endRound(null);
            }
        }
    }

    public void start(){
        IsRunning = true;
    }

    public void restart(){
        IsRunning = true;
        timeRemaining = lifeTime;
    }

    public override string ToString(){
        return string.Format("{0:0}:{1:0}", timeRemaining/60, timeRemaining%60);
    }
}
