-- phpMyAdmin SQL Dump
-- version 4.1.14
-- http://www.phpmyadmin.net
--
-- Client :  127.0.0.1
-- Généré le :  Mar 31 Mars 2015 à 16:53
-- Version du serveur :  5.6.17
-- Version de PHP :  5.5.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Base de données :  `bdd_service_web`
--

DELIMITER $$
--
-- Procédures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `AjouterQuestion`(IN `p_question_contenu` VARCHAR(500))
    COMMENT 'Ajoute une nouvelle question'
BEGIN
	INSERT INTO questions (question_contenu) VALUES (p_question_contenu);
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `AjouterReponse`(IN `p_reponse_contenu` VARCHAR(500), IN `p_question_fid` INT)
BEGIN
	INSERT INTO reponses (reponse_contenu, question_fid) VALUES (p_reponse_contenu, p_question_fid);
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `questions`
--

CREATE TABLE IF NOT EXISTS `questions` (
  `question_id` int(11) NOT NULL AUTO_INCREMENT,
  `question_contenu` varchar(500) NOT NULL,
  PRIMARY KEY (`question_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=3 ;

--
-- Contenu de la table `questions`
--

INSERT INTO `questions` (`question_id`, `question_contenu`) VALUES
(1, 'Comment installe-on windows XP 32 bits ?'),
(2, '');

-- --------------------------------------------------------

--
-- Structure de la table `reponses`
--

CREATE TABLE IF NOT EXISTS `reponses` (
  `reponse_id` int(11) NOT NULL AUTO_INCREMENT,
  `reponse_contenu` varchar(500) NOT NULL,
  `question_fid` int(11) NOT NULL,
  PRIMARY KEY (`reponse_id`),
  KEY `question_fid` (`question_fid`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=2 ;

--
-- Contenu de la table `reponses`
--

INSERT INTO `reponses` (`reponse_id`, `reponse_contenu`, `question_fid`) VALUES
(1, 'Tu te débrouilles....', 1);

--
-- Contraintes pour les tables exportées
--

--
-- Contraintes pour la table `reponses`
--
ALTER TABLE `reponses`
  ADD CONSTRAINT `FK_REPONSE_QUESTION` FOREIGN KEY (`question_fid`) REFERENCES `questions` (`question_id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
