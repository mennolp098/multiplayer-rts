using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EntityHealthbar : MonoBehaviour {

    public Image greenBar;
    public Entity entity;

    private CanvasGroup _group;
    private Entity _entity;
    private float _maxBarWidth;
    private float _maxBarHeight;

    private void Start()
    {
        _maxBarWidth = greenBar.rectTransform.sizeDelta.x;
        _maxBarHeight = greenBar.rectTransform.sizeDelta.y;

        _group = GetComponent<CanvasGroup>();

        if(entity != null)
            SetEntity(entity);

        HideHealthBar();
    }

    private void SetEntity(Entity entity)
    {
        this._entity = entity;
        entity.OnDamageReceive += UpdateHealthBar;
        entity.OnSelect += ShowHealthBar;
        entity.OnDeselect += HideHealthBar;
    }

    private void ShowHealthBar()
    {
        _group.alpha = 1;
    }

    private void HideHealthBar()
    {
        _group.alpha = 0;
    }

    private void UpdateHealthBar()
    {
        greenBar.rectTransform.sizeDelta = new Vector2(_maxBarWidth / ((float)_entity.maxHitPoints / (float)_entity.currentHitPoints), _maxBarHeight);
    }


}
