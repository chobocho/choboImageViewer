using ImageViewer.Properties;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ImageViewer
{
    public partial class ImageViewerForm : Form
    {
        private const string version = "Chobocho's Image Viewer V0.11";
        private string ImageFileName = string.Empty;

        private const int minimumSize = 256;
        private const int maximumSize = 800;

        private IFileManager _fileManager = new FileManager();

        public ImageViewerForm(string[] args)
        {
            InitializeComponent();
            if (args != null && args.Length > 0)
            {
                ImageFileName = args[0];
            }

            Text = version;
        }

        private void ImageViewerForm_Load(object sender, EventArgs e)
        {
            if (ImageFileName == string.Empty || !File.Exists(ImageFileName))
            {
                applyDefaultImage();
                return;
            }

            applyImage(ImageFileName);
            _fileManager.setFileName(ImageFileName);
        }

        void ImageViewrForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void ImageViewrForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null || !File.Exists(files[0]))
            {
                applyDefaultImage();
            }

            applyImage(files[0]);
            _fileManager.setFileName(files[0]);
        }

        void applyImage(string filename)
        {
            if (filename == null || filename == string.Empty) return;
            var LoadedImage = Image.FromFile(filename);
            pictureBox.BackgroundImage = LoadedImage;

            var width = LoadedImage.Width > minimumSize ? LoadedImage.Width : minimumSize;
            width = LoadedImage.Width > maximumSize ? maximumSize : width;

            var height = LoadedImage.Height > minimumSize ? LoadedImage.Height : minimumSize;
            height = LoadedImage.Height > maximumSize ? maximumSize : height;

            this.Width = width;
            this.Height = height;

            Text = filename;
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

            var width = LoadedImage.Width > minimumSize ? LoadedImage.Width : minimumSize;
            width = LoadedImage.Width > maximumSize ? maximumSize : width;

            var height = LoadedImage.Height > minimumSize ? LoadedImage.Height : minimumSize;
            height = LoadedImage.Height > maximumSize ? maximumSize : height;

            this.Width = width;
            this.Height = height;

            pictureBox.Refresh();
        }

        private void ImageViewForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.L: 
                    rotateImage(RotateFlipType.Rotate90FlipXY);
                    break;
                case Keys.R: 
                    rotateImage(RotateFlipType.Rotate270FlipXY);
                    break;
                case Keys.Left: 
                    applyImage(_fileManager.prev());
                    break;
                case Keys.Right: 
                    applyImage(_fileManager.next());
                    break;
                case Keys.Up:
                    applyImage(_fileManager.head());
                    break;
                case Keys.Down:
                    applyImage(_fileManager.tail());
                    break;
                default:
                    break;
            }
        }
    }
}