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
                string path = GetOutputFilePath();

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
                string prompt_bk = this.Parameter.Prompt;

                // プロンプトを改行で分割する
                string[] prompt_list = prompt_bk.Split("\r\n");

                // 親ディレクトリ
                var parent_dir = Path.Combine(this.SettingConf.Item.CurrentDir, "outputs", "txt2img-samples");
                var path = Path.Combine(parent_dir, this.Parameter.Prefix);

                // DirectoryInfoのインスタンスを生成する
                DirectoryInfo di = new DirectoryInfo(path);

                StringBuilder article = new StringBuilder();

                // プロンプト補助リスト
                foreach (var composer in this.PromptComposerConf.Item.Items)
                {
                    // 有効なもののみ実行
                    if (composer.IsEnable)
                    {
                        article.AppendLine($"## {composer.Description}({composer.Prompt})");
                        foreach (var prompt in prompt_list)
                        {
                            // 空の改行の場合は次の行へ
                            if (string.IsNullOrWhiteSpace(prompt))
                            {
                                continue;
                            }

                            article.AppendLine($"### {prompt}");
                            article.AppendLine($"");

                            this.Parameter.Prompt = prompt + " " + composer.Prompt; // プロンプトの作成
                            string command = this.Parameter.Command;                // コマンドの保持

                            // 記事作成の場合
                            if (this.Parameter.IsOutArticle)
                            {
                                ExecuteSub(sender, ev, true, $"Prompt -> {this.Parameter.Prompt}\r\n{command}", this.Parameter.Prefix);

                                // ディレクトリ直下のすべてのファイル一覧を取得する
                                FileInfo[] fiAlls = di.GetFiles();
                                var file = fiAlls.Last();
                                article.AppendLine($"Prompt:{this.Parameter.Prompt}");
                                article.AppendLine($"```");
                                article.AppendLine($"{command}");
                                article.AppendLine($"```");
                                article.AppendLine($"");
                                article.AppendLine($"[![]({this.Parameter.Prefix}/{file.Name})]({this.Parameter.Prefix}/{file.Name})");
                            }
                            else
                            {
                                ExecuteSub(sender, ev);
                            }
                        }
                    }
                }

                if(this.Parameter.IsOutArticle)
                {
                    string outpath = Path.Combine(parent_dir, $"{this.Parameter.Prefix}.md");
                    // （1）テキストファイルを開いて（なければ作って）StreamWriterオブジェクトを得る
                    using (StreamWriter writer = new StreamWriter(outpath, false, Encoding.UTF8))
                    {
                        // （2）ファイルにテキストを書き込む
                        writer.WriteLine(article.ToString());

                    } // （3）usingブロックを抜けるときにファイルが閉じられる
                }
                this.Parameter.Prompt = prompt_bk;
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion



        private string _LastFilePath = string.Empty;

        ////private string GetArticle()
        ////{
        ////    string[] prompts = new string[] {
        ////            "日本画", "油彩画", "アクリル画", "水彩画",
        ////            "グアッシュ", "パステル画", "ドローイング", "リトグラフ", "シルクスクリーン",
        ////            "木版画", "銅版画", "ジクレー", "ミクストメディア",
        ////            "ステンドグラス"
        ////        };

        ////    StringBuilder txt = new StringBuilder();

        ////    foreach (var tmp in prompts)
        ////    {
        ////        txt.AppendLine($"## {tmp}");
        ////        txt.AppendLine($"[{tmp} - Google画像検索](https://www.google.co.jp/search?q={tmp.Replace(" ", "+")}&tbm=isch)");
        ////        txt.AppendLine($"[{tmp} - Wiki](https://ja.wikipedia.org/wiki/{tmp})");
        ////    }

        ////    // 記事を出力する場合
        ////    if (this.Parameter.IsOutArticle)
        ////    {
        ////        // 親ディレクトリ
        ////        var parent_dir = Path.Combine(this.SettingConf.Item.CurrentDir, "outputs", "txt2img-samples");
        ////        var path = Path.Combine(parent_dir, this.Parameter.Prefix);

        ////        File.WriteAllText(Path.Combine(parent_dir, $"{this.Parameter.Prefix}.md"), text.ToString());
        ////    }

        ////    return txt.ToString();
        ////}


        //#region コマンドの実行
        ///// <summary>
        ///// コマンドの実行
        ///// </summary>
        //public void Execute2(object sender, EventArgs ev)
        //{
        //    try
        //    {
        //        //string txt = GetArticle();

        //        //string[] prompts = new string[] {
        //        //    "Katsushika Hokusai", "Giuseppe Arcimboldo", "Andy Warhol", "Gustav Klimt",
        //        //    "Eugène Henri Paul Gauguin", "Vincent Willem van Gogh", "Raffaello Santi", "Paul Cézanne", "Leonardo da Vinci",
        //        //    "Jacques-Louis David", "Salvador Dalí", "Francisco José de Goya y Lucientes", "Ferdinand Victor Eugène Delacroix",
        //        //    "Pablo Ruiz Picasso", "Camille Pissarro", "Johannes Vermeer","Pieter Bruegel", "Diego Velázquez", "Édouard Manet",
        //        //    "Jean-François Millet", "Edvard Munch", "Claude Monet", "Peter Paul Rubens", "Pierre-Auguste Renoir", "Rembrandt van Rijn"
        //        //};

        //        //string[] prompts = new string[] {
        //        //    "japan Painting", "Oil Painting", "Acrylic Painting", "watercolor painting",
        //        //    "Gouache", "Pastel paintings", "drawing", "lithograph", "Silkscreen",
        //        //    "Woodcut", "Copper engravings", "Giclee", "Mixed Media", "Stained Glass"
        //        //};

        //        //foreach (var prompt in prompts)
        //        //{
        //        //    this.Parameter.Prompt = "cat by " + prompt;
        //        //    ExecuteSub(sender, ev, true, $"Prompt -> {this.Parameter.Prompt}\r\n{this.Parameter.Command}");
        //        //}

        //        //for (int i = 0; i < 100; i++)
        //        //{
        //        //    this.Parameter.Seed = 0;
        //        //    ExecuteSub(sender, ev, true, $"Seed -> {i}\r\n{this.Parameter.Command}");
        //        //}

        //        var prompts = new string[,] {
        //                { "halloween", "ハロウィン" }
        //            };

        //        var prompts2 = new string[,] {
        //                { "by Oil Painting", "油彩画" },
        //                { "by watercolor painting", "水彩画" },
        //                { "by japan Painting", "日本画" },
        //                { "by Stained Glass", "ステンドグラス" },
        //                { "by Katsushika Hokusai", "葛飾北斎" },
        //            };

        //        int iter = 3;
        //        string keyword = "stablediffusion-09";

        //        StringBuilder text = new StringBuilder();

        //        // 親ディレクトリ
        //        var parent_dir = Path.Combine(this.SettingConf.Item.CurrentDir, "outputs", "txt2img-samples");
        //        var path = Path.Combine(parent_dir, keyword);

        //        for (int i = 0; i < prompts.GetLength(0); i ++)
        //        {
        //            var prompt = prompts[i, 0];
        //            var title = prompts[i, 1];

        //            text.AppendLine($"## {title}");

        //            for (int j = 0; j< prompts2.GetLength(0); j++)
        //            {
        //                var prompt2 = prompts2[j,0];
        //                var title2 = prompts2[j, 1];
        //                text.AppendLine($"### {title2}");

        //                for (int k = 1; k <= iter; k++)
        //                {
        //                    this.Parameter.Seed = k;
        //                    this.Parameter.Prompt = prompt + " " + prompt2;
        //                    ExecuteSub(sender, ev, true, $"Prompt -> {this.Parameter.Prompt}\r\n{this.Parameter.Command}", keyword);

        //                    // DirectoryInfoのインスタンスを生成する
        //                    DirectoryInfo di = new DirectoryInfo(path);

        //                    // ディレクトリ直下のすべてのファイル一覧を取得する
        //                    FileInfo[] fiAlls = di.GetFiles();
        //                    var file = fiAlls.Last();

        //                    if (k == 1)
        //                    {
        //                        text.AppendLine($"Prompt:{this.Parameter.Prompt}");
        //                    }

        //                    text.AppendLine($"```");
        //                    text.AppendLine($"{this.Parameter.Command}");
        //                    text.AppendLine($"```");
        //                    text.AppendLine($"");
        //                    text.AppendLine($"[![]({keyword}/{file.Name})]({keyword}/{file.Name})");
        //                }
        //            }
        //        }

        //        File.WriteAllText(Path.Combine(parent_dir, $"{keyword}.md"), text.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage.ShowErrorOK(ex.Message, "Error");
        //    }
        //}
        //#endregion

        #region サブ関数
        /// <summary>
        /// サブ関数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        /// <param name="debug_f"></param>
        /// <param name="msg"></param>
        /// <param name="keyword">文字列挿入画像のフォルダ名とファイルプレフィスク</param>
        private void ExecuteSub(object sender, EventArgs ev, bool debug_f = false, string msg = "", string keyword = "debug")
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

                // 画像ファイル確認
                if (this._LastFilePath != filepath)
                {
                    this._LastFilePath = this.ImagePath = filepath;
                }
                else return;

                this.Parameter.OutputFilePath = filepath;

                // ファイルリストの表示
                this.ImagePathList.Items = new System.Collections.ObjectModel.ObservableCollection<FileInfo>(fiAlls);

                // 最初の行に履歴を保存
                this.History.Items.Insert(0, this.Parameter.ShallowCopy<ParameterM>());

                // 最後に実行したプロンプトを保存
                this.SettingConf.Item.LastPrompt = this.Parameter.Prompt;

                if(debug_f)
                {
                    // 文字列を画像に挿入
                    SetDetail(this._LastFilePath, msg, keyword);
                }
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
