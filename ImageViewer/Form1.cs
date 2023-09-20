using ImageViewer.Properties;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ImageViewer
{
    public partial class ImageViewerForm : Form
    {
        private string ImageFileName = string.Empty;

        private const int  minimumSize = 256;
        private const int  maximumSize = 800;

        private bool isProcessing = false;

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

            var width = LoadedImage.Width > minimumSize ? LoadedImage.Width : minimumSize;
            width = LoadedImage.Width > maximumSize ? maximumSize : LoadedImage.Width;

            var height = LoadedImage.Height > minimumSize ? LoadedImage.Height : minimumSize;
            height = LoadedImage.Height > maximumSize ? maximumSize : LoadedImage.Height;

            this.Width = width;
            this.Height = height;

            isProcessing = false;
        }

        void applyDefaultImage()
        {
            var LoadedImage = Properties.Resources.default_image;
            pictureBox.BackgroundImage = LoadedImage;
            isProcessing = false;
        }

        void rotateImage(System.Drawing.RotateFlipType angle)
        {
            var LoadedImage = pictureBox.BackgroundImage;
            LoadedImage.RotateFlip(angle);
            pictureBox.BackgroundImage = LoadedImage;

            this.Width = LoadedImage.Width;
            this.Height = LoadedImage.Height;
        }

        private void ImageViewForm_KeyDown(object sender, KeyEventArgs e)
        {
            /**
             * By speed issue, block this code for temporary
             * 
            if (isProcessing) return;

            isProcessing = true;
            if (e.KeyData == Keys.R)
            {
                rotateImage(RotateFlipType.Rotate270FlipXY);
            } else if (e.KeyData == Keys.L)
            {
                rotateImage(RotateFlipType.Rotate90FlipXY);
            }
            isProcessing = false;
            */
        }
    }
}