using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Items
{
    Invincibility,
    Hp,
    Fuel,
    Attack
}

public class Item : MonoBehaviour
{
    #region 변수
    [SerializeField] private Items itemType;
    #endregion

    #region Unity Life Cycle
    private void Update()
    {
        Move();
        CheckOutOfBounds();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        UseItem();
        Destroy(gameObject);
    }
    #endregion

    #region 아이템 동작
    private void Move() => transform.Translate(0, 0, -3f * Time.deltaTime);

    private void CheckOutOfBounds()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPos.x > 1.5f || viewPos.x < -0.5f || viewPos.y > 1.5f || viewPos.y < -0.5f)
            Destroy(gameObject);
    }

    private void UseItem()
    {
        switch (itemType)
        {
            case Items.Invincibility:
                Player.player.isInvincible = true;
                break;
            case Items.Hp:
                AddHp(20);
                break;
            case Items.Fuel:
                AddFuel(300);
                break;
            case Items.Attack:
                IncreaseAttackLevel();
                break;
        }
    }
    #endregion

    #region 세부 기능
    private void AddHp(int amount) => Player.player.hp = Mathf.Min(Player.player.hp + amount, 100);

    private void AddFuel(int amount) => Player.player.fuel = Mathf.Min(Player.player.fuel + amount, 1000);

    private void IncreaseAttackLevel() => Player.player.LevelCount = Mathf.Min(Player.player.LevelCount + 1, 3);
    #endregion
}
