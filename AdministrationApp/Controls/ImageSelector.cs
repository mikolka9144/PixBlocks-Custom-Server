using System;
using Xwt;
using Xwt.Drawing;

namespace AdministrationApp.Controls
{
    public class ImageSelector:VBox
    {
        private ImageView image = new ImageView
        ();
        private FileSelector file = new FileSelector
        {
            Name = "Select Image",
        };
        public ImageSelector()
        {
            file.Filters.Add(new FileDialogFilter("Pictures", "*.png", "*.jpg"));
            file.FileChanged += (sender, e) => {
                var base_image = Image.FromFile(file.FileName);
                image.Image = Utills.TransformImage(base_image);
            };
            PackStart(image);
            PackStart(file);
        }

        public Image Image { get => image.Image; set => image.Image = value; }
    }
}
