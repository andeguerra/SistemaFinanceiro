-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Tempo de geração: 20/05/2024 às 21:20
-- Versão do servidor: 10.4.32-MariaDB
-- Versão do PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Banco de dados: `dbfn`
--

-- --------------------------------------------------------

--
-- Estrutura para tabela `tab_categorias`
--

CREATE TABLE `tab_categorias` (
  `id` int(11) NOT NULL,
  `categoria` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `tab_categorias`
--

INSERT INTO `tab_categorias` (`id`, `categoria`) VALUES
(1, 'Receita'),
(2, 'Despesa');

-- --------------------------------------------------------

--
-- Estrutura para tabela `tab_financas`
--

CREATE TABLE `tab_financas` (
  `id` int(11) NOT NULL,
  `valor` float NOT NULL,
  `categoria_id` int(11) NOT NULL,
  `usuario_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `tab_financas`
--

INSERT INTO `tab_financas` (`id`, `valor`, `categoria_id`, `usuario_id`) VALUES
(23, 444, 1, 12),
(24, 200, 2, 12),
(25, 33, 1, 12),
(26, 2222, 2, 12),
(27, 3344, 1, 12);

-- --------------------------------------------------------

--
-- Estrutura para tabela `tab_logs`
--

CREATE TABLE `tab_logs` (
  `id` int(11) NOT NULL,
  `msg_erro` varchar(255) NOT NULL,
  `usuario_id` int(11) NOT NULL,
  `dt_erro` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `tab_logs`
--

INSERT INTO `tab_logs` (`id`, `msg_erro`, `usuario_id`, `dt_erro`) VALUES
(1, 'Table \'dbfn.tab_usuario\' doesn\'t exist', 12, '2024-05-15 19:34:54'),
(2, 'Table \'dbfn.tab_usuario\' doesn\'t exist', 12, '2024-05-15 19:58:59');

-- --------------------------------------------------------

--
-- Estrutura para tabela `tab_nivel`
--

CREATE TABLE `tab_nivel` (
  `id` int(11) NOT NULL,
  `nivel` varchar(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `tab_nivel`
--

INSERT INTO `tab_nivel` (`id`, `nivel`) VALUES
(1, 'ADM'),
(2, 'USUÁRIO');

-- --------------------------------------------------------

--
-- Estrutura para tabela `tab_usuarios`
--

CREATE TABLE `tab_usuarios` (
  `id` int(11) NOT NULL,
  `usuario` varchar(30) NOT NULL,
  `senha` varchar(15) NOT NULL,
  `nivel_id` int(11) NOT NULL,
  `dt_user` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `tab_usuarios`
--

INSERT INTO `tab_usuarios` (`id`, `usuario`, `senha`, `nivel_id`, `dt_user`) VALUES
(12, 'adm', '123', 1, '2024-05-13 20:01:32'),
(16, 'ana', '123', 2, '2024-05-15 20:01:47');

--
-- Índices para tabelas despejadas
--

--
-- Índices de tabela `tab_categorias`
--
ALTER TABLE `tab_categorias`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `tab_financas`
--
ALTER TABLE `tab_financas`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_categoria` (`categoria_id`),
  ADD KEY `fk_usuarios` (`usuario_id`);

--
-- Índices de tabela `tab_logs`
--
ALTER TABLE `tab_logs`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_usuario_log` (`usuario_id`);

--
-- Índices de tabela `tab_nivel`
--
ALTER TABLE `tab_nivel`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `tab_usuarios`
--
ALTER TABLE `tab_usuarios`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `usuario` (`usuario`),
  ADD KEY `fk_nivel` (`nivel_id`);

--
-- AUTO_INCREMENT para tabelas despejadas
--

--
-- AUTO_INCREMENT de tabela `tab_categorias`
--
ALTER TABLE `tab_categorias`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de tabela `tab_financas`
--
ALTER TABLE `tab_financas`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT de tabela `tab_logs`
--
ALTER TABLE `tab_logs`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de tabela `tab_nivel`
--
ALTER TABLE `tab_nivel`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de tabela `tab_usuarios`
--
ALTER TABLE `tab_usuarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- Restrições para tabelas despejadas
--

--
-- Restrições para tabelas `tab_financas`
--
ALTER TABLE `tab_financas`
  ADD CONSTRAINT `fk_categoria` FOREIGN KEY (`categoria_id`) REFERENCES `tab_categorias` (`id`),
  ADD CONSTRAINT `fk_usuarios` FOREIGN KEY (`usuario_id`) REFERENCES `tab_usuarios` (`id`);

--
-- Restrições para tabelas `tab_logs`
--
ALTER TABLE `tab_logs`
  ADD CONSTRAINT `fk_usuario_log` FOREIGN KEY (`usuario_id`) REFERENCES `tab_usuarios` (`id`);

--
-- Restrições para tabelas `tab_usuarios`
--
ALTER TABLE `tab_usuarios`
  ADD CONSTRAINT `fk_nivel` FOREIGN KEY (`nivel_id`) REFERENCES `tab_nivel` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
