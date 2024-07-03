using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Financeiro
{
    internal class Geral
    {

        public static int nivel_usuario = 0;
        public static int id_usuario = 0;

        public static void Sair()
        {
            nivel_usuario = 0; 
            id_usuario = 0;
        }
        public static DataTable Selecionar(string sql)
        {
            Conexao.Conectar();
            MySqlCommand cmd = new MySqlCommand(sql, Conexao.conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            Conexao.Desconectar();

            return dt;
        }

        public static void InserirLog(string msg, int idUsuario)
        {
            Conexao.Conectar();
            string sql = $"insert into tab_logs(msg_erro,usuario_id)" +
                $" VALUES(@msg_erro,@usuario_id)";
            MySqlCommand cmd = new MySqlCommand(sql,Conexao.conn);
            cmd.Parameters.AddWithValue("msg_erro", msg);
            cmd.Parameters.AddWithValue("usuario_id", idUsuario);
            cmd.ExecuteNonQuery();
            Conexao.Desconectar();
        }
    }
}
