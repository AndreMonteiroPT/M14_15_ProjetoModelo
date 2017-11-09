using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M14_15_ProjetoModelo
{
    public partial class FrLivros : Form
    {
        public FrLivros()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //validar dados do form, copiar imagem para pasta,insert into
            string nome = textBox1.Text;
            int ano = int.Parse(textBox2.Text);
            DateTime data = dateTimePicker1.Value;
            decimal preco = decimal.Parse(textBox3.Text);

            string sql = $@"INSERT INTO livros(nome, ano, data_aquisicao,preco) 
            VALUES(' {nome}', {ano}, '{data}', {preco})";

            BaseDados BD = new BaseDados();
            BD.executaSQL(sql);

        }
    }
}
