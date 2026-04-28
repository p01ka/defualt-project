using UnityEngine;

namespace IdxZero.Base.MVP
{
    public interface IViewFactory<out T> where T : MonoBehaviour
    {
        T Create();
    }
}