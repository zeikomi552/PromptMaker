using Microsoft.Win32;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using MVVMCore.Common.Wrapper;
using PromptMaker.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Xml.Serialization;
using System.Windows.Shapes;
using Path = System.IO.Path;
using System.IO.Compression;
using static System.Net.Mime.MediaTypeNames;

namespace PromptMaker.Models
{
    public class CardDataM : ModelBase
    {
        #region ワードクラウド画像パス[ImagePath]プロパティ
        /// <summary>
        /// 画像パス[ImagePath]プロパティ用変数
        /// </summary>
        string _ImagePath = string.Empty;
        /// <summary>
        /// 画像パス[ImagePath]プロパティ
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

        #region 背景画像[BackgroundImage]プロパティ
        /// <summary>
        /// 背景画像[BackgroundImage]プロパティ用変数
        /// </summary>
        string _BackgroundImage = string.Empty;
        /// <summary>
        /// 背景画像[BackgroundImage]プロパティ
        /// </summary>
        public string BackgroundImage
        {
            get
            {
                return _BackgroundImage;
            }
            set
            {
                if (_BackgroundImage == null || !_BackgroundImage.Equals(value))
                {
                    _BackgroundImage = value;
                    NotifyPropertyChanged("BackgroundImage");
                }
            }
        }
        #endregion

        #region ホログラムパス[HologramPath]プロパティ
        /// <summary>
        /// ホログラムパス[HologramPath]プロパティ用変数
        /// </summary>
        string _HologramPath = string.Empty;
        /// <summary>
        /// ホログラムパス[HologramPath]プロパティ
        /// </summary>
        public string HologramPath
        {
            get
            {
                return _HologramPath;
            }
            set
            {
                if (_HologramPath == null || !_HologramPath.Equals(value))
                {
                    _HologramPath = value;
                    NotifyPropertyChanged("HologramPath");
                }
            }
        }
        #endregion

        #region タイトル[Title]プロパティ
        /// <summary>
        /// タイトル[Title]プロパティ用変数
        /// </summary>
        string _Title = "タイトル";
        /// <summary>
        /// タイトル[Title]プロパティ
        /// </summary>
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (_Title == null || !_Title.Equals(value))
                {
                    _Title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }
        #endregion

        #region タイプ名[TypeName]プロパティ
        /// <summary>
        /// タイプ名[TypeName]プロパティ用変数
        /// </summary>
        string _TypeName = "属性";
        /// <summary>
        /// タイプ名[TypeName]プロパティ
        /// </summary>
        public string TypeName
        {
            get
            {
                return _TypeName;
            }
            set
            {
                if (_TypeName == null || !_TypeName.Equals(value))
                {
                    _TypeName = value;
                    NotifyPropertyChanged("TypeName");
                }
            }
        }
        #endregion

        #region フッター[Footer]プロパティ
        /// <summary>
        /// フッター[Footer]プロパティ用変数
        /// </summary>
        string _Footer = "Happy Programmer Games";
        /// <summary>
        /// フッター[Footer]プロパティ
        /// </summary>
        public string Footer
        {
            get
            {
                return _Footer;
            }
            set
            {
                if (_Footer == null || !_Footer.Equals(value))
                {
                    _Footer = value;
                    NotifyPropertyChanged("Footer");
                }
            }
        }
        #endregion

        #region 説明[Description]プロパティ
        /// <summary>
        /// 説明[Description]プロパティ用変数
        /// </summary>
        string _Description = string.Empty;
        /// <summary>
        /// 説明[Description]プロパティ
        /// </summary>
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (_Description == null || !_Description.Equals(value))
                {
                    _Description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }
        #endregion

        #region 透明度[Opacity]プロパティ
        /// <summary>
        /// 透明度[Opacity]プロパティ用変数
        /// </summary>
        decimal _Opacity = (decimal)0.3;
        /// <summary>
        /// 透明度[Opacity]プロパティ
        /// </summary>
        public decimal Opacity
        {
            get
            {
                return _Opacity;
            }
            set
            {
                if (!_Opacity.Equals(value))
                {
                    _Opacity = value;
                    NotifyPropertyChanged("Opacity");
                }
            }
        }
        #endregion



