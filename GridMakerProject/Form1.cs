﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace GridMakerProject
{

    public partial class Form1 : Form
    {
        private GridDrawer drawer;                      //그리드를 실질적으로 그리는 객체
        private GridSetting gridSet;                //form 클래스와 GridDrawer 클래스 간 매개변수로 사용되는 그리드의 설정
        private Color cl = Color.Black;                 //기본 색상값
        private Dictionary<Button, Align> alignButtons; //정렬 버튼 컬렉션
        private Dictionary<RadioButton, Button> modeButtons; //정렬 버튼 컬렉션
        private bool suppressValueChanged= false;
        public bool isSubGridChecked = false;
        public static GridSetting subGridSet;
        private string lastChange = "행";
        private int previousTrackBarValue=0; // 트랙바의 이전 값을 저장하기 위한 변수

        public Form1()
        {
            InitializeComponent();
            drawer = new GridDrawer();
            InitializeAlignButtons();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;
        }
        private void RemovePaintEventHandlers() // pictureBox에 등록된 이벤트를 삭제하는 함수
        {
            FieldInfo fieldInfo = typeof(Control).GetField("EventPaint", BindingFlags.Static | BindingFlags.NonPublic);
            if (fieldInfo != null)
            {
                object eventKey = fieldInfo.GetValue(pictureBox1);
                PropertyInfo eventsProperty = typeof(Control).GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
                if (eventsProperty != null)
                {
                    EventHandlerList eventHandlers = (EventHandlerList)eventsProperty.GetValue(pictureBox1, null);
                    eventHandlers.RemoveHandler(eventKey, eventHandlers[eventKey]);
                }
            }
        }

        private void DrawGrid(GridType gridType, Align align = Align.Center)
        /* 그리드 생성 버튼을 등을 입력받았을 때 호출되는 함수, 그리드를 그려줍니다 */
        {
            if (!isImageExist() || !isValidData(gridType, align))
            /* pictureBox에 이미지가 없거나 그리드 설정에 대한 값이 정상적이지 않는다면 함수를 종료합니다. */
            {
                return;
            }
            RemovePaintEventHandlers();
            /* 이전에 등록한 paint 이벤트를 삭제하는 함수입니다. 
             * 함수를 처음 호출할때는 있어야할 이유가 없지만, 새로운 그리드를 그리고 싶을 때 이 동작이 없다면 
             * 이전에 그려진 그리드도 중복되어 같이 그려지게 됩니다.
             * 따라서 이벤트를 삭제해주어야 합니다. */
            pictureBox1.Paint += new PaintEventHandler((s, e) => drawer.DrawGrid(e, gridType,this.pictureBox1));
            /* 사용자가 입력한 gridType에 맞는 그리드를 그려주는 함수를 pictureBox.paint 이벤트에 등록해줍니다. 
             * pictureBox가 Refresh() 될때마다 drawer클래스의 DrawGrid() 함수가 실행되어 그리드를 그리도록 하는 역할을 합니다. 
             * 예를들어 윈도우창 크기를 변경했을때도 paint 이벤트가 발생하여 그리드를 유지해주는 역할을 합니다. */
            pictureBox1.Refresh();
            /* 이벤트를 등록한 뒤에 새로고침 하여 drawer.DrawGrid()함수를 호출해서 
             * 그리드를 그려주어야 하기 때문에 pictureBox를 새로고침을 합니다.*/
        }
        private bool isSquare() // 그리드가 정사각형 그리드로 설정되어있는지 확인하는 함수
        {
            if (gridSet.gridType == GridType.Squares)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isValidData(GridType gridType, Align align) // 유효한 데이터가 올바르게 입력되었는지 확인하는 함수
        {
            if (int.TryParse(numericUpDown3.Value.ToString(), out int rows)
                && int.TryParse(numericUpDown4.Value.ToString(), out int columns)
                && float.TryParse(numericUpDown1.Value.ToString(), out float lineWeight)
                && float.TryParse(numericUpDown2.Value.ToString(), out float lineOpacity))
            {
                lineOpacity *= 0.01f;
                gridSet = new GridSetting(rows, columns, gridType, cl, align, lineWeight, lineOpacity);
                
                drawer.SetGrid(gridSet);
                return true;
            }
            else
            {
                MessageBox.Show($"입력 데이터가 올바르지 않습니다.");
                return false;
            }
        }

        private bool isImageExist() //이미지가 pictureBox에 존재하는지 확인하는 함수
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show($"이미지가 존재하지 않습니다.");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void InitializeAlignButtons() 
        {
        /* 
         * Form1 생성자에서 실행되며 정렬 버튼에 클릭 이벤트를 등록해준다.
         * 동일한 자료형에 동일한 기능을 하기 때문에 컬렉션으로 만들어 코드의 중복성을 줄이고 유지보수성을 높였다.
         */
            alignButtons = new Dictionary<Button, Align>
            //각 버튼에 대해서 상세 정렬 기능을 설정해줌
            {
                { button9, Align.Left },    //왼쪽 맞춤 정렬
                { button10, Align.Right },  //오른쪽 맞춤 정렬
                { button11, Align.Center }, //중앙 맞춤 정렬
                { button12, Align.Top },    //상단 맞춤 정렬
                { button13, Align.Bottom }, //하단 맞춤 정렬
                { button14, Align.Center }  //중앙 맞춤 정렬
            };

            modeButtons = new Dictionary<RadioButton, Button>
            {
                {radioButton1 , button8},    
                {radioButton2, button2 },
                {radioButton4, button3  },   
                {radioButton5, button4  },
                {radioButton6, button5  }  
            };

            foreach (var button in alignButtons.Keys)
            //각 버튼에 대해서 클릭이벤트를 설정해줌
            {
                button.Click += AlignButton_Click;
            }

            foreach (var bt in modeButtons.Keys)
            //각 버튼에 대해서 클릭이벤트를 설정해줌
            {
                bt.CheckedChanged += radioBTCheckChange;
            }


        }
        private void radioBTCheckChange(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button != null && modeButtons.TryGetValue(button,out Button b))
            {
                foreach (var bt in modeButtons.Values)
                {
                    if(bt == b)
                    {
                        if(b.Name=="button8" || b.Name == "button2")
                        {
                            numericUpDown3.Enabled = true;
                            numericUpDown4.Enabled = true;
                            foreach (var alb in alignButtons.Keys)
                            {
                                alb.Enabled = true;
                            }
                            button6.Enabled = true;
                            groupBox7.Enabled = true;
                        }
                        else if(b.Name == "button3")
                        {
                            numericUpDown3.Enabled = true;
                            numericUpDown4.Enabled = true;
                            foreach (var alb in alignButtons.Keys)
                            {
                                alb.Enabled = false;
                            }
                            button6.Enabled = false;
                            groupBox7.Enabled = false;
                        }
                        else if (b.Name == "button4")
                        {
                            numericUpDown3.Enabled = true;
                            numericUpDown4.Enabled = false;
                            foreach (var alb in alignButtons.Keys)
                            {
                                alb.Enabled = false;
                            }
                            button6.Enabled = false;
                            groupBox7.Enabled = false;
                        }
                        else if (b.Name == "button5")
                        {
                            numericUpDown3.Enabled = false;
                            numericUpDown4.Enabled = true;
                            foreach (var alb in alignButtons.Keys)
                            {
                                alb.Enabled = false;
                            }
                            button6.Enabled = false;
                            groupBox7.Enabled = false;
                        }
                        b.Enabled = true;
                        continue;
                    }
                    else
                    {
                        bt.Enabled = false;
                    }

                }
            }
        }
        private void gridSizeChange()
        {
            KeyValuePair<PointF, float> KVP = drawer.getCurrentGridSize();

            if (KVP.Key != null)
            {
                label1.Text = $"넓이 : {(int)(KVP.Key).X}";
                label2.Text = $"높이 : {(int)(KVP.Key).Y}";
            }
            trackBar1.Maximum = (int)(KVP.Value);
            trackBar1.Value = (int)(KVP.Value);
        }
        private void AlignButton_Click(object sender, EventArgs e) 
        {
            // 정렬 버튼 클릭 이벤트 발생시 실행되는 함수
            if (!isImageExist() || !isSquare()) return;

            Button button = sender as Button;
            // 클릭한 버튼이 무엇인지 가져온다.
            
            if (button != null && alignButtons.TryGetValue(button, out Align align))
            // 클릭한 버튼에 알맞는 값을 가져온 뒤 DrawGrid()함수의 인자로 넘겨준다.
            {
                DrawGrid(GridType.Squares, align);
            }
        }

        private void button1_Click(object sender, EventArgs e) // 파일열기 버튼 클릭 이벤트 발생시 실행되는 함수
        {
            if (!isImageExist())
            {
                return;
            }
            RemovePaintEventHandlers();
            pictureBox1.Refresh();
        }
        private void 모두지우기ToolStripMenuItem_Click(object sender, EventArgs e) //그리드 모두 지우기 메뉴 눌렀을때 실행되는 함수
        {
            if (!isImageExist())
            {
                return;
            }
            RemovePaintEventHandlers();
            pictureBox1.Refresh();
        }
        private void 파일열기ToolStripMenuItem_Click(object sender, EventArgs e)
        // 사용자가 파일 열기 메뉴를 클릭했을 때 실행되는 함수
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // 파일 열기 대화 상자를 생성합니다.
            openFileDialog1.InitialDirectory = @"C:\";
            // 대화 상자가 처음 열릴 때 기본 디렉토리를 C:\로 설정합니다.
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            // 사용자가 선택할 수 있는 파일 유형을 이미지 파일로 제한합니다.
            openFileDialog.Title = "파일 열기";
            // 파일 열기 대화 상자의 제목을 설정합니다.
            openFileDialog1.Multiselect = false;
            // 사용자가 한 번에 하나의 파일만 선택할 수 있도록 설정합니다.
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                RemovePaintEventHandlers();
                /* 이전에 picturebox1.paint에 등록한 이벤트를 삭제하는 함수입니다. 
                 * 함수를 처음 호출할때는 있어야할 이유가 없지만, 새로운 그리드를 그리고 싶을 때 이 동작이 없다면 
                 * 이전에 그려진 그리드도 중복되어 같이 그려지게 됩니다.
                 * 따라서 이벤트를 삭제해주어야 합니다. */

                string imagePath = openFileDialog.FileName;
                // 사용자가 선택한 파일의 경로를 가져옵니다.

                Image image = Image.FromFile(imagePath);
                // 파일 경로를 이용해 이미지를 로드합니다.

                pictureBox1.Image = image;
                // pictureBox1에 로드한 이미지를 설정합니다.

                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                // pictureBox1의 이미지 맞춤 설정을 Zoom으로 설정합니다.

            }
        }

        private void 저장ToolStripMenuItem_Click(object sender, EventArgs e) // 저장 메뉴 늘렀을때 실행되는 함수
        {
            if (!isImageExist())
            {
                return;
            }
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG Files|*.png|JPEG Files|*.jpg|Bitmap Files|*.bmp";
                saveFileDialog.Title = "파일 저장";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    PictureBox saveBox = new PictureBox();
                    saveBox.Image = pictureBox1.Image;
                    saveBox.SizeMode = PictureBoxSizeMode.AutoSize;
                    this.Controls.Add(saveBox);                             //컨트롤로 추가되어야 정상적인 이미지가 출력됨
                    string filePath = saveFileDialog.FileName;
                    drawer.CaptureGrid(filePath, saveBox, pictureBox1);
                    this.Controls.Remove(saveBox);
                    DrawGrid(gridSet.gridType, Align.None);
                    MessageBox.Show($"이미지가 {filePath}에 저장되었습니다.", "저장 완료");
                }
            }
        }
        private void 끝내기ToolStripMenuItem_Click(object sender, EventArgs e) //끝내기 메뉴 눌렀을때 실행되는 함수
        {
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e) //행 x 열 정사각형 그리기 버튼 클릭시 실행되는 함수
        {
            if (!isImageExist())
            {
                return;
            }
            
            DrawGrid(GridType.Squares,Align.Center);
            gridSizeChange();
            trackBar1_Scroll(sender, e);
        }
        private void button3_Click(object sender, EventArgs e) //행 x 열 직사각형 그리기 버튼 클릭시 실행되는 함수
        {
            if (!isImageExist())
            {
                return;
            }
            DrawGrid(GridType.Rectangles);
        }
        
        private void button4_Click(object sender, EventArgs e) //행의 개수만큼 수평선 그리기 버튼 클릭시 실행되는 함수
        {

            if (!isImageExist())
            {
                return ;
            }
            
            numericUpDown4.Value = 0;
            DrawGrid(GridType.HorizontalLines);
        }

        private void button5_Click(object sender, EventArgs e) //열의 개수만큼 수직선 그리기 버튼 클릭시 실행되는 함수
        {
            if (!isImageExist())
            {
                return ;
            }
            numericUpDown3.Value = 0;
            DrawGrid(GridType.VerticalLines);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) // 마우스 down 이벤트 발생시 실행되는 함수
        {
            if (!isImageExist())
            {
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                string s =drawer.getSubGridLocation(e.Location,isSubGridChecked,subGridSet);
                if (s == "체크")
                {
                    isSubGridChecked = true;
                }
                else if(s =="미체크")
                {
                    isSubGridChecked = false;
                }
                pictureBox1.Refresh();
            }
            else
            {
                drawer.StartDragging(e.Location);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) // picturebox에서 마우스 움직이며 드래그시 실행되는 함수
        {
            
            if (e.Button == MouseButtons.Left)
            {
                drawer.Drag(e.Location,this.pictureBox1,!checkBox1.Checked);
            }
        }
        
        private void button8_Click(object sender, EventArgs e) //행 기반 정사각형 그리기 버튼 클릭시 실행되는 함수
        {
            if(!isImageExist())
            {
                return;
            }
            
            try
            {
                drawer.setImageOffset(this.pictureBox1);
                int newCols = drawer.CalculateColumnsForRows((int)numericUpDown3.Value);

                numericUpDown4.ValueChanged -= numericUpDown4_ValueChanged;
                numericUpDown4.Value = newCols;
                numericUpDown4.ValueChanged += numericUpDown4_ValueChanged;

                DrawGrid(GridType.Squares, Align.Center);
                gridSizeChange();
                trackBar1_Scroll(sender, e);

            }
            catch (Exception fe)
            {
                MessageBox.Show($"행이 존재하지 않습니다.");
            }
        }
        private void button15_Click(object sender, EventArgs e) //열 기반 정사각형 그리기 버튼 클릭시 실행되는 함수
        {
            if (!isImageExist())
            {
                return;
            }
            try
            {
                drawer.setImageOffset(this.pictureBox1);
                int newRows = drawer.CalculateRowsForColumns((int)numericUpDown4.Value);
                numericUpDown3.ValueChanged -= numericUpDown3_ValueChanged;
                numericUpDown3.Value = newRows;
                numericUpDown3.ValueChanged += numericUpDown3_ValueChanged;
                DrawGrid(GridType.Squares, Align.Center);
                gridSizeChange();
                trackBar1_Scroll(sender, e);
            }
            catch (Exception fe)
            {
                MessageBox.Show($"열이 존재하지 않습니다.");
            }
        }
        
        private void numericUpDown1_ValueChanged(object sender, EventArgs e) //선 굵기 수치 변경시 실행되는 함수
        {
            if (!isImageExist() || gridSet == null)
            {
                numericUpDown1.Value = 1;
                return;
            }
            DrawGrid(gridSet.gridType, Align.None);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e) //선 투명도 수치 변경시 실행되는 함수
        {
            if (!isImageExist() || gridSet == null)
            {
                numericUpDown2.Value = 100;
                return;
            }
            DrawGrid(gridSet.gridType, Align.None);
        }
        private void button7_Click(object sender, EventArgs e) //선 색상 변경 버튼 클릭시 실행되는 함수
        {
            if (!isImageExist() || gridSet == null)
            {
                return;
            }
            colorDialog1.ShowDialog();
            cl = colorDialog1.Color;
            DrawGrid(gridSet.gridType, Align.None);
            button7.BackColor = cl;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            lastChange = "행";
            if (radioButton1.Checked)
            {
                button8_Click(sender, e);
            }
            else if (radioButton2.Checked)
            {
                button2_Click(sender, e);

            }else if (radioButton4.Checked)
            {
                button3_Click(sender, e);
            }else if (radioButton5.Checked)
            {
                button4_Click(sender, e);
            }
            else if (radioButton6.Checked)
            {
                button5_Click(sender, e);
            }

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            lastChange = "열";
            if (radioButton1.Checked)
            {
                button15_Click(sender, e);
            }
            else if (radioButton2.Checked)
            {
                button2_Click(sender, e);
            }
            else if (radioButton4.Checked)
            {
                button3_Click(sender, e);
            }
            else if (radioButton5.Checked)
            {
                button4_Click(sender, e);
            }
            else if (radioButton6.Checked)
            {
                button5_Click(sender, e);
            }
        }

        private void 그리드세부설정SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Form2 form2 = new Form2(isSubGridChecked,subGridSet);
                //form2.Owner = this;        //현재  폼이  form2를  소유 this.AddOwnedForm(form2);
                form2.ShowDialog();
                subGridSet = form2.gS;

                int a;
                a = subGridSet.rows;

                subGridSet = form2.gS;
                isSubGridChecked = form2.checkBox1.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"그리드 세부 설정 취소");
            }
        }


        private void button8_Click_2(object sender, EventArgs e)
        {
            if (lastChange == "행")
            {
                button8_Click(sender, e);
            }
            else
            {
                button15_Click(sender, e);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            그리드세부설정SToolStripMenuItem_Click(sender, e);
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (!isImageExist() || gridSet == null)
            {
                return;
            }
            drawer.gridSizeSet(trackBar1.Value);
            pictureBox1.Refresh();
            Application.DoEvents();

            KeyValuePair<PointF, float> KVP = drawer.getCurrentGridSize();

            if (KVP.Key != null)
            {
                label1.Text = $"넓이 : {(int)(KVP.Key).X}";
                label2.Text = $"높이 : {(int)(KVP.Key).Y}";
            }
        }

        private void button15_Click_3(object sender, EventArgs e)
        {
            if (!isImageExist()||gridSet==null)
            {
                return;
            }
            if(trackBar1.Value > 1)
            {
                trackBar1.Value--;
                trackBar1_Scroll(sender, e);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (!isImageExist() || gridSet == null)
            {
                return;
            }
            if (trackBar1.Value < trackBar1.Maximum)
            {
                trackBar1.Value++;
                trackBar1_Scroll(sender, e);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            trackBar1.Value=trackBar1.Maximum;
            trackBar1_Scroll(sender, e);
            button8.PerformClick();
        }

        private void 새파일NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }
    }
    public class GridDrawer 
    /*  실질적으로 그리드를 생성하고 그리는 클래스입니다.
     *  pictureBox와 pictureBox 이미지, 그리드의 Offset이 필드변수로 설정되어있습니다. */
    {
        private GridSetting gridSet;                        //현재 그리드 설정상태
        private PointF gridMoveDistance = new PointF(0, 0); //드래그해서 이동했을때의 그리드 위치
        private PointF imageSize = new PointF(0, 0);        //picturebox 내의 이미지 사이즈
        private PointF imageOffset = new PointF(0, 0);      //picturebox 기준 이미지 오프셋
        private PointF gridOffset = new PointF(0, 0);       //이미지 기준 그리드 오프셋
        private PointF gridSize = new PointF(0, 0);         //이미지 내의 그리드 사이즈
        private PointF dragStartPoint;                      //드래그 시작 지점
        private float past_Size = 0;                        //이전에 그렸다면, 사이즈를 이곳에 저장
        private float gridSizeChanged = 0;
        private Dictionary<Point,GridSetting> clickPoints = new Dictionary<Point, GridSetting>();



        public void SetGrid(GridSetting gridSet) // 그리드를 설정하는 함수
        {
            this.gridSet = gridSet;

            switch (gridSet.align) // 그리드 정렬시 grid 중앙을 기준으로 그리드 이동거리 조절
            {
                case(Align.Left):
                    gridMoveDistance.X = (int)(-((imageSize.X / 2) - (gridSize.X / 2)));
                    break;
                case(Align.Right):
                    gridMoveDistance.X = (int)((imageSize.X / 2) - (gridSize.X / 2));
                    break;
                case(Align.Top):
                    gridMoveDistance.Y = (int)(-((imageSize.Y / 2) - (gridSize.Y / 2)));
                    break;
                case(Align.Bottom):
                    gridMoveDistance.Y = (int)((imageSize.Y / 2) - (gridSize.Y / 2));
                    break;
                case (Align.Center):
                    gridMoveDistance.X = 0;
                    gridMoveDistance.Y = 0;
                    break;
                case (Align.None):
                    break;
            }
        }

        public int CalculateColumnsForRows(int rows)
        // 사용자에게 행을 입력받으면 그 행 수에 맞게 최대한 
        // 사진의 남는 부분이 없도록 하는 열을 계산하여 반환해주는 함수
        {
            float avgRectSize = imageSize.Y / rows;

            return (int)Math.Round(imageSize.X / avgRectSize);
            /* 
             * 이미지의 너비가 1000 높이가 500 각 행의 수가 10개일때 한개의 정사각형의 높이는 50이고
             * 이를 너비로 나누면 20이 나옵니다. 이렇게 한다면 사진의 대부분에 최대로 그리드를 넣을 수 있습니다.
             */
        }

        public int CalculateRowsForColumns(int columns)
        // 사용자에게 열을 입력받으면 그 열 수에 맞게 최대한 
        // 사진의 남는 부분이 없도록 하는 행을 계산하여 반환해주는 함수
        {
            float avgRectSize = imageSize.X / columns;

            return (int)Math.Round(imageSize.Y / avgRectSize);
        }

        public void setImageOffset(PictureBox pictureBox)
        /* Form1.pictureBox의 이미지 오프셋을 계산해주는 함수 이미지 오프셋을 계산해야 그리드를 그릴 수 있다. */
        {
            imageSize.X = imageSize.Y = imageOffset.X = imageOffset.Y = 0;
            /* 초기화 작업 */
            if (pictureBox.Image == null) return;
            /* picturebox에 이미지가 없다면 종료한다. */
            int imgWidth = pictureBox.Image.Width;      //picturebox 내 이미지의 넓이
            int imgHeight = pictureBox.Image.Height;    //picturebox 내 이미지의 높이
            int pbWidth = pictureBox.ClientSize.Width;  //picturebox의 넓이
            int pbHeight = pictureBox.ClientSize.Height;//picturebox의 높이

            float imgRatio = (float)imgWidth / imgHeight;
            float pbRatio = (float)pbWidth / pbHeight;
            /*
            이미지와 picturebox의 가로세로 비를 계산하여 이미지가 중앙에 표시되도록 합니다.
            만약 picturebox의 가로가 800 세로가 100이고 이미지의 가로가 300 세로가 50 이면
            이미지의 크기를 2배 늘려준다면 이미지 비율 그대로 picturebox에 최대한 꽉 맞게 나타날 것입니다.
            따라서 이미지의 크기를 설정하고 그 뒤 이미지가 picturebox의 중앙에 정렬되기 위해서 image의 X offset을 계산합니다.
             */
            if (imgRatio > pbRatio)
            {
                imageSize.X = pbWidth;
                imageSize.Y = pbWidth / imgRatio;
                imageOffset.Y = (pbHeight - imageSize.Y) / 2;
            }
            else
            {
                imageSize.X = pbHeight * imgRatio;
                imageSize.Y = pbHeight;
                imageOffset.X = (pbWidth - imageSize.X) / 2;
            }
        }

        private void setGridOffset(PictureBox pb)
        /*
         * 이미지의 오프셋을 계산했다면 이제 그리드의 오프셋을 계산해야 합니다.
         * 그리드가 제대로 그려지기 위해서는 그리드의 시작 위치와 크기를 정확히 계산해야 합니다.
         */
        {
            if (gridSet.gridType == GridType.Squares) 
            /* 
             * 그리드 타입이 정사각형인 경우에는 복잡한 계산을 해야합니다.
             * 또한 정사각형인 경우에만 드래그를 하여 그리드를 이동시킬 수 있기 때문에
             * 많은 계산 작업을 필요로 합니다.
             */
            {
                float avgRectSize = Math.Min(imageSize.X / gridSet.columns, imageSize.Y / gridSet.rows);
                if (gridSizeChanged != 0)
                {
                    if (pb.Tag != null)
                    {
                        float ratio = (float)pb.Tag;
                        avgRectSize += (gridSizeChanged*ratio);
                    }
                    else
                    {
                        avgRectSize += gridSizeChanged;
                    }
                }
                /*
                 * 정사각형 그리드에 표현되는 한개의 정사각형 사이즈를 계산합니다.
                 * 예를들어 picturebox의 image에서 가로와 세로에 사용자가 입력한 선을 그으면 만들어지는 직사각형에서 큰 값을 선택하면 
                 * 그리드가 이미지 범위 밖으로 나타납니다. 따라서 이미지 내부에 그리드를 생성하기 위해 Math.Min()을 사용해 작은값을 선택합니다. 
                 */
                float totalGridWidth = avgRectSize * gridSet.columns;
                float totalGridHeight = avgRectSize * gridSet.rows;
                //정사각형 그리드를 맞춤 정렬하기 위해 총 그리드의 가로 세로 크기를 구합니다.

                float startX = imageOffset.X;
                float startY = imageOffset.Y;

                float imgRatio = imageSize.X / imageSize.Y;
                float gridRatio = totalGridWidth / totalGridHeight;

                if(gridSizeChanged != 0)
                {
                    startX = imageOffset.X + (imageSize.X - totalGridWidth) / 2;
                    startY = imageOffset.Y + (imageSize.Y - totalGridHeight) / 2;
                }
                else if(imgRatio > gridRatio)
                {
                    startX = imageOffset.X + (imageSize.X - totalGridWidth) / 2;
                }
                else
                {
                    startY = imageOffset.Y + (imageSize.Y - totalGridHeight) / 2;
                }

                if (past_Size == 0 && pb.Tag==null) // 이전 사이즈에 대한 정보가 없고, pb.Tag가 null이라면 (저장 picturebox가 아니라면)
                {
                    startX += gridMoveDistance.X;
                    startY += gridMoveDistance.Y;
                    past_Size = this.imageSize.X;
                }
                else if(pb.Tag==null)   // 이전 사이즈에 대한 정보가 있고, 저장 picturebox가 아니라면
                {
                    float ratio = this.imageSize.X/past_Size;
                    if (ratio > 0) // 크기가 늘어났다면 그리드의 이동거리를 늘린다.
                    {
                        gridMoveDistance.X *= ratio;
                        gridMoveDistance.Y *= ratio;
                    }
                    else           // 크기가 줄어들었다면 그리드의 이동거리를 줄인다. 
                    {
                        gridMoveDistance.X /= ratio;
                        gridMoveDistance.Y /= ratio;
                    }
                    // 늘어난만큼, 줄어든만큼 반영
                    startX += gridMoveDistance.X; 
                    startY += gridMoveDistance.Y;
                    past_Size = this.imageSize.X;
                }else if (pb.Tag != null) // 저장 picturebox라면 그 비율을 구해 곱한다.
                {
                    float ratio = (float)pb.Tag;
                    if (gridSizeChanged != 0)
                    {
                        startX += (gridMoveDistance.X * ratio);
                        startY += (gridMoveDistance.Y * ratio);
                    }
                    else if (imgRatio > gridRatio)
                    {
                        startX += (gridMoveDistance.X * ratio);
                    }
                    else
                    {
                        startY += (gridMoveDistance.Y * ratio);
                    }
                }

                gridOffset.X = startX; // 최종반영
                gridOffset.Y = startY;
                gridSize.X = totalGridWidth;
                gridSize.Y = totalGridHeight;
                //위에서 계산된 결과를 그리드의 Offset, 크기를 반영하여 저장합니다.
            }
            else //정사각형이 아니라면 그리드 오프셋의 결과는 이미지 오프셋과 크기와 동일하기때문에 그대로 설정해줍니다.
            {
                gridSize.X = imageSize.X;
                gridSize.Y = imageSize.Y;
                if(pb.Tag != null)
                {
                    float ratio = (float)pb.Tag;
                    gridOffset.X = imageOffset.X + (gridMoveDistance.X * ratio);
                    gridOffset.Y = imageOffset.Y + (gridMoveDistance.Y * ratio);
                }
                else
                {
                    gridOffset.X = imageOffset.X + gridMoveDistance.X;
                    gridOffset.Y = imageOffset.Y + gridMoveDistance.Y;
                }
            }
        }
 


        public void DrawGrid(PaintEventArgs e, GridType gridType,PictureBox pictureBox) 
        {
            /*
             *그리드와 이미지의 오프셋을 계산하여 반영하고 조건에 맞는 사각형을 그리는 함수를 호출하도록 하는 함수입니다.
             *pictureBox1의 paint 이벤트로 등록되어 있어 pictureBox.refresh() 가 호출되면 호출됩니다. 
             */
            setImageOffset(pictureBox);   //이미지 오프셋 및 크기 설정
            setGridOffset(pictureBox);    //그리드 오프셋 및 크기 설정
            if (imageSize.X == 0 || imageSize.Y == 0) return;
            // 값이 유효하지 않는다면 종료


            /*if(pictureBox.Tag != null)
            {
                float ratio = (float)pictureBox.Tag;
                gridSet.lineWeight *= (ratio*(float)0.2);
            }*/
            Graphics g = e.Graphics;
            Color transparentColor = Color.FromArgb((int)(gridSet.lineOpacity * 255), gridSet.color);
            Pen pen = new Pen(transparentColor, gridSet.lineWeight);


            /* 
             * PaintEventArgs에서 그리기 작업을 수행하는 데 사용되는 Graphics 객체를 가져옵니다. 이 객체를 이용하여 그리드를 그릴 수 있습니다.
             * 사용자가 설정한 그리드 선의 색상, 투명도, 굵기를 반영해 그릴 Pen 객체를 생성합니다.
            */
            switch (gridType) // 이 switch 문에서 실행되는 함수들에서 실질적으로 그리드가 그려집니다.
            {
                case GridType.Rectangles:   // 직사각형 그리드를 그리는 경우 DrawRectangles() 함수 호출
                    DrawRectangles(g, pen,pictureBox);
                    pictureBox.Cursor = Cursors.SizeAll;
                    break;
                case GridType.Squares:      
                    /* 
                     * 정사각형을 그리는 경우 DrawSquare()함수 호출합니다. 
                     * 정사각형 그리드의 경우 정사각형 사진이 아니라면 그리드를 그릴 경우 남는 공간이 존재하기 때문에
                     * 드래그하여 그리드를 움직일 수 있습니다. 사용자에게 이를 표시해주기 위해서 
                     * picturebox에 마우스를 가져다가 댈 경우 커서를 바꿔줍니다.
                    */
                    DrawSquares(g, pen,pictureBox);
                    pictureBox.Cursor = Cursors.SizeAll;
                    break;
                case GridType.HorizontalLines:  // 수평선을 그리는 경우  DrawHorizontalLines() 함수 호출 
                    DrawHorizontalLines(g, pen,pictureBox);
                    pictureBox.Cursor = Cursors.SizeAll;
                    break;
                case GridType.VerticalLines:    // 수직선을 그리는 경우  DrawVerticalLines() 함수 호출 
                    DrawVerticalLines(g, pen,pictureBox);
                    pictureBox.Cursor = Cursors.SizeAll;
                    break;
            }

        }

        private void DrawRectangles(Graphics g, Pen pen,PictureBox pb) // 직사각형 그리드를 그리는 함수
        {
            float rectWidth = imageSize.X / gridSet.columns;
            float rectHeight = imageSize.Y / gridSet.rows;

            for (int row = 0; row < gridSet.rows; row++)
            {
                for (int col = 0; col < gridSet.columns; col++)
                {
                    if (pb.Tag != null)
                    {
                        float ratio = (float)pb.Tag;
                        float x = imageOffset.X + (gridMoveDistance.X*ratio) + col * rectWidth;
                        float y = imageOffset.Y + (gridMoveDistance.Y*ratio) + row * rectHeight;
                        g.DrawRectangle(pen, x, y, rectWidth, rectHeight);
                    }
                    else
                    {
                        float x = imageOffset.X + gridMoveDistance.X + col * rectWidth;
                        float y = imageOffset.Y + gridMoveDistance.Y + row * rectHeight;
                        g.DrawRectangle(pen, x, y, rectWidth, rectHeight);
                    }
                }
            }
        }

        private void DrawSquares(Graphics g, Pen pen,PictureBox pictureBox)
        {
            // 실제로 정사각형 그리드를 그려주는 함수입니다.
            float avgRectSize = Math.Min(imageSize.X / gridSet.columns, imageSize.Y / gridSet.rows);
            if (gridSizeChanged != 0)
            {
                if (pictureBox.Tag != null)
                {
                    float ratio = (float)pictureBox.Tag;
                    avgRectSize += (gridSizeChanged * ratio);
                }
                else
                {
                    avgRectSize += gridSizeChanged;
                }
            }
            // 정사각형 그리드에 표현되는 한개의 정사각형 사이즈를 계산합니다.
            for (int row = 0; row < gridSet.rows; row++)
            {
                for (int col = 0; col < gridSet.columns; col++)
                {
                    float x = gridOffset.X + col * avgRectSize;
                    float y = gridOffset.Y + row * avgRectSize;
                    g.DrawRectangle(pen, x, y, avgRectSize, avgRectSize);


                    ICollection<Point> keys = clickPoints.Keys;

                    foreach (Point p in keys)
                    {
                        if (p.X == row && p.Y == col)
                        {
                            clickPoints.TryGetValue(p, out GridSetting subGridSet);
                            /*if (pictureBox.Tag != null)
                            {
                                float ratio = (float)pictureBox.Tag;
                                subGridSet.lineWeight *= (ratio * (float)0.1);
                            }*/
                            float subGridSetSize = avgRectSize;
                            subGridSetSize = subGridSetSize / subGridSet.rows;
                            Color transparentColor = Color.FromArgb((int)(subGridSet.lineOpacity * 255), subGridSet.color);
                            Pen subPen = new Pen(transparentColor, subGridSet.lineWeight);
                            for (int subRow = 0; subRow < subGridSet.rows; subRow++)
                            {
                                for (int subCol = 0; subCol < subGridSet.columns; subCol++)
                                {
                                    float subX = x + subCol * subGridSetSize;
                                    float subY = y + subRow * subGridSetSize;
                                    g.DrawRectangle(subPen, subX, subY, subGridSetSize, subGridSetSize);
                                }
                            }
                        }
                    }

                    // 이전 함수들에서 계산된 오프셋을 가지고 정사각형을 한개한개 그립니다.
                }
            }
        }

        private void DrawHorizontalLines(Graphics g, Pen pen,PictureBox pb) // 수평선을 그리는 함수
        {
            float avgRectHeight = imageSize.Y / gridSet.rows;

            for (int row = 0; row < gridSet.rows; row++)
            {
                if (pb.Tag != null)
                {
                    float ratio = (float)pb.Tag;
                    float y = imageOffset.Y + (gridMoveDistance.Y * ratio) +row * avgRectHeight;
                    g.DrawLine(pen, imageOffset.X, y, imageOffset.X + imageSize.X, y);
                }
                else
                {
                    float y = imageOffset.Y + (gridMoveDistance.Y ) + row * avgRectHeight;
                    g.DrawLine(pen, imageOffset.X, y, imageOffset.X + imageSize.X, y);
                }
            }
        }

        private void DrawVerticalLines(Graphics g, Pen pen, PictureBox pb) //수직선을 그리는 함수
        {
            float avgRectWidth = imageSize.X / gridSet.columns;

            for (int col = 0; col < gridSet.columns; col++)
            {
                if (pb.Tag != null)
                {
                    float ratio = (float)pb.Tag;
                    float x = imageOffset.X + (gridMoveDistance.X*ratio)+col * avgRectWidth;
                    g.DrawLine(pen, x, imageOffset.Y, x, imageOffset.Y + imageSize.Y);
                }
                else
                {
                    float x = imageOffset.X + (gridMoveDistance.X)+ col * avgRectWidth;
                    g.DrawLine(pen, x, imageOffset.Y, x, imageOffset.Y + imageSize.Y);
                }
            }
        }

        public void StartDragging(Point location) 
        //마우스 Down 이벤트가 발생하면 호출되며 드래그의 시작 지점을 설정하는 함수
        {
            dragStartPoint = location;
        }
        
        public void Drag(Point currentLocation,PictureBox pictureBox,bool canMoveEverywhere =false) 
        // 마우스로 드래그를 할 때 호출되어 이동거리를 계산하여 그리드를 이동시키는 함수
        {
            if(gridSet == null )
            {
                return;
            }

            float dx = currentLocation.X - dragStartPoint.X;
            float dy = currentLocation.Y - dragStartPoint.Y;

            if (canMoveEverywhere)
            {
                gridMoveDistance.X += (int)dx;
                gridMoveDistance.Y += (int)dy;
                dragStartPoint = currentLocation;
                pictureBox.Refresh();
                return;
            }

            // 시작 위치에서 움직인 거리를 계산하여 dx,dy에 반영합니다.
            float imgRatio = imageSize.X / imageSize.Y;
            float gridRatio = gridSize.X / gridSize.Y;


            if (gridSizeChanged != 0)
            {
                float startX = imageOffset.X + (imageSize.X - gridSize.X) / 2;
                if ((int)(startX + gridMoveDistance.X) + dx < (int)(imageOffset.X) ||
                    (int)(startX + gridMoveDistance.X + gridSize.X) + dx > (int)(pictureBox.ClientSize.Width - imageOffset.X))
                {
                    return;
                }
                float startY = imageOffset.Y + (imageSize.Y - gridSize.Y) / 2;
                if ((int)(startY + gridMoveDistance.Y) + dy < (int)(imageOffset.Y) ||
                    (int)(startY + gridMoveDistance.Y + gridSize.Y) + dy > (int)(pictureBox.ClientSize.Height - imageOffset.Y))
                {
                    return;
                }
            }
            else if (imgRatio > gridRatio) // 이동거리 초과시 Return
            {
                float startX = imageOffset.X + (imageSize.X - gridSize.X) / 2;
                if ((int)(startX + gridMoveDistance.X)+dx < (int)(imageOffset.X) ||
                    (int)(startX + gridMoveDistance.X + gridSize.X)+dx > (int)(pictureBox.ClientSize.Width - imageOffset.X))
                {
                    return;
                }
            }
            else
            {
                float startY = imageOffset.Y + (imageSize.Y - gridSize.Y) / 2;
                if ((int)(startY + gridMoveDistance.Y)+dy < (int)(imageOffset.Y)||
                    (int)(startY + gridMoveDistance.Y + gridSize.Y)+dy > (int)(pictureBox.ClientSize.Height - imageOffset.Y))
                {
                    return;
                }
            }


            if (gridSizeChanged != 0 )
            {
                gridMoveDistance.X += (int)dx;
                gridMoveDistance.Y += (int)dy;
            }
            else if (imgRatio > gridRatio)
            /* 
             * 이미지의 가로 세로 비율과 그리드의 가로 세로 비율을 비교하여 드래그할 때 그리드가 이미지 내에서 어떻게 이동할지를 결정합니다.
             * 예를들어 이미지의 크기가 가로500 세로100 이고 (정사각형)그리드의 크기가 가로세로 100 이라면 가로로만 이동할 수 있도록 합니다.
             */
            {
                gridMoveDistance.X += (int)dx;
            }
            else
            {
                gridMoveDistance.Y += (int)dy;
            }
            dragStartPoint = currentLocation;
            pictureBox.Refresh();
            // 그리드를 다시 그리기 위해(드래그 행위를 반영하기 위해) 다시 그려줍니다. (setGridOffset() 함수에서 dx,dy가 반영됨)
        }

        public string getSubGridLocation(Point clickLocation,bool iSGS,GridSetting sGS)
        {
            if (gridSet == null || gridSet.gridType != GridType.Squares)
            {
                return $"조기종료";
            }

            float avgRectSize = Math.Min(imageSize.X / gridSet.columns, imageSize.Y / gridSet.rows);
            avgRectSize += gridSizeChanged;
            for (int row = 0; row < gridSet.rows; row++)
            {
                for (int col = 0; col < gridSet.columns; col++)
                {
                    float x = gridOffset.X + col * avgRectSize;
                    float y = gridOffset.Y + row * avgRectSize;
                    if (clickLocation.X > x && clickLocation.X < x+avgRectSize
                        && clickLocation.Y >y && clickLocation.Y < y + avgRectSize)
                    {
                        Point newPoint = new Point(row, col);
                        bool pointExists = false;

                        ICollection<Point> keys = clickPoints.Keys;
                        foreach (Point p in keys)
                        {
                            if (p.X == newPoint.X && p.Y == newPoint.Y)
                            {
                                clickPoints.Remove(p);
                                pointExists = true;
                                break;
                            }
                        }

                        if (!pointExists)
                        {
                            try
                            {
                                if (iSGS)
                                {
                                    clickPoints.Add(newPoint, sGS);
                                }
                                else
                                {
                                    Form2 form2 = new Form2(iSGS, sGS);
                                    //form2.Owner = this;        //현재  폼이  form2를  소유 this.AddOwnedForm(form2);
                                    form2.ShowDialog();
                                    GridSetting subGridSet = form2.gS;

                                    int a;
                                    a = subGridSet.rows;

                                    subGridSet = form2.gS;
                                    clickPoints.Add(newPoint, subGridSet);
                                    Form1.subGridSet = subGridSet;
                                    if (form2.checkBox1.Checked)
                                    {
                                        return "체크";
                                    }
                                    else
                                    {
                                        return "미체크";
                                    }
                                }
                                
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show($"세부 그리드 설정 취소");
                                return "오류종료";
                            }

                        }
                    }
                }
            }
            return "오류종료";
        }

        public void CaptureGrid(string path, PictureBox saveBox, PictureBox drawBox) // 그리드 부분만 파일로 출력해주는 함수
        {
            GridSetting gS = this.gridSet;
            float ratio = saveBox.Image.Width / imageSize.X;
            saveBox.Tag = ratio;
            saveBox.Paint += new PaintEventHandler((s, e) => this.DrawGrid(e, this.gridSet.gridType,saveBox));
            saveBox.Refresh();
            Application.DoEvents();

            Rectangle clipRect = new Rectangle((int)gridOffset.X, (int)gridOffset.Y, (int)gridSize.X, (int)gridSize.Y);
            using (var originalBitmap = new Bitmap(saveBox.Image.Width, saveBox.Image.Height))
            {
                saveBox.DrawToBitmap(originalBitmap, saveBox.ClientRectangle);

                using (var clippedBitmap = new Bitmap(clipRect.Width, clipRect.Height))
                {
                    using (Graphics g = Graphics.FromImage(clippedBitmap))
                    {
                        g.DrawImage(originalBitmap, new Rectangle(0, 0, clippedBitmap.Width, clippedBitmap.Height),
                            clipRect, GraphicsUnit.Pixel);
                    }

                    System.Drawing.Imaging.ImageFormat imageFormat = null;
                    var extension = Path.GetExtension(path);
                    switch (extension.ToLower())
                    {
                        case ".bmp":
                            imageFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                            break;
                        case ".png":
                            imageFormat = System.Drawing.Imaging.ImageFormat.Png;
                            break;
                        case ".jpeg":
                        case ".jpg":
                            imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                            break;
                        case ".gif":
                            imageFormat = System.Drawing.Imaging.ImageFormat.Gif;
                            break;
                        default:
                            throw new NotSupportedException("File extension is not supported");
                    }

                    clippedBitmap.Save(path, imageFormat);
                }
            }

        }
        public void gridSizeSet(int changeValue)
        {
            float avgRectSize = Math.Min(imageSize.X / gridSet.columns, imageSize.Y / gridSet.rows);
            this.gridSizeChanged = -avgRectSize + changeValue;
        }
        
        public void gridSizeReset()
        {
            this.gridSizeChanged=0;
        }
        public KeyValuePair<PointF, float> getCurrentGridSize()
        {
            float avgRectSize = Math.Min(imageSize.X / gridSet.columns, imageSize.Y / gridSet.rows);
            KeyValuePair<PointF,float> gridSizeKVP = new KeyValuePair<PointF,float>(this.gridSize,avgRectSize);
            return gridSizeKVP;
        }
    }
    public enum GridType    
    /* 그리드 종류 열거형 데이터 입니다. 직사각형, 정사각형, 수평선, 수직선으로 정의되어 있습니다.*/
    {
        Rectangles,
        Squares,
        HorizontalLines,
        VerticalLines,
    }
    public enum Align
    /* 정렬 상태 열거형 입니다. 왼쪽맞춤, 오른쪽맞춤, 상단맞춤, 하단맞춤, 중앙맞춤으로 정의되어 있습니다. */
    {
        Left,
        Right,
        Top,
        Bottom,
        Center,
        None,
    }
    public class GridSetting
    /*그리드의 설정을 저장한 클래스 입니다*/
    {
        public GridType gridType;           // 그리드의 타입입니다.
        public Align align;                 // 그리드의 정렬상태입니다
        public Color color;                 // 그리드 선의 색상 입니다.
        public int rows, columns;           // 그리드의 행,열 입니다.
        public float lineWeight, lineOpacity;//그리드 선의 굵기, 투명도 입니다.
        public GridSetting(int rows, int cols, GridType gt, Color c, Align a , float LW = 1.0f, float LO = 1.0f)
        /* 그리드 설정 생성자 입니다. 정렬상태와 선굵기,선투명도는 기본값을 Center, 1.0, 1.0으로 설정하여 선택적 매개변수로 받습니다. */
        {
            this.gridType = gt;
            this.align = a;
            this.color = c;
            this.rows = rows;
            this.columns = cols;
            this.lineWeight = LW;
            this.lineOpacity = LO;
        }
    }

    

}
