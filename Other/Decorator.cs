using GTA;
using GTA.Chrono;
using GTA.Math;
using System;
using static FusionLibrary.FusionEnums;

namespace FusionLibrary
{
    public class Decorator
    {
        private const string dDoNotDelete = "FusionProp_DoNotDelete";
        private const string dDrivenByPlayer = "FusionProp_DrivenByPlayer";
        private const string dRemoveFromUsed = "FusionProp_RemoveFromUsed";
        private const string dModelSwapped = "FusionProp_ModelSwapped";
        private const string dIgnoreForSwap = "FusionProp_IgnoreForSwap";

        private const string dInteractableEntity = "FusionProp_InteractableEntity";
        private const string dInteractableId = "FusionProp_InteractableId";

        private const string dGrip = "FusionProp_Grip";

        private const string dTorque = "FusionProp_Torque";

        internal static void Initialize()
        {
            Register(dDoNotDelete, DecorType.Bool);
            Register(dDrivenByPlayer, DecorType.Bool);
            Register(dRemoveFromUsed, DecorType.Bool);
            Register(dModelSwapped, DecorType.Bool);
            Register(dIgnoreForSwap, DecorType.Bool);
            Register(dInteractableEntity, DecorType.Bool);
            Register(dInteractableId, DecorType.Int);
            Register(dGrip, DecorType.Float);
            Register(dTorque, DecorType.Float);
            DecoratorInterface.IsLocked = true;
        }

        private readonly Entity Entity;

        public Decorator(Entity entity)
        {
            Entity = entity;
        }

        public bool DoNotDelete
        {
            get => Exists(dDoNotDelete) && GetBool(dDoNotDelete);

            set => SetBool(dDoNotDelete, value);
        }

        public bool DrivenByPlayer
        {
            get => Exists(dDrivenByPlayer) && GetBool(dDrivenByPlayer);

            set => SetBool(dDrivenByPlayer, value);
        }

        public bool RemoveFromUsed
        {
            get => Exists(dRemoveFromUsed) && GetBool(dRemoveFromUsed);

            set => SetBool(dRemoveFromUsed, value);
        }

        public bool ModelSwapped
        {
            get => Exists(dModelSwapped) && GetBool(dModelSwapped);

            set => SetBool(dModelSwapped, value);
        }

        public bool IgnoreForSwap
        {
            get => Exists(dIgnoreForSwap) && GetBool(dIgnoreForSwap);

            set => SetBool(dIgnoreForSwap, value);
        }

        public bool InteractableEntity
        {
            get => Exists(dInteractableEntity) && GetBool(dInteractableEntity);

            set => SetBool(dInteractableEntity, value);
        }

        public int InteractableId
        {
            get => GetInt(dInteractableId);

            set => SetInt(dInteractableId, value);
        }

        public int Grip
        {
            get => GetInt(dGrip);

            set => SetInt(dGrip, value);
        }

        public float Torque
        {
            get => GetFloat(dTorque);

            set => SetFloat(dTorque, value);
        }

        public bool Exists(string propertyName)
        {
            return DecoratorInterface.ExistsOn(Entity, propertyName);
        }

        public bool Remove(string propertyName)
        {
            if (DecoratorInterface.IsLocked)
            {
                DecoratorInterface.IsLocked = false;
            }

            return DecoratorInterface.Remove(Entity, propertyName);
        }

        public bool SetTime(string propertyName, int timeStamp)
        {
            return DecoratorInterface.SetTime(Entity, propertyName, timeStamp);
        }

        public bool SetInt(string propertyName, int value)
        {
            return DecoratorInterface.SetInt(Entity, propertyName, value);
        }

        public int GetInt(string propertyName)
        {
            return DecoratorInterface.GetInt(Entity, propertyName);
        }

        public bool SetFloat(string propertyName, float value)
        {
            return DecoratorInterface.SetFloat(Entity, propertyName, value);
        }

        public float GetFloat(string propertyName)
        {
            return DecoratorInterface.GetFloat(Entity, propertyName);
        }

        public bool SetBool(string propertyName, bool value)
        {
            return DecoratorInterface.SetBool(Entity, propertyName, value);
        }

        public bool GetBool(string propertyName)
        {
            return DecoratorInterface.GetBool(Entity, propertyName);
        }

        public bool SetVector3(string propertyName, Vector3 value)
        {
            bool ret = false;

            for (int i = 0; i < 3; i++)
            {
                ret = SetFloat(propertyName + i.ToString(), value[i]);

                if (!ret)
                    break;
            }

            return ret;
        }

        public Vector3 GetVector3(string propertyName)
        {
            Vector3 ret = new Vector3();

            for (int i = 0; i < 3; i++)
                ret[i] = GetFloat(propertyName + i.ToString());

            return ret;
        }

        public unsafe bool SetDateTime(string propertyName, GameClockDateTime value)
        {
            bool ret = false;

            long ticks = (value - GameClockDateTime.MinValue).WholeSeconds;

            ulong ticksBytes = *(ulong*)&ticks;
            uint ticksLow = (uint)ticksBytes & 0xFFFFFFFF;
            uint ticksHigh = (uint)(ticksBytes >> 32) & 0xFFFFFFFF;

            ret = SetInt(propertyName + "_LOW", (int)ticksLow);
            if (!ret)
                return ret;

            ret = SetInt(propertyName + "_HIGH", (int)ticksHigh);

            return ret;
        }

        public unsafe GameClockDateTime GetDateTime(string propertyName)
        {
            int ticksLowSigned = GetInt(propertyName + "_LOW");
            int ticksHighSigned = GetInt(propertyName + "_HIGH");

            uint ticksLow = *(uint*)&ticksLowSigned;
            uint ticksHigh = *(uint*)&ticksHighSigned;

            ulong ticksBytes = ticksLow | ((ulong)ticksHigh) << 32;

            long ticks = *(long*)&ticksBytes;

            GameClockDateTime.MinValue.TryAdd(GameClockDuration.FromSeconds(ticks), out GameClockDateTime output);

            return output;
        }

        public static bool IsRegistered(string propertyName, DecorType decorType)
        {
            if (decorType == DecorType.Vector3)
                return DecoratorInterface.IsRegisteredAsType(propertyName, DecoratorType.Float);
            else if (decorType == DecorType.DateTime)
                return DecoratorInterface.IsRegisteredAsType(propertyName, DecoratorType.Int);

            return DecoratorInterface.IsRegisteredAsType(propertyName, (DecoratorType)decorType);
        }

        public static bool Register(string propertyName, DecorType decorType)
        {
            if (IsRegistered(propertyName, decorType))
            {
                return true;
            }

            if (DecoratorInterface.IsLocked)
            {
                DecoratorInterface.IsLocked = false;
            }

            if (decorType == DecorType.Vector3)
            {
                for (int i = 0; i < 3; i++)
                    Register(propertyName + i.ToString(), DecorType.Float);

                Register(propertyName, DecorType.Float);
            }
            else if (decorType == DecorType.DateTime)
            {
                Register(propertyName + "_LOW", DecorType.Int);
                Register(propertyName + "_HIGH", DecorType.Int);
                Register(propertyName, DecorType.Int);
            }
            else
            {
                DecoratorInterface.Register(propertyName, (DecoratorType)decorType);
            }

            return IsRegistered(propertyName, decorType);
        }
    }
}
