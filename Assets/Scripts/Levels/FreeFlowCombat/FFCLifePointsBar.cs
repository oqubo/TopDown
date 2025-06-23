using UnityEngine;

public class FFCLifePointsBar : ProgressBar
{
    private new void Start()
    {
        base.Start();
    }

    protected override float GetProgress()
    {
        return GameInstance.Singleton.GetLifePercentage();
    }
}
