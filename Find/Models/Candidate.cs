using System.ComponentModel.DataAnnotations;

namespace Find.Models
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }
        public string? Image { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Email { get; set; }
        public string? Adress { get; set; }
        public string? Phone { get; set; }
        public string? MilitaryStatus { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Experience { get; set; }
        public string? EducationalStatus { get; set; }
        public double? GraduationScore { get; set; }
        public string? Language { get; set; }
        public string? LanguageLevel { get; set; }
        public string? Reference { get; set; }
        public string? Explanation { get; set; }
        public double? Rate { get; set; }
        public string? Profession { get; set; }

    }
}
