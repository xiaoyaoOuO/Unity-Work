using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public static class ConStantSetting
{
    public const float PlayerSpeed = 6f; //玩家移动速度
    public const float PlayerJumpForce = 10f; //玩家跳跃力度
    public const float PlayerAcceleration = 10f; //玩家加速度
    public const float PlayerGravity = 2f; //玩家重力加速度
    public const float PlayerJumpGraceTime = 0.2f; //玩家跳跃宽限时间
    public const float PlayerAttackCooldown = 0.1f; //玩家攻击冷却时间
    public const float PlayerDashSpeed = 17f; //玩家冲刺速度
    public const float PlayerDashUpSpeed = 12f; //玩家冲刺时竖直方向的速度
    public const float PlayerDashDuration = 0.3f; //玩家冲刺持续时间
    public const float PlayerMaxDashCount = 1; //玩家最大冲刺次数
    public const float trailFXInterval = 0.08f; //玩家冲刺时拖尾特效的间隔时间
    public const float PlayerBulletTimeDuration = 0.5f; //玩家子弹时间持续时间
    public const float PlayerBulletTimeRefillSpeed = 0.5f; //玩家子弹时间充能速度
    public const float PlayerRollSpeed = 12f; //玩家翻滚速度
    public const float PlayerRollCooldown = 1f; //玩家翻滚冷却时间
    public const float PlayerWallClimbSpeed = 2f; //玩家攀爬墙壁时的速度
    public const float PlayerWallBoostForceY = 10f; //玩家从墙壁上弹跳时的y方向力度
    public const float PlayerWallBoostForceX = 10f; //玩家从墙壁上弹跳时的x方向力度
    public const float PlayerWallJumpDuration = 0.5f; //玩家从墙壁上弹跳的持续时间

}
