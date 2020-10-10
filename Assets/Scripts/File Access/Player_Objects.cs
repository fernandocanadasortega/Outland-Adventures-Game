using System.Collections;
using System.Collections.Generic;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// Static class that hold the player inventory during the game session
/// </summary>
public static class Player_Objects
{
    private static string small_Key;
    private static string chest_Key;
    private static string sword;

    public static string Small_Key { get => small_Key; set => small_Key = value; }
    public static string Chest_Key { get => chest_Key; set => chest_Key = value; }
    public static string Sword { get => sword; set => sword = value; }
}
