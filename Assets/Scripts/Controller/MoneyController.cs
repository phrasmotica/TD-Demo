using System;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.UI;
using TMPro;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Controller
{
    public class MoneyController : MonoBehaviour
    {
        public GameObject Canvas;

        public GameObject RewardTextPrefab;

        public GameOver GameOver;

        public TowerController TowerController;

        [Range(10, 100)]
        public int StartingMoney;

        [Range(0.5f, 1)]
        public float SellFraction;

        public int Money { get; private set; }

        public event Action<int> OnMoneyChange;

        private void Start()
        {
            OnMoneyChange += money => Money = money;

            GameOver.OnRestart += ResetMoney;

            TowerController.OnPlaceTower += tower => AddMoney(-tower.GetPrice());

            TowerController.OnStartUpgradeSelectedTower += tower =>
            {
                if (tower != null)
                {
                    AddMoney(-tower.GetUpgradeCost().Value);

                    tower.DoUpgrade();
                }
            };

            TowerController.OnSellSelectedTower += tower =>
            {
                if (tower != null)
                {
                    AddMoney(GetSellPrice(tower));
                }
            };

            ResetMoney();
        }

        public bool CanAfford(int price) => price <= Money;

        public bool CanAffordToBuy(TowerBehaviour tower) => CanAfford(tower.GetPrice());

        public void AddMoney(int amount) => SetMoney(Money + amount);

        public void ResetMoney() => SetMoney(StartingMoney);

        private void SetMoney(int money) => OnMoneyChange(money);

        public int GetSellPrice(TowerBehaviour tower)
        {
            var adjustedSellPrice = (int) (tower.TotalValue * SellFraction);
            return Mathf.Max(adjustedSellPrice, 1);
        }

        public bool CanAffordToUpgrade(TowerBehaviour tower)
        {
            var cost = tower.GetUpgradeCost();
            return cost.HasValue && CanAfford(cost.Value);
        }

        public void CreateRewardText(Enemy e)
        {
            var textPos = e.transform.position + new Vector3(0, 0.2f);
            var text = Instantiate(RewardTextPrefab, textPos, Quaternion.identity, Canvas.transform);
            text.GetComponent<TextMeshProUGUI>().text = $"+{e.BaseGoldReward}";
        }
    }
}
