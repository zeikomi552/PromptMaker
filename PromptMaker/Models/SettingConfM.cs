﻿using Microsoft.Win32;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
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

        #region 出力先フォルダの選択ダイアログ
        /// <summary>
        /// 出力先フォルダの選択ダイアログ
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

                if (!string.IsNullOrEmpty(this.CurrentDir))
                {
                    string sDirName = System.IO.Path.GetDirectoryName(this.CurrentDir)!;
                    dlg.InitialDirectory = dlg.FileName;
                }


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


    }
}
