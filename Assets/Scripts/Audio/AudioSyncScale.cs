using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSyncScale : AudioSyncer
{
    public Vector3 beatScale;
    public Vector3 restScale;

    public Color[] beatColors;
    public Color restColor;
    private int randomIndex;
    private Image img;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    private IEnumerator MoveToScale(Vector3 target)
    {
        Vector3 current = transform.localScale;
        Vector3 initial = current;
        float timer = 0f;

        while (current != target)
        {
            current = Vector3.Lerp(initial, target, timer / timeToBeat);
            timer += Time.deltaTime;

            transform.localScale = current;

            yield return null;
        }

        isBeat = false;
    }

    private IEnumerator MoveToColor(Color target)
    {
        Color curr = img.color;
        Color initial = curr;
        float timer = 0;

        while (curr != target)
        {
            curr = Color.Lerp(initial, target, timer / timeToBeat);
            timer += Time.deltaTime;

            img.color = curr;

            yield return null;
        }

        isBeat = false;
    }

    private Color RandomColor()
    {
        if (beatColors == null || beatColors.Length == 0) return Color.white;
        randomIndex = Random.Range(0, beatColors.Length);
        return beatColors[randomIndex];
    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        if (isBeat) return;

        img.color = Color.Lerp(img.color, restColor, restSmoothTime * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, restScale, restSmoothTime * Time.deltaTime);
    }

    public override void OnBeat()
    {
        base.OnBeat();

        StopCoroutine("MoveToScale");
        StartCoroutine("MoveToScale", beatScale);

        Color c = RandomColor();
        
        StopCoroutine("MoveToColor");
        StartCoroutine("MoveToColor", c);
    }
}