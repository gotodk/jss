using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Com.Seezt.Skins
{
    public class TaskMenu
    {
        private TaskMenu() {
        }
        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        private static extern int GetWindowLong(HandleRef hWnd, int nIndex);
        [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        private static extern IntPtr SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        private static extern bool DeleteMenu(int hMenu, int IDItem, int Flagsw);
        [DllImport("user32.dll")]
        private static extern int GetSystemMenu(int hwnd, int bRevert);

        private const int WS_SYSMENU = 0x00080000;

        public static void Show(Form form)
        {
            int windowLong = (GetWindowLong(new HandleRef(form, form.Handle), -16));

            SetWindowLong(new HandleRef(form, form.Handle), -16, windowLong | WS_SYSMENU | 0x00020000);
            int menu = GetSystemMenu(form.Handle.ToInt32(), 0);

            if (form.FormBorderStyle != FormBorderStyle.Sizable && form.FormBorderStyle != FormBorderStyle.SizableToolWindow)
            {
                DeleteMenu(menu, 0xF000, 0x0);//大小
            }
            if (!form.MinimizeBox)
            {
                DeleteMenu(menu, 0xF020, 0x0);//最小化
            }
            if (!form.MaximizeBox)
            {
                DeleteMenu(menu, 0xF030, 0x0);//最大化
            }
        }
    }
}
