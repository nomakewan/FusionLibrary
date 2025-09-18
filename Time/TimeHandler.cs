using FusionLibrary.Extensions;
using GTA;
using GTA.Chrono;
using GTA.Native;
using System;
using System.Collections.Generic;
using static FusionLibrary.FusionEnums;

namespace FusionLibrary
{
    public delegate void OnTimeChanged(GameClockDateTime time);
    public delegate void OnDayNightChange();

    public class TimeHandler : Script
    {
        public static List<Vehicle> UsedVehiclesByPlayer { get; } = new List<Vehicle>();
        private static readonly List<Vehicle> RemoveUsedVehicle = new List<Vehicle>();

        public static OnTimeChanged OnTimeChanged;
        public static OnDayNightChange OnDayNightChange;

        public static bool IsNight { get; internal set; }

        public static bool TrafficVolumeYearBased { get; set; }

        public static bool MissionTraffic = false;

        public static bool RealTime
        {
            get => realTime;

            set
            {
                if (realTime == value)
                    return;

                realTime = value;
                realSecond = Game.GameTime + 1000;
                GameClock.IsPaused = value;
            }
        }
        private static bool realTime;
        private static int realSecond;

        public TimeHandler()
        {
            Tick += TimeHandler_Tick;
        }

        private void TimeHandler_Tick(object sender, EventArgs e)
        {
            if (realTime)
            {
                GameClock.IsPaused = true;

                if (Game.GameTime >= realSecond)
                {
                    GameClock.AddToCurrentTime(0, 0, 1);
                    realSecond = Game.GameTime + 1000;
                }
            }

            UsedVehiclesByPlayer.ForEach(x =>
            {
                if (!x.IsFunctioning() || x.Decorator().RemoveFromUsed)
                {
                    RemoveUsedVehicle.Add(x);
                }
            });

            if (RemoveUsedVehicle.Count > 0)
            {
                RemoveUsedVehicle.ForEach(x => UsedVehiclesByPlayer.Remove(x));
                RemoveUsedVehicle.Clear();
            }

            if (FusionUtils.PlayerVehicle.IsFunctioning() && !FusionUtils.PlayerVehicle.IsTrain && !FusionUtils.PlayerVehicle.Decorator().DrivenByPlayer && !FusionUtils.PlayerVehicle.Decorator().RemoveFromUsed && !UsedVehiclesByPlayer.Contains(FusionUtils.PlayerVehicle))
            {
                UsedVehiclesByPlayer.Add(FusionUtils.PlayerVehicle);
                FusionUtils.PlayerVehicle.Decorator().DrivenByPlayer = true;
            }

            bool isNight = GameClock.Now.Hour >= 20 || (GameClock.Now.Hour >= 0 && GameClock.Now.Hour <= 5);

            if (isNight != IsNight)
            {
                IsNight = isNight;
                OnDayNightChange?.Invoke();
            }

            if (!TrafficVolumeYearBased || MissionTraffic)
            {
                return;
            }

            float vehDensity = 1;

            float year = GameClock.Now.Year;

            if (year > 1900 && year < 1950)
            {
                year -= 1900;

                if (!FusionUtils.IsTrafficAlive)
                    FusionUtils.IsTrafficAlive = true;

                vehDensity = year / 50f;
            }
            else if (year <= 1900)
            {
                vehDensity = 0;

                if (FusionUtils.IsTrafficAlive)
                    FusionUtils.IsTrafficAlive = false;
            }

            if (vehDensity >= 1)
            {
                if (!FusionUtils.IsTrafficAlive)
                    FusionUtils.IsTrafficAlive = true;

                return;
            }

            World.SetAmbientVehicleDensityMultiplierThisFrame(vehDensity);
        }

        public static void SetTimer(ScriptTimer scriptTimer, int value)
        {
            Function.Call(scriptTimer == ScriptTimer.A ? Hash.SETTIMERA : Hash.SETTIMERB, value);
        }

        public static int GetTimer(ScriptTimer scriptTimer)
        {
            return Function.Call<int>(scriptTimer == ScriptTimer.A ? Hash.TIMERA : Hash.TIMERB);
        }

        public static void TimeTravelTo(GameClockDateTime destinationTime)
        {
            new MomentReplica();

            FusionUtils.ClearWorld();

            UsedVehiclesByPlayer.Clear();

            GameClock.Now = destinationTime;

            MomentReplica.MomentReplicas?.ForEach(x =>
                {
                    x.Applied = false;
                });

            MomentReplica momentReplica = MomentReplica.SearchForMoment();

            if (momentReplica == null)
            {
                MomentReplica.Randomize();
            }
            else
            {
                momentReplica.Apply();
            }

            OnTimeChanged?.Invoke(destinationTime);
        }
    }
}
