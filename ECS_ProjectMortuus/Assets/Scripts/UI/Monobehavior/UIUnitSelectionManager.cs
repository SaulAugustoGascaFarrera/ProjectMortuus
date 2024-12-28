using UnityEngine;

public class UIUnitSelectionManager : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform.gameObject.SetActive(false);
    }

    private void Start()
    {
        UnitSelectionManager.Instance.OnSelectionAreaStart += Instance_OnSelectionAreaStart;
        UnitSelectionManager.Instance.OnSelectionAreaEnd += Instance_OnSelectionAreaEnd;
    }


    private void Update()
    {
        if(rectTransform.gameObject.activeSelf)
        {
            UpdateVisual();
        }
        
    }

    private void Instance_OnSelectionAreaStart(object sender, System.EventArgs e)
    {
        rectTransform.gameObject.SetActive(true);

        UpdateVisual();
    }

    private void Instance_OnSelectionAreaEnd(object sender, System.EventArgs e)
    {
        rectTransform.gameObject.SetActive(false);
    }

    public void UpdateVisual()
    {
        Rect selectionRect = UnitSelectionManager.Instance.GetSelectionAreaReact();

        rectTransform.anchoredPosition = new Vector2(selectionRect.x,selectionRect.y);

        rectTransform.sizeDelta = new Vector2(selectionRect.width,selectionRect.height);
    }

   
}
