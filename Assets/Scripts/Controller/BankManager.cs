using System;
using TDDemo.Assets.Scripts.Distractions;
using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Controller
{
    public class BankManager : MonoBehaviour
    {
        public GameObject Canvas;

        public GameObject RewardTextPrefab;

        [Range(10, 100)]
        public int StartingMoney;

        [Range(0, 5)]
        public int StartingCoupons;

        [Range(0.5f, 1)]
        public float SellFraction;

        private bool _useCoupons;

        public int Money { get; private set; }

        public int Coupons { get; private set; }

        // TODO: pass a function in these events that can compute whether
        // we can afford a given price
        public UnityEvent<int> OnMoneyChange;

        public UnityEvent<int> OnCouponsChange;

        public UnityEvent<bool> OnChangeUseCoupons;

        private void Start()
        {
            ResetMoney();
            ResetCoupons();
        }

        public void BuyTower(TowerBehaviour tower)
        {
            if (tower != null)
            {
                Buy(tower.GetPrice());
            }
        }

        public void BuyDistraction(DistractionSource distractionSource)
        {
            if (distractionSource != null)
            {
                Buy(distractionSource.Price);
            }
        }

        public void SellTower(TowerBehaviour tower)
        {
            if (tower != null)
            {
                AddMoney(GetSellPrice(tower));
            }
        }

        public PurchaseMethod CanAfford(int price)
        {
            if (_useCoupons && Coupons > 0)
            {
                return PurchaseMethod.Coupons;
            }

            return price <= Money ? PurchaseMethod.Money : PurchaseMethod.None;
        }

        public bool CanAffordVia(int price, PurchaseMethod purchaseMethod)
        {
            if (purchaseMethod == PurchaseMethod.Coupons)
            {
                return Coupons > 0;
            }

            return price <= Money;
        }

        public PurchaseMethod CanAffordToBuy(TowerBehaviour tower) => CanAfford(tower.GetPrice());

        public void AddMoney(int amount) => SetMoney(Money + amount);

        public void ResetMoney() => SetMoney(StartingMoney);

        private void SetMoney(int money)
        {
            Money = money;
            OnMoneyChange.Invoke(Money);
        }

        public void AddCoupons(int amount) => SetCoupons(Coupons + amount);

        public void ResetCoupons() => SetCoupons(StartingCoupons);

        private void SetCoupons(int coupons)
        {
            Coupons = coupons;
            OnCouponsChange.Invoke(coupons);
        }

        public bool TryBuyVia(int price, PurchaseMethod purchaseMethod)
        {
            if (CanAffordVia(price, purchaseMethod))
            {
                BuyVia(price, purchaseMethod);
                return true;
            }

            return false;
        }

        private void Buy(int price)
        {
            if (_useCoupons && Coupons > 0)
            {
                AddCoupons(-1);
            }
            else
            {
                AddMoney(-price);
            }
        }

        private void BuyVia(int price, PurchaseMethod purchaseMethod)
        {
            if (purchaseMethod == PurchaseMethod.Coupons)
            {
                AddCoupons(-1);
            }
            else
            {
                AddMoney(-price);
            }
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

        public void CreateCouponText(Vector3 position, int amount)
        {
            var textPos = position + new Vector3(0, 0.2f);
            var text = Instantiate(RewardTextPrefab, textPos, Quaternion.identity, Canvas.transform);
            var textComponent = text.GetComponent<TextMeshProUGUI>();

            textComponent.text = $"+{amount}";
            textComponent.color = ColourHelper.Coupon;
        }

        public void SetUseCoupons(bool useCoupons)
        {
            _useCoupons = useCoupons;
            OnChangeUseCoupons.Invoke(useCoupons);
        }
    }

    public enum PurchaseMethod
    {
        Money,
        Coupons,
        None,
    }
}
