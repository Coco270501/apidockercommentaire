using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

List<Comment> comments = new List<Comment>();
Random random = new Random();

for(int i = 0; i < 5; i++){
    Comment coment = new Comment(id:i, idUser:random.Next(1, 101),text:"zeubilamouche " + i);
    comments.Add(coment);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

//setter
app.MapPost("/create_commentaire", (int id, int id_user, string text) =>
{
    Comment coment = new Comment(id:id, idUser:id_user,text:"Commentaire ajouté");
    comments.Add(coment);
    return coment;
})
.WithName("Create_commentaire")
.WithOpenApi();

app.MapPost("/edit_commentaire", (int id, string text) =>
{
    Comment comment_temp = comments.Where(x => x.Id == id).First();
    Comment comment_to_edit = comments.Where(x => x.Id == id).First();
    comment_to_edit.Text = text;
    comments.Remove(comment_temp);
    comments.Add(comment_to_edit);
    return comment_to_edit;
})
.WithName("edit_commentaire")
.WithOpenApi();

app.MapPost("/delete_commentaire", (int id) =>
{
    Comment comment_to_delete = comments.Where(x => x.Id == id).First();
    if(comment_to_delete != null){
        comments.Remove(comment_to_delete);
            return "Commentaire supprimé";

    }else{
        return "Commentaire inexistant";

    }
})
.WithName("delete_commentaire")
.WithOpenApi();

//getter
app.MapPost("/get_commentaire_from_id", (int id) =>
{
    return comments.Where(x=> x.Id == id).First();
})
.WithName("get_commentaire_from_id")
.WithOpenApi();

app.MapGet("/get_all_commentaire", () =>
{
    return comments;
})
.WithName("get_all_commentaire")
.WithOpenApi();

app.Run();

public class Comment
{
    // Propriétés
    public int IdUser { get; set; }

    public int Id { get; set; }
    public string Text { get; set; }


    // Constructeur
    public Comment(int id ,int idUser, string text)
    {
        Id = id;
        IdUser = idUser;
        Text = text;
    }

    // Méthode pour afficher le commentaire
    public void DisplayComment()
    {
        Console.WriteLine($"ID  : {Id}");
        Console.WriteLine($"ID utilisateur : {IdUser}");
        Console.WriteLine($"Texte du commentaire : {Text}");
    }
}