using System.Collections.Generic;
using UnityEngine;

public class Parameters
{
    public enum DRINK { RED, PURPLE, GREEN, ORANGE, BLUE }

    public static int startCash = 1000;
    public static float timePerRound = 90;
    public static float newPatronInterval = 5;
    public static float patronWaitTime = 15;

    public static bool isGameStart = false;

    public static Dictionary<DRINK, Color> drinkToColor = new() {
        {DRINK.RED, Color.red},
        {DRINK.GREEN, Color.green},
        {DRINK.ORANGE, Color.yellow},
        {DRINK.PURPLE, Color.magenta},
        {DRINK.BLUE, Color.blue}
    };

    public static Dictionary<DRINK, float> drinkToCost = new()
    {
        {DRINK.RED, 6},
        {DRINK.GREEN, 8},
        {DRINK.ORANGE, 12},
        {DRINK.PURPLE, 20},
        {DRINK.BLUE, 36}
    };
}
