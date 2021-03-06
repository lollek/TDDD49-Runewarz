﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RuneWarz
{
    public partial class Runewarz : Form
    {
        // See HandleMouseDown()
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private GamePanel GamePanel;
        private TitleBar TitleBar;
        private MainMenu GUIPanel;

        public Runewarz()
        {
            this.GamePanel = new GamePanel();
            this.TitleBar = new TitleBar(this);
            this.GUIPanel = new MainMenu();
            this.SuspendLayout();

            this.AccessibleName = "Runewarz";
            this.Name = "Runewarz";
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowIcon = false;
            this.Text = "Runewarz";

            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 630);
            this.ControlBox = false;

            this.TitleBar.MouseDown += HandleMouseDown;

            this.GUIPanel.NewGameButton.MouseClick += HandleMouseClick;
            this.GUIPanel.ResumeGameButton.MouseClick += HandleMouseClick;
            this.GUIPanel.QuitButton.MouseClick += HandleMouseClick;

            this.Controls.Add(this.GamePanel);
            this.Controls.Add(this.TitleBar);
            this.Controls.Add(this.GUIPanel);

            this.KeyPreview = true;
            this.KeyDown += HandleKeyPress;

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void HandleMouseClick(object sender, MouseEventArgs e)
        {
            if (sender.Equals(this.GUIPanel.ResumeGameButton))
            {
                this.GUIPanel.Visible = false;
                this.GamePanel.Visible = true;
                this.GamePanel.ResumeGame();
            }
            else if (sender.Equals(this.GUIPanel.NewGameButton))
            {
                this.GUIPanel.Visible = false;
                this.GamePanel.Visible = true;
                this.GamePanel.StartNewGame();
            }
            else if (sender.Equals(this.GUIPanel.QuitButton))
                Application.Exit();
        }

        private void HandleMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)            {
                // Drag the window while holding down the left mouse button
                // ReleaseCapture() releases the Window's control of the mouse
                // SendMessage() Sends a custom event that we hand craft, 
                //  in this case it's a MouseDown event on the Title Bar (which is hidden)
                const int WM_NCLBUTTONDOWN = 0x00A1; // User MouseDown Event
                const int HTCAPTION = 0x0002;        // Title Bar
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.GamePanel.Visible = false;
                this.GUIPanel.Visible = true;
            }
        }

    }
}
