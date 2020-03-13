
using System;
using System.Drawing;
using System.Windows.Forms;

namespace KG_4_Lipatova
{
    public partial class Form1 : Form
    {
        int x1, y1, x2, y2, x3, y3, x4, y4, yt,xt, yt2,xt2;
        int width;
        int Level ;
        int height;
        int px;
        int py;
        int pz;

        Bitmap obg;//базовый тип, который отображает изображение
        //указываю, что obg будет изображением
        Graphics g;
        //использую для отрисовки на PictureBox

        //Длина стрелки
        const int ARR_LEN = 10;

        public Form1()
        {
            InitializeComponent();
            

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            width = pictureBox1.Width;
            height = pictureBox1.Height;
            string text;
            //создаю Bitmap 
            // и создаю новый обьект Graphics для указаного Bitmap
            obg = new Bitmap(width, height);
            g = Graphics.FromImage(obg);
            Level = (int)Size.Value;
            int PIX_IN_ONE = 15 ;//Пикселей в одном делении оси
            px = (int)Pxx.Value * PIX_IN_ONE;
            py = (int)Pyy.Value * PIX_IN_ONE;
            pz = (int)Pzz.Value* PIX_IN_ONE;
            
            //наожу кординаты центра
            int w = pictureBox1.ClientSize.Width / 2;
            int h = pictureBox1.ClientSize.Height / 2;
            //Смещаю начало координат в центр PictureBox
            g.TranslateTransform(w, h);
            //Вызываю свои функции рисования осей
            DrawXAxis(new Point(-w, 0), new Point(w, 0), g, h, PIX_IN_ONE);
            DrawYAxis(new Point(0, h), new Point(0, -h), g, w, PIX_IN_ONE);
            DrawZAxis(new Point(-w+ 4*PIX_IN_ONE, h), new Point(w - 4 * PIX_IN_ONE, -h), g, w,h, PIX_IN_ONE);
            //Для наглядности рисую маленькую  точку в центре
            g.FillEllipse(Brushes.DarkBlue, -2, -2, 4, 4);
            pictureBox1.BackgroundImage = obg;
            
            x1 = 2 * PIX_IN_ONE + px +  pz; y1 = 2 * PIX_IN_ONE  * (-1)-py - pz;//пользователь по точкам строит фигуру
            x2 = 2 * PIX_IN_ONE + px + pz; y2 = 4 * PIX_IN_ONE  * (-1)-5*Level - py - pz;
            x3 = 4 * PIX_IN_ONE +5  *Level + px + pz; y3 = 4 * PIX_IN_ONE * (-1)  -5 * Level - py - pz;
            x4 = 4 * PIX_IN_ONE + 5 * Level + px + pz; y4 = 2 * PIX_IN_ONE  * (-1) - py - pz;

            //Координаты объекта
            PointF[] o = new PointF[] {//передняя грань
            new Point(x1, y1),
            new Point(x2, y2),
            new Point(x3, y3),
            new Point(x4, y4)
        };
            PointF[] a = new PointF[] {//задняя грань
            new Point(x1+3*PIX_IN_ONE+5 * Level, y1-3*PIX_IN_ONE-5* Level),
            new Point(x2+3*PIX_IN_ONE+5 * Level, y2-3*PIX_IN_ONE-5* Level),
            new Point(x3+3*PIX_IN_ONE+5 * Level, y3-3*PIX_IN_ONE-5* Level),
            new Point(x4+3*PIX_IN_ONE+5 * Level, y4-3*PIX_IN_ONE-5* Level)
        };
            
            PointF[] b = new PointF[] {
            new Point(x1, y1),
            new Point(x2, y2),
            new Point(x2+3*PIX_IN_ONE+5 * Level, y2-3*PIX_IN_ONE-5 * Level),
            new Point(x1+3*PIX_IN_ONE+5 * Level, y1-3*PIX_IN_ONE-5 * Level)
        };
            PointF[] c = new PointF[] {
            
            new Point(x4+3*PIX_IN_ONE+5 * Level, y4-3*PIX_IN_ONE-5 * Level),
            new Point(x4, y4),
            new Point(x3, y3),
            new Point(x3+3*PIX_IN_ONE+5 * Level, y3-3*PIX_IN_ONE-5 * Level)
        }; 
            Pen GPen = new Pen(Color.FromArgb(100, 0, 130, 0), 2);
            //Рисую трапецию для поворота 
            g.DrawPolygon(GPen, new[]{
            new Point(x1, y1),
            new Point(x2, y2),
            new Point(x3, y3),
            new Point(x4, y4)
        });
            if (checkBox2.Checked)
            {
               
                g.DrawPolygon(GPen, c);
                g.DrawPolygon(GPen, b);
                g.DrawPolygon(GPen, a);
            }
            g.FillEllipse(Brushes.Green, x1-3, y1-3, 5, 5);
            g.FillEllipse(Brushes.Green, x2-3, y2-3, 5, 5);
            g.FillEllipse(Brushes.Green, x3-3, y3-3, 5, 5);
            g.FillEllipse(Brushes.Green, x4-3, y4-3, 5, 5);
            g.DrawLine(Pens.Black, xt, yt, xt2, yt2);
            
            PointF STp = new PointF(0, 0); // точка относительно которой поворачиваем
            
            double angle = (double)hScrollBar1.Value; //угол поворота
            g.FillEllipse(Brushes.Red, xt-3, yt-3, 7, 7);
            double angleRadian = angle * Math.PI / 180; //переводим угол в радианы
            PointF[] rr = new PointF[o.Length]; //для хранения новых координат обьекта
            PointF[] rr1 = new PointF[a.Length]; //для хранения новых координат обьекта
            PointF[] rr2 = new PointF[b.Length]; //для хранения новых координат обьекта
            PointF[] rr3 = new PointF[c.Length]; //для хранения новых координат обьекта


            string t = comboBox1.Text;
            if (t == "x")
            {
                for (int j = 0; j < o.Length; j++)
                {//кординаті обьекта - кординаті точки поворота 
                    float x, y, x1, y1, x2, y2, x3, y3, z, z1, z2, z3;
                    x = (float)((o[j].X - STp.X));
                    y = (float)((o[j].Y - STp.Y) * Math.Cos(angleRadian) + (o[j].Y + o[j].X - STp.Y + STp.X) * Math.Sin(angleRadian) + STp.Y);
                    z = (float)((o[j].Y - STp.Y) * Math.Sin(angleRadian) + STp.X + (o[j].Y + o[j].X - STp.Y + STp.X) * Math.Cos(angleRadian) + STp.Y + STp.X);
                    x1 = (float)((a[j].X - STp.X));
                    y1 = (float)((a[j].Y - STp.Y) * Math.Cos(angleRadian) + (a[j].Y + a[j].X - STp.Y + STp.X) * Math.Sin(angleRadian) + STp.Y);
                    z1 = (float)((a[j].Y - STp.Y) * Math.Sin(angleRadian) + STp.X + (a[j].Y + a[j].X - STp.Y + STp.X) * Math.Cos(angleRadian) + STp.Y + STp.X);
                    x2 = (float)((b[j].X - STp.X));
                    y2 = (float)((b[j].Y - STp.Y) * Math.Cos(angleRadian) + (b[j].Y + b[j].X - STp.Y + STp.X) * Math.Sin(angleRadian) + STp.Y);
                    z2 = (float)((b[j].Y - STp.Y) * Math.Sin(angleRadian) + STp.X + (b[j].Y + b[j].X - STp.Y + STp.X) * Math.Cos(angleRadian) + STp.Y + STp.X);
                    x3 = (float)((c[j].X - STp.X));
                    y3 = (float)((c[j].Y - STp.Y) * Math.Cos(angleRadian) + (c[j].Y + c[j].X - STp.Y + STp.X) * Math.Sin(angleRadian) + STp.Y);
                    z3 = (float)((c[j].Y - STp.Y) * Math.Sin(angleRadian) + STp.X + (c[j].Y + c[j].X - STp.Y + STp.X) * Math.Cos(angleRadian) + STp.Y + STp.X);

                    //}
                    rr1[j] = new PointF(x1 - z1, y1 - z1);
                    rr2[j] = new PointF(x2 - z2, y2 - z2);
                    rr3[j] = new PointF(x3 - z3, y3 - z3);
                    rr[j] = new PointF(x - z, y - z);
                    //string[] masABCD = new string[4] { "A","B"," C", " D"};
                    //if (checkBox1.Checked)
                    // {
                    //    Drawcoordinates2(new Font("Yu Gothic", 12),  PIX_IN_ONE, x , y, (int)x, (int)y, masABCD[j]);
                    //}
                }
                //Рисуем повернутый объект
                g.DrawPolygon(Pens.Red, rr);
                if (checkBox2.Checked)
                {
                    g.DrawPolygon(Pens.Red, rr1);
                    g.DrawPolygon(Pens.Red, rr2);
                    g.DrawPolygon(Pens.Red, rr3);
                }
            }

            if (t == "y")
            {
                for (int j = 0; j < o.Length; j++)
                {//кординаті обьекта - кординаті точки поворота 
                    float x, y, x1, y1, x2, y2, x3, y3, z, z1, z2, z3;
                    x = (float)((o[j].X - STp.X) * Math.Cos(angleRadian) + STp.X + (o[j].Y + o[j].X - STp.Y + STp.X) * Math.Sin(angleRadian) );
                    y = (float)((o[j].Y - STp.Y));
                    z = (float)((o[j].X - STp.X) * Math.Sin(angleRadian) + STp.X + (o[j].Y + o[j].X - STp.Y + STp.X) * Math.Cos(angleRadian) );
                    x1 = (float)((a[j].X - STp.X) * Math.Cos(angleRadian) + STp.X + (a[j].Y + a[j].X - STp.Y + STp.X) * Math.Sin(angleRadian));
                    y1 = (float)((a[j].Y - STp.Y));
                    z1 = (float)((a[j].X - STp.X) * Math.Sin(angleRadian) + STp.X + (a[j].Y + a[j].X - STp.Y + STp.X) * Math.Cos(angleRadian));
                    x2 = (float)((b[j].X - STp.X) * Math.Cos(angleRadian) + STp.X + (b[j].Y + b[j].X - STp.Y + STp.X) * Math.Sin(angleRadian));
                    y2 = (float)((b[j].Y - STp.Y));
                    z2 = (float)((b[j].X - STp.X) * Math.Sin(angleRadian) + STp.X + (b[j].Y + b[j].X - STp.Y + STp.X) * Math.Cos(angleRadian));
                    x3 = (float)((c[j].X - STp.X) * Math.Cos(angleRadian) + STp.X + (c[j].Y + c[j].X - STp.Y + STp.X) * Math.Sin(angleRadian));
                    y3 = (float)((c[j].Y - STp.Y));
                    z3 = (float)((c[j].X - STp.X) * Math.Sin(angleRadian) + STp.X + (c[j].Y + c[j].X - STp.Y + STp.X) * Math.Cos(angleRadian));
                    //}
                    rr1[j] = new PointF(x1 - z1, y1 - z1);
                    rr2[j] = new PointF(x2 - z2, y2 - z2);
                    rr3[j] = new PointF(x3 - z3, y3 - z3);
                    rr[j] = new PointF(x - z, y - z);
                    //string[] masABCD = new string[4] { "A","B"," C", " D"};
                    //if (checkBox1.Checked)
                    // {
                    //    Drawcoordinates2(new Font("Yu Gothic", 12),  PIX_IN_ONE, x , y, (int)x, (int)y, masABCD[j]);
                    //}
                }
                //Рисуем повернутый объект
                g.DrawPolygon(Pens.Turquoise, rr);
                if (checkBox2.Checked)
                {
                    g.DrawPolygon(Pens.Turquoise, rr1);
                    g.DrawPolygon(Pens.Turquoise, rr2);
                    g.DrawPolygon(Pens.Turquoise, rr3);
                }
            }
            if (t == "z")
            {
                for (int j = 0; j < o.Length; j++)
                {
                    float x, y, x1, y1, x2, y2, x3, y3, z, z1, z2, z3;
                    x = (float)((o[j].X - STp.X) * Math.Cos(angleRadian) + (o[j].Y  - STp.Y ) * Math.Sin(angleRadian) );
                    y = (float)((o[j].X - STp.X) * Math.Sin(angleRadian) + (o[j].Y - STp.Y) * Math.Cos(angleRadian));
                    z = (float)((o[j].Y + o[j].X - STp.Y + STp.X));
                    x1 = (float)((a[j].X - STp.X) * Math.Cos(angleRadian) + (a[j].Y - STp.Y) * Math.Sin(angleRadian));
                    y1 = (float)((a[j].X - STp.X) * Math.Sin(angleRadian) + (a[j].Y - STp.Y) * Math.Cos(angleRadian));
                    z1 = (float)((a[j].Y + a[j].X - STp.Y + STp.X));
                    x2 = (float)((b[j].X - STp.X) * Math.Cos(angleRadian) + (b[j].Y - STp.Y) * Math.Sin(angleRadian));
                    y2 = (float)((b[j].X - STp.X) * Math.Sin(angleRadian) + (b[j].Y - STp.Y) * Math.Cos(angleRadian));
                    z2 = (float)((b[j].Y + b[j].X - STp.Y + STp.X));
                    x3 = (float)((c[j].X - STp.X) * Math.Cos(angleRadian) + (c[j].Y - STp.Y) * Math.Sin(angleRadian));
                    y3 = (float)((c[j].X - STp.X) * Math.Sin(angleRadian) + (c[j].Y - STp.Y) * Math.Cos(angleRadian));
                    z3 = (float)((c[j].Y + c[j].X - STp.Y + STp.X));
                   
                    rr1[j] = new PointF(x1 - z1, y1 - z1);
                    rr2[j] = new PointF(x2 - z2, y2 - z2);
                    rr3[j] = new PointF(x3 - z3, y3 - z3);
                    rr[j] = new PointF(x - z, y - z);
                }
                //Рисуем повернутый объект
                g.DrawPolygon(Pens.Fuchsia, rr);
                if (checkBox2.Checked)
                {
                    g.DrawPolygon(Pens.Fuchsia, rr1);
                    g.DrawPolygon(Pens.Fuchsia, rr2);
                    g.DrawPolygon(Pens.Fuchsia, rr3);
                }
            }
        }
        //Рисование оси X
        private void DrawXAxis(Point start, Point end, Graphics g, int h, int PIX_IN_ONE)
        {
            //для рисования сетки создаю полупрозрачную кисть
            Pen semiTransPen = new Pen(Color.FromArgb(60, 0, 0, 255), 1);
            //Деления в положительном направлении оси
            for (int i = PIX_IN_ONE; i < end.X - ARR_LEN; i += PIX_IN_ONE)
            {
                //Рисует линию, соединяющую две точки, указанные парами координат.
                g.DrawLine(semiTransPen, i, -h, i, h);
                g.DrawLine(Pens.Black, i, -4, i, 4);
                DrawText(new Point(i, 5), (i / PIX_IN_ONE).ToString(), g);
            }
            //Деления в отрицательном направлении оси
            for (int i = -PIX_IN_ONE; i > start.X; i -= PIX_IN_ONE)
            {
                g.DrawLine(semiTransPen, i, -h, i, h);
                g.DrawLine(Pens.Black, i, -4, i, 4);
                DrawText(new Point(i, 5), (i / PIX_IN_ONE).ToString(), g);
            }
            //Ось
            g.DrawLine(Pens.Black, start, end);
            //Стрелка
            //Вычисление стрелки оси
            g.DrawPolygon(Pens.Black, new[]{
            new Point(end.X,end.Y),
            new Point(end.X-ARR_LEN, end.Y+5),
            new Point(end.X-ARR_LEN,end.Y-5)
        });
        }
        //Рисование оси z
        private void DrawZAxis(Point start, Point end, Graphics g, int w, int h, int PIX_IN_ONE)
        {
            int u = 2;

            //Ось
            if (checkBox1.Checked)
            {
                g.DrawLine(Pens.Black, start, end);
            
            //Стрелка
            //Вычисление стрелки оси
            g.DrawPolygon(Pens.Black, new[]{
            new Point(-end.X,-end.Y),
            new Point(-end.X+ARR_LEN-3, -end.Y-12),
            new Point(-end.X+ARR_LEN+4,-end.Y-3)});
            }
        }
        //Рисование оси Y
        private void DrawYAxis(Point start, Point end, Graphics g, int w, int PIX_IN_ONE)
        {
            //для рисования сетки создаю полупрозрачную кисть
            Pen semiTransPen = new Pen(Color.FromArgb(60, 0, 0, 255), 1);
            //Деления в отрицательном направлении оси
            for (int i = PIX_IN_ONE; i < start.Y; i += PIX_IN_ONE)
            {
                g.DrawLine(semiTransPen, -w, i, w, i);
                g.DrawLine(Pens.Black, -4, i, 4, i);
                DrawText(new Point(5, i), (-i / PIX_IN_ONE).ToString(), g, true);
            }
            //Деления в положительном направлении оси
            for (int i = -PIX_IN_ONE; i > end.Y + ARR_LEN; i -= PIX_IN_ONE)
            {
                g.DrawLine(semiTransPen, -w, i, w, i);
                g.DrawLine(Pens.Black, -4, i, 4, i);
                DrawText(new Point(5, i), (-i / PIX_IN_ONE).ToString(), g, true);
            }
            //Ось
            g.DrawLine(Pens.Black, start, end);
            //Стрелка
            //Вычисление стрелки оси
            g.DrawPolygon(Pens.Black, new[]{
            new Point(end.X,end.Y),
            new Point(end.X+5, end.Y+ARR_LEN),
            new Point(end.X-5,end.Y+ARR_LEN)
        });
        }

        //Рисование текста
        private void DrawText(Point point, string text, Graphics g, bool isYAxis = false)
        {
            var f = new Font("Yu Gothic", 7);//шрифт
            var size = g.MeasureString(text, f);
            var pt = isYAxis
                ? new PointF(point.X + 1, point.Y - size.Height / 2)
                : new PointF(point.X - size.Width / 2, point.Y + 1);
            var rect = new RectangleF(pt, size); 
            g.DrawString(text, f, Brushes.Black, rect);
        }
        //И при каждой загрузке формы будет срабатывать код в обработчике Form1_Load.
        //автосгенерированный метод Form1_Load:
        //применяемый к визуальным компонентам
        private void Form1_Load(object sender, EventArgs e)
        {
            

            //Делаем кнопку круглой
            //Получает или задает область окна, связанную с элементом управления.
            System.Drawing.Drawing2D.GraphicsPath Button_Path = new System.Drawing.Drawing2D.GraphicsPath();
            //В следующем примере кода демонстрируется использование свойства Region путем создания круглой кнопки. 
            Button_Path.AddEllipse(2, 2, this.button1.Width-5, this.button1.Height-7);
            Region Button_Region = new Region(Button_Path);
            this.button1.Region = Button_Region;
        }

        
    }
}
