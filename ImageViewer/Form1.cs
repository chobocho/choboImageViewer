using ImageViewer.Properties;
using System.Windows.Forms;

namespace ImageViewer
{
    public partial class ImageViewerForm : Form
    {
        private string ImageFileName = string.Empty;

        public ImageViewerForm(string[] args)
        {
            InitializeComponent();
            if (args != null && args.Length > 0)
            {
                ImageFileName = args[0];
            }
        }

        private void ImageViewerForm_Load(object sender, EventArgs e)
        {
            if (ImageFileName == string.Empty || !File.Exists(ImageFileName))
            {
                applyDefaultImage();
                return;
            }

            applyImage(ImageFileName);
        }

        void ImageViewrForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void ImageViewrForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null || !File.Exists(files[0])) { applyDefaultImage(); }
            applyImage(files[0]);
        }

        void applyImage(string filename)
        {
            var LoadedImage = Image.FromFile(filename);
            pictureBox.BackgroundImage = LoadedImage;

            Width = LoadedImage.Width > 256 ? LoadedImage.Width : 256;
            Width = LoadedImage.Width > 1024 ? 1024 : LoadedImage.Width;

            Height = LoadedImage.Height > 256 ? LoadedImage.Height : 256;
            Height = LoadedImage.Height > 1024 ? 1024 : LoadedImage.Height;
        }

        void applyDefaultImage()
        {
            var LoadedImage = Properties.Resources.default_image;
            pictureBox.BackgroundImage = LoadedImage;
        }

        void rotateImage(System.Drawing.RotateFlipType angle)
        {
            var LoadedImage = pictureBox.BackgroundImage;
            LoadedImage.RotateFlip(angle);
            pictureBox.BackgroundImage = LoadedImage;

            Width = LoadedImage.Width > 256 ? LoadedImage.Width : 256;
            Width = LoadedImage.Width > 1024 ? 1024 : LoadedImage.Width;

            Height = LoadedImage.Height > 256 ? LoadedImage.Height : 256;
            Height = LoadedImage.Height > 1024 ? 1024 : LoadedImage.Height;
        }

        private void ImageViewForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.R)
            {
                rotateImage(RotateFlipType.Rotate270FlipXY);
            } else if (e.KeyData == Keys.L)
            {
                rotateImage(RotateFlipType.Rotate90FlipXY);
            }

        }
    }
}