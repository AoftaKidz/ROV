using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteNumberManager : MonoBehaviour
{

    public static string ToRed(string number)
    {
        string newNumber = "";

        foreach(char s in number)
        {
            var f = SpriteNumberManager.GetRedSprite(s);
            newNumber += f;
        }
        return newNumber;
    }
    public static string ToYellow(string number)
    {
        string newNumber = "";

        foreach (char s in number)
        {
            var f = GetYellowSprite(s);
            newNumber += f;
        }
        return newNumber;
    }
    public static string ToWhite(string number)
    {
        string newNumber = "";

        foreach (char s in number)
        {
            var f = GetWhiteSprite(s);
            newNumber += f;
        }
        return newNumber;
    }
    public static string ToMeowWhite(string number)
    {
        string newNumber = "";

        foreach (char s in number)
        {
            var f = GetMeowWhiteSprite(s);
            newNumber += f;
        }
        return newNumber;
    }
    public static string GetRedSprite(char c)
    {
        switch (c)
        {
            case '0':
                {
                    return "<sprite index=0>";
                }
            case '1':
                {
                    return "<sprite index=1>";
                }
            case '2':
                {
                    return "<sprite index=2>";
                }
            case '3':
                {
                    return "<sprite index=3>";
                }
            case '4':
                {
                    return "<sprite index=4>";
                }
            case '5':
                {
                    return "<sprite index=5>";
                }
            case '6':
                {
                    return "<sprite index=6>";
                }
            case '7':
                {
                    return "<sprite index=7>";
                }
            case '8':
                {
                    return "<sprite index=8>";
                }
            case '9':
                {
                    return "<sprite index=9>";
                }
            case ',':
                {
                    return "<sprite index=10>";
                }
            case '-':
                {
                    return "<sprite index=11>";
                }
            case '.':
                {
                    return "<sprite index=12>";
                }
        }
        return "";
    }
    public static string GetYellowSprite(char c)
    {
        switch (c)
        {
            case '0':
                {
                    return "<sprite index=13>";
                }
            case '1':
                {
                    return "<sprite index=14>";
                }
            case '2':
                {
                    return "<sprite index=15>";
                }
            case '3':
                {
                    return "<sprite index=16>";
                }
            case '4':
                {
                    return "<sprite index=17>";
                }
            case '5':
                {
                    return "<sprite index=18>";
                }
            case '6':
                {
                    return "<sprite index=19>";
                }
            case '7':
                {
                    return "<sprite index=20>";
                }
            case '8':
                {
                    return "<sprite index=21>";
                }
            case '9':
                {
                    return "<sprite index=22>";
                }
            case ',':
                {
                    return "<sprite index=23>";
                }
            case '.':
                {
                    return "<sprite index=24>";
                }
        }
        return "";
    }
    public static string GetWhiteSprite(char c)
    {
        switch (c)
        {
            case '0':
                {
                    return "<sprite index=25>";
                }
            case '1':
                {
                    return "<sprite index=26>";
                }
            case '2':
                {
                    return "<sprite index=27>";
                }
            case '3':
                {
                    return "<sprite index=28>";
                }
            case '4':
                {
                    return "<sprite index=29>";
                }
            case '5':
                {
                    return "<sprite index=30>";
                }
            case '6':
                {
                    return "<sprite index=31>";
                }
            case '7':
                {
                    return "<sprite index=32>";
                }
            case '8':
                {
                    return "<sprite index=33>";
                }
            case '9':
                {
                    return "<sprite index=34>";
                }
            case '.':
                {
                    return "<sprite index=35>";
                }
        }
        return "";
    }
    public static string GetMeowWhiteSprite(char c)
    {
        switch (c)
        {
            case '0':
                {
                    return "<sprite index=0>";
                }
            case '1':
                {
                    return "<sprite index=1>";
                }
            case '2':
                {
                    return "<sprite index=2>";
                }
            case '3':
                {
                    return "<sprite index=3>";
                }
            case '4':
                {
                    return "<sprite index=4>";
                }
            case '5':
                {
                    return "<sprite index=5>";
                }
            case '6':
                {
                    return "<sprite index=6>";
                }
            case '7':
                {
                    return "<sprite index=7>";
                }
            case '8':
                {
                    return "<sprite index=8>";
                }
            case '9':
                {
                    return "<sprite index=9>";
                }
            case ',':
                {
                    return "<sprite index=10>";
                }
            case '.':
                {
                    return "<sprite index=11>";
                }
        }
        return "";
    }
}
