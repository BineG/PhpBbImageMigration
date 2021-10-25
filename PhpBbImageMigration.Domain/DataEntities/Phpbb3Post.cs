using System;
using System.Collections.Generic;

#nullable disable

namespace PhpBbImageMigration.Domain.DataEntities
{
    public partial class Phpbb3Post
    {
        public int PostId { get; set; }
        public int TopicId { get; set; }
        public int ForumId { get; set; }
        public int PosterId { get; set; }
        public int IconId { get; set; }
        public string PosterIp { get; set; }
        public int PostTime { get; set; }
        public byte PostApproved { get; set; }
        public byte PostReported { get; set; }
        public byte EnableBbcode { get; set; }
        public byte EnableSmilies { get; set; }
        public byte EnableMagicUrl { get; set; }
        public byte EnableSig { get; set; }
        public string PostUsername { get; set; }
        public string PostSubject { get; set; }
        public string PostText { get; set; }
        public string PostChecksum { get; set; }
        public byte PostAttachment { get; set; }
        public string BbcodeBitfield { get; set; }
        public string BbcodeUid { get; set; }
        public byte PostPostcount { get; set; }
        public int PostEditTime { get; set; }
        public string PostEditReason { get; set; }
        public int PostEditUser { get; set; }
        public short PostEditCount { get; set; }
        public byte PostEditLocked { get; set; }
        public string PostWpuXpostParent { get; set; }
        public string PostWpuXpostMeta1 { get; set; }
        public string PostWpuXpostMeta2 { get; set; }
    }
}
