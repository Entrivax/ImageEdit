using ImageEdit.Maths;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageEdit
{
    public partial class Form1 : Form
    {
        private List<IPlugin> _plugins;

        private IPlugin _selectedPlugin;

        private Bitmap OriginalImage;
        private Bitmap ComputedImage;

        public Form1()
        {
            _plugins = new List<IPlugin>();
            InitializeComponent();
            FileSystemWatcher watcher = new FileSystemWatcher("Plugins");
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.dll";
            watcher.Changed += ReloadPlugins;
            watcher.EnableRaisingEvents = true;

            ReloadPlugins(null, null);
        }

        private void ReloadPlugins(object sender, FileSystemEventArgs e)
        {
            _plugins.Clear();
            var pluginPaths = Directory.EnumerateFiles("Plugins");
            foreach (var pluginPath in pluginPaths)
            {
                if (!pluginPath.ToLower().EndsWith(".dll"))
                    continue;
                var assembly = Assembly.LoadFrom(pluginPath);
                Type pluginType = null;
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.GetInterfaces().Contains(typeof(IPlugin)))
                    {
                        pluginType = type;
                        try
                        {
                            var plugin = (IPlugin)Activator.CreateInstance(pluginType);
                            _plugins.Add(plugin);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(this, $"Error loading plugin {pluginType} : {exception.Message}{Environment.NewLine}{exception.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            foreach (var plugin in _plugins)
            {
                CurrentPlugin.Items.Add(plugin.Name);
            }
        }

        private void CurrentPlugin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentPlugin.SelectedIndex == -1)
                return;
            _selectedPlugin = _plugins[CurrentPlugin.SelectedIndex];

            ComputeAndShow();
        }

        private void ComputeAndShow()
        {
            PictureBox.Image = null;

            ComputeImage();
        }

        private void ComputeImage()
        {
            ComputedImage?.Dispose();
            ComputedImage = null;
            if (OriginalImage == null)
            {
                PictureBox.Image = null;
                return;
            }
            if (_selectedPlugin == null)
            {
                PictureBox.Image = OriginalImage;
                return;
            }
            ComputedImage = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            using (var lockedOriginal = new LockBitmap(OriginalImage))
            using (var lockedComputed = new LockBitmap(ComputedImage))
            {
                //for(int i = 0; i < lockedOriginal.Width + lockedOriginal.Height; i++)
                Parallel.For(0, lockedOriginal.Width * lockedOriginal.Height, i =>
                    {
                        var iX = i % lockedOriginal.Width;
                        var iY = i / lockedOriginal.Width;
                        var position = new Vector2(iX / (float)(lockedOriginal.Width - 1), iY / (float)(lockedOriginal.Height - 1));
                        lockedComputed.SetPixel(iX, iY, _selectedPlugin.Compute(position, lockedOriginal));
                    }
                );
            }
            PictureBox.Image = ComputedImage;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                CheckFileExists = true,
                Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp",
                Multiselect = false
            };
            var result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (File.Exists(ofd.FileName))
                {
                    try
                    {
                        var imageToLoad = new Bitmap(ofd.FileName);
                        OriginalImage?.Dispose();
                        OriginalImage = imageToLoad;
                    }
                    catch
                    {
                        MessageBox.Show(this, "Failed to open image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            ComputeAndShow();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ComputedImage != null)
            {
                var sfd = new SaveFileDialog()
                {
                    Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp",
                    CheckPathExists = true,
                };
                var result = sfd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ComputedImage.Save(sfd.FileName);
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var whd = new WidthHeightDialog(200, 200);
            var result = whd.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                OriginalImage?.Dispose();
                OriginalImage = new Bitmap(whd.SelectedWidth, whd.SelectedHeight);
                ComputeAndShow();
            }
        }
    }
}
