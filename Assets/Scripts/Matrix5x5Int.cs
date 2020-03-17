﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
[StructLayout(LayoutKind.Sequential)]
public class Matrix5x5Int : IEquatable<Matrix5x5Int>
{
    public static int ROW_LENGTH = 5;
    public static int COL_LENGTH = 5;
    public static int LENGTH = ROW_LENGTH * COL_LENGTH;

    // memory layout:
    //
    //                row no (=vertical)
    //               |  0   1   2   3   4
    //            ---+------------
    //            0  | m00 m10 m20 m30 m40
    // column no  1  | m01 m11 m21 m31 m41
    // (=horiz)   2  | m02 m12 m22 m32 m42
    //            3  | m03 m13 m23 m33 m43
    //            4  | m04 m14 m24 m34 m44
    public int m00, m10, m20, m30, m40,
               m01, m11, m21, m31, m41,
               m02, m12, m22, m32, m42,
               m03, m13, m23, m33, m43,
               m04, m14, m24, m34, m44;

    public int difficultyLevel;

    // used to allow Matrix5x5Ints to be used as keys in hash tables
    public override int GetHashCode()
    {
        int c0 = m00.GetHashCode() ^ (m01.GetHashCode() << 2) ^ (m02.GetHashCode() >> 2);
        int c1 = m10.GetHashCode() ^ (m11.GetHashCode() << 2) ^ (m12.GetHashCode() >> 2);
        int c2 = m20.GetHashCode() ^ (m21.GetHashCode() << 2) ^ (m22.GetHashCode() >> 2);
        int c3 = m30.GetHashCode() ^ (m31.GetHashCode() << 2) ^ (m32.GetHashCode() >> 2);
        int c4 = m40.GetHashCode() ^ (m41.GetHashCode() << 2) ^ (m42.GetHashCode() >> 2);
        return c0 ^ (c1 << 2) ^ (c2 >> 2) ^ (c3 >> 1) ^ (c4 >> 0);
    }

    // also required for being able to use Matrix5x5Ints as keys in hash tables
    public override bool Equals(object other)
    {
        if (!(other is Matrix5x5Int)) return false;

        return Equals((Matrix5x5Int)other);
    }

    public bool Equals(Matrix5x5Int other)
    {
        bool c0 = m00 == other.m00 && m01 == other.m01 && m02 == other.m02 && m03 == other.m03 && m04 == other.m04;
        bool c1 = m10 == other.m10 && m11 == other.m11 && m12 == other.m12 && m13 == other.m13 && m14 == other.m14;
        bool c2 = m20 == other.m20 && m21 == other.m21 && m22 == other.m22 && m23 == other.m23 && m24 == other.m24;
        bool c3 = m30 == other.m30 && m31 == other.m31 && m32 == other.m32 && m33 == other.m33 && m34 == other.m34;
        bool c4 = m40 == other.m40 && m41 == other.m41 && m42 == other.m42 && m43 == other.m43 && m44 == other.m44;
        return c0 && c1 && c2 && c3 && c4;
    }

    public static bool operator ==(Matrix5x5Int lhs, Matrix5x5Int rhs)
    {
        bool c0 = lhs.m00 == rhs.m00 && lhs.m01 == rhs.m01 && lhs.m02 == rhs.m02 && lhs.m03 == rhs.m03 && lhs.m04 == rhs.m04;
        bool c1 = lhs.m10 == rhs.m10 && lhs.m11 == rhs.m11 && lhs.m12 == rhs.m12 && lhs.m13 == rhs.m13 && lhs.m14 == rhs.m14;
        bool c2 = lhs.m20 == rhs.m20 && lhs.m21 == rhs.m21 && lhs.m22 == rhs.m22 && lhs.m23 == rhs.m23 && lhs.m24 == rhs.m24;
        bool c3 = lhs.m30 == rhs.m30 && lhs.m31 == rhs.m31 && lhs.m32 == rhs.m32 && lhs.m33 == rhs.m33 && lhs.m34 == rhs.m34;
        bool c4 = lhs.m40 == rhs.m40 && lhs.m41 == rhs.m41 && lhs.m42 == rhs.m42 && lhs.m43 == rhs.m43 && lhs.m44 == rhs.m44;
        // Returns false in the presence of NaN values.
        return c0 && c1 && c2 && c3 && c4;
    }

    public static bool operator !=(Matrix5x5Int lhs, Matrix5x5Int rhs)
    {
        // Returns true in the presence of NaN values.
        return !(lhs == rhs);
    }

    // Access element at [row, column].
    public int this[int row, int column]
    {
        get
        {
            return this[row + column * 5];
        }

        set
        {
            this[row + column * 5] = value;
        }
    }

