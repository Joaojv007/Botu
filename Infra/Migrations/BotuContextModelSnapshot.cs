﻿// <auto-generated />
using System;
using ApiTcc.Infra.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApiTcc.Migrations
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

                    b.Property<Guid?>("DisciplinaId")
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

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("FaculdadeId");

                    b.ToTable("Cursos");
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

                    b.Property<int>("Frequencia")
                        .HasColumnType("int");

                    b.Property<decimal>("Media")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Professor")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Resultado")
                        .HasColumnType("int");

                    b.Property<Guid?>("SemestreId")
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

                    b.Property<Guid?>("AlunoId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AlunoId");

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

                    b.Property<Guid?>("CursoId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("DataFinal")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DataInicio")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CursoId");

                    b.ToTable("Semestres");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Avaliacao", b =>
                {
                    b.HasOne("ApiTcc.Infra.DB.Entities.Disciplina", null)
                        .WithMany("Avaliacoes")
                        .HasForeignKey("DisciplinaId");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Curso", b =>
                {
                    b.HasOne("ApiTcc.Infra.DB.Entities.Faculdade", null)
                        .WithMany("Cursos")
                        .HasForeignKey("FaculdadeId");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Disciplina", b =>
                {
                    b.HasOne("ApiTcc.Infra.DB.Entities.Semestre", null)
                        .WithMany("Disciplinas")
                        .HasForeignKey("SemestreId");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.Faculdade", b =>
                {
                    b.HasOne("ApiTcc.Infra.DB.Entities.Aluno", null)
                        .WithMany("Faculdade")
                        .HasForeignKey("AlunoId");
                });

            modelBuilder.Entity("ApiTcc.Infra.DB.Entities.FaculdadeAluno", b =>
                {
                    b.HasOne("ApiTcc.Infra.DB.Entities.Aluno", "Aluno")
                        .WithMany()
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
                        .HasForeignKey("CursoId");
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
