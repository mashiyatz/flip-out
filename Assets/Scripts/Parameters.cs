using System.Collections.Generic;
using UnityEngine;

public class Parameters
{
    public enum DRINK { MEAD, ALE, GROG, WATER, JUICE }

    public static int startCash = 1000;
    public static float timePerRound = 30;
    public static float newPatronInterval = 5;
    public static float patronWaitTime = 15;

    public static bool isGameStart = false;

    public static Dictionary<DRINK, Color> drinkToColor = new() {
        {DRINK.MEAD, new Color(0.698f, 0.122f, 0.02f)},
        {DRINK.GROG, new Color(0.698f, 0.122f, 0.11f)},
        {DRINK.WATER, new Color(0.698f, 0.122f, 0.565f)},
        {DRINK.ALE, new Color(0.698f, 0.122f, 0.11f)},
        {DRINK.JUICE, new Color(0.031f, 0.325f, 0.643f)}
    };
}
