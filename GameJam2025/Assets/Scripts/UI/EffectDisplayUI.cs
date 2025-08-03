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

    public void RefreshUI(List<Effects> effects)
    {
        foreach (Transform child in effectContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var effect in effects)
        {
            GameObject go = Instantiate(effectItemPrefab, effectContainer.transform);
            go.transform.Find("Icon").GetComponent<Image>().sprite = GetCorrectSprite(effect);
        }
    }

    private Sprite GetCorrectSprite(Effects effect)
    {
        return sprites.ElementAt((int)effect);
    }
}
