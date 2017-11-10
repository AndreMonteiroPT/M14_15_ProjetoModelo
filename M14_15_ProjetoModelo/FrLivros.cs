using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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
            atualizarGrelha();
        }
        public void atualizarGrelha()
        {
            dataGridView1.DataSource = BaseDados.Instance.devolveConsulta("SELECT * FROM Livros");
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
            //Copiar imagem para uma pasta
            string nomeImagem = DateTime.Now.Ticks.ToString();
            string pastaImagens = Application.UserAppDataPath;
            if(Directory.Exists(pastaImagens) == false)
            {
                Directory.CreateDirectory(pastaImagens);
            }
            string[] extensao = lbCapa.Text.Split('.');
            string nomeCompleto = pastaImagens + '\\' + nomeImagem + "." + extensao[extensao.Length - 1];
            File.Copy(lbCapa.Text, nomeCompleto);

            string sql = $@"INSERT INTO livros(nome, ano, data_aquisicao,preco,capa,estado) 
            VALUES(@nome,@ano,@data_aquisicao,@preco, @capa, @estado)";
            //parametros
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter(){ParameterName = "@nome", SqlDbType = SqlDbType.VarChar, Value = nome},
                new SqlParameter(){ParameterName = "@ano", SqlDbType = SqlDbType.Int, Value = ano},
                new SqlParameter(){ParameterName = "@data_aquisicao", SqlDbType = SqlDbType.Date, Value = data},
                new SqlParameter(){ParameterName = "@preco", SqlDbType = SqlDbType.Decimal, Value = preco},
                new SqlParameter(){ParameterName = "@capa", SqlDbType = SqlDbType.VarChar, Value = nomeCompleto},
                new SqlParameter(){ParameterName = "@estado", SqlDbType = SqlDbType.Bit, Value = true},
            };
            BaseDados.Instance.executaSQL(sql, parametros);
           

            //atualiza a grelha
            atualizarGrelha();
        }
        //escolher capa
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog cxDialogo = new OpenFileDialog();
            DialogResult resposta = cxDialogo.ShowDialog();
            if(resposta != DialogResult.OK)
                return;
             lbCapa.Text = cxDialogo.FileName;
             pictureBox1.Image = Image.FromFile(lbCapa.Text);
            
        }

        //Remover livros
        private void button3_Click(object sender, EventArgs e)
        {
            //Saber linha selecionada
            if (dataGridView1.CurrentCell == null)
                return;//Garantir que nao esta vazio e que esta qualquer coisa selecionada
            int linha = dataGridView1.CurrentCell.RowIndex;
            if (linha < 0) { MessageBox.Show("Selecione o livro a remover");
                return;
            }
            //nlivro
            int nlivro = int.Parse(dataGridView1.Rows[linha].Cells[0].Value.ToString());
            if (MessageBox.Show("Tem acerteza que pretende eliminar o livro nº" + nlivro, "Remover?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            //consulta pra remover a capa
            string capa = dataGridView1.Rows[linha].Cells[5].Value.ToString();
            //possivel erro!
            if (File.Exists(capa))
            {
                File.Delete(capa);
            }
            //executar o sql com o delete
            string sql = "DELETE from livros where nlivro=" + nlivro;
            BaseDados.Instance.executaSQL(sql);
            //atualizar a grelha
            atualizarGrelha();
        }

        private void FrLivros_Load(object sender, EventArgs e)
        {

        }
        //menu contexto da grid
        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //nlivro
            if (dataGridView1.CurrentCell == null) return;
            int linha = dataGridView1.CurrentCell.RowIndex;
            int nlivro = int.Parse(dataGridView1.Rows[linha].Cells[0].Value.ToString());
            //FORM EDITAR LIVRO 
            frEditarLivro F = new frEditarLivro(nlivro);
            F.ShowDialog();
            atualizarGrelha();
            

            
        }
    }
}
