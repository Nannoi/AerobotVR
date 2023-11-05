using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent upEvent;
    public UnityEvent downEvent;
    public GameObject effect;
    public float speedEffect = 1;
    public float scaleEffect = 1.2f;
    float speed;
    float scale = 1;

    // Start is called before the first frame update
    private void Start()
    {
        effect.SetActive(false);
        speed = speedEffect;
    }

    void Update()
    {
        if (effect.activeSelf)
        {
            scale += Time.deltaTime * speed;
            if (scale > scaleEffect)
            {
                speed = -speedEffect;
            }
            if (scale < scaleEffect - 0.1f)
            {
                speed = speedEffect;
            }
            effect.transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");
        downEvent?.Invoke();
        effect.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Up");
        upEvent?.Invoke();
        effect.SetActive(false);
    }
}