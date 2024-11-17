using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public static int health = 3;
    public Image[] hearts;
    public Sprite FullHeart;
    public Sprite EmptyHeart;

    // Update is called once per frame
    void Update()
    {
        foreach (Image img in hearts)
        {
            img.sprite = EmptyHeart;
        }

        for (int i = 0; i < health; i++)
        {
            hearts[i].sprite = FullHeart;
        }
    }
}
