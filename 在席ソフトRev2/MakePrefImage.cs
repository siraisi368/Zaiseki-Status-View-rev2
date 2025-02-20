using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Drawing.Drawing2D;

namespace 在席ソフトRev2
{
    public class MakePrefImage
    {
        public const int IMAGE_SIZE = 40;
        public static Image MakePrefImageFromGeoJson(string filepath, int prefnum)
        {

            var geoJson = JObject.Parse(File.ReadAllText(filepath));

            Bitmap bitmap = new Bitmap(40, 40);
            Graphics g = Graphics.FromImage(bitmap);

            double minX = double.MaxValue, maxX = double.MinValue;
            double minY = double.MaxValue, maxY = double.MinValue;

            foreach (var feature in geoJson["features"])
            {
                int pref = (int)feature["properties"]["pref"];
                if (pref == prefnum)
                {
                    var geometry = feature["geometry"];
                    var polygons = geometry["type"].ToString() == "Polygon" ?
                                   new List<JToken> { geometry["coordinates"] } :
                                   geometry["coordinates"].ToList();

                    foreach (var polygon in polygons)
                    {
                        foreach (var ring in polygon)
                        {
                            foreach (var coord in ring)
                            {
                                double x = (double)coord[0];
                                double y = (double)coord[1];
                                minX = Math.Min(minX, x);
                                maxX = Math.Max(maxX, x);
                                minY = Math.Min(minY, y);
                                maxY = Math.Max(maxY, y);
                            }
                        }
                    }
                }
            }

            double scaleX = IMAGE_SIZE / (maxX - minX);
            double scaleY = IMAGE_SIZE / (maxY - minY);
            double scale = Math.Min(scaleX, scaleY); // 比率を揃えるため小さい方を使用
            double offsetX = minX;
            double offsetY = minY;

            // 描画開始
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias; // アンチエイリアスで滑らかに

            Pen borderPen = new Pen(Color.LightGray, 1);
            Brush fillBrush = new SolidBrush(Color.Green); // 塗りつぶし色

            // 指定した都道府県を検索して描画
            foreach (var feature in geoJson["features"])
            {
                int pref = (int)feature["properties"]["pref"];
                if (pref == prefnum)
                {
                    var geometry = feature["geometry"];
                    var polygons = geometry["type"].ToString() == "Polygon" ?
                                   new List<JToken> { geometry["coordinates"] } :
                                   geometry["coordinates"].ToList();

                    foreach (var polygon in polygons)
                    {
                        DrawPolygon(g, fillBrush, borderPen, polygon, scale, offsetX, offsetY);
                    }
                }
            }
            return bitmap;
        }

        private static void DrawPolygon(Graphics g, Brush fillBrush, Pen borderPen, JToken coordinates, double scale, double offsetX, double offsetY)
        {
            foreach (var ring in coordinates)
            {
                List<PointF> points = new List<PointF>();
                foreach (var coord in ring)
                {
                    float x = (float)((((double)coord[0] - offsetX) * scale));
                    float y = (float)((((double)coord[1] - offsetY) * scale));

                    // Y座標を反転（画像座標系は左上が原点のため）
                    y = IMAGE_SIZE - y;
                    points.Add(new PointF(x, y));
                }

                if (points.Count > 2) // 三角形以上なら描画
                {
                    g.FillPolygon(fillBrush, points.ToArray()); // 塗りつぶし
                    g.DrawPolygon(borderPen, points.ToArray()); // 境界線
                }
            }
        }
    }
}
