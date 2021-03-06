﻿using System;
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
    public partial class frEditarLivro : Form
    {
        string capaAtual;
        public frEditarLivro(int nlivro)
        {
            InitializeComponent();
            //consulta a base de dados
            string sql = "select * from Livros WHERE nlivro=" + nlivro;
            DataTable livro = BaseDados.Instance.devolveConsulta(sql);
            //preencher o form
            lbnlivro.Text = nlivro.ToString();
            //nome
            textBox1.Text = livro.Rows[0][1].ToString();
            //ano
            textBox2.Text = livro.Rows[0][2].ToString();
            dateTimePicker1.Value = DateTime.Parse(livro.Rows[0][3].ToString());
            //preco
            textBox3.Text = livro.Rows[0][4].ToString();
            //capa
            if(File.Exists(livro.Rows[0][5].ToString()))
            pictureBox1.Image = Image.FromFile(livro.Rows[0][5].ToString());
            capaAtual = livro.Rows[0][5].ToString();
            lbCapa.Text = capaAtual;

        }

        private void frEditarLivro_Load(object sender, EventArgs e)
        {

        }

        private void fsd_Click(object sender, EventArgs e)
        {
            OpenFileDialog cxDialogo = new OpenFileDialog();
            DialogResult resposta = cxDialogo.ShowDialog();
            if (resposta != DialogResult.OK)
                return;
            lbCapa.Text = cxDialogo.FileName;
            pictureBox1.Image = Image.FromFile(lbCapa.Text);

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //validar dados do form, copiar imagem para pasta,insert into
            string nome = textBox1.Text;
            int ano = int.Parse(textBox2.Text);
            DateTime data = dateTimePicker1.Value;
            decimal preco = decimal.Parse(textBox3.Text);
            //Copiar imagem para uma pasta
            string nomeImagem = DateTime.Now.Ticks.ToString();
            string pastaImagens = Application.UserAppDataPath;
            if (Directory.Exists(pastaImagens) == false)
            {
                Directory.CreateDirectory(pastaImagens);
            }
            string[] extensao = lbCapa.Text.Split('.');
            string nomeCompleto = pastaImagens + '\\' + nomeImagem + "." + extensao[extensao.Length - 1];
            File.Copy(lbCapa.Text, nomeCompleto);

            string sql = $@"UPDATE Livros SET nome=@nome,ano=@ano,data_aquisicao=@data_aquisicao,preco=@preco,capa=@capa where nlivro=@nlivro";
           
          
            //parametros
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter(){ParameterName = "@nome", SqlDbType = SqlDbType.VarChar, Value = nome},
                new SqlParameter(){ParameterName = "@ano", SqlDbType = SqlDbType.Int, Value = ano},
                new SqlParameter(){ParameterName = "@data_aquisicao", SqlDbType = SqlDbType.Date, Value = data},
                new SqlParameter(){ParameterName = "@preco", SqlDbType = SqlDbType.Decimal, Value = preco},
                new SqlParameter(){ParameterName = "@capa", SqlDbType = SqlDbType.VarChar, Value = nomeCompleto},
                new SqlParameter(){ParameterName = "@nlivro", SqlDbType = SqlDbType.Int, Value = int.Parse(lbnlivro.Text)},
            };
            BaseDados.Instance.executaSQL(sql, parametros);
            //apagar capa atual
            pictureBox1.Image = null;
            GC.Collect();
            try
            {
                File.Delete(capaAtual);
            }catch(Exception erro)
            {
                Console.Write(erro.Message);
            }
            File.Delete(capaAtual);
            //fechar formulario
            this.Close();
            
        }
    }
}
