using MVVMCore.BaseClass;
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
        /// 設定ファイルオブジェクト[SettingConf]プロパティ用変数
        /// </summary>
        ConfigManager<SettingConfM> _SettingConf = new ConfigManager<SettingConfM>("Config", "Setting.conf", new SettingConfM());
        /// <summary>
        /// 設定ファイルオブジェクト[SettingConf]プロパティ
        /// </summary>
        public ConfigManager<SettingConfM> SettingConf
        {
            get
            {
                return _SettingConf;
            }
            set
            {
                if (_SettingConf == null || !_SettingConf.Equals(value))
                {
                    _SettingConf = value;
                    NotifyPropertyChanged("SettingConf");
                }
            }
        }
        #endregion

        #region プロンプト構成要素コンフィグ[PromptComposerConf]プロパティ
        /// <summary>
        /// プロンプト構成要素コンフィグ[PromptComposerConf]プロパティ用変数
        /// </summary>
        ConfigManager<ModelList<PromptConsistM>> _PromptComposerConf = new ConfigManager<ModelList<PromptConsistM>>("Config", "PromptComposer.conf",　new ModelList<PromptConsistM>());
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
                string path = GetOutputFilePath();

                // DirectoryInfoのインスタンスを生成する
                DirectoryInfo di = new DirectoryInfo(path);

                // ディレクトリ直下のすべてのファイル一覧を取得する
                FileInfo[] fiAlls = di.GetFiles();

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
                        Process.Start("explorer.exe", str); // 指定したフォルダを開く
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

        #region 出力先フォルダの取得
        /// <summary>
        /// 出力先フォルダの取得
        /// </summary>
        /// <returns>出力先フォルダ</returns>
        private string GetOutputFilePath()
        {
            string path = Path.Combine(this.SettingConf.Item.CurrentDir, "outputs", "txt2img-samples");
            switch (this.Parameter.ScriptType)
            {
                case ScriptTypeEnum.Txt2Img:
                default:
                    {
                        path = Path.Combine(this.SettingConf.Item.CurrentDir, "outputs", "txt2img-samples");
                        PathManager.CreateDirectory(path);
                        break;
                    }
                case ScriptTypeEnum.Img2Img:
                    {
                        path = Path.Combine(this.SettingConf.Item.CurrentDir, "outputs", "img2img-samples");
                        PathManager.CreateDirectory(path);
                        break;
                    }
                case ScriptTypeEnum.Inpaint:
                    {
                        path = Path.Combine(this.SettingConf.Item.CurrentDir, "outputs", "inpainting-samples");
                        PathManager.CreateDirectory(path);
                        break;
                    }
            }
            return path;
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
                if (this.Parameter.Img2ImgF && string.IsNullOrEmpty(this.Parameter.InitFilePath))
                {
                    ShowMessage.ShowNoticeOK("img2imgはファイルパスが必須です", "通知");
                    return;
                }

                var config = this.SettingConf;

                // Set working directory and create process
                var workingDirectory = Path.GetFullPath("Scripts");

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

                string command = this.Parameter.Command!;   // コマンドの保存
                this.Parameter.CommandBackup = command;     // コマンドの保存

                myProcess.Start();
                using (var sw = myProcess.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        // Vital to activate Anaconda
                        sw.WriteLine(@"C:\ProgramData\Anaconda3\Scripts\activate.bat");
                        // Activate your environment
                        sw.WriteLine("conda activate ldm");
                        // change directory
                        sw.WriteLine($"cd {this.SettingConf.Item.CurrentDir}");
                        // Any other commands you want to run
                        sw.WriteLine(command!);
                    }
                }

                StreamReader myStreamReader = myProcess.StandardOutput;

                string? myString = myStreamReader.ReadLine();
                myProcess.WaitForExit();
                myProcess.Close();

                Console.WriteLine(myString);

                // 出力ファイルパスを取得する
                string path = GetOutputFilePath();

                // DirectoryInfoのインスタンスを生成する
                DirectoryInfo di = new DirectoryInfo(path);

                // ディレクトリ直下のすべてのファイル一覧を取得する
                FileInfo[] fiAlls = di.GetFiles();

                // イメージリストの更新
                RefreshImageList();

                // 出力先ファイルパスを取得し画面表示
                string filepath = fiAlls.Last().FullName;
                this.ImagePath = filepath;
                this.Parameter.OutputFilePath = filepath;

                // ファイルリストの表示
                this.ImagePathList.Items = new System.Collections.ObjectModel.ObservableCollection<FileInfo>(fiAlls);

                // 最初の行に履歴を保存
                this.History.Items.Insert(0,this.Parameter.ShallowCopy<ParameterM>());

                // 最後に実行したプロンプトを保存
                this.SettingConf.Item.LastPrompt = this.Parameter.Prompt;

                // Composerに追加
                SetPromptComposer(this.Parameter.Prompt);

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
                ctx.DrawRectangle(Brushes.Black, null, new Rect(new System.Windows.Point(width, height), new System.Windows.Point(width, height)));
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

        #region プロンプト構成要素に追加
        /// <summary>
        /// プロンプト構成要素に追加
        /// </summary>
        /// <param name="prompt">プロンプト</param>
        private void SetPromptComposer(string prompt)
        {
            string[] composer_items = prompt.Split(",");

            foreach (var composer_item in composer_items)
            {
                var tmp = composer_item.Trim();
                if (!(from x in this.PromptComposerConf.Item.Items
                     where x.Keyword.Equals(tmp)
                     select x).Any())
                {
                    this.PromptComposerConf.Item.Items.Add(new PromptConsistM()
                    { 
                        Keyword　= tmp,
                    });
                }
            }

            // コンポーザーの保存
            this.PromptComposerConf.SaveXML();
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

        #region 行ダブルクリック時にプロンプトに追加する処理
        /// <summary>
        /// 行ダブルクリック時にプロンプトに追加する処理
        /// </summary>
        public void PromptComposerDoubleClick()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.Parameter.Prompt))
                {
                    this.Parameter.Prompt += this.PromptComposerConf.Item.SelectedItem.Keyword;
                }
                else
                {
                    this.Parameter.Prompt += ", " + this.PromptComposerConf.Item.SelectedItem.Keyword;
                }

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

        #region 選択行を削除する処理
        /// <summary>
        /// 選択行を削除する処理
        /// </summary>
        public void DeletePromptComposerRow()
        {
            try
            {
                // 選択行を削除
                if (this.PromptComposerConf.Item.SelectedItem != null)
                {
                    var tmp = (from x in this.PromptComposerConf.Item.Items
                     where x.Equals(this.PromptComposerConf.Item.SelectedItem)
                     select x).First();
                    this.PromptComposerConf.Item.Items.Remove(tmp);
                }

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 下位の行と入れ替え
        /// <summary>
        /// 下位の行と入れ替え
        /// </summary>
        public void MoveUpPromptRow()
        {
            try
            {
                this.PromptComposerConf.Item.MoveUP();
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 上位の行と入れ替え
        /// <summary>
        /// 上位の行と入れ替え
        /// </summary>
        public void MoveDownPromptRow()
        {
            try
            {
                this.PromptComposerConf.Item.MoveDown();
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
