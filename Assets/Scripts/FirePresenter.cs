using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class FirePresenter : MonoBehaviour, ILevelObjectPresenter
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
