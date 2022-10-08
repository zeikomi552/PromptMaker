﻿using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using PromptMaker.Common;
using PromptMaker.Common.Extensions;
using PromptMaker.Models;
using PromptMaker.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using Path = System.IO.Path;
using System.Xml.Linq;
using MVVMCore.Common.Wrapper;
using System.Reflection;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.TextFormatting;
using System.Windows.Input;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;

namespace PromptMaker.ViewModels
{
    internal class MainWindowVM : ViewModelBase
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowVM()
        {
            Dictionary<ScriptTypeEnum, string> tmp = new Dictionary<ScriptTypeEnum, string>();
            foreach (var value in Enum.GetValues(typeof(ScriptTypeEnum)))
            {
                tmp.Add((ScriptTypeEnum)value, ScriptTypeEnumExtensions.GetName((ScriptTypeEnum)value)!);
            }

            this.ScriptTypeEnumEx = tmp;
        }
        #endregion

        #region Enum用変数[ScriptTypeEnumEx]プロパティ
        /// <summary>
        /// Enum用変数[ScriptTypeEnumEx]プロパティ用変数
        /// </summary>
        Dictionary<ScriptTypeEnum, string> _ScriptTypeEnumEx = new Dictionary<ScriptTypeEnum, string>();
        /// <summary>
        /// Enum用変数[ScriptTypeEnumEx]プロパティ
        /// </summary>
        public Dictionary<ScriptTypeEnum, string> ScriptTypeEnumEx
        {
            get
            {
                return _ScriptTypeEnumEx;
            }
            set
            {
                if (_ScriptTypeEnumEx == null || !_ScriptTypeEnumEx.Equals(value))
                {
                    _ScriptTypeEnumEx = value;
                    NotifyPropertyChanged("ScriptTypeEnumEx");
                }
            }
        }
        #endregion

        #region 共通変数
        /// <summary>
        /// 共通変数
        /// </summary>
        public GBLValues CommonValues
        {
            get
            {
                return GBLValues.GetInstance();
            }
        }
        #endregion

        #region 設定ファイルオブジェクト[SettingConf]プロパティ
        /// <summary>
        /// 設定ファイルオブジェクト[SettingConf]プロパティ
        /// </summary>
        public ConfigManager<SettingConfM> SettingConf
        {
            get
            {
                return GBLValues.GetInstance().SettingConf;
            }
            set
            {
                if (GBLValues.GetInstance().SettingConf == null || !GBLValues.GetInstance().SettingConf.Equals(value))
                {
                    GBLValues.GetInstance().SettingConf = value;
                    NotifyPropertyChanged("SettingConf");
                }
            }
        }
        #endregion

        #region プロンプト構成要素コンフィグ[PromptComposerConf]プロパティ
        /// <summary>
        /// プロンプト構成要素コンフィグ[PromptComposerConf]プロパティ用変数
        /// </summary>
        ConfigManager<ModelList<PromptConsistM>> _PromptComposerConf = new ConfigManager<ModelList<PromptConsistM>>("Config", "PromptComposer.conf", new ModelList<PromptConsistM>());
        /// <summary>
        /// プロンプト構成要素コンフィグ[PromptComposerConf]プロパティ
        /// </summary>
        public ConfigManager<ModelList<PromptConsistM>> PromptComposerConf
        {
            get
            {
                return _PromptComposerConf;
            }
            set
            {
                if (_PromptComposerConf == null || !_PromptComposerConf.Equals(value))
                {
                    _PromptComposerConf = value;
                    NotifyPropertyChanged("PromptComposerConf");
                }
            }
        }
        #endregion

        #region Stable Diffusionパラメータ[Parameter]プロパティ
        /// <summary>
        /// Stable Diffusionパラメータ[Parameter]プロパティ用変数
        /// </summary>
        ParameterM _Parameter = new ParameterM();
        /// <summary>
        /// Stable Diffusionパラメータ[Parameter]プロパティ
        /// </summary>
        public ParameterM Parameter
        {
            get
            {
                return _Parameter;
            }
            set
            {
                if (_Parameter == null || !_Parameter.Equals(value))
                {
                    _Parameter = value;
                    NotifyPropertyChanged("Parameter");
                }
            }
        }
        #endregion

