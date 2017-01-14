using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace 主控服务器
{
   public class CRichTestBoxMenu
    {
       public ContextMenuStrip richMenu = new ContextMenuStrip();  
       public ToolStripMenuItem CMcopy = new ToolStripMenuItem("复制");  
       public ToolStripMenuItem CMcut = new ToolStripMenuItem("剪贴");  
       public ToolStripMenuItem CMdel = new ToolStripMenuItem("删除");  
       public ToolStripMenuItem CMcancle = new ToolStripMenuItem("撤销");  
       public ToolStripMenuItem CMpaste = new ToolStripMenuItem("粘贴");  
       public ToolStripMenuItem CMselectall = new ToolStripMenuItem("全选");  
       public ToolStripMenuItem CMalign = new ToolStripMenuItem("右对齐");  
 
       public RichTextBox richTextBox;  
 
       public CRichTestBoxMenu()  
       {  
           init();  
       }  
 
       public CRichTestBoxMenu(RichTextBox rchTBox)  
       {  
           richTextBox = rchTBox;  
           richTextBox.ContextMenuStrip = richMenu;  
           init();  
       }  
 
       public void init()  
       {  
           richMenu.Items.Add(CMcopy);  
           richMenu.Items.Add(CMcut);  
           richMenu.Items.Add(CMdel);  
           richMenu.Items.Add(CMcancle);  
           richMenu.Items.Add(CMpaste);  
           richMenu.Items.Add(CMselectall);  
           richMenu.Items.Add(CMalign);  
 
           CMcopy.Click += CMcopy_Click;  
           CMcut.Click += CMcut_Click;  
           CMdel.Click += CMdel_Click;  
           CMcancle.Click += CMcancle_Click;  
           CMpaste.Click += CMpaste_Click;  
           CMselectall.Click += CMselectall_Click;  
           CMalign.Click += CMalign_Click;  
 
           richMenu.Opened += contextMenuStrip1_Opened;  
       }  
 
       //右键菜单 按钮可见     
       private void contextMenuStrip1_Opened(object sender, EventArgs e)  
       {  
           if (richTextBox.SelectedText.Length > 0)  
           {  
               CMcopy.Enabled = true;  
               CMcut.Enabled = true;  
               CMdel.Enabled = true;  
           }  
           else  
           {  
               CMcopy.Enabled = false;  
               CMcut.Enabled = false;  
               CMdel.Enabled = false;  
           }  
 
           if (richTextBox.CanUndo == true)  
           {  
               this.CMcancle.Enabled = true;  
           }  
           else  
           {  
               this.CMcancle.Enabled = false;  
           }  
 
           if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))  
           {  
               this.CMpaste.Enabled = true;  
           }  
           else  
           {  
               this.CMpaste.Enabled = false;  
           }  
 
           if (richTextBox.Text != "")  
           {  
               CMselectall.Enabled = true;  
           }  
           else  
           {  
               CMselectall.Enabled = false;  
            }  
  
        }  
  
        //右键菜单 撤销     
        private void CMcancle_Click(object sender, EventArgs e)  
        {  
            if (CMcancle.Enabled == true)  
            {  
                richTextBox.Undo();  
                richTextBox.ClearUndo();  
            }  
        }  
  
        //右键菜单剪切     
        private void CMcut_Click(object sender, EventArgs e)  
        {  
            if (CMcut.Enabled == true)  
            {  
                richTextBox.Cut();  
            }  
        }  
  
        //右键菜单 复制     
        private void CMcopy_Click(object sender, EventArgs e)  
        {  
            if (CMcopy.Enabled == true)  
            {  
                richTextBox.Copy();  
            }  
        }  
  
        //右键菜单 粘贴     
        private void CMpaste_Click(object sender, EventArgs e)  
        {  
            if (CMpaste.Enabled == true)  
            {  
                richTextBox.Paste();  
            }  
        }  
  
        //右键菜单 删除     
        private void CMdel_Click(object sender, EventArgs e)  
        {  
            if (CMdel.Enabled == true)  
            {  
                richTextBox.SelectedText = "";  
            }  
        }  
  
        //右键菜单 全选     
        private void CMselectall_Click(object sender, EventArgs e)  
        {  
            richTextBox.SelectAll();  
        }  
  
        //右键菜单 阅读顺序     
        private void CMalign_Click(object sender, EventArgs e)  
        {  
            CMalign.Checked = !CMalign.Checked;  
            if (CMalign.Checked == true)  
            {  
                richTextBox.SelectionAlignment = HorizontalAlignment.Right;  
            }  
            else  
            {  
                richTextBox.SelectionAlignment = HorizontalAlignment.Left;  
            }  
        }  




    }
}
