using Microsoft.Win32;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using MVVMCore.Common.Wrapper;
using PromptMaker.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace PromptMaker.Models
{
    public class ParameterM : ModelBase
    {
        #region 繰り返し回数[Repeat]プロパティ
        /// <summary>
        /// 繰り返し回数[Repeat]プロパティ用変数
        /// </summary>
        int _Repeat = 1;
        /// <summary>
        /// 繰り返し回数[Repeat]プロパティ
        /// </summary>
        public int Repeat
        {
            get
            {
                return _Repeat;
            }
            set
            {
                if (!_Repeat.Equals(value))
                {
                    _Repeat = value;
                    NotifyPropertyChanged("Repeat");
                }
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

        #region 幅(px)[Width]プロパティ
        /// <summary>
        /// 幅(px)[Width]プロパティ用変数
        /// </summary>
        int _Width = 640;
        /// <summary>
        /// 幅(px)[Width]プロパティ
        /// </summary>
        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                if (!_Width.Equals(value))
                {
                    _Width = value;
                    NotifyPropertyChanged("Width");
                }
            }
        }
        #endregion

        #region 高さ(px)[Height]プロパティ
        /// <summary>
        /// 高さ(px)[Height]プロパティ用変数
        /// </summary>
        int _Height = 320;
        /// <summary>
        /// 高さ(px)[Height]プロパティ
        /// </summary>
        public int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                if (!_Height.Equals(value))
                {
                    _Height = value;
                    NotifyPropertyChanged("Height");
                }
            }
        }
        #endregion

        #region Seed(0:ランダム 1～固定)[Seed]プロパティ
        /// <summary>
        /// Seed(0:ランダム 1～固定)[Seed]プロパティ用変数
        /// </summary>
        int _Seed = 0;
        /// <summary>
        /// Seed(0:ランダム 1～固定)[Seed]プロパティ
        /// </summary>
        public int Seed
        {
            get
            {
                return _Seed;
            }
            set
            {
                if (!_Seed.Equals(value))
                {
                    _Seed = value;
                    NotifyPropertyChanged("Seed");
                }
            }
        }
        #endregion

        #region プロンプト(呪文)[Prompt]プロパティ
        /// <summary>
        /// プロンプト(呪文)[Prompt]プロパティ用変数
        /// </summary>
        string _Prompt = string.Empty;
        /// <summary>
        /// プロンプト(呪文)[Prompt]プロパティ
        /// </summary>
        public string Prompt
        {
            get
            {
                return _Prompt;
            }
            set
            {
                if (_Prompt == null || !_Prompt.Equals(value))
                {
                    _Prompt = value;
                    NotifyPropertyChanged("Prompt");
                }
            }
        }
        #endregion

        #region ステップ数(数が多いほど品質が上がる)[Ddim_steps]プロパティ
        /// <summary>
        /// ステップ数(数が多いほど品質が上がる)[Ddim_steps]プロパティ用変数
        /// </summary>
        int _Ddim_steps = 75;
        /// <summary>
        /// ステップ数(数が多いほど品質が上がる)[Ddim_steps]プロパティ
        /// </summary>
        public int Ddim_steps
        {
            get
            {
                return _Ddim_steps;
            }
            set
            {
                if (!_Ddim_steps.Equals(value))
                {
                    _Ddim_steps = value;
                    NotifyPropertyChanged("Ddim_steps");
                }
            }
        }
        #endregion

        #region 出力先ディレクトリ[Outdir]プロパティ
        /// <summary>
        /// 出力先ディレクトリ[Outdir]プロパティ用変数
        /// </summary>
        string _Outdir = string.Empty;
        /// <summary>
        /// 出力先ディレクトリ[Outdir]プロパティ
        /// </summary>
        public string Outdir
        {
            get
            {
                return _Outdir;
            }
            set
            {
                if (_Outdir == null || !_Outdir.Equals(value))
                {
                    _Outdir = value;
                    NotifyPropertyChanged("Outdir");
                }
            }
        }
        #endregion

        #region モデルファイルパス[CkptPath]プロパティ
        /// <summary>
        /// モデルファイルパス[CkptPath]プロパティ用変数
        /// </summary>
        string _CkptPath = string.Empty;
        /// <summary>
        /// モデルファイルパス[CkptPath]プロパティ
        /// </summary>
        public string CkptPath
        {
            get
            {
                return _CkptPath;
            }
            set
            {
                if (_CkptPath == null || !_CkptPath.Equals(value))
                {
                    _CkptPath = value;
                    NotifyPropertyChanged("CkptPath");
                }
            }
        }
        #endregion

        #region スキップグリッド[Skip_grid]プロパティ
        /// <summary>
        /// スキップグリッド[Skip_grid]プロパティ用変数
        /// </summary>
        int _Skip_grid = 0;
        /// <summary>
        /// スキップグリッド[Skip_grid]プロパティ
        /// </summary>
        public int Skip_grid
        {
            get
            {
                return _Skip_grid;
            }
            set
            {
                if (!_Skip_grid.Equals(value))
                {
                    _Skip_grid = value;
                    NotifyPropertyChanged("Skip_grid");
                }
            }
        }
        #endregion

        #region 実行後のコマンドを保存しておく領域[CommandBackup]プロパティ
        /// <summary>
        /// 実行後のコマンドを保存しておく領域[CommandBackup]プロパティ用変数
        /// </summary>
        string _CommandBackup = string.Empty;
        /// <summary>
        /// 実行後のコマンドを保存しておく領域[CommandBackup]プロパティ
        /// </summary>
        public string CommandBackup
        {
            get
            {
                return _CommandBackup;
            }
            set
            {
                if (_CommandBackup == null || !_CommandBackup.Equals(value))
                {
                    _CommandBackup = value;
                    NotifyPropertyChanged("CommandBackup");
                }
            }
        }
        #endregion

        #region 実行後のパスを保存しておく領域[OutputFilePath]プロパティ
        /// <summary>
        /// 実行後のパスを保存しておく領域[OutputFilePath]プロパティ用変数
        /// </summary>
        string _OutputFilePath = string.Empty;
        /// <summary>
        /// 実行後のパスを保存しておく領域[OutputFilePath]プロパティ
        /// </summary>
        public string OutputFilePath
        {
            get
            {
                return _OutputFilePath;
            }
            set
            {
                if (_OutputFilePath == null || !_OutputFilePath.Equals(value))
                {
                    _OutputFilePath = value;
                    NotifyPropertyChanged("OutputFilePath");
                }
            }
        }
        #endregion

        #region 初期ファイルパス[InitFilePath]プロパティ
        /// <summary>
        /// 初期ファイルパス[InitFilePath]プロパティ用変数
        /// </summary>
        string _InitFilePath = string.Empty;
        /// <summary>
        /// 初期ファイルパス[InitFilePath]プロパティ
        /// </summary>
        public string InitFilePath
        {
            get
            {
                return _InitFilePath;
            }
            set
            {
                if (_InitFilePath == null || !_InitFilePath.Equals(value))
                {
                    _InitFilePath = value;
                    NotifyPropertyChanged("InitFilePath");
                }
            }
        }
        #endregion

        #region スクリプトのタイプ(txt2img, img2img, inpaint)[ScriptType]プロパティ
        /// <summary>
        /// スクリプトのタイプ(txt2img, img2img, inpaint)[ScriptType]プロパティ用変数
        /// </summary>
        ScriptTypeEnum _ScriptType = ScriptTypeEnum.Txt2Img;
        /// <summary>
        /// スクリプトのタイプ(txt2img, img2img, inpaint)[ScriptType]プロパティ
        /// </summary>
        public ScriptTypeEnum ScriptType
        {
            get
            {
                return _ScriptType;
            }
            set
            {
                if (!_ScriptType.Equals(value))
                {
                    _ScriptType = value;
                    NotifyPropertyChanged("ScriptType");
                    NotifyPropertyChanged("Img2ImgF");
                    NotifyPropertyChanged("Txt2ImgF");
                    NotifyPropertyChanged("InpaintF");
                }
            }
        }
        #endregion

        #region Img2Imgフラグ
        /// <summary>
        /// Img2Imgフラグ
        /// </summary>
        public bool Img2ImgF
        {
            get
            {
                return this.ScriptType == ScriptTypeEnum.Img2Img;
            }
        }
        #endregion

        #region Txt2Imgフラグ
        /// <summary>
        /// Txt2Imgフラグ
        /// </summary>
        public bool Txt2ImgF
        {
            get
            {
                return this.ScriptType == ScriptTypeEnum.Txt2Img;
            }
        }
        #endregion

        #region InpaintF
        /// <summary>
        /// InpaintF
        /// </summary>
        public bool InpaintF
        {
            get
            {
                return this.ScriptType == ScriptTypeEnum.Inpaint;
            }
        }
        #endregion

        #region ストレングス[Strength]プロパティ
        /// <summary>
        /// ストレングス[Strength]プロパティ用変数
        /// </summary>
        decimal _Strength = (decimal)0.6;
        /// <summary>
        /// ストレングス[Strength]プロパティ
        /// </summary>
        public decimal Strength
        {
            get
            {
                return _Strength;
            }
            set
            {
                if (!_Strength.Equals(value))
                {
                    _Strength = value;
                    NotifyPropertyChanged("Strength");
                }
            }
        }
        #endregion

        #region ガイダンススケール(0-20)[Guidance_Scale]プロパティ
        /// <summary>
        /// ガイダンススケール(0-20)[Guidance_Scale]プロパティ用変数
        /// </summary>
        decimal _Guidance_Scale = (decimal)7.5;
        /// <summary>
        /// ガイダンススケール(0-20)[Guidance_Scale]プロパティ
        /// </summary>
        public decimal Guidance_Scale
        {
            get
            {
                return _Guidance_Scale;
            }
            set
            {
                if (!_Guidance_Scale.Equals(value))
                {
                    _Guidance_Scale = value;
                    NotifyPropertyChanged("Guidance_Scale");
                }
            }
        }
        #endregion

        #region サンプル数[N_Sample]プロパティ
        /// <summary>
        /// サンプル数[N_Sample]プロパティ用変数
        /// </summary>
        int _N_Sample = 3;
        /// <summary>
        /// サンプル数[N_Sample]プロパティ
        /// </summary>
        public int N_Sample
        {
            get
            {
                return _N_Sample;
            }
            set
            {
                if (!_N_Sample.Equals(value))
                {
                    _N_Sample = value;
                    NotifyPropertyChanged("N_Sample");
                }
            }
        }
        #endregion

        #region 作成回数[N_iter]プロパティ
        /// <summary>
        /// 作成回数[N_iter]プロパティ用変数
        /// </summary>
        int _N_iter = 3;
        /// <summary>
        /// 作成回数[N_iter]プロパティ
        /// </summary>
        public int N_iter
        {
            get
            {
                return _N_iter;
            }
            set
            {
                if (!_N_iter.Equals(value))
                {
                    _N_iter = value;
                    NotifyPropertyChanged("N_iter");
                }
            }
        }
        #endregion

        #region プレフィックス(ファイル出力時のフォルダ名とファイル名接頭辞に使用する)[Prefix]プロパティ
        /// <summary>
        /// プレフィックス(ファイル出力時のフォルダ名とファイル名接頭辞に使用する)[Prefix]プロパティ用変数
        /// </summary>
        string _Prefix = "output";
        /// <summary>
        /// プレフィックス(ファイル出力時のフォルダ名とファイル名接頭辞に使用する)[Prefix]プロパティ
        /// </summary>
        public string Prefix
        {
            get
            {
                return _Prefix;
            }
            set
            {
                if (_Prefix == null || !_Prefix.Equals(value))
                {
                    _Prefix = value;
                    NotifyPropertyChanged("Prefix");
                }
            }
        }
        #endregion

        #region PLMSを使用する[UsePlms]プロパティ
        /// <summary>
        /// PLMSを使用する[UsePlms]プロパティ用変数
        /// </summary>
        bool _UsePlms = true;
        /// <summary>
        /// PLMSを使用する[UsePlms]プロパティ
        /// </summary>
        public bool UsePlms
        {
            get
            {
                return _UsePlms;
            }
            set
            {
                if (!_UsePlms.Equals(value))
                {
                    _UsePlms = value;
                    NotifyPropertyChanged("UsePlms");
                }
            }
        }
        #endregion

        #region デバッグフラグ[DebugF]プロパティ
        /// <summary>
        /// デバッグフラグ[DebugF]プロパティ用変数
        /// </summary>
        bool _DebugF = false;
        /// <summary>
        /// デバッグフラグ[DebugF]プロパティ
        /// </summary>
        public bool DebugF
        {
            get
            {
                return _DebugF;
            }
            set
            {
                if (!_DebugF.Equals(value))
                {
                    _DebugF = value;
                    NotifyPropertyChanged("DebugF");
                }
            }
        }
        #endregion

        #region 大きいサイズで作成するフラグ[HugeSizeF]プロパティ
        /// <summary>
        /// 大きいサイズで作成するフラグ[HugeSizeF]プロパティ用変数
        /// </summary>
        bool _HugeSizeF = false;
        /// <summary>
        /// 大きいサイズで作成するフラグ[HugeSizeF]プロパティ
        /// </summary>
        public bool HugeSizeF
        {
            get
            {
                return _HugeSizeF;
            }
            set
            {
                if (!_HugeSizeF.Equals(value))
                {
                    _HugeSizeF = value;
                    NotifyPropertyChanged("HugeSizeF");
                }
            }
        }
        #endregion



        Random _Rand = new Random();

        #region コマンド
        /// <summary>
        /// コマンド
        /// </summary>
        public string Command
        {
            get
            {
                switch (this.ScriptType)
                {
                    case ScriptTypeEnum.Txt2Img:
                    default:
                        {
                            return Command_Txt2Img;
                        }
                    case ScriptTypeEnum.Img2Img:
                        {
                            return Command_Img2Img;
                        }
                    case ScriptTypeEnum.Inpaint:
                        {
                            return Command_Inpaint;
                        }
                }
            }
        }

        /// <summary>
        /// コマンド
        /// </summary>
        /// <returns>コマンド</returns>
        public string Command_Txt2Img
        {
            get
            {
                StringBuilder command = new StringBuilder();

                // 大きいサイズで作成する場合Optimizeの方を使用する
                if (!this.HugeSizeF)
                {
                    command.AppendLine("python scripts/txt2img.py");
                }
                else
                {
                    command.AppendLine("python optimizedSD/optimized_txt2img.py");
                }

                command.AppendLine($"--prompt \"{this.Prompt}\"");
                command.AppendLine($"--n_iter {N_iter}");

                // 大きいサイズで作成する場合Optimizeの方を使用する
                // 高速化のためturboを立てる
                if (this.HugeSizeF)
                {
                    command.AppendLine($"--turbo");
                }
                command.AppendLine(this.Width <= 0 ? "" : $"--W {this.Width}");
                command.AppendLine(this.Height <= 0 ? "" : $"--H {this.Height}");
                command.AppendLine(this.Seed <= 0 ? $"--seed {_Rand.Next(1, 99999)}" : $"--seed {this.Seed}");
                command.AppendLine(this.Ddim_steps <= 0 ? "" : $"--ddim_steps {this.Ddim_steps}");
                command.AppendLine(this.UsePlms ? "--plms" : "");
                command.AppendLine(!string.IsNullOrWhiteSpace(this.Outdir) ? $"--outdir {this.Outdir}" : "");
                command.AppendLine(!string.IsNullOrWhiteSpace(this.CkptPath) ? $"--ckpt {this.CkptPath}" : "");

                return command.ToString().Replace("\r\n", " ");
            }
        }

        /// <summary>
        /// コマンド
        /// </summary>
        public string Command_Img2Img
        {
            get
            {
                StringBuilder command = new StringBuilder();
                command.AppendLine("python scripts/img2img.py");
                command.AppendLine($"--prompt \"{this.Prompt}\"");
                //command.AppendLine($"--plms");
                command.AppendLine($"--init-img");
                command.AppendLine($"{this.InitFilePath}");
                command.AppendLine($"--n_iter {N_iter}");

                command.AppendLine($"--strength {this.Strength}");
                command.AppendLine($"--n_sample {this.N_Sample}");
                command.AppendLine($"--scale {this.Guidance_Scale}");
                command.AppendLine(!string.IsNullOrWhiteSpace(this.Outdir) ? $"--outdir {this.Outdir}" : "");
                command.AppendLine(this.UsePlms ? "--plms" : "");
                command.AppendLine(this.Seed <= 0 ? $"--seed {_Rand.Next(1, 99999)}" : $"--seed {this.Seed}");
                command.AppendLine(this.Ddim_steps <= 0 ? "" : $"--ddim_steps {this.Ddim_steps}");

                return command.ToString().Replace("\r\n", " ");
            }

        }

        /// <summary>
        /// コマンド
        /// </summary>
        public string Command_Inpaint
        {
            get
            {
                string indir_path = Path.Combine(this.SettingConf.Item.CurrentDir, "inputs", "inpainting");
                string outdir_path = Path.Combine(this.SettingConf.Item.CurrentDir, "outputs", "inpainting-samples");

                StringBuilder command = new StringBuilder();
                command.AppendLine("python scripts/inpaint.py");
                command.AppendLine($"--indir");
                command.AppendLine(indir_path);
                command.AppendLine($"--outdir");
                command.AppendLine(outdir_path);
                command.AppendLine($"--steps");
                command.AppendLine($"{this.Ddim_steps}");
                return command.ToString().Replace("\r\n", " ");
            }
        }
        #endregion

        #region ファイルを開くダイアログ
        /// <summary>
        /// ファイルを開くダイアログ
        /// </summary>
        public void OpenInitFile()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "画像ファイル (*.png)|*.png|全てのファイル (*.*)|*.*";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    SetInitFile(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 初期ファイルパスをセットする
        /// <summary>
        /// 初期ファイルパスをセットする
        /// </summary>
        /// <param name="filepath"></param>
        public void SetInitFile(string filepath)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    this.InitFilePath = filepath;
                }
                else
                {
                    ShowMessage.ShowNoticeOK("ファイルが存在しません。", "通知");
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 出力先ディレクトリの選択
        /// <summary>
        /// 出力先ディレクトリの選択
        /// </summary>
        public void OpenOutDir()
        {
            try
            {
                var dlg = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();

                // フォルダ選択ダイアログ（falseにするとファイル選択ダイアログ）
                dlg.IsFolderPicker = true;
                // タイトル
                dlg.Title = "フォルダを選択してください";

                // 初期ディレクトリ
                if (dlg.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
                {
                    this.Outdir = dlg.FileName;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region ファイルを開くダイアログ
        /// <summary>
        /// ファイルを開くダイアログ
        /// </summary>
        public void OpenCkptFile()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "モデルファイル (*.ckpt)|*.ckpt";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    this.CkptPath = dialog.FileName;
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
        public string GetOutputFilePath()
        {
            if (string.IsNullOrWhiteSpace(this.Outdir))
            {
                string path = Path.Combine(this.SettingConf.Item.CurrentDir, "outputs", "txt2img-samples");
                switch (this.ScriptType)
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
            else
            {
                return this.Outdir;
            }

        }
        #endregion

        #region コマンド実行処理
        /// <summary>
        /// コマンド実行処理
        /// </summary>
        public void CommandExecute()
        {
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

            string command = this.Command!;   // コマンドの保存
            this.CommandBackup = command;     // コマンドの保存

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
            string output = myProcess.StandardOutput.ReadToEnd(); // 標準出力の読み取り
            myProcess.Close();
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
    }
}
