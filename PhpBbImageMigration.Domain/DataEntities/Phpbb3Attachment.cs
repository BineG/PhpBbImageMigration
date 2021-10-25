using System;
using System.Collections.Generic;

#nullable disable

namespace PhpBbImageMigration.Domain.DataEntities
{
    public partial class Phpbb3Attachment
    {
        public int AttachId { get; set; }
        public int PostMsgId { get; set; }
        public int TopicId { get; set; }
        public byte InMessage { get; set; }
        public int PosterId { get; set; }
        public byte IsOrphan { get; set; }
        public string PhysicalFilename { get; set; }
        public string RealFilename { get; set; }
        public int DownloadCount { get; set; }
        public string AttachComment { get; set; }
        public string Extension { get; set; }
        public string Mimetype { get; set; }
        public int Filesize { get; set; }
        public int Filetime { get; set; }
        public byte Thumbnail { get; set; }
    }
}
