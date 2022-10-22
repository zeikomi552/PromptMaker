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

        #region 現在選択中の画像パス(アウトプット側)[ImagePath]プロパティ
        /// <summary>
        /// 現在選択中の画像パス(アウトプット側)[ImagePath]プロパティ用変数
        /// </summary>
        string _ImagePath = string.Empty;
        /// <summary>
        /// 現在選択中の画像パス(アウトプット側)[ImagePath]プロパティ
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

        #region テストのリピート回数[TestRepeatCount]プロパティ
        /// <summary>
        /// テストのリピート回数[TestRepeatCount]プロパティ用変数
        /// </summary>
        int _TestRepeatCount = 1;
        /// <summary>
        /// テストのリピート回数[TestRepeatCount]プロパティ
        /// </summary>
        public int TestRepeatCount
        {
            get
            {
                return _TestRepeatCount;
            }
            set
            {
                if (!_TestRepeatCount.Equals(value))
                {
                    _TestRepeatCount = value;
                    NotifyPropertyChanged("TestRepeatCount");
                }
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
                       this.ImagePathList.Items.Clear();
                       this.ImagePath = String.Empty;

                       if (fiAlls.Length > 0)
                       {
                           // 出力先ファイルパスを取得し画面表示
                           this.Parameter.OutputFilePath = this.ImagePath = fiAlls.Last().FullName;

                           // ファイルリストの表示
                           this.ImagePathList.Items = new System.Collections.ObjectModel.ObservableCollection<FileInfo>(fiAlls);
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
                // ファイルが選択されていてファイルパスが存在する場合
                if (!string.IsNullOrEmpty(this.ImagePath) && File.Exists(this.ImagePath))
                {
                    string str = Path.GetDirectoryName(this.ImagePath)!;
                    Process.Start("explorer.exe", string.Format(@"/select,""{0}", this.ImagePath)); // エクスプローラを開く
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
                // ファイルが選択されていてファイルパスが存在する場合
                if (!string.IsNullOrEmpty(this.ImagePath) && File.Exists(this.ImagePath))
                {
                    Process.Start("mspaint", this.ImagePath); // 指定したファイルを開く
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }

        #endregion

        #region ESRGANの実行処理
        /// <summary>
        /// ESRGANの実行処理
        /// </summary>
        public void ExecuteRealESRGAN()
        {
            try
            {
                this.Parameter.ExecuteRealESRGAN(this.ImagePath);
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
                this.Parameter.ExecuteGFPGAN(this.ImagePath);
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion


        private string LastFilePath
        {
            get
            {
                // DirectoryInfoのインスタンスを生成する
                DirectoryInfo di = new DirectoryInfo(Path.Combine(this.Parameter.Outdir, "samples"));

                // ディレクトリ直下のすべてのファイル一覧を取得する
                FileInfo[] fiAlls = di.GetFiles("*.png");

                return fiAlls.Last().FullName;
            }
        }

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
        private void AutoTestForMoveForward(object sender, EventArgs ev)
        {
            // 初期ファイルの設定
            this.Parameter.SetInitFile(this.ImagePath);

            // 後ろへ移動
            this.Parameter.ShiftPic.ShiftBackward();

            // 右へ移動
            this.Parameter.ShiftPic.ShiftRight();

            // 下へ移動
            this.Parameter.ShiftPic.ShiftDown();

            // StableDiffusion実行
            this.Parameter.Execute(sender, ev);

            // 不要ファイルを削除
            DeleteFile();

            // 出力先ファイルパスを取得し画面表示
            this.Parameter.OutputFilePath = this.ImagePath = LastFilePath;

            // 初期ファイルの設定
            this.Parameter.SetInitFile(this.ImagePath);

            string imgpath = this.ImagePath;    // 高画質化の前のイメージパスの保持

            // RealESRGANの実行
            ExecuteRealESRGAN();

            // ファイル名の変更と上書き
            File.Move(this.LastFilePath, imgpath, true);

            // 出力先ファイルパスを取得し画面表示
            this.Parameter.OutputFilePath = this.ImagePath = LastFilePath;

            // 初期ファイルの設定
            this.Parameter.SetInitFile(this.ImagePath);

            // リサイズ前のファイルパスの保持
            imgpath = this.ImagePath;

            // サイズ縮小
            Utilities.ResizePic(this.ImagePath, this.Parameter.Width, this.Parameter.Height);

            // ファイル名の変更と上書き
            File.Move(this.LastFilePath, imgpath, true);

            // 出力先ファイルパスを取得し画面表示
            this.Parameter.OutputFilePath = this.ImagePath = LastFilePath;

            // 初期ファイルの設定
            this.Parameter.SetInitFile(this.ImagePath);
        }

        private void AutoTestForMoveBackward(object sender, EventArgs ev)
        {
            // 初期ファイルの設定
            this.Parameter.SetInitFile(this.ImagePath);

            // 後ろへ移動
            this.Parameter.ShiftPic.ShiftForward();

            // 右へ移動
            this.Parameter.ShiftPic.ShiftLeft();

            // 下へ移動
            this.Parameter.ShiftPic.ShiftUp();

            // StableDiffusion実行
            this.Parameter.Execute(sender, ev);

            // 不要ファイルを削除
            DeleteFile();

            // 出力先ファイルパスを取得し画面表示
            this.Parameter.OutputFilePath = this.ImagePath = LastFilePath;

            // 初期ファイルの設定
            this.Parameter.SetInitFile(this.ImagePath);

            string imgpath = this.ImagePath;    // 高画質化の前のイメージパスの保持

            // RealESRGANの実行
            ExecuteRealESRGAN();

            // ファイル名の変更と上書き
            File.Move(this.LastFilePath, imgpath, true);

            // 出力先ファイルパスを取得し画面表示
            this.Parameter.OutputFilePath = this.ImagePath = LastFilePath;

            // 初期ファイルの設定
            this.Parameter.SetInitFile(this.ImagePath);

            // リサイズ前のファイルパスの保持
            imgpath = this.ImagePath;

            // サイズ縮小
            Utilities.ResizePic(this.ImagePath, this.Parameter.Width, this.Parameter.Height);

            // ファイル名の変更と上書き
            File.Move(this.LastFilePath, imgpath, true);

            // 出力先ファイルパスを取得し画面表示
            this.Parameter.OutputFilePath = this.ImagePath = LastFilePath;

            // 初期ファイルの設定
            this.Parameter.SetInitFile(this.ImagePath);
        }

        private void SetRandomPrompt()
        {
            for (int i = 0; i < this.PromptComposerConf.Item.Items.Count; i++)
            {
                var elem = this.PromptComposerConf.Item.Items.ElementAt(i);
                elem.IsEnable = false;
            }

            if (this.PromptComposerConf.Item.Items.Count > 0)
            {
                int index = _rand.Next(0, this.PromptComposerConf.Item.Items.Count - 1);
                this.PromptComposerConf.Item.Items.ElementAt(index).IsEnable = true;
            }
        }

        Random _rand = new Random();
        public void AutoTest(object sender, EventArgs ev)
        {
            try
            {
                int max = this.TestRepeatCount;

                for (int i = 0; i < max; i++)
                {
                    if (i % 30 == 0)
                    {
                        SetRandomPrompt();  // プロンプトの変更
                    }

                    if (i >= 0 && i < 10)
                    {
                        this.Parameter.Strength = (decimal)0.3;
                    }
                    else if (i >= 10 && i < 20)
                    {
                        this.Parameter.Strength = (decimal)0.45;
                    }
                    else
                    {
                        this.Parameter.Strength = (decimal)0.6;
                    }

                    AutoTestForMoveBackward(sender, ev);
                }

                // イメージリストの更新
                RefreshImageList();

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }

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
                this.Parameter.SetInitFile(this.ImagePath);
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
    }
}
