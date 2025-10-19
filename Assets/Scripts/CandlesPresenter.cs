using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class CandlesPresenter : MonoBehaviour, IPossessablePresenter
{
    public Candles candles;

    public Sprite litSprite;
    public Sprite unlitSprite;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = litSprite;
    }

    public void Extinguish()
    {
        sr.sprite = unlitSprite;
    }

    public void Possess()
    {
        sr.color = Color.green;
    }

    public void Depossess()
    {
        sr.color = Color.white;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
