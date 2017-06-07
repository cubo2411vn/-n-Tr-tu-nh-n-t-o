using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class p : Form
    {
        // 
        int column = 6;// 1 cột biến và 5 cột công thức R1~R5
        int row = 9; // tương ứng 9 biến 
        int[,] state; // trạng thái -1 0 1
        int[] flag = new int[10]; // nhận sự thay đổi giá trị của biến sau mỗi lần Nhập

        int[,] R = new int[2,6]; // xét số lần xuất hiện của trạng thái -1 và 1

        // biến
        double alpha;
        double beta;
        double gamma;
        double a;
        double b;
        double c;
        double hc;
        double S;
        double P;
        // nhận giả thiết cho lần đầu
        int valid = -1; 

        
        public p()
        {
            
            InitializeComponent();
            initArray();
            load();
            newR();
            initLblKq();
            btnTaoMoi.Visible = true; // btn Test
        }

        // tạo lại mảng R , mãng ẩn , de tao so -1, 2 dong 6 cot
        public void newR()
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 6; j++)
                    R[i, j] = 0;
        }

        // set lại kết quả cho biến
        public void kq()
        {
            this.alpha = Convert.ToDouble(lblkqAlpha.Text);
            this.beta = Convert.ToDouble(lblkqBeta.Text);
            this.gamma = Convert.ToDouble(lblkqgamma.Text);
            this.a = Convert.ToDouble(lblkqa.Text);
            this.b = Convert.ToDouble(lblkqb.Text);
            this.c = Convert.ToDouble(lblkqc.Text);
            this.hc = Convert.ToDouble(lblkqHc.Text);
            this.S = Convert.ToDouble(lblkqS.Text);
            this.P = Convert.ToDouble(lblkqP.Text);
        }
        
        public Label labelAt(int index)
        {
            
            if (index == 0)
                return lblkqAlpha;
            else if (index == 1)
                return lblkqBeta;
            else if (index == 2)
                return lblkqgamma;
            else if (index == 3)
                return lblkqa;
            else if (index == 4)
                return lblkqb;
            else if (index == 5)
                return lblkqc;
            else if (index == 6)
                return lblkqS;
            else if (index == 7)
                return lblkqHc;
            return null;
        } // trả về label để gán kết quả


        private void Form1_Load(object sender, EventArgs e)
        {
         
        }

        int check()
        {
            int canh=0;
            int goc=0;
            if (!txta.Text.Equals("0"))
                canh++;
            else
                canh--;
            if (!txtb.Text.Equals("0"))
                canh++;
            else
                canh--;
            if (!txtc.Text.Equals("0"))
                canh++;
            else
                canh--;

            if (!txtalpha.Text.Equals("0"))
                goc++;
            else
                goc--;
            if (!txtbeta.Text.Equals("0"))
                goc++;
            else
                goc--;
            if (!txtgamma.Text.Equals("0"))
                goc++;
            else
                goc--;
            if (canh == 3)
                return 5;
            if ( goc == -3 || canh == -3)
                return 0;

            return canh+goc+6;
        }

        bool isRunable()
        {
            //R1
            if (!txta.Text.Equals("0") && !txtb.Text.Equals("0") && !txtalpha.Text.Equals("0")) // thiếu beta
                return true;
            else if (!txta.Text.Equals("0") && !txtb.Text.Equals("0") && !txtbeta.Text.Equals("0")) // thiếu alpha
                return true;
            else if (!txta.Text.Equals("0") && !txtalpha.Text.Equals("0") && !txtbeta.Text.Equals("0")) // thiếu b
                return true;
            else if (!txtb.Text.Equals("0") && !txtalpha.Text.Equals("0") && !txtbeta.Text.Equals("0")) // thiếu a
                return true;

            //R2
            else if (!txtc.Text.Equals("0") && !txtb.Text.Equals("0") && !txtgamma.Text.Equals("0")) // thiếu beta
                return true;
            else if (!txtc.Text.Equals("0") && !txtb.Text.Equals("0") && !txtbeta.Text.Equals("0")) // thiếu gamma
                return true;
            else if (!txtc.Text.Equals("0") && !txtgamma.Text.Equals("0") && !txtbeta.Text.Equals("0")) // thiếu b
                return true;
            else if (!txtb.Text.Equals("0") && !txtgamma.Text.Equals("0") && !txtbeta.Text.Equals("0")) // thiếu c
                return true;

            //R3
            else if (!txta.Text.Equals("0") && !txtb.Text.Equals("0") && !txtP.Text.Equals("0")) // thiếu c
                return true;
            else if (!txtc.Text.Equals("0") && !txtb.Text.Equals("0") && !txtP.Text.Equals("0")) // thiếu a
                return true;
            else if (!txta.Text.Equals("0") && !txtc.Text.Equals("0") && !txtP.Text.Equals("0")) // thiếu b
                return true;
            else if (!txta.Text.Equals("0") && !txtc.Text.Equals("0") && !txtb.Text.Equals("0")) // thiếu P
                return true;

            //R4
            else if (!txtalpha.Text.Equals("0") && !txtbeta.Text.Equals("0")) // thiếu gamma
                return true;
            else if (!txtbeta.Text.Equals("0") && !txtgamma.Text.Equals("0")) // thiếu alpha
                return true;
            else if (!txtalpha.Text.Equals("0") && !txtgamma.Text.Equals("0")) // thiếu beta
                return true;

            //R5
            else if (!txtc.Text.Equals("0") && !txthc.Text.Equals("0")) // thiếu S
                return true;
            else if (!txtc.Text.Equals("0") && !txtS.Text.Equals("0")) // thiếu hc
                return true;
            else if (!txthc.Text.Equals("0") && !txtS.Text.Equals("0")) // thiếu c
                return true;
            return false;
        }

        public double toRad(double input)
        {
            return input * Math.PI / 180;
        } // chuyển đổi góc degree sang radians

        // gán kết quả của biến
        public void setKq() // set 1 lần đầu duy nhất
        {
            //1
            if (!txta.Text.Equals(null))
            {
                lblkqa.Text = txta.Text;

            }
            //2
            if (!txtb.Text.Equals(null))
            {
                lblkqb.Text = txtb.Text;
                
            }
            //3
            if (!txtc.Text.Equals(null))
            {
                lblkqc.Text = txtc.Text;
               
            }
            //4
            if (!txtalpha.Text.Equals(null))
            {
                lblkqAlpha.Text = txtalpha.Text;
                
            }
            //5
            if (!txtbeta.Text.Equals(null))
            {
                lblkqBeta.Text = txtbeta.Text;
                
            }
            //6
            if (!txtgamma.Text.Equals(null))
            {
                lblkqgamma.Text = txtgamma.Text;
                
            }
            //7
            if (!txthc.Text.Equals(null))
            {
                lblkqHc.Text = txthc.Text;
                
            }
            //8
            if (!txtS.Text.Equals(null))
            {
                lblkqS.Text = txtS.Text;
                
            }
            //9
            if (!txtP.Text.Equals(null))
            {
                lblkqP.Text = txtP.Text;

            }

        }
        // kết thúc gán

        // khởi tạo mảng ban đầu
        public void initArray()
        {
            state = new int[row,column];
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                {
                    state[i, j] = 0;
                }
        }
        // kết thúc khởi tạo

        public void initImg()
        {
            imgA.Visible = false;
            imgB.Visible = false;
            imgC.Visible = false;
            imgAlpha.Visible = false;
            imgBeta.Visible = false;
            imgGama.Visible = false;
            imgHC.Visible = false;
            imgS.Visible = false;
            img1.Visible = false;
            img2.Visible = false;
            img3.Visible = false;
            img4.Visible = false;
            img5.Visible = false;
        }

        public void initLblKq()
        {
            lblkqa.Text = "0";
            lblkqb.Text = "0";
            lblkqc.Text = "0";
            lblkqS.Text = "0";
            lblkqHc.Text = "0";
            lblkqAlpha.Text = "0";
            lblkqBeta.Text = "0";
            lblkqgamma.Text = "0";
            lblkqP.Text = "0";

        }

        // khởi tạo value cho label các biến
        public void initVariable()
        {
            txtalpha.Text = "0";
            txtbeta.Text = "0";
            txtgamma.Text = "0";
            txta.Text = "0";
            txtb.Text = "0";
            txtc.Text = "0";
            txtS.Text = "0";
            txthc.Text = "0";
            txtP.Text = "0";
        }
        // kết thúc khởi tạo value

        public void initTableValue()
        {
            tblDuLieu[1, 0].Value = "-1";
            tblDuLieu[1, 1].Value = "-1";
            tblDuLieu[1, 3].Value = "-1";
            tblDuLieu[1, 4].Value = "-1";
            tblDuLieu[2, 1].Value = "-1";
            tblDuLieu[2, 2].Value = "-1";
            tblDuLieu[2, 4].Value = "-1";
            tblDuLieu[2, 5].Value = "-1";
            tblDuLieu[3, 3].Value = "-1";
            tblDuLieu[3, 4].Value = "-1";
            tblDuLieu[3, 5].Value = "-1";
            tblDuLieu[3, 6].Value = "-1";
            tblDuLieu[4, 0].Value = "-1";
            tblDuLieu[4, 1].Value = "-1";
            tblDuLieu[4, 2].Value = "-1";
            tblDuLieu[5, 5].Value = "-1";
            tblDuLieu[5, 6].Value = "-1";
            tblDuLieu[5, 7].Value = "-1";

        }

        // chỉnh giao diện khởi tạo
        public void load()
        {
            initVariable();
            tblDuLieu.ReadOnly = true;
            tblDuLieu.Rows.Add(8);
            tblDuLieu[0, 0].Value = "α";
            
            tblDuLieu[0, 1].Value = "β";
            tblDuLieu[0, 2].Value = "γ";
            tblDuLieu[0, 3].Value = "a";
            tblDuLieu[0, 4].Value = "b";
            tblDuLieu[0, 5].Value = "c";
            tblDuLieu[0, 6].Value = "S";
            tblDuLieu[0, 7].Value = "hc";
            tblDuLieu[0, 8].Value = "P";

            for (int i = 0; i < row; i++)
                for (int j = 1; j < column; j++)
                {
                    {
                        tblDuLieu[j, i].Value = "0";
                    }
                }
            initTableValue();


            for (int i = 0; i < flag.Length; i++)
                flag[i] = 0;
        }
        // kết thúc chỉnh giao diện

        //tìm dòng của biến được xác định bởi công thức tương ứng với cột
        public int isXacDinhAt(int column)
        {
            for (int i = 0; i < row; i++)
                if (tblDuLieu[column, i].Value.Equals("-1"))
                    return i;
            return 0;
        }
        // kết thúc tìm

        // nhận giả thiết
        private void txtalpha_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Double.Parse(txtalpha.Text) > 0)
                { 
                    flag[0] = 1;
                    imgAlpha.Visible = true;
                }
            }
            catch (FormatException fe)
            {
                flag[0] = 0;
                //MessageBox.Show("phải là số");
                //txtalpha.Text = "0";
                imgAlpha.Visible = false;
            }
        }

        private void txtbeta_TextChanged(object sender, EventArgs e)
        {
        
            try
            {
                
                if (Double.Parse(txtbeta.Text) > 0)
                {
                    flag[1] = 1;
                    imgBeta.Visible = true;
                }


            }
            catch (FormatException fe)
            {
                    flag[1] = 0;
                    //MessageBox.Show("phải là số");
                    //txtbeta.Text = "0";
                    imgBeta.Visible = false;
                

            }
        }

        private void txtgamma_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Double.Parse(txtgamma.Text) > 0)
                {
                    flag[2] = 1;
                    imgGama.Visible = true;
                }
            }
            catch (FormatException fe)
            {
                flag[2] = 0;
                //MessageBox.Show("phải là số");
                //txtgamma.Text = "0";
                imgGama.Visible = false;
            }
        }

        private void txthc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Double.Parse(txthc.Text) > 0)
                {
                    flag[7] = 1;
                    imgHC.Visible = true;
                }
            }
            catch (FormatException fe)
            {
                flag[7] = 0;
                //MessageBox.Show("phải là số");
                //txthc.Text = "0";
                imgHC.Visible = false;
            }
        }

        private void txta_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Double.Parse(txta.Text) > 0)
                { 
                    flag[3] = 1;
                    imgA.Visible = true;
                }
            }
            catch (FormatException fe)
            {
                flag[3] = 0;
                //MessageBox.Show("phải là số");
                //txta.Text = "0";
                imgA.Visible = false;
            }
        }

        private void txtb_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Double.Parse(txtb.Text) > 0)
                {
                    flag[4] = 1;
                    imgB.Visible = true;
                }
            }
            catch (FormatException fe)
            {
                flag[4] = 0;
                //MessageBox.Show("phải là số");
                //txtb.Text = "0";
                imgB.Visible = false;
            }
        }

        private void txtc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Double.Parse(txtc.Text) > 0)
                {
                    flag[5] = 1;
                    imgC.Visible = true;
                }
            }
            catch (FormatException fe)
            {
                flag[5] = 0;
                //MessageBox.Show("phải là số");
                //txtc.Text = "0";
                imgC.Visible = false;
            }
        }

        private void txtS_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Double.Parse(txtS.Text) > 0)
                {
                    flag[6] = 1;
                    imgS.Visible = true;
                }
            }
            catch (FormatException fe)
            {
                flag[6] = 0;
                //MessageBox.Show("phải là số");
                //txtS.Text = "0";
                imgS.Visible = false;
            }
        }

        // kết thúc nhận giả thiết


        void update(int[] flag)
        {
            newR(); // tạo lại R
            alpha = Convert.ToDouble(txtalpha.Text);
            beta = Convert.ToDouble(txtbeta.Text);
            gamma = Convert.ToDouble(txtgamma.Text);
            a = Convert.ToDouble(txta.Text);
            b = Convert.ToDouble(txtb.Text);
            c = Convert.ToDouble(txtc.Text);
            S = Convert.ToDouble(txtS.Text);
            hc = Convert.ToDouble(txthc.Text);
            P = Convert.ToDouble(txtP.Text);
            // update bảng -1 thành 1
            for (int i = 0; i < 9; i++)
            {
                if (flag[i] != 0)
                {
                        for (int c = 1; c < column; c++)
                        {
                            if (tblDuLieu[c, i].Value.Equals("-1"))
                                tblDuLieu[c, i].Value = "1";
                        }
                }
            }

            // kết thúc update bảng

            // update R, đếm số lần số 1 và -1 xuất hiện
            //for (int r = 0; r < row; r++)
            //{
            //    for (int c = 1; c < column; c++)
            //    {
            //        if (tblDuLieu[c, r].Value.Equals("1"))
            //            R[0, c]++; // R[2,6]
            //        else if (tblDuLieu[c, r].Value.Equals("-1"))
            //            R[1, c]++;
            //    }
            //}
            //for (int c = 1; c < column; c++)
            //{
            //    for (int r = 0; r < row; r++)
            //    {
            //        if (tblDuLieu[c, r].Value.Equals("1"))
            //            R[0, c]++; // R[2,6]
            //        else if (tblDuLieu[c, r].Value.Equals("-1"))
            //            R[1, c]++;
            //    }
            //}


            // R[0,c] là tổng số 1 có trong cột c, còn R[1,c] là tổng số -1 có trong cột c
            // kết thúc update R

            // Tìm và xác định các biến có thể xác định dựa vào R
            for (int c = 1; c < column; c++ )
            {
                loop();
                Console.WriteLine(R[1,3]);
                if (R[1, c] == 1)
                {
                    kq(); // set giá trị cho biến
                    MessageBox.Show("Cột R" + c + " có biến ở dòng " + (isXacDinhAt(c) + 1).ToString() + " được xác định! ");
                    int temp = isXacDinhAt(c); // tìm vị trí số dòng của biến được xác định
                    //MessageBox.Show("temp la: " + temp.ToString());

                    // truyền số chỉ mục của cột vào hàm labelAt để gán kết quả biến sau khi tính vào đúng label
                    if (c == 1) 
                    {
                        labelAt(temp).Text = R1Formula(a, b, alpha, beta, temp).ToString();
                    }
                    if (c == 2)
                    {
                        labelAt(temp).Text = R2Formula(this.c, b, beta, gamma, temp).ToString();
                    
                    }
                    if (c == 3)
                    {
                        labelAt(temp).Text = R3Formula(this.c, b, a, S, temp).ToString();
                    
                    }
                    if (c == 4)
                    {
                        labelAt(temp).Text = R4Formula(alpha, beta, gamma, temp).ToString();
                    }
                    if (c == 5)
                    {
                        labelAt(temp).Text = R5Formula(S, hc, this.c, temp).ToString();
                    }
                    // sau khi tinh toan xong
                    for (int c1 = 1; c1 < column; c1++)
                    {
                        MessageBox.Show(tblDuLieu[c1, temp].ToString());
                        if (tblDuLieu[c1, temp].Value.Equals("-1"))
                            tblDuLieu[c1, temp].Value = "1";
                    }
                    c = 1;
                }
            }

        }


        // hàm tính toán
        public double R1Formula(double a, double b, double alpha, double beta, int index)
        {
            //R1:  a/ sina = b/sinb
            MessageBox.Show("run R1");
            img1.Visible = true;
            double kq = 0;
            if (index ==3 ) //(a == 0)
            {
                double tempa = toRad(alpha);
                double tempb = toRad(beta);
                a = b * Math.Sin(tempa) / Math.Sin(tempb);
                kq = a;
                imgA.Visible = true;
            }
            else if (index ==  4)//(b == 0)
            {
                double tempb = toRad(beta);
                double tempa = toRad(alpha);
                b = a * Math.Sin(tempb) / Math.Sin(tempa);
                kq = b;
                imgB.Visible = true;
            }
            else if (index == 0) // (alpha == 0)
            {
                //(Math.Asin(1)*180/Math.PI).ToString() là 90;  
                //R1:  a/ sina = b/sinb
                double sinalpha = a * Math.Sin(toRad(beta)) / b;
          
                alpha = Math.Asin(sinalpha)*180/Math.PI;
                kq = alpha;
                imgAlpha.Visible = true;
            }
            else if (index ==  1)// (beta == 0)
            {
                double sinbeta = b * Math.Sin(toRad(alpha)) / a;
                beta = Math.Asin(Math.Sin(sinbeta)) * 180 / Math.PI;
                //beta = Math.Asin(b * Math.Sin(alpha * Math.PI / 180) / a) * 180 / Math.PI;
                kq = beta;
                imgBeta.Visible = true;
            }

            return Math.Round((Double)kq, 2);
        }

        public double R2Formula(double c, double b, double beta, double gamma, int index)
        {
            //R2: c/siny = b/sinb
            //MessageBox.Show("run R2: c-" + c + "  b-" + b + "  beta-" +beta + " gamma-" + gamma);
            MessageBox.Show("run R2");
            img2.Visible = true;
            double kq = 0;
            if (index ==  5) //(c == 0)
            {
                double tempb = toRad(beta);
                double tempc = toRad(gamma);
                c = b * Math.Sin(tempc) / Math.Sin(tempb);
                kq = c;
                imgC.Visible = true;
            }
            else if (index ==  4) // (b == 0)
            {
                double tempc = toRad(gamma);
                double tempb = toRad(beta);
                b = c * Math.Sin(tempb) / Math.Sin(tempc);
                kq = b;
                imgB.Visible = true;
            }
            else if (index ==  1) //(beta == 0)
            {
                double sinbeta = b * Math.Sin(toRad(gamma)) / c;
                //beta = Math.Asin(b * Math.Sin(gamma * Math.PI / 180) / c) / Math.PI * 180;
                beta = Math.Asin(Math.Sin(sinbeta)) / Math.PI * 180;
                kq = beta;
                imgBeta.Visible = true;
            }
            else if (index ==  2) //(gamma == 0)
            {
                double singamma = c * Math.Sin(toRad(beta)) / b;
                gamma = Math.Asin(Math.Sin(singamma)) / Math.PI * 180;
                //gamma = Math.Asin(c * Math.Sin(beta * Math.PI / 180) / b) * 180 / Math.PI;
                kq = gamma;
                imgGama.Visible = true;
            }

            return Math.Round((Double)kq, 2);
        }

        public double R3Formula(double c, double b, double a, double S, int index)
        {
            //R3: S = căn(p.(p-a) (p-b) (p-c) )

            MessageBox.Show("run R3");
            img3.Visible = true;
            double kq = 0;
            double p = (a + b + c) / 2;
            double p1 = Math.Pow(9, 2) / 4.5;
            MessageBox.Show(p1.ToString());
            if (index ==  5) //(c == 0)
            {
                c = p - Math.Pow(S, 2) / (p * (p - b) * (p - a));
                kq = c;
                imgC.Visible = true;
            }
            else if (index ==  4) // (b == 0)
            {
                b = p - Math.Pow(S, 2) / (p * (p - c) * (p - a));
                kq = b;
                imgB.Visible = true;
            }
            else if (index ==  3) // (a == 0)
            {
                a = p - Math.Pow(S, 2) / (p * (p - b) * (p - c));
                kq = a;
                imgA.Visible = true;
            }
            else if (index ==  6)//(S == 0)
            {
                S = Math.Sqrt(p*(p-a)*(p-b)*(p-c));
                kq = S;
                imgS.Visible = true;
            }

            return Math.Round((Double)kq, 2);
        }

        public double R4Formula(double alpha, double beta, double gamma, int index)
        {
            MessageBox.Show("run R4");
            img4.Visible = true;
            //R4: alpha + beta + gamma = 180;
            double kq = 0;
            if (index ==  0) // (alpha == 0)
            {
                alpha = 180 - beta - gamma;
                kq = alpha;
                imgAlpha.Visible = true;
            }
            else if (index ==  1) //(beta == 0)
            {
                beta = 180 - alpha - gamma;
                kq = beta;
                imgBeta.Visible = true;
            }
            else if (index ==  2) // (gamma == 0)
            {
                gamma = 180 - beta - alpha;
                kq = gamma;
                imgGama.Visible = true;
            }
            return Math.Round((Double)kq,2);
        }

        public double R5Formula(double S, double hc, double c, int index)
        {
            //R5: S = 1/2 hc.c
            MessageBox.Show("run R5");
            img5.Visible = true;
            double kq = 0;
            if (index ==  6) // (S == 0)
            {
                S = 0.5 * hc * c;
                kq = S;
                imgS.Visible = true;
            }
            else if (index ==  7) //(hc == 0)
            {
                hc = 2 * S / c;
                kq = hc;
                imgHC.Visible = true;
            }
            else if (index ==  5) //(c == 0)
            {
                c = 2 * S / hc;
                kq = c;
                imgC.Visible = true;
            }
            return Math.Round((Double)kq, 2);
        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        
        private void btnNhap_Click(object sender, EventArgs e)
        {
        if (!isRunable() || check() < 5)
        {
            MessageBox.Show("Dữ liệu nhập chưa phù hợp");
            initVariable();
            initImg();
        }
        else
        {
            for (int i = 0; i < flag.Length; i++)
            {
                if (flag[i] != 0)
                {   
                    if (this.valid == -1)
                    {
                        setKq();
                        this.valid = 0;
                    }
                    update(flag);
                    break;
                    
                }
                if (i == flag.Length - 1)
                {
                    MessageBox.Show("Chưa có dữ liệu");
                    initVariable();
                    initImg();
                }
     
            }
        }
}

        
        // btn test
        private void btnTaoMoi_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đã tạo mới chương trình!");
            this.valid = -1;
            initArray();
            initImg();
            //load();
            initVariable();
            initLblKq();
            initTableValue();
            for (int i = 0; i < flag.Length; i++)
                flag[i] = 0;
            newR();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void imgC_Click(object sender, EventArgs e)
        {

        }

        private void txtbeta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtalpha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        private void txtgama_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        private void txta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        private void txtb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        private void txtc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        private void txthc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        private void txtS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        private void txtP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
        private void loop()
        {
            R = new int[2, 6];
            for (int c = 1; c < column; c++)
            {
                for (int r = 0; r < row; r++)
                {
                    if (tblDuLieu[c, r].Value.Equals("1"))
                        R[0, c]++; // R[2,6]
                    else if (tblDuLieu[c, r].Value.Equals("-1"))
                        R[1, c]++;
                }
            }
        
}
    }
}
