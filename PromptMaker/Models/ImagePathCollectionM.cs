using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
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
        public string LastFilePath(string outdir)
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
    }
}
