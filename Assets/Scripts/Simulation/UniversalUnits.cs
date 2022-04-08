using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/SebLague/Solar-System/blob/Episode_01/Assets/Scripts/Planets/Universe.cs
public static class UniversalUnits
{
    public const float gravitationalConstant = 0.00000000066743f; // m^3*kg^-1*s^-2
    public const float simulationTimeStep = 50f; // s

    public const float physicsTimeStep = 0.01f;

    public const float floatLimitOffset = 1000000f;
}
