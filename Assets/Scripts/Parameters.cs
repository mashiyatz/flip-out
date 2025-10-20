using System.Collections.Generic;
using UnityEngine;

public class Parameters
{
    public enum DRINK { MEAD, ALE, GROG, WATER }

    public static int startCash = 1000;
    public static float timePerRound = 30;
    public static float newPatronInterval = 5;
    public static float patronWaitTime = 15;

    public static bool isGameStart = false;

    public static Dictionary<DRINK, Color> drinkToColor = new() {
        {DRINK.MEAD, Color.cyan},
        {DRINK.GROG, Color.gray},
        {DRINK.WATER, Color.blue},
        {DRINK.ALE, Color.green}
    };
}
