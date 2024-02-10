CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    CREATE TABLE `Alunos` (
        `Id` char(36) COLLATE ascii_general_ci NOT NULL,
        `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
        `DataNascimento` datetime(6) NOT NULL,
        CONSTRAINT `PK_Alunos` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    CREATE TABLE `Faculdades` (
        `Id` char(36) COLLATE ascii_general_ci NOT NULL,
        `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
        `AlunoId` char(36) COLLATE ascii_general_ci NULL,
        CONSTRAINT `PK_Faculdades` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Faculdades_Alunos_AlunoId` FOREIGN KEY (`AlunoId`) REFERENCES `Alunos` (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    CREATE TABLE `Cursos` (
        `Id` char(36) COLLATE ascii_general_ci NOT NULL,
        `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
        `FaculdadeId` char(36) COLLATE ascii_general_ci NULL,
        CONSTRAINT `PK_Cursos` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Cursos_Faculdades_FaculdadeId` FOREIGN KEY (`FaculdadeId`) REFERENCES `Faculdades` (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    CREATE TABLE `Integracoes` (
        `Id` char(36) COLLATE ascii_general_ci NOT NULL,
        `AlunoId` char(36) COLLATE ascii_general_ci NOT NULL,
        `FaculdadeId` char(36) COLLATE ascii_general_ci NOT NULL,
        `TipoIntegracao` int NOT NULL,
        `Login` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Senha` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ErroDescricao` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Erro` tinyint(1) NOT NULL,
        `DataIntegracao` datetime(6) NOT NULL,
        CONSTRAINT `PK_Integracoes` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Integracoes_Alunos_AlunoId` FOREIGN KEY (`AlunoId`) REFERENCES `Alunos` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_Integracoes_Faculdades_FaculdadeId` FOREIGN KEY (`FaculdadeId`) REFERENCES `Faculdades` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    CREATE TABLE `Semestres` (
        `Id` char(36) COLLATE ascii_general_ci NOT NULL,
        `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
        `DataInicio` datetime(6) NOT NULL,
        `DataFinal` datetime(6) NOT NULL,
        `CursoId` char(36) COLLATE ascii_general_ci NULL,
        CONSTRAINT `PK_Semestres` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Semestres_Cursos_CursoId` FOREIGN KEY (`CursoId`) REFERENCES `Cursos` (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    CREATE TABLE `Disciplinas` (
        `Id` char(36) COLLATE ascii_general_ci NOT NULL,
        `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Professor` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Frequencia` int NOT NULL,
        `Faltas` int NOT NULL,
        `Aulas` int NOT NULL,
        `Resultado` int NOT NULL,
        `SemestreId` char(36) COLLATE ascii_general_ci NULL,
        CONSTRAINT `PK_Disciplinas` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Disciplinas_Semestres_SemestreId` FOREIGN KEY (`SemestreId`) REFERENCES `Semestres` (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    CREATE TABLE `Avaliacoes` (
        `Id` char(36) COLLATE ascii_general_ci NOT NULL,
        `Nome` longtext CHARACTER SET utf8mb4 NOT NULL,
        `DataEntrega` datetime(6) NOT NULL,
        `Conteudo` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Nota` decimal(65,30) NOT NULL,
        `TipoTarefa` int NOT NULL,
        `DisciplinaId` char(36) COLLATE ascii_general_ci NULL,
        CONSTRAINT `PK_Avaliacoes` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Avaliacoes_Disciplinas_DisciplinaId` FOREIGN KEY (`DisciplinaId`) REFERENCES `Disciplinas` (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    CREATE INDEX `IX_Avaliacoes_DisciplinaId` ON `Avaliacoes` (`DisciplinaId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    CREATE INDEX `IX_Cursos_FaculdadeId` ON `Cursos` (`FaculdadeId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    CREATE INDEX `IX_Disciplinas_SemestreId` ON `Disciplinas` (`SemestreId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    CREATE INDEX `IX_Faculdades_AlunoId` ON `Faculdades` (`AlunoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    CREATE INDEX `IX_Integracoes_AlunoId` ON `Integracoes` (`AlunoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    CREATE INDEX `IX_Integracoes_FaculdadeId` ON `Integracoes` (`FaculdadeId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    CREATE INDEX `IX_Semestres_CursoId` ON `Semestres` (`CursoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240128023150_InicioDataBase') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20240128023150_InicioDataBase', '7.0.15');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240208225419_AjusteTabelaFaculdade') THEN

    ALTER TABLE `Disciplinas` ADD `Media` decimal(65,30) NOT NULL DEFAULT 0.0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240208225419_AjusteTabelaFaculdade') THEN

    CREATE TABLE `FaculdadeAluno` (
        `Id` char(36) COLLATE ascii_general_ci NOT NULL,
        `FaculdadeId` char(36) COLLATE ascii_general_ci NOT NULL,
        `AlunoId` char(36) COLLATE ascii_general_ci NOT NULL,
        CONSTRAINT `PK_FaculdadeAluno` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_FaculdadeAluno_Alunos_AlunoId` FOREIGN KEY (`AlunoId`) REFERENCES `Alunos` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_FaculdadeAluno_Faculdades_FaculdadeId` FOREIGN KEY (`FaculdadeId`) REFERENCES `Faculdades` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240208225419_AjusteTabelaFaculdade') THEN

    CREATE INDEX `IX_FaculdadeAluno_AlunoId` ON `FaculdadeAluno` (`AlunoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240208225419_AjusteTabelaFaculdade') THEN

    CREATE INDEX `IX_FaculdadeAluno_FaculdadeId` ON `FaculdadeAluno` (`FaculdadeId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240208225419_AjusteTabelaFaculdade') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20240208225419_AjusteTabelaFaculdade', '7.0.15');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

