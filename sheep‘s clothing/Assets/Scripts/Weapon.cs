using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 武器类型枚举（后续添加新武器只需在这里加）
public enum WeaponType { Bow, Shotgun, Arrow }

public abstract class Weapon : MonoBehaviour
{
    public WeaponType weaponType; // 武器类型
    public float damage;    // 伤害值
    public float maxRange;  // 最大射程

    // 抽象方法：拉弓/上膛（弓箭专属，其他武器可重写为上膛等）
    public abstract void Draw();
    // 抽象方法：发射（核心方法，每种武器实现不同）
    public abstract void Shoot();
    // 抽象方法：重置武器状态（比如箭矢回到弓上）
    public abstract void ResetWeapon();
}
