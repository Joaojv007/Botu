﻿// <auto-generated />
using System;
using ApiTcc.Infra.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infra.Migrations
{
    [DbContext(typeof(BotuContext))]
    partial class BotuContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Aluno", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Alunos");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Avaliacao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Conteudo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DataEntrega")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("DisciplinaId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Nota")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("TipoTarefa")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DisciplinaId");

                    b.ToTable("Avaliacoes");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Curso", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("FaculdadeId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsCursando")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("FaculdadeId");

                    b.ToTable("Cursos");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.CursoAluno", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AlunoId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("FaculdadeId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("AlunoId");

                    b.HasIndex("FaculdadeId");

                    b.ToTable("CursoAluno");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Disciplina", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Aulas")
                        .HasColumnType("int");

                    b.Property<int>("Faltas")
                        .HasColumnType("int");

                    b.Property<double>("Frequencia")
                        .HasColumnType("double");

                    b.Property<decimal>("Media")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Professor")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Resultado")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("SemestreId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("SemestreId");

                    b.ToTable("Disciplinas");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Faculdade", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Faculdades");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.FaculdadeAluno", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AlunoId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("FaculdadeId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("AlunoId");

                    b.HasIndex("FaculdadeId");

                    b.ToTable("FaculdadeAluno");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Integracao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AlunoId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("CapturouSemestresPassados")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("DataIntegracao")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Erro")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ErroDescricao")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("FaculdadeId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TipoIntegracao")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AlunoId");

                    b.HasIndex("FaculdadeId");

                    b.ToTable("Integracoes");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Semestre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AlunoId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CursoId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DataFinal")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DataInicio")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CursoId");

                    b.ToTable("Semestres");
                });

            modelBuilder.Entity("Infra.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AlunoId")
                        .HasColumnType("char(36)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ResetPasswordToken")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ResetPasswordTokenExpiry")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Avaliacao", b =>
                {
                    b.HasOne("ApiTcc.Infra.DB.Entities.Disciplina", "Disciplina")
                        .WithMany("Avaliacoes")
                        .HasForeignKey("DisciplinaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Disciplina");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Curso", b =>
                {
                    b.HasOne("ApiTcc.Infra.DB.Entities.Faculdade", null)
                        .WithMany("Cursos")
                        .HasForeignKey("FaculdadeId");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.CursoAluno", b =>
                {
                    b.HasOne("ApiTcc.Infra.DB.Entities.Aluno", "Aluno")
                        .WithMany()
                        .HasForeignKey("AlunoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiTcc.Infra.DB.Entities.Curso", "Faculdade")
                        .WithMany()
                        .HasForeignKey("FaculdadeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aluno");

                    b.Navigation("Faculdade");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Disciplina", b =>
                {
                    b.HasOne("ApiTcc.Infra.DB.Entities.Semestre", "Semestre")
                        .WithMany("Disciplinas")
                        .HasForeignKey("SemestreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Semestre");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.FaculdadeAluno", b =>
                {
                    b.HasOne("ApiTcc.Infra.DB.Entities.Aluno", "Aluno")
                        .WithMany("Faculdade")
                        .HasForeignKey("AlunoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiTcc.Infra.DB.Entities.Faculdade", "Faculdade")
                        .WithMany()
                        .HasForeignKey("FaculdadeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aluno");

                    b.Navigation("Faculdade");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Integracao", b =>
                {
                    b.HasOne("ApiTcc.Infra.DB.Entities.Aluno", "Aluno")
                        .WithMany("Integracoes")
                        .HasForeignKey("AlunoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiTcc.Infra.DB.Entities.Faculdade", "Faculdade")
                        .WithMany()
                        .HasForeignKey("FaculdadeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aluno");

                    b.Navigation("Faculdade");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Semestre", b =>
                {
                    b.HasOne("ApiTcc.Infra.DB.Entities.Curso", null)
                        .WithMany("Semestres")
                        .HasForeignKey("CursoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Aluno", b =>
                {
                    b.Navigation("Faculdade");

                    b.Navigation("Integracoes");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Curso", b =>
                {
                    b.Navigation("Semestres");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Disciplina", b =>
                {
                    b.Navigation("Avaliacoes");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Faculdade", b =>
                {
                    b.Navigation("Cursos");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Semestre", b =>
                {
                    b.Navigation("Disciplinas");
                });
#pragma warning restore 612, 618
        }
    }
}
