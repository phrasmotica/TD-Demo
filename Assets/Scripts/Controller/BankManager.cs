using System;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.UI;
using TMPro;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Controller
{
    public class BankManager : MonoBehaviour
    {
        public GameObject Canvas;

        public GameObject RewardTextPrefab;

        public GameOver GameOver;

        public TowerController TowerController;

        [Range(10, 100)]
        public int StartingMoney;

        [Range(0, 5)]
        public int StartingCoupons;

        [Range(0.5f, 1)]
        public float SellFraction;

        public int Money { get; private set; }

        // TODO: make enemies drop coupons and require the player to click on them to pick them up
        public int Coupons { get; private set; }

        public event Action<int> OnMoneyChange;

        public event Action<int> OnCouponsChange;

        private void Start()
        {
            GameOver.OnRestart += ResetMoney;

            TowerController.OnPlaceTower += tower =>
            {
                if (Coupons > 0)
                {
                    AddCoupons(-1);
                }
                else
                {
                    AddMoney(-tower.GetPrice());
                }
            };

            TowerController.OnStartUpgradeSelectedTower += tower =>
            {
                if (tower != null)
                {
                    var (canUpgrade, cost) = tower.GetUpgradeInfo();
                    if (canUpgrade)
                    {
                        AddMoney(-cost);
                        tower.DoUpgrade();
                    }
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
            ResetCoupons();
        }

        public bool CanAfford(int price) => Coupons > 0 || price <= Money;

        public bool CanAffordToBuy(TowerBehaviour tower) => CanAfford(tower.GetPrice());

        public void AddMoney(int amount) => SetMoney(Money + amount);

        public void ResetMoney() => SetMoney(StartingMoney);

        private void SetMoney(int money)
        {
            Money = money;
            OnMoneyChange?.Invoke(money);
        }

        public void AddCoupons(int amount) => SetCoupons(Coupons + amount);

        private void ResetCoupons() => SetCoupons(StartingCoupons);

        private void SetCoupons(int coupons)
        {
            Coupons = coupons;
            OnCouponsChange?.Invoke(coupons);
        }

        public void AddReward(Enemy e, TowerBehaviour tower)
        {
            var reward = tower.ComputeGoldReward(e.BaseGoldReward);
            AddMoney(reward);
            CreateRewardText(e, reward);
        }

        public int GetSellPrice(TowerBehaviour tower)
        {
            var adjustedSellPrice = (int) (tower.GetTotalValue() * SellFraction);
            return Mathf.Max(adjustedSellPrice, 1);
        }

        public void CreateRewardText(Enemy e, int reward)
        {
            var textPos = e.transform.position + new Vector3(0, 0.2f);
            var text = Instantiate(RewardTextPrefab, textPos, Quaternion.identity, Canvas.transform);
            text.GetComponent<TextMeshProUGUI>().text = $"+{reward}";
        }
    }
}
