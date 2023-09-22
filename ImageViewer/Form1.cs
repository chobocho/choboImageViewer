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

            if (pictureBox.BackgroundImage != null)
            {
                pictureBox.BackgroundImage.Dispose();
                pictureBox.BackgroundImage = null;
            }

            var LoadedImage = Image.FromFile(filename);
            pictureBox.BackgroundImage = LoadedImage;

            resizeWindow(LoadedImage.Width, LoadedImage.Height);

            Text = filename;
        }

        private void resizeWindow(int imageWidth, int imageHeight)
        {
            if (imageWidth > minimumSize && imageWidth < maximumSize &&
                imageHeight > minimumSize && imageHeight < maximumSize)
            {
                this.Width = imageWidth;
                this.Height = imageHeight;
                return;
            }
            
            if (imageWidth < imageHeight)
            {
                this.Width = imageWidth * maximumSize / imageHeight;
                this.Height = maximumSize;
                return;
            }
            
            this.Width = maximumSize;
            this.Height = imageHeight * maximumSize / imageWidth;
        }

        void applyDefaultImage()
        {
            var defaultImage = Properties.Resources.default_image;
            pictureBox.BackgroundImage = defaultImage;
        }

        void rotateImage(System.Drawing.RotateFlipType angle)
        {
            var loadedImage = pictureBox.BackgroundImage;
            loadedImage?.RotateFlip(angle);

            pictureBox.BackgroundImage = loadedImage;
            resizeWindow(loadedImage.Width, loadedImage.Height);
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