using Microsoft.Win32;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PromptMaker.Models
{
    public class SettingConfM : ModelBase
    {
        #region Python.exeのパス[PythonPath]プロパティ
        /// <summary>
        /// Python.exeのパス[PythonPath]プロパティ用変数
        /// </summary>
        string _PythonPath = @"C:\ProgramData\Anaconda3\python.exe";
        /// <summary>
        /// Python.exeのパス[PythonPath]プロパティ
        /// </summary>
        public string PythonPath
        {
            get
            {
                return _PythonPath;
            }
            set
            {
                if (_PythonPath == null || !_PythonPath.Equals(value))
                {
                    _PythonPath = value;
                    NotifyPropertyChanged("PythonPath");
                }
            }
        }
        #endregion

        #region カレントディレクトリ[CurrentDir]プロパティ
        /// <summary>
        /// カレントディレクトリ[CurrentDir]プロパティ用変数
        /// </summary>
        string _CurrentDir = string.Empty;
        /// <summary>
        /// カレントディレクトリ[CurrentDir]プロパティ
        /// </summary>
        public string CurrentDir
        {
            get
            {
                return _CurrentDir;
            }
            set
            {
                if (_CurrentDir == null || !_CurrentDir.Equals(value))
                {
                    _CurrentDir = value;
                    NotifyPropertyChanged("CurrentDir");
                }
            }
        }
        #endregion

        #region Python実行ファイルパスの選択ダイアログ
        /// <summary>
        /// Python実行ファイルパスの選択ダイアログ
        /// </summary>
        public void OpenFileDialogForPythonExe()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "Python Exeパス (*.exe)|*.exe";

                if (!string.IsNullOrEmpty(this.PythonPath))
                {
                    string sDirName = System.IO.Path.GetDirectoryName(this.PythonPath)!;
                    dialog.InitialDirectory = sDirName;
                }

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    this.PythonPath = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region Real-ESRGAN実行ファイルパスの選択ダイアログ
        /// <summary>
        /// Python実行ファイルパスの選択ダイアログ
        /// </summary>
        public void OpenFileDialogForRealESRGANExe()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "Real-ESRGAN Exeパス (*.exe)|*.exe";

                if (!string.IsNullOrEmpty(this.PythonPath))
                {
                    string sDirName = System.IO.Path.GetDirectoryName(this.PythonPath)!;
                    dialog.InitialDirectory = sDirName;
                }

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    this.RealEsrganExePath = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion


        #region Real-ESRGAN実行ファイルパスの選択ダイアログ
        /// <summary>
        /// Python実行ファイルパスの選択ダイアログ
        /// </summary>
        public void OpenFileDialogForGFPGANPath()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "GFPGAN Python (*.py)|*.py";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    this.GFPGANPyPath = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region カレントディレクトリの選択ダイアログ
        /// <summary>
        /// カレントディレクトリの選択ダイアログ
        /// </summary>
        public void OpenCurrentDir()
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
                    this.CurrentDir = dlg.FileName;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 最後に実行したプロンプト[LastPrompt]プロパティ
        /// <summary>
        /// 最後に実行したプロンプト[LastPrompt]プロパティ用変数
        /// </summary>
        string _LastPrompt = string.Empty;
        /// <summary>
        /// 最後に実行したプロンプト[LastPrompt]プロパティ
        /// </summary>
        public string LastPrompt
        {
            get
            {
                return _LastPrompt;
            }
            set
            {
                if (_LastPrompt == null || !_LastPrompt.Equals(value))
                {
                    _LastPrompt = value;
                    NotifyPropertyChanged("LastPrompt");
                }
            }
        }
        #endregion

        #region Real-ESRGANの実行ファイルパス[RealEsrganExePath]プロパティ
        /// <summary>
        /// Real-ESRGANの実行ファイルパス[RealEsrganExePath]プロパティ用変数
        /// </summary>
        string _RealEsrganExePath = string.Empty;
        /// <summary>
        /// Real-ESRGANの実行ファイルパス[RealEsrganExePath]プロパティ
        /// </summary>
        public string RealEsrganExePath
        {
            get
            {
                return _RealEsrganExePath;
            }
            set
            {
                if (_RealEsrganExePath == null || !_RealEsrganExePath.Equals(value))
                {
                    _RealEsrganExePath = value;
                    NotifyPropertyChanged("RealEsrganExePath");
                }
            }
        }
        #endregion

        #region GFPGANPythonファイルパス[GFPGANPyPath]プロパティ
        /// <summary>
        /// GFPGANPythonファイルパス[GFPGANPyPath]プロパティ用変数
        /// </summary>
        string _GFPGANPyPath = string.Empty;
        /// <summary>
        /// GFPGANPythonファイルパス[GFPGANPyPath]プロパティ
        /// </summary>
        public string GFPGANPyPath
        {
            get
            {
                return _GFPGANPyPath;
            }
            set
            {
                if (_GFPGANPyPath == null || !_GFPGANPyPath.Equals(value))
                {
                    _GFPGANPyPath = value;
                    NotifyPropertyChanged("GFPGANPyPath");
                }
            }
        }
        #endregion


        #region Hugging Face用アクセストークン(inpaint2使用時のみ)[AccessToken]プロパティ
        /// <summary>
        /// Hugging Face用アクセストークン(inpaint2使用時のみ)[AccessToken]プロパティ用変数
        /// </summary>
        string _AccessToken = string.Empty;
        /// <summary>
        /// Hugging Face用アクセストークン(inpaint2使用時のみ)[AccessToken]プロパティ
        /// </summary>
        public string AccessToken
        {
            get
            {
                return _AccessToken;
            }
            set
            {
                if (_AccessToken == null || !_AccessToken.Equals(value))
                {
                    _AccessToken = value;
                    NotifyPropertyChanged("AccessToken");
                }
            }
        }
        #endregion





    }
}
