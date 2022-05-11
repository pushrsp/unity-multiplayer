using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    void Start()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        Managers.Map.Load(1);
        // Managers.Resource.Instantiate("Creature/Player");
        Screen.SetResolution(840, 640, false);
    }
}