using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    [Header("弓箭专属参数")]
    public Transform arrowSpawnPoint; // 箭矢生成点（拖弓的弓弦位置物体）
    public GameObject arrowPrefab;    // 箭矢预制体（拖前面做的Arrow预制体）
    public GameObject arrowInBow;     // 未发射时弓上的箭矢（拖场景中的ArrowInBow）
    public float drawSpeed;      // 拉弓速度
    public float maxDrawForce;  // 最大拉力（影响箭矢速度）
    public float currentDrawForce;    // 当前拉力
    public bool isDrawing = false;    // 是否正在拉弓

    [Header("UI与反馈")]
    public RectTransform pullForceBar; // 拉力进度条（UI滑块，可选）
    public AudioClip drawSound;       // 拉弓音效（可选）
    public AudioClip shootSound;      // 发射音效（可选）
    private AudioSource audioSource;

    private void Awake()
    {
        weaponType = WeaponType.Bow;
        // 获取音效播放组件（如果挂载了AudioSource，就自动用它播放音效）
        //audioSource = GetComponent<AudioSource>();
        //if (pullForceBar != null) { pullForceBar.localScale = new Vector3(0, 1, 1); }
    }

    void Update()
    {
        // 监听鼠标右键操作：按下→开始拉弓，长按→蓄力，松开→发射
        if (Input.GetMouseButtonDown(1)) StartDrawing();  // 按下右键（1=右键）→ 触发“开始拉弓”
        if (Input.GetMouseButton(1) && isDrawing) Draw(); // 长按右键 + 正在拉弓→ 触发“蓄力”
        if (Input.GetMouseButtonUp(1) && isDrawing) Shoot(); // 松开右键 + 正在拉弓→ 触发“发射”
    }

    private void StartDrawing()
    {
        isDrawing = true;
        arrowInBow.SetActive(true); // 确保弓上有箭（防止没箭还能拉弓）
        //if (drawSound != null) audioSource.PlayOneShot(drawSound); // 播放拉弓音效（有就播）
    }

    public override void Draw()
    {
        // 拉力递增：从0慢慢涨到maxDrawForce（drawSpeed越大，涨得越快）
        currentDrawForce = Mathf.MoveTowards(currentDrawForce, maxDrawForce, drawSpeed * Time.deltaTime);

        // 拉力条更新：实时显示蓄力进度（比如拉力到15，进度条就显示一半）
        //if (pullForceBar != null)
        //{
        //    float forcePercent = currentDrawForce / maxDrawForce; // 计算拉力百分比（0~1）
        //    pullForceBar.localScale = new Vector3(forcePercent, 1, 1); // 调整进度条宽度
        //}

        
        
    }


    public override void Shoot()
    {
        if (currentDrawForce < 5f) // 拉力不足5（比如轻轻碰了一下右键），不发射（避免误射）
        {
            ResetWeapon(); // 重置武器状态（比如拉力条归零）
            return;
        }

        // 1. 生成箭矢：在arrowSpawnPoint位置，创建arrowPrefab（你做的箭预制体）
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>(); // 获取箭矢的刚体组件（控制物理运动）

        // 2. 计算发射方向：摄像机朝向=发射方向，略微向下补偿重力（避免箭飞得太高）
        Vector3 shootDirection = Camera.main.transform.forward;
        shootDirection.y -= 0.05f; // 轻微向下，模拟真实弓箭的抛射（可微调，比如-0.03）

        // 3. 给箭矢施加力：拉力越大，力越大，箭飞得越快
        arrowRb.AddForce(shootDirection * currentDrawForce, ForceMode.Impulse);

        // 4. 箭矢生命周期：5秒后自动销毁（避免场景里箭太多卡内存）
        Destroy(arrow, 10f);

        // 5. 音效与视觉反馈：播放发射音效，隐藏弓上的箭（模拟发射出去）
        //if (shootSound != null) audioSource.PlayOneShot(shootSound);
        arrowInBow.SetActive(false);

        // 6. 重置武器状态：拉力归零、弓恢复原状、准备下次拉弓
        ResetWeapon();

    }

    public override void ResetWeapon()
    {
        currentDrawForce = 0f; // 拉力归零
        isDrawing = false; // 标记为“未拉弓”，可以再次拉弓
        if (pullForceBar != null) pullForceBar.localScale = new Vector3(0, 1, 1); // 拉力条隐藏
                                                                                  // 弓恢复原状（从放大的1.1倍回到1倍）
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 2f);
        // 0.5秒后重新显示弓上的箭（模拟上箭动作，不用手动上箭）
        Invoke("ShowArrowInBow", 2f);
    }

    private void ShowArrowInBow()
    {
        if (arrowInBow != null) arrowInBow.SetActive(true); // 重新显示弓上的箭
    }



}
