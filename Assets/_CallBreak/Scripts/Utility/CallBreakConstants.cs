using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FGSOfflineCallBreak.CallBreakRemoteConfigClass;

public static class CallBreakConstants
{
    public const string TurnMsgDes = "It's not your turn yet , please wait.";
    public const string CardMsgDes = "You should play     follow the first player";
    public const string StandardRoundInfoDes = "Enjoy a standard 5-round game.";
    public const string QuickRoundInfoDes = "Enjoy a quick 3-round game.";

    public const string ExitFromGamePlayMessage = "Are you sure to quit this round ? Your coin will be lost!";
    public const string ExitFromDashboardPlayMessage = "Are you sure to quit ?";

    public static List<string> worldMapCountry = new List<string> { "Singapore", "Egypt", "India", "Australia", "China", "Germany", "France", "Europe", "Canada", "USA" };

    public static List<int> coinsToClearLevel = new List<int> { 1000, 5000, 15000, 27500, 40000, 50000, 62500, 75000, 87500, 100000 };
    public static List<int> allLobbyAmount = new List<int> { 0, 10, 20, 30, 40, 50, 100, 200, 300, };

    private const string UserDetialsJsonStringKey = "UserDetialsJsonString";
    private const string AvatarPurchaseDetailsKey = "AvatarPurchaseDetails";
    private const string DailyRewardDetailsKey = "DailyRewardDetails";

    public static CallBreakRemoteConfig callBreakRemoteConfig;

    public static bool RegisterOrNot()
    {
        return PlayerPrefs.HasKey(UserDetialsJsonStringKey);
    }

    public static string UserDetialsJsonString
    {
        get => PlayerPrefs.GetString(UserDetialsJsonStringKey);
        set => PlayerPrefs.SetString(UserDetialsJsonStringKey, value);
    }

    public static bool ItHasPurchaseDataOrNot()
    {
        return PlayerPrefs.HasKey(AvatarPurchaseDetailsKey);
    }

    public static string AvatarPurchaseJsonString
    {
        get => PlayerPrefs.GetString(AvatarPurchaseDetailsKey);
        set => PlayerPrefs.SetString(AvatarPurchaseDetailsKey, value);
    }

    public static bool ItHasDailyRewardDataOrNot()
    {
        return PlayerPrefs.HasKey(DailyRewardDetailsKey);
    }

    public static string DailyRewardJsonString
    {
        get => PlayerPrefs.GetString(DailyRewardDetailsKey);
        set => PlayerPrefs.SetString(DailyRewardDetailsKey, value);
    }

    public static bool IsSound
    {
        get => PlayerPrefs.GetInt("SOUND", 1) == 1 ? true : false;
        set => PlayerPrefs.SetInt("SOUND", value == true ? 1 : 0);
    }

    public static bool IsMusic
    {
        get => PlayerPrefs.GetInt("MUSIC", 1) == 1 ? true : false;
        set => PlayerPrefs.SetInt("MUSIC", value == true ? 1 : 0);
    }

    public static bool IsVibration
    {
        get => PlayerPrefs.GetInt("VIBRATION", 1) == 1 ? true : false;
        set => PlayerPrefs.SetInt("VIBRATION", value == true ? 1 : 0);
    }
}

//North America
//1
//USA
//Level 1: Las Vegas
//Level 2: Atlantic City
//Level 3: New York
//Level 4: Miami
//Level 5: Los Angeles

//2
//Canada
//Level 1: Montreal
//Level 2: Toronto
//Level 3: Vancouver
//Level 4: Calgary
//Level 5: Ottawa

//3
//Europe
//UK
//Level 1: London
//Level 2: Birmingham
//Level 3: Manchester
//Level 4: Glasgow
//Level 5: Edinburgh

//4
//France
//Level 1: Paris
//Level 2: Nice
//Level 3: Lyon
//Level 4: Marseille
//Level 5: Bordeaux

//5
//Germany
//Level 1: Berlin
//Level 2: Munich
//Level 3: Hamburg
//Level 4: Frankfurt
//Level 5: Cologne

//6
//Asia
//China
//Level 1: Macau
//Level 2: Beijing
//Level 3: Shanghai
//Level 4: Guangzhou
//Level 5: Shenzhen


//7
//Australia & Oceania
//Australia
//Level 1: Melbourne
//Level 2: Sydney
//Level 3: Brisbane
//Level 4: Perth
//Level 5: Adelaide

//8
//Africa
//South Africa
//Level 1: Sun City
//Level 2: Johannesburg
//Level 3: Cape Town
//Level 4: Durban
//Level 5: Pretoria


//9
//New Zealand
//Level 1: Auckland
//Level 2: Wellington
//Level 3: Christchurch
//Level 4: Queenstown
//Level 5: Hamilton

//10
//Argentina
//Level 1: Buenos Aires
//Level 2: C?rdoba
//Level 3: Rosario
//Level 4: Mendoza
//Level 5: La Plata

//11
//India
//Level 1: Goa
//Level 2: Mumbai
//Level 3: New Delhi
//Level 4: Bangalore
//Level 5: Kolkata

//12
//Egypt
//Level 1: Cairo
//Level 2: Alexandria
//Level 3: Giza
//Level 4: Sharm El Sheikh
//Level 5: Luxor

//Singapore
//Level 1: Marina Bay Sands
//Level 2: Resorts World Sentosa
//Level 3: Clarke Quay
//Level 4: Orchard Road
//Level 5: Sentosa Island


//"Singapore","Egypt","India","Argentina","New Zealand","Africa","Australia","China","Germany","France","Europe","Canada","USA"

