using UnityEngine;

namespace TDDemo.Assets.Scripts.Util
{
    public class BaseBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The logger. Must be set in a deriving script's Start() method.
        /// </summary>
        protected MethodLogger logger;
    }
}
