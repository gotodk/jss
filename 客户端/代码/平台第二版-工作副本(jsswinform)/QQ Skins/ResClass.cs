using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace Com.Seezt.Skins
{
    public class ResClass
    {
        public static Bitmap GetResObj(string name)
        {
            if (name == null || name == "")
                return null;
            return (Bitmap)Com.Seezt.Skins.Properties.Resources.ResourceManager.GetObject(name);
        }

        public static Image GetPNG(string name)
        {
            if (name == null || name == "")
                return null;
            return (Image)Com.Seezt.Skins.Properties.Resources.ResourceManager.GetObject(name);
        }

        public static Icon GetResToIcon(string name)
        {
            if (name == null || name == "")
                return null;
            return (Icon)Com.Seezt.Skins.Properties.Resources.ResourceManager.GetObject(name);
        }

        public static Icon GetIcon(string name)
        {
            if (name == null || name == "")
                return null;
            return (Icon)Com.Seezt.Skins.Properties.Resources.ResourceManager.GetObject(name);
        }

        public static Image GetResToImage(string name)
        {
            if (name == null || name == "")
                return null;
            return (Bitmap)Com.Seezt.Skins.Properties.Resources.ResourceManager.GetObject(name);
        }

        public static string[] GetAllFace(string name)
        {
            return null;
        }

        public static Bitmap GetHead(string name)
        {
            if (name == null || name == "")
                name = "big1";
            return (Bitmap)Com.Seezt.Skins.Properties.Resources.ResourceManager.GetObject(name);
        }

        public static string[] GetAllHead(string name)
        {
            return null;
        }
    }
}
