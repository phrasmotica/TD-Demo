using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controller
{
    public class GameOverScreen : MonoBehaviour
    {
        /// <summary>
        /// The game over canvas prefab.
        /// </summary>
        public GameObject GameOverCanvasPrefab;

        /// <summary>
        /// Ends the game.
        /// </summary>
        public void EndGame()
        {
            var gameOverCanvas = Instantiate(GameOverCanvasPrefab);

            var moneyStore = gameObject.GetComponent<MoneyStore>();
            var finishingMoney = moneyStore.Money;

            var moneyText = gameOverCanvas.transform.Find("Panel").Find("MoneyText");
            moneyText.gameObject.GetComponent<Text>().text = $"You finished with {finishingMoney} money";
        }
    }
}
