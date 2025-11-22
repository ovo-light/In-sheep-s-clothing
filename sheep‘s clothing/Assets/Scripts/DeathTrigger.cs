using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    [Header("基础配置")]
    public int maxHealth = 1; // 标靶生命值（默认1，一箭致命）
    private int currentHealth;

    [Header("死亡效果")]
    public float destroyDelay = 0.5f; // 被击中后延迟0.5秒消失（可看命中反馈）
    public AudioClip hitSound; // 被击中音效（可选）
    public ParticleSystem hitEffect; // 被击中粒子效果（可选）


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // 扣除生命值
        currentHealth -= damage;

        // 播放命中反馈（音效+粒子）
        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, transform.position); // 在标靶位置播放音效
        if (hitEffect != null)
            Instantiate(hitEffect, transform.position, Quaternion.identity); // 生成命中粒子

        // 生命值为0时，执行死亡逻辑
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 联机场景：只有本地玩家执行死亡逻辑（避免多端重复销毁）
        PhotonView photonView = GetComponent<PhotonView>();
        if (photonView != null && !photonView.IsMine)
            return;
        // （可选）添加死亡动画：比如标靶倒下、消失时缩放
        //transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, destroyDelay);

        // 延迟销毁标靶（让死亡动画/反馈完成）
        Destroy(gameObject, destroyDelay);
    }

}
