using FluentValidation;
using NewsLetter.Models;

public class TopicValidator: AbstractValidator<Topic>
{
    public TopicValidator(){
        RuleFor(topic => topic.Name).NotEmpty();
      
    }

}