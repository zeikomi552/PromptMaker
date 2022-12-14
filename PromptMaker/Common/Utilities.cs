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
using Pen = System.Drawing.Pen;
using Color = System.Drawing.Color;
using System.Text.RegularExpressions;
using OpenCvSharp;
using Rect = System.Windows.Rect;

namespace PromptMaker.Common
{
    public class Utilities
    {
        #region 00000.pngの最後のファイル番号を取得する
        /// <summary>
        /// 00000.pngの最後のファイル番号を取得する
        /// </summary>
        /// <param name="outdir"></param>
        /// <returns>ファイル番号</returns>
        public static int LastSampleFileNo(string outdir)
        {
            string outdir_path = Path.Combine(outdir, "samples");
            var reg = new Regex("\\\\[0-9]{5}.png");
            var filepath = Directory.GetFiles(outdir_path).Where(f => reg.IsMatch(f)).OrderByDescending(n => n).ToArray().FirstOrDefault();
            var filename = filepath != null ? filepath.Split("\\").Last() : string.Empty;

            if (string.IsNullOrEmpty(filename))
            {
                return 0;
            }
            else
            {
                var filenameNotEx = int.Parse(filename.Replace(".png", ""));
                return filenameNotEx;
            }
        }
        #endregion

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
                    Utilities.BitmapSave(fs, resizebmp);
                }
            }
        }
        #endregion

        #region 画像のリサイズ
        /// <summary>
        /// 画像のリサイズ
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        public static void ResizePic(string path, int width, int height, int retry = 10)
        {
            try
            {
                string filename = Path.GetFileName(path);
                string folderPath = System.IO.Path.GetDirectoryName(path)!;
                PathManager.CreateDirectory(folderPath);

                string tmp_path = Path.Combine(folderPath, "tmp_" + filename);

                using (var bmp = new Bitmap(path))
                using (var fs = new FileStream(tmp_path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (var resizebmp = new Bitmap(width, height))
                    {
                        using (var g = Graphics.FromImage(resizebmp))
                        {

                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.DrawImage(bmp, 0, 0, width, height);
                        }

                        Utilities.BitmapSave(fs, resizebmp);
                    }
                }

                // ファイル名の変更と上書き
                Utilities.FileMove(tmp_path, path, true);
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message + "\r\n" + e.Source, "Error");
            }

        }
        #endregion

        #region キャンバスの保存処理
        /// <summary>
        /// キャンバスの保存処理
        /// </summary>
        /// <param name="element">キャンバス</param>
        /// <param name="filePath">出力先</param>
        public static void SaveCanvas(InkCanvas element, String filePath)
        {
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

        #region Maskファイル作成処理
        /// <summary>
        /// Maskファイル作成処理
        /// </summary>
        /// <param name="path">入力ファイルパス</param>
        /// <param name="x">ずらしたXの移動量px</param>
        /// <param name="y">ずらしたYの移動量px</param>
        /// <param name="z">ずらしたZの移動量px</param>
        public static void SetMask(string path, int x, int y, int z, int offset = 5)
        {
            string filename = Path.GetFileName(path);
            string folderPath = System.IO.Path.GetDirectoryName(path)!;
            //folderPath = Path.Combine(folderPath, keyword);
            PathManager.CreateDirectory(folderPath);
            string tmppath = Path.Combine(folderPath, "tmp-" + filename);

            int x_tmp = x == 0 ? 0 : x + offset;
            int y_tmp = y == 0 ? 0 : y + offset;

            using (var bmp = new Bitmap(path))
            using (var fs = new FileStream(tmppath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var g = Graphics.FromImage(bmp))
                {

                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);

                    if (x_tmp < 0)
                    {
                        for (int ix = bmp.Width; ix >= bmp.Width + x_tmp; ix--)
                        {
                            g.DrawLine(new Pen(Color.White), new System.Drawing.Point(ix, 0), new System.Drawing.Point(ix, bmp.Height));
                        }
                    }
                    else
                    {
                        for (int ix = 0; ix <= x_tmp; ix++)
                        {
                            g.DrawLine(new Pen(Color.White), new System.Drawing.Point(ix, 0), new System.Drawing.Point(ix, bmp.Height));
                        }
                    }

                    if (y_tmp < 0)
                    {
                        for (int iy = bmp.Height; iy >= bmp.Height + y_tmp; iy--)
                        {
                            g.DrawLine(new Pen(Color.White), new System.Drawing.Point(0, iy), new System.Drawing.Point(bmp.Width, iy));
                        }
                    }
                    else
                    {
                        for (int iy = 0; iy <= y_tmp; iy++)
                        {
                            g.DrawLine(new Pen(Color.White), new System.Drawing.Point(0, iy), new System.Drawing.Point(bmp.Width, iy));
                        }
                    }

                    int tz = (z * 2 - x);
                    tz = z <= 0 ? 0 : tz + offset;

                    if (tz >= 0)
                    {
                        for (int iz = bmp.Width; iz >= bmp.Width - tz; iz--)
                        {
                            g.DrawLine(new Pen(Color.White), new System.Drawing.Point(iz, 0), new System.Drawing.Point(iz, bmp.Height));
                        }
                    }

                    tz = (z * 2 - y);
                    tz = z <= 0 ? 0 : tz + offset;

                    if (tz >= 0)
                    {
                        for (int iz = bmp.Height; iz >= bmp.Height - tz; iz--)
                        {
                            g.DrawLine(new Pen(Color.White), new System.Drawing.Point(0, iz), new System.Drawing.Point(bmp.Width, iz));
                        }
                    }

                }

                Utilities.BitmapSave(fs, bmp);
            }

            Utilities.FileMove(tmppath, path, true);

        }
        #endregion

        /// <summary>
        /// 画像の重ね合わせ処理
        /// </summary>
        /// <param name="base_pic">下になる画像</param>
        /// <param name="add_pic">上に重ね合わせる画像</param>
        /// <param name="outfile">出力ファイル</param>
        /// <param name="move_x">Xの移動量</param>
        /// <param name="move_y">Yの移動量</param>
        /// <param name="move_z">Zの移動量(奥行き)</param>
        public static void OverridePic(string base_pic, string add_pic, string outfile, int move_x, int move_y, int move_z)
        {
            string filename = Path.GetFileName(outfile);
            string folderPath = System.IO.Path.GetDirectoryName(outfile)!;
            PathManager.CreateDirectory(folderPath);

            using (var bmp = new Bitmap(add_pic))
            using (var fs = new FileStream(outfile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var outbmp = new Bitmap(base_pic))
                {
                    using (var g = Graphics.FromImage(outbmp))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        int move_x_tmp = move_x > 0 ? move_x : 0;
                        int move_y_tmp = move_y > 0 ? move_y : 0;
                        int x_max = bmp.Width + (move_x - 2 * move_z) > bmp.Width ? bmp.Width : bmp.Width + (move_x - 2 * move_z);
                        int y_max = bmp.Height + (move_y - 2 * move_z) > bmp.Height ? bmp.Height : bmp.Height + (move_y - 2 * move_z);


                        for (int x = move_x_tmp; x < x_max; x++)
                        {
                            for (int y = move_y_tmp; y < y_max; y++)
                            {
                                outbmp.SetPixel(x, y, bmp.GetPixel(x, y));
                            }
                        }
                    }

                    outbmp.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }

        #region 移動処理
        /// <summary>
        /// 移動処理
        /// </summary>
        /// <param name="infile">初期ファイル</param>
        /// <param name="outfile">出力先ファイル</param>
        /// <param name="move_x">X移動量</param>
        /// <param name="move_y">Y移動量</param>
        /// <param name="move_z">Z移動量</param>
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
                    outbmp.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }
        #endregion

        #region 画像を前後へ移動する
        /// <summary>
        /// 画像を前後へ移動する
        /// </summary>
        /// <param name="infile">入力ファイル</param>
        /// <param name="movevalue">移動量</param>
        /// <param name="overwrite">true:元画像に上書き false:上書きしない</param>
        /// <returns>出力先ファイルパス</returns>
        public static string ShiftPos(string infile, int move_x, int move_y, int move_z, bool overwrite = false, int retry = 10)
        {
            string filename = Path.GetFileName(infile);                    // ファイル名の取得
            string folderPath = System.IO.Path.GetDirectoryName(infile)!;  // フォルダパスの取得
            string outfile = Path.Combine(folderPath, "tmp-" + filename);                       // 出力先ファイルパス(一時的)
                                                                                                // ずらして保存する
            Utilities.MovePos(infile, outfile, move_x, move_y, move_z);

            if (overwrite)
            {
                // ずらしたファイルをオリジナル画像に上書きする
                Utilities.FileMove(outfile, infile, true, retry);
                return infile;
            }
            else
            {
                return outfile;
            }
        }
        #endregion

        /// <summary>
        /// ファイル移動処理
        /// </summary>
        /// <param name="source_path">移動元ファイルパス</param>
        /// <param name="dest_path">移動先ファイルパス</param>
        /// <param name="overwrite">上書き</param>
        /// <param name="retry">リトライ回数</param>
        public static void FileMove(string source_path, string dest_path, bool overwrite = true, int retry = 10 )
        {
            int count = 0;
            while (count <= retry)
            {
                try
                {
                    // ファイル名の変更と上書き
                    File.Move(source_path, dest_path, true);
                    break;
                }
                catch
                {
                    if (count == retry)
                        throw;

                    System.Threading.Thread.Sleep(100);
                    count++;
                }
            }
        }

        /// <summary>
        /// ファイルコピー処理
        /// </summary>
        /// <param name="source_path">移動元ファイルパス</param>
        /// <param name="dest_path">移動先ファイルパス</param>
        /// <param name="overwrite">上書き</param>
        /// <param name="retry">リトライ回数</param>
        public static void FileCopy(string source_path, string dest_path, bool overwrite = true, int retry = 10)
        {
            int count = 0;
            while (count <= retry)
            {
                try
                {
                    // ファイル名の変更と上書き
                    File.Copy(source_path, dest_path, true);
                    break;
                }
                catch
                {
                    if (count == retry)
                        throw;

                    System.Threading.Thread.Sleep(100);
                    count++;
                }
            }
        }

        /// <summary>
        /// リトライ回数を設定できるビットマップ保存処理
        /// </summary>
        /// <param name="fs">ファイルストリーム</param>
        /// <param name="outbmp">ビットマップオブジェクト</param>
        /// <param name="retry">リトライ回数[デフォルト3]</param>
        public static void BitmapSave(FileStream fs, Bitmap outbmp, int retry = 10)
        {
            int count = 0;
            while (count <= retry)
            {
                try
                {
                    fs.SetLength(0);
                    outbmp.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                }
                catch
                {
                    if (count == retry)
                        throw;

                    System.Threading.Thread.Sleep(100);
                    count++;
                }
            }
        }

        /// <summary>
        /// グレースケール
        /// </summary>
        /// <param name="fileName">ファイルパス</param>
        /// <returns>ビットマップ値</returns>
        public static void GrayScale(string fileName, string outfile)
        {
            using (Bitmap bitmap = new Bitmap(fileName))
            {
                int w = bitmap.Width;
                int h = bitmap.Height;
                byte[,] data = new byte[w, h];

                // bitmapクラスの画像pixel値を配列に挿入
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        // グレイスケール変換処理
                        data[j, i] = (byte)((bitmap.GetPixel(j, i).R * 0.2126 + bitmap.GetPixel(j, i).B * 0.0722 + bitmap.GetPixel(j, i).G) * 0.7152);///(byte)((bitmap.GetPixel(j, i).R + bitmap.GetPixel(j, i).B + bitmap.GetPixel(j, i).G) / 3);
                        bitmap.SetPixel(j, i, Color.FromArgb(data[j, i], data[j, i], data[j, i]));
                    }
                }

                bitmap.Save(outfile);
            }
        }
        static Random _rand = new Random();
        public static void Binarization(string fileName, string outfile, byte threshold = 128, bool reverse_f = false)
        {
            
            using (Bitmap bitmap = new Bitmap(fileName))
            {
                int w = bitmap.Width;
                int h = bitmap.Height;
                byte[,] data = new byte[w, h];

                // bitmapクラスの画像pixel値を配列に挿入
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        var color = bitmap.GetPixel(j, i);
                        // グレイスケール変換処理
                        data[j, i] = (byte)((color.R * 0.2126 + color.B * 0.0722 + color.G) * 0.7152);



                        if (!reverse_f)
                        {
                            // 閾値による判定
                            if (data[j, i] >= threshold)
                            {
                                bitmap.SetPixel(j, i, Color.FromArgb(0, 255, 255, 255));    // 黒のセット
                            }
                            else
                            {
                                bitmap.SetPixel(j, i, Color.FromArgb(0, 0, 0, 0));          // 白のセット
                            }
                        }
                        else
                        {
                            // 閾値による判定
                            if (data[j, i] >= threshold)
                            {
                                bitmap.SetPixel(j, i, Color.FromArgb(0, 0, 0, 0));          // 白のセット
                            }
                            else
                            {
                                bitmap.SetPixel(j, i, Color.FromArgb(0, 255, 255, 255));    // 黒のセット
                            }
                        }
                    }
                }

                bitmap.Save(outfile);
            }
        }

        public static void RGBNoise(string fileName, string outfile, byte threshold = 128, bool reverse_f = false)
        {
            using (Bitmap bitmap = new Bitmap(fileName))
            {
                int w = bitmap.Width;
                int h = bitmap.Height;
                byte[,] data = new byte[w, h];

                // bitmapクラスの画像pixel値を配列に挿入
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        var color = bitmap.GetPixel(j, i);
                        // グレイスケール変換処理
                        data[j, i] = (byte)((color.R * 0.2126 + color.B * 0.0722 + color.G) * 0.7152);

                        if (!reverse_f)
                        {
                            // 閾値による判定
                            if (data[j, i] >= threshold)
                            {
                                bitmap.SetPixel(j, i, Color.FromArgb(0, 0, 255, 0));    // ノイズのセット
                            }
                            else
                            {
                                bitmap.SetPixel(j, i, color);          // 白のセット
                            }
                        }
                        else
                        {
                            // 閾値による判定
                            if (data[j, i] >= threshold)
                            {
                                bitmap.SetPixel(j, i, color);          // 白のセット
                            }
                            else
                            {
                                bitmap.SetPixel(j, i, Color.FromArgb(0, 0, 255, 0));    // ノイズのセット
                            }
                        }
                    }
                }

                bitmap.Save(outfile);
            }
        }

        /// <summary>
        /// チャネル入れ替え
        /// </summary>
        /// <param name="fileName">ファイルパス</param>
        /// <returns>ビットマップ値</returns>
        public static void ChanelChangeBGR(string fileName, string outfile)
        {
            using (Bitmap bitmap = new Bitmap(fileName))
            {
                int w = bitmap.Width;
                int h = bitmap.Height;
                byte[,] data = new byte[w, h];

                // bitmapクラスの画像pixel値を配列に挿入
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        bitmap.SetPixel(j, i, Color.FromArgb(bitmap.GetPixel(j, i).G, bitmap.GetPixel(j, i).B, bitmap.GetPixel(j, i).R));
                    }
                }
                bitmap.Save(outfile);
            }
        }

        /// <summary>
        /// 明るさの変更
        /// </summary>
        /// <param name="bmp">ファイル名</param>
        /// <param name="bright">明るさ</param>
        /// <returns>ビットマップ</returns>
        public static Bitmap BrightnessChange(string fileName, int bright)
        {
            using (Bitmap bmp = new Bitmap(fileName))
            {
                // 画像データの幅と高さを取得
                int w = bmp.Width;
                int h = bmp.Height;
                byte[,] data = new byte[w, h];
                byte[,] brightdata = new byte[w, h];

                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        // bmpのデータを取得
                        data[j, i] = (byte)((bmp.GetPixel(j, i).R + bmp.GetPixel(j, i).B + bmp.GetPixel(j, i).G) / 3);

                        // 明るさ変更処理
                        if ((int)data[j, i] + bright >= 256)
                        {
                            brightdata[j, i] = 255;
                        }
                        else if ((int)data[j, i] + bright < 0)
                        {
                            brightdata[j, i] = 0;
                        }
                        else
                        {
                            brightdata[j, i] = (byte)(data[j, i] + bright);
                        }
                        // bmpに設定
                        bmp.SetPixel(j, i, Color.FromArgb(brightdata[j, i], brightdata[j, i], brightdata[j, i]));
                    }
                }
                return bmp;
            }

                
        }
    }
}
