using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Common
{
    public class FileEntity : BaseEntity
    {
        public string FileName { get; set; }
        public string Path { get; set; }
    }
}
