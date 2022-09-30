using Microsoft.Win32;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using PromptMaker.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using MVVMCore.Common.Wrapper;

namespace PromptMaker.ViewModels
{
    public class CardVM : ViewModelBase
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

        #region 名前[Name]プロパティ
        /// <summary>
        /// 名前[Name]プロパティ用変数
        /// </summary>
        string _Name = "題名2";
        /// <summary>
        /// 名前[Name]プロパティ
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name == null || !_Name.Equals(value))
                {
                    _Name = value;
                    NotifyPropertyChanged("Name");
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


        #region 保存ボタン処理(.png)
        /// <summary>
        /// 保存ボタン処理(.png)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Save(object sender, RoutedEventArgs e)
        {
            var wnd = VisualTreeHelperWrapper.GetWindow<CardV>(sender) as CardV;

            if (wnd != null)
            {
                Microsoft.Win32.SaveFileDialog dlgSave = new Microsoft.Win32.SaveFileDialog();

                dlgSave.Filter = "PNGファイル(*.png)|*.png";
                dlgSave.AddExtension = true;

                if ((bool)dlgSave.ShowDialog()!)
                {
                    // レンダリング
                    var bmp = new RenderTargetBitmap(
                        (int)(wnd.card_border.ActualWidth),
                        (int)(wnd.card_border.ActualHeight),
                        96, 96, // DPI
                        PixelFormats.Pbgra32);
                    bmp.Render(wnd.card_border);

                    // jpegで保存
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    using (var fs = File.Open(dlgSave.FileName, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }
                }
            }
        }
        #endregion



        public override void Init(object sender, EventArgs e)
        {
        }

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

        public override void Close(object sender, EventArgs e)
        {
        }
    }
}
