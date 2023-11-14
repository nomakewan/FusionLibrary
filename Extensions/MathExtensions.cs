﻿using GTA;
using GTA.Chrono;
using GTA.Math;
using System;
using System.Collections.Generic;
using static FusionLibrary.FusionEnums;

namespace FusionLibrary.Extensions
{
    public static class MathExtensions
    {
        /// <summary>
        /// Returns a random <see cref="double"/> between <paramref name="minValue"/> and <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="random">Instance of a <see cref="System.Random"/>.</param>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <returns>Random value.</returns>
        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }

        /// <summary>
        /// Returns a random <see cref="int"/> between <paramref name="minValue"/> and <paramref name="maxValue"/>. Ignoring <paramref name="except"/> value.
        /// </summary>
        /// <param name="random">Instance of a <see cref="System.Random"/>.</param>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <param name="except">Value to be ignored.</param>
        /// <returns>Random value.</returns>
        public static int NextExcept(this Random random, int minValue, int maxValue, int except)
        {
            int ret;

            do
            {
                ret = random.Next(minValue, maxValue);
            } while (ret == except);

            return ret;
        }

        /// <summary>
        /// Returns a random <see cref="int"/> between <paramref name="minValue"/> and <paramref name="maxValue"/>. Ignoring <paramref name="except"/> values.
        /// </summary>
        /// <param name="random">Instance of a <see cref="System.Random"/>.</param>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <param name="except">Values to be ignored.</param>
        /// <returns>Random value.</returns>
        public static int NextExcept(this Random random, int minValue, int maxValue, List<int> except)
        {
            int ret;

            do
            {
                ret = random.Next(minValue, maxValue);
            } while (except.Contains(ret));

            return ret;
        }

        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
            {
                return min;
            }
            else if (value.CompareTo(max) > 0)
            {
                return max;
            }
            else
            {
                return value;
            }
        }

        public static float Lerp(this float firstFloat, float secondFloat, float by)
        {
            return FusionUtils.Lerp(firstFloat, secondFloat, by);
        }

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        /// <summary>
        /// Converts <paramref name="value"/> from m/s (game's unit) to mph.
        /// </summary>
        /// <param name="value">Speed value in m/s.</param>
        /// <returns>Speed in mph.</returns>
        public static float ToMPH(this float value)
        {
            return value * 2.23694f;
        }

        /// <summary>
        /// Converts <paramref name="value"/> from mph to m/s (game's unit).
        /// </summary>
        /// <param name="value">Speed value in mph.</param>
        /// <returns>Speed in m/s</returns>
        public static float ToMS(this float value)
        {
            return value * 0.44704f;
        }

        /// <summary>
        /// Converts degrees in radiants.
        /// </summary>
        /// <param name="value">Angle in degrees.</param>
        /// <returns>Radiants.</returns>
        public static float ToDeg(this float value)
        {
            return value * (180 / (float)Math.PI);
        }

        /// <summary>
        /// Converts radiants in degrees.
        /// </summary>
        /// <param name="value">Angle in radiants.</param>
        /// <returns>Degrees.</returns>
        public static float ToRad(this float value)
        {
            return value * ((float)Math.PI / 180);
        }

        /// <summary>
        /// Sets the Z coordinate of <paramref name="position"/> to the ground height.
        /// </summary>
        /// <param name="position">Original position.</param>
        /// <returns>Position with Z set to ground height.</returns>
        public static Vector3 SetToGroundHeight(this Vector3 position)
        {
            World.GetGroundHeight(position, out position.Z);

            return position;
        }

        /// <summary>
        /// Transfers Z offset from <paramref name="src"/> to <paramref name="dst"/>.
        /// </summary>
        /// <param name="src">Original point.</param>
        /// <param name="dst">Destination point.</param>
        /// <returns><paramref name="dst"/> with Z axis offsetted of <paramref name="src"/> Z.</returns>
        public static Vector3 TransferHeight(this Vector3 src, Vector3 dst)
        {
            World.GetGroundHeight(src, out float temp);
            dst.Z += src.Z - temp;

            return dst;
        }

        /// <summary>
        /// Returns the direction <see cref="Vector3"/> from <paramref name="src"/> to <paramref name="dst"/>.
        /// </summary>
        /// <param name="src">Start point.</param>
        /// <param name="dst">End point.</param>
        /// <returns>Direction from start to end point.</returns>
        public static Vector3 GetDirectionTo(this Vector3 src, Vector3 dst)
        {
            Vector3 ret = Vector3.Subtract(dst, src);
            ret.Normalize();

            return ret;
        }

        /// <summary>
        /// Gets the corresponding positive angle of a positive one.
        /// </summary>
        /// <param name="value">Negative angle.</param>
        /// <returns>Positive angle.</returns>
        public static float PositiveAngle(this float value)
        {
            if (value < 0)
            {
                value = 360 - Math.Abs(value);
            }

            return value;
        }

        /// <summary>
        /// Wraps an angle between 0 and 360.
        /// </summary>
        /// <param name="value">Angle in degrees.</param>
        /// <returns>Wrapped angle.</returns>
        public static float WrapAngle(this float value)
        {
            value %= 360;

            if (value < 0)
            {
                value += 360;
            }

            return value;
        }

        /// <summary>
        /// Converts a linear speed to angular applied to a wheel.
        /// </summary>
        /// <param name="wheelSpeed">Linear speed of the wheel.</param>
        /// <param name="wheelLength">Circumference of the wheel.</param>
        /// <param name="currentAngle">Current angle of the wheel.</param>
        /// <returns>Angular speed of the wheel.</returns>
        public static float AngularSpeed(this float wheelSpeed, float wheelLength, float currentAngle)
        {
            float newAngle = ((wheelSpeed / wheelLength) * 360) / Game.FPS;

            return (currentAngle - newAngle).WrapAngle();
        }

        /// <summary>
        /// Gets the arccosine of <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle">Angle in degrees.</param>
        /// <returns>Arccosine of <paramref name="angle"/>.</returns>
        public static double ArcCos(this double angle)
        {
            return Math.Atan(-angle / Math.Sqrt(-angle * angle + 1)) + 2 * Math.Atan(1);
        }

        /// <summary>
        /// Checks if two values are near enough.
        /// </summary>
        /// <param name="src">First value.</param>
        /// <param name="to">Second value.</param>
        /// <param name="by">Max difference accepted.</param>
        /// <returns><see langword="true"/> if the two values are near; otherwise <see langword="false"/>.</returns>
        public static bool Near(this float src, float to, float by = 5)
        {
            return (to - by) <= src && src <= (to + by);
        }

        /// <summary>
        /// Checks if two values are near enough.
        /// </summary>
        /// <param name="src">First value.</param>
        /// <param name="to">Second value.</param>
        /// <param name="by">Max difference accepted.</param>
        /// <param name="onlyFromLeft">Checks only if <paramref name="src"/> is minor or equal than <paramref name="to"/>.</param>
        /// <returns><see langword="true"/> if the two values are near; otherwise <see langword="false"/>.</returns>
        public static bool Near(this DateTime src, DateTime to, TimeSpan by, bool onlyFromLeft = false)
        {
            return to.Subtract(by) <= src && ((onlyFromLeft && src <= to) || (!onlyFromLeft && src <= to.Add(by)));
        }

        /// <summary>
        /// Checks if a <see cref="GameClockDateTime"/> is between <paramref name="start"/> and <paramref name="end"/> values.
        /// </summary>
        /// <param name="src">Evaluated <see cref="GameClockDateTime"/>.</param>
        /// <param name="start">Start of range.</param>
        /// <param name="end">End of range.</param>
        /// <returns><see langword="true"/> if <paramref name="src"/> is between; otherwise <see langword="false"/>.</returns>
        public static bool Between(this GameClockDateTime src, GameClockDateTime start, GameClockDateTime end)
        {
            return src >= start && src <= end;
        }

        /// <summary>
        /// Checks if a <see cref="GameClockDateTime"/> is between <paramref name="start"/> and <paramref name="end"/> values. It evaluates only the time expressed in 12 hour format.
        /// </summary>
        /// <param name="src">Evaluated <see cref="GameClockDateTime"/>.</param>
        /// <param name="start">Start of range.</param>
        /// <param name="end">End of range.</param>
        /// <returns><see langword="true"/> if <paramref name="src"/> is between; otherwise <see langword="false"/>.</returns>
        public static bool BetweenHours(this GameClockDateTime src, GameClockDateTime start, GameClockDateTime end)
        {
            int hour = src.Hour12.hour;
            int hourStart = start.Hour12.hour;
            int hourEnd = end.Hour12.hour;

            return hour >= hourStart && hour <= hourEnd && src.Minute >= start.Minute && src.Minute <= end.Minute && src.Second >= start.Second && src.Second <= end.Second;
        }

        /// <summary>
        /// Converts a <see cref="Vector3"/> direction to a <see cref="Vector3"/> rotation with <paramref name="roll"/>.
        /// </summary>
        /// <param name="dir">Direction vector.</param>
        /// <param name="roll">Roll.</param>
        /// <returns>Rotation vector.</returns>
        public static Vector3 DirectionToRotation(this Vector3 dir, float roll)
        {
            dir.Normalize();

            Vector3 vector3_1 = Vector3.Zero;

            vector3_1.Z = -(ToDeg((float)Math.Atan2(dir.X, dir.Y)));

            Vector3 vector3_2 = Vector3.Normalize(new Vector3(dir.Z, new Vector3(dir.X, dir.Y, 0).Length(), 0));

            vector3_1.X = -(ToDeg((float)Math.Atan2(vector3_2.X, vector3_2.Y)));

            vector3_1.Y = roll;

            return vector3_1;
        }

        /// <summary>
        /// Returns <paramref name="vector3"/> offsetted by <paramref name="value"/> in <paramref name="coordinate"/> axis.
        /// </summary>
        /// <param name="vector3">Original <see cref="Vector3"/>.</param>
        /// <param name="coordinate"><see cref="Coordinate"/> of the axis.</param>
        /// <param name="value">Offset.</param>
        /// <returns></returns>
        public static Vector3 GetSingleOffset(this Vector3 vector3, Coordinate coordinate, float value)
        {
            switch (coordinate)
            {
                case Coordinate.X:
                    vector3.X += value;
                    break;
                case Coordinate.Y:
                    vector3.Y += value;
                    break;
                default:
                    vector3.Z += value;
                    break;
            }

            return vector3;
        }

        /// <summary>
        /// Inverts the sign of <paramref name="coordinate"/> of <paramref name="vector3"/>.
        /// </summary>
        /// <param name="vector3">Original <see cref="Vector3"/>.</param>
        /// <param name="coordinate"><see cref="Coordinate"/> of the axis.</param>
        /// <returns>Original <paramref name="vector3"/> with the inverted axis.</returns>
        public static Vector3 InvertCoordinate(this Vector3 vector3, Coordinate coordinate)
        {
            switch (coordinate)
            {
                case Coordinate.X:
                    vector3.X = -vector3.X;
                    break;
                case Coordinate.Y:
                    vector3.Y = -vector3.Y;
                    break;
                default:
                    vector3.Z = -vector3.Z;
                    break;
            }

            return vector3;
        }
    }
}