        [XmlIgnore]
        System.Windows.Controls.Border _ImageArea = new System.Windows.Controls.Border();

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="border"></param>
        public void Init(System.Windows.Controls.Border border)
        {
            _ImageArea = border;
        }
        #endregion

        #region イメージファイルを開く
        /// <summary>
        /// イメージファイルを開く
        /// </summary>
        public void OpenImageFile()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "画像ファイル (*.png)|*.png";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    this.BackgroundImage = this.ImagePath = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 背景ファイルを開く処理
        /// <summary>
        /// 背景ファイルを開く処理
        /// </summary>
        public void OpenBackgroundImageFile()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "画像ファイル (*.png)|*.png";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    this.BackgroundImage = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion


        #region ホログラムファイルを開く処理
        /// <summary>
        /// ホログラムファイルを開く処理
        /// </summary>
        public void OpenHologramImageFile()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "画像ファイル (*.png)|*.png";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    this.HologramPath = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 画像保存ボタン処理(.png)
        /// <summary>
        /// 保存ボタン処理(.png)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Save(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlgSave = new Microsoft.Win32.SaveFileDialog();

                dlgSave.Filter = "PNGファイル(*.png)|*.png";
                dlgSave.AddExtension = true;

                if ((bool)dlgSave.ShowDialog()!)
                {
                    // レンダリング
                    var bmp = new RenderTargetBitmap(
                        (int)(_ImageArea.ActualWidth),
                        (int)(_ImageArea.ActualHeight),
                        96, 96, // DPI
                        PixelFormats.Pbgra32);
                    bmp.Render(_ImageArea);

                    // jpegで保存
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    using (var fs = File.Open(dlgSave.FileName, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }

        #region カードの保存処理
        /// <summary>
        /// カードの保存処理
        /// </summary>
        public void SaveCard()
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlgSave = new Microsoft.Win32.SaveFileDialog();

                dlgSave.Filter = "カードファイル(*.dfcard)|*.dfcard";
                dlgSave.AddExtension = true;

                if ((bool)dlgSave.ShowDialog()!)
                {
                    // ディレクトリ名の取り出し
                    string dirName = System.IO.Path.GetDirectoryName(dlgSave.FileName)!;

                    // ファイル名の取り出し
                    var fileName = System.IO.Path.GetFileNameWithoutExtension(dlgSave.FileName);

                    // カードファイルの保存処理
                    SaveCardFile(Path.Combine(dirName, fileName));
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #endregion

        #region ロード処理
        /// <summary>
        /// ロード処理
        /// </summary>
        public void Load()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "カードファイル (*.dfcard)|*.dfcard";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    LoadCardFile(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion


        #region プロジェクトファイルの作成処理
        /// <summary>
        /// プロジェクトファイルの作成処理
        /// </summary>
        /// <param name="save_file_name">保存先ファイル名</param>
        private void CreateProjectFile(string save_file_name)
        {
            try
            {
                var tmp = this.ShallowCopy<CardDataM>();
                var dirname = "Images";

                // ファイル名の取り出し
                var fileName = System.IO.Path.GetFileName(this.ImagePath);

                tmp.ImagePath = Path.Combine(dirname, System.IO.Path.GetFileName(this.ImagePath));              // Imageファイルパスの修正
                tmp.BackgroundImage = Path.Combine(dirname, System.IO.Path.GetFileName(this.BackgroundImage));  // 背景画像パスの修正
                tmp.HologramPath = Path.Combine(dirname, System.IO.Path.GetFileName(this.HologramPath));        // ホログラムファイルパスの修正

                // シリアライズ
                XMLUtil.Seialize(save_file_name, tmp);
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion



        /// <summary>
        /// 画像ファイルの移動処理
        /// </summary>
        /// <param name="dirpath">保存先ディレクトリ</param>
        private void SaveCardFile(string dirpath)
        {
            try
            {
                // 一時フォルダの取得
                string tempDir = Path.GetTempPath();

                // 新しいGUIDを生成する
                var guid = Guid.NewGuid();

                tempDir = Path.Combine(tempDir, "PromptMaker-" + guid.ToString());

                // ディレクトリが無ければ作成する
                PathManager.CreateDirectory(tempDir);

                // イメージ保存先
                string img_dir = Path.Combine(tempDir, "Images");

                // イメージファイルの移動
                MoveData(img_dir, this.ImagePath);

                // ホログラムファイルの移動
                MoveData(img_dir, this.HologramPath);

                // ホログラムファイルの移動
                MoveData(img_dir, this.BackgroundImage);

                // 構造XMLファイル名の作成
                string proj_file_path = System.IO.Path.Combine(tempDir, "content.xml");

                // xmlファイルを作成
                CreateProjectFile(proj_file_path);

                // 現在のディレクトリ名がファイル名になる
                var fileName = System.IO.Path.GetFileName(dirpath);

                // 親ディレクトリの取得
                string parent = Directory.GetParent(dirpath)!.FullName;
                string zippath = Path.Combine(parent, $"{fileName}.dfcard");

                // 既にファイルが存在する場合は削除する
                if (File.Exists(zippath))
                {
                    File.Delete(zippath);   // ファイルの削除
                }

                // zipファイルの作成
                ZipFile.CreateFromDirectory(tempDir, zippath);

                // DirectoryInfoのインスタンスを生成する
                DirectoryInfo di = new DirectoryInfo(tempDir);

                // 元のディレクトリを削除する
                di.Delete(true);

            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }

        /// <summary>
        /// ファイルの移動処理
        /// </summary>
        /// <param name="dirpath">ディレクトリパス</param>
        /// <param name="srcfile">元ファイル</param>
        private void MoveData(string dirpath, string srcfile)
        {
            try
            {
                // ファイルパスが存在する場合のみ動作
                if (!string.IsNullOrWhiteSpace(srcfile) && File.Exists(srcfile))
                {
                    // ディレクトリが無ければ作成する
                    PathManager.CreateDirectory(dirpath);

                    var fileName = System.IO.Path.GetFileName(srcfile);

                    // コピーするファイルパス
                    string srcFilePath = srcfile;
                    // ターゲットファイルパス
                    string tgtFilePath = Path.Combine(dirpath, fileName);

                    // ファイルのコピー（同じ名前のファイルがある場合は上書き）
                    File.Copy(srcFilePath, tgtFilePath, true);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }

        #region カードファイルのロード処理
        /// <summary>
        /// カードファイルのロード処理
        /// </summary>
        /// <param name="file_path">ファイルパス</param>
        private void LoadCardFile(string file_path)
        {
            try
            {
                // 一時フォルダの取得
                string tempDir = Path.GetTempPath();

                // 新しいGUIDを生成する
                var guid = Guid.NewGuid();
                // 一時フォルダ生成
                tempDir = Path.Combine(tempDir, "PromptMaker-" + guid.ToString());

                // zipファイル解凍
                ZipFile.ExtractToDirectory(file_path, tempDir);

                var contentxml = Path.Combine(tempDir, "content.xml");

                var tmp = XMLUtil.Deserialize<CardDataM>(contentxml);

                tmp.BackgroundImage = Path.Combine(tempDir, tmp.BackgroundImage);
                tmp.HologramPath = Path.Combine(tempDir, tmp.HologramPath);
                tmp.ImagePath = Path.Combine(tempDir, tmp.ImagePath);
                Clone<CardDataM>(tmp, this);


                // DirectoryInfoのインスタンスを生成する
                DirectoryInfo di = new DirectoryInfo(tempDir);

                // 元のディレクトリを削除する
                di.Delete(true);
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion
    }
}
