using UnityEngine;

namespace IdxZero.Utils.Extensions
{
    public static class TextureExtensions
    {
        public static Sprite ConvertToSprite(this Texture2D texture)
        {
            texture.filterMode = FilterMode.Point;
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            return Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
        }

        public static Texture2D FlipTexture(this Texture2D original, bool upSideDown = true)
        {
            Texture2D flipped = new Texture2D(original.width, original.height);
            int xN = original.width;
            int yN = original.height;
            for (int i = 0; i < xN; i++)
            {
                for (int j = 0; j < yN; j++)
                {
                    if (upSideDown)
                    {
                        flipped.SetPixel(j, xN - i - 1, original.GetPixel(j, i));
                    }
                    else
                    {
                        flipped.SetPixel(xN - i - 1, j, original.GetPixel(i, j));
                    }
                }
            }
            ReleaseLoadedTextureFromMemmory(original);
            return flipped;
        }

        public static Texture2D RotateTexture(this Texture2D originalTexture, bool clockwise)
        {
            Color32[] original = originalTexture.GetPixels32();
            Color32[] rotated = new Color32[original.Length];
            int width = originalTexture.width;
            int height = originalTexture.height;

            for (int j = 0; j < height; ++j)
            {
                for (int i = 0; i < width; ++i)
                {
                    var iRotated = (i + 1) * height - j - 1;
                    var iOriginal = clockwise ? original.Length - 1 - (j * width + i) : j * width + i;
                    rotated[iRotated] = original[iOriginal];
                }
            }
            Texture2D rotatedTexture = new Texture2D(height, width);
            rotatedTexture.SetPixels32(rotated);
            rotatedTexture.Apply();
            ReleaseLoadedTextureFromMemmory(originalTexture);
            return rotatedTexture;
        }

        public static Texture2D CutTextureByDefaultRect(this Texture2D sourceTexture)
        {
            int height = sourceTexture.height;
            int width = sourceTexture.width;
            int textSize;
            int x = 0;
            int y = 0;
            if (width > height)
            {
                textSize = height;
                x = (width - height) / 2;
            }
            else
            {
                textSize = width;
                y = (height - width) / 2;
            }
            return CutByRect(sourceTexture, textSize, textSize, x, y);
        }

        public static Texture2D CutTextureByHieghtRect(this Texture2D sourceTexture)
        {
            int height = sourceTexture.height;
            int width = sourceTexture.width;
            int y = 0;
            float screenGap = (float)Screen.height / (float)Screen.width;
            var textHeight = height;
            var textWidth = Mathf.FloorToInt(textHeight / screenGap);
            int x = (width - textWidth) / 2;
            return CutByRect(sourceTexture, textWidth, textHeight, x, y);
        }

        public static Texture2D CutByRect(Texture2D sourceTexture, int textWidth, int textHeight, int x, int y)
        {
            Color[] cuttedColors = sourceTexture.GetPixels(x, y, textWidth, textHeight);
            Texture2D cuttedTexture = new Texture2D(textWidth, textWidth);
            cuttedTexture.SetPixels(cuttedColors);
            cuttedTexture.Apply();
            Object.Destroy(sourceTexture);
            return cuttedTexture;
        }

        public static void ReleaseLoadedTextureFromMemmory(Texture2D texture)
        {
            if (texture != null)
            {
                UnityEngine.Object.DestroyImmediate(texture, true);
            }
        }
    }
}