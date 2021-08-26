﻿using FusionLibrary.Memory;
using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using static FusionLibrary.FusionEnums;

namespace FusionLibrary.Extensions
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Checks if given <paramref name="entity"/> is not <c>null</c> and exists in game's world.
        /// </summary>
        /// <param name="entity">Instance of an <see cref="Entity"/>.</param>
        /// <returns><c>true</c> if <paramref name="entity"/> is not <c>null</c> and exists; otherwise <c>false</c></returns>
        public static bool NotNullAndExists(this Entity entity)
        {
            return entity != null && entity.Exists();
        }

        /// <summary>
        /// Returns the <see cref="FusionLibrary.Decorator"/> object of the given <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">Instance of an <see cref="Entity"/>.</param>
        /// <returns>Instance of the <see cref="FusionLibrary.Decorator"/> of <paramref name="entity"/></returns>
        public static Decorator Decorator(this Entity entity)
        {
            return new Decorator(entity);
        }

        /// <summary>
        /// Returns the <see cref="Vector3"/>'s relative velocity of the given <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">Instance of an <see cref="Entity"/>.</param>
        /// <returns><see cref="Vector3"/>'s relative velocity of <paramref name="entity"/></returns>
        public static Vector3 RelativeVelocity(this Entity entity)
        {
            return Function.Call<Vector3>(Hash.GET_ENTITY_SPEED_VECTOR, entity, true);
        }

        /// <summary>
        /// Checks if <paramref name="entity"/> is going forward.
        /// </summary>
        /// <param name="entity">Instance of an <see cref="Entity"/>.</param>
        /// <returns><c>true</c> if <paramref name="entity"/> is going forward or is still; otherwise <c>false</c></returns>
        public static bool IsGoingForward(this Entity entity)
        {
            return entity.RelativeVelocity().Y >= 0;
        }

        /// <summary>
        /// Returns the 2D squared distance between <paramref name="src"/> and <paramref name="entity"/>.
        /// </summary>
        /// <param name="src">Instance of an <see cref="Entity"/>.</param>
        /// <param name="entity">Instance of an <see cref="Entity"/>.</param>
        /// <returns>Distance in <c>float</c> between the entities</returns>
        public static float DistanceToSquared2D(this Entity src, Entity entity)
        {
            return FusionUtils.DistanceToSquared2D(src, entity);
        }

        /// <summary>
        /// Returns the 2D squared distance between <paramref name="src"/> and <paramref name="boneName"/> of <paramref name="entity"/>.
        /// </summary>
        /// <param name="src">Instance of an <see cref="Entity"/>.</param>
        /// <param name="entity">Instance of an <see cref="Entity"/>.</param>
        /// <param name="boneName">Bone's name.</param>
        /// <returns>Distance in <c>float</c> between the entities</returns>
        public static float DistanceToSquared2D(this Entity src, Entity entity, string boneName)
        {
            return src.Position.DistanceToSquared2D(entity.Bones[boneName].Position);
        }

        /// <summary>
        /// Returns the 2D squared distance between <paramref name="src"/> and <paramref name="worldPosition"/>.
        /// </summary>
        /// <param name="src">Instance of an <see cref="Entity"/>.</param>
        /// <param name="worldPosition">Instance of a <see cref="Vector3"/>.</param>
        /// <returns>Distance in <c>float</c></returns>
        public static float DistanceToSquared2D(this Entity src, Vector3 worldPosition)
        {
            return src.Position.DistanceToSquared2D(worldPosition);
        }

        /// <summary>
        /// Checks if the distance between <paramref name="src"/> and <paramref name="entity"/> is less or equal than <paramref name="maxDistance"/>.
        /// </summary>
        /// <param name="src">Instance of an <see cref="Entity"/>.</param>
        /// <param name="entity">Instance of an <see cref="Entity"/>.</param>
        /// <param name="maxDistance">Max distance allowed.</param>
        /// <returns><c>true</c> if distance is less or equal than <paramref name="maxDistance"/>; otherwise <c>false</c></returns>
        public static bool DistanceToSquared2D(this Entity src, Entity entity, float maxDistance)
        {
            return DistanceToSquared2D(src, entity) <= maxDistance * maxDistance;
        }

        /// <summary>
        /// Checks if the distance between <paramref name="src"/> and <paramref name="boneName"/> of <paramref name="entity"/> is less or equal than <paramref name="maxDistance"/>.
        /// </summary>
        /// <param name="src">Instance of an <see cref="Entity"/>.</param>
        /// <param name="entity">Instance of an <see cref="Entity"/>.</param>
        /// <param name="boneName">Bone's name.</param>
        /// <param name="maxDistance">Max distance allowed.</param>
        /// <returns><c>true</c> if distance is less or equal than <paramref name="maxDistance"/>; otherwise <c>false</c></returns>
        public static bool DistanceToSquared2D(this Entity src, Entity entity, string boneName, float maxDistance)
        {
            return DistanceToSquared2D(src, entity, boneName) <= maxDistance * maxDistance;
        }

        /// <summary>
        /// Checks if the distance between <paramref name="src"/> and <paramref name="worldPosition"/> is less or equal than <paramref name="maxDistance"/>.
        /// </summary>
        /// <param name="src">Instance of an <see cref="Entity"/>.</param>
        /// <param name="worldPosition">Instance of a <see cref="Vector3"/>.</param>
        /// <param name="maxDistance">Max distance allowed.</param>
        /// <returns><c>true</c> if distance is less or equal than <paramref name="maxDistance"/>; otherwise <c>false</c></returns>
        public static bool DistanceToSquared2D(this Entity src, Vector3 worldPosition, float maxDistance)
        {
            return DistanceToSquared2D(src, worldPosition) <= maxDistance * maxDistance;
        }

        /// <summary>
        /// Checks if <paramref name="ped"/> is not <c>null</c>, <seealso cref="Ped.Exists()"/> and also is alive.
        /// </summary>
        /// <param name="ped">Instance of a <see cref="Ped"/>.</param>
        /// <returns><c>true</c> if <paramref name="ped"/> is not <c>null</c>, <seealso cref="Ped.Exists()"/> and also is alive; otherwise <c>false</c></returns>
        public static bool ExistsAndAlive(this Ped ped)
        {
            return ped.NotNullAndExists() && ped.IsAlive;
        }

        /// <summary>
        /// Checks if <paramref name="ped"/> has been damaged.
        /// </summary>
        /// <param name="ped">Instance of a <see cref="Ped"/>.</param>
        /// <returns><c>true</c> if <paramref name="ped"/> has been damaged; otherwise <c>false</c></returns>
        public static bool HasBeenDamaged(this Ped ped)
        {
            return Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_ANY_OBJECT, ped) || Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_ANY_PED, ped) || Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_ANY_VEHICLE, ped);
        }

        /// <summary>
        /// Returns the <see cref="AlphaLevel"/> of the given <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">Instance of an <see cref="Entity"/>.</param>
        /// <returns><see cref="AlphaLevel"/> of <paramref name="entity"/></returns>
        public static AlphaLevel GetAlpha(this Entity entity)
        {
            int value = Function.Call<int>(Hash.GET_ENTITY_ALPHA, entity);

            if (value < (int)AlphaLevel.L1)
                return AlphaLevel.L0;

            if (value < (int)AlphaLevel.L2)
                return AlphaLevel.L1;

            if (value < (int)AlphaLevel.L3)
                return AlphaLevel.L2;

            if (value < (int)AlphaLevel.L4)
                return AlphaLevel.L4;

            return AlphaLevel.L5;
        }

        /// <summary>
        /// Sets the <see cref="AlphaLevel"/> of the given <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">Instance of an <see cref="Entity"/>.</param>
        /// <param name="level">Desired <see cref="AlphaLevel"/>.</param>
        public static void SetAlpha(this Entity entity, AlphaLevel level)
        {
            Function.Call(Hash.SET_ENTITY_ALPHA, entity, (int)level);
        }

        /// <summary>
        /// Checks if <paramref name="taskType"/> is running for <paramref name="ped"/>.
        /// </summary>
        /// <param name="ped">Instance of a <see cref="Ped"/>.</param>
        /// <param name="taskType">A <see cref="TaskType"/>.</param>
        /// <returns></returns>
        public static bool IsTaskActive(this Ped ped, TaskType taskType)
        {
            return Function.Call<bool>(Hash.GET_IS_TASK_ACTIVE, ped, (int)taskType);
        }

        public static bool IsAnyTaskActive(this Ped ped)
        {
            foreach (TaskType taskType in Enum.GetValues(typeof(TaskType)).Cast<TaskType>().ToList())
                if (ped.IsTaskActive(taskType))
                    return true;

            return false;
        }

        public static List<TaskType> GetActiveTasks(this Ped ped)
        {
            List<TaskType> ret = new List<TaskType>();

            foreach (TaskType taskType in Enum.GetValues(typeof(TaskType)).Cast<TaskType>().ToList())
                if (ped.IsTaskActive(taskType))
                    ret.Add(taskType);

            return ret;
        }

        public static Vehicle GetClosestVehicle(this Ped ped, float radius = 10)
        {
            return World.GetClosestVehicle(ped.Position, radius);
        }

        public static Vehicle GetEnteringVehicle(this Ped ped)
        {
            return Function.Call<Vehicle>(Hash.GET_VEHICLE_PED_IS_ENTERING, ped);
        }

        public static Vehicle GetUsingVehicle(this Ped ped)
        {
            return Function.Call<Vehicle>(Hash.GET_VEHICLE_PED_IS_USING, ped);
        }

        public static Hash GetStreetHash(this Vehicle vehicle)
        {
            Hash street;
            Hash cross;

            unsafe
            {
                Function.Call(Hash.GET_STREET_NAME_AT_COORD, vehicle.Position.X, vehicle.Position.Y, vehicle.Position.Z, &street, &cross);
            }

            return street;
        }

        public static Hash GetCrossingHash(this Vehicle vehicle)
        {
            Hash street;
            Hash cross;

            unsafe
            {
                Function.Call(Hash.GET_STREET_NAME_AT_COORD, vehicle.Position.X, vehicle.Position.Y, vehicle.Position.Z, &street, &cross);
            }

            return cross;
        }

        public static TaskDrive TaskDrive(this Vehicle vehicle)
        {
            return new TaskDrive(vehicle);
        }

        public static TaskDrive TaskDrive(this Ped ped)
        {
            return new TaskDrive(ped);
        }

        public static TaskDrive TaskDrive(this Ped ped, Vehicle vehicle)
        {
            return new TaskDrive(ped, vehicle);
        }

        public static void TaskGoStraightTo(this Ped ped, Vector3 position, float speed, float heading, int timeout = -1, float distanceToSlide = 0)
        {
            Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, ped, position.X, position.Y, position.Z, speed, timeout, heading, distanceToSlide);
        }

        public static bool IsFullyInVehicle(this Ped ped)
        {
            return ped.IsSittingInVehicle() && !ped.IsTaskActive(TaskType.EnterVehicle) && !ped.IsTaskActive(TaskType.ExitVehicle);
        }

        public static bool IsFullyOutVehicle(this Ped ped)
        {
            return ped.CurrentVehicle == null && !ped.IsTaskActive(TaskType.ExitVehicle) && !ped.IsTaskActive(TaskType.EnterVehicle);
        }

        public static bool IsEnteringVehicle(this Ped ped)
        {
            return !ped.IsSittingInVehicle() && ped.IsTaskActive(TaskType.EnterVehicle) && !ped.IsTaskActive(TaskType.ExitVehicle);
        }

        public static bool IsLeavingVehicle(this Ped ped)
        {
            return !IsFullyOutVehicle(ped) && ped.IsTaskActive(TaskType.ExitVehicle);
        }

        //public static void AttachTo(this Entity entity1, Entity toEntity, string boneName, Vector3 offset, Vector3 rotation)
        //{
        //    Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, toEntity, toEntity.Bones[boneName].Index, offset.X, offset.Y, offset.Z, rotation.X, rotation.Y, rotation.Z, false, false, true, false, 2, true);
        //}

        //public static void AttachTo(this Entity entity1, Entity toEntity, Vector3 offset, Vector3 rotation)
        //{
        //    Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, toEntity, 0, offset.X, offset.Y, offset.Z, rotation.X, rotation.Y, rotation.Z, false, false, true, false, 2, true);
        //}

        public static void AttachToPhysically(this Entity entity1, Entity toEntity, Vector3 offset, Vector3 rotation)
        {
            Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY, entity1, toEntity, 0, 0, offset.X, offset.Y, offset.Z, 0, 0, 0, rotation.X, rotation.Y, rotation.Z, 1000000.0f, true, true, false, false, 2);
        }

        public static void AttachToPhysically(this Entity entity1, Entity toEntity, int boneIndex, Vector3 offset, Vector3 rotation)
        {
            Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY, entity1, toEntity, boneIndex, 0, offset.X, offset.Y, offset.Z, 0, 0, 0, rotation.X, rotation.Y, rotation.Z, 1000000.0f, true, true, false, false, 2);
        }

        public static void AttachToPhysically(this Entity entity1, Entity toEntity, string boneName, Vector3 offset, Vector3 rotation)
        {
            AttachToPhysically(entity1, toEntity, toEntity.Bones[boneName].Index, offset, rotation);
        }

        public static bool IsEntirelyInGarage(this Entity entity, GarageDoor garage)
        {
            return Function.Call<bool>(Hash.IS_OBJECT_ENTIRELY_INSIDE_GARAGE, garage, entity, 0, 0);
        }

        public static bool IsPartiallyInGarage(this Entity entity, GarageDoor garage)
        {
            return Function.Call<bool>(Hash.IS_OBJECT_PARTIALLY_INSIDE_GARAGE, garage, entity, 0);
        }

        public static void SetReduceGrip(this Vehicle vehicle, bool state)
        {
            Function.Call(Hash.SET_VEHICLE_REDUCE_GRIP, vehicle, state);
        }

        public static float GetKineticEnergy(this Vehicle vehicle)
        {
            return 0.5f * HandlingData.GetByVehicleModel(vehicle.Model).Mass * (float)Math.Pow(vehicle.Speed, 2);
        }

        public static VehicleReplica Clone(this Vehicle vehicle)
        {
            return new VehicleReplica(vehicle);
        }

        public static void TeleportTo(this Vehicle vehicle, Vector3 position)
        {
            position = vehicle.Position.TransferHeight(position);

            position.RequestCollision();
            vehicle.Position = position;
        }

        public static void SetVisible(this Vehicle vehicle, bool isVisible)
        {
            vehicle.IsVisible = isVisible;
            vehicle.IsCollisionEnabled = isVisible;
            vehicle.IsPositionFrozen = !isVisible;
            vehicle.IsEngineRunning = isVisible;

            foreach (Ped ped in vehicle.Occupants)
            {
                ped.IsVisible = isVisible;
                ped.CanBeDraggedOutOfVehicle = isVisible;
            }
        }

        public static bool IsPlayerDriving(this Vehicle vehicle)
        {
            return vehicle.IsFunctioning() && FusionUtils.PlayerVehicle == vehicle && FusionUtils.PlayerPed.SeatIndex == VehicleSeat.Driver;
        }

        public static bool IsDMC12TimeMachine(this Vehicle vehicle)
        {
            return vehicle.Model == FusionUtils.DMC12 && vehicle.Mods[VehicleModType.TrimDesign].Index != 0;
        }

        public static float GetMPHSpeed(this Vehicle vehicle)
        {
            return vehicle.Speed.ToMPH();
        }

        public static void SetMPHSpeed(this Vehicle vehicle, float value)
        {
            vehicle.ForwardSpeed = value.ToMS();
        }

        public static bool CanHoverTransform(this Vehicle vehicle)
        {
            return (vehicle.Bones["misc_a"].Index != 0 && vehicle.Bones["misc_b"].Index != -1 && vehicle.Bones["misc_c"].Index != 0 && vehicle.Bones["misc_e"].Index != -1 && vehicle.Bones["misc_q"].Index != -1 && vehicle.Bones["misc_s"].Index != -1 && vehicle.Bones["misc_z"].Index != -1);
        }

        public static void SetLightsBrightness(this Vehicle vehicle, float brightness)
        {
            Function.Call(Hash.SET_VEHICLE_LIGHT_MULTIPLIER, vehicle, brightness);
        }

        public static bool SameDirection(this Vehicle vehicle, Vehicle vehicle1)
        {
            return vehicle.Rotation.Z.Near(vehicle1.Rotation.Z);
        }

        public static void DeleteCompletely(this Vehicle vehicle)
        {
            if (vehicle.NotNullAndExists())
                foreach (Ped x in vehicle.Occupants)
                    if (x != FusionUtils.PlayerPed)
                        x?.Delete();

            vehicle?.Delete();
        }

        public static bool IsFunctioning(this Vehicle vehicle)
        {
            return vehicle.NotNullAndExists() && vehicle.IsAlive && !vehicle.IsDead;
        }

        public static void SetLightsMode(this Vehicle vehicle, LightsMode lightsMode)
        {
            Function.Call(Hash._SET_VEHICLE_LIGHTS_MODE, vehicle, lightsMode);
            Function.Call(Hash.SET_VEHICLE_LIGHTS, vehicle, lightsMode);
        }

        public static void GetLightsState(this Vehicle vehicle, out bool lightsOn, out bool highbeamsOn)
        {
            bool _lightsOn;
            bool _highbeamsOn;

            unsafe
            {
                Function.Call(Hash.GET_VEHICLE_LIGHTS_STATE, vehicle, &_lightsOn, &_highbeamsOn);
            }

            lightsOn = _lightsOn;
            highbeamsOn = _highbeamsOn;
        }

        public static bool DecreaseSpeedAndWait(this Vehicle vehicle, float by = 20)
        {
            Vector3 vel = vehicle.RelativeVelocity();

            if (vel.Y >= -2 && vel.Y <= 2)
                return true;

            vehicle.Speed -= by * Game.LastFrameTime;

            return false;
        }

        /// <summary>
        /// Checks if the given <paramref name="vehicle"/> is a train.
        /// </summary>
        /// <param name="vehicle">Instance of a <see cref="Vehicle"/>.</param>
        /// <returns><c>true</c> if <paramref name="vehicle"/> is a train; otherwise <c>false</c></returns>
        public static bool IsTrain(this Vehicle vehicle)
        {
            return Function.Call<bool>(Hash.IS_THIS_MODEL_A_TRAIN, vehicle.Model.Hash);
        }

        /// <summary>
        /// Sets <paramref name="train"/>'s cruise <paramref name="speed"/> value (m/s).
        /// </summary>
        /// <param name="train">Instance of a <see cref="Vehicle"/>.</param>
        /// <param name="speed">Cruise speed (m/s).</param>
        public static void SetTrainCruiseSpeed(this Vehicle train, float speed)
        {
            if (!train.IsTrain())
                return;

            Function.Call(Hash.SET_TRAIN_CRUISE_SPEED, train, speed);
        }

        /// <summary>
        /// Sets <paramref name="train"/>'s cruise <paramref name="speed"/> value (MPH).
        /// </summary>
        /// <param name="train">Instance of a <see cref="Vehicle"/>.</param>
        /// <param name="speed">Cruise speed (MPH).</param>
        public static void SetTrainCruiseMPHSpeed(this Vehicle train, float speed)
        {
            if (!train.IsTrain())
                return;

            train.SetTrainCruiseSpeed(speed.ToMS());
        }

        /// <summary>
        /// Sets <paramref name="train"/>'s <paramref name="speed"/> value (m/s).
        /// </summary>
        /// <param name="train">Instance of a <see cref="Vehicle"/>.</param>
        /// <param name="speed">Speed (m/s).</param>
        public static void SetTrainSpeed(this Vehicle train, float speed)
        {
            if (!train.IsTrain())
                return;

            Function.Call(Hash.SET_TRAIN_SPEED, train, speed);
        }

        /// <summary>
        /// Sets <paramref name="train"/>'s <paramref name="speed"/> value (MPH).
        /// </summary>
        /// <param name="train">Instance of a <see cref="Vehicle"/>.</param>
        /// <param name="speed">Speed (MPH).</param>
        public static void SetTrainMPHSpeed(this Vehicle train, float speed)
        {
            if (!train.IsTrain())
                return;

            train.SetTrainSpeed(speed.ToMS());
        }

        /// <summary>
        /// Makes <paramref name="train"/> derail.
        /// </summary>
        /// <param name="train">Instance of a <see cref="Vehicle"/>.</param>
        public static void Derail(this Vehicle train)
        {
            if (!train.IsTrain())
                return;

            Function.Call(Hash.SET_RENDER_TRAIN_AS_DERAILED, train, true);
        }

        /// <summary>
        /// Returns the carriage at <paramref name="index"/>'s position of the <paramref name="train"/>. 0 returns the first carriage behind the <paramref name="train"/>.
        /// </summary>
        /// <param name="train">Instance of a <see cref="Vehicle"/>.</param>
        /// <param name="index">Position of the carriage.</param>
        /// <returns><see cref="Vehicle"/> instance of the carriage</returns>
        public static Vehicle GetTrainCarriage(this Vehicle train, int index)
        {
            if (!train.IsTrain())
                return null;

            return Function.Call<Vehicle>(Hash.GET_TRAIN_CARRIAGE, train, index);
        }

        /// <summary>
        /// Sets the <paramref name="train"/>'s <paramref name="position"/>.
        /// </summary>
        /// <param name="train">Instance of a <see cref="Vehicle"/>.</param>
        /// <param name="position">Instance of a <see cref="Vector3"/>.</param>
        public static void SetTrainPosition(this Vehicle train, Vector3 position)
        {
            Function.Call(Hash.SET_MISSION_TRAIN_COORDS, train, position.X, position.Y, position.Z);
        }

        public static unsafe Vector3 GetBoneOriginalTranslation(this Vehicle vehicle, int index)
        {
            CVehicle* veh = (CVehicle*)vehicle.MemoryAddress;
            NativeVector3 v = veh->inst->archetype->skeleton->skeletonData->bones[index].translation;
            return v;
        }

        public static unsafe Quaternion GetBoneOriginalRotation(this Vehicle vehicle, int index)
        {
            CVehicle* veh = (CVehicle*)vehicle.MemoryAddress;
            NativeVector4 v = veh->inst->archetype->skeleton->skeletonData->bones[index].rotation;
            return v;
        }

        public static unsafe int GetBoneIndex(this Vehicle vehicle, string boneName)
        {
            if (vehicle == null)
                return -1;

            CVehicle* veh = (CVehicle*)vehicle.MemoryAddress;
            crSkeletonData* skelData = veh->inst->archetype->skeleton->skeletonData;
            uint boneCount = skelData->bonesCount;

            for (uint i = 0; i < boneCount; i++)
            {
                if (skelData->GetBoneNameForIndex(i) == boneName)
                    return unchecked((int)i);
            }

            return -1;
        }

        public static Dictionary<string, Vector3> GetWheelPositions(this Vehicle vehicle)
        {
            Dictionary<string, Vector3> ret = new Dictionary<string, Vector3>();

            foreach (string wheel in FusionUtils.WheelsBonesNames)
                if (vehicle.Bones[wheel].Index > 0)
                    ret.Add(wheel, vehicle.Bones[wheel].RelativePosition.GetSingleOffset(Coordinate.Z, -0.05f));

            return ret;
        }
    }
}
