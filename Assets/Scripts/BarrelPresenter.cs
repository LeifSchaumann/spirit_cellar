using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class BarrelPresenter : MonoBehaviour, IPossessablePresenter
{
    public Barrel barrel;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public async Task Roll(BarrelRoll br)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = PresentationManager.main.GetWorldCoords(br.endPos);

        float duration = (endPos - startPos).magnitude / 5f;

        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            await Task.Yield();
        }
        transform.position = endPos;
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
