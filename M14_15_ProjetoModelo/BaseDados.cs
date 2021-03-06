﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;

namespace M14_15_ProjetoModelo
{
    class BaseDados
    {
        private static BaseDados instance;
        public static BaseDados Instance
        {
            get {
                if (instance == null)
                    instance = new BaseDados();
                return instance;
                }
        }

        string strLigacao;
        SqlConnection ligacaoBD;

        public BaseDados()
        {
            strLigacao = ConfigurationManager.ConnectionStrings["sql"].ToString();
            ligacaoBD = new SqlConnection(strLigacao);
            ligacaoBD.Open();
        }
        ~BaseDados()
        {
            try
            {
                ligacaoBD.Close();
            } catch (Exception e)
            {
                Console.Write(e.Message);
            }
            
        }
        /// <summary>
        /// Recebe um comando SQL e executa-o na base de dados
        /// </summary>
        /// <param name="sql">Comando SQL</param>
        public void executaSQL(string sql)
        {
            SqlCommand comando = new SqlCommand(sql, ligacaoBD);
            comando.ExecuteNonQuery();
            comando.Dispose();
            comando = null;
        }
        /// <summary>
        /// Recebe um comando SQL e a lista de parametros e executa-o na base de dados
        /// </summary>
        /// <param name="sql"> Comando SQL</param>
        /// <param name="parametros"></param>
        public void executaSQL(string sql, List<SqlParameter> parametros)
        {
            SqlCommand comando = new SqlCommand(sql, ligacaoBD);
            comando.Parameters.AddRange(parametros.ToArray());
            comando.ExecuteNonQuery();
            comando.Dispose();
            comando = null;
        }
        public DataTable devolveConsulta(string sql)
        {
            SqlCommand comando = new SqlCommand(sql, ligacaoBD);
            DataTable registos = new DataTable();
            SqlDataReader dados = comando.ExecuteReader();
            registos.Load(dados);
            dados = null;
            comando.Dispose();
            return registos;

        }
    }
}
