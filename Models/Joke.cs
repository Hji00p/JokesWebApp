using System.ComponentModel.DataAnnotations;

namespace JokesWebApp.Models
{
    public class Joke
    {
        public int Id { get; set; }
        [Display(Name = "Question")]
        public string JokeQuestion { get; set; } = string.Empty;
        // The answer to the joke; initialized with an empty string
        [Display(Name = "Answer")]
        public string JokeAnswer { get; set; } = string.Empty;
        // Author's email address; can be null if not specified
        [Display(Name = "Author")]
        public string? AuthorEmail { get; set; }
        public Joke()
        {

        }
    }
}