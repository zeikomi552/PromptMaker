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
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.TextFormatting;
using System.Windows.Input;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System.Windows.Threading;
using Application = System.Windows.Application;
using static System.Net.WebRequestMethods;
using System.Text.RegularExpressions;
using File = System.IO.File;
using System.Security.Cryptography;
using System.Windows.Ink;
using MahApps.Metro.Converters;
using System.Threading;
using System.IO.Compression;
using Image = System.Drawing.Image;

namespace PromptMaker.ViewModels
{
    public class MainWindowVM : ViewModelBase
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

        #region イメージリスト[ImageList]プロパティ
        /// <summary>
        /// イメージリスト[ImageList]プロパティ用変数
        /// </summary>
        ImagePathCollectionM _ImageList = new ImagePathCollectionM();
        /// <summary>
        /// イメージリスト[ImageList]プロパティ
        /// </summary>
        public ImagePathCollectionM ImageList
        {
            get
            {
                return _ImageList;
            }
            set
            {
                if (_ImageList == null || !_ImageList.Equals(value))
                {
                    _ImageList = value;
                    NotifyPropertyChanged("ImageList");
                }
            }
        }
        #endregion

        #region 実行中フラグ[ExecuteF]プロパティ
        /// <summary>
        /// 実行中フラグ[ExecuteF]プロパティ用変数
        /// </summary>
        bool _ExecuteF = false;
        /// <summary>
        /// 実行中フラグ[ExecuteF]プロパティ
        /// </summary>
        public bool ExecuteF
        {
            get
            {
                return _ExecuteF;
            }
            set
            {
                if (!_ExecuteF.Equals(value))
                {
                    _ExecuteF = value;
                    NotifyPropertyChanged("ExecuteF");
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
                this._Parameter.Parent = this;  // 親データを保持

                this.SettingConf.LoadXML();
                this.PromptComposerConf.LoadXML();
                this.Parameter.Outdir = Path.Combine(this.SettingConf.Item.CurrentDir, "outputs");

                this.Parameter.ShiftPic.Initialize(this.Parameter);   // パラメータの保存

                var wnd = VisualTreeHelperWrapper.FindAncestor<Window>((Grid)sender) as MainWindow;

                // nullチェック
                if (wnd != null)
                {
                    this.Parameter.InkCanvasStroke = wnd.inkCanvas.Strokes;
                }


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
        public void RefreshImageList()
        {
            Task.Run(() =>
            {
                // ディレクトリパス
                string path = this.Parameter.Outdir;

                // サンプルフォルダ配下
                path = Path.Combine(path, "samples");

                PathManager.CreateDirectory(path);

                // DirectoryInfoのインスタンスを生成する
                DirectoryInfo di = new DirectoryInfo(path);

                // ディレクトリ直下のすべてのファイル一覧を取得する
                FileInfo[] fiAlls = di.GetFiles("*.png");

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                   new Action(() =>
                   {
                       this.ImageList.ImagePathList.Items.Clear();

                       if (fiAlls.Length > 0)
                       {
                           // 出力先ファイルパスを取得し画面表示
                           this.Parameter.OutputFilePath = fiAlls.Last().FullName;
                           this.ImageList.ImagePathList.SelectedItem = fiAlls.Last();

                           // ファイルリストの表示
                           this.ImageList.ImagePathList.Items = new System.Collections.ObjectModel.ObservableCollection<FileInfo>(fiAlls);
                       }
                   }));
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

        #region ESRGANの実行処理
        public void ExecuteRealESRGAN()
        {
            try
            {
                if (this.ImageList.ImagePathList.SelectedItem != null && File.Exists(this.ImageList.ImagePathList.SelectedItem.FullName))
                {
                    ExecuteRealESRGAN(this.ImageList.ImagePathList.SelectedItem.FullName);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }

        /// <summary>
        /// ESRGANの実行処理
        /// </summary>
        public void ExecuteRealESRGAN(string img_path)
        {
            try
            {
                this.Parameter.ExecuteRealESRGAN(img_path);
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region GFPGANの実行処理
        /// <summary>
        /// GFPGANの実行処理
        /// </summary>
        public void ExecuteGFPGAN()
        {
            try
            {
                if (this.ImageList.ImagePathList.SelectedItem != null && File.Exists(this.ImageList.ImagePathList.SelectedItem.FullName))
                {
                    this.Parameter.ExecuteGFPGAN(this.ImageList.ImagePathList.SelectedItem.FullName);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region ファイルの削除処理
        /// <summary>
        /// ファイルの削除処理
        /// </summary>
        private void DeleteFile()
        {
            // DirectoryInfoのインスタンスを生成する
            DirectoryInfo di = new DirectoryInfo(Path.Combine(this.Parameter.Outdir, "samples"));

            // ディレクトリ直下のすべてのファイル一覧を取得する
            var fiAlls = di.GetFiles("*.png").ToList<FileInfo>();

            if(this.Parameter.ScriptType == ScriptTypeEnum.Img2Img)
            {
                string file = fiAlls.ElementAt(fiAlls.Count - 1).FullName;
                File.Delete(file);
                file = fiAlls.ElementAt(fiAlls.Count - 2).FullName;
                File.Delete(file);
            }
        }
        #endregion


        private void AutoTestForMoveBackward(object sender, EventArgs ev, int retry =10)
        {

            // プロンプトの有効数確認
            var enable_prompt_count = (from x in this.PromptComposerConf.Item.Items
                       where x.IsEnable == true
                       select x).Count();

            // 有効になってるものが無い場合処理を実行しない
            if(enable_prompt_count <= 0)
                return;

            if (this.ImageList.ImagePathList.SelectedItem != null && File.Exists(this.ImageList.ImagePathList.SelectedItem.FullName))
            {
                string img_path = this.ImageList.ImagePathList.SelectedItem.FullName;

                // 初期ファイルの設定
                this.Parameter.SetInitFile(img_path);


                // 指定場所に移動
                this.Parameter.ShiftPic.ShiftMove();

                string last = this.ImageList.GetLastFilePath(this.Parameter.Outdir);

                // StableDiffusion実行
                this.Parameter.Execute(sender, ev);

                // 不要ファイルを削除
                DeleteFile();

                // 出力先ファイルパスを取得し画面表示
                this.Parameter.OutputFilePath = img_path = this.ImageList.GetLastFilePath(this.Parameter.Outdir);

                // 初期ファイルの設定
                this.Parameter.SetInitFile(img_path);

                // 出力先ファイルパスを取得し画面表示
                this.Parameter.OutputFilePath = img_path = this.ImageList.GetLastFilePath(this.Parameter.Outdir);

                // 初期ファイルの設定
                this.Parameter.SetInitFile(img_path);

                // サイズ縮小
                Utilities.ResizePic(img_path, this.Parameter.Width, this.Parameter.Height);

                // 出力先ファイルパスを取得し画面表示
                this.Parameter.OutputFilePath = img_path = this.ImageList.GetLastFilePath(this.Parameter.Outdir);

                // 初期ファイルの設定
                this.Parameter.SetInitFile(img_path);

                // 最後の画像をセット
                this.ImageList.ImagePathList.SelectedItem = this.ImageList.GetLastFileInfo(this.Parameter.Outdir);

                // イメージリストの更新
                RefreshImageList();
            }
        }

        /// <summary>
        /// プロンプトの変更処理
        /// </summary>
        private void SetRandomPrompt(int index)
        {
            for (int i = 0; i < this.PromptComposerConf.Item.Items.Count; i++)
            {
                var elem = this.PromptComposerConf.Item.Items.ElementAt(i);
                elem.IsEnable = false;
            }

            if (this.PromptComposerConf.Item.Items.Count > 0)
            {
                this.PromptComposerConf.Item.Items.ElementAt(index).IsEnable = true;
            }
        }

        Random _rand = new Random();

        #region 自動テスト
        /// <summary>
        /// 自動テスト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        public void AutoTest(object sender, EventArgs ev)
        {
            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        this.Parameter.N_iter = 1;

                        while(this.ExecuteF)
                        {
                            AutoTestForMoveBackward(sender, ev);
                        }

                        // 実行フラグのOFF
                        this.ExecuteF = false;
                    }
                    catch (Exception ex2)
                    {
                        ShowMessage.ShowErrorOK(ex2.Message, "Error");
                    }
                });
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

                    // nullチェック
                    if (file_info != null)
                    {
                        // 要素の削除
                        this.ImageList.RemoveAt(file_info);

                        // 最後の要素を選択
                        this.ImageList.LastElementSelect();
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
                this.Parameter.SetInitFile(this.ImageList.ImagePathList.SelectedItem.FullName);
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region プロンプトリストの保存処理
        /// <summary>
        /// プロンプトリストの保存処理
        /// </summary>
        public void SavePromptList()
        {
            try
            {
                // 保存処理
                this.PromptComposerConf.SaveXML("プロンプト構成要素ファイル (*.promptxml)|*.promptxml");
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region プロンプトリストの読み込み処理
        /// <summary>
        /// プロンプトリストの読み込み処理
        /// </summary>
        public void LoadPromptList()
        {
            try
            {
                // ロード処理
                this.PromptComposerConf.LoadXML("プロンプト構成要素ファイル (*.promptxml)|*.promptxml");
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region URLへ移動
        /// <summary>
        /// URLへ移動
        /// </summary>
        public void MoveURL()
        {
            try
            {
                if (this.PromptComposerConf != null && this.PromptComposerConf.Item.SelectedItem != null)
                {
                    string url = string.Empty;
                    // URLが設定されている
                    if (!string.IsNullOrEmpty(this.PromptComposerConf.Item.SelectedItem.URL))
                    {
                        url = this.PromptComposerConf.Item.SelectedItem.URL;
                    }
                    // URLが未設定
                    else
                    {
                        url = $"https://lexica.art/?q={this.PromptComposerConf.Item.SelectedItem.Prompt}";
                    }


                    ProcessStartInfo pi = new ProcessStartInfo()
                    {
                        FileName = url,
                        UseShellExecute = true,
                    };

                    Process.Start(pi);
                }

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region プロンプトメーカーの起動
        /// <summary>
        /// プロンプトメーカーの起動
        /// </summary>
        public void OpenPromptMaker()
        {
            try
            {
                var wnd = new PromptMakerV();
                var vm = wnd.DataContext as PromptMakerVM;

                // nullチェック
                if (vm != null)
                {
                    vm.PromptComposerConf = this.PromptComposerConf;    // プロンプトリストのセット
                    vm.Prompt = this.Parameter.Prompt;                  // プロンプトのセット

                    // プロンプトメーカー表示
                    if (wnd.ShowDialog() == true)
                    {
                        this.Parameter.Prompt = vm.Prompt;  // プロンプトの反映
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");   
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

        #region プロンプトへセット
        /// <summary>
        /// プロンプトへセット
        /// </summary>
        public void SetPrompt()
        {
            try
            {
                if (this.PromptComposerConf.Item != null && this.PromptComposerConf.Item.SelectedItem != null)
                {
                    this.Parameter.Prompt = this.PromptComposerConf.Item.SelectedItem.Prompt;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        public void AddList()
        {
            try
            {
                if (this.PromptComposerConf.Item != null)
                {
                    var exsist_f = (from x in this.PromptComposerConf.Item.Items
                               where x.Prompt.Equals(this.Parameter.Prompt)
                               select x).Any();

                    if(!exsist_f)
                    {
                        this.PromptComposerConf.Item.Items.Add(new PromptConsistM()
                        {
                            Prompt = this.Parameter.Prompt,
                            Description = "Dummy",
                            IsEnable = false                           
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }

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

        public void CreateMovie()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new SaveFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "動画ファイル (*.mp4)|*.mp4";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    var outdir = Path.Combine(this.Parameter.Outdir, "Sample");
                    var Converter = new ImageConverter();
                    //H264を使う場合、openh264-*.dllが必要。このdllをソフトウェアと同一のフォルダに入れる。
                    using (var Writer = new OpenCvSharp.VideoWriter(dialog.FileName, OpenCvSharp.FourCC.H264, 20, new OpenCvSharp.Size(this.Parameter.Width, this.Parameter.Height)))
                    {
                        // ディレクトリパス
                        string path = this.Parameter.Outdir;

                        // サンプルフォルダ配下
                        path = Path.Combine(path, "samples");

                        PathManager.CreateDirectory(path);

                        // DirectoryInfoのインスタンスを生成する
                        DirectoryInfo di = new DirectoryInfo(path);

                        // ディレクトリ直下のすべてのファイル一覧を取得する
                        FileInfo[] fiAlls = di.GetFiles("*.png");

                        foreach (var file in fiAlls)
                        {
                            var image = OpenCvSharp.Mat.FromImageData((byte[])Converter.ConvertTo(Image.FromFile(file.FullName), typeof(byte[]))!);
                            Writer.Write(image);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
    }
}
