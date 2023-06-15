using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Point_of_Sale
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public double CostOfItems()//просто сумма чисел
        {
            double sum = 0;
            int i = 0;
            for (i = 0; i < dataGridView1.Rows.Count; i++)//цикл персчитывает относительно числа строк 
            {
                sum = sum + Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
            }
            return sum;
        }

        private void AddCost()
        {
            Double tax, q;//налог и добавочная цена
            tax = 3.9;
            if (dataGridView1.Rows.Count > 0)
            {
                lblTax.Text = String.Format("{0:c2}", (((CostOfItems() * tax) / 100)));//{ 0:c2} в данном коде представляет форматирование числа в денежный формат с двумя знаками после запятой.
                lblSubTotal.Text = String.Format("{0:c2}", (CostOfItems())); //промежуточная стоимость (до добавления налога)
                q = ((CostOfItems() * tax) / 100);//вычисляет добавочную стоимость q путем умножения стоимости товаров на налог в процентах, деления на 100 и добавления стоимости товаров.
                lblTotal.Text = String.Format("{0:c2}", (CostOfItems() + q));//итоговая цена = сумма товаров плюс добавочная стоимость с налога
                lblBarCode.Text = Convert.ToString(q + CostOfItems());//бар код
            }
           
        }

        private void Change()
        {
            Double tax, q, c;//налог, итоговая цена, полученные деньги
            tax = 3.9;
            if (dataGridView1.Rows.Count >  0)//если количество строк больше нуля
            {
                q = ((CostOfItems() * tax) / 100) + CostOfItems();
                c = Convert.ToInt32(lblCash.Text);
                lblChange.Text = String.Format("{0:c2}", c - q);//сдача отображается денежным типом данных
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        Bitmap bitmap;
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                int height = dataGridView1.Height;//Получаем высоту таблицы данных height.
                int printheight = dataGridView1.RowCount * dataGridView1.RowTemplate.Height * 30;//	Рассчитываем высоту для печати printheight, умножая количество строк таблицы данных на высоту шаблона строки RowTemplate.Height, умноженную на 30.
                bitmap = new Bitmap(dataGridView1.Width, printheight);//	Создаём новый объект типа Bitmap с размерами таблицы данных dataGridView1.Width и printheight.
                dataGridView1.DrawToBitmap(bitmap, new Rectangle(0, 0, printheight, dataGridView1.Width));//	Заполняем объект типа Bitmap содержимым таблицы данных dataGridView1, используя метод DrawToBitmap.
                printPreviewDialog1.PrintPreviewControl.Zoom = 1;//Устанавливаем масштаб печати PrintPreviewDialog.Zoom в 1 и отображаем диалог предварительного просмотра печати printPreviewDialog1.ShowDialog().
                printPreviewDialog1.ShowDialog();
                //dataGridView1.Height = height;
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)//(на будущее)не удалять, полетит верстка
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                e.Graphics.DrawImage(bitmap, 0, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       /*  Этот код отвечает за печать страницы при использовании компонента PrintDocument.В методе printDocument1_PrintPage выполняется рисование изображения на странице печати.

e это объект PrintPageEventArgs, который содержит информацию о странице печати и предоставляет доступ к графическому контексту для рисования на странице.
e.Graphics это методы и свойства для рисования на странице печати.
DrawImage используется для рисования изображения bitmap на странице печати с координатами (0, 0), что означает, что изображение будет размещено в левом верхнем углу страницы.*/
        private void btnReset_Click(object sender, EventArgs e)//обнуление всех текстбоксов
        {
            try
            {
                lblBarCode.Text = "";
                lblCash.Text = "0"; 
                lblSubTotal.Text = "";
                lblTax.Text = "";
                lblTotal.Text = "";
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                cboPayment.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)//добавление данных в форму
        {
            cboPayment.Items.Add("Наличные");
            cboPayment.Items.Add("Сбербанк");
            cboPayment.Items.Add("Втб");
            cboPayment.Items.Add("Тинькофф банк");
        }

        private void NumbersOnly(object sender, EventArgs e)
        {
            Button b = (Button)sender;//ссылка на кнопку, которая вызвала метод
            if (lblCash.Text == "0")// проверяет, если текстовое значение lblCash равно "0", то очищает его перед добавлением нового значения.
            {
                lblCash.Text = "";
                lblCash.Text = b.Text;
            }
            else if (b.Text == ".")
            {
                if (! lblCash.Text.Contains("."))// проверяет, если в поле lblCash.Text уже содержится символ ".", то символ не добавляется, чтобы избежать повторных точек в числе.(Contains - содержит)
                {
                    lblCash.Text = lblCash.Text + b.Text;
                }
            }
            else
            {
                lblCash.Text = lblCash.Text + b.Text;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            lblCash.Text = "0";
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (cboPayment.Text == "Наличные")//если в форме выбора расплаты выбраны наличные, используем метод для расчета сдачи, если нет то просто обнуляем счетчик(будто бы товар оплачен)
            {
                Change();
            }
            else
            {
                lblChange.Text = "";
                lblCash.Text = "0";
            }
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            foreach ( DataGridViewRow row in  this.dataGridView1.SelectedRows)//сносим значение каждой строки и сносим саму строку в датагрид впринципе
            {
                dataGridView1.Rows.Remove(row);
            }
            AddCost();
            if (cboPayment.Text == "Наличные")//тоже самое, что и 18 строк выше
            {
                Change();
            }
            else
            {
                lblChange.Text = "";
                lblCash.Text = "0";
            }
        }

        private void burgerbtn1_Click(object sender, EventArgs e)
        {
            Double CostofItem = 2.3;//цена товара
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)//просто цикл с переменной
            {
                if ((bool)(row.Cells[0].Value = "Гамбургер"))//одновременно присвоение значение в первый столбик строки и проверка на пустоту, всегда выводит true, так как строка 
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);//значение второй ячейки увеличивается на 1(впринципе оно всегда 1)
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;//произведение значения во втором столбике и CostofItem(цена товара)

                }
            }
            dataGridView1.Rows.Add("Гамбургер" , "1", CostofItem);//обновление строки
            AddCost();//вызываю вычисление налога, промежуточной цены и итоговой цены с добавочной ценой
        }

        private void burgerbtn2_Click(object sender, EventArgs e)
        {
            Double CostofItem = 1.2;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Чизбургер"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Чизбургер", "1", CostofItem);
            AddCost();
        }

        private void burgerbnt3_Click(object sender, EventArgs e)
        {
            Double CostofItem = 2.9;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Гамбургер с картошкой"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Гамбургер с картошкой", "1", CostofItem);
            AddCost();
        }

        private void burgerbtn4_Click(object sender, EventArgs e)
        {
            Double CostofItem = 2.1;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Чизбургер макс"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Чизбургер макс", "1", CostofItem);
            AddCost();
        }

        private void shaurmabtn1_Click(object sender, EventArgs e)
        {
            Double CostofItem = 1.9;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Тако"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Тако", "1", CostofItem);
            AddCost();
        }

        private void pizzabtn1_Click(object sender, EventArgs e)
        {
            Double CostofItem = 1.9;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Пицца пеперони"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Пицца пеперони", "1", CostofItem);
            AddCost();
        }

        private void waterbtn3_Click(object sender, EventArgs e)
        {
            Double CostofItem = 2.9;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Кофе растворимый"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Кофе растворимый", "1", CostofItem);
            AddCost();
        }

        private void pizzabtn2_Click(object sender, EventArgs e)
        {
            Double CostofItem = 1.2;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Маргарита"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Маргарита", "1", CostofItem);
            AddCost();
        }

        private void pizzabtn3_Click(object sender, EventArgs e)
        {
            Double CostofItem = 2.2;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Пицца тысяча сыров"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Чизбургер", "1", CostofItem);
            AddCost();
        }

        private void pizzabtn4_Click(object sender, EventArgs e)
        {
            Double CostofItem = 2.75;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Чизбергер"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Чизбургер", "1", CostofItem);
            AddCost();
        }

        private void shaurmabtn2_Click(object sender, EventArgs e)
        {
            Double CostofItem = 1.8;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Шаурма стандарт"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Шаурма стандарт", "1", CostofItem);
            AddCost();
        }

        private void shaurmabtn3_Click(object sender, EventArgs e)
        {
            Double CostofItem = 2.0;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Шаурма в сырном лаваше"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Шаурма в сырном лаваше", "1", CostofItem);
            AddCost();
        }

        private void waterbtn1_Click(object sender, EventArgs e)
        {
            Double CostofItem = 0.9;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Чай черный"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Чай черный", "1", CostofItem);
            AddCost();
        }

        private void waterbtn2_Click(object sender, EventArgs e)
        {
            Double CostofItem = 1.20;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Латте"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Латте", "1", CostofItem);
            AddCost();
        }

        private void waterbtn4_Click(object sender, EventArgs e)
        {
            Double CostofItem = 0.59;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Апельсиновый сок"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Апельсиновый сок", "1", CostofItem);
            AddCost();
        }

        private void waterbtn5_Click(object sender, EventArgs e)
        {
            Double CostofItem = 2.57;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Чай зеленый"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Чай зеленый", "1", CostofItem);
            AddCost();
        }

        private void waterbtn6_Click(object sender, EventArgs e)
        {
            Double CostofItem = 1.2;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Сок яблочный"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Сок яблочный", "1", CostofItem);
            AddCost();
        }

        private void waterbtn7_Click(object sender, EventArgs e)
        {
            Double CostofItem = 2.0;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Чай китайский"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Чай китайский", "1", CostofItem);
            AddCost();
        }

        private void waterbtn8_Click(object sender, EventArgs e)
        {
            Double CostofItem = 1.59;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Чай с индийский"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Чай ндийский", "1", CostofItem);
            AddCost();
        }

        private void cakebtn1_Click(object sender, EventArgs e)
        {
            Double CostofItem = 3.95;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Кекс клубничный"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Кекс клубничный", "1", CostofItem);
            AddCost();
        }

        private void cakebtn2_Click(object sender, EventArgs e)
        {
            Double CostofItem = 3.2;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Кекс вишневый"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Кекс вишневый", "1", CostofItem);
            AddCost();
        }

        private void cakebtn3_Click(object sender, EventArgs e)
        {
            Double CostofItem = 2.78;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Кекс с сердечком"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Кекс с сердеком", "1", CostofItem);
            AddCost();
        }

        private void cakebtn4_Click(object sender, EventArgs e)
        {
            Double CostofItem = 3.40;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                if ((bool)(row.Cells[0].Value = "Кекс с голубикой"))
                {
                    row.Cells[1].Value = Double.Parse((string)row.Cells[1].Value + 1);
                    row.Cells[2].Value = Double.Parse((string)row.Cells[1].Value) * CostofItem;

                }
            }
            dataGridView1.Rows.Add("Кекс с голубикой", "1", CostofItem);
            AddCost();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
