-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gazdă: 127.0.0.1
-- Timp de generare: mart. 01, 2026 la 04:16 PM
-- Versiune server: 10.4.32-MariaDB
-- Versiune PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Bază de date: `market`
--

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `cat`
--

CREATE TABLE `cat` (
  `id` int(11) NOT NULL,
  `nume` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Eliminarea datelor din tabel `cat`
--

INSERT INTO `cat` (`id`, `nume`) VALUES
(1, 'Fructe'),
(2, 'Legume'),
(3, 'Verdeturi'),
(4, 'Bauturi alcoolice'),
(5, 'Bauturi nealcoolice'),
(6, 'Apa plata'),
(7, 'Ingrijire par'),
(8, 'Lavete Burete'),
(9, 'Ingrijire corporala');

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `comenzi`
--

CREATE TABLE `comenzi` (
  `id` int(11) NOT NULL,
  `nume` varchar(100) NOT NULL,
  `prenume` varchar(100) NOT NULL,
  `telefon` varchar(20) NOT NULL,
  `email` varchar(150) NOT NULL,
  `id_prod` int(11) NOT NULL,
  `cantitate` int(11) NOT NULL,
  `data_cmd` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Eliminarea datelor din tabel `comenzi`
--

INSERT INTO `comenzi` (`id`, `nume`, `prenume`, `telefon`, `email`, `id_prod`, `cantitate`, `data_cmd`) VALUES
(1, 'Gilca', 'Cristina', '37367200699', 'cleoc2058@gmail.com', 33, 2, '2026-03-01 15:15:01'),
(2, 'Cristina', 'Gilca', '0672007888', 'djsdhsbd@gmail.com', 24, 2, '2026-03-01 15:30:06');

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `prod`
--

CREATE TABLE `prod` (
  `id` int(11) NOT NULL,
  `id_cat` int(11) NOT NULL,
  `nume` varchar(150) NOT NULL,
  `pret` decimal(10,2) NOT NULL,
  `stoc` int(11) NOT NULL DEFAULT 0,
  `um` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Eliminarea datelor din tabel `prod`
--

INSERT INTO `prod` (`id`, `id_cat`, `nume`, `pret`, `stoc`, `um`) VALUES
(1, 1, 'Mere', 8.50, 120, 'kg'),
(2, 1, 'Banane', 9.99, 80, 'kg'),
(3, 1, 'Portocale', 7.20, 95, 'kg'),
(4, 1, 'Capsuni', 22.00, 40, 'kg'),
(5, 1, 'Struguri', 18.00, 55, 'kg'),
(6, 2, 'Rosii', 12.50, 60, 'kg'),
(7, 2, 'Castraveti', 5.00, 75, 'kg'),
(8, 2, 'Morcovi', 4.50, 100, 'kg'),
(9, 2, 'Ardei rosu', 11.00, 50, 'kg'),
(10, 2, 'Cartofi', 6.00, 200, 'kg'),
(11, 2, 'Ceapa', 4.00, 180, 'kg'),
(12, 3, 'Patrunjel', 2.50, 200, 'buc'),
(13, 3, 'Marar', 2.50, 180, 'buc'),
(14, 3, 'Salata verde', 4.00, 90, 'buc'),
(15, 3, 'Ceapa verde', 3.00, 110, 'buc'),
(16, 4, 'Vin rosu', 45.00, 30, 'sticla'),
(17, 4, 'Bere 0.5L', 7.50, 144, 'buc'),
(18, 4, 'Sampanie', 89.00, 15, 'sticla'),
(19, 5, 'Cola 2L', 11.99, 60, 'buc'),
(20, 5, 'Suc portocale 1L', 14.50, 48, 'buc'),
(21, 5, 'Ceai verde 400ml', 9.00, 72, 'buc'),
(22, 5, 'Fanta 2L', 11.99, 55, 'buc'),
(23, 6, 'Apa 0.5L', 3.50, 240, 'buc'),
(24, 6, 'Apa 2L', 7.00, 120, 'buc'),
(25, 6, 'Apa 5L', 14.00, 60, 'buc'),
(26, 7, 'Sampon Pantene', 28.00, 35, 'buc'),
(27, 7, 'Balsam par', 24.00, 28, 'buc'),
(28, 7, 'Vopsea par', 55.00, 20, 'buc'),
(29, 8, 'Burete vase', 3.00, 150, 'buc'),
(30, 8, 'Laveta microfibra', 8.00, 80, 'buc'),
(31, 8, 'Mop', 35.00, 25, 'buc'),
(32, 9, 'Gel dus Nivea', 19.50, 55, 'buc'),
(33, 9, 'Crema maini', 15.00, 40, 'buc'),
(34, 9, 'Sapun solid', 6.00, 90, 'buc');

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `nume` varchar(100) NOT NULL,
  `prenume` varchar(100) NOT NULL,
  `username` varchar(80) NOT NULL,
  `parola` varchar(255) NOT NULL,
  `role` varchar(20) NOT NULL DEFAULT 'user',
  `email` varchar(150) NOT NULL,
  `phone` varchar(20) DEFAULT NULL,
  `data_add` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Eliminarea datelor din tabel `users`
--

INSERT INTO `users` (`id`, `nume`, `prenume`, `username`, `parola`, `role`, `email`, `phone`, `data_add`) VALUES
(1, 'Celestia', 'Admin', 'Celestia', 'celestia', 'admin', 'cleo@gmail.com', '069000000', '2026-03-01 17:15:15');

--
-- Indexuri pentru tabele eliminate
--

--
-- Indexuri pentru tabele `cat`
--
ALTER TABLE `cat`
  ADD PRIMARY KEY (`id`);

--
-- Indexuri pentru tabele `comenzi`
--
ALTER TABLE `comenzi`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_prod` (`id_prod`);

--
-- Indexuri pentru tabele `prod`
--
ALTER TABLE `prod`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_cat` (`id_cat`);

--
-- Indexuri pentru tabele `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`),
  ADD UNIQUE KEY `email` (`email`);

--
-- AUTO_INCREMENT pentru tabele eliminate
--

--
-- AUTO_INCREMENT pentru tabele `cat`
--
ALTER TABLE `cat`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT pentru tabele `comenzi`
--
ALTER TABLE `comenzi`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT pentru tabele `prod`
--
ALTER TABLE `prod`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=35;

--
-- AUTO_INCREMENT pentru tabele `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Constrângeri pentru tabele eliminate
--

--
-- Constrângeri pentru tabele `comenzi`
--
ALTER TABLE `comenzi`
  ADD CONSTRAINT `comenzi_ibfk_1` FOREIGN KEY (`id_prod`) REFERENCES `prod` (`id`);

--
-- Constrângeri pentru tabele `prod`
--
ALTER TABLE `prod`
  ADD CONSTRAINT `prod_ibfk_1` FOREIGN KEY (`id_cat`) REFERENCES `cat` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
