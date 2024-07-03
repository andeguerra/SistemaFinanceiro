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

namespace Sistema_Financeiro
{
    public partial class FrmControleFn : Form
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


        int id = 0;
        public FrmControleFn()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }

        // Função para buscar os dados de 'Receita'
        public void SelecionarDados()
        {
            try
            {
                string sql = $"SELECT tf.id, tf.valor, tf.data, tf.categoria_id, tc.categoria " +
                    $"FROM tab_financas as tf " +
                    $"inner join tab_categorias as tc on tf.categoria_id = tc.id " +
                    $"WHERE tf.usuario_id='{Geral.id_usuario}' and tc.categoria = 'Receita'";
                dgvReceita.DataSource = Geral.Selecionar(sql);
                
            }
            catch (MySqlException exm)
            {
                Geral.InserirLog(exm.Message, Geral.id_usuario);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Geral.InserirLog(ex.Message, Geral.id_usuario);
            }
        }


        // Função para buscar os dados de 'Despesa'
        public void SelecionarDados2()
        {
            try
            {
                string sql = $"SELECT tf.id, tf.valor, tf.data, tf.categoria_id, tc.categoria " +
                    $"FROM tab_financas as tf " +
                    $"inner join tab_categorias as tc on tf.categoria_id = tc.id " +
                    $"WHERE tf.usuario_id='{Geral.id_usuario}' and tc.categoria = 'Despesa'";
                dgvDespesa.DataSource = Geral.Selecionar(sql);
                
            }
            catch (MySqlException exm)
            {
                MessageBox.Show("Erro ao tentar selecionar os dados da tabela de despesas");
                Geral.InserirLog(exm.Message, Geral.id_usuario);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao tentar selecionar os dados da tabela de despesas");
                Geral.InserirLog(ex.Message, Geral.id_usuario);
            }
        }

        public void LimparCampos()
        {
            txtValor.Clear();
            txtPesquisa.Clear();
            cbCategoria.SelectedIndex = 0;
            btnCadastrar.Enabled = true;
            btnAtualizar.Enabled = false;
            btnExcluir.Enabled = false;
            dtpData.Value = DateTime.Now;
            dtpDe.Value = DateTime.Now;
            dtpAte.Value = DateTime.Now;
        }

        // Função apra mostrar saldo
        public void CarregarSaldo()
        {
            string sql = $"SELECT SUM(CASE WHEN tf.categoria_id = 1 THEN tf.valor ELSE 0 END) AS total_receita, " +
                $"SUM(CASE WHEN tf.categoria_id = 2 THEN tf.valor ELSE 0 END) AS total_despesas " +
                $"FROM tab_financas AS tf INNER JOIN tab_usuarios AS tu ON tf.usuario_id = tu.id WHERE tu.id = '{Geral.id_usuario}';";
            Conexao.Conectar();
            MySqlCommand cmd = new MySqlCommand(sql, Conexao.conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            // Teste para verificar o que estava retornando
            //bool teste = dt.Rows[0]["total_receita"].ToString() == "" ? true : false;
            //MessageBox.Show(teste.ToString());

            if (dt.Rows[0]["total_receita"].ToString() == "" && dt.Rows[0]["total_despesas"].ToString() == "")
            {
                btnSaldo.Text = "R$ 0,00";
            }
            else
            {
                double receita = double.Parse(dt.Rows[0]["total_receita"].ToString());
                double despesa = double.Parse(dt.Rows[0]["total_despesas"].ToString());
                double saldo = receita - despesa;
                btnSaldo.Text = saldo.ToString("C");
            }
        }

        // Mostra nas tabelas os dados ao iniciar o formulário e carrega os dados da cb de forma dinâmica
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT * FROM tab_categorias";
                DataTable dt = Geral.Selecionar(sql);
                cbCategoria.DataSource = dt;
                cbCategoria.DisplayMember = "categoria";
                cbCategoria.ValueMember = "id";
                SelecionarDados();
                SelecionarDados2();
                CarregarSaldo();

                dgvReceita.Columns[0].Visible = false;
                dgvReceita.Columns[3].Visible = false;
                dgvDespesa.Columns[0].Visible = false;
                dgvDespesa.Columns[3].Visible = false;
            }
            catch(MySqlException exm)
            {
                MessageBox.Show("Erro ao carregar dados da tabela");
                MessageBox.Show(exm.Message);
                Geral.InserirLog(exm.Message, Geral.id_usuario);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados da tabela");
                MessageBox.Show(ex.Message);
                Geral.InserirLog(ex.Message, Geral.id_usuario);
            }

        }

        // Insere um dado de receita/despesa no BD
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Conexao.Conectar();

                double v = double.Parse(txtValor.Text);
                int c = int.Parse(cbCategoria.SelectedValue.ToString());
                DateTime data = dtpData.Value;

                string sql = "insert into tab_financas(valor,data,categoria_id,usuario_id) " +
                    "values(@valor,@data,@categoria_id,@usuario_id)";

                MySqlCommand cmd = new MySqlCommand(sql, Conexao.conn);
                cmd.Parameters.AddWithValue("valor", v);
                cmd.Parameters.AddWithValue("categoria_id", c);
                cmd.Parameters.AddWithValue("data", data);
                cmd.Parameters.AddWithValue("usuario_id", Geral.id_usuario);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Cadastro efetuado com sucesso!!!");

                LimparCampos();
                SelecionarDados();
                SelecionarDados2();
                CarregarSaldo();

                Conexao.Desconectar();
            }
            catch(MySqlException exm)
            {
                MessageBox.Show("Erro ao tentar salvar os dados");
                Geral.InserirLog(exm.Message, Geral.id_usuario);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao tentar salvar os dados");
                Geral.InserirLog(ex.Message, Geral.id_usuario);
            }
            
        }

        // Atualiza uma informação da tabela de finanças
        private void button2_Click(object sender, EventArgs e)
        {
            var dialogRes = MessageBox.Show("Deseja atualizar essas informações?",
                "Atualizar Dados",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dialogRes == DialogResult.Yes)
            {
                try
                {
                    Conexao.Conectar();

                    double valor = double.Parse(txtValor.Text);
                    int categoria = int.Parse(cbCategoria.SelectedValue.ToString());
                    DateTime data = dtpData.Value;

                    string sql = "UPDATE tab_financas " +
                        "SET valor=@valor,data=@data,categoria_id=@categoria_id " +
                        "WHERE id=@id";

                    MySqlCommand cmd = new MySqlCommand(sql, Conexao.conn);
                    cmd.Parameters.AddWithValue("valor", valor);
                    cmd.Parameters.AddWithValue("data", data);
                    cmd.Parameters.AddWithValue("categoria_id", categoria);
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Atualização efetuada com sucesso!");

                    LimparCampos();
                    SelecionarDados();
                    SelecionarDados2();
                    CarregarSaldo();

                    Conexao.Desconectar();
                }
                catch (MySqlException exm)
                {
                    MessageBox.Show("Erro ao tentar atualizar");
                    Geral.InserirLog(exm.Message, Geral.id_usuario);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao tentar atualizar");
                    Geral.InserirLog(ex.Message, Geral.id_usuario);
                }
            }       
        }

        // Exclui um informação da tabela de finanças
        private void button3_Click(object sender, EventArgs e)
        {
            var dialogRes = MessageBox.Show("Deseja excluir os dados?",
                "Excluir Dados",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dialogRes == DialogResult.Yes)
            {
                try
                {
                    Conexao.Conectar();

                    string sql = "DELETE FROM tab_financas WHERE id=@id";
                    MySqlCommand cmd = new MySqlCommand(sql, Conexao.conn);
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Item excluido com sucesso!!!");

                    SelecionarDados();
                    SelecionarDados2();
                    LimparCampos();
                    CarregarSaldo();

                    Conexao.Desconectar();
                }
                catch(MySqlException exm)
                {
                    MessageBox.Show("Erro ao tentar excluir");
                    Geral.InserirLog(exm.Message, Geral.id_usuario);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao tentar excluir");
                    Geral.InserirLog(ex.Message, Geral.id_usuario);
                }
            }
        }

        // Mostra os dados da dgv nos txt e libera botões de atualizar e excluir
        private void dgvReceita_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            id = int.Parse(dgvReceita.CurrentRow.Cells[0].Value.ToString());
            txtValor.Text = dgvReceita.CurrentRow.Cells[1].Value.ToString();
            cbCategoria.SelectedValue = dgvReceita.CurrentRow.Cells[3].Value.ToString();
            btnAtualizar.Enabled = true;
            btnExcluir.Enabled = true;
            btnCadastrar.Enabled = false;
        }

        // Mostra os dados da dgv nos txt e libera botões de atualizar e excluir
        private void dgvDespesa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            id = int.Parse(dgvDespesa.CurrentRow.Cells[0].Value.ToString());
            txtValor.Text = dgvDespesa.CurrentRow.Cells[1].Value.ToString();
            cbCategoria.SelectedValue = dgvDespesa.CurrentRow.Cells[3].Value.ToString();
            btnAtualizar.Enabled = true;
            btnExcluir.Enabled = true;
            btnCadastrar.Enabled = false;
        }

        // Filtro de pesquisa por escrita
        private void txtPesquisa_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                string valor = txtPesquisa.Text;
                string sql = $"SELECT tf.id, tf.valor, tf.data, tf.categoria_id, tc.categoria FROM tab_financas as tf inner join tab_categorias as tc on tf.categoria_id = tc.id " +
                    $"WHERE tf.valor LIKE '%{valor}%' and tf.categoria_id = 1";
                string sql2 = $"SELECT tf.id, tf.valor, tf.data, tf.categoria_id, tc.categoria FROM tab_financas as tf inner join tab_categorias as tc on tf.categoria_id = tc.id " +
                    $"WHERE tf.valor LIKE '%{valor}%' and tf.categoria_id = 2";
                dgvReceita.DataSource = Geral.Selecionar(sql);
                dgvDespesa.DataSource = Geral.Selecionar(sql2);
            }
            catch (MySqlException exm)
            {
                MessageBox.Show("Erro na pesquisa dos dados");
                Geral.InserirLog(exm.Message, Geral.id_usuario);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na pesquisa dos dados");
                Geral.InserirLog(ex.Message, Geral.id_usuario);
            }
        }

        // Filtro de pesquisa por data
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                string sql = $"SELECT tf.id, tf.valor, tf.data, tf.categoria_id, tc.categoria " +
                    $"FROM tab_financas as tf inner join tab_categorias as tc on tf.categoria_id = tc.id " +
                    $"WHERE tf.usuario_id={Geral.id_usuario} and tc.categoria = 'Receita' and tf.data BETWEEN '{dtpDe.Value.ToString("yyyy-MM-dd")}' and '{dtpAte.Value.ToString("yyyy-mm-dd")}'";
                string sql2 = $"SELECT tf.id, tf.valor, tf.data, tf.categoria_id, tc.categoria " +
                    $"FROM tab_financas as tf inner join tab_categorias as tc on tf.categoria_id = tc.id " +
                    $"WHERE tf.usuario_id={Geral.id_usuario} and tc.categoria = 'Despesa' and tf.data BETWEEN '{dtpDe.Value.ToString("yyyy-MM-dd")}' and '{dtpAte.Value.ToString("yyyy-mm-dd")}'";

                dgvReceita.DataSource = Geral.Selecionar(sql);
                dgvDespesa.DataSource = Geral.Selecionar(sql2);
                
            }
            catch (MySqlException exm)
            {
                MessageBox.Show("Erro ao tentar efetuar o filtro de pesquisa" + exm.Message);
                Geral.InserirLog(exm.Message, Geral.id_usuario);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao tentar efetuar o filtro de pesquisa");
                Geral.InserirLog(ex.Message, Geral.id_usuario);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            SelecionarDados();
            SelecionarDados2();
            LimparCampos();
            CarregarSaldo();

            dgvReceita.Columns[0].Visible = false;
            dgvReceita.Columns[3].Visible = false;
            dgvDespesa.Columns[0].Visible = false;
            dgvDespesa.Columns[3].Visible = false;
        }
    }
}
