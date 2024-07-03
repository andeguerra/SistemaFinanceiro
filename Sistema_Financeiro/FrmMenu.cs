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
    public partial class FrmMenu : Form
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

        private Button btnAtual;
        private Form formAtual;
        public FrmMenu()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }
        private void AbrirForm(Form formsel, object btnSender)
        {
            if(formAtual != null)
            {
                formAtual.Close();
            }

            AtivarBtn(btnSender);
            formAtual = formsel;
            formsel.TopLevel = false;
            formsel.FormBorderStyle = FormBorderStyle.None;
            formsel.Dock = DockStyle.Fill;
            this.pnCentral.Controls.Add(formsel);
            this.pnCentral.Tag = formsel;
            formsel.BringToFront();
            formsel.Show();

        }
        private void AtivarBtn(object btnSender)
        {
            if(btnSender != null) 
            {
                if(btnAtual != (Button)btnSender)    
                {
                    DesativarBtn();
                    btnAtual = (Button)btnSender;
                    btnAtual.BackColor = Color.FromArgb(24, 30, 54);
                    btnAtual.Font = new System.Drawing.Font("Verdana",
                        10F, System.Drawing.FontStyle.Bold,
                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btnAtual.ForeColor = Color.White;
                }
            }
        }

        private void DesativarBtn()
        {
            foreach(Control btnAnt in pnMenu.Controls)
            {
                if(btnAnt.GetType() == typeof(Button))
                {
                    btnAnt.BackColor = Color.FromArgb(255, 255, 255);
                    btnAnt.Font = new System.Drawing.Font("Verdana", 9.75F,
                        System.Drawing.FontStyle.Regular,
                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btnAnt.ForeColor = Color.DimGray;
                    btnAtual = null;
                    
                }
            }
        }

        private void FrmMenu_Load(object sender, EventArgs e)
        {
            if (Geral.nivel_usuario == 1)
            {
                btnNivel.Text = "Nível de acesso Administrador";
            }
            else
            {
                btnNivel.Text = "Nivel de acesso Usuário";
                button4.Visible = false;
            }
            try
            {
                string sql = $"SELECT usuario FROM tab_usuarios WHERE id = {Geral.id_usuario}";
                Conexao.Conectar();
                MySqlCommand cmd = new MySqlCommand(sql, Conexao.conn);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                btnNome.Text = "Bem vindo(a) " + dt.Rows[0]["usuario"].ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           if(formAtual != null)
            {
                formAtual.Close();
                DesativarBtn();
                AtivarBtn(sender);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AbrirForm(new FrmControleFn(), sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AtivarBtn(sender);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AbrirForm(new FrmUsuarios(), sender);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var dialogRes = MessageBox.Show("Deseja sair do sistema?",
                "Sair do sistema",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if(dialogRes == DialogResult.Yes)
            {
                Geral.Sair();
                FrmLogin tela = new FrmLogin();
                this.Hide();
                tela.Show();
            }
        }
    }
}
