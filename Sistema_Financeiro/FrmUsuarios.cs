using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistema_Financeiro
{
    public partial class FrmUsuarios : Form
    {
        int id = 0;
        public FrmUsuarios()
        {
            InitializeComponent();
        }

        // Função para limpar os campos txt, voltar os btn ao estado inicial, recarregar oa dados da dgv
        private void Resetar()
        {
            try
            {
                dataGridView1.DataSource = Geral.Selecionar("select * from tab_usuarios");
                txtPesquisa.Clear();
                txtUsuario.Clear();
                txtSenha.Clear();
                btnCadastrar.Enabled = true;
                btnAtualizar.Enabled = false;
                btnExcluir.Enabled = false;
                id = 0;
            }
            catch (MySqlException exm)
            {
                MessageBox.Show("Erro ao tentar atualizar os dados da tabela de usuários");
                Geral.InserirLog(exm.Message, Geral.id_usuario);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao tentar atualizar os dados da tabela de usuários");
                Geral.InserirLog(ex.Message, Geral.id_usuario);
            }
        }

        // Carrega os dados na dgv ao iniciar o Form
        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            string sql = "select * from tab_nivel";
            cbNivel.DataSource = Geral.Selecionar(sql);
            cbNivel.DisplayMember = "nivel";
            cbNivel.ValueMember = "id";

            dataGridView1.DataSource = Geral.Selecionar($"select u.id, u.usuario,u.dt_user,u.nivel_id,n.nivel " +
                $"from tab_usuarios as u inner join tab_nivel as n on u.nivel_id = n.id");

            // Esconder a coluna de ID
            dataGridView1.Columns[3].Visible = false;

        }

        // Insere no DB um usuário
        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text; 
            string senha = txtSenha.Text;
            int nivel = int.Parse(cbNivel.SelectedValue.ToString());

            if(!String.IsNullOrWhiteSpace(usuario))
            {                              
                try
                {
                    Conexao.Conectar();
                    string sql = "insert into tab_usuarios(usuario,senha,nivel_id)" +
                        " values(@usuario,@senha,@nivel_id)";
                    MySqlCommand cmd = new MySqlCommand(sql, Conexao.conn);
                    cmd.Parameters.AddWithValue("usuario", usuario);
                    cmd.Parameters.AddWithValue("senha", senha);
                    cmd.Parameters.AddWithValue("nivel_id", nivel);
                    cmd.ExecuteNonQuery();//executa o script sql
                    MessageBox.Show("Novo usuário cadastrado!!!",
                        "Cadastro de usuário",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    Resetar();
                }
                catch(MySqlException exm)
                {
                    MessageBox.Show("Usuário já existe!!!");
                    //insert na tabela log - exm.Message
                    Geral.InserirLog(exm.Message, Geral.id_usuario);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Geral.InserirLog(ex.Message, Geral.id_usuario);
                }
            }
            else
            {
                MessageBox.Show("Preencha todos os campos!!!", "Erro ao cadastrar",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }         

        }

        // Atualiza os dados de um usuário no BD
        private void button2_Click(object sender, EventArgs e)
        {
            var dialogRes = MessageBox.Show("Deseja atualizar este usuário?", "Atualizar usuário",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogRes == DialogResult.Yes)
            {
                try
                {
                    if (id > 0)
                    {
                        string sql = $"UPDATE tab_usuarios SET usuario=@usuario, ";
                        if (txtSenha.Text.Length > 0)
                        {
                            sql += $"senha={txtSenha.Text}, ";
                        }
                        sql += $"nivel_id=@nivel_id WHERE id =@id";

                        Conexao.Conectar();
                        MySqlCommand cmd = new MySqlCommand(sql, Conexao.conn);
                        cmd.Parameters.AddWithValue("usuario", txtUsuario.Text);
                        //cmd.Parameters.AddWithValue("senha", txtSenha.Text);
                        cmd.Parameters.AddWithValue("nivel_id", cbNivel.SelectedValue);
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.ExecuteNonQuery();
                        Resetar();
                        Conexao.Desconectar();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // Deleta um usuário do BD
        private void button3_Click(object sender, EventArgs e)
        {
            var dialogResult = MessageBox.Show("Deseja excluir este registro?"
                , "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    Conexao.Conectar();

                    string sql = $"DELETE FROM tab_usuarios WHERE id='{id}'";
                    
                    MySqlCommand cmd = new MySqlCommand(sql, Conexao.conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Usuário foi excluido!!!");

                    Resetar();

                    Conexao.Desconectar();
                }
                catch(MySqlException exm)
                {
                    MessageBox.Show("Erro ao excluir usuário");
                    Geral.InserirLog(exm.Message, Geral.id_usuario);
                }
                catch(Exception ex) 
                {
                    MessageBox.Show("Erro ao excluir usuário");
                    Geral.InserirLog(ex.Message, Geral.id_usuario);
                }
            }
        }

        // Seleciona os dados da row clicada nos campos txt e cb
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            txtUsuario.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();   
            cbNivel.SelectedValue = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            if(id > 0)
            {
                btnAtualizar.Enabled = true;
                btnExcluir.Enabled = true;
                btnCadastrar.Enabled = false;
            }
        }

        // Faz a busca por usuários na tabela ao digitar na caixa de pesquisa
        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {      
            try
            {
                dataGridView1.DataSource = Geral.Selecionar($"select u.id, u.usuario,u.dt_user,u.nivel_id,n.nivel\r\nfrom tab_usuarios as u \r\ninner join tab_nivel as n on u.nivel_id = n.id\r\nWHERE usuario LIKE '%{txtPesquisa.Text}%'");
            }
            catch(MySqlException exm)
            {
                MessageBox.Show("Erro ao tentar efetuar pesquisa");
                Geral.InserirLog(exm.Message, Geral.id_usuario);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao tentar efetuar pesquisa");
                Geral.InserirLog(ex.Message, Geral.id_usuario);
            }
        }

        // Exemplo de tratamento de data
        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            //DateTime data01 = DateTime.Parse(dateTimePicker1.Text);//  20/05/2024  19:53
            //DateTime data02 = new DateTime(2024, 5, 21, 20, 53, 0);//  21/05/2024  20:53
            //data02.AddDays(1);
            //TimeSpan diff = data02 - data01;

            //MessageBox.Show(diff.Hours.ToString());
            //MessageBox.Show(diff.Days.ToString());
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            
        }

        private void FrmUsuarios_Shown(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }
    }
}
