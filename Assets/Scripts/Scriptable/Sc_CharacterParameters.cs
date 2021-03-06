﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="NewCharacter", menuName ="CreateNewCharacterParameters")]
public class Sc_CharacterParameters : ScriptableObject
{
   [Header("UI")]
    public string characterName;
    public Texture2D characterIcon;

    [Header("Character Parameters")]
    public int movementGiven;
    public CharacterSkills characterSkills;
}

[System.Serializable]
[System.Flags]
public enum CharacterSkills
{
    Swift = 1 << 0,
    JumpOff = 1 << 1,
    FinisherMove = 1 << 2,
    Absorber = 1 << 3,
    RandomizerFirstMove = 1 << 4,
    Foresight = 1 << 5
}
