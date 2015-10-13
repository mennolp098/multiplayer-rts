using UnityEngine;
using UnityEngine.UI;

public enum StatBarType
{
    Health,
    BuildProgress
}


public class EntityStatBar : MonoBehaviour
{

    public Image progressBar;
    public Entity entity;
    public StatBarType type;

    private CanvasGroup _group;
    private float _maxBarWidth;
    private float _maxBarHeight;

    private void Start()
    {
        _maxBarWidth = progressBar.rectTransform.sizeDelta.x;
        _maxBarHeight = progressBar.rectTransform.sizeDelta.y;

        _group = GetComponent<CanvasGroup>();

        if (entity != null)
            SetEntity(entity);

        HideStatBar();

        UpdateStatBar();
    }

    private void SetEntity(Entity entity)
    {
        this.entity = entity;

        switch (type)
        {
            case StatBarType.Health:
                entity.OnSelect += ShowStatBar;
                entity.OnDeselect += HideStatBar;
                entity.OnDamageReceive += UpdateStatBar;
                break;
            case StatBarType.BuildProgress:
                entity.OnConstructionProgress += UpdateStatBar;
                if (entity.GetType() == typeof(Building))
                    ((Building)entity).OnPlace += ShowStatBar;
                break;
        }
    }

    private void ShowStatBar()
    {
        _group.alpha = 1;
    }

    private void HideStatBar()
    {
        _group.alpha = 0;
    }

    private void UpdateStatBar(int amount = 0)
    {
        switch (type)
        {
            case StatBarType.Health:
                progressBar.rectTransform.sizeDelta = new Vector2(_maxBarWidth / ((float)entity.maxHitPoints / (float)entity.currentHitPoints), _maxBarHeight);
                break;
            case StatBarType.BuildProgress:
                progressBar.rectTransform.sizeDelta = new Vector2(_maxBarWidth / ((float)entity.requiredLabor / (float)entity.currentLabor), _maxBarHeight);
                if (entity.currentLabor >= entity.requiredLabor)
                {
                    entity.OnConstructionProgress -= UpdateStatBar;
                    HideStatBar();
                }
                break;
        }
    }
}
