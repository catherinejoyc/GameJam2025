using Assets.Scripts.Collectibles;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class EffectDisplayUI : MonoBehaviour
{
    public static EffectDisplayUI Instance;
    public List<Sprite> sprites = new List<Sprite>();
    public GameObject effectContainer;
    public GameObject effectItemPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private List<Effects> oldEffects = new List<Effects>();

    public void RefreshUI(List<Effects> effects)
    {
        if (effects.Equals(oldEffects)) return;
        oldEffects = effects;
        foreach (Transform child in effectContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var effect in effects)
        {
            GameObject go = Instantiate(effectItemPrefab, effectContainer.transform);
            go.transform.localScale = new Vector3(1, 2f, 1);
            go.transform.position += Vector3.down;
            go.transform.Find("Icon").GetComponent<Image>().sprite = GetCorrectSprite(effect);
        }
    }

    private Sprite GetCorrectSprite(Effects effect)
    {
        return sprites.ElementAt((int)effect);
    }
}
