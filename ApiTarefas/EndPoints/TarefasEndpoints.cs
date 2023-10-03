using Microsoft.EntityFrameworkCore;

namespace ApiTarefas.EndPoints
{
    public static class TarefasEndpoints
    {
        public static void MapTarefas(this WebApplication app)
        {
            app.MapGet("/tarefas", async (AppDbContext db) => await db.Tarefas.ToListAsync());
           



            app.MapGet("/tarefas/{id}", async (int id, AppDbContext db) =>
               await db.Tarefas.FindAsync(id) is Tarefa tarefa ? Results.Ok(tarefa) : Results.NotFound());
           


            app.MapGet("/tarefas/concluida", async (AppDbContext db) =>
                                             await db.Tarefas.Where(t => t.IsConcluida).ToListAsync());
           



            app.MapPost("/tarefas", async (Tarefa tarefa, AppDbContext db) =>
            {  
                db.Tarefas.Add(tarefa);  
                await db.SaveChangesAsync();
                return Results.Created($"/tarefas/{tarefa.Id}", tarefa); 
            });



            app.MapPut("/tarefas/{id}", async (int id, Tarefa inputTarefa, AppDbContext db) =>
            {

                var tarefa = await db.Tarefas.FindAsync(id); 

                if (tarefa is null) return Results.NotFound(); 

                tarefa.Nome = inputTarefa.Nome; 
                tarefa.IsConcluida = inputTarefa.IsConcluida;

                await db.SaveChangesAsync(); 
                return Results.NoContent(); 
            });

            app.MapDelete("/tarefas/{id}", async (int id, AppDbContext db) =>
            {
                if (await db.Tarefas.FindAsync(id) is Tarefa tarefa)
                {
                    db.Tarefas.Remove(tarefa);
                    await db.SaveChangesAsync();
                    return Results.Ok(tarefa); 
                }
                return Results.NotFound(); 
            });
        }
    }
}
