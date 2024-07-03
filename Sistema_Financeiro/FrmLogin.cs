using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Mysqlx.Crud;

namespace Sistema_Financeiro
{
    public partial class FrmLogin : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );


        public FrmLogin()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string usuario = txtLogin.Text;
            string senha = txtSenha.Text;
            string sql = $"SELECT * FROM tab_usuarios " +
                $"WHERE usuario=@usuario AND senha=@senha";
            Conexao.Conectar();
            MySqlCommand cmd = new MySqlCommand(sql, Conexao.conn);
            DataTable dt = new DataTable();
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.Parameters.AddWithValue("senha", senha);
            dt.Load(cmd.ExecuteReader());
            if (dt.Rows.Count == 1)
            {
                Geral.id_usuario = int.Parse(dt.Rows[0]["id"].ToString());
                Geral.nivel_usuario = int.Parse(dt.Rows[0]["nivel_id"].ToString());
                FrmMenu tela = new FrmMenu();
                this.Hide();
                tela.Show();
            }
            else
            {
                MessageBox.Show("Usuário ou senha invalido");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void chbSenha_CheckedChanged(object sender, EventArgs e)
        {
            txtSenha.PasswordChar = chbSenha.Checked ? '\0' : '*';
        }
    }
}
