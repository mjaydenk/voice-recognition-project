using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Windows.Input;
using System.Runtime.InteropServices;
//using System.IO;

namespace Voice_rec_program
{

    public partial class pickPointFrm : Form
    {
        //used in mouseLeftClick. Has to be declared outside the event, otherwise
        // it can't rewrite the amount (don't know why).
        public int pointNum = 1;
        fileControl flcn = new fileControl();
        bool leftDone = true;

        private void mouseLeftClick (object sender, MouseEventArgs e)
        {
            AppDomain root = AppDomain.CurrentDomain;
            string directory = root.BaseDirectory.ToString();
            directory = flcn.cleanDirectory(directory) + "\\system\\screenPointPosition.txt";

            //if (flcn.writeToFile("HELLO", directory) == true)
            //{
            //    MessageBox.Show("YES");
            //}
            //else
            //{
            //    MessageBox.Show("NO");
            //}

            //attempt to lessen RAM usage.
            if (e.Button == MouseButtons.Left && leftDone == true)
            {
                int[] mousePositionXY = new int[2] {0, 0};
                //do stuff
                mousePositionXY[0] = MousePosition.X;
                mousePositionXY[1] = MousePosition.Y;
                switch (pointNum)
                {
                    //write the coordiantes into the three labels.
                    case 1:
                        {
                            label1.Text = ("Point A: X: " + mousePositionXY[0] + " Y: " + mousePositionXY[1]);
                            xyz1ChoosePicbx.BackgroundImage = Properties.Resources.tik_mark;
                            pointNum = 2;
                            //string the = mousePositionXY[0].ToString() + ' ' + mousePositionXY[1].ToString();
                            if (flcn.writeToFile(mousePositionXY[0].ToString() + '*' + mousePositionXY[1].ToString(), directory))
                            {
                                
                            }
                            else
                            {

                                MessageBox.Show("An error has occured and the porgram has to close");
                                Application.Exit();
                            }
                            break;
                        }
                    case 2:
                        {
                            label2.Text = ("Point A: X: " + mousePositionXY[0] + " Y: " + mousePositionXY[1]);
                            xyz2ChoosePicbx.BackgroundImage = Properties.Resources.tik_mark;
                            pointNum = 3;
                            if (flcn.writeToFile(mousePositionXY[0].ToString() + '*' + mousePositionXY[1].ToString(), directory))
                            {
                                
                            }
                            else
                            {

                                MessageBox.Show("An error has occured and the program has to close,");
                                Application.Exit();
                            }
                            break;
                        }
                    case 3:
                        {
                            xyzPoint3Lbl.Text = ("Point A: X: " + mousePositionXY[0] + " Y: " + mousePositionXY[1]);
                            point3ChoosePicbx.BackgroundImage = Properties.Resources.tik_mark;
                            this.WindowState = FormWindowState.Normal;
                            this.FormBorderStyle = FormBorderStyle.Sizable;
                            pickPointFrm.ActiveForm.Opacity = 1;
                            pointSelect_btn.Enabled = true;
                            if (flcn.writeToFile(mousePositionXY[0].ToString() + '*' + mousePositionXY[1].ToString(), directory))
                            {
                                
                            }
                            else
                            {

                                MessageBox.Show("An error has occured and the program has to close");
                                Application.Exit();
                            }
                            //attempt to lessen RAM usage.
                            leftDone = false;
                            break;
                        }
                }
            }
            return;
        }

        public pickPointFrm()
        {
            InitializeComponent();
        }

        [DllImport("user32")]
        public static extern void mouse_event(int dwFlag, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        public static void Tri_Click (int x, int y)
        {
            //Console.SetCursorPosition(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }

        private void pointSelect_btn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            pickPointFrm.ActiveForm.Opacity = .5;
            //Starts mouse event that detects and records mouse click locations.
            flcn.clickPoint = fileControl.ClickPoints.reset;
            this.MouseClick += mouseLeftClick;
            pointSelect_btn.Enabled = false;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void xyz1ChoosePicbx_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void xyz2ChoosePicbx_Click(object sender, EventArgs e)
        {

        }

        private void xyzPoint3Lbl_Click(object sender, EventArgs e)
        {

        }

        private void point3ChoosePicbx_Click(object sender, EventArgs e)
        {

        }

        private void ext_btn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pickPointFrm_Load(object sender, EventArgs e)
        {

            //new speechRecognition instance
            SpeechRecognizer sr = new SpeechRecognizer();

            Choices recWords = new Choices();
            recWords.Add(new string[] { "restart" });

            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(recWords);

            Grammar g = new Grammar(gb);

            sr.LoadGrammar(g);
            sr.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
        }

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "restart")
            {
                AppDomain fileRoot = AppDomain.CurrentDomain;
                fileControl pointFile = new fileControl();
                string fileDomain = fileRoot.BaseDirectory.ToString();
                fileDomain = pointFile.cleanDirectory(fileDomain) + "\\system\\screenPointPosition.txt";
                string[] screenPointStrArry = pointFile.readScreenPointFile(fileDomain);
                if (screenPointStrArry == null || screenPointStrArry[0] == "")
                {
                    MessageBox.Show("An error has occured and the program has crashed");
                    Application.Exit();
                }

                int[] screenPointXY1 =pointFile.parsPointIntxy(screenPointStrArry[0]);

                //first point clicked
                this.Cursor = new Cursor(Cursor.Current.Handle);
                Cursor.Position = new Point(screenPointXY1[0], screenPointXY1[1]);
                Tri_Click(screenPointXY1[0], screenPointXY1[1]);
                System.Threading.Thread.Sleep(100);

                int[] screenPointXY2 = pointFile.parsPointIntxy(screenPointStrArry[1]);

                //second point clicked
                this.Cursor = new Cursor(Cursor.Current.Handle);
                Cursor.Position = new Point(screenPointXY2[0], screenPointXY2[1]);
                Tri_Click(screenPointXY2[0], screenPointXY2[1]);
                System.Threading.Thread.Sleep(100);

                int[] screenPointXY3 = pointFile.parsPointIntxy(screenPointStrArry[2]);

                //second point clicked
                this.Cursor = new Cursor(Cursor.Current.Handle);
                Cursor.Position = new Point(screenPointXY3[0], screenPointXY3[1]);
                Tri_Click(screenPointXY3[0], screenPointXY3[1]);
                System.Threading.Thread.Sleep(100);

            }
        }
    }

}
