using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PhpBbImageMigration.Domain.DataEntities;

#nullable disable

namespace PhpBbImageMigration.Infrastructure.MySql.Context
{
    public partial class PhpbbContext : DbContext
    {
        public PhpbbContext()
        {
        }

        public PhpbbContext(DbContextOptions<PhpbbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Phpbb3Attachment> Phpbb3Attachments { get; set; }
        public virtual DbSet<Phpbb3Post> Phpbb3Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("Server=localhost;Database=alfaklub_phpbb;Uid=root;Pwd=sasa;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Phpbb3Attachment>(entity =>
            {
                entity.HasKey(e => e.AttachId)
                    .HasName("PRIMARY");

                entity.ToTable("phpbb3_attachments");

                entity.HasIndex(e => e.Filetime, "filetime");

                entity.HasIndex(e => e.IsOrphan, "is_orphan");

                entity.HasIndex(e => e.PostMsgId, "post_msg_id");

                entity.HasIndex(e => e.PosterId, "poster_id");

                entity.HasIndex(e => e.TopicId, "topic_id");

                entity.Property(e => e.AttachId)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("attach_id");

                entity.Property(e => e.AttachComment)
                    .IsRequired()
                    .HasColumnName("attach_comment");

                entity.Property(e => e.DownloadCount)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("download_count")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("extension")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Filesize)
                    .HasColumnType("int unsigned")
                    .HasColumnName("filesize");

                entity.Property(e => e.Filetime)
                    .HasColumnType("int unsigned")
                    .HasColumnName("filetime");

                entity.Property(e => e.InMessage).HasColumnName("in_message");

                entity.Property(e => e.IsOrphan)
                    .HasColumnName("is_orphan")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Mimetype)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("mimetype")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.PhysicalFilename)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("physical_filename")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.PostMsgId)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("post_msg_id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PosterId)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("poster_id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.RealFilename)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("real_filename")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Thumbnail).HasColumnName("thumbnail");

                entity.Property(e => e.TopicId)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("topic_id")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<Phpbb3Post>(entity =>
            {
                //entity.HasNoKey();
                entity.HasKey(x => x.PostId);

                entity.ToTable("phpbb3_posts");

                entity.Property(e => e.BbcodeBitfield)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("bbcode_bitfield")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.BbcodeUid)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("bbcode_uid")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.EnableBbcode)
                    .HasColumnName("enable_bbcode")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.EnableMagicUrl)
                    .HasColumnName("enable_magic_url")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.EnableSig)
                    .HasColumnName("enable_sig")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.EnableSmilies)
                    .HasColumnName("enable_smilies")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.ForumId)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("forum_id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IconId)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("icon_id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PostApproved)
                    .HasColumnName("post_approved")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.PostAttachment).HasColumnName("post_attachment");

                entity.Property(e => e.PostChecksum)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("post_checksum")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.PostEditCount)
                    .HasColumnType("smallint unsigned")
                    .HasColumnName("post_edit_count");

                entity.Property(e => e.PostEditLocked).HasColumnName("post_edit_locked");

                entity.Property(e => e.PostEditReason)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("post_edit_reason")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.PostEditTime)
                    .HasColumnType("int unsigned")
                    .HasColumnName("post_edit_time");

                entity.Property(e => e.PostEditUser)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("post_edit_user")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PostId)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("post_id");

                entity.Property(e => e.PostPostcount)
                    .HasColumnName("post_postcount")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.PostReported).HasColumnName("post_reported");

                entity.Property(e => e.PostSubject)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("post_subject")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.PostText)
                    .IsRequired()
                    .HasColumnType("mediumtext")
                    .HasColumnName("post_text");

                entity.Property(e => e.PostTime)
                    .HasColumnType("int unsigned")
                    .HasColumnName("post_time");

                entity.Property(e => e.PostUsername)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("post_username")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.PostWpuXpostMeta1)
                    .HasMaxLength(255)
                    .HasColumnName("post_wpu_xpost_meta1");

                entity.Property(e => e.PostWpuXpostMeta2)
                    .HasMaxLength(255)
                    .HasColumnName("post_wpu_xpost_meta2");

                entity.Property(e => e.PostWpuXpostParent)
                    .HasMaxLength(10)
                    .HasColumnName("post_wpu_xpost_parent");

                entity.Property(e => e.PosterId)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("poster_id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PosterIp)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("poster_ip")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TopicId)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("topic_id")
                    .HasDefaultValueSql("'0'");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
