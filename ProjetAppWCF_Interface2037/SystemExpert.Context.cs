﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Ce code a été généré à partir d'un modèle.
//
//    Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//    Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjetAppWCF_Interface2037
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class bdd_service_web : DbContext
    {
        public bdd_service_web()
            : base("name=bdd_service_web")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<EntiteQuestion> questions { get; set; }
        public DbSet<EntiteReponse> reponses { get; set; }
    
        public virtual int AjouterQuestion(string p_question_contenu)
        {
            var p_question_contenuParameter = p_question_contenu != null ?
                new ObjectParameter("p_question_contenu", p_question_contenu) :
                new ObjectParameter("p_question_contenu", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AjouterQuestion", p_question_contenuParameter);
        }
    }
}