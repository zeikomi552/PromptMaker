using Microsoft.VisualBasic.FileIO;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromptMaker.Models
{
    public class ImagePathCollectionM : ModelBase
    {

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

        #region 最後のファイルパスを取得
        /// <summary>
        /// 最後のファイルパスを取得
        /// </summary>
        public string GetLastFilePath(string outdir)
        {
            return GetLastFileInfo(outdir).FullName;
        }
        #endregion

        #region 最終ファイルパス情報の取得
        /// <summary>
        /// 最終ファイルパス情報の取得
        /// </summary>
        public FileInfo GetLastFileInfo(string outdir)
        {
            // DirectoryInfoのインスタンスを生成する
            DirectoryInfo di = new DirectoryInfo(Path.Combine(outdir, "samples"));

            // ディレクトリ直下のすべてのファイル一覧を取得する
            FileInfo[] fiAlls = di.GetFiles("*.png");

            return fiAlls.Last();
        }
        #endregion

        #region 要素の削除処理
        /// <summary>
        /// 要素の削除処理
        /// </summary>
        /// <param name="item"></param>
        public void RemoveAt(FileInfo item)
        {
            var file_info = item;

            if (file_info != null)
            {
                var index = this.ImagePathList.Items.IndexOf(file_info);
                this.ImagePathList.Items.Remove(file_info);
                FileSystem.DeleteFile(file_info.FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                if (index >= 1)
                {
                    this.ImagePathList.SelectedItem = this.ImagePathList.Items.Last();
                }
                else
                {
                    this.ImagePathList.SelectedItem = null;
                }
            }
        }
        #endregion

        #region 最後の要素を選択する
        /// <summary>
        /// 最後の要素を選択する
        /// </summary>
        public void LastElementSelect()
        {
            this.ImagePathList.SelectedItem = this.ImagePathList.Items.Last();
        }
        #endregion


        #region ファイルを開く処理
        /// <summary>
        /// ファイルを開く処理
        /// </summary>
        public void FileOpen()
        {
            try
            {
                // ファイルが選択されていてファイルパスが存在する場合
                if (!string.IsNullOrEmpty(this.ImagePathList.SelectedItem.FullName) && File.Exists(this.ImagePathList.SelectedItem.FullName))
                {
                    Process.Start("mspaint", this.ImagePathList.SelectedItem.FullName); // 指定したファイルを開く
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
        public void FolderOpen()
        {
            try
            {
                // ファイルが選択されていてファイルパスが存在する場合
                if (this.ImagePathList.SelectedItem != null && File.Exists(this.ImagePathList.SelectedItem.FullName))
                {
                    string str = Path.GetDirectoryName(this.ImagePathList.SelectedItem.FullName)!;
                    Process.Start("explorer.exe", string.Format(@"/select,""{0}", this.ImagePathList.SelectedItem.FullName)); // エクスプローラを開く
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion
    }
}
