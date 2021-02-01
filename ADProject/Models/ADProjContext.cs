using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ADProject.Models
{
    public partial class ADProjContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ADProjContext()
        {
        }

        public ADProjContext(DbContextOptions<ADProjContext> options)
            : base(options)
        {
        }

       
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<FollowUser> FollowUsers { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupTag> GroupTags { get; set; }
        public virtual DbSet<LikesDislike> LikesDislikes { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<RecipeGroup> RecipeGroups { get; set; }
        public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public virtual DbSet<RecipeStep> RecipeSteps { get; set; }
        public virtual DbSet<RecipeTag> RecipeTags { get; set; }
        public virtual DbSet<SavedRecipe> SavedRecipes { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAllergen> UserAllergens { get; set; }
        public virtual DbSet<UsersGroup> UsersGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDb;Initial Catalog=ADProj;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //This is actually a bug fix so please don't remove this -- Wilson
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

         

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_RecipeId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_UserId");
            });

            modelBuilder.Entity<FollowUser>(entity =>
            {
                entity.HasOne(d => d.FollowedUser)
                    .WithMany()
                    .HasForeignKey(d => d.FollowedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FollowUsers_followedUserId");

                entity.HasOne(d => d.FollowingUser)
                    .WithMany()
                    .HasForeignKey(d => d.FollowingUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FollowUsers_followingUserId");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('This group has no description')");

                entity.Property(e => e.GroupName).IsUnicode(false);

                entity.Property(e => e.GroupPhoto).IsUnicode(false);
            });

            modelBuilder.Entity<GroupTag>(entity =>
            {
                entity.HasOne(d => d.Group)
                    .WithMany()
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupTags_GroupId");

                entity.HasOne(d => d.Tag)
                    .WithMany()
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupTags_TagId");
            });

            modelBuilder.Entity<LikesDislike>(entity =>
            {
                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.LikesDislikes)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LikesDislikes_RecipeId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LikesDislikes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LikesDislikes_UserId");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.Calories).HasDefaultValueSql("((10))");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('This recipe has no description')");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((0))");

                entity.Property(e => e.MainMediaUrl).IsUnicode(false);

                entity.Property(e => e.ServingSize).HasDefaultValueSql("((1))");

                entity.Property(e => e.Title).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Recipe_UserId");
            });

            modelBuilder.Entity<RecipeGroup>(entity =>
            {
                entity.HasOne(d => d.Group)
                    .WithMany()
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeGroup_GroupId");

                entity.HasOne(d => d.Recipe)
                    .WithMany()
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeGroup_RecipeId");
            });

            modelBuilder.Entity<RecipeIngredient>(entity =>
            {
                entity.Property(e => e.Ingredient).IsUnicode(false);

                entity.Property(e => e.UnitOfMeasurement).IsUnicode(false);

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeIngredients)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeIngredients_RecipeId");
            });

            modelBuilder.Entity<RecipeStep>(entity =>
            {
                entity.Property(e => e.MediaFileUrl).IsUnicode(false);

                entity.Property(e => e.TextInstructions).IsUnicode(false);

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeSteps)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeSteps_RecipeId");
            });

            modelBuilder.Entity<RecipeTag>(entity =>
            {
                entity.HasOne(d => d.Recipe)
                    .WithMany()
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeTags_RecipeId");

                entity.HasOne(d => d.Tag)
                    .WithMany()
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeTags_TagId");
            });

            modelBuilder.Entity<SavedRecipe>(entity =>
            {
                entity.HasOne(d => d.Recipe)
                    .WithMany()
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SavedRecipes_RecipeId");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SavedRecipes_UserId");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.TagName).IsUnicode(false);

                entity.Property(e => e.Warning)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.IsAdmin).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Username).IsUnicode(false);
            });

            modelBuilder.Entity<UserAllergen>(entity =>
            {
                entity.HasOne(d => d.Tag)
                    .WithMany()
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAllergen_TagId");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAllergen_UserId");
            });

            modelBuilder.Entity<UsersGroup>(entity =>
            {
                entity.HasOne(d => d.Group)
                    .WithMany()
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersGroup_GroupId");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersGroup_UserId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
