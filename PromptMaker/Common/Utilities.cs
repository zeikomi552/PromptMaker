using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace PromptMaker.Common
{
    public class Utilities
    {
        #region 画像に文字列を埋め込んで移動する処理
        /// <summary>
        /// 画像に文字列を埋め込んで移動する処理
        /// </summary>
        /// <param name="path">文字列を埋め込む元画像</param>
        /// <param name="text">埋め込む文字列</param>
        /// <param name="keyword">文字列を埋め込んだ画像のフォルダ名とファイル名のプレフィクスになります</param>
        public static void SetDetail(string path, string text, string keyword)
        {
            string filename = Path.GetFileName(path);
            string folderPath = System.IO.Path.GetDirectoryName(path)!;
            folderPath = Path.Combine(folderPath, keyword);
            PathManager.CreateDirectory(folderPath);

            using (var bmp = new Bitmap(path))
            using (var fs = new FileStream(Path.Combine(folderPath, keyword + "_" + filename), FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                int resizeWidth = bmp.Width / 2;
                int resizeHeight = bmp.Height / 2;
                using (var resizebmp = new Bitmap(resizeWidth, resizeHeight))
                {
                    using (var g = Graphics.FromImage(resizebmp))
                    {

                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(bmp, 0, 0, resizeWidth, resizeHeight);
                        g.DrawString(text, new Font("Arial", 12), System.Drawing.Brushes.Red, new PointF(10.0f, 10.0f));
                    }

                    fs.SetLength(0);

                    resizebmp.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }
        #endregion

        #region キャンバスの保存処理
        /// <summary>
        /// キャンバスの保存処理
        /// </summary>
        /// <param name="element"></param>
        /// <param name="filePath"></param>
        public static void SaveCanvas(UIElement element, String filePath)
        {
            var bounds = VisualTreeHelper.GetDescendantBounds(element);
            var width = (int)bounds.Width;
            var height = (int)bounds.Height;

            // 描画先
            var drawingVisual = new DrawingVisual();
            using (var ctx = drawingVisual.RenderOpen())
            {
                var vb = new VisualBrush(element);
                ctx.DrawRectangle(System.Windows.Media.Brushes.Black, null, new Rect(new System.Windows.Point(width, height), new System.Windows.Point(width, height)));
                ((InkCanvas)element).Strokes.Draw(ctx);
            }

            // ビットマップに変換
            var rtb = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Pbgra32);
            rtb.Render(drawingVisual);

            // エンコーダー
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            // ファイル保存
            using (var fs = File.Create(filePath))
            {
                encoder.Save(fs);
            }
        }
        #endregion
    }
}
