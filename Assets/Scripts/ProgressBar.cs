using UnityEngine;

public abstract class ProgressBar : MonoBehaviour
{
    [SerializeField] 
    public GameObject barRef; // UI Image

    private RectTransform _rectTransformBar;
    
    private Vector3 _scaleRepresenter = new Vector3(1,1,1);
    
    protected void Start()
    {
        _rectTransformBar = barRef.GetComponent<RectTransform>();
    }

    protected void Update()
    {
        _scaleRepresenter.x = GetProgress();
        _rectTransformBar.localScale = _scaleRepresenter;
    }

    protected abstract float GetProgress();
}