        #region 履歴[History]プロパティ
        /// <summary>
        /// 履歴[History]プロパティ用変数
        /// </summary>
        ModelList<ParameterM> _History = new ModelList<ParameterM>();
        /// <summary>
        /// 履歴[History]プロパティ
        /// </summary>
        public ModelList<ParameterM> History
        {
            get
            {
                return _History;
            }
            set
            {
                if (_History == null || !_History.Equals(value))
                {
                    _History = value;
                    NotifyPropertyChanged("History");
                }
            }
        }
        #endregion

        #region ワードクラウド画像パス[ImagePath]プロパティ
        /// <summary>
        /// ワードクラウド画像パス[ImagePath]プロパティ用変数
        /// </summary>
        string _ImagePath = string.Empty;
        /// <summary>
        /// ワードクラウド画像パス[ImagePath]プロパティ
        /// </summary>
        public string ImagePath
        {
            get
            {
                return _ImagePath;
            }
            set
            {
                _ImagePath = value;
                NotifyPropertyChanged("ImagePath");
            }
        }
        #endregion

        #region イメージ画像リスト[ImagePathList]プロパティ
        /// <summary>
        /// イメージ画像リスト[ImagePathList]プロパティ用変数
        /// </summary>
        ModelList<FileInfo> _ImagePathList = new ModelList<FileInfo>();
        /// <summary>
        /// イメージ画像リスト[ImagePathList]プロパティ
        /// </summary>
        public ModelList<FileInfo> ImagePathList
        {
            get
            {
                return _ImagePathList;
            }
            set
            {
                if (_ImagePathList == null || !_ImagePathList.Equals(value))
                {
                    _ImagePathList = value;
                    NotifyPropertyChanged("ImagePathList");
                }
            }
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Init(object sender, EventArgs e)
        {
            try
            {
                this.SettingConf.LoadXML();
                this.PromptComposerConf.LoadXML();

                RefreshImageList();

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region イメージリストの更新
        /// <summary>
        /// イメージリストの更新
        /// </summary>
        private void RefreshImageList()
        {
            Task.Run(()=>{
                // ディレクトリパス
                string path = this.Parameter.GetOutputFilePath();

                // サンプルフォルダ配下
                path = Path.Combine(path, "samples");

                PathManager.CreateDirectory(path);

                // DirectoryInfoのインスタンスを生成する
                DirectoryInfo di = new DirectoryInfo(path);

                // ディレクトリ直下のすべてのファイル一覧を取得する
                FileInfo[] fiAlls = di.GetFiles("*.png");

                if (fiAlls.Length > 0)
                {
                    // 出力先ファイルパスを取得し画面表示
                    this.Parameter.OutputFilePath = this.ImagePath = fiAlls.Last().FullName;

                    // ファイルリストの表示
                    this.ImagePathList.Items = new System.Collections.ObjectModel.ObservableCollection<FileInfo>(fiAlls);
                }
            });
        }
        #endregion

        #region 設定画面を開く
        /// <summary>
        /// 設定画面を開く
        /// </summary>
        public void OpenSettingV()
        {
            try
            {
                SettingV wnd = new SettingV();

                if (wnd.ShowDialog() == true)
                {
                }
                this.SettingConf.LoadXML();

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region カード編集画面を開く
        /// <summary>
        /// カード編集画面を開く
        /// </summary>
        public void OpenCardV()
        {
            try
            {
                CardV wnd = new CardV();

                if (wnd.ShowDialog() == true)
                {

                }

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region フォルダを開く処理
        /// <summary>
        /// フォルダを開く処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        public void FolderOpen(object sender, EventArgs ev)
        {
            try
            {
                var tmp = sender as MenuItem;
                if (tmp != null)
                {
                    var file_info = tmp.DataContext as FileInfo;

                    if (file_info != null)
                    {
                        string str = Path.GetDirectoryName(file_info.FullName)!;
                        Process.Start("explorer.exe", string.Format(@"/select,""{0}", file_info.FullName)); // エクスプローラを開く
                    }
                }

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region ファイルを開く処理
        /// <summary>
        /// ファイルを開く処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        public void FileOpen(object sender, EventArgs ev)
        {
            try
            {
                var tmp = sender as MenuItem;
                if (tmp != null)
                {
                    var file_info = tmp.DataContext as FileInfo;

                    if (file_info != null)
                    {
                        Process.Start("mspaint", file_info.FullName); // 指定したフォルダを開く
                    }
                }

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region RealESRGANの保存先を開くダイアログ
        /// <summary>
        /// RealESRGANの保存先を開くダイアログ
        /// </summary>
        public void SaveFilePathOpen()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new SaveFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "PNGファイル (*.png)|*.png";

                // 実行ファイルの存在確認
                if(!File.Exists(this.SettingConf.Item.RealEsrganExePath))
                {
                    ShowMessage.ShowNoticeOK("Real-ESRGANの実行ファイルが見つかりませんでした。\r\n設定画面からReal-ESRGANのパス設定をしてください", "通知");
                    return;
                }

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    var myProcess = new Process
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            FileName = "cmd.exe",
                            RedirectStandardInput = true,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            WorkingDirectory = this.SettingConf.Item.CurrentDir
                        }
                    };

                    myProcess.Start();
                    using (var sw = myProcess.StandardInput)
                    {
                        if (sw.BaseStream.CanWrite)
                        {
                            sw.WriteLine($"\"{this.SettingConf.Item.RealEsrganExePath}\" -i \"{this.ImagePathList.SelectedItem}\" -o \"{dialog.FileName}\"");
                        }
                    }

                    StreamReader myStreamReader = myProcess.StandardOutput;

                    string? myString = myStreamReader.ReadLine();
                    myProcess.WaitForExit();
                    myProcess.Close();
                }

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region ファイル削除処理
        /// <summary>
        /// ファイル削除処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        public void FileDelete(object sender, EventArgs ev)
        {
            try
            {
                var tmp = sender as MenuItem;
                if (tmp != null)
                {
                    var file_info = tmp.DataContext as FileInfo;

                    if (file_info != null)
                    {
                        var index = this.ImagePathList.Items.IndexOf(file_info);
                        this.ImagePathList.Items.Remove(file_info);
                        FileSystem.DeleteFile(file_info.FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                        if (index >= 1)
                        {
                            this.ImagePath = this.ImagePathList.ElementAt(index - 1).FullName;
                        }
                        else
                        {
                            this.ImagePath = String.Empty;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region Img2Imgの初期化フォルダに移動
        /// <summary>
        /// Img2Imgの初期化フォルダに移動
        /// </summary>
        public void MoveImg2Img()
        {
            try
            {
                this.Parameter.SetInitFile(this.ImagePathList.SelectedItem.FullName);
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 画像パスの選択変更
        /// <summary>
        /// 画像パスの選択変更
        /// </summary>
        public void ImagePathSelectionChanged()
        {
            try
            {
                // nullチェック
                if (this.ImagePathList.SelectedItem != null)
                {
                    this.ImagePath = this.ImagePathList.SelectedItem.FullName;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region コマンドの実行
        /// <summary>
        /// コマンドの実行
        /// </summary>
        public void Execute(object sender, EventArgs ev)
        {
            try
            {
                string prompt_bk = this.Parameter.Prompt;

                // プロンプトを改行で分割する
                string[] prompt_list = prompt_bk.Split("\r\n");

                // 親ディレクトリ
                var parent_dir = this.Parameter.GetOutputFilePath();
                var path = Path.Combine(parent_dir, this.Parameter.Prefix);

                // 繰り返し回数指定
                for (int cnt = 0; cnt < this.Parameter.Repeat; cnt++)
                {
                    // プロンプト補助リスト
                    foreach (var composer in this.PromptComposerConf.Item.Items)
                    {
                        // 有効なもののみ実行
                        if (composer.IsEnable)
                        {
                            foreach (var prompt in prompt_list)
                            {
                                this.Parameter.Prompt = prompt + " " + composer.Prompt;
                                ExecuteSub(sender, ev);
                            }
                        }
                    }
                }

                this.Parameter.Prompt = prompt_bk;
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region サブ関数
        /// <summary>
        /// サブ関数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        /// <param name="debug_f"></param>
        /// <param name="msg"></param>
        /// <param name="keyword">文字列挿入画像のフォルダ名とファイルプレフィスク</param>
        private void ExecuteSub(object sender, EventArgs ev)
        {
            try
            {
                if (this.Parameter.Img2ImgF && string.IsNullOrEmpty(this.Parameter.InitFilePath))
                {
                    ShowMessage.ShowNoticeOK("img2imgはファイルパスが必須です", "通知");
                    return;
                }

                // Inpaintingの場合のみ事前にファイルを作成する
                if (this.Parameter.InpaintF)
                {
                    // ファイルコピー
                    File.Copy(this.Parameter.InitFilePath, Path.Combine(this.SettingConf.Item.CurrentDir, "inputs", "inpainting", "example.png"), true);

                    var wnd = VisualTreeHelperWrapper.FindAncestor<Window>((Button)sender) as MainWindow;

                    if (wnd != null)
                    {
                        var canvas = wnd!.inkCanvas;
                        SaveCanvas(canvas, Path.Combine(this.SettingConf.Item.CurrentDir, "inputs", "inpainting", "example_mask.png"));
                    }
                }

                string path = this.Parameter.Outdir;
                if (string.IsNullOrEmpty((this.Parameter.Outdir)))
                {
                    path = Path.Combine(this.SettingConf.Item.CurrentDir, "outputs", "txt2img-samples"); ;
                }

                // テキストファイル出力（新規作成）
                using (StreamWriter sw = new StreamWriter(Path.Combine(path, $"{DateTime.Today.ToString("yyyy-MM-dd")}.txt"), true))
                {
                    sw.WriteLine($"--------");
                    sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                    sw.WriteLine($"Prompt->{this.Parameter.Prompt}");
                    sw.WriteLine($"Command->{this.Parameter.Command}");
                }

                // コマンドの実行処理
                this.Parameter.CommandExecute();

                // イメージリストの更新
                RefreshImageList();
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
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
        private void SetDetail(string path, string text, string keyword)
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

        #region 出力先ディレクトリを選択する
        /// <summary>
        /// 出力先ディレクトリを選択する
        /// </summary>
        public void OpenOutDir()
        {
            try
            {
                // 出力先の変更
                this.Parameter.OpenOutDir();

                // イメージリストの更新
                RefreshImageList();
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region キャンバスの保存処理
        /// <summary>
        /// キャンバスの保存処理
        /// </summary>
        /// <param name="element"></param>
        /// <param name="filePath"></param>
        private static void SaveCanvas(UIElement element, String filePath)
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

        #region InkCanvasのオブジェクトを削除
        /// <summary>
        /// InkCanvasのオブジェクトを削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        public void ClearObject(object sender, EventArgs ev)
        {
            try
            {
                var wnd = VisualTreeHelperWrapper.FindAncestor<Window>((Button)sender) as MainWindow;

                if (wnd != null)
                {
                    var canvas = wnd!.inkCanvas;
                    canvas.Strokes.Clear();
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 選択行の変更
        /// <summary>
        /// 選択行の変更
        /// </summary>
        public void SelectionChanged()
        {
            try
            {
                this.ImagePath = this.History.SelectedItem.OutputFilePath;

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 履歴のグリッドダブルクリック時に対象の画像ファイルを開く
        /// <summary>
        /// 履歴のグリッドダブルクリック時に対象の画像ファイルを開く
        /// </summary>
        public void GridDoubleClick()
        {
            try
            {
                Process.Start("mspaint", this.History.SelectedItem.OutputFilePath); // 指定したフォルダを開く
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 閉じる
        /// <summary>
        /// 閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Close(object sender, EventArgs e)
        {
            try
            {
                this.PromptComposerConf.SaveXML();

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion
    }
}
