using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;  // Asegúrate de agregar esta línea para usar LINQ
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Form1 : Form
    {
        MySqlConnection con = new MySqlConnection("Server=localhost;Database=texturasbd;User=root;Password=123456;");
        int cR, cG, cB;
        List<int[]> colores = new List<int[]>();
        List<int> idobtenidos = new List<int>();

        public Form1()
        {
            InitializeComponent();
            ActualizarDataGridView();
            this.Font = new Font("Courier", 10, FontStyle.Regular);
            this.BackColor = ColorTranslator.FromHtml("#d5e2cd");
        }

        private void ActualizarDataGridView()
        {
            dataGridView1.DataSource = info();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Color c = new Color();
            int sR = 0, sG = 0, sB = 0;
            for (int i = e.X; i < e.X + 10; i++)
                for (int j = e.Y; j < e.Y + 10; j++)
                {
                    c = bmp.GetPixel(i, j);
                    sR += c.R;
                    sG += c.G;
                    sB += c.B;
                }
            sR /= 100;
            sG /= 100;
            sB /= 100;
            cR = sR;
            cG = sG;
            cB = sB;
            textBox1.Text = sR.ToString();
            textBox2.Text = sG.ToString();
            textBox3.Text = sB.ToString();
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "archivos img|*.*";
            openFileDialog1.ShowDialog();
            Bitmap bmp = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = bmp;
            dataGridView2.DataSource = null;
        }

        private void btnColores_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            Color c = new Color();
            int sR, sG, sB;
            for (int i = 0; i < bmp.Width - 10; i = i + 10)
            {
                for (int j = 0; j < bmp.Height - 10; j = j + 10)
                {
                    sR = 0; sG = 0; sB = 0;
                    for (int ip = i; ip < i + 10; ip++)
                    {
                        for (int jp = j; jp < j + 10; jp++)
                        {
                            c = bmp.GetPixel(ip, jp);
                            sR += c.R;
                            sG += c.G;
                            sB += c.B;
                        }
                    }
                    sR /= 100;
                    sG /= 100;
                    sB /= 100;

                    bool verda = false;
                    foreach (var color in colores)
                    {
                        if ((sR - 25 <= color[1] && color[1] <= sR + 25) && (sG - 25 <= color[2] && color[2] <= sG +25) && (sB - 25 <= color[3] && color[3] <= sB + 25))
                        {
                            int rid = color[0];
                            verda = true;
                            if (!idobtenidos.Contains(rid))
                            {
                                idobtenidos.Add(rid);
                            }
                            break;
                        }
                    }

                    if (verda)
                    {
                        for (int ip = i; ip < i + 10; ip++)
                        {
                            for (int jp = j; jp < j + 10; jp++)
                            {
                                Color oppositeColor = Color.FromArgb(255 - sR, 255 - sG, 255 - sB);
                                bmp2.SetPixel(ip, jp, oppositeColor);
                            }
                        }
                    }
                    else
                    {
                        for (int ip = i; ip < i + 10; ip++)
                        {
                            for (int jp = j; jp < j + 10; jp++)
                            {
                                c = bmp.GetPixel(ip, jp);
                                bmp2.SetPixel(ip, jp, Color.FromArgb(c.R, c.G, c.B));
                            }
                        }
                    }
                }
            }
            pictureBox1.Image = bmp2;
            dataGridView2.DataSource = infodos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            Color c = new Color();
            int sR, sG, sB;
            for (int i = 0; i < bmp.Width - 10; i = i + 10)
                for (int j = 0; j < bmp.Height - 10; j = j + 10)
                {
                    sR = 0; sG = 0; sB = 0;
                    for (int ip = i; ip < i + 10; ip++)
                        for (int jp = j; jp < j + 10; jp++)
                        {
                            c = bmp.GetPixel(ip, jp);
                            sR = sR + c.R;
                            sG = sG + c.G;
                            sB = sB + c.B;
                        }
                    sR = sR / 100;
                    sG = sG / 100;
                    sB = sB / 100;

                    if (((cR - 20 <= sR) && (sR <= cR + 20)) && ((cG - 20 <= sG) && (sG <= cG + 20)) && ((cB - 20 <= sB) && (sB <= cB + 20)))
                    {
                        for (int ip = i; ip < i + 10; ip++)
                            for (int jp = j; jp < j + 10; jp++)
                            {
                                bmp2.SetPixel(ip, jp, Color.Black);
                            }
                    }
                    else
                    {
                        for (int ip = i; ip < i + 10; ip++)
                            for (int jp = j; jp < j + 10; jp++)
                            {
                                c = bmp.GetPixel(ip, jp);
                                bmp2.SetPixel(ip, jp, Color.FromArgb(c.R, c.G, c.B));
                            }
                    }

                }
            pictureBox1.Image = bmp2;
        }

        public DataTable info()
        {
            DataTable datos = new DataTable();

            con.Open();

            string llenar = "SELECT * FROM texturas;";
            string obtenerColores = "SELECT id, r, g, b FROM texturas;";
            MySqlCommand com = new MySqlCommand(llenar, con);
            MySqlDataAdapter dat = new MySqlDataAdapter(com);
            dat.Fill(datos);

            MySqlCommand comColores = new MySqlCommand(obtenerColores, con);
            MySqlDataReader reader = comColores.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32("id");
                int r = reader.GetInt32("r");
                int g = reader.GetInt32("g");
                int b = reader.GetInt32("b");
                colores.Add(new int[] { id, r, g, b });
            }

            con.Close();
            return datos;
        }

        public DataTable infodos()
        {
            DataTable datos = new DataTable();

            if (idobtenidos.Count > 0)
            {
                string idList = string.Join(",", idobtenidos);

                con.Open();

                string llenar = $"SELECT * FROM texturas WHERE id IN ({idList});";
                MySqlCommand com = new MySqlCommand(llenar, con);
                MySqlDataAdapter dat = new MySqlDataAdapter(com);
                dat.Fill(datos);

                con.Close();
            }

            return datos;
        }
    }
}