    // Access element at sequential index (0..24 inclusive).
    public int this[int index]
    {
        get
        {
            switch (index)
            {
                case 00: return m00;
                case 01: return m10;
                case 02: return m20;
                case 03: return m30;
                case 04: return m40;

                case 05: return m01;
                case 06: return m11;
                case 07: return m21;
                case 08: return m31;
                case 09: return m41;

                case 10: return m02;
                case 11: return m12;
                case 12: return m22;
                case 13: return m32;
                case 14: return m42;

                case 15: return m03;
                case 16: return m13;
                case 17: return m23;
                case 18: return m33;
                case 19: return m43;

                case 20: return m04;
                case 21: return m14;
                case 22: return m24;
                case 23: return m34;
                case 24: return m44;

                default:
                    throw new IndexOutOfRangeException("Invalid matrix index!");
            }
        }

        set
        {
            switch (index)
            {
                case 00: m00 = value; break;
                case 01: m10 = value; break;
                case 02: m20 = value; break;
                case 03: m30 = value; break;
                case 04: m40 = value; break;

                case 05: m01 = value; break;
                case 06: m11 = value; break;
                case 07: m21 = value; break;
                case 08: m31 = value; break;
                case 09: m41 = value; break;

                case 10: m02 = value; break;
                case 11: m12 = value; break;
                case 12: m22 = value; break;
                case 13: m32 = value; break;
                case 14: m42 = value; break;

                case 15: m03 = value; break;
                case 16: m13 = value; break;
                case 17: m23 = value; break;
                case 18: m33 = value; break;
                case 19: m43 = value; break;

                case 20: m04 = value; break;
                case 21: m14 = value; break;
                case 22: m24 = value; break;
                case 23: m34 = value; break;
                case 24: m44 = value; break;

                default:
                    throw new IndexOutOfRangeException("Invalid matrix index!");
            }
        }
    }

    // Access element at [row, column].
    public int FromLocation(int x, int z)
    {
        if (x == -2 && z ==  2)
            return this[00];
        if (x == -1 && z ==  2)
            return this[01];
        if (x ==  0 && z ==  2)
            return this[02];
        if (x ==  1 && z ==  2)
            return this[03];
        if (x ==  2 && z ==  2)
            return this[04];

        if (x == -2 && z ==  1)
            return this[05];
        if (x == -1 && z ==  1)
            return this[06];
        if (x ==  0 && z ==  1)
            return this[07];
        if (x ==  1 && z ==  1)
            return this[08];
        if (x ==  2 && z ==  1)
            return this[09];

        if (x == -2 && z ==  0)
            return this[10];
        if (x == -1 && z ==  0)
            return this[11];
        if (x ==  0 && z ==  0)
            return this[12];
        if (x ==  1 && z ==  0)
            return this[13];
        if (x ==  2 && z ==  0)
            return this[14];

        if (x == -2 && z == -1)
            return this[15];
        if (x == -1 && z == -1)
            return this[16];
        if (x ==  0 && z == -1)
            return this[17];
        if (x ==  1 && z == -1)
            return this[18];
        if (x ==  2 && z == -1)
            return this[19];

        if (x == -2 && z == -2)
            return this[20];
        if (x == -1 && z == -2)
            return this[21];
        if (x ==  0 && z == -2)
            return this[22];
        if (x ==  1 && z == -2)
            return this[23];
        if (x ==  2 && z == -2)
            return this[24];

        throw new IndexOutOfRangeException("Invalid location!");
    }

    public Vector2Int GetLocation(int index)
    {
        switch (index)
        {
            case 00: return new Vector2Int(-2,  2); // m00
            case 01: return new Vector2Int(-1,  2); // m10
            case 02: return new Vector2Int( 0,  2); // m20
            case 03: return new Vector2Int( 1,  2); // m30
            case 04: return new Vector2Int( 2,  2); // m40

            case 05: return new Vector2Int(-2,  1); // m01
            case 06: return new Vector2Int(-1,  1); // m11
            case 07: return new Vector2Int( 0,  1); // m21
            case 08: return new Vector2Int( 1,  1); // m31
            case 09: return new Vector2Int( 2,  1); // m41

            case 10: return new Vector2Int(-2,  0); // m02
            case 11: return new Vector2Int(-1,  0); // m12
            case 12: return new Vector2Int( 0,  0); // m22
            case 13: return new Vector2Int( 1,  0); // m32
            case 14: return new Vector2Int( 2,  0); // m42

            case 15: return new Vector2Int(-2, -1); // m03
            case 16: return new Vector2Int(-1, -1); // m13
            case 17: return new Vector2Int( 0, -1); // m23
            case 18: return new Vector2Int( 1, -1); // m33
            case 19: return new Vector2Int( 2, -1); // m43

            case 20: return new Vector2Int(-2, -2); // m04
            case 21: return new Vector2Int(-1, -2); // m14
            case 22: return new Vector2Int( 0, -2); // m24
            case 23: return new Vector2Int( 1, -2); // m34
            case 24: return new Vector2Int( 2, -2); // m44

            default:
                throw new IndexOutOfRangeException("Invalid matrix index!");
        }
    }

    public override string ToString()
    {
        string r0 = m00 + ", " + m10 + ", " + m20 + ", " + m30 + ", " + m40;
        string r1 = m01 + ", " + m11 + ", " + m21 + ", " + m31 + ", " + m41;
        string r2 = m02 + ", " + m12 + ", " + m22 + ", " + m32 + ", " + m42;
        string r3 = m03 + ", " + m13 + ", " + m23 + ", " + m33 + ", " + m43;
        string r4 = m04 + ", " + m14 + ", " + m24 + ", " + m34 + ", " + m44;
        return r0 + "\n" + r1 + "\n" + r2 + "\n" + r3 + "\n" + r4;
    }
}