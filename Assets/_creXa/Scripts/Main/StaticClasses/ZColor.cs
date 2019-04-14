using UnityEngine;
using System.Collections;

namespace creXa.GameBase
{
    public static class ZColor
    {
        public static Color TRANSPARENT = new Color(1, 1, 1, 0);

        public static Color Rand
        {
            get { return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)); }
        }

        public static Color Alpha(Color rgb, float a)
        {
            return new Color(rgb.r, rgb.g, rgb.b, a);
        }

        public static Color Alpha(Vector3 rgb, float a)
        {
            return new Color(rgb.x, rgb.y, rgb.z, a);
        }

        public static Color32 HexToRGBA(int HexVal)
        {
            byte R = (byte)((HexVal >> 16) & 0xFF);
            byte G = (byte)((HexVal >> 8) & 0xFF);
            byte B = (byte)((HexVal) & 0xFF);
            return new Color32(R, G, B, 255);
        }

        public static Color32 HexToRGBA(uint HexVal)
        {
            byte R = (byte)((HexVal >> 24) & 0xFF);
            byte G = (byte)((HexVal >> 16) & 0xFF);
            byte B = (byte)((HexVal >> 8) & 0xFF);
            byte A = (byte)((HexVal) & 0xFF);
            return new Color32(R, G, B, A);
        }

        public static Color32 HSVtoRGB(Vector4 ahsv)
        {
            return HSVtoRGB(ahsv.x, ahsv.y, ahsv.z, ahsv.w);
        }

        public static Color32 HSVtoRGB(float h, float s, float v, float a)
        {
            h %= 360;
            if (h < 0) h += 360;

            float c = v * s;
            float x = c * (1 - Mathf.Abs((h / 60) % 2 - 1));
            float m = v - c;

            Vector3 rgb = Vector3.zero;
            int i = Mathf.FloorToInt(h);
            switch (i / 60)
            {
                case 0: rgb = new Vector3(c, x, 0); break;
                case 1: rgb = new Vector3(x, c, 0); break;
                case 2: rgb = new Vector3(0, c, x); break;
                case 3: rgb = new Vector3(0, x, c); break;
                case 4: rgb = new Vector3(x, 0, c); break;
                case 5: rgb = new Vector3(c, 0, x); break;
            }

            return new Color32((byte)((rgb.x+ m) * 255.0f), (byte)((rgb.y + m) * 255.0f), (byte)((rgb.z + m) * 255.0f), (byte)(a * 255.0f));
        }

        public static Vector4 RGBToHSV(Color rgbColor)
        {
            if (rgbColor.b > rgbColor.g && rgbColor.b > rgbColor.r)
            {
                return RGBToHSVHelper(4f, rgbColor.b, rgbColor.r, rgbColor.g, rgbColor.a);
            }
            else
            {
                if (rgbColor.g > rgbColor.r)
                {
                    return RGBToHSVHelper(2f, rgbColor.g, rgbColor.b, rgbColor.r, rgbColor.a);
                }
                else
                {
                    return RGBToHSVHelper(0f, rgbColor.r, rgbColor.g, rgbColor.b, rgbColor.a);
                }
            }
        }

        private static Vector4 RGBToHSVHelper(float offset, float dominantcolor, float colorone, float colortwo, float alpha)
        {
            Vector4 rtn = Vector4.zero;
            rtn.z = dominantcolor;
            if (rtn.z != 0f)
            {
                float num = 0f;
                if (colorone > colortwo)
                {
                    num = colortwo;
                }
                else
                {
                    num = colorone;
                }
                float num2 = rtn.z - num;
                if (num2 != 0f)
                {
                    rtn.y = num2 / rtn.z;
                    rtn.x = offset + (colorone - colortwo) / num2;
                }
                else
                {
                    rtn.y = 0f;
                    rtn.x = offset + (colorone - colortwo);
                }
                rtn.x /= 6f;
                if (rtn.x < 0f)
                {
                    rtn.x += 1f;
                }
                rtn.x *= 360;
            }
            else
            {
                rtn.y = 0f;
                rtn.x = 0f;
            }

            rtn.w = alpha;
            return rtn;
        }
    }
}
