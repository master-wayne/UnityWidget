using UnityEngine;

namespace DCG_UI
{
    public class Utilities : MonoBehaviour
    {
        public static void PositionUI(RectTransform rectTransform, Vector2 sizeDelta, ANCHOR_POSITION anchorPos)
        {
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = sizeDelta;
                switch (anchorPos)
                {
                    case ANCHOR_POSITION.TOP_LEFT:
                        rectTransform.anchorMin = new Vector2(0, 1);
                        rectTransform.anchorMax = new Vector2(0, 1);
                        rectTransform.pivot = new Vector2(0, 1);
                        break;

                    case ANCHOR_POSITION.TOP_CENTER:
                        rectTransform.anchorMin = new Vector2(0.5f, 1);
                        rectTransform.anchorMax = new Vector2(0.5f, 1);
                        rectTransform.pivot = new Vector2(0.5f, 1);
                        break;

                    case ANCHOR_POSITION.TOP_RIGHT:
                        rectTransform.anchorMin = new Vector2(1, 1);
                        rectTransform.anchorMax = new Vector2(1, 1);
                        rectTransform.pivot = new Vector2(1, 1);
                        break;

                    case ANCHOR_POSITION.MIDDLE_LEFT:
                        rectTransform.anchorMin = new Vector2(0, 0.5f);
                        rectTransform.anchorMax = new Vector2(0, 0.5f);
                        rectTransform.pivot = new Vector2(0, 0.5f);
                        break;

                    case ANCHOR_POSITION.MIDDLE_CENTER:
                        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                        rectTransform.pivot = new Vector2(0.5f, 0.05f);
                        break;

                    case ANCHOR_POSITION.MIDDLE_RIGHT:
                        rectTransform.anchorMin = new Vector2(1, 0.5f);
                        rectTransform.anchorMax = new Vector2(1, 0.5f);
                        rectTransform.pivot = new Vector2(1, 0.5f);
                        break;

                    case ANCHOR_POSITION.BOTTOM_LEFT:
                        rectTransform.anchorMin = new Vector2(0, 0);
                        rectTransform.anchorMax = new Vector2(0, 0);
                        rectTransform.pivot = new Vector2(0, 0);
                        break;

                    case ANCHOR_POSITION.BOTTOM_CENTER:
                        rectTransform.anchorMin = new Vector2(0.5f, 0);
                        rectTransform.anchorMax = new Vector2(0.5f, 0);
                        rectTransform.pivot = new Vector2(0.5f, 0);
                        break;

                    case ANCHOR_POSITION.BOTTOM_RIGHT:
                        rectTransform.anchorMin = new Vector2(1, 0);
                        rectTransform.anchorMax = new Vector2(1, 0);
                        rectTransform.pivot = new Vector2(1, 0);
                        break;
                }
                rectTransform.anchoredPosition = Vector2.zero;
            }
        }
    }
}
