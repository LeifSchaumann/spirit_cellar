using System.Threading.Tasks;
using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    public async Task Possess(Possession p)
    {
        Vector3 startPos = PresentationManager.main.GetWorldCoords(p.depossessed.pos);
        Vector3 endPos = PresentationManager.main.GetWorldCoords(p.possessed.pos);

        transform.position = startPos;
        sr.enabled = true;

        float duration = (endPos - startPos).magnitude / 5f;

        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            await Task.Yield();
        }
        transform.position = endPos;
        sr.enabled = false;
    }
}
