using System.Collections;
using System.Collections.Generic;

namespace Core.Entities
{
    // ovo si stavio u birthdy file, u stvarnosti to stavi u neki nadfajl
    public class ServiceIncluded : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string VideoClip { get; set; }

        public ICollection<BirthdayPackageService> BirthdayPackageServices { get; set; }
    }
}