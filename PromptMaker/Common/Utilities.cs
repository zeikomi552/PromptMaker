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
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Input;
using System.Windows.Shapes;
using Path = System.IO.Path;
using System.IO.Compression;
using System.Runtime.CompilerServices;

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
        public static void SaveCanvas(InkCanvas element, String filePath)
        {
            //var bounds = VisualTreeHelper.GetDescendantBounds(element);
            var width = (int)element.ActualWidth;
            var height = (int)element.ActualHeight;

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


        #region キャンバスの保存処理
        /// <summary>
        /// Imageをずらして保存
        /// </summary>
        /// <param name="infile">元ファイル</param>
        /// <param name="outfile">出力先ファイル</param>
        /// <param name="shift_px_x">X座標のシフト分</param>
        /// <param name="shift_px_y">Y座標のシフト分</param>
        public static void ShiftSaveImage(string infile, string outfile, int shift_px_x, int shift_px_y)
        {
            string filename = Path.GetFileName(outfile);
            string folderPath = System.IO.Path.GetDirectoryName(outfile)!;
            PathManager.CreateDirectory(folderPath);

            using (var bmp = new Bitmap(infile))
            using (var fs = new FileStream(outfile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var outbmp = new Bitmap(bmp.Width, bmp.Height))
                {
                    using (var g = Graphics.FromImage(outbmp))
                    {

                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(bmp, shift_px_x, shift_px_y);
                    }

                    fs.SetLength(0);
                    outbmp.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }
        #endregion

        public static void MovePos(string infile, string outfile, int move_x, int move_y, int move_z)
        {
            string filename = Path.GetFileName(outfile);
            string folderPath = System.IO.Path.GetDirectoryName(outfile)!;
            PathManager.CreateDirectory(folderPath);

            using (var bmp = new Bitmap(infile))
            using (var fs = new FileStream(outfile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var outbmp = new Bitmap(bmp.Width, bmp.Height))
                {
                    using (var g = Graphics.FromImage(outbmp))
                    {

                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(bmp, move_x, move_y, bmp.Width - (move_z * 2), bmp.Height - (move_z * 2));
                    }

                    fs.SetLength(0);
                    outbmp.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        ///// <summary>
        ///// 画像のシフト
        ///// </summary>
        ///// <param name="infile">入力画像</param>
        ///// <param name="shift_x">X座標シフト量</param>
        ///// <param name="shift_y">Y座標シフト量</param>
        ///// <param name="overwrite">true:元画像に上書き false:上書きしない</param>
        ///// <returns>出力先ファイルパス</returns>
        //public static string ShiftPic(string infile, int shift_x, int shift_y, bool overwrite = false)
        //{
        //    string filename = Path.GetFileName(infile);                    // ファイル名の取得
        //    string folderPath = System.IO.Path.GetDirectoryName(infile)!;  // フォルダパスの取得
        //    string outfile = Path.Combine(folderPath, "tmp-" + filename);                       // 出力先ファイルパス(一時的)

        //    // ずらして保存する
        //    Utilities.ShiftSaveImage(infile, outfile, shift_x, shift_y);

        //    if (overwrite)
        //    {
        //        // ずらしたファイルをオリジナル画像に上書きする
        //        File.Move(outfile, infile, true);
        //        return infile;
        //    }
        //    else
        //    {
        //        return outfile;
        //    }
        //}

        /// <summary>
        /// 画像を前後へ移動する
        /// </summary>
        /// <param name="infile">入力ファイル</param>
        /// <param name="movevalue">移動量</param>
        /// <param name="overwrite">true:元画像に上書き false:上書きしない</param>
        /// <returns>出力先ファイルパス</returns>
        public static string ShiftPos(string infile, int move_x, int move_y, int move_z, bool overwrite = false)
        {
            string filename = Path.GetFileName(infile);                    // ファイル名の取得
            string folderPath = System.IO.Path.GetDirectoryName(infile)!;  // フォルダパスの取得
            string outfile = Path.Combine(folderPath, "tmp-" + filename);                       // 出力先ファイルパス(一時的)
            //string infile = this.Parameter.InitFilePath;                                        // 入力ファイルパス

            // ずらして保存する
            Utilities.MovePos(infile, outfile, move_x, move_y, move_z);

            if (overwrite)
            {
                // ずらしたファイルをオリジナル画像に上書きする
                File.Move(outfile, infile, true);
                return infile;
            }
            else
            {
                return outfile;
            }
        }

    }
}
