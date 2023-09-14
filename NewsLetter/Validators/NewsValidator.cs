using FluentValidation;
using NewsLetter.Models;
using NewsLetter.Models.Context;

public class NewsValidator: AbstractValidator<News>
{
    public NewsValidator(){
        RuleFor(news => news.Title).NotEmpty();
        RuleFor(news=>news.Title).Must(BeUnique).WithMessage("Title already exists!!");
    }

    private bool BeUnique(String title){
        return new DataContext().News.FirstOrDefault(x=>x.Title == title) == null;
    }

}