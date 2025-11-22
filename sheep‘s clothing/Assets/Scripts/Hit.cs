using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public AudioClip hitSound; // 箭矢命中物体的音效（可选）
    public ParticleSystem hitEffect; // 命中粒子效果（可选）
    public int arrowDamage = 1; // 箭矢伤害（和标靶生命值对应）

    private void OnTriggerEnter(Collider other)
    {
        // 1. 过滤无效碰撞（自己、其他箭矢）
        if (other.CompareTag("FirstPerson") || other.CompareTag("Arrow"))
            return;

        // 2. 检测是否命中标靶（Tag为Target）
        DeathTrigger targetHealth = other.GetComponent<DeathTrigger>();
        if (targetHealth != null)
        {
            // 调用标靶的受伤方法，触发死亡
            targetHealth.TakeDamage(arrowDamage);
        }

        // 3. 箭矢自身的命中反馈（嵌入物体、播放音效等，原有逻辑保留）
        //if (hitSound != null)
            //AudioSource.PlayClipAtPoint(hitSound, transform.position);
        //if (hitEffect != null)
            //Instantiate(hitEffect, transform.position, Quaternion.LookRotation(transform.forward));

        // 4. 箭矢嵌入物体（原有逻辑）
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        transform.parent = other.transform;
        GetComponent<Collider>().enabled = false;
    }
}